import {
    Button, Checkbox, NativeSelect,
} from "@equinor/eds-core-react"
import {
    ChangeEvent,
    Dispatch, SetStateAction, useCallback, useEffect, useMemo, useRef, useState,
} from "react"
import { AgGridReact } from "ag-grid-react"
import { RowNode } from "ag-grid-enterprise"
import { Project } from "../../models/Project"
import SharePointImport from "./SharePointImport"
import { DriveItem } from "../../models/sharepoint/DriveItem"
import { ImportStatusEnum } from "./ImportStatusEnum"
import { GetProspService } from "../../Services/ProspService"
import { EMPTY_GUID } from "../../Utils/constants"
import { GetCaseService } from "../../Services/CaseService"
import { Case } from "../../models/case/Case"

interface Props {
    setProject: Dispatch<SetStateAction<Project | undefined>>
    project: Project
    driveItems: DriveItem[] | undefined
}

interface RowData {
    id: string,
    name: string,
    surfState: ImportStatusEnum
    substructureState: ImportStatusEnum
    topsideState: ImportStatusEnum
    transportState: ImportStatusEnum
    sharePointFileName?: string
    sharePointFileId?: string
    sharePointSiteUrl?: string
    driveItem: [DriveItem[] | undefined, string | undefined]
}

function PROSPCaseList({
    setProject,
    project,
    driveItems,
}: Props) {
    const gridRef = useRef<any>(null)
    const [rowData, setRowData] = useState<RowData[]>()

    const casesToRowData = () => {
        if (project.cases) {
            const tableCases: RowData[] = []
            project.cases.forEach((c) => {
                const tableCase: RowData = {
                    id: c.id!,
                    name: c.name ?? "",
                    surfState: SharePointImport.surfStatus(c, project),
                    substructureState: SharePointImport.substructureStatus(c, project),
                    topsideState: SharePointImport.topsideStatus(c, project),
                    transportState: SharePointImport.transportStatus(c, project),
                    sharePointFileId: c.sharepointFileId,
                    driveItem: [driveItems, c.sharepointFileId],
                }
                tableCases.push(tableCase)
            })
            setRowData(tableCases)
        }
    }

    useEffect(() => {
        casesToRowData()
    }, [project, driveItems])

    const defaultColDef = useMemo(() => ({
        sortable: true,
        filter: true,
        resizable: true,
    }), [])

    const changeStatus = (p: any, value: ImportStatusEnum) => {
        p.setValue(value)
    }

    const checkBoxStatus = (
        p: any,
    ) => {
        if (p.value === ImportStatusEnum.PROSP) {
            return <Checkbox disabled checked />
        }
        if (p.value === ImportStatusEnum.Selected) {
            return <Checkbox checked onChange={() => changeStatus(p, ImportStatusEnum.NotSelected)} />
        }
        if (p.value === ImportStatusEnum.NotSelected) {
            return <Checkbox checked={false} onChange={() => changeStatus(p, ImportStatusEnum.Selected)} />
        }
        return <Checkbox onChange={() => changeStatus(p, ImportStatusEnum.Selected)} />
    }

    const sharePointFileDropdownOptions = (items: DriveItem[]) => {
        const options: JSX.Element[] = []
        items?.forEach((item) => {
            options.push((<option key={item.id} value={item.id!}>{item.name}</option>))
        })
        return options
    }

    const handleFileChange = (event: ChangeEvent<HTMLSelectElement>, p: any) => {
        const value = { ...p.value }
        value[1] = event.currentTarget.selectedOptions[0].value
        p.setValue(value)
    }

    const fileIdDropDown = (p: any) => {
        const fileId = p.value[1]
        const items: DriveItem[] = p.value[0]

        return (
            <NativeSelect
                id="sharePointFile"
                label=""
                value={fileId}
                onChange={(e: ChangeEvent<HTMLSelectElement>) => handleFileChange(e, p)}
            >
                {sharePointFileDropdownOptions(items)}
                <option aria-label="empty value" key="" value="" />
            </NativeSelect>
        )
    }

    const unlinkAssetsOnCase = (newCase: Case) => {
        const unlinkingCase = Case.Copy(newCase)
        unlinkingCase.transportLink = EMPTY_GUID
        unlinkingCase.surfLink = EMPTY_GUID
        unlinkingCase.substructureLink = EMPTY_GUID
        unlinkingCase.topsideLink = EMPTY_GUID
        return unlinkingCase
    }

    const clearSelectedAssets = async (p: any) => {
        if (p.newValue[1] !== p.oldValue[1]) {
            p.node.setDataValue("surfState", 2)
            p.node.setDataValue("substructureState", 2)
            p.node.setDataValue("topsideState", 2)
            p.node.setDataValue("transportState", 2)

            try {
                const caseItem = project.cases.find((c) => c.id === p.node.data.id)
                if (caseItem) {
                    const unlinkedCase = unlinkAssetsOnCase(caseItem)
                    // eslint-disable-next-line prefer-destructuring
                    unlinkedCase.sharepointFileId = p.newValue[1]
                    await (await GetCaseService()).updateCase(unlinkedCase)
                }
            } catch (e) {
                console.error(e)
            }
        }
    }

    type SortOrder = "desc" | "asc" | null
    const order: SortOrder = "asc"

    const [columnDefs] = useState([
        {
            field: "name", sort: order,
        },
        {
            field: "surfState", headerName: "Surf", width: 90, cellRenderer: checkBoxStatus,
        },
        {
            field: "substructureState", headerName: "Substructure", width: 110, cellRenderer: checkBoxStatus,
        },
        {
            field: "topsideState", headerName: "Topside", width: 90, cellRenderer: checkBoxStatus,
        },
        {
            field: "transportState", headerName: "Transport", width: 90, cellRenderer: checkBoxStatus,
        },
        {
            field: "driveItem",
            headerName: "SharePoint file",
            cellRenderer: fileIdDropDown,
            onCellValueChanged: clearSelectedAssets,
            sortable: false,
            width: 350,
        },
    ])

    const onGridReady = (params: any) => {
        gridRef.current = params.api
    }

    const gridDataToDtos = () => {
        const dtos: any[] = []
        gridRef.current.forEachNode((node: RowNode<RowData>) => {
            const dto: any = {}
            dto.sharePointFileId = node.data?.driveItem[1]

            dto.sharePointFileName = node.data?.driveItem[0]?.find(
                (di) => di.sharepointIds === dto.sharePointFileId,
            )?.name ?? ""

            dto.sharePointSiteUrl = project.sharepointSiteUrl
            dto.id = node.data?.id
            dto.surf = node.data?.surfState === ImportStatusEnum.Selected
            dto.substructure = node.data?.substructureState === ImportStatusEnum.Selected
            dto.topside = node.data?.topsideState === ImportStatusEnum.Selected
            dto.transport = node.data?.transportState === ImportStatusEnum.Selected
            dtos.push(dto)
        })
        return dtos
    }

    const save = useCallback(async () => {
        const dtos = gridDataToDtos()
        const newProject = await (await GetProspService()).importFromSharepoint(project.id!, dtos)
        setProject(newProject)
    }, [])

    return (
        <>
            <div
                style={{
                    display: "flex", flexDirection: "column", width: "100%",
                }}
                className="ag-theme-alpine"
            >
                <AgGridReact
                    ref={gridRef}
                    rowData={rowData}
                    columnDefs={columnDefs}
                    defaultColDef={defaultColDef}
                    animateRows
                    domLayout="autoHeight"
                    onGridReady={onGridReady}
                />
            </div>
            <Button
                onClick={save}
            >
                Save
            </Button>
        </>
    )
}

export default PROSPCaseList

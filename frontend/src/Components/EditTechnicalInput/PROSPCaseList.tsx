import { Button, Checkbox, NativeSelect } from "@equinor/eds-core-react"
import {
    ChangeEvent, Dispatch, SetStateAction, useCallback, useEffect, useMemo, useRef, useState,
} from "react"
import { AgGridReact } from "ag-grid-react"
import { RowNode } from "ag-grid-enterprise"
import { Project } from "../../models/Project"
import SharePointImport from "./SharePointImport"
import { DriveItem } from "../../models/sharepoint/DriveItem"
import { ImportStatusEnum } from "./ImportStatusEnum"
import { GetProspService } from "../../Services/ProspService"

interface Props {
    setProject: Dispatch<SetStateAction<Project | undefined>>
    project: Project
    driveItems: DriveItem[] | undefined
    check: boolean
}

interface RowData {
    id: string,
    name: string,
    surfState: ImportStatusEnum
    substructureState: ImportStatusEnum
    topsideState: ImportStatusEnum
    transportState: ImportStatusEnum
    sharePointFileName?: string | null
    sharePointFileId?: string | null
    sharepointFileUrl?: string | null
    driveItem: [DriveItem[] | undefined, string | undefined | null]
    fileLink?: string | null
    caseSelected: boolean,
}

function PROSPCaseList({
    setProject,
    project,
    driveItems,
    check,
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
                    sharePointFileName: c.sharepointFileName,
                    sharepointFileUrl: c.sharepointFileUrl,
                    fileLink: c.sharepointFileUrl,
                    driveItem: [driveItems, c.sharepointFileId],
                    caseSelected: false,
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

    const caseSelectedRenderer = (p:any) => {
        console.log(p.value)
        return <Checkbox checked />
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
    const fileLinkRenderer = (p:any) => {
        const link = p.data?.fileLink
        if (link && link !== "") {
            return (<a href={link}>Test</a>)
        }
        return null
    }

    type SortOrder = "desc" | "asc" | null
    const order: SortOrder = "asc"

    const [columnDefs, setColumnDefs] = useState([
        { field: "checkbox", cellRenderer: caseSelectedRenderer, width: 100 },
        {
            field: "name", sort: order, flex: 3,
        },
        {
            field: "surfState", headerName: "Surf", flex: 1, cellRenderer: checkBoxStatus, hide: check,
        },
        {
            field: "substructureState", headerName: "Substructure", flex: 1, cellRenderer: checkBoxStatus, hide: check,
        },
        {
            field: "topsideState", headerName: "Topside", flex: 1, cellRenderer: checkBoxStatus, hide: check,
        },
        {
            field: "transportState", headerName: "Transport", flex: 1, cellRenderer: checkBoxStatus, hide: check,
        },
        {
            field: "driveItem",
            headerName: "SharePoint file",
            cellRenderer: fileIdDropDown,
            sortable: false,
            flex: 5,
        },
        {
            field: "fileLink",
            headerName: "Link",
            cellRenderer: fileLinkRenderer,
            flex: 6,
        },
    ])

    useEffect(() => {
        const assetFields = ["surfState", "substructureState", "topsideState", "transportState"]
        const newColumnDefs = [...columnDefs]
        const columnData: any = []
        newColumnDefs.forEach((cd) => {
            if (assetFields.indexOf(cd.field) > -1) {
                const colDef = { ...cd }
                colDef.hide = !check
                columnData.push(colDef)
            } else {
                columnData.push(cd)
            }
        })
        if (columnData.length > 0) {
            setColumnDefs(columnData)
        }
    }, [check])

    const onGridReady = (params: any) => {
        gridRef.current = params.api
    }

    const gridDataToDtos = (p: Project) => {
        const dtos: any[] = []
        gridRef.current.forEachNode((node: RowNode<RowData>) => {
            const dto: any = {}
            dto.sharePointFileId = node.data?.driveItem[1]

            dto.sharePointFileName = node.data?.driveItem[0]?.find(
                (di) => di.id === dto.sharePointFileId,
            )?.name

            dto.sharepointFileUrl = node.data?.driveItem[0]?.find(
                (di) => di.id === dto.sharePointFileId,
            )?.sharepointFileUrl

            dto.sharePointSiteUrl = p.sharepointSiteUrl
            dto.id = node.data?.id
            dto.surf = node.data?.surfState === ImportStatusEnum.Selected
            dto.substructure = node.data?.substructureState === ImportStatusEnum.Selected
            dto.topside = node.data?.topsideState === ImportStatusEnum.Selected
            dto.transport = node.data?.transportState === ImportStatusEnum.Selected
            if (node.data?.caseSelected) {
                dtos.push(dto)
            }
        })
        return dtos
    }

    const save = useCallback(async (p: Project) => {
        const dtos = gridDataToDtos(p)
        if (dtos.length > 0) {
            const newProject = await (await GetProspService()).importFromSharepoint(p.id!, dtos)
            setProject(newProject)
        }
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
                onClick={() => save(project)}
            >
                Save
            </Button>
        </>
    )
}

export default PROSPCaseList

/* eslint-disable camelcase */
import {
    Button, Checkbox, Icon, NativeSelect, Progress,
} from "@equinor/eds-core-react"
import {
    ChangeEvent, Dispatch, SetStateAction, useCallback, useEffect, useMemo, useRef, useState,
} from "react"
import { AgGridReact } from "ag-grid-react"
import { GetRowIdFunc, GetRowIdParams, RowNode } from "ag-grid-enterprise"
import styled from "styled-components"
import { external_link } from "@equinor/eds-icons"
import { Project } from "../../models/Project"
import SharePointImport from "./SharePointImport"
import { DriveItem } from "../../models/sharepoint/DriveItem"
import { ImportStatusEnum } from "./ImportStatusEnum"
import { GetProspService } from "../../Services/ProspService"

const ApplyButtonWrapper = styled.div`
    display: flex;
    padding-top: 1em;
`
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

    const [isApplying, setIsApplying] = useState<boolean>()

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

    const caseAutoSelect = (nodeId: string) => {
        const rowNode = gridRef.current?.getRowNode(nodeId)
        rowNode.setDataValue("caseSelected", true)
    }

    const changeStatus = (p: any, value: ImportStatusEnum) => {
<<<<<<< HEAD
        const nodeId: string = p.node?.data.id
        const rowNode = gridRef.current?.getRowNode(nodeId)
        rowNode.setDataValue("caseSelected", true)

=======
        caseAutoSelect(p.node?.data.id)
>>>>>>> c5b142dd6e3acd02e50570362bc93e3c1223ed64
        p.setValue(value)
    }

    const handleCheckboxChange = (p: any, value: boolean) => {
        p.setValue(value)
    }

    const caseSelectedRenderer = (p:any) => {
        if (p.value) {
            return <Checkbox checked onChange={() => handleCheckboxChange(p, false)} />
        }
        return <Checkbox onChange={() => handleCheckboxChange(p, true)} />
    }

    const checkBoxStatus = (
        p: any,
    ) => {
        if (p.value === ImportStatusEnum.PROSP) {
            // Imported assets should have checked checkboxes and remaining assets should remain unchecked.
            return <Checkbox checked onChange={() => changeStatus(p, ImportStatusEnum.NotSelected)} />
        }
        if (p.value === ImportStatusEnum.Selected && p.node.data.sharePointFileName !== "") {
            return <Checkbox onChange={() => changeStatus(p, ImportStatusEnum.Selected)} />
        }
        if (p.value === ImportStatusEnum.Selected && p.node.data.sharePointFileName === "") {
            return <Checkbox checked onChange={() => changeStatus(p, ImportStatusEnum.NotSelected)} />
        }
        if (p.value === ImportStatusEnum.NotSelected) {
            return <Checkbox onChange={() => changeStatus(p, ImportStatusEnum.Selected)} />
        }
        return <Checkbox checked onChange={() => changeStatus(p, ImportStatusEnum.Selected)} />
    }

    const sharePointFileDropdownOptions = (items: DriveItem[]) => {
        const options: JSX.Element[] = []
        items?.forEach((item) => {
            options.push((<option key={item.id} value={item.id!}>{item.name}</option>))
        })
        return options
    }

    const getRowId = useMemo<GetRowIdFunc>(() => (params: GetRowIdParams) => params.data.id, [])

<<<<<<< HEAD
    const handleFileChange = useCallback((event: ChangeEvent<HTMLSelectElement>, p: any) => {
        const value = { ...p.value }
        value[1] = event.currentTarget.selectedOptions[0].value

        const nodeId: string = p.node?.data.id
        const rowNode = gridRef.current?.getRowNode(nodeId)
        rowNode.setDataValue("caseSelected", true)

=======
    const handleFileChange = (event: ChangeEvent<HTMLSelectElement>, p: any) => {
        const value = { ...p.value }
        value[1] = event.currentTarget.selectedOptions[0].value
        caseAutoSelect(p.node?.data.id)
>>>>>>> c5b142dd6e3acd02e50570362bc93e3c1223ed64
        p.setValue(value)
    }, [])

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
            return (
                <a
                    href={link}
                    aria-label="SharePoint File link"
                    target="_blank"
                    rel="noopener noreferrer"
                >
                    <Icon data={external_link} />
                </a>
            )
        }
        return null
    }

    type SortOrder = "desc" | "asc" | null
    const order: SortOrder = "asc"

    const [columnDefs, setColumnDefs] = useState([
        {
            field: "caseSelected", headerName: "", cellRenderer: caseSelectedRenderer, flex: 1,
        },
        {
            field: "name", sort: order, flex: 3,
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
            width: 60,
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
            setIsApplying(true)
            const newProject = await (await GetProspService()).importFromSharepoint(p.id!, dtos)
            setProject(newProject)
            setIsApplying(false)
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
                    getRowId={getRowId}
                />
            </div>
            <ApplyButtonWrapper>
                {!isApplying ? (
                    <Button
                        onClick={() => save(project)}
                        color="secondary"
                    >
                        Apply changes
                    </Button>
                ) : (
                    <Button variant="outlined">
                        <Progress.Dots color="primary" />
                    </Button>
                )}
            </ApplyButtonWrapper>

        </>
    )
}

export default PROSPCaseList

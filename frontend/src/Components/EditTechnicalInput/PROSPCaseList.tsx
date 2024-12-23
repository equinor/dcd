import {
    Button, Checkbox, Icon, NativeSelect, Progress,
} from "@equinor/eds-core-react"
import {
    ChangeEvent, useCallback, useEffect, useMemo, useRef, useState,
} from "react"
import { AgGridReact } from "@ag-grid-community/react"
import useStyles from "@equinor/fusion-react-ag-grid-styles"
import {
    GetRowIdFunc,
    GetRowIdParams,
    RowNode,
} from "@ag-grid-community/core"
import { external_link } from "@equinor/eds-icons"
import Grid from "@mui/material/Grid"

import { DriveItem } from "@/Models/sharepoint/DriveItem"
import { GetProspService } from "@/Services/ProspService"
import useEditProject from "@/Hooks/useEditProject"
import useEditDisabled from "@/Hooks/useEditDisabled"
import { useAppContext } from "@/Context/AppContext"
import { ImportStatusEnum } from "./ImportStatusEnum"
import SharePointImport from "./SharePointImport"
import { useDataFetch } from "@/Hooks/useDataFetch"

interface Props {
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
    surfStateChanged: boolean,
    substructureStateChanged: boolean,
    topsideStateChanged: boolean,
    transportStateChanged: boolean
    sharePointFileChanged: boolean,
}
const PROSPCaseList = ({
    driveItems,
    check,
}: Props) => {
    const gridRef = useRef<any>(null)
    const styles = useStyles()
    const revisionAndProjectData = useDataFetch()
    const { addProjectEdit } = useEditProject()
    const { isEditDisabled } = useEditDisabled()
    const { editMode } = useAppContext()

    const [rowData, setRowData] = useState<RowData[]>()
    const [isApplying, setIsApplying] = useState<boolean>()

    const casesToRowData = () => {
        if (revisionAndProjectData && revisionAndProjectData.commonProjectAndRevisionData.cases) {
            const tableCases: RowData[] = []
            revisionAndProjectData.commonProjectAndRevisionData.cases.forEach((c) => {
                const tableCase: RowData = {
                    id: c.caseId!,
                    name: c.name ?? "",
                    surfState: SharePointImport.surfStatus(c, revisionAndProjectData.commonProjectAndRevisionData),
                    substructureState: SharePointImport.substructureStatus(c, revisionAndProjectData.commonProjectAndRevisionData),
                    topsideState: SharePointImport.topsideStatus(c, revisionAndProjectData.commonProjectAndRevisionData),
                    transportState: SharePointImport.transportStatus(c, revisionAndProjectData.commonProjectAndRevisionData),
                    sharePointFileId: c.sharepointFileId,
                    sharePointFileName: c.sharepointFileName,
                    sharepointFileUrl: c.sharepointFileUrl,
                    fileLink: c.sharepointFileUrl,
                    driveItem: [driveItems, c.sharepointFileId],
                    surfStateChanged: false,
                    substructureStateChanged: false,
                    topsideStateChanged: false,
                    transportStateChanged: false,
                    sharePointFileChanged: false,
                }
                tableCases.push(tableCase)
            })
            setRowData(tableCases)
        }
    }

    const defaultColDef = useMemo(() => ({
        sortable: true,
        filter: true,
        resizable: true,
        suppressHeaderMenuButton: true,
    }), [])

    const rowIsChanged = (p: any) => (p.data.surfStateChanged
        || p.data.substructureStateChanged
        || p.data.topsideStateChanged
        || p.data.transportStateChanged
        || p.data.sharePointFileChanged)

    const caseAutoSelect = (nodeId: string) => {
        const rowNode = gridRef.current?.getRowNode(nodeId)

        if (rowIsChanged(rowNode)) {
            rowNode.selected = true
            rowNode.setSelected(true)
            rowNode.selectable = true
        } else {
            rowNode.selected = false
            rowNode.setSelected(false)
            rowNode.selectable = false
        }

        gridRef.current.redrawRows()
    }

    const handleAdvancedSettingsChange = (p: any, value: ImportStatusEnum) => {
        if (revisionAndProjectData && revisionAndProjectData.commonProjectAndRevisionData.cases) {
            const projectCase = revisionAndProjectData.commonProjectAndRevisionData.cases.find((el: any) => p.data.id && p.data.id === el.id)
            const rowNode = gridRef.current?.getRowNode(p.node?.data.id)
            if (projectCase) {
                switch (p.column.colId) {
                case "surfState":
                    rowNode.data.surfStateChanged = (SharePointImport.surfStatus(projectCase, revisionAndProjectData.commonProjectAndRevisionData) !== value)
                    break
                case "substructureState":
                    rowNode.data.substructureStateChanged = (
                        SharePointImport.substructureStatus(projectCase, revisionAndProjectData.commonProjectAndRevisionData) !== value)
                    break
                case "topsideState":
                    rowNode.data.topsideStateChanged = (SharePointImport.topsideStatus(projectCase, revisionAndProjectData.commonProjectAndRevisionData) !== value)
                    break
                case "transportState":
                    rowNode.data.transportStateChanged = (SharePointImport.transportStatus(projectCase, revisionAndProjectData.commonProjectAndRevisionData) !== value)
                    break
                default:
                    break
                }
            }
        }
        p.setValue(value)
        caseAutoSelect(p.node?.data.id)
    }

    const advancedSettingsRenderer = (
        p: any,
    ) => {
        if (p.value === ImportStatusEnum.Selected) {
            return (
                <Checkbox
                    checked
                    disabled={isEditDisabled || !editMode}
                    onChange={() => handleAdvancedSettingsChange(p, ImportStatusEnum.NotSelected)}
                />
            )
        }
        if (p.value === ImportStatusEnum.NotSelected) {
            return (
                <Checkbox
                    checked={false}
                    disabled={isEditDisabled || !editMode}
                    onChange={() => handleAdvancedSettingsChange(p, ImportStatusEnum.Selected)}
                />
            )
        }
        return (
            <Checkbox
                checked
                disabled={isEditDisabled || !editMode}
                onChange={() => handleAdvancedSettingsChange(p, ImportStatusEnum.Selected)}
            />
        )
    }

    const sharePointFileDropdownOptions = (items: DriveItem[]) => {
        const options: JSX.Element[] = []
        items?.slice(1).forEach((item) => {
            options.push(<option key={item.id} value={item.id!}>{item.name}</option>)
        })
        return options
    }

    const getRowId = useMemo<GetRowIdFunc>(() => (params: GetRowIdParams) => params.data.id, [])

    const getFileLink = (p: any, selectedFileId: any) => {
        const driveItemChosen = p.data?.driveItem[0]
        let link = null
        if (selectedFileId && driveItemChosen !== null && driveItemChosen !== undefined) {
            const item = driveItemChosen.find((el: any) => selectedFileId && selectedFileId === el.id)
            if (item) {
                link = item.sharepointFileUrl
            }
        }
        return link
    }

    const updateFileLink = (nodeId: string, selectedFileId: any) => {
        const rowNode = gridRef.current?.getRowNode(nodeId)
        if (selectedFileId !== "") {
            const link = getFileLink(rowNode, selectedFileId)
            rowNode.setDataValue(
                "fileLink",
                (
                    <a href={link} aria-label="SharePoint File link" target="_blank" rel="noopener noreferrer">
                        <Icon data={external_link} />
                    </a>),
            )
        } else {
            rowNode.setDataValue("fileLink", null)
        }
    }

    const handleFileChange = (event: ChangeEvent<HTMLSelectElement>, p: any) => {
        const value = { ...p.value }
        value[1] = event.currentTarget.selectedOptions[0].value || ""
        updateFileLink(p.node?.data.id, value[1])
        const rowNode = gridRef.current?.getRowNode(p.node?.data.id)
        if (value[1] === rowNode.data.sharePointFileId) {
            rowNode.data.sharePointFileChanged = false
        } else {
            rowNode.data.sharePointFileChanged = true
        }
        p.setValue(value)
        caseAutoSelect(p.node?.data.id)
    }

    const fileSelectorRenderer = (p: any) => {
        const fileId = p.value[1] || ""
        const items: DriveItem[] = p.value[0] || []

        return (
            <NativeSelect
                id="sharePointFile"
                label=""
                value={fileId}
                onChange={(e: ChangeEvent<HTMLSelectElement>) => handleFileChange(e, p)}
                disabled={isEditDisabled || !editMode}
            >
                {sharePointFileDropdownOptions(items)}
                <option aria-label="empty value" key="" value="" />
            </NativeSelect>
        )
    }

    const fileLinkRenderer = (p: any) => {
        const link = getFileLink(p, p.data.driveItem[1])
        if (link) {
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

    const GetColumnDefs = () => [
        {
            field: "name",
            flex: 2,
        },
        {
            field: "driveItem",
            headerName: "SharePoint file",
            cellRenderer: fileSelectorRenderer,
            valueFormatter: (p: any) => (p.value?.[1] ?? ""),
            sortable: false,
            flex: 3,
        },
        {
            field: "fileLink",
            headerName: "Link",
            cellRenderer: fileLinkRenderer,
            flex: 1,
        },
        {
            field: "surfState",
            headerName: "Surf",
            flex: 1,
            cellRenderer: advancedSettingsRenderer,
            hide: check,
        },
        {
            field: "substructureState",
            headerName: "Substructure",
            flex: 1,
            cellRenderer: advancedSettingsRenderer,
            hide: check,
        },
        {
            field: "topsideState",
            headerName: "Topside",
            flex: 1,
            cellRenderer: advancedSettingsRenderer,
            hide: check,
        },
        {
            field: "transportState",
            headerName: "Transport",
            flex: 1,
            cellRenderer: advancedSettingsRenderer,
            hide: check,
        },
    ]

    const [columnDefs, setColumnDefs] = useState(GetColumnDefs())

    useEffect(() => {
        setColumnDefs(GetColumnDefs())
    }, [editMode])

    const onGridReady = (params: any) => {
        gridRef.current = params.api
    }

    const gridDataToDtos = (p: Components.Schemas.CommonProjectAndRevisionDto) => {
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
            if (node.isSelected()) {
                dtos.push(dto)
            }
        })
        return dtos
    }

    const save = useCallback(async (p: Components.Schemas.ProjectDataDto | Components.Schemas.RevisionDataDto) => {
        const dtos = gridDataToDtos(p.commonProjectAndRevisionData)
        if (dtos.length > 0) {
            setIsApplying(true)
            const newProject = await (await GetProspService()).importFromSharepoint(p.projectId, dtos)
            addProjectEdit(newProject.projectId, newProject.commonProjectAndRevisionData)
            setIsApplying(false)
        }
    }, [])

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

    useEffect(() => {
        casesToRowData()
        if (gridRef.current.redrawRows) {
            gridRef.current.redrawRows()
        }
    }, [revisionAndProjectData, driveItems])

    return (
        <Grid container spacing={1}>
            <Grid item xs={12} className={styles.root}>
                <div
                    style={{
                        display: "flex", flexDirection: "column", width: "100%",
                    }}
                >
                    <AgGridReact
                        ref={gridRef}
                        rowData={rowData}
                        columnDefs={columnDefs}
                        defaultColDef={defaultColDef}
                        rowSelection={{
                            mode: "multiRow",
                            enableClickSelection: false,
                            headerCheckbox: true,
                            checkboxes: true,
                            isRowSelectable: rowIsChanged,
                        }}
                        animateRows
                        domLayout="autoHeight"
                        onGridReady={onGridReady}
                        getRowId={getRowId}
                    />
                </div>
            </Grid>
            <Grid item>
                {!isApplying && revisionAndProjectData ? (
                    <Button
                        onClick={() => save(revisionAndProjectData)}
                        color="secondary"
                        disabled={isEditDisabled || !editMode}
                    >
                        Apply changes
                    </Button>
                ) : (
                    <Button variant="outlined">
                        <Progress.Dots color="primary" />
                    </Button>
                )}
            </Grid>
        </Grid>
    )
}

export default PROSPCaseList

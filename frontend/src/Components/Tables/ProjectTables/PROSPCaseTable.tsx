import {
    Button, Icon, NativeSelect, Progress,
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
import Grid from "@mui/material/Grid2"

import { GetProspService } from "@/Services/ProspService"
import useEditProject from "@/Hooks/useEditProject"
import useCanUserEdit from "@/Hooks/useCanUserEdit"
import { useAppStore } from "@/Store/AppStore"
import { useDataFetch } from "@/Hooks"

interface Props {
    sharePointFiles: Components.Schemas.SharePointFileDto[]
}
interface RowData {
    id: string,
    name: string,
    sharePointFileName?: string | null
    sharePointFileId?: string | null
    sharepointFileUrl?: string | null
    driveItem: [Components.Schemas.SharePointFileDto[] | undefined, string | undefined | null]
    fileLink?: string | null
    surfStateChanged: boolean,
    substructureStateChanged: boolean,
    topsideStateChanged: boolean,
    transportStateChanged: boolean
    sharePointFileChanged: boolean,
}
const PROSPCaseList = ({
    sharePointFiles,
}: Props) => {
    const gridRef = useRef<any>(null)
    const styles = useStyles()
    const revisionAndProjectData = useDataFetch()
    const { addProjectEdit } = useEditProject()
    const { canEdit, isEditDisabled } = useCanUserEdit()
    const { editMode } = useAppStore()

    const [rowData, setRowData] = useState<RowData[]>()
    const [isApplying, setIsApplying] = useState<boolean>()

    const casesToRowData = () => {
        if (revisionAndProjectData && revisionAndProjectData.commonProjectAndRevisionData.cases) {
            const tableCases: RowData[] = []
            revisionAndProjectData.commonProjectAndRevisionData.cases.forEach((c) => {
                const tableCase: RowData = {
                    id: c.caseId!,
                    name: c.name ?? "",
                    sharePointFileId: c.sharepointFileId,
                    sharePointFileName: c.sharepointFileName,
                    sharepointFileUrl: c.sharepointFileUrl,
                    fileLink: c.sharepointFileUrl,
                    driveItem: [sharePointFiles, c.sharepointFileId],
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

    const sharePointFileDropdownOptions = (items: Components.Schemas.SharePointFileDto[]) => {
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
        const items: Components.Schemas.SharePointFileDto[] = p.value[0] || []

        return (
            <NativeSelect
                id="sharePointFile"
                label=""
                value={fileId}
                onChange={(e: ChangeEvent<HTMLSelectElement>) => handleFileChange(e, p)}
                disabled={!canEdit()}
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
    ]

    const [columnDefs, setColumnDefs] = useState(GetColumnDefs())

    useEffect(() => {
        setColumnDefs(GetColumnDefs())
    }, [editMode, isEditDisabled])

    const onGridReady = (params: any) => {
        gridRef.current = params.api
    }

    const gridDataToDtos = (p: Components.Schemas.CommonProjectAndRevisionDto) => {
        const dtos: Components.Schemas.SharePointImportDto[] = []
        gridRef.current.forEachNode((node: RowNode<RowData>) => {
            if (!node.isSelected()) {
                return
            }

            if (!node.data?.id) {
                return
            }

            if (!p.sharepointSiteUrl) {
                return
            }

            const sharePointFileId = node.data?.driveItem[1]
            const sharePointFileName = node.data?.driveItem[0]?.find((di) => di.id === sharePointFileId)?.name

            dtos.push({
                caseId: node.data?.id,
                sharePointFileId: sharePointFileId || "",
                sharePointFileName: sharePointFileName || "",
                sharePointSiteUrl: p.sharepointSiteUrl,
            })
        })

        return dtos
    }

    const save = useCallback(async (p: Components.Schemas.ProjectDataDto | Components.Schemas.RevisionDataDto) => {
        const dtos = gridDataToDtos(p.commonProjectAndRevisionData)
        if (dtos.length > 0) {
            setIsApplying(true)
            const newProject = await GetProspService().importFromSharePoint(p.projectId, dtos)
            addProjectEdit(newProject.projectId, newProject.commonProjectAndRevisionData)
            setIsApplying(false)
        }
    }, [])

    useEffect(() => {
        casesToRowData()
        if (gridRef.current.redrawRows) {
            gridRef.current.redrawRows()
        }
    }, [revisionAndProjectData, sharePointFiles])

    return (
        <Grid container spacing={1}>
            <Grid size={12} className={styles.root}>
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
                            isRowSelectable: () => canEdit(),
                        }}
                        animateRows
                        domLayout="autoHeight"
                        onGridReady={onGridReady}
                        getRowId={getRowId}
                    />
                </div>
            </Grid>
            <Grid>
                {!isApplying && revisionAndProjectData ? (
                    <Button
                        onClick={() => save(revisionAndProjectData)}
                        color="secondary"
                        disabled={!canEdit()}
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

import {
    Button,
    Icon,
    Tooltip,
    Typography,
} from "@equinor/eds-core-react"
import {
    useState,
    useEffect,
    useMemo,
    useRef,
    MouseEventHandler,
} from "react"
import { useNavigate, useParams } from "react-router-dom"
import { useModuleCurrentContext } from "@equinor/fusion-framework-react-module-context"
import { AgGridReact } from "@ag-grid-community/react"
import {
    arrow_drop_down, arrow_drop_up, more_vertical, archive,
} from "@equinor/eds-icons"
import styled from "styled-components"
import { ColDef } from "@ag-grid-community/core"
import { useQuery } from "@tanstack/react-query"

import {
    productionStrategyOverviewToString, cellStyleRightAlign, unwrapProjectId,
    caseRevisionPath,
} from "@/Utils/common"
import { GetProjectService } from "@/Services/ProjectService"
import { GetSTEAService } from "@/Services/STEAService"
import { projectQueryFn, revisionQueryFn } from "@/Services/QueryFunctions"
import { ReferenceCaseIcon } from "../Components/ReferenceCaseIcon"
import { useProjectContext } from "@/Context/ProjectContext"
import { useAppContext } from "@/Context/AppContext"

const AgTableContainer = styled.div`
    overflow: auto;
`

const Wrapper = styled.div`
    justify-content: center;
    align-items: center;
    display: inline-flex;
`
const DownloadButton = styled.div`
    margin-top: 12px;
    margin-bottom: 24px;
`

const ArchivedTitle = styled.div`
    margin-bottom: 12px;
    display: flex;
`

interface TableCase {
    id: string,
    name: string,
    description: string,
    productionStrategyOverview: Components.Schemas.ProductionStrategyOverview | undefined,
    producerCount: number,
    gasInjectorCount: number,
    waterInjectorCount: number,
    createdAt?: string
    referenceCaseId?: string
}

interface CasesAgGridTableProps {
    setSelectedCaseId: (id: string) => void
    setMenuAnchorEl: (el: HTMLElement | null) => void
    setIsMenuOpen: (isMenuOpen: boolean) => void
    isMenuOpen: boolean
}

const CasesAgGridTable = ({
    setSelectedCaseId,
    setMenuAnchorEl,
    setIsMenuOpen,
    isMenuOpen,
}: CasesAgGridTableProps): JSX.Element => {
    const { isRevision, projectId } = useProjectContext()
    const { revisionId } = useParams()
    const gridRef = useRef<AgGridReact>(null)
    const [rowData, setRowData] = useState<TableCase[]>()
    const [archivedRowData, setArchivedRowData] = useState<TableCase[]>()
    const [expandList, setExpandList] = useState<boolean>(false)
    const { currentContext } = useModuleCurrentContext()
    const { setShowRevisionReminder } = useAppContext()
    const navigate = useNavigate()
    const externalId = currentContext?.externalId

    const defaultColDef = useMemo(() => ({
        sortable: true,
        filter: true,
        resizable: true,
        suppressHeaderMenuButton: true,
    }), [])

    const { data: apiData } = useQuery({
        queryKey: ["projectApiData", externalId],
        queryFn: () => projectQueryFn(externalId),
        enabled: !!externalId,
    })

    const { data: apiRevisionData } = useQuery({
        queryKey: ["revisionApiData", revisionId],
        queryFn: () => revisionQueryFn(projectId, revisionId),
        enabled: !!projectId && !!revisionId && !!externalId && isRevision,
    })

    const selectCase = (p: any) => {
        if (!currentContext || !p.node.data) { return null }

        navigate(caseRevisionPath(currentContext.id, p.node.data.id, isRevision, revisionId))
        return null
    }

    const nameWithReferenceCase = (p: any) => (
        <Tooltip title={p.value} placement="bottom-start">
            <Wrapper>
                {p.node.data.referenceCaseId === p.node.data.id && (
                    <ReferenceCaseIcon iconPlacement="casesTable" />
                )}
                <Button as="span" variant="ghost" className="GhostButton" onClick={() => selectCase(p)}>{p.value}</Button>
            </Wrapper>
        </Tooltip>
    )

    const onMoreClick = (data: TableCase, target: HTMLElement) => {
        setSelectedCaseId(data.id)
        setMenuAnchorEl(target)
        setIsMenuOpen(!isMenuOpen)
    }

    const menuButton = (p: any) => (
        <Button
            variant="ghost"
            onClick={(e) => onMoreClick(p.node.data, e.currentTarget)}

        >
            <Icon data={more_vertical} />
        </Button>
    )

    const productionStrategyToString = (p: any) => {
        const stringValue = productionStrategyOverviewToString(p.value)
        return <div>{stringValue}</div>
    }

    const GetColumnDefs = () => [
        {
            field: "name",
            cellRenderer: nameWithReferenceCase,
            minWidth: 150,
            maxWidth: 500,
            flex: 1,
        },
        {
            field: "productionStrategyOverview",
            headerName: "Production Strategy Overview",
            headerTooltip: "Production Strategy Overview",
            cellRenderer: productionStrategyToString,
            width: 280,
        },
        {
            field: "producerCount",
            headerName: "Producers",
            width: 130,
            cellStyle: cellStyleRightAlign,
        },
        {
            field: "gasInjectorCount",
            headerName: "Gas injectors",
            width: 155,
            cellStyle: cellStyleRightAlign,
        },
        {
            field: "waterInjectorCount",
            headerName: "Water injectors",
            width: 170,
            cellStyle: cellStyleRightAlign,
        },
        {
            field: "createdAt",
            headerName: "Created",
            width: 120,
        },
        {
            field: "Options",
            cellRenderer: menuButton,
            width: 120,
        },
    ]

    const [columnDefs, setColumnDefs] = useState<ColDef[]>(GetColumnDefs())

    useEffect(() => {
        setColumnDefs(GetColumnDefs())
    }, [isRevision])

    const casesToRowData = (isArchived: boolean) => {
        let data: Components.Schemas.RevisionDataDto | Components.Schemas.ProjectDataDto | null | undefined = apiData
        if (isRevision && apiRevisionData) {
            data = apiRevisionData
        }
        if (data && data.commonProjectAndRevisionData.cases) {
            const cases = isArchived ? data.commonProjectAndRevisionData.cases.filter((c) => !c.archived) : data.commonProjectAndRevisionData.cases.filter((c) => c.archived)
            const tableCases: TableCase[] = []
            cases.forEach((c) => {
                const tableCase: TableCase = {
                    id: c.id!,
                    name: c.name ?? "",
                    description: c.description ?? "",
                    productionStrategyOverview: c.productionStrategyOverview,
                    producerCount: c.producerCount ?? 0,
                    waterInjectorCount: c.waterInjectorCount ?? 0,
                    gasInjectorCount: c.gasInjectorCount ?? 0,
                    createdAt: c.createTime?.substring(0, 10),
                    referenceCaseId: data.commonProjectAndRevisionData.referenceCaseId,
                }
                tableCases.push(tableCase)
            })
            if (isArchived) {
                setRowData(tableCases)
            } else {
                setArchivedRowData(tableCases)
            }
        }
    }

    useEffect(() => {
        casesToRowData(true)
        casesToRowData(false)
    }, [apiData, apiRevisionData])

    if (!apiData) {
        return <p>project not found</p>
    }
    if (isRevision && !apiRevisionData) {
        return <p>revision not found</p>
    }

    const submitToSTEA: MouseEventHandler<HTMLButtonElement> = async (e) => {
        e.preventDefault()

        if (apiData) {
            try {
                setShowRevisionReminder(true)
                const unwrappedProjectId = unwrapProjectId(apiData.projectId)
                const projectResult = await (await GetProjectService()).getProject(unwrappedProjectId)
                await (await GetSTEAService()).excelToSTEA(projectResult)
            } catch (error) {
                console.error("[ProjectView] error while submitting form data", error)
            }
        }
    }

    return (
        <div>
            <AgTableContainer>
                <AgGridReact
                    ref={gridRef}
                    rowData={rowData}
                    columnDefs={columnDefs}
                    defaultColDef={defaultColDef}
                    animateRows
                    domLayout="autoHeight"
                />
            </AgTableContainer>
            <DownloadButton>
                <Button variant="outlined" onClick={submitToSTEA}>
                    <Icon data={archive} size={18} />
                    Download input to STEA
                </Button>
            </DownloadButton>
            {archivedRowData && archivedRowData.length > 0 ? (
                <div>
                    <ArchivedTitle>
                        <Typography variant="h3">Archived Cases</Typography>
                        {!expandList ? (
                            <Tooltip title="Expand Archived Cases">
                                <Button variant="ghost_icon" className="GhostButton">
                                    <Icon data={arrow_drop_down} onClick={() => setExpandList(true)} />
                                </Button>
                            </Tooltip>
                        ) : (
                            <Tooltip title="Collapse Archived Cases">
                                <Button variant="ghost_icon" className="GhostButton">
                                    <Icon data={arrow_drop_up} onClick={() => setExpandList(false)} />
                                </Button>
                            </Tooltip>
                        )}
                    </ArchivedTitle>
                    {expandList && (
                        <AgTableContainer>
                            <AgGridReact
                                ref={gridRef}
                                rowData={archivedRowData}
                                columnDefs={columnDefs}
                                defaultColDef={defaultColDef}
                                animateRows
                                domLayout="autoHeight"
                            />
                        </AgTableContainer>
                    )}
                </div>
            ) : null}
        </div>
    )
}

export default CasesAgGridTable

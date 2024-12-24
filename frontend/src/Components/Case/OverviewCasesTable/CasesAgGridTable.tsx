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
} from "react"
import { useNavigate, useParams } from "react-router-dom"
import { useModuleCurrentContext } from "@equinor/fusion-framework-react-module-context"
import { AgGridReact } from "@ag-grid-community/react"
import {
    arrow_drop_down, arrow_drop_up, more_vertical, archive,
} from "@equinor/eds-icons"
import styled from "styled-components"
import { ColDef } from "@ag-grid-community/core"

import {
    productionStrategyOverviewToString, cellStyleRightAlign, unwrapProjectId,
    caseRevisionPath,
} from "@/Utils/common"
import { GetProjectService } from "@/Services/ProjectService"
import { GetSTEAService } from "@/Services/STEAService"
import { ReferenceCaseIcon } from "../Components/ReferenceCaseIcon"
import { useProjectContext } from "@/Context/ProjectContext"
import { useAppContext } from "@/Context/AppContext"
import { useDataFetch } from "@/Hooks/useDataFetch"

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
    const { isRevision } = useProjectContext()
    const { revisionId } = useParams()
    const gridRef = useRef<AgGridReact>(null)
    const [activeCases, setActiveCases] = useState<TableCase[]>()
    const [archivedCases, setArchivedCases] = useState<TableCase[]>()
    const [isArchivedExpanded, setIsArchivedExpanded] = useState<boolean>(false)
    const { currentContext } = useModuleCurrentContext()
    const { setShowRevisionReminder } = useAppContext()
    const navigate = useNavigate()
    const revisionAndProjectData = useDataFetch()

    const defaultColumnDefinition = useMemo(() => ({
        sortable: true,
        filter: true,
        resizable: true,
        suppressHeaderMenuButton: true,
    }), [])

    const navigateToCase = (caseData: any) => {
        if (!currentContext || !caseData.node.data) {
            return
        }

        const path = caseRevisionPath(currentContext.id, caseData.node.data.id, isRevision, revisionId)
        navigate(path)
    }

    const renderCaseName = (caseData: any) => (
        <Tooltip title={caseData.value} placement="bottom-start">
            <Wrapper>
                {caseData.node.data.referenceCaseId === caseData.node.data.id && (
                    <ReferenceCaseIcon iconPlacement="casesTable" />
                )}
                <Button
                    as="span"
                    variant="ghost"
                    className="GhostButton"
                    onClick={() => navigateToCase(caseData)}
                >
                    {caseData.value}
                </Button>
            </Wrapper>
        </Tooltip>
    )

    const handleMenuClick = (caseData: TableCase, target: HTMLElement) => {
        setSelectedCaseId(caseData.id)
        setMenuAnchorEl(target)
        setIsMenuOpen(!isMenuOpen)
    }

    const renderMenuButton = (caseData: any) => (
        <Button
            variant="ghost"
            onClick={(e) => handleMenuClick(caseData.node.data, e.currentTarget)}
        >
            <Icon data={more_vertical} />
        </Button>
    )

    const renderProductionStrategy = (caseData: any) => {
        const strategyString = productionStrategyOverviewToString(caseData.value)
        return <div>{strategyString}</div>
    }

    const getColumnDefinitions = () => [
        {
            field: "name",
            cellRenderer: renderCaseName,
            minWidth: 150,
            maxWidth: 500,
            flex: 1,
        },
        {
            field: "productionStrategyOverview",
            headerName: "Production Strategy Overview",
            headerTooltip: "Production Strategy Overview",
            cellRenderer: renderProductionStrategy,
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
            cellRenderer: renderMenuButton,
            width: 120,
        },
    ]

    const [columnDefinitions, setColumnDefinitions] = useState<ColDef[]>(getColumnDefinitions())

    useEffect(() => {
        setColumnDefinitions(getColumnDefinitions())
    }, [isRevision, revisionAndProjectData])

    const mapCasesToTableData = (isArchived: boolean) => {
        if (!revisionAndProjectData?.commonProjectAndRevisionData?.cases) { return [] }
        return revisionAndProjectData.commonProjectAndRevisionData.cases
            .filter((c) => c.archived === isArchived)
            .map((c) => ({
                id: c.caseId!,
                name: c.name ?? "",
                description: c.description ?? "",
                productionStrategyOverview: c.productionStrategyOverview,
                producerCount: c.producerCount ?? 0,
                waterInjectorCount: c.waterInjectorCount ?? 0,
                gasInjectorCount: c.gasInjectorCount ?? 0,
                createdAt: c.createTime?.substring(0, 10),
                referenceCaseId: revisionAndProjectData.commonProjectAndRevisionData.referenceCaseId,
            }))
    }

    useEffect(() => {
        if (!revisionAndProjectData) { return }

        const mappedActiveCases = mapCasesToTableData(false)
        const mappedArchivedCases = mapCasesToTableData(true)

        setActiveCases(mappedActiveCases)
        setArchivedCases(mappedArchivedCases)
    }, [isRevision, revisionAndProjectData])

    if (!revisionAndProjectData && !isRevision) {
        return <p>Project not found</p>
    }
    if (!revisionAndProjectData && isRevision) {
        return <p>Revision not found</p>
    }

    const handleSTEAExport = async (e: React.MouseEvent<HTMLButtonElement>) => {
        e.preventDefault()

        if (revisionAndProjectData) {
            try {
                setShowRevisionReminder(true)
                if (!revisionAndProjectData) {
                    return
                }
                const projectOrRevisionId = isRevision
                    ? (revisionAndProjectData as { revisionId: string }).revisionId
                    : (revisionAndProjectData as { projectId: string }).projectId
                const unwrappedProjectId = unwrapProjectId(projectOrRevisionId)
                const projectResult = await (await GetProjectService()).getProject(unwrappedProjectId)
                await (await GetSTEAService()).excelToSTEA(projectResult)
            } catch (error) {
                console.error("[ProjectView] Error while submitting form data", error)
            }
        }
    }

    return (
        <div>
            <AgTableContainer>
                <AgGridReact
                    ref={gridRef}
                    rowData={activeCases}
                    columnDefs={columnDefinitions}
                    defaultColDef={defaultColumnDefinition}
                    animateRows
                    domLayout="autoHeight"
                />
            </AgTableContainer>
            <DownloadButton>
                <Button variant="outlined" onClick={handleSTEAExport}>
                    <Icon data={archive} size={18} />
                    Download input to STEA
                </Button>
            </DownloadButton>
            {archivedCases && archivedCases.length > 0 && (
                <div>
                    <ArchivedTitle>
                        <Typography variant="h3">Archived Cases</Typography>
                        <Tooltip title={isArchivedExpanded ? "Collapse Archived Cases" : "Expand Archived Cases"}>
                            <Button variant="ghost_icon" className="GhostButton" onClick={() => setIsArchivedExpanded(!isArchivedExpanded)}>
                                <Icon data={isArchivedExpanded ? arrow_drop_up : arrow_drop_down} />
                            </Button>
                        </Tooltip>
                    </ArchivedTitle>
                    {isArchivedExpanded && (
                        <AgTableContainer>
                            <AgGridReact
                                ref={gridRef}
                                rowData={archivedCases}
                                columnDefs={columnDefinitions}
                                defaultColDef={defaultColumnDefinition}
                                animateRows
                                domLayout="autoHeight"
                            />
                        </AgTableContainer>
                    )}
                </div>
            )}
        </div>
    )
}

export default CasesAgGridTable

import { default as Grid } from "@mui/material/Grid2"
import { Tooltip } from "@equinor/eds-core-react"
import { useLocation, useParams } from "react-router-dom"
import styled from "styled-components"
import { useMemo } from "react"

import { EMPTY_GUID } from "@/Utils/constants"
import { productionStrategyOverviewToString, truncateText } from "@/Utils/common"
import { ReferenceCaseIcon } from "@/Components/Tables/ProjectTables/OverviewCasesTable/CellRenderers/ReferenceCaseIcon"
import { useDataFetch } from "@/Hooks/useDataFetch"
import { useAppNavigation } from "@/Hooks/useNavigate"
import { useAppContext } from "@/Context/AppContext"
import { TimelineElement } from "../../SidebarWrapper"
import { sortUtcDateStrings } from "@/Utils/DateUtils"

const SideBarRefCaseWrapper = styled.div`
    justify-content: center;
`

interface ArchivedCase extends Components.Schemas.CaseOverviewDto { isReferenceCase: boolean}

const sortCasesByDate = (a: ArchivedCase, b: ArchivedCase) => sortUtcDateStrings(a.createTime, b.createTime)

const getTooltipText = (
    caseName: string | undefined,
    strategy: Components.Schemas.ProductionStrategyOverview | undefined,
) => `${caseName || "Untitled"} - Strategy: ${productionStrategyOverviewToString(strategy)}`

const getCaseDisplayName = (caseName: string | undefined, index: number, isSidebarOpen: boolean) => {
    if (!isSidebarOpen) { return `#${index + 1}` }
    return caseName ? truncateText(caseName, 30) : "Untitled"
}

const ArchivedCasesList = (): JSX.Element | null => {
    const { revisionId } = useParams()
    const { sidebarOpen } = useAppContext()
    const location = useLocation()
    const revisionAndProjectData = useDataFetch()
    const { navigateToCase, navigateToRevisionCase } = useAppNavigation()

    const archivedCases = useMemo(
        () => {
            if (!revisionAndProjectData?.commonProjectAndRevisionData.cases) { return [] }

            const { referenceCaseId } = revisionAndProjectData.commonProjectAndRevisionData
            return revisionAndProjectData.commonProjectAndRevisionData.cases
                .filter((c) => c.archived)
                .map((c) => ({
                    ...c,
                    isReferenceCase: c.caseId === referenceCaseId,
                }))
                .sort(sortCasesByDate)
        },
        [revisionAndProjectData?.commonProjectAndRevisionData],
    )

    const handleCaseClick = (caseId: string) => {
        const navigate = revisionId ? navigateToRevisionCase : navigateToCase
        navigate(revisionId ?? "", caseId)
    }

    const renderCaseContent = (projectCase: ArchivedCase, index: number) => (
        <>
            {projectCase.isReferenceCase && (
                <SideBarRefCaseWrapper>
                    <ReferenceCaseIcon iconPlacement="sideBar" />
                </SideBarRefCaseWrapper>
            )}
            {projectCase.caseId !== EMPTY_GUID
                && getCaseDisplayName(projectCase.name, index, sidebarOpen)}
        </>
    )

    if (!archivedCases?.length) { return null }

    return (
        <>
            {archivedCases.map((projectCase, index) => (
                <Grid
                    container
                    key={projectCase.caseId}
                    data-timeline-active={location.pathname.includes(projectCase.caseId)}
                >
                    <Tooltip
                        title={getTooltipText(projectCase.name, projectCase.productionStrategyOverview)}
                        placement="right"
                    >
                        <TimelineElement
                            variant="ghost"
                            className="GhostButton"
                            onClick={() => handleCaseClick(projectCase.caseId)}
                            $isSelected={location.pathname.includes(projectCase.caseId)}
                            $isArchived
                        >
                            {renderCaseContent(projectCase, index)}
                        </TimelineElement>
                    </Tooltip>
                </Grid>
            ))}
        </>
    )
}

export default ArchivedCasesList

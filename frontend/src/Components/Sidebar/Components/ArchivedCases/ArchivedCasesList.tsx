import { Tooltip } from "@equinor/eds-core-react"
import Grid from "@mui/material/Grid2"
import { useMemo } from "react"
import { useLocation, useParams } from "react-router-dom"
import styled from "styled-components"

import { TimelineElement } from "@/Components/Sidebar/SidebarWrapper"
import { ReferenceCaseIcon } from "@/Components/Tables/Components/CellRenderers/ReferenceCaseIcon"
import { useDataFetch } from "@/Hooks"
import { useAppNavigation } from "@/Hooks/useNavigate"
import { useAppStore } from "@/Store/AppStore"
import { useCaseStore } from "@/Store/CaseStore"
import { caseTabNames, PRODUCTION_STRATEGY } from "@/Utils/Config/constants"
import { sortUtcDateStrings } from "@/Utils/DateUtils"
import { truncateText } from "@/Utils/FormatingUtils"

const SideBarRefCaseWrapper = styled.div`
    justify-content: center;
`

interface ArchivedCase extends Components.Schemas.CaseOverviewDto { isReferenceCase: boolean}

const sortCasesByDate = (a: ArchivedCase, b: ArchivedCase) => sortUtcDateStrings(a.createdUtc, b.createdUtc)

const getTooltipText = (
    caseName: string | undefined,
    strategy: Components.Schemas.ProductionStrategyOverview | undefined,
) => `${caseName || "Untitled"} - Strategy: ${strategy !== undefined ? PRODUCTION_STRATEGY[strategy] : "Unknown"}`

const getCaseDisplayName = (caseName: string | undefined, index: number, isSidebarOpen: boolean) => {
    if (!isSidebarOpen) { return `#${index + 1}` }

    return caseName ? truncateText(caseName, 30) : "Untitled"
}

const ArchivedCasesList = (): JSX.Element | null => {
    const { revisionId } = useParams()
    const { sidebarOpen } = useAppStore()
    const location = useLocation()
    const revisionAndProjectData = useDataFetch()
    const { navigateToCase, navigateToRevisionCase } = useAppNavigation()
    const { activeTabCase } = useCaseStore()

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
        const currentTab = caseTabNames[activeTabCase]

        if (revisionId) {
            navigateToRevisionCase(revisionId, caseId, currentTab)
        } else {
            navigateToCase(caseId, currentTab)
        }
    }

    const renderCaseContent = (projectCase: ArchivedCase, index: number) => (
        <>
            {projectCase.isReferenceCase && (
                <SideBarRefCaseWrapper>
                    <ReferenceCaseIcon iconPlacement="sideBar" />
                </SideBarRefCaseWrapper>
            )}
            {projectCase.caseId !== null
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

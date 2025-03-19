import { Tooltip } from "@equinor/eds-core-react"
import Grid from "@mui/material/Grid2"
import React, { useMemo } from "react"
import { useLocation, useParams } from "react-router-dom"
import styled from "styled-components"

import { TimelineElement } from "@/Components/Sidebar/SidebarWrapper"
import { ReferenceCaseIcon } from "@/Components/Tables/Components/CellRenderers/ReferenceCaseIcon"
import { useDataFetch } from "@/Hooks"
import { useAppNavigation } from "@/Hooks/useNavigate"
import { useAppStore } from "@/Store/AppStore"
import { useCaseStore } from "@/Store/CaseStore"
import { useProjectContext } from "@/Store/ProjectContext"
import { caseTabNames } from "@/Utils/Config/constants"
import { sortUtcDateStrings } from "@/Utils/DateUtils"
import {
    productionStrategyOverviewToString, truncateText,
} from "@/Utils/commonUtils"

const SideBarRefCaseWrapper = styled.div`
    justify-content: center;
    align-items: center;
    display: inline-flex;
`

const CasesList: React.FC = () => {
    const { sidebarOpen, setEditMode } = useAppStore()
    const { isRevision } = useProjectContext()
    const { revisionId } = useParams()
    const revisionAndProjectData = useDataFetch()
    const { navigateToCase, navigateToRevisionCase } = useAppNavigation()
    const { activeTabCase } = useCaseStore()

    const location = useLocation()

    const selectCase = (caseId: string) => {
        if (!caseId) { return null }
        setEditMode(false)
        const currentTab = caseTabNames[activeTabCase]

        if (isRevision && revisionId) {
            navigateToRevisionCase(revisionId, caseId, currentTab)
        } else {
            navigateToCase(caseId, currentTab)
        }

        return null
    }

    const cases = useMemo(
        () => {
            const filteredCases = revisionAndProjectData?.commonProjectAndRevisionData.cases

            return filteredCases ? filteredCases.filter((c) => !c.archived) : []
        },
        [revisionAndProjectData],
    )

    if (!revisionAndProjectData) {
        return null
    }

    return (
        <>
            {cases.sort((a, b) => sortUtcDateStrings(a.createdUtc, b.createdUtc)).map((projectCase, index) => (
                <Grid
                    container
                    justifyContent="space-between"
                    key={`menu - item - ${index + 1} `}
                    data-timeline-active={location.pathname.includes(projectCase.caseId)}
                >
                    <Tooltip
                        title={`${
                            projectCase.name ? truncateText(projectCase.name, 120) : "Untitled"
                        } - Strategy: ${
                            productionStrategyOverviewToString(projectCase.productionStrategyOverview)
                        }`}
                        placement="right"
                    >
                        <TimelineElement variant="ghost" className="GhostButton" onClick={() => selectCase(projectCase.caseId)}>
                            {revisionAndProjectData?.commonProjectAndRevisionData.referenceCaseId !== null
                                ? (
                                    <SideBarRefCaseWrapper>
                                        {sidebarOpen && revisionAndProjectData?.commonProjectAndRevisionData.referenceCaseId === projectCase.caseId && (
                                            <ReferenceCaseIcon iconPlacement="sideBar" />
                                        )}
                                        {!sidebarOpen && `#${index + 1}`}
                                        {(sidebarOpen && projectCase.name) && truncateText(projectCase.name, 30)}
                                        {(sidebarOpen && (projectCase.name === "" || projectCase.name === undefined)) && "Untitled"}
                                        {!sidebarOpen && revisionAndProjectData?.commonProjectAndRevisionData.referenceCaseId === projectCase.caseId && (
                                            <ReferenceCaseIcon iconPlacement="sideBar" />
                                        )}
                                    </SideBarRefCaseWrapper>
                                )
                                : (
                                    <>
                                        {!sidebarOpen && `#${index + 1}`}
                                        {(sidebarOpen && projectCase.name) && truncateText(projectCase.name, 30)}
                                        {(sidebarOpen && (projectCase.name === "" || projectCase.name === undefined)) && "Untitled"}
                                    </>
                                )}
                        </TimelineElement>
                    </Tooltip>
                </Grid>
            ))}
        </>
    )
}

export default CasesList

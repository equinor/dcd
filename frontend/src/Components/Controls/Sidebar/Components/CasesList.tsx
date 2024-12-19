import React, { useMemo } from "react"
import { Grid } from "@mui/material"
import { Tooltip } from "@equinor/eds-core-react"
import { useLocation, useNavigate, useParams } from "react-router-dom"
import { useModuleCurrentContext } from "@equinor/fusion-framework-react-module-context"
import styled from "styled-components"

import {
    productionStrategyOverviewToString, truncateText, caseRevisionPath,
} from "@/Utils/common"
import { ReferenceCaseIcon } from "@/Components/Case/Components/ReferenceCaseIcon"
import { useProjectContext } from "@/Context/ProjectContext"
import { useAppContext } from "@/Context/AppContext"
import { useDataFetch } from "@/Hooks/useDataFetch"
import { EMPTY_GUID } from "@/Utils/constants"
import { TimelineElement } from "../Sidebar"

const SideBarRefCaseWrapper = styled.div`
    justify-content: center;
    align-items: center;
    display: inline-flex;
`

const CasesList: React.FC = () => {
    const { sidebarOpen } = useAppContext()
    const { currentContext } = useModuleCurrentContext()
    const { isRevision } = useProjectContext()
    const { revisionId } = useParams()
    const revisionAndProjectData = useDataFetch()

    const location = useLocation()
    const navigate = useNavigate()

    const selectCase = (caseId: string) => {
        if (!currentContext || !caseId) { return null }
        navigate(caseRevisionPath(currentContext.id, caseId, isRevision, revisionId))
        return null
    }

    const cases = useMemo(
        () => {
            const filteredCases = revisionAndProjectData?.commonProjectAndRevisionData.cases
            return filteredCases ? filteredCases.filter((c) => !c.archived) : []
        },
        [revisionAndProjectData],
    )

    if ((!revisionAndProjectData && !currentContext)) {
        return null
    }

    return (
        <>
            {cases.sort((a, b) => new Date(a.createTime).getDate() - new Date(b.createTime).getDate()).map((projectCase, index) => (
                <Grid
                    item
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
                            {revisionAndProjectData?.commonProjectAndRevisionData.referenceCaseId !== EMPTY_GUID
                                ? (
                                    <SideBarRefCaseWrapper>

                                        {!sidebarOpen && `#${index + 1}`}
                                        {(sidebarOpen && projectCase.name) && truncateText(projectCase.name, 30)}
                                        {(sidebarOpen && (projectCase.name === "" || projectCase.name === undefined)) && "Untitled"}
                                        {revisionAndProjectData?.commonProjectAndRevisionData.referenceCaseId === projectCase.caseId && (
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

import React, { useMemo } from "react"
import { Grid } from "@mui/material"
import { Tooltip } from "@equinor/eds-core-react"
import { useLocation, useNavigate } from "react-router-dom"
import { useModuleCurrentContext } from "@equinor/fusion-framework-react-module-context"
import styled from "styled-components"

import { EMPTY_GUID } from "@/Utils/constants"
import { productionStrategyOverviewToString, casePath, truncateText } from "@/Utils/common"
import { ReferenceCaseIcon } from "@/Components/Case/Components/ReferenceCaseIcon"
import { useAppContext } from "@/Context/AppContext"
import { useDataFetch } from "@/Hooks/useDataFetch"
import { TimelineElement } from "../Sidebar"

const SideBarRefCaseWrapper = styled.div`
    justify-content: center;
    align-items: center;
    display: inline-flex;
`

const ArchivedCasesList: React.FC = () => {
    const { sidebarOpen } = useAppContext()
    const { currentContext } = useModuleCurrentContext()
    const revisionAndProjectData = useDataFetch()

    if (!revisionAndProjectData || !currentContext) { return null }

    const location = useLocation()
    const navigate = useNavigate()

    const selectCase = (caseId: string) => {
        if (!currentContext || !caseId) { return null }
        navigate(casePath(currentContext.id, caseId))
        return null
    }

    const archivedCases = useMemo(
        () => revisionAndProjectData.commonProjectAndRevisionData.cases.filter((c) => c.archived),
        [revisionAndProjectData.commonProjectAndRevisionData.cases],
    )

    return (
        <>
            {archivedCases.sort((a, b) => new Date(a.createTime).getDate() - new Date(b.createTime).getDate()).map((projectCase, index) => (
                <Grid
                    item
                    container
                    justifyContent="space-between"
                    key={`menu - item - ${index + 1} `}
                    data-timeline-active={location.pathname.includes(projectCase.caseId)}
                >
                    <Tooltip
                        title={`
                            ${projectCase.name
                    ? truncateText(projectCase.name, 120)
                    : "Untitled"} - Strategy: ${productionStrategyOverviewToString(projectCase.productionStrategyOverview)}`}
                        placement="right"
                    >
                        <TimelineElement variant="ghost" className="GhostButton" onClick={() => selectCase(projectCase.caseId)}>
                            {revisionAndProjectData?.commonProjectAndRevisionData.referenceCaseId !== EMPTY_GUID
                                ? (
                                    <SideBarRefCaseWrapper>

                                        {!sidebarOpen && `#${index + 1}`}
                                        {(sidebarOpen && projectCase.name) && truncateText(projectCase.name, 30)}
                                        {(sidebarOpen && (projectCase.name === "" || projectCase.name === undefined)) && "Untitled"}
                                        {revisionAndProjectData?.commonProjectAndRevisionData.referenceCaseId === projectCase?.caseId && (
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

export default ArchivedCasesList

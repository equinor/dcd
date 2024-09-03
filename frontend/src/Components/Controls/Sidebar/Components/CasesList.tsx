import React from "react"
import { Grid } from "@mui/material"
import { Tooltip } from "@equinor/eds-core-react"
import { useLocation, useNavigate } from "react-router-dom"
import { useModuleCurrentContext } from "@equinor/fusion-framework-react-module-context"
import styled from "styled-components"
import { useProjectContext } from "../../../../Context/ProjectContext"
import { productionStrategyOverviewToString, casePath } from "../../../../Utils/common"
import { TimelineElement } from "../Sidebar"
import { useAppContext } from "../../../../Context/AppContext"
import { ReferenceCaseIcon } from "../../../Case/Components/ReferenceCaseIcon"
import { EMPTY_GUID } from "../../../../Utils/constants"

const SideBarRefCaseWrapper = styled.div`
    justify-content: center;
    align-items: center;
    display: inline-flex;
`

const CasesList: React.FC = () => {
    const { project } = useProjectContext()
    const { sidebarOpen } = useAppContext()
    const { currentContext } = useModuleCurrentContext()

    if (!project || !currentContext) { return null }

    const location = useLocation()
    const navigate = useNavigate()

    const selectCase = (caseId: string) => {
        if (!currentContext || !caseId) { return null }
        navigate(casePath(currentContext.id, caseId))
        return null
    }

    return (
        <>
            {project.cases.sort((a, b) => new Date(a.createTime).getDate() - new Date(b.createTime).getDate()).map((projectCase, index) => (
                <Grid
                    item
                    container
                    justifyContent="space-between"
                    key={`menu - item - ${index + 1} `}
                    data-timeline-active={location.pathname.includes(projectCase.id)}
                >
                    <Tooltip
                        title={`${projectCase.name ? projectCase.name : "Untitled"} - Strategy: ${productionStrategyOverviewToString(projectCase.productionStrategyOverview)}`}
                        placement="right"
                    >
                        <TimelineElement variant="ghost" className="GhostButton" onClick={() => selectCase(projectCase.id)}>
                            {project?.referenceCaseId !== EMPTY_GUID
                                ? (
                                    <SideBarRefCaseWrapper>

                                        {!sidebarOpen && `#${index + 1}`}
                                        {(sidebarOpen && projectCase.name) && projectCase.name}
                                        {(sidebarOpen && (projectCase.name === "" || projectCase.name === undefined)) && "Untitled"}
                                        {project?.referenceCaseId === projectCase?.id && (
                                            <ReferenceCaseIcon iconPlacement="sideBar" />
                                        )}
                                    </SideBarRefCaseWrapper>
                                )
                                : (
                                    <>
                                        {!sidebarOpen && `#${index + 1}`}
                                        {(sidebarOpen && projectCase.name) && projectCase.name}
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

import React from "react"
import { Grid } from "@mui/material"
import { Tooltip } from "@equinor/eds-core-react"
import { useLocation, useNavigate } from "react-router-dom"
import { useModuleCurrentContext } from "@equinor/fusion-framework-react-module-context"
import { useProjectContext } from "../../../../Context/ProjectContext"
import { productionStrategyOverviewToString, casePath } from "../../../../Utils/common"
import { TimelineElement } from "../Sidebar"
import { useAppContext } from "../../../../Context/AppContext"
import { useCaseContext } from "../../../../Context/CaseContext"
import { ReferenceCaseIcon, SideBarRefCaseWrapper } from "../../../Case/Components/ReferenceCaseIcon"
import { EMPTY_GUID } from "../../../../Utils/constants"

const CasesList: React.FC = () => {
    const { project } = useProjectContext()
    const { sidebarOpen } = useAppContext()
    const { currentContext } = useModuleCurrentContext()

    if (!project || !currentContext) { return null }

    const location = useLocation()
    const navigate = useNavigate()
    const { setProjectCase } = useCaseContext()

    const selectCase = (caseId: string) => {
        if (!currentContext || !caseId) { return null }
        const caseResult = project.cases.find((o) => o.id === caseId)
        setProjectCase(caseResult)
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
                                        {project?.referenceCaseId === projectCase?.id && (
                                            <ReferenceCaseIcon iconPlacement="sideBar" />
                                        )}
                                        {!sidebarOpen && `#${index + 1}`}
                                        {(sidebarOpen && projectCase.name) && projectCase.name}
                                        {(sidebarOpen && (projectCase.name === "" || projectCase.name === undefined)) && "Untitled"}
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

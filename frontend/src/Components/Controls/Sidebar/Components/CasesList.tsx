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
            {project.cases.sort((a, b) => new Date(a.createTime).getDate() - new Date(b.createTime).getDate()).map((subItem, index) => (
                <Grid
                    item
                    container
                    justifyContent="space-between"
                    key={`menu - item - ${index + 1} `}
                    data-timeline-active={location.pathname.includes(subItem.id)}
                >
                    <Tooltip
                        title={`${subItem.name ? subItem.name : "Untitled"} - Strategy: ${productionStrategyOverviewToString(subItem.productionStrategyOverview)}`}
                        placement="right"
                    >
                        <TimelineElement variant="ghost" className="GhostButton" onClick={() => selectCase(subItem.id)}>
                            {!sidebarOpen && `#${index + 1}`}
                            {(sidebarOpen && subItem.name) && subItem.name}
                            {(sidebarOpen && (subItem.name === "" || subItem.name === undefined)) && "Untitled"}
                        </TimelineElement>
                    </Tooltip>
                </Grid>
            ))}
        </>
    )
}

export default CasesList

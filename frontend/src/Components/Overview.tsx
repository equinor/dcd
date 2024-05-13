import { useEffect } from "react"
import { Outlet, useParams } from "react-router-dom"
import Grid from "@mui/material/Grid"
import { Progress, Typography } from "@equinor/eds-core-react"
import Sidebar from "./Sidebar"
import Controls from "./Controls"
import { useAppContext } from "../Context/AppContext"
import { useProjectContext } from "../Context/ProjectContext"
import { useCaseContext } from "../Context/CaseContext"

const Overview = () => {
    const {
        isCreating, isLoading, editMode, sidebarOpen,
    } = useAppContext()
    const { project } = useProjectContext()
    const { setProjectCase } = useCaseContext()

    const { caseId } = useParams()

    useEffect(() => {
        if (project && !editMode && caseId) {
            const foundCase = project.cases.find((c) => c.id === caseId)
            setProjectCase(foundCase)
        } else if (!caseId) {
            setProjectCase(undefined)
        }
    }, [project, caseId, editMode])

    return (
        <Grid container className="ConceptApp MainGrid" direction="column">
            <Grid item alignSelf="stretch" flexGrow={1}>
                <Sidebar />
            </Grid>
            {(isCreating || isLoading)
                ? (
                    <Grid item flexGrow={0} alignSelf="stretch" container spacing={1} alignItems="center" justifyContent="center">
                        <Grid item>
                            <Progress.Circular size={24} color="primary" />
                        </Grid>
                        <Grid item>
                            <Typography variant="h3">{isCreating ? "Creating" : "Loading"}</Typography>
                        </Grid>
                    </Grid>
                )
                : (
                    <Grid
                        item
                        className="ag-theme-alpine-fusion ContentOverview"
                        spacing={2}
                        sx={{ width: `calc(100% - ${sidebarOpen ? "200px" : "72px"})` }}
                    >
                        <Grid>
                            <Controls />
                        </Grid>
                        <Grid className="ContentPanels">
                            <Outlet />
                        </Grid>
                    </Grid>
                )}
        </Grid>
    )
}

export default Overview

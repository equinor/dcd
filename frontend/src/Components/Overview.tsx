import { useEffect } from "react"
import { Outlet, useParams } from "react-router-dom"
import Grid from "@mui/material/Grid"
import { Progress, Snackbar, Typography } from "@equinor/eds-core-react"
import Sidebar from "./Controls/Sidebar/Sidebar"
import Controls from "./Controls/Controls"
import { useAppContext } from "../Context/AppContext"
import { useProjectContext } from "../Context/ProjectContext"
import { useCaseContext } from "../Context/CaseContext"

const Overview = () => {
    const {
        isCreating,
        isLoading,
        editMode,
        setEditMode,
        sidebarOpen,
        snackBarMessage,
        setSnackBarMessage,
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
        <Grid container display="grid" className="ConceptApp MainGrid" gridTemplateColumns={sidebarOpen ? "256px 1fr" : "72px 1fr"}>
            <Snackbar open={snackBarMessage !== undefined} autoHideDuration={6000} onClose={() => setSnackBarMessage(undefined)}>
                {snackBarMessage}
            </Snackbar>
            <Grid item alignSelf="stretch">
                <Sidebar />
            </Grid>
            {(isCreating || isLoading)
                ? (
                    <Grid item alignSelf="stretch" container spacing={1} alignItems="center" justifyContent="center">
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
                        alignSelf="flex-start"
                        className="ag-theme-alpine-fusion ContentOverview"
                        container
                        spacing={2}
                        alignItems="flex-start"
                        alignContent="flex-start"
                    >
                        <Grid item xs={12}>
                            <Controls />
                        </Grid>
                        <Grid item xs={12}>
                            <Outlet />
                        </Grid>
                    </Grid>
                )}
        </Grid>
    )
}

export default Overview

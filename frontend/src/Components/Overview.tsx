import { useEffect } from "react"
import { Outlet, useParams } from "react-router-dom"
import Sidebar from "./Sidebar"
import Controls from "./Controls"
import Grid from "@mui/material/Grid"
import { useAppContext } from "../Context/AppContext"
import { Progress, Typography } from "@equinor/eds-core-react"
import { useProjectContext } from "../Context/ProjectContext"
import { useCaseContext } from "../Context/CaseContext"

const Overview = () => {
    const { isCreating, isLoading, editMode } = useAppContext()
    const { project } = useProjectContext()
    const { projectCase, setProjectCase } = useCaseContext()
    
    let { caseId } = useParams()

    useEffect(() => {
        (project && !editMode) && setProjectCase(project.cases.find((c) => c.id === caseId))
        !caseId && setProjectCase(undefined)
    }, [project, caseId])

    return (
        <Grid container display="grid" gridTemplateColumns="256px auto" sx={{ height: "calc(100vh - 60px)" }} className="ConceptApp">
            <Grid item alignSelf="stretch">
                <Sidebar />
            </Grid>
            {(isCreating || isLoading)
            ? <Grid item alignSelf="stretch" container spacing={1} alignItems="center" justifyContent="center">
                <Grid item>
                    <Progress.Circular size={24} color="primary" />
                </Grid>
                <Grid item>
                    <Typography variant="h3">{isCreating ? "Creating" : "Loading"}</Typography>
                </Grid>
            </Grid>
            : <Grid item alignSelf="flex-start" className="ag-theme-alpine-fusion" sx={{ padding: "1rem" }} container spacing={2} alignItems="flex-start" alignContent="flex-start">
                <Grid item xs={12}>
                    <Controls />
                </Grid>
                <Grid item xs={12}>
                    <Outlet />
                </Grid>
            </Grid>}
        </Grid>
    )
}

export default Overview

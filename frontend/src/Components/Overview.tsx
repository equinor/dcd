import { useState, useEffect } from "react"
import { Outlet, useParams } from "react-router-dom"
import Grid from "@mui/material/Grid"
import { Button, Progress, Typography } from "@equinor/eds-core-react"
import { useCurrentUser } from "@equinor/fusion-framework-react/hooks"
import Sidebar from "./Controls/Sidebar/Sidebar"
import Controls from "./Controls/Controls"
import { useAppContext } from "../Context/AppContext"
import { useProjectContext } from "../Context/ProjectContext"
import { useCaseContext } from "../Context/CaseContext"
import Modal from "./Modal/Modal"
import { PROJECT_CLASSIFICATION } from "../Utils/constants"

interface WarnedProjectInterface {
    [key: string]: string[]
}

const Overview = () => {
    const currentUser = useCurrentUser()
    const {
        isCreating,
        isLoading,
        editMode,
        sidebarOpen,
    } = useAppContext()
    const { project } = useProjectContext()
    const { setProjectCase } = useCaseContext()
    const { caseId } = useParams()
    // Local variable for ease of object handling
    const [warnedProjects, setWarnedProjects] = useState<WarnedProjectInterface | null>(null)
    // Local variable for toggling warning modal
    const [projectClassificationWarning, setProjectClassificationWarning] = useState<boolean>(false)
    // Local variable to store logged in user ID
    const [currentUserId, setCurrentUserId] = useState<string | null>(null)

    function addVisitedProject() {
        if (project && currentUserId) {
            if (warnedProjects && warnedProjects[currentUserId]) {
                const wp = { ...warnedProjects }
                wp[currentUserId].push(project.id)
                setWarnedProjects(wp)
            } else {
                setWarnedProjects({ [currentUserId]: [project.id] })
            }
        }
    }

    useEffect(() => {
        // On load: start listening for the locally stored value of the classified projects visited
        window.addEventListener("storage", () => {
            // Update the local variable with the locally stored value. Usefull if localStorage is modified manually by the user (clear cache)
            setWarnedProjects(JSON.parse(localStorage.getItem("pv") || "null"))
            // NOTE: pv stands for "projects visited". It's been abbreviated to avoid disclosing the classification of the project's ID
        })
    }, [])

    useEffect(() => {
        if (project && !editMode && caseId) {
            const foundCase = project.cases.find((c) => c.id === caseId)
            setProjectCase(foundCase)
        } else if (!caseId) {
            setProjectCase(undefined)
        }
    }, [project, caseId, editMode])

    useEffect(() => {
        // Update the ID const with the fetched user data as long as ID hasn't yet been assigned or is a different user
        if (currentUser && (!currentUserId || currentUser.localAccountId !== currentUserId)) {
            // currentUserId string must be set as keyof to match the interface of warned projects
            setCurrentUserId(currentUser.localAccountId as keyof typeof warnedProjects)
        }
    }, [currentUser])

    useEffect(() => {
        if (warnedProjects && JSON.stringify(warnedProjects) !== localStorage.getItem("pv")) {
            localStorage.setItem("pv", JSON.stringify(warnedProjects))
        }
    }, [warnedProjects])

    useEffect(() => {
        if (project && currentUserId) {
            if (
                !projectClassificationWarning
                && PROJECT_CLASSIFICATION[project.classification].warn
                && (
                    (warnedProjects && !warnedProjects[currentUserId].some((vp: string) => vp === project.id))
                    || (warnedProjects && !warnedProjects[currentUserId])
                    || !warnedProjects
                )
            ) {
                // Show project classification warning if:
                // - Project's classification requires warning
                // - Current project ID has not been stored in localStorage
                setProjectClassificationWarning(true)
            }
            if (warnedProjects && warnedProjects[currentUserId].some((vp: string) => vp === project.id)) {
                // Hide project classification warning if:
                // - Current project ID has already been stored in localStorage
                setProjectClassificationWarning(false)
            }
        }
    }, [project, currentUserId, warnedProjects])

    return (
        <>
            <Grid container display="grid" className="ConceptApp MainGrid" gridTemplateColumns={sidebarOpen ? "256px 1fr" : "72px 1fr"}>
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
            {project && (
                <Modal
                    isOpen={projectClassificationWarning}
                    size="sm"
                    title={`Attention - ${PROJECT_CLASSIFICATION[project.classification].label} project`}
                >
                    <Typography>{PROJECT_CLASSIFICATION[project.classification].description}</Typography>
                    <Button onClick={() => addVisitedProject()}>OK</Button>
                </Modal>
            )}
        </>
    )
}

export default Overview

import { useState, useEffect } from "react"
import { Outlet, useParams } from "react-router-dom"
import Grid from "@mui/material/Grid"
import {
    Button,
    Progress,
    Snackbar,
    Typography,
} from "@equinor/eds-core-react"
import { useCurrentUser } from "@equinor/fusion-framework-react/hooks"
import styled from "styled-components"
import Sidebar from "./Controls/Sidebar/Sidebar"
import Controls from "./Controls/Controls"
import { useAppContext } from "../Context/AppContext"
import { useProjectContext } from "../Context/ProjectContext"
import Modal from "./Modal/Modal"
import { PROJECT_CLASSIFICATION } from "../Utils/constants"
import { useModalContext } from "../Context/ModalContext"
import ProjectSkeleton from "./LoadingSkeletons/ProjectSkeleton"

const ControlsWrapper = styled.div`
    position: sticky;
    top: 0;
    z-index: 1000;

`
const ContentWrapper = styled.div`
    display: flex;
    padding-bottom: 20px;
`

const MainView = styled.div`
    flex: 1;
`

interface WarnedProjectInterface {
    [key: string]: string[]
}

const Overview = () => {
    const currentUser = useCurrentUser()
    const {
        isCreating,
        isLoading,
        snackBarMessage,
        setSnackBarMessage,
    } = useAppContext()
    const { project } = useProjectContext()
    const { featuresModalIsOpen } = useModalContext()
    const [warnedProjects, setWarnedProjects] = useState<WarnedProjectInterface | null>(null)
    const [projectClassificationWarning, setProjectClassificationWarning] = useState<boolean>(false)
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

    function fetchPV() {
        setWarnedProjects(JSON.parse(String(localStorage.getItem("pv"))))
        // NOTE: pv stands for "projects visited". It's been abbreviated to avoid disclosing the classification of the project's ID
    }

    useEffect(() => {
        fetchPV()
        window.addEventListener("storage", () => fetchPV())
    }, [])

    useEffect(() => {
        if (currentUser && (!currentUserId || currentUser.localAccountId !== currentUserId)) {
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
                if (!featuresModalIsOpen) {
                    setProjectClassificationWarning(true)
                }
            }
            if (warnedProjects && warnedProjects[currentUserId].some((vp: string) => vp === project.id)) {
                setProjectClassificationWarning(false)
            }
        }
    }, [project, currentUserId, warnedProjects, featuresModalIsOpen])

    if (isCreating || isLoading) {
        return (
            <ProjectSkeleton />
        )
    }

    return (
        <>
            <Snackbar open={snackBarMessage !== undefined} autoHideDuration={6000} onClose={() => setSnackBarMessage(undefined)}>
                {snackBarMessage}
            </Snackbar>
            <ContentWrapper>
                <Sidebar />
                <MainView className="ag-theme-alpine-fusion ">
                    {project && (
                        <Modal
                            isOpen={projectClassificationWarning}
                            size="sm"
                            title={`Attention - ${PROJECT_CLASSIFICATION[project.classification].label} project`}
                            content={(
                                <Typography key="text">
                                    {PROJECT_CLASSIFICATION[project.classification].description}
                                </Typography>
                            )}
                            actions={
                                <Button key="ok" onClick={() => addVisitedProject()}>OK</Button>
                            }
                        />
                    )}
                    <ControlsWrapper>
                        <Controls />
                    </ControlsWrapper>
                    <Outlet />
                </MainView>
            </ContentWrapper>

        </>
    )
}

export default Overview

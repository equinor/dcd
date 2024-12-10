import { useState, useEffect } from "react"
import { Outlet } from "react-router-dom"
import {
    Button,
    Icon,
    Snackbar,
    Typography,
} from "@equinor/eds-core-react"
import { clear } from "@equinor/eds-icons"
import { useCurrentUser } from "@equinor/fusion-framework-react/hooks"
import styled from "styled-components"
import { useModuleCurrentContext } from "@equinor/fusion-framework-react-module-context"
import { useQuery } from "@tanstack/react-query"
import { GetProjectService } from "../Services/ProjectService"
import { useAppContext } from "@/Context/AppContext"
import { PROJECT_CLASSIFICATION } from "@/Utils/constants"
import { useModalContext } from "@/Context/ModalContext"
import { projectQueryFn } from "@/Services/QueryFunctions"
import { useProjectContext } from "@/Context/ProjectContext"
import ProjectSkeleton from "./LoadingSkeletons/ProjectSkeleton"
import CreateRevisionModal from "./Modal/CreateRevisionModal"
import Sidebar from "./Controls/Sidebar/Sidebar"
import Controls from "./Controls/Controls"
import Modal from "./Modal/Modal"
import { useFeatureContext } from "@/Context/FeatureContext"

const ControlsWrapper = styled.div`
    position: sticky;
    top: 0;
    z-index: 3;
`

const ContentWrapper = styled.div`
    display: flex;
    padding-bottom: 20px;
`

const MainView = styled.div`
    flex: 1;
`

const SnackbarCentering = styled.div`
    display: flex;
    align-items: center;
    justify-content: center;
`

interface WarnedProjectInterface {
    [key: string]: string[]
}

const Overview = () => {
    const currentUser = useCurrentUser()
    const { currentContext } = useModuleCurrentContext()
    const externalId = currentContext?.externalId
    const {
        isCreating,
        isLoading,
        snackBarMessage,
        setSnackBarMessage,
        showRevisionReminder,
        setShowRevisionReminder,
        editMode,
    } = useAppContext()
    const {
        setProjectId,
        isRevision,
        setIsCreateRevisionModalOpen,
    } = useProjectContext()
    const { featuresModalIsOpen } = useModalContext()
    const [warnedProjects, setWarnedProjects] = useState<WarnedProjectInterface | null>(null)
    const [projectClassificationWarning, setProjectClassificationWarning] = useState<boolean>(false)
    const [currentUserId, setCurrentUserId] = useState<string | null>(null)
    const [canCreateRevision, setCanCreateRevision] = useState<boolean>(false)
    const { Features } = useFeatureContext()

    const { data: apiData } = useQuery({
        queryKey: ["projectApiData", externalId],
        queryFn: () => projectQueryFn(externalId),
        enabled: !!externalId,
    })

    async function userCanCreateRevision() {
        if (!externalId) { return }
        const projectService = await GetProjectService()
        const access = await projectService.getAccess(externalId)
        console.log("access", access)
        if (access?.canEdit || access?.isAdmin) {
            setCanCreateRevision(true)
        }
    }

    function handleCreateRevision() {
        setIsCreateRevisionModalOpen(true)
        setShowRevisionReminder(false)
    }

    function checkIfNewRevisionIsRecommended() {
        if (!apiData) { return }

        const lastModified = new Date(apiData.commonProjectAndRevisionData.modifyTime)
        const currentTime = new Date()

        const timeDifferenceInDays = (currentTime.getTime() - lastModified.getTime()) / (1000 * 60 * 60 * 24)
        const hasChangesSinceLastRevision = apiData.revisionDetailsList.some((r) => new Date(r.revisionDate) < lastModified)

        if (timeDifferenceInDays > 30 && hasChangesSinceLastRevision && editMode && !isRevision) {
            setShowRevisionReminder(true)
        }
    }

    function addVisitedProject() {
        if (apiData && currentUserId) {
            if (warnedProjects && warnedProjects[currentUserId]) {
                const wp = { ...warnedProjects }
                wp[currentUserId].push(apiData.projectId)
                setWarnedProjects(wp)
            } else {
                setWarnedProjects({ [currentUserId]: [apiData.projectId] })
            }
        }
    }

    function fetchPV() {
        setWarnedProjects(JSON.parse(String(localStorage.getItem("pv"))))
        // NOTE: pv stands for "projects visited". It's been abbreviated to avoid disclosing the classification of the project's ID
    }

    useEffect(() => {
        userCanCreateRevision()
    }, [externalId])

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
        if (apiData) {
            setProjectId(apiData.projectId)
        }
    }, [apiData])

    useEffect(() => {
        if (apiData) {
            checkIfNewRevisionIsRecommended()
        }
    }, [apiData, editMode])

    useEffect(() => {
        if (apiData && currentUserId) {
            if (
                !projectClassificationWarning
                && PROJECT_CLASSIFICATION[apiData.commonProjectAndRevisionData.classification].warn
                && (
                    (warnedProjects && !warnedProjects[currentUserId].some((vp: string) => vp === apiData.projectId))
                    || (warnedProjects && !warnedProjects[currentUserId])
                    || !warnedProjects
                )
            ) {
                if (!featuresModalIsOpen) {
                    setProjectClassificationWarning(true)
                }
            }
            if (warnedProjects && warnedProjects[currentUserId].some((vp: string) => vp === apiData.projectId)) {
                setProjectClassificationWarning(false)
            }
        }
    }, [apiData, currentUserId, warnedProjects, featuresModalIsOpen])

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
            {Features?.revisionEnabled
                && (
                    <>
                        <Snackbar open={showRevisionReminder} placement="bottom-right" autoHideDuration={300000000} onClose={() => setShowRevisionReminder(false)}>
                            <SnackbarCentering>
                                <Button variant="ghost_icon" onClick={() => setShowRevisionReminder(false)}>
                                    <Icon data={clear} />
                                </Button>
                                <Typography variant="body_short" color="#FFF" style={{ marginLeft: "10px" }}>
                                    Remember to create a new revision after completing a project phase!
                                </Typography>
                                <Snackbar.Action>
                                    <Button
                                        variant="ghost"
                                        onClick={() => handleCreateRevision()}
                                        disabled={!canCreateRevision}
                                    >
                                        Create revision
                                    </Button>
                                </Snackbar.Action>
                            </SnackbarCentering>
                        </Snackbar>
                        <CreateRevisionModal />
                    </>
                )}
            <ContentWrapper>
                <Sidebar />
                <MainView className="ag-theme-alpine-fusion ">
                    {apiData && (
                        <Modal
                            isOpen={projectClassificationWarning}
                            size="sm"
                            title={`Attention - ${PROJECT_CLASSIFICATION[apiData.commonProjectAndRevisionData.classification].label} project`}
                            content={(
                                <Typography key="text">
                                    {PROJECT_CLASSIFICATION[apiData.commonProjectAndRevisionData.classification].description}
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

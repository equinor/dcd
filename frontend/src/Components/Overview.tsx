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
import { useQuery } from "@tanstack/react-query"
import { useModuleCurrentContext } from "@equinor/fusion-framework-react-module-context"

import { useFeatureContext } from "@/Context/FeatureContext"
import { useProjectContext } from "@/Context/ProjectContext"
import { useModalContext } from "@/Context/ModalContext"
import { useAppContext } from "@/Context/AppContext"
import { GetProjectService } from "@/Services/ProjectService"
import { peopleQueryFn } from "@/Services/QueryFunctions"
import { PROJECT_CLASSIFICATION } from "@/Utils/constants"
import NoAccessErrorView from "@/Views/NoAccessErrorView"
import { useDataFetch } from "@/Hooks/useDataFetch"
import ProjectSkeleton from "./LoadingSkeletons/ProjectSkeleton"
import CreateRevisionModal from "./Modal/CreateRevisionModal"
import Sidebar from "./Sidebar/Sidebar"
import Controls from "./Controls/Controls"
import Modal from "./Modal/Modal"

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
        projectId,
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
    const revisionAndProjectData = useDataFetch()

    const { data: peopleApiData } = useQuery({
        queryKey: ["peopleApiData", projectId],
        queryFn: () => peopleQueryFn(projectId),
        enabled: !!projectId,
    })

    async function userCanCreateRevision() {
        if (!externalId) { return }
        const projectService = await GetProjectService()
        const access = await projectService.getAccess(externalId)
        if (access?.canEdit || access?.isAdmin) {
            setCanCreateRevision(true)
        }
    }

    function handleCreateRevision() {
        setIsCreateRevisionModalOpen(true)
        setShowRevisionReminder(false)
    }

    const projectData = revisionAndProjectData?.dataType === "project"
        ? (revisionAndProjectData as Components.Schemas.ProjectDataDto)
        : null

    function checkIfNewRevisionIsRecommended() {
        if (!projectData) { return }

        const lastModified = new Date(projectData.commonProjectAndRevisionData.modifyTime)
        const currentTime = new Date()

        const timeDifferenceInDays = (currentTime.getTime() - lastModified.getTime()) / (1000 * 60 * 60 * 24)
        const hasChangesSinceLastRevision = projectData.revisionDetailsList.some((r) => new Date(r.revisionDate) < lastModified)

        if (timeDifferenceInDays > 30 && hasChangesSinceLastRevision && editMode && !isRevision) {
            setShowRevisionReminder(true)
        }
    }

    function addVisitedProject() {
        if (revisionAndProjectData && currentUserId) {
            if (warnedProjects && warnedProjects[currentUserId]) {
                const wp = { ...warnedProjects }
                wp[currentUserId].push(revisionAndProjectData.projectId)
                setWarnedProjects(wp)
            } else {
                setWarnedProjects({ [currentUserId]: [revisionAndProjectData.projectId] })
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
        if (revisionAndProjectData) {
            setProjectId(revisionAndProjectData.projectId)
        }
    }, [revisionAndProjectData])

    useEffect(() => {
        if (revisionAndProjectData) {
            checkIfNewRevisionIsRecommended()
        }
    }, [revisionAndProjectData, editMode])

    useEffect(() => {
        if (revisionAndProjectData && currentUserId) {
            if (
                !projectClassificationWarning
                && PROJECT_CLASSIFICATION[revisionAndProjectData.commonProjectAndRevisionData.classification].warn
                && (
                    (warnedProjects && !warnedProjects[currentUserId].some((vp: string) => vp === revisionAndProjectData.projectId))
                    || (warnedProjects && !warnedProjects[currentUserId])
                    || !warnedProjects
                )
            ) {
                if (!featuresModalIsOpen) {
                    setProjectClassificationWarning(true)
                }
            }
            if (warnedProjects && warnedProjects[currentUserId].some((vp: string) => vp === revisionAndProjectData.projectId)) {
                setProjectClassificationWarning(false)
            }
        }
    }, [revisionAndProjectData, currentUserId, warnedProjects, featuresModalIsOpen])

    if (isCreating || isLoading || !revisionAndProjectData || !peopleApiData || !currentUser) {
        return (
            <ProjectSkeleton />
        )
    }

    if (!revisionAndProjectData.hasAccess) {
        return (
            <NoAccessErrorView
                projectClassification={revisionAndProjectData.commonProjectAndRevisionData.classification}
            />
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
                    {revisionAndProjectData && (
                        <Modal
                            isOpen={projectClassificationWarning}
                            size="sm"
                            title={`Attention - ${PROJECT_CLASSIFICATION[revisionAndProjectData.commonProjectAndRevisionData.classification].label} project`}
                            content={(
                                <Typography key="text">
                                    {PROJECT_CLASSIFICATION[revisionAndProjectData.commonProjectAndRevisionData.classification].description}
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

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

import { useFeatureContext } from "@/Store/FeatureContext"
import { useProjectContext } from "@/Store/ProjectContext"
import { useModalContext } from "@/Store/ModalContext"
import { useAppStore } from "@/Store/AppStore"
import { peopleQueryFn } from "@/Services/QueryFunctions"
import { PROJECT_CLASSIFICATION } from "@/Utils/constants"
import { useDataFetch, useLocalStorage } from "@/Hooks"
import ProjectSkeleton from "./LoadingSkeletons/ProjectSkeleton"
import CreateRevisionModal from "./Modal/CreateRevisionModal"
import SidebarWrapper from "./Sidebar/SidebarWrapper"
import Controls from "./Controls/Controls"
import Modal from "./Modal/Modal"
import { dateStringToDateUtc } from "@/Utils/DateUtils"
import useCanUserEdit from "@/Hooks/useCanUserEdit"

const ControlsWrapper = styled.div`
    position: sticky;
    top: 0;
    z-index: 6;
`

const ContentWrapper = styled.div`
    display: flex;
    width: 100%;
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
    const {
        isCreating,
        isLoading,
        snackBarMessage,
        setSnackBarMessage,
        showRevisionReminder,
        setShowRevisionReminder,
        editMode,
    } = useAppStore()
    const {
        projectId,
        setProjectId,
        isRevision,
        setIsCreateRevisionModalOpen,
    } = useProjectContext()

    const { featuresModalIsOpen } = useModalContext()
    const [projectClassificationWarning, setProjectClassificationWarning] = useState<boolean>(false)
    const [currentUserId, setCurrentUserId] = useState<string | null>(null)
    const { Features } = useFeatureContext()
    const revisionAndProjectData = useDataFetch()
    const [warnedProjects, setWarnedProjects] = useLocalStorage<WarnedProjectInterface | null>("pv", null)
    const { canEdit, isEditDisabled } = useCanUserEdit()

    const { data: peopleApiData } = useQuery({
        queryKey: ["peopleApiData", projectId],
        queryFn: () => peopleQueryFn(projectId),
        enabled: !!projectId,
    })

    function handleCreateRevision() {
        setIsCreateRevisionModalOpen(true)
        setShowRevisionReminder(false)
    }

    const projectData = revisionAndProjectData?.dataType === "project"
        ? (revisionAndProjectData as Components.Schemas.ProjectDataDto)
        : null

    function checkIfNewRevisionIsRecommended() {
        if (!projectData) { return }

        const updatedUtc = dateStringToDateUtc(projectData.commonProjectAndRevisionData.updatedUtc)
        const currentTime = new Date()

        const timeDifferenceInDays = (currentTime.getTime() - updatedUtc.getTime()) / (1000 * 60 * 60 * 24)
        const hasChangesSinceLastRevision = projectData.revisionDetailsList.some((r) => dateStringToDateUtc(r.revisionDate) < updatedUtc)

        if (timeDifferenceInDays > 30 && hasChangesSinceLastRevision && canEdit() && !isRevision) {
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

    useEffect(() => {
        if (currentUser && (!currentUserId || currentUser.localAccountId !== currentUserId)) {
            setCurrentUserId(currentUser.localAccountId as keyof typeof warnedProjects)
        }
    }, [currentUser])

    useEffect(() => {
        if (revisionAndProjectData) {
            setProjectId(revisionAndProjectData.projectId)
        }
    }, [revisionAndProjectData])

    useEffect(() => {
        if (revisionAndProjectData) {
            checkIfNewRevisionIsRecommended()
        }
    }, [revisionAndProjectData, editMode, isEditDisabled])

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
                                        disabled={!revisionAndProjectData?.userActions.canCreateRevision}
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
                <SidebarWrapper />
                <MainView className="ag-theme-alpine-fusion container-padding">
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

import {
    Banner, Icon, Typography, Button,
} from "@equinor/eds-core-react"
import { info_circle } from "@equinor/eds-icons"
import { useModuleCurrentContext } from "@equinor/fusion-framework-react-module-context"
import { useQuery } from "@tanstack/react-query"
import { useEffect, useRef, useState } from "react"
import { Outlet, useLocation } from "react-router-dom"
import styled from "styled-components"

import { GetProjectService } from "../../Services/ProjectService"
import { useAppStore } from "../../Store/AppStore"
import BaseControls from "../Controls/BaseControls"
import ProjectSkeleton from "../LoadingSkeletons/ProjectSkeleton"
import BaseModal from "../Modal/BaseModal"
import CreateCaseModal from "../Modal/CreateCaseModal"
import SidebarWrapper from "../Sidebar/SidebarWrapper"
import SnackbarComponent from "../Snackbar/Snackbar"

import { useDataFetch, useLocalStorage } from "@/Hooks"
import useCanUserEdit from "@/Hooks/useCanUserEdit"
import { useAppNavigation } from "@/Hooks/useNavigate"
import { NoAccessReason } from "@/Models/enums"
import { peopleQueryFn } from "@/Services/QueryFunctions"
import { useProjectContext } from "@/Store/ProjectContext"
import { PROJECT_CLASSIFICATION } from "@/Utils/Config/constants"
import { dateStringToDateUtc } from "@/Utils/DateUtils"
import IndexView from "@/Views/IndexView"
import NoAccessErrorView from "@/Views/NoAccessErrorView"

const ControlsWrapper = styled.div`
    position: sticky;
    top: 0;
    z-index: 4;
`

const ContentWrapper = styled.div`
    display: flex;
    width: 100%;
`

const MainView = styled.div`
    flex: 1;
`

interface WarnedProjectInterface {
    [key: string]: string[]
}

const NoAccessBanner = () => (
    <Banner>
        <Banner.Icon variant="info">
            <Icon data={info_circle} />
        </Banner.Icon>
        <Banner.Message>You do not have access to view this project</Banner.Message>
    </Banner>
)

const ProjectLayout = (): JSX.Element => {
    const [projectExists, setProjectExists] = useState<boolean>(false)
    const [noAccessReason, setNoAccessReason] = useState<NoAccessReason | null>(null)
    const [projectClassificationWarning, setProjectClassificationWarning] = useState<boolean>(false)
    const { setIsRevision, setProjectId } = useProjectContext()
    const {
        setIsCreating,
        setIsLoading,
        setSnackBarMessage,
        isLoading,
        setShowRevisionReminder,
        editMode,
    } = useAppStore()
    const { currentContext } = useModuleCurrentContext()
    const { navigateToProject } = useAppNavigation()
    const location = useLocation()
    const previousContextRef = useRef(currentContext?.id)
    const revisionAndProjectData = useDataFetch()
    const [warnedProjects, setWarnedProjects] = useLocalStorage<WarnedProjectInterface | null>("pv", null)
    const { canEdit, isEditDisabled } = useCanUserEdit()

    const { data: peopleApiData } = useQuery({
        queryKey: ["peopleApiData", currentContext?.externalId],
        queryFn: () => peopleQueryFn(currentContext?.externalId),
        enabled: !!currentContext?.externalId,
    })

    // Update revision state based on URL changes
    useEffect(() => {
        const isInRevisionPath = location.pathname.includes("/revision/")

        setIsRevision(isInRevisionPath)
    }, [location.pathname])

    // Handle project context changes
    useEffect(() => {
        if (!currentContext?.externalId) { return }

        const isProjectChanged = previousContextRef.current !== currentContext.id

        previousContextRef.current = currentContext.id

        if (isProjectChanged) {
            setProjectExists(false)
            setIsRevision(false)
            navigateToProject(currentContext.id)
        }
    }, [currentContext])

    // Project initialization
    useEffect(() => {
        const initializeProject = async () => {
            if (!currentContext?.externalId) { return }

            setIsLoading(true)

            try {
                const projectService = GetProjectService()
                const results = await projectService.projectExists(currentContext.externalId)

                setProjectExists(results.projectExists)
                setNoAccessReason(results.noAccessReason)

                if (results.projectExists) {
                    setIsLoading(false)

                    return
                }

                if (!results.projectExists && results.canCreateProject) {
                    setSnackBarMessage("No project found for this search. Creating new.")
                    setIsCreating(true)

                    await projectService.createProject(currentContext.externalId)
                    setProjectExists(true)
                    setIsCreating(false)
                    setIsLoading(false)

                    return
                }

                if (!results.projectExists) {
                    setSnackBarMessage("You do not have access to view this project")
                    setIsLoading(false)
                }
            } catch (error) {
                setSnackBarMessage("An error occurred while fetching project access. Please try again later.")
                setIsLoading(false)
                setIsCreating(false)
            }
        }

        initializeProject()
    }, [currentContext?.externalId])

    // Check for revision recommendations
    useEffect(() => {
        if (!revisionAndProjectData) { return }

        const updatedUtc = dateStringToDateUtc(revisionAndProjectData.commonProjectAndRevisionData.updatedUtc)
        const currentTime = new Date()

        const timeDifferenceInDays = (currentTime.getTime() - updatedUtc.getTime()) / (1000 * 60 * 60 * 24)
        const hasChangesSinceLastRevision = revisionAndProjectData.revisionDetailsList.some((r) => dateStringToDateUtc(r.revisionDate) < updatedUtc)

        if (timeDifferenceInDays > 30 && hasChangesSinceLastRevision && canEdit() && !location.pathname.includes("/revision/")) {
            setShowRevisionReminder(true)
        }
    }, [revisionAndProjectData, editMode, isEditDisabled])

    // Project classification warnings
    useEffect(() => {
        if (!revisionAndProjectData || !currentContext?.externalId) { return }

        if (
            !projectClassificationWarning
            && PROJECT_CLASSIFICATION[revisionAndProjectData.commonProjectAndRevisionData.classification].warn
            && (
                (warnedProjects && !warnedProjects[currentContext.externalId]?.some((vp: string) => vp === revisionAndProjectData.projectId))
                || (warnedProjects && !warnedProjects[currentContext.externalId])
                || !warnedProjects
            )
        ) {
            setProjectClassificationWarning(true)
        }
    }, [revisionAndProjectData, currentContext?.externalId, warnedProjects])

    // Set projectId when project data is loaded
    useEffect(() => {
        if (revisionAndProjectData?.projectId) {
            setProjectId(revisionAndProjectData.projectId)
        }
    }, [revisionAndProjectData?.projectId])

    function addVisitedProject() {
        if (revisionAndProjectData && currentContext?.externalId) {
            if (warnedProjects && warnedProjects[currentContext.externalId]) {
                const wp = { ...warnedProjects }

                wp[currentContext.externalId].push(revisionAndProjectData.projectId)
                setWarnedProjects(wp)
            } else {
                setWarnedProjects({ [currentContext.externalId]: [revisionAndProjectData.projectId] })
            }
        }
    }

    if (!currentContext) {
        return <IndexView />
    }

    if (isLoading || !revisionAndProjectData || !peopleApiData) {
        return <ProjectSkeleton />
    }

    // Show no access views if needed
    if (noAccessReason && noAccessReason !== NoAccessReason.ProjectDoesNotExist) {
        return <NoAccessErrorView projectClassification={noAccessReason} />
    }

    if (!projectExists) {
        return <NoAccessBanner />
    }

    return (
        <>
            <SnackbarComponent />
            <CreateCaseModal />
            <ContentWrapper>
                <SidebarWrapper />
                <MainView className="ag-theme-alpine-fusion container-padding">
                    {revisionAndProjectData && (
                        <BaseModal
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
                        <BaseControls />
                    </ControlsWrapper>
                    <Outlet />
                </MainView>
            </ContentWrapper>
        </>
    )
}

export default ProjectLayout

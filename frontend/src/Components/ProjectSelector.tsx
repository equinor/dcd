import { useEffect, useRef, useState } from "react"
import { useModuleCurrentContext } from "@equinor/fusion-framework-react-module-context"
import { Banner, Icon } from "@equinor/eds-core-react"
import { info_circle } from "@equinor/eds-icons"
import { Outlet, useLocation } from "react-router-dom"
import { GetProjectService } from "../Services/ProjectService"
import { useAppStore } from "../Store/AppStore"
import { useProjectContext } from "@/Store/ProjectContext"
import { useAppNavigation } from "@/Hooks/useNavigate"
import IndexView from "@/Views/IndexView"
import CreateCaseModal from "./Modal/CreateCaseModal"
import NoAccessErrorView from "@/Views/NoAccessErrorView"
import ProjectSkeleton from "@/Components/LoadingSkeletons/ProjectSkeleton"
import { NoAccessReason } from "@/Models/enums"

// Banner components
const SelectProjectBanner = () => (
    <IndexView />
)

const NoAccessBanner = () => (
    <Banner>
        <Banner.Icon variant="info">
            <Icon data={info_circle} />
        </Banner.Icon>
        <Banner.Message>You do not have access to view this project</Banner.Message>
    </Banner>
)

const ProjectSelector = (): JSX.Element => {
    const [projectExists, setProjectExists] = useState<boolean>(false)
    const [noAccessReason, setNoAccessReason] = useState<NoAccessReason | null>(null)
    const { setIsRevision } = useProjectContext()
    const {
        setIsCreating,
        setIsLoading,
        setSnackBarMessage,
        isLoading,
    } = useAppStore()
    const { currentContext } = useModuleCurrentContext()
    const { navigateToProject } = useAppNavigation()
    const location = useLocation()
    const previousContextRef = useRef(currentContext?.id)

    // Update revision state based on URL changes (including browser navigation)
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

    useEffect(() => {
        const initializeProject = async () => {
            if (!currentContext?.externalId) { return }

            setIsLoading(true)

            try {
                const projectService = await GetProjectService()
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

    if (!currentContext) {
        return <SelectProjectBanner />
    }

    if (isLoading) {
        return <ProjectSkeleton />
    }

    if (noAccessReason && noAccessReason !== NoAccessReason.ProjectDoesNotExist) {
        return <NoAccessErrorView projectClassification={noAccessReason} />
    }

    if (projectExists) {
        return (
            <>
                <CreateCaseModal />
                <Outlet />
            </>
        )
    }

    return <NoAccessBanner />
}

export default ProjectSelector

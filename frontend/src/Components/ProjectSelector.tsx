import { useEffect, useRef, useState } from "react"
import { useModuleCurrentContext } from "@equinor/fusion-framework-react-module-context"
import { Banner, Icon } from "@equinor/eds-core-react"
import { info_circle } from "@equinor/eds-icons"
import { Outlet, useLocation } from "react-router-dom"
import { __ProjectService, GetProjectService } from "../Services/ProjectService"
import { useAppContext } from "../Context/AppContext"
import { useProjectContext } from "@/Context/ProjectContext"
import { useAppNavigation } from "@/Hooks/useNavigate"
import CreateCaseModal from "./Modal/CreateCaseModal"

// Banner components
const SelectProjectBanner = () => (
    <Banner>
        <Banner.Icon variant="info">
            <Icon data={info_circle} />
        </Banner.Icon>
        <Banner.Message>Select a project to view</Banner.Message>
    </Banner>
)

const LoadingBanner = () => (
    <Banner>
        <Banner.Message>Loading project...</Banner.Message>
    </Banner>
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

    const { setIsRevision } = useProjectContext()
    const {
        setIsCreating,
        setIsLoading,
        setSnackBarMessage,
        isLoading,
    } = useAppContext()
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
                const projectExists = await projectService.projectExists(currentContext.externalId);

                setProjectExists(projectExists.projectExists)

                if (projectExists.projectExists) {
                    setIsLoading(false)
                    return
                }

                if (!projectExists.projectExists && projectExists.canCreateProject) {
                    setSnackBarMessage("No project found for this search. Creating new.")
                    setIsCreating(true)
                    const projectService = await GetProjectService()
                    await projectService.createProject(currentContext.externalId)
                    setProjectExists(true)
                    setIsCreating(false)
                    setIsLoading(false)
                    return
                }

                if (!projectExists.projectExists) {
                    setSnackBarMessage("You do not have access to view this project")
                    setIsLoading(false)
                    return
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
        return <LoadingBanner />
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

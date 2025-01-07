import { useEffect, useRef } from "react"
import { useModuleCurrentContext } from "@equinor/fusion-framework-react-module-context"
import { Banner, Icon } from "@equinor/eds-core-react"
import { info_circle } from "@equinor/eds-icons"
import { Outlet, useLocation } from "react-router-dom"
import { GetProjectService } from "../Services/ProjectService"
import CreateCaseModal from "./Modal/CreateCaseModal"
import { useAppContext } from "../Context/AppContext"
import { useProjectContext } from "@/Context/ProjectContext"
import { isAxiosError } from "@/Utils/common"
import { useAppNavigation } from "@/Hooks/useNavigate"

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
    const {
        setIsRevision,
        setAccessRights,
        accessRights,
    } = useProjectContext()
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

    const handleError = (message: string, error: unknown) => {
        console.error(message, error)
        setSnackBarMessage(message)
        setIsLoading(false)
    }

    const createNewProject = async (projectService: any, externalId: string) => {
        setIsCreating(true)
        setSnackBarMessage("No project found for this search. Creating new.")
        const createdProject = await projectService.createProject(externalId)
        return projectService.getAccess(createdProject.commonProjectAndRevisionData.fusionProjectId)
    }

    const getProjectAccess = async (projectService: any, externalId: string) => {
        try {
            return await projectService.getAccess(externalId)
        } catch (error) {
            if (isAxiosError(error) && error.response?.status === 404) {
                return createNewProject(projectService, externalId)
            }
            console.error("Error fetching project access:", error)
            setSnackBarMessage("An error occurred while fetching project access. Please try again later.")
            setIsLoading(false)
            return null
        }
    }

    const fetchProjectData = async (projectService: any, externalId: string) => {
        const fetchedProject = await projectService.getProject(externalId)
        if (fetchedProject) {
            setIsCreating(false)
            setIsLoading(false)
        }
    }

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
            setIsRevision(false)
            navigateToProject(currentContext.id)
        }
    }, [currentContext])

    // Initialize or update project data
    useEffect(() => {
        const initializeProject = async () => {
            if (!currentContext?.externalId) { return }

            setIsLoading(true)
            const projectService = await GetProjectService()

            try {
                const access = await getProjectAccess(projectService, currentContext.externalId)
                setAccessRights(access)

                if (!access.canView) {
                    setSnackBarMessage("You do not have access to view this project")
                    setIsLoading(false)
                    return
                }

                await fetchProjectData(projectService, currentContext.externalId)
            } catch (error) {
                handleError("Error fetching or setting project in context", error)
            }
        }

        initializeProject()
    }, [currentContext?.externalId])

    if (!currentContext) {
        return <SelectProjectBanner />
    }

    if (accessRights?.canView) {
        return (
            <>
                <CreateCaseModal />
                <Outlet />
            </>
        )
    }

    if (isLoading) {
        return <LoadingBanner />
    }

    return <NoAccessBanner />
}

export default ProjectSelector

import { useEffect } from "react"
import { useModuleCurrentContext } from "@equinor/fusion-framework-react-module-context"
import { Banner, Icon } from "@equinor/eds-core-react"
import { info_circle } from "@equinor/eds-icons"
import { Outlet, useLocation, useNavigate } from "react-router-dom"
import { GetProjectService } from "../Services/ProjectService"
import CreateCaseModal from "./Modal/CreateCaseModal"
import { useAppContext } from "../Context/AppContext"
import { useProjectContext } from "@/Context/ProjectContext"
import { isAxiosError } from "@/Utils/common"
import IndexView from "@/Views/IndexView"

const RouteCoordinator = (): JSX.Element => {
    const { setIsRevision, setAccessRights, accessRights } = useProjectContext()
    const {
        setIsCreating, setIsLoading, setSnackBarMessage, isLoading,
    } = useAppContext()
    const { currentContext } = useModuleCurrentContext()

    const navigate = useNavigate()
    const location = useLocation()

    useEffect(() => {
        const getPathToNavigate = () => {
            if (!currentContext?.externalId) {
                return "/"
            }
            if (location.pathname.includes("/revision")) {
                setIsRevision(true)
                return location.pathname
            }
            return location.pathname.includes("/case") ? location.pathname : currentContext.id
        }

        const pathToNavigate = getPathToNavigate()
        if (location.pathname !== pathToNavigate) {
            navigate(pathToNavigate)
        }
    }, [currentContext, location.pathname, navigate])

    const handleError = (message: string, error: unknown) => {
        console.error(message, error)
        setSnackBarMessage(message)
        setIsLoading(false)
    }

    useEffect(() => {
        const fetchAndSetProject = async () => {
            if (!currentContext?.externalId) {
                console.log("No externalId in context")
                return
            }

            setIsLoading(true)
            const projectService = await GetProjectService()

            try {
                let access
                try {
                    access = await projectService.getAccess(currentContext.externalId)
                } catch (error) {
                    if (isAxiosError(error) && error.response?.status === 404) {
                        // Project not found, attempt to create it
                        setIsCreating(true)
                        setSnackBarMessage("No project found for this search. Creating new.")
                        const createdProject = await projectService.createProject(currentContext.id)
                        access = await projectService.getAccess(createdProject.commonProjectAndRevisionData.fusionProjectId)
                    } else {
                        handleError("Error fetching access rights", error)
                        return
                    }
                }
                setAccessRights(access)

                if (!access.canView) {
                    setSnackBarMessage("You do not have access to view this project")
                    setIsLoading(false)
                    return
                }

                try {
                    const fetchedProject = await projectService.getProject(currentContext.externalId)
                    if (fetchedProject) {
                        setIsCreating(false)
                        setIsLoading(false)
                    }
                } catch (error) {
                    handleError("Error fetching project", error)
                }
            } catch (error) {
                handleError("Error fetching or setting project in context", error)
            }
        }

        fetchAndSetProject()
    }, [currentContext?.externalId])

    if (!currentContext) {
        return <IndexView />
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
        return (
            <Banner>
                <Banner.Message>
                    Loading project...
                </Banner.Message>
            </Banner>
        )
    }
    return (
        <Banner>
            <Banner.Icon variant="info">
                <Icon data={info_circle} />
            </Banner.Icon>
            <Banner.Message>
                You do not have access to view this project
            </Banner.Message>
        </Banner>
    )
}

export default RouteCoordinator

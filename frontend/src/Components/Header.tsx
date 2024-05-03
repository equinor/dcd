import { useEffect } from "react"
import { useModuleCurrentContext } from "@equinor/fusion-framework-react-module-context"
import { Banner, Icon } from "@equinor/eds-core-react"
import { info_circle } from "@equinor/eds-icons"
import { Outlet, useLocation, useNavigate } from "react-router-dom"
import { useProjectContext } from "../Context/ProjectContext"
import { GetProjectService } from "../Services/ProjectService"
import CreateCaseModal from "./CreateCaseModal"
import EditTechnicalInputModal from "./EditTechnicalInput/EditTechnicalInputModal"
import { useAppContext } from "../Context/AppContext"
import EditsSideBar from "./EditTracker/EditsSideBar"

const RouteCoordinator = (): JSX.Element => {
    const { setIsCreating, setIsLoading } = useAppContext()
    const { setProject } = useProjectContext()
    const { currentContext } = useModuleCurrentContext()

    const navigate = useNavigate()
    const location = useLocation()

    useEffect(() => {
        const getPathToNavigate = () => {
            if (!currentContext?.externalId) {
                return "/"
            }

            return location.pathname.includes("/case") ? location.pathname : currentContext.id
        }

        navigate(getPathToNavigate())
    }, [currentContext])

    useEffect(() => {
        const fetchAndSetProject = async () => {
            if (!currentContext?.externalId) {
                console.log("No externalId in context")
                setProject(undefined)
                return
            }

            try {
                setIsLoading(true)
                const projectService = await GetProjectService()
                let fetchedProject = await projectService.getProject(currentContext.externalId)

                if (!fetchedProject || fetchedProject.id === "") {
                    setIsCreating(true)
                    fetchedProject = await projectService.createProject(currentContext.id)
                }

                if (fetchedProject) {
                    setProject(fetchedProject)
                    setIsCreating(false)
                    setIsLoading(false)
                }
            } catch (error) {
                console.error("Error fetching or setting project in context:", error)
            }
        }

        fetchAndSetProject()
    }, [currentContext, setProject])

    if (!currentContext) {
        return (
            <Banner>
                <Banner.Icon variant="info">
                    <Icon data={info_circle} />
                </Banner.Icon>
                <Banner.Message>
                    Select a project to view
                </Banner.Message>
            </Banner>
        )
    } return (
        <>
            <CreateCaseModal />
            <EditTechnicalInputModal />
            <EditsSideBar />
            <Outlet />
        </>
    )
}

export default RouteCoordinator

import { useEffect } from "react"
import { useModuleCurrentContext } from "@equinor/fusion-framework-react-module-context"
import { Banner, Icon } from "@equinor/eds-core-react"
import { info_circle } from "@equinor/eds-icons"
import { Outlet, useLocation, useNavigate } from "react-router-dom"
import { GetProjectService } from "../Services/ProjectService"
import CreateCaseModal from "./Modal/CreateCaseModal"
import { useAppContext } from "../Context/AppContext"
import useEditProject from "../Hooks/useEditProject"
import { useProjectContext } from "@/Context/ProjectContext"

const RouteCoordinator = (): JSX.Element => {
    const { setIsRevision } = useProjectContext()
    const { setIsCreating, setIsLoading, setSnackBarMessage } = useAppContext()
    const { currentContext } = useModuleCurrentContext()
    const { addProjectEdit } = useEditProject()

    const navigate = useNavigate()
    const location = useLocation()

    useEffect(() => {
        const getPathToNavigate = () => {
            if (!currentContext?.externalId) {
                return "/"
            }
            else if (location.pathname.includes("/revision")) {
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

    useEffect(() => {
        const fetchAndSetProject = async () => {
            if (!currentContext?.externalId) {
                console.log("No externalId in context")
                return
            }

            try {
                setIsLoading(true)
                const projectService = await GetProjectService()
                let fetchedProject
                try {
                    fetchedProject = await projectService.getProject(currentContext.externalId)
                } catch (error) {
                    if (!fetchedProject || fetchedProject.id === "") {
                        setIsCreating(true)
                        setSnackBarMessage("No project found for this search. Creating new.")
                        fetchedProject = await projectService.createProject(currentContext.id)
                    }
                }

                if (fetchedProject) {
                    addProjectEdit(fetchedProject.id, fetchedProject)
                    setIsCreating(false)
                    setIsLoading(false)
                }
            } catch (error) {
                console.error("Error fetching or setting project in context:", error)
                setSnackBarMessage("Error fetching or setting project in context")
            }
        }

        fetchAndSetProject()
    }, [currentContext?.externalId])

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
            <Outlet />
        </>
    )
}

export default RouteCoordinator

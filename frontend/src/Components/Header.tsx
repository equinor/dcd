import { useEffect, useState } from "react"
import { useModuleCurrentContext } from "@equinor/fusion-framework-react-module-context"
import {
    Progress, Banner, Icon, Typography,
} from "@equinor/eds-core-react"
import { info_circle } from "@equinor/eds-icons"
import { Outlet, useNavigate } from "react-router-dom"
import { useProjectContext } from "../Context/ProjectContext"
import { GetProjectService } from "../Services/ProjectService"
import CreateCaseModal from "./CreateCaseModal"
import EditTechnicalInputModal from "./EditTechnicalInput/EditTechnicalInputModal"
import Grid from "@mui/material/Grid"
import { useAppContext } from "../Context/AppContext"

const RouteCoordinator = (): JSX.Element => {
    const { isCreating, setIsCreating, isLoading, setIsLoading } = useAppContext()
    const { project, setProject } = useProjectContext()
    const { currentContext } = useModuleCurrentContext()

    const navigate = useNavigate()

    useEffect(() => {
        if (currentContext?.externalId) {
            navigate(currentContext.id)
        } else {
            navigate("/")
        }
    }, [currentContext])

    useEffect(() => {
        console.log("getting project", currentContext)
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
                console.log("fetchedProject", fetchedProject)

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
            <Outlet />
        </>
    )
}

export default RouteCoordinator

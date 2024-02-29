import { useEffect, useState } from "react"
import { useModuleCurrentContext } from "@equinor/fusion-framework-react-module-context"
import {
    Progress, Banner, Icon, Typography,
} from "@equinor/eds-core-react"
import { info_circle } from "@equinor/eds-icons"
import styled from "styled-components"
import { Outlet, useNavigate } from "react-router-dom"
import { useAppContext } from "../Context/AppContext"
import { GetProjectService } from "../Services/ProjectService"
import CreateCaseModal from "./CreateCaseModal"
import EditTechnicalInputModal from "./EditTechnicalInput/EditTechnicalInputModal"

const Wrapper = styled.div`
    display: flex;
    flex-direction: column;
    gap: 16px;
    align-items: center;
    justify-content: center;
    height: 100%;
    width: 100%;
`

const RouteCoordinator = (): JSX.Element => {
    const { project, setProject } = useAppContext()
    const { currentContext } = useModuleCurrentContext()
    const [isLoading, setIsLoading] = useState<boolean>(false)
    const [isCreating, setIsCreating] = useState<boolean>(false)

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

    if ((isLoading || !project || project.id === "") && currentContext) {
        if (isCreating) {
            return (
                <Wrapper>
                    <Progress.Circular size={24} color="primary" />
                    <Typography variant="h4">Creating project</Typography>
                </Wrapper>
            )
        }
        return (
            <Wrapper>
                <Progress.Circular size={24} color="primary" />
                <Typography variant="h4">Loading project</Typography>
            </Wrapper>
        )
    }
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

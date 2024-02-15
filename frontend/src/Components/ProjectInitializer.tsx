import { FC, useEffect, useState } from "react"
import { useCurrentContext } from "@equinor/fusion"
import {
    Progress, Banner, Icon, Typography,
} from "@equinor/eds-core-react"
import { info_circle } from "@equinor/eds-icons"
import styled from "styled-components"
import { useAppContext } from "../context/AppContext"
import { GetProjectService } from "../Services/ProjectService"

const Wrapper = styled.div`
    display: flex;
    flex-direction: column;
    gap: 16px;
    align-items: center;
    justify-content: center;
    height: 100%;
    width: 100%;
`

const ProjectInitializer: FC = () => {
    const { project, setProject } = useAppContext()
    const currentProject = useCurrentContext()

    const [isLoading, setIsLoading] = useState<boolean>()
    const [isCreating, setIsCreating] = useState<boolean>()

    useEffect(() => {
        console.log("current project", currentProject)
        const fetchAndSetProject = async () => {
            if (!currentProject?.externalId) {
                return
            }

            try {
                setIsLoading(true)
                const projectService = await GetProjectService()
                let fetchedProject = await projectService.getProjectByID(currentProject.externalId)

                if (!fetchedProject || fetchedProject.id === "") {
                    setIsCreating(true)
                    fetchedProject = await projectService.createProjectFromContextId(currentProject.id)
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
    }, [currentProject, setProject]) // Re-run effect if currentProject changes

    if (!currentProject) {
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
    }

    if (isLoading || !project || project.id === "") {
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
    return null
}

export default ProjectInitializer

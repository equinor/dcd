import { FC, useEffect, useState } from "react"
import { useModuleCurrentContext } from "@equinor/fusion-framework-react-module-context"
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
    const { currentContext } = useModuleCurrentContext()
    const [isLoading, setIsLoading] = useState<boolean>()
    const [isCreating, setIsCreating] = useState<boolean>()

    useEffect(() => {
        console.log("current project", currentContext)
        const fetchAndSetProject = async () => {
            if (!currentContext?.externalId) {
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
    }, [currentContext, setProject]) // Re-run effect if currentProject changes

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

import styled from "styled-components"
import { useModuleCurrentContext } from "@equinor/fusion-framework-react-module-context"
import { Typography } from "@equinor/eds-core-react"
import { useQuery, useQueryClient } from "react-query"
import { useNavigate } from "react-router"
import Classification from "./Classification"
import { GetProjectService } from "../../Services/ProjectService"
import { useAppContext } from "../../Context/AppContext"

const Wrapper = styled.div`
    background-color: white;
    padding: 20px;
    display: flex;
    flex-direction: row;
    gap: 20px;
    align-items: center;
    justify-content: space-between;
`

const ProjectControls = () => {
    const { currentContext } = useModuleCurrentContext()
    const queryClient = useQueryClient()
    const { setSnackBarMessage } = useAppContext()
    const navigate = useNavigate()

    const projectId = currentContext?.externalId

    const fetchProjectData = async () => {
        const projectService = await GetProjectService()
        return projectService.getProject(projectId!)
    }

    useQuery(
        ["apiData", projectId],
        fetchProjectData,
        {
            enabled: !!projectId,
            onSuccess: (result) => {
                const projectData = result

                queryClient.setQueryData([{
                    projectId,
                }], projectData)
            },
            onError: (error: Error) => {
                console.error("Error fetching data:", error)
                setSnackBarMessage("Project data not found")
                navigate("/")
            },
        },
    )

    return (
        <Wrapper>
            <Typography variant="h4">
                {currentContext?.title}
            </Typography>
            <Classification />
        </Wrapper>
    )
}

export default ProjectControls

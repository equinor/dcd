import styled from "styled-components"
import { useModuleCurrentContext } from "@equinor/fusion-framework-react-module-context"
import { Typography } from "@equinor/eds-core-react"
import { useQuery, useQueryClient } from "react-query"
import { useNavigate } from "react-router"
import Classification from "./Classification"
import { GetProjectService } from "../../Services/ProjectService"
import { useProjectContext } from "../../Context/ProjectContext"
import useDataEdits from "../../Hooks/useDataEdits"
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
    const { project } = useProjectContext()
    const queryClient = useQueryClient()
    const { setSnackBarMessage, editMode } = useAppContext()
    const { addEdit } = useDataEdits()
    const navigate = useNavigate()

    const projectId = project?.id || null

    const fetchCaseData = async () => {
        const caseService = await GetProjectService()
        return caseService.getProject(projectId!)
    }

    useQuery(
        ["apiData", { projectId }],
        fetchCaseData,
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

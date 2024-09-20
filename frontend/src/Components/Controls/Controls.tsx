import { useState, useEffect } from "react"
import { useNavigate, useParams, useLocation } from "react-router-dom"
import { useModuleCurrentContext } from "@equinor/fusion-framework-react-module-context"
import {
    Button,
    Typography,
} from "@equinor/eds-core-react"
import Grid from "@mui/material/Grid"
import { useQuery, useQueryClient } from "react-query"
import styled from "styled-components"
import { projectPath } from "../../Utils/common"
import { useProjectContext } from "../../Context/ProjectContext"
import { GetProjectService } from "../../Services/ProjectService"
import { useAppContext } from "../../Context/AppContext"
import CaseControls from "./CaseControls"
import WhatsNewModal from "../Modal/WhatsNewModal"
import Modal from "../Modal/Modal"
import ProjectControls from "./ProjectControls"

const Wrapper = styled(Grid)`
    padding-top: 20px;
    margin-bottom: 20px;
    background-color: white;
`

const Controls = () => {
    const {
        project,
        setProject,
    } = useProjectContext()

    const navigate = useNavigate()
    const location = useLocation()
    const { currentContext } = useModuleCurrentContext()
    const { editMode, setEditMode } = useAppContext()
    const { caseId } = useParams()

    const [isCanceling, setIsCanceling] = useState<boolean>(false)
    const [projectLastUpdated, setProjectLastUpdated] = useState<string>("")
    const [caseLastUpdated, setCaseLastUpdated] = useState<string>("")

    const cancelEdit = async () => {
        setEditMode(false)
        setIsCanceling(false)
    }

    const backToProject = async () => {
        cancelEdit()
        navigate(projectPath(currentContext?.id!))
    }

    const handleEdit = () => {
        if (editMode) { // user is going out of edit mode in case
            cancelEdit()
        } else if (!editMode) { // user is going into edit mode in case
            setEditMode(true)
        }
    }

    const projectId = project?.id || null

    const queryClient = useQueryClient()
    const { data: apiData } = useQuery<Components.Schemas.CaseWithAssetsDto | undefined>(
        ["apiData", { projectId, caseId }],
        () => queryClient.getQueryData(["apiData", { projectId, caseId }]),
        {
            enabled: !!projectId && !!caseId,
            initialData: () => queryClient.getQueryData(["apiData", { projectId, caseId }]),
        },
    )

    const caseData = apiData?.case

    useEffect(() => {
        if (location.pathname.includes("case")) {
            setCaseLastUpdated(caseData?.modifyTime ?? "")
            setProjectLastUpdated(caseData?.modifyTime ?? "")
        } else {
            setProjectLastUpdated(caseData?.modifyTime ?? "")
        }
    }, [location.pathname, caseData, project])

    useEffect(() => {
        cancelEdit()
    }, [caseId])

    useEffect(() => {
        setProjectLastUpdated(project?.modifyTime ?? "")
    }, [caseData, project])

    useEffect(() => {
        const fetchData = async () => {
            if (location.pathname.includes("case") && project?.id && caseId) {
                const projectService = await GetProjectService()
                const projectData = await projectService.getProject(project.id)
                setProject(projectData)
                setProjectLastUpdated(projectData.modifyTime)
            }
        }
        fetchData()
    }, [location.pathname, project?.id, caseId, setProject])

    return (
        <Wrapper>
            <WhatsNewModal />
            <Modal
                isOpen={isCanceling}
                title="Are you sure you want to cancel?"
                size="sm"
                content={<Typography>All unsaved changes will be lost. This action cannot be undone.</Typography>}
                actions={(
                    <>
                        <Button onClick={() => setIsCanceling(false)} variant="outlined">
                            Continue editing
                        </Button>
                        <Button onClick={cancelEdit} variant="contained" color="danger">
                            Discard changes
                        </Button>
                    </>
                )}
            />

            {project && caseId ? (
                <CaseControls
                    backToProject={backToProject}
                    projectId={project.id}
                    caseId={caseId}
                    caseLastUpdated={caseLastUpdated}
                    handleEdit={handleEdit}
                />
            )
                : (
                    <ProjectControls
                        projectLastUpdated={projectLastUpdated}
                        handleEdit={handleEdit}
                    />
                )}
        </Wrapper>
    )
}

export default Controls

import { useEffect } from "react"
import { useParams } from "react-router-dom"
import Grid from "@mui/material/Grid2"
import styled from "styled-components"
import { Typography } from "@equinor/eds-core-react"
import { useModuleCurrentContext } from "@equinor/fusion-framework-react-module-context"

import { useProjectContext } from "@/Store/ProjectContext"
import { useAppStore } from "@/Store/AppStore"
import { useDataFetch } from "@/Hooks"
import { useAppNavigation } from "@/Hooks/useNavigate"
import WhatsNewModal from "../Modal/WhatsNewModal"
import ProjectControls from "./ProjectControls"
import CaseControls from "./CaseControls"
import RevisionChip from "./Chips/RevisionChip"
import Classification from "./Chips/ClassificationChip"

const Wrapper = styled(Grid)`
    padding-top: 20px;
    margin-bottom: 20px;
    background-color: white;
`
const Project = styled.div`
    display: grid;
    grid-template-rows: auto auto; 
    grid-template-columns: 1fr 1fr;
    grid-template-areas: 
        "top-left top-right"
        "bottom bottom";
    gap: 10px;
`
const ProjectHeader = styled.div`
    display: flex;
    align-items: center;
    gap: 15px;
    padding: 0 15px;
    grid-area: top-left;
`

const Controls = () => {
    const { editMode, setEditMode } = useAppStore()
    const { currentContext } = useModuleCurrentContext()
    const { caseId, revisionId } = useParams()
    const { isRevision } = useProjectContext()
    const revisionAndProjectData = useDataFetch()
    const { navigateToProject, navigateToRevision } = useAppNavigation()

    const backToProject = async () => {
        setEditMode(false)
        if (isRevision && revisionId) {
            navigateToRevision(revisionId)
        } else {
            navigateToProject()
        }
    }

    const handleEdit = () => {
        if (editMode) {
            setEditMode(false)
        } else {
            setEditMode(true)
        }
    }

    return (
        <Wrapper>
            <WhatsNewModal />
            <Project>
                <ProjectHeader>
                    <Typography variant="h4" color="var(--text-static-icons-tertiary, #6F6F6F);">
                        {currentContext?.title}
                    </Typography>
                    <Classification />
                    {isRevision && (
                        <RevisionChip />
                    )}
                </ProjectHeader>
                {revisionAndProjectData && !caseId && (
                    <ProjectControls
                        projectLastUpdated={revisionAndProjectData.commonProjectAndRevisionData.updatedUtc}
                        handleEdit={handleEdit}
                    />
                )}
            </Project>

            {revisionAndProjectData && caseId && (
                <CaseControls
                    backToProject={backToProject}
                    projectId={revisionAndProjectData.projectId}
                    caseId={caseId}
                    handleEdit={handleEdit}
                />
            )}
        </Wrapper>
    )
}

export default Controls

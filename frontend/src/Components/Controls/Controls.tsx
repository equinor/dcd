import { useState, useEffect } from "react"
import { useNavigate, useParams } from "react-router-dom"
import { useModuleCurrentContext } from "@equinor/fusion-framework-react-module-context"
import Grid from "@mui/material/Grid"
import { useQuery } from "@tanstack/react-query"
import styled from "styled-components"

import { Typography } from "@equinor/eds-core-react"
import { caseQueryFn } from "@/Services/QueryFunctions"
import { useProjectContext } from "@/Context/ProjectContext"
import { useAppContext } from "@/Context/AppContext"
import { useDataFetch } from "@/Hooks/useDataFetch"
import { projectPath } from "@/Utils/common"
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
    const navigate = useNavigate()
    const { currentContext } = useModuleCurrentContext()
    const { editMode, setEditMode } = useAppContext()
    const { caseId, revisionId } = useParams()
    const { projectId, isRevision } = useProjectContext()
    const revisionAndProjectData = useDataFetch()

    const [projectLastUpdated, setProjectLastUpdated] = useState<string>("")
    const [caseLastUpdated, setCaseLastUpdated] = useState<string>("")

    const { data: apiData } = useQuery({
        queryKey: ["caseApiData", isRevision ? revisionId : projectId, caseId],
        queryFn: () => caseQueryFn(isRevision ? revisionId ?? "" : projectId, caseId),
        enabled: !!projectId && !!caseId,
    })

    const cancelEdit = async () => {
        setEditMode(false)
    }

    const backToProject = async () => {
        cancelEdit()
        if (isRevision && revisionId) {
            navigate(`${projectPath(currentContext?.id!)}/revision/${revisionId}`)
        } else {
            navigate(projectPath(currentContext?.id!))
        }
    }

    const handleEdit = () => {
        if (editMode) {
            cancelEdit()
        } else {
            setEditMode(true)
        }
    }

    const caseData = apiData?.case

    useEffect(() => {
        cancelEdit()
    }, [caseId])

    useEffect(() => {
        setProjectLastUpdated(revisionAndProjectData?.commonProjectAndRevisionData.modifyTime ?? "")
    }, [revisionAndProjectData])

    useEffect(() => {
        if (apiData && caseData) {
            setCaseLastUpdated(caseData.modifyTime)
        }
    }, [caseData])

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
                        projectLastUpdated={projectLastUpdated}
                        handleEdit={handleEdit}
                    />
                )}
            </Project>

            {revisionAndProjectData && caseId && (
                <CaseControls
                    backToProject={backToProject}
                    projectId={revisionAndProjectData.projectId}
                    caseId={caseId}
                    caseLastUpdated={caseLastUpdated}
                    handleEdit={handleEdit}
                />
            )}
        </Wrapper>
    )
}

export default Controls

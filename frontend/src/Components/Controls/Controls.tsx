import { useState, useEffect } from "react"
import { useNavigate, useParams } from "react-router-dom"
import { useModuleCurrentContext } from "@equinor/fusion-framework-react-module-context"
import Grid from "@mui/material/Grid"
import { useQuery } from "@tanstack/react-query"
import styled from "styled-components"
import { projectPath } from "../../Utils/common"
import { useProjectContext } from "../../Context/ProjectContext"
import { useAppContext } from "../../Context/AppContext"
import CaseControls from "./CaseControls"
import WhatsNewModal from "../Modal/WhatsNewModal"
import ProjectControls from "./ProjectControls"
import { caseQueryFn, projectQueryFn, revisionQueryFn } from "../../Services/QueryFunctions"

const Wrapper = styled(Grid)`
    padding-top: 20px;
    margin-bottom: 20px;
    background-color: white;
`

const Controls = () => {
    const navigate = useNavigate()
    const { currentContext } = useModuleCurrentContext()
    const { editMode, setEditMode } = useAppContext()
    const { caseId, revisionId } = useParams()
    const { projectId, isRevision } = useProjectContext()

    const [projectLastUpdated, setProjectLastUpdated] = useState<string>("")
    const [caseLastUpdated, setCaseLastUpdated] = useState<string>("")

    const { data: apiData } = useQuery({
        queryKey: ["caseApiData", isRevision ? revisionId : projectId, caseId],
        queryFn: () => caseQueryFn(isRevision ? revisionId ?? "" : projectId, caseId),
        enabled: !!projectId && !!caseId,
    })

    const { data: projectData } = useQuery({
        queryKey: ["projectApiData", projectId],
        queryFn: () => projectQueryFn(projectId),
        enabled: !!projectId,
    })

    const { data: revisionData } = useQuery({
        queryKey: ["revisionApiData", revisionId],
        queryFn: () => revisionQueryFn(projectId, revisionId),
        enabled: !!revisionId,
    })

    const cancelEdit = async () => {
        setEditMode(false)
    }

    const backToProject = async () => {
        cancelEdit()
        navigate(projectPath(currentContext?.id!))
    }

    const handleEdit = () => {
        if (editMode) {
            cancelEdit()
        } else {
            setEditMode(true)
        }
    }

    const caseData = apiData?.case as Components.Schemas.CaseWithProfilesDto

    useEffect(() => {
        cancelEdit()
    }, [caseId])

    useEffect(() => {
        if (isRevision && revisionData) {
            setProjectLastUpdated(revisionData.modifyTime)
        } else if (projectData) {
            setProjectLastUpdated(projectData.modifyTime)
        }
    }, [projectData, isRevision, revisionData])

    useEffect(() => {
        if (apiData) {
            setCaseLastUpdated(caseData.modifyTime)
        }
    }, [caseData])

    /*
    useEffect(() => {
        const fetchData = async () => {
            if (location.pathname.includes("case") && projectData?.id && caseId) {
                const projectService = await GetProjectService()
                const data = await projectService.getProject(projectData.id)
                setProject(data)
                setProjectLastUpdated(data.modifyTime)
            }
        }
        fetchData()
    }, [location.pathname, projectData, caseId, setProject])
    */

    // useEffect(() => {
    //     if (location.pathname.includes("case")) {
    //         setCaseLastUpdated(caseData?.modifyTime ?? "")
    //         setProjectLastUpdated(caseData?.modifyTime ?? "")
    //     } else {
    //         setProjectLastUpdated(caseData?.modifyTime ?? "")
    //     }
    // }, [location.pathname, caseData, projectData])

    return (
        <Wrapper>
            <WhatsNewModal />
            {projectData && caseId ? (
                <CaseControls
                    backToProject={backToProject}
                    projectId={projectData.id}
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

import React, { useRef, useEffect, useState } from "react"
import { useQuery, useQueryClient } from "react-query"
import {
    Icon, Button, Input, Typography,
} from "@equinor/eds-core-react"
import { arrow_back } from "@equinor/eds-icons"
import Grid from "@mui/material/Grid"
import { useNavigate } from "react-router-dom"
import { useProjectContext } from "../../Context/ProjectContext"
import { GetCaseService } from "../../Services/CaseService"
import { useAppContext } from "../../Context/AppContext"
import { ChooseReferenceCase, ReferenceCaseIcon } from "../Case/Components/ReferenceCaseIcon"
import Classification from "./Classification"
import { EMPTY_GUID } from "../../Utils/constants"
import { GetProjectService } from "../../Services/ProjectService"
import useDataEdits from "../../Hooks/useDataEdits"

interface props {
    backToProject: () => void;
    projectId: string;
    caseId: string;
}

const CaseControls: React.FC<props> = ({ backToProject, projectId, caseId }) => {
    const nameInputRef = useRef<HTMLInputElement>(null)
    const { project, setProject } = useProjectContext()
    const { setSnackBarMessage, editMode } = useAppContext()
    const { addEdit } = useDataEdits()
    const navigate = useNavigate()
    const queryClient = useQueryClient()

    const [caseName, setCaseName] = useState("")

    const fetchCaseData = async () => {
        const caseService = await GetCaseService()
        return caseService.getCaseWithAssets(projectId, caseId)
    }

    useQuery(
        ["apiData", { projectId, caseId }],
        fetchCaseData,
        {
            enabled: !!projectId && !!caseId,
            onSuccess: (result) => {
                queryClient.setQueryData([{
                    projectId, caseId, resourceId: EMPTY_GUID, resourceProfileId: EMPTY_GUID,
                }], result.case)
            },
            onError: (error: Error) => {
                console.error("Error fetching data:", error)
                setSnackBarMessage("Case data not found. Redirecting back to project")
                navigate("/")
            },
        },
    )

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
        if (caseData?.name) {
            setCaseName(caseData.name)
        }
    }, [caseData?.name])

    const handleCaseNameChange = (name: string) => {
        if (caseData) {
            addEdit({
                newValue: name,
                previousValue: caseData.name,
                inputLabel: "Name",
                projectId,
                resourceName: "case",
                resourcePropertyKey: "name",
                resourceId: EMPTY_GUID,
                caseId,
            })
        }
    }

    const handleReferenceCaseChange = async (referenceCaseId: string) => {
        if (project) {
            const newProject = {
                ...project,
            }
            if (newProject.referenceCaseId === referenceCaseId) {
                newProject.referenceCaseId = EMPTY_GUID
            } else {
                newProject.referenceCaseId = referenceCaseId ?? ""
            }
            const updateProject = await (await GetProjectService()).updateProject(projectId, newProject)
            setProject(updateProject)
        }
    }

    if (!caseData) {
        return null
    }

    return (
        <>
            <Grid item xs={0}>
                <Button onClick={backToProject} variant="ghost_icon">
                    <Icon data={arrow_back} />
                </Button>
            </Grid>

            <Grid item xs display="flex" alignItems="center" gap={1}>
                {editMode ? (
                    <>
                        <ChooseReferenceCase
                            projectRefCaseId={project?.referenceCaseId}
                            projectCaseId={caseId}
                            handleReferenceCaseChange={() => handleReferenceCaseChange(caseId)}
                        />
                        <Input
                            ref={nameInputRef}
                            type="text"
                            value={caseName}
                            onChange={(e: any) => setCaseName(e.target.value)}
                            onBlur={() => handleCaseNameChange(nameInputRef.current?.value || "")}
                        />
                    </>
                ) : (
                    <>
                        {project?.referenceCaseId === caseId && <ReferenceCaseIcon />}
                        <Typography variant="h4">{caseData.name}</Typography>
                        <Classification />
                    </>
                )}
            </Grid>
        </>
    )
}

export default CaseControls

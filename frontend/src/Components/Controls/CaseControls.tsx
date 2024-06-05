import React, { useRef } from "react"
import { useQuery, useQueryClient } from "react-query"
import {
    Icon,
    Button,
    Input,
    Typography,
} from "@equinor/eds-core-react"
import { arrow_back } from "@equinor/eds-icons"
import Grid from "@mui/material/Grid"
import { useProjectContext } from "../../Context/ProjectContext"
import { GetCaseService } from "../../Services/CaseService"
import { useAppContext } from "../../Context/AppContext"
import { ChooseReferenceCase, ReferenceCaseIcon } from "../Case/Components/ReferenceCaseIcon"
import Classification from "./Classification"
import { EMPTY_GUID } from "../../Utils/constants"
import { GetProjectService } from "../../Services/ProjectService"
import useDataEdits from "../../Hooks/useDataEdits"

interface props {
    backToProject: () => void
    projectId: string
    caseId: string
}

const CaseControls: React.FC<props> = ({ backToProject, projectId, caseId }) => {
    const nameInput = useRef<any>(null)
    const { project, setProject } = useProjectContext()
    const { setSnackBarMessage, editMode } = useAppContext()
    const { addEdit } = useDataEdits()

    const queryClient = useQueryClient()

    useQuery(
        ["apiData", { projectId, caseId }],
        async () => {
            const caseService = await GetCaseService()
            return caseService.getCaseWithAssets(projectId, caseId)
        },
        {
            enabled: !!projectId && !!caseId,
            onSuccess: (result) => {
                const caseData = result.case
                const drainageStrategyData = result.drainageStrategy
                const explorationData = result.exploration
                const substructureData = result.substructure
                const surfData = result.surf
                const topsideData = result.topside
                const transportData = result.transport
                const wellProjectData = result.wellProject

                queryClient.setQueryData([{ projectId, caseId, resourceId: "" }], caseData)
                queryClient.setQueryData([{ projectId, caseId, resourceId: topsideData.id }], topsideData)
                queryClient.setQueryData([{ projectId, caseId, resourceId: surfData.id }], surfData)
                queryClient.setQueryData([{ projectId, caseId, resourceId: substructureData.id }], substructureData)
                queryClient.setQueryData([{ projectId, caseId, resourceId: transportData.id }], transportData)
                queryClient.setQueryData([{ projectId, caseId, resourceId: drainageStrategyData.id }], drainageStrategyData)
                queryClient.setQueryData([{ projectId, caseId, resourceId: explorationData.id }], explorationData)
                queryClient.setQueryData([{ projectId, caseId, resourceId: wellProjectData.id }], wellProjectData)
            },
            onError: (error: Error) => {
                console.error("Error fetching data:", error)
                setSnackBarMessage(error.message)
            },
        },
    )

    const caseData = queryClient.getQueryData([{ projectId, caseId, resourceId: "" }]) as Components.Schemas.CaseDto

    const handleCaseNameChange = (name: string) => {
        if (caseData) {
            addEdit({
                newValue: name,
                previousValue: caseData.name,
                inputLabel: "Name",
                projectId,
                resourceName: "case",
                resourcePropertyKey: "name",
                resourceId: "",
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

    return (
        <>
            <Grid item xs={0}>
                <Button
                    onClick={backToProject}
                    variant="ghost_icon"
                >
                    <Icon data={arrow_back} />
                </Button>
            </Grid>
            {caseData && (
                <Grid item xs display="flex" alignItems="center" gap={1}>
                    {editMode
                        ? (
                            <>
                                <ChooseReferenceCase
                                    projectRefCaseId={project?.referenceCaseId}
                                    projectCaseId={caseId}
                                    handleReferenceCaseChange={() => handleReferenceCaseChange(caseId)}
                                />
                                <Input // todo: should not be allowed to be empty
                                    ref={nameInput}
                                    type="text"
                                    defaultValue={caseData.name ?? ""}
                                    onBlur={() => handleCaseNameChange(nameInput.current.value)}
                                />
                            </>
                        )
                        : (
                            <>
                                {project?.referenceCaseId === caseId && (
                                    <ReferenceCaseIcon />
                                )}
                                <Typography variant="h4">
                                    {caseData.name ?? ""}
                                </Typography>
                                <Classification />
                            </>
                        )}
                </Grid>
            )}
        </>
    )
}

export default CaseControls

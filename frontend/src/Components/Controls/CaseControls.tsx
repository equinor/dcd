import React, { useRef, useEffect, useState } from "react"
import { useQuery } from "@tanstack/react-query"
import {
    Icon, Button, Input, Typography,
    Tooltip,
} from "@equinor/eds-core-react"
import {
    arrow_back,
    more_vertical,
    visibility,
    edit,
} from "@equinor/eds-icons"
import { useParams } from "react-router-dom"
import styled from "styled-components"

import { ChooseReferenceCase, ReferenceCaseIcon } from "@/Components/Case/Components/ReferenceCaseIcon"
import CaseDropMenu from "@/Components/Case/Components/CaseDropMenu"
import { caseQueryFn } from "@/Services/QueryFunctions"
import { GetProjectService } from "@/Services/ProjectService"
import { EMPTY_GUID } from "@/Utils/constants"
import { formatDateAndTime } from "@/Utils/common"
import { useProjectContext } from "@/Context/ProjectContext"
import { useAppContext } from "@/Context/AppContext"
import useEditDisabled from "@/Hooks/useEditDisabled"
import { useDataFetch } from "@/Hooks/useDataFetch"
import useEditProject from "@/Hooks/useEditProject"
import useEditCase from "@/Hooks/useEditCase"
import UndoControls from "./UndoControls"
import CaseTabs from "./TabNavigators/CaseTabs"

const Header = styled.div`
    display: flex;
    align-items: center;
    justify-content: space-between;
    gap: 10px;
    padding: 0 5px;
`

const CenteringContainer = styled.div`
    display: flex;
    align-items: center;
    gap: 10px;
`

const CaseTitleEdit = styled.div`
    display: flex;
    gap: 10px;
`

const DropButton = styled(Icon)`
    cursor: pointer;
    padding: 0 5px;
`

const CaseContainer = styled.div`
    display: flex;
    flex-direction: column;
    gap: 10px;
`

interface props {
    backToProject: () => void;
    projectId: string;
    caseId: string;
    handleEdit: () => void;
}

const CaseControls: React.FC<props> = ({
    backToProject,
    projectId,
    caseId,
    handleEdit,
}) => {
    const { isRevision } = useProjectContext()
    const nameInputRef = useRef<HTMLInputElement>(null)
    const { addProjectEdit } = useEditProject()
    const { setSnackBarMessage, editMode } = useAppContext()
    const { addEdit } = useEditCase()
    const { isEditDisabled, getEditDisabledText } = useEditDisabled()
    const revisionAndProjectData = useDataFetch()
    const { revisionId } = useParams()

    const [caseName, setCaseName] = useState("")
    const [menuAnchorEl, setMenuAnchorEl] = useState<any | null>(null)
    const [isMenuOpen, setIsMenuOpen] = useState<boolean>(false)

    const { data: apiData, error } = useQuery({
        queryKey: ["caseApiData", isRevision ? revisionId : projectId, caseId],
        queryFn: () => caseQueryFn(isRevision ? revisionId ?? "" : projectId, caseId),
        enabled: !!projectId && !!caseId,
        refetchInterval: 20000,
    })

    if (error) {
        setSnackBarMessage(error.message)
    }

    useEffect(() => {
        if (apiData && apiData.case.name) {
            setCaseName(apiData.case.name)
        }
    }, [apiData])

    if (!apiData) { return null }
    const caseData = apiData.case

    const handleCaseNameChange = (name: string) => {
        const previousResourceObject = structuredClone(caseData)
        const newResourceObject = structuredClone(caseData)
        newResourceObject.name = name
        if (caseData) {
            addEdit({
                previousDisplayValue: previousResourceObject.name,
                newDisplayValue: name,
                newResourceObject,
                previousResourceObject,
                inputLabel: "caseName",
                projectId,
                resourceName: "case",
                resourcePropertyKey: "name",
                resourceId: EMPTY_GUID,
                caseId,
            })
        }
    }

    const handleReferenceCaseChange = async (referenceCaseId: string) => {
        if (revisionAndProjectData) {
            const newProject: Components.Schemas.UpdateProjectDto = {
                ...revisionAndProjectData.commonProjectAndRevisionData,
            }
            if (newProject.referenceCaseId === referenceCaseId) {
                newProject.referenceCaseId = EMPTY_GUID
            } else {
                newProject.referenceCaseId = referenceCaseId ?? ""
            }
            const updateProject = await (await GetProjectService()).updateProject(projectId, newProject)
            if (updateProject) {
                addProjectEdit(updateProject.projectId, updateProject.commonProjectAndRevisionData)
            }
        }
    }

    return (
        <>
            <Header>
                <CaseContainer>
                    <CenteringContainer>
                        <Button onClick={backToProject} variant="ghost_icon">
                            <Icon data={arrow_back} />
                        </Button>
                        <CenteringContainer>
                            {editMode ? (
                                <CaseTitleEdit>
                                    <ChooseReferenceCase
                                        projectRefCaseId={revisionAndProjectData?.commonProjectAndRevisionData.referenceCaseId}
                                        projectCaseId={caseId}
                                        handleReferenceCaseChange={() => handleReferenceCaseChange(caseId)}
                                    />
                                    <Input
                                        id="caseName"
                                        ref={nameInputRef}
                                        type="text"
                                        value={caseName}
                                        onChange={(e: any) => setCaseName(e.target.value)}
                                        onBlur={() => handleCaseNameChange(nameInputRef.current?.value || "")}
                                    />
                                </CaseTitleEdit>
                            ) : (
                                <>
                                    {revisionAndProjectData?.commonProjectAndRevisionData.referenceCaseId === caseId && <ReferenceCaseIcon />}
                                    <Typography variant="h4">{caseData.name}</Typography>
                                </>
                            )}
                        </CenteringContainer>
                    </CenteringContainer>
                </CaseContainer>
                <CenteringContainer>
                    {!editMode
                        ? (
                            <Typography variant="caption">
                                Case last updated
                                {" "}
                                {formatDateAndTime(caseData.modifyTime)}
                            </Typography>
                        )
                        : <UndoControls />}
                    <Tooltip title={getEditDisabledText()}>
                        <Button
                            onClick={handleEdit}
                            disabled={isEditDisabled}
                            variant={editMode ? "outlined" : "contained"}
                            id="toggleCaseEditButton"
                        >
                            {editMode && (
                                <>
                                    <Icon data={visibility} />
                                    <span>View</span>
                                </>
                            )}
                            {!editMode && (
                                <>
                                    <Icon data={edit} />
                                    <span>Edit</span>
                                </>
                            )}

                        </Button>
                    </Tooltip>
                    <div>
                        <DropButton
                            id="caseDropMenuButton"
                            ref={setMenuAnchorEl}
                            onClick={() => setIsMenuOpen(!isMenuOpen)}
                            data={more_vertical}
                            size={32}
                        />
                    </div>
                    <CaseDropMenu
                        isMenuOpen={isMenuOpen}
                        setIsMenuOpen={setIsMenuOpen}
                        menuAnchorEl={menuAnchorEl}
                        caseId={caseId}
                        isArchived={caseData.archived}
                    />
                </CenteringContainer>
            </Header>
            <CaseTabs
                caseId={caseId}
            />
        </>
    )
}
export default CaseControls

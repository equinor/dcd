import React, { useRef, useEffect, useState } from "react"
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
import styled from "styled-components"

import { ChooseReferenceCase, ReferenceCaseIcon } from "@/Components/Tables/ProjectTables/OverviewCasesTable/CellRenderers/ReferenceCaseIcon"
import CaseDropMenu from "./CaseDropMenu"
import { GetProjectService } from "@/Services/ProjectService"
import { EMPTY_GUID } from "@/Utils/constants"
import { formatDateAndTime } from "@/Utils/DateUtils"
import { useAppStore } from "@/Store/AppStore"
import useCanUserEdit from "@/Hooks/useCanUserEdit"
import {
    useDataFetch, useEditProject, useCaseApiData,
} from "@/Hooks"
import { useCaseMutation } from "@/Hooks/Mutations"
import UndoControls from "./UndoControls"
import CaseTabs from "./TabNavigators/CaseTabs"

const Header = styled.div`
    display: flex;
    align-items: center;
    justify-content: space-between;
    gap: 10px;
    padding: 0 5px;
`

const BackButton = styled(Button)`
    min-width: 40px;
`

const TabContainer = styled.div`
    display: flex;
    margin-top: 8px;
    border-bottom: 1px solid #DCDCDC;
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
    const nameInputRef = useRef<HTMLInputElement>(null)
    const { addProjectEdit } = useEditProject()
    const { editMode } = useAppStore()
    const { isEditDisabled, getEditDisabledText } = useCanUserEdit()
    const revisionAndProjectData = useDataFetch()
    const { apiData } = useCaseApiData()
    const { updateName } = useCaseMutation()

    const [caseName, setCaseName] = useState("")
    const [menuAnchorEl, setMenuAnchorEl] = useState<any | null>(null)
    const [isMenuOpen, setIsMenuOpen] = useState<boolean>(false)

    useEffect(() => {
        if (apiData && apiData.case.name) {
            setCaseName(apiData.case.name)
        }
    }, [apiData])

    if (!apiData) { return null }
    const caseData = apiData.case

    const handleCaseNameChange = (name: string) => {
        if (name !== caseData.name) {
            updateName(name)
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
            const updateProject = await GetProjectService().updateProject(projectId, newProject)
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
                        <BackButton onClick={backToProject} variant="ghost_icon">
                            <Icon data={arrow_back} />
                        </BackButton>
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
                                {formatDateAndTime(caseData.updatedUtc)}
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
            <TabContainer>
                <CaseTabs
                    caseId={caseId}
                />
            </TabContainer>
        </>
    )
}
export default CaseControls

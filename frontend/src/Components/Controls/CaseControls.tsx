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
import { useNavigate, useLocation, useParams } from "react-router-dom"
import Tabs from "@mui/material/Tabs"
import Tab from "@mui/material/Tab"
import styled from "styled-components"
import { useModuleCurrentContext } from "@equinor/fusion-framework-react-module-context"

import { ChooseReferenceCase, ReferenceCaseIcon } from "@/Components/Case/Components/ReferenceCaseIcon"
import CaseDropMenu from "@/Components/Case/Components/CaseDropMenu"
import { caseQueryFn } from "@/Services/QueryFunctions"
import { GetProjectService } from "@/Services/ProjectService"
import { EMPTY_GUID, caseTabNames } from "@/Utils/constants"
import { formatDateAndTime } from "@/Utils/common"
import { useProjectContext } from "@/Context/ProjectContext"
import { useCaseContext } from "@/Context/CaseContext"
import { useAppContext } from "@/Context/AppContext"
import useEditDisabled from "@/Hooks/useEditDisabled"
import { useDataFetch } from "@/Hooks/useDataFetch"
import useEditProject from "@/Hooks/useEditProject"
import useEditCase from "@/Hooks/useEditCase"
import Classification from "./ClassificationChip"
import UndoControls from "./UndoControls"
import RevisionChip from "./RevisionChip"

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

const ProjectAndCaseContainer = styled.div`
    display: flex;
    flex-direction: column;
    gap: 10px;
`

const Project = styled.div`
    display: flex;
    flex-direction: row;
    align-items: center;
    gap: 10px;
    margin-left: 25px;
`

interface props {
    backToProject: () => void;
    projectId: string;
    caseId: string;
    caseLastUpdated: string;
    handleEdit: () => void;
}

const CaseControls: React.FC<props> = ({
    backToProject,
    projectId,
    caseId,
    caseLastUpdated,
    handleEdit,
}) => {
    const { currentContext } = useModuleCurrentContext()
    const { isRevision } = useProjectContext()
    const nameInputRef = useRef<HTMLInputElement>(null)
    const { addProjectEdit } = useEditProject()
    const { setSnackBarMessage, editMode } = useAppContext()
    const { addEdit } = useEditCase()
    const navigate = useNavigate()
    const { activeTabCase } = useCaseContext()
    const location = useLocation()
    const { isEditDisabled, getEditDisabledText } = useEditDisabled()
    const revisionAndProjectData = useDataFetch()

    const [caseName, setCaseName] = useState("")
    const [menuAnchorEl, setMenuAnchorEl] = useState<any | null>(null)
    const [isMenuOpen, setIsMenuOpen] = useState<boolean>(false)
    const { revisionId } = useParams()

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

    const handleTabChange = (index: number) => {
        const projectUrl = location.pathname.split("/case")[0]
        navigate(`${projectUrl}/case/${caseId}/${caseTabNames[index]}`)
    }

    return (
        <>
            <Header>
                <ProjectAndCaseContainer>
                    <Project>
                        <Typography variant="h6" color="var(--text-static-icons-tertiary, #6F6F6F);">
                            {currentContext?.title}
                        </Typography>
                        <Classification />
                        {isRevision && (
                            <RevisionChip />
                        )}
                    </Project>
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
                </ProjectAndCaseContainer>
                <CenteringContainer>
                    {!editMode
                        ? (
                            <Typography variant="caption">
                                Case last updated
                                {" "}
                                {formatDateAndTime(caseLastUpdated)}
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
            <Tabs
                value={activeTabCase}
                onChange={(_, index) => handleTabChange(index)}
                variant="scrollable"
            >
                {caseTabNames.map((tabName) => <Tab key={tabName} label={tabName} />)}
            </Tabs>

        </>
    )
}
export default CaseControls

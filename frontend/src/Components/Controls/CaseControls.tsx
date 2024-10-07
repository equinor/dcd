import React, { useRef, useEffect, useState } from "react"
import { useQuery } from "@tanstack/react-query"
import {
    Icon, Button, Input, Typography,
} from "@equinor/eds-core-react"
import {
    arrow_back,
    more_vertical,
    visibility,
    edit,
} from "@equinor/eds-icons"
import { useNavigate, useLocation } from "react-router-dom"
import Tabs from "@mui/material/Tabs"
import Tab from "@mui/material/Tab"
import styled from "styled-components"

import { useAppContext } from "@/Context/AppContext"
import { EMPTY_GUID, caseTabNames } from "@/Utils/constants"
import { GetProjectService } from "@/Services/ProjectService"
import { useCaseContext } from "@/Context/CaseContext"
import useEditCase from "@/Hooks/useEditCase"
import { formatDateAndTime } from "@/Utils/common"
import UndoControls from "./UndoControls"
import { caseQueryFn, projectQueryFn } from "@/Services/QueryFunctions"
import useEditProject from "@/Hooks/useEditProject"
import { ChooseReferenceCase, ReferenceCaseIcon } from "../Case/Components/ReferenceCaseIcon"
import CaseDropMenu from "../Case/Components/CaseDropMenu"
import Classification from "./Classification"

const Header = styled.div`
    display: flex;
    align-items: center;
    justify-content: space-between;
    gap: 10px;
    padding: 0 5px;

    & div {
        display: flex;
        align-items: center;
        gap: 10px;
    }
`
const DropButton = styled(Icon)`
    cursor: pointer;
    padding: 0 5px;
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
    const nameInputRef = useRef<HTMLInputElement>(null)
    const { addProjectEdit } = useEditProject()
    const { setSnackBarMessage, editMode } = useAppContext()
    const { addEdit } = useEditCase()
    const navigate = useNavigate()
    const { activeTabCase } = useCaseContext()
    const location = useLocation()

    const [caseName, setCaseName] = useState("")
    const [menuAnchorEl, setMenuAnchorEl] = useState<any | null>(null)
    const [revisionMenuAnchorEl, setRevisionMenuAnchorEl] = useState<any | null>(null)
    const [isMenuOpen, setIsMenuOpen] = useState<boolean>(false)
    const [isRevisionMenuOpen, setIsRevisionMenuOpen] = useState<boolean>(false)

    const { data: apiData, error } = useQuery({
        queryKey: ["caseApiData", projectId, caseId],
        queryFn: () => caseQueryFn(projectId, caseId),
        enabled: !!projectId && !!caseId,
        refetchInterval: 20000,
    })

    const { data: projectData } = useQuery({
        queryKey: ["projectApiData", projectId],
        queryFn: () => projectQueryFn(projectId),
        enabled: !!projectId,
    })

    /*
        // divides the data into separate queries for each resource
        useEffect(() => {
        if (isSuccess && apiData) {
            console.log("refreshing case data")
            const defaultParams = { projectId, caseId }
            Object.entries(apiData).forEach(([key, data]) => {
                if (data) {
                    queryClient.setQueryData([key, defaultParams], data)
                }
            })
        }
    }, [isSuccess, apiData, projectId, caseId])
    */

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
        if (projectData) {
            const newProject = {
                ...projectData,
            }
            if (newProject.referenceCaseId === referenceCaseId) {
                newProject.referenceCaseId = EMPTY_GUID
            } else {
                newProject.referenceCaseId = referenceCaseId ?? ""
            }
            const updateProject = await (await GetProjectService()).updateProject(projectId, newProject)
            if (updateProject) {
                addProjectEdit(updateProject.id, updateProject)
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
                <div>
                    <Button onClick={backToProject} variant="ghost_icon">
                        <Icon data={arrow_back} />
                    </Button>
                    <div>
                        {editMode ? (
                            <>
                                <ChooseReferenceCase
                                    projectRefCaseId={projectData?.referenceCaseId}
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
                            </>
                        ) : (
                            <>
                                {projectData?.referenceCaseId === caseId && <ReferenceCaseIcon />}
                                <Typography variant="h4">{caseData.name}</Typography>
                                <Classification />
                            </>
                        )}
                    </div>
                </div>
                <div>
                    {!editMode
                        ? (
                            <Typography variant="caption">
                                Case last updated
                                {" "}
                                {formatDateAndTime(caseLastUpdated)}
                            </Typography>
                        )
                        : <UndoControls />}
                    <Button onClick={handleEdit} variant={editMode ? "outlined" : "contained"}>

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
                    <div>
                        <DropButton
                            ref={setMenuAnchorEl}
                            onClick={() => setIsMenuOpen(!isMenuOpen)}
                            data={more_vertical}
                            size={32}
                        />
                    </div>
                    <CaseDropMenu
                        isMenuOpen={isMenuOpen}
                        setIsMenuOpen={setIsMenuOpen}
                        isRevisionMenuOpen={isRevisionMenuOpen}
                        setIsRevisionMenuOpen={setIsRevisionMenuOpen}
                        menuAnchorEl={menuAnchorEl}
                        setRevisionMenuAnchorEl={setRevisionMenuAnchorEl}
                        revisionMenuAnchorEl={revisionMenuAnchorEl}
                        caseId={caseId}
                        isArchived={caseData.archived}
                    />
                </div>
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

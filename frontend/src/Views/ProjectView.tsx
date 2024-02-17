import {
    Button,
    Progress,
    Tabs, Typography,
} from "@equinor/eds-core-react"
import React, { useState } from "react"
import styled from "styled-components"
import { useAppContext } from "../Context/AppContext"
import { GetProjectService } from "../Services/ProjectService"
import ProjectOverviewTab from "../Components/Project/ProjectOverviewTab"
import ProjectCompareCasesTab from "../Components/Project/ProjectCompareCasesTab"
import ProjectSettingsTab from "../Components/Project/ProjectSettingsTab"
import EditTechnicalInputModal from "../Components/EditTechnicalInput/EditTechnicalInputModal"

const { Panel } = Tabs
const { List, Tab, Panels } = Tabs

const StyledTabPanel = styled(Panel)`
    padding-top: 0px;
    border-top: 1px solid LightGray;
`

const TopWrapper = styled.div`
    display: flex;
    flex-direction: row;
    padding: 20px 20px 0 20px;
`

const PageTitle = styled(Typography)`
    flex-grow: 1;
`

const TransparentButton = styled(Button)`
    color: #007079;
    background-color: white;
    border: 1px solid #007079;
    margin-left: 1rem;
`

const Wrapper = styled.div`
    margin: 0 20px;
    display: flex;
    flex-direction: column;
`

const ProjectView = () => {
    const { project, setProject } = useAppContext()

    const [activeTab, setActiveTab] = React.useState(0)
    const [editTechnicalInputModalIsOpen, setEditTechnicalInputModalIsOpen] = useState<boolean>()
    const [isSaving, setIsSaving] = useState<boolean>()

    const toggleEditTechnicalInputModal = () => setEditTechnicalInputModalIsOpen(!editTechnicalInputModalIsOpen)

    const handleSave = async () => {
        if (!project) return

        setIsSaving(true)
        const updatedProject = { ...project }
        const result = await (await GetProjectService()).updateProject(updatedProject)
        setIsSaving(false)
        setProject(result)
    }

    return (
        <>
            <TopWrapper>
                <PageTitle variant="h4">{project?.name}</PageTitle>
                {!isSaving ? <Button onClick={handleSave}>Save</Button> : (
                    <Button>
                        <Progress.Dots />
                    </Button>
                )}
                <TransparentButton
                    onClick={toggleEditTechnicalInputModal}
                    variant="outlined"
                >
                    Edit technical input
                </TransparentButton>
            </TopWrapper>
            <Wrapper>
                <Tabs activeTab={activeTab} onChange={setActiveTab}>
                    <List>
                        <Tab>Overview </Tab>
                        <Tab>Compare cases</Tab>
                        <Tab>Settings</Tab>
                    </List>
                    <Panels>
                        <StyledTabPanel>
                            <ProjectOverviewTab />
                        </StyledTabPanel>
                        <StyledTabPanel>
                            <ProjectCompareCasesTab />
                        </StyledTabPanel>
                        <StyledTabPanel>
                            <ProjectSettingsTab />
                        </StyledTabPanel>
                    </Panels>
                </Tabs>
            </Wrapper>
            <EditTechnicalInputModal
                toggleEditTechnicalInputModal={toggleEditTechnicalInputModal}
                isOpen={editTechnicalInputModalIsOpen ?? false}
            />
        </>
    )
}

export default ProjectView

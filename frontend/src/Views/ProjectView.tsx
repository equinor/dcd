import {
    Button,
    Progress,
    Tabs, Typography,
} from "@equinor/eds-core-react"
import React, {
    useEffect,
    useState,
} from "react"
import { useModuleCurrentContext } from "@equinor/fusion-framework-react-module-context"
import { useParams } from "react-router-dom"
import styled from "styled-components"
import { useAppContext } from "../context/AppContext"
import { GetProjectService } from "../Services/ProjectService"
import ProjectOverviewTab from "./Project/ProjectOverviewTab"
import ProjectCompareCasesTab from "./Project/ProjectCompareCasesTab"
import ProjectSettingsTab from "./Project/ProjectSettingsTab"
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
    padding: 1.5rem 2rem;
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
    margin: 2rem;
    display: flex;
    flex-direction: column;
`

const ProjectView = () => {
    const { currentContext } = useModuleCurrentContext()
    const { fusionContextId } = useParams<Record<string, string | undefined>>()
    const { project, setProject } = useAppContext()

    const [activeTab, setActiveTab] = React.useState(0)
    const [editTechnicalInputModalIsOpen, setEditTechnicalInputModalIsOpen] = useState<boolean>()
    const [isSaving, setIsSaving] = useState<boolean>()
    const [isLoading, setIsLoading] = useState<boolean>()
    const [isCreating, setIsCreating] = useState<boolean>()

    useEffect(() => {
        (async () => {
            try {
                setIsLoading(true)
                if (currentContext?.externalId) {
                    let res = await (await GetProjectService()).getProjectByID(currentContext?.externalId)
                    if (!res || res.id === "") {
                        setIsCreating(true)
                        res = await (await GetProjectService()).createProjectFromContextId(currentContext.id)
                    }

                    setProject(res)
                    setIsCreating(false)
                    setIsLoading(false)
                }
            } catch (error) {
                console.error(`[ProjectView] Error while fetching project. Context: ${fusionContextId}, Project: ${currentContext?.externalId}`, error)
            }
        })()
    }, [currentContext?.externalId])

    const toggleEditTechnicalInputModal = () => setEditTechnicalInputModalIsOpen(!editTechnicalInputModalIsOpen)

    if (isLoading || !project || project.id === "") {
        if (isCreating) {
            return (
                <>
                    <Progress.Circular size={16} color="primary" />
                    <p>Creating project</p>
                </>
            )
        }
        return (
            <>
                <Progress.Circular size={16} color="primary" />
                <p>Loading project</p>
            </>
        )
    }

    const handleSave = async () => {
        setIsSaving(true)
        const updatedProject = { ...project }
        const result = await (await GetProjectService()).updateProject(updatedProject)
        setIsSaving(false)
        setProject(result)
    }

    return (
        <>
            <TopWrapper>
                <PageTitle variant="h4">{project.name}</PageTitle>
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
                project={project}
                setProject={setProject}
            />
        </>
    )
}

export default ProjectView

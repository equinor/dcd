import {
    MouseEventHandler, useState,
    FormEventHandler,
} from "react"
import styled from "styled-components"
import {
    Button, Icon, Label, Typography, Divider,
} from "@equinor/eds-core-react"
import { add, archive } from "@equinor/eds-icons"
import TextArea from "@equinor/fusion-react-textarea/dist/TextArea"
import { getProjectPhaseName, getProjectCategoryName, unwrapProjectId } from "../../Utils/common"
import { WrapperColumn } from "../Asset/StyledAssetComponents"
import { GetProjectService } from "../../Services/ProjectService"
import { GetSTEAService } from "../../Services/STEAService"
import EditCaseModal from "../../Components/Case/EditCaseModal"
import CasesTable from "../../Components/Case/CasesTable"
import { useAppContext } from "../../context/AppContext"

const Wrapper = styled.div`
    margin: 20px 0;
    display: flex;
    flex-direction: column;
    gap: 10px;
`
const HeaderContainer = styled.div`
    display: flex;
    flex-direction: row;
    justify-content: space-between;
    align-items: center;
`

const DataDiv = styled.div`
    display: flex;
    justify-content: space-between;
    margin-bottom: 10px;
 `
const DescriptionDiv = styled.div`

`
const ProjectDataFieldLabel = styled(Typography)`
    margin-right: 0.5rem;
    font-weight: bold;
    white-space: pre-wrap;
`
const DescriptionField = styled(TextArea)`
  
`

const ProjectOverviewTab = () => {
    const { project, setProject } = useAppContext()
    const [createCaseModalIsOpen, setCreateCaseModalIsOpen] = useState<boolean>(false)
    const toggleCreateCaseModal = () => setCreateCaseModalIsOpen(!createCaseModalIsOpen)

    const handleDescriptionChange: FormEventHandler = (e) => {
        const target = e.target as typeof e.target & {
            value: string
        }
        if (project) {
            const updatedProject = { ...project, description: target.value }
            setProject(updatedProject)
        }
    }

    const submitToSTEA: MouseEventHandler<HTMLButtonElement> = async (e) => {
        e.preventDefault()

        if (project) {
            try {
                const projectId = unwrapProjectId(project.id)
                const projectResult = await (await GetProjectService()).getProject(projectId)
                await (await GetSTEAService()).excelToSTEA(projectResult)
            } catch (error) {
                console.error("[ProjectView] error while submitting form data", error)
            }
        }
    }

    if (!project) {
        return <div>Loading project data...</div>
    }

    return (
        <Wrapper>
            <EditCaseModal
                isOpen={createCaseModalIsOpen}
                toggleModal={toggleCreateCaseModal}
                editMode={false}
                shouldNavigate={false}
            />
            <HeaderContainer>
                <Typography variant="h3">Project Overview</Typography>
                <Button onClick={submitToSTEA}>
                    <Icon data={archive} size={18} />
                    Export to STEA
                </Button>
            </HeaderContainer>
            <Wrapper>
                <DataDiv>
                    <div>
                        <ProjectDataFieldLabel>Project Phase:</ProjectDataFieldLabel>
                        <Typography aria-label="Project phase">
                            {getProjectPhaseName(project.projectPhase)}
                        </Typography>
                    </div>
                    <div>
                        <ProjectDataFieldLabel>Project Category:</ProjectDataFieldLabel>
                        <Typography aria-label="Project category">
                            {getProjectCategoryName(project.projectCategory)}
                        </Typography>
                    </div>
                    <div>
                        <ProjectDataFieldLabel>Country:</ProjectDataFieldLabel>
                        <Typography aria-label="Country">
                            {project.country ?? "Not defined in Common Library"}
                        </Typography>
                    </div>
                </DataDiv>
                <DescriptionDiv>
                    <WrapperColumn>
                        <Label htmlFor="description" label="Project description" />
                        <DescriptionField
                            id="description"
                            placeholder="Enter a description"
                            onInput={handleDescriptionChange}
                            value={project.description ?? undefined}
                            cols={100}
                            rows={8}
                        />
                    </WrapperColumn>
                </DescriptionDiv>
            </Wrapper>
            <HeaderContainer>
                <Typography variant="h3">Cases</Typography>
                <Button onClick={toggleCreateCaseModal}>
                    <Icon data={add} size={24} />
                    Add new Case
                </Button>
            </HeaderContainer>
            <CasesTable />
        </Wrapper>
    )
}

export default ProjectOverviewTab

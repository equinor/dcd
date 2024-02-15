import {
    MouseEventHandler, useState,
    FormEventHandler,
} from "react"
import styled from "styled-components"
import {
    Button, Icon, Label, Typography,
} from "@equinor/eds-core-react"
import { add, archive } from "@equinor/eds-icons"
import TextArea from "@equinor/fusion-react-textarea/dist/TextArea"
import { getProjectPhaseName, getProjectCategoryName, unwrapProjectId } from "../../Utils/common"
import { WrapperColumn, WrapperRow } from "../Asset/StyledAssetComponents"
import { GetProjectService } from "../../Services/ProjectService"
import { GetSTEAService } from "../../Services/STEAService"
import EditCaseModal from "../../Components/Case/EditCaseModal"
import CasesTable from "../../Components/Case/CasesTable"
import { useAppContext } from "../../context/AppContext"

const Wrapper = styled.div`
    margin: 1rem;
    display: flex;
    flex-direction: column;
`
const RowWrapper = styled.div`
    margin: 1rem;
    display: flex;
    flex-direction: row;
    justify-content: space-between;
`
const StyledButton = styled(Button)`
    color: white;
    background-color: #007079;
`
const DataDiv = styled.div`

 `
const DescriptionDiv = styled.div`
    width: 42.875rem;
    display: flex;
    flex-wrap: wrap;
    @media screen and (max-width: 1390px) {
    margin-right: 1.875rem;
  }
`
const Header = styled.header`
    display: flex;
    align-items: center;

    > *:first-child {
        margin-right: 2rem;
    }
`
const ProjectDataFieldLabel = styled(Typography)`
    margin-right: 0.5rem;
    font-weight: bold;
    white-space: pre-wrap;
`
const DescriptionField = styled(TextArea)`
    width: 60rem;
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
                const projectResult = await (await GetProjectService()).getProjectByID(projectId)
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
            <Header>
                <StyledButton
                    onClick={submitToSTEA}
                >
                    <Icon data={archive} />
                    Export to STEA
                </StyledButton>
            </Header>
            <RowWrapper>
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
                <DataDiv>
                    <WrapperRow>
                        <ProjectDataFieldLabel>Project Phase:</ProjectDataFieldLabel>
                        <Typography aria-label="Project phase">
                            {getProjectPhaseName(project.projectPhase)}
                        </Typography>
                    </WrapperRow>
                    <WrapperRow>
                        <ProjectDataFieldLabel>Project Category:</ProjectDataFieldLabel>
                        <Typography aria-label="Project category">
                            {getProjectCategoryName(project.projectCategory)}
                        </Typography>
                    </WrapperRow>
                    <WrapperRow>
                        <ProjectDataFieldLabel>Country:</ProjectDataFieldLabel>
                        <Typography aria-label="Country">
                            {project.country ?? "Not defined in Common Library"}
                        </Typography>
                    </WrapperRow>
                    <WrapperRow>
                        <ProjectDataFieldLabel>Description:</ProjectDataFieldLabel>
                        <Typography aria-label="Country">
                            {project.description ?? "empty"}
                        </Typography>
                    </WrapperRow>
                </DataDiv>
            </RowWrapper>
            <EditCaseModal
                isOpen={createCaseModalIsOpen}
                toggleModal={toggleCreateCaseModal}
                editMode={false}
                shouldNavigate={false}
            />
            <RowWrapper>
                <Typography variant="h2">Cases</Typography>
                <StyledButton onClick={toggleCreateCaseModal}>
                    <Icon data={add} />
                    Add new Case
                </StyledButton>

            </RowWrapper>
            <CasesTable />
        </Wrapper>
    )
}

export default ProjectOverviewTab

/* eslint-disable camelcase */
import React, {
    MouseEventHandler, useState,
    ChangeEventHandler,
    Dispatch,
    SetStateAction,
} from "react"
import styled from "styled-components"
import {
    Button, Icon, TextField, Typography,
} from "@equinor/eds-core-react"
import { add, archive } from "@equinor/eds-icons"
import { useHistory } from "react-router"
import { GetProjectPhaseName, GetProjectCategoryName, unwrapProjectId } from "../Utils/common"
import { WrapperColumn, WrapperRow } from "./Asset/StyledAssetComponents"
import { Project } from "../models/Project"
import { GetProjectService } from "../Services/ProjectService"
import { GetSTEAService } from "../Services/STEAService"
import { Modal } from "../Components/Modal"
import { GetCaseService } from "../Services/CaseService"
import CasesTableView from "./CasesTableView"

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

const CreateCaseForm = styled.form`
    width: 30rem;

    > * {
        margin-bottom: 1.5rem;
    }
`

interface Props {
    project: Project,
    setProject: Dispatch<SetStateAction<Project | undefined>>
}

function OverviewView({
    project, setProject,
}: Props) {
    const [createCaseModalIsOpen, setCreateCaseModalIsOpen] = useState<boolean>(false)
    const [caseName, setCaseName] = useState<string>("")
    const [caseDescription, setCaseDescription] = useState<string>("")
    // const navigate = useNavigate()
    const history = useHistory()
    const toggleCreateCaseModal = () => setCreateCaseModalIsOpen(!createCaseModalIsOpen)

    const handleCaseNameChange: ChangeEventHandler<HTMLInputElement> = (e) => {
        const { value } = e.target
        setCaseName(value)
    }

    const handleDescriptionChange: ChangeEventHandler<HTMLInputElement> = (e) => {
        const { value } = e.target
        setCaseDescription(value)
    }
    const submitToSTEA: MouseEventHandler<HTMLButtonElement> = async (e) => {
        e.preventDefault()

        try {
            const projectId: string = unwrapProjectId(project.projectId)
            const projectResult: Project = await (await GetProjectService()).getProjectByID(projectId)
            const result = (await GetSTEAService()).excelToSTEA(projectResult)
        } catch (error) {
            console.error("[ProjectView] error while submitting form data", error)
        }
    }

    const submitCreateCaseForm: MouseEventHandler<HTMLButtonElement> = async (e) => {
        e.preventDefault()

        try {
            const projectResult: Project = await (await GetCaseService()).createCase({
                description: caseDescription,
                name: caseName,
                projectId: project.projectId,
            })
            toggleCreateCaseModal()
            history.push(`/${projectResult.id}/case/${projectResult.cases.find((o) => (
                o.name === caseName
            ))?.id}`)
        } catch (error) {
            console.error("[ProjectView] error while submitting form data", error)
        }
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
                        <ProjectDataFieldLabel>Description:</ProjectDataFieldLabel>
                        <Typography variant="h3">{project.description}</Typography>
                    </WrapperColumn>
                </DescriptionDiv>
                <DataDiv>
                    <WrapperRow>
                        <ProjectDataFieldLabel>Project Phase:</ProjectDataFieldLabel>
                        <Typography aria-label="Project phase">
                            {GetProjectPhaseName(project.phase)}
                        </Typography>
                    </WrapperRow>
                    <WrapperRow>
                        <ProjectDataFieldLabel>Project Category:</ProjectDataFieldLabel>
                        <Typography aria-label="Project category">
                            {GetProjectCategoryName(project.category)}
                        </Typography>
                    </WrapperRow>
                    <WrapperRow>
                        <ProjectDataFieldLabel>Country:</ProjectDataFieldLabel>
                        <Typography aria-label="Country">
                            {project.country ?? "Not defined in Common Library"}
                        </Typography>
                    </WrapperRow>
                </DataDiv>
            </RowWrapper>
            <Modal isOpen={createCaseModalIsOpen} title="Create a case" shards={[]}>
                <CreateCaseForm>
                    <TextField
                        label="Name"
                        id="name"
                        name="name"
                        placeholder="Enter a name"
                        onChange={handleCaseNameChange}
                    />

                    <TextField
                        label="Description"
                        id="description"
                        name="description"
                        placeholder="Enter a description"
                        onChange={handleDescriptionChange}
                    />
                    <div>
                        <Button
                            type="submit"
                            onClick={submitCreateCaseForm}
                            disabled={caseName === "" || caseDescription === ""}
                        >
                            Create case
                        </Button>
                        <Button
                            type="button"
                            color="secondary"
                            variant="ghost"
                            onClick={toggleCreateCaseModal}
                        >
                            Cancel
                        </Button>
                    </div>
                </CreateCaseForm>
            </Modal>
            <RowWrapper>
                <Typography variant="h2">Cases</Typography>
                <StyledButton onClick={toggleCreateCaseModal}>
                    <Icon data={add} />
                    Add new Case
                </StyledButton>

            </RowWrapper>
            <CasesTableView project={project} setProject={setProject} />
        </Wrapper>
    )
}

export default OverviewView

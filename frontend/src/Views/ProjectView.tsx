// eslint-disable-next-line camelcase
import { add, archive } from "@equinor/eds-icons"
import {
    Button,
    EdsProvider,
    Icon,
    TextField,
    Tooltip,
    Typography,
} from "@equinor/eds-core-react"
import {
    ChangeEventHandler,
    MouseEventHandler,
    useEffect,
    useMemo,
    useState,
} from "react"
import { useParams, useNavigate } from "react-router-dom"
import styled from "styled-components"

import BarChart from "../Components/BarChart"

import { Project } from "../models/Project"
import { GetProjectService } from "../Services/ProjectService"

import { Modal } from "../Components/Modal"
import { GetCaseService } from "../Services/CaseService"

import { GetSTEAService } from "../Services/STEAService"

const Wrapper = styled.div`
    margin: 2rem;
    display: flex;
    flex-direction: column;
`

const Header = styled.header`
    display: flex;
    align-items: center;

    > *:first-child {
        margin-right: 2rem;
    }
`

const ActionsContainer = styled.div`
    > *:not(:last-child) {
        margin-right: 0.5rem;
    }
`

const ChartsContainer = styled.div`
    display: flex;
`

const CreateCaseForm = styled.form`
    width: 30rem;

    > * {
        margin-bottom: 1.5rem;
    }
`

const ProjectView = () => {
    const navigate = useNavigate()
    const params = useParams()
    const [project, setProject] = useState<Project>()
    const [createCaseModalIsOpen, setCreateCaseModalIsOpen] = useState<boolean>(false)
    const [caseName, setCaseName] = useState<string>("")
    const [caseDescription, setCaseDescription] = useState<string>("")

    useEffect(() => {
        (async () => {
            try {
                const res = await GetProjectService().getProjectByID(params.projectId!)
                console.log("[ProjectView]", res)
                setProject(res)
            } catch (error) {
                console.error(`[ProjectView] Error while fetching project ${params.projectId}`, error)
            }
        })()
    }, [params.projectId])

    const chartData = useMemo(() => (project ? {
        x: project?.cases.map((c) => c.name ?? ""),
        y: project?.cases.map((c) => c.capex ?? 0),
    } : { x: [], y: [] }), [project])

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
            const projectResult: Project = await GetProjectService().getProjectByID(params.projectId!)
            GetSTEAService().excelToSTEA(projectResult)
        } catch (error) {
            console.error("[ProjectView] error while submitting form data", error)
        }
    }

    const submitCreateCaseForm: MouseEventHandler<HTMLButtonElement> = async (e) => {
        e.preventDefault()

        try {
            const projectResult = await GetCaseService().createCase({
                description: caseDescription,
                name: caseName,
                projectId: params.projectId,
            })
            toggleCreateCaseModal()
            navigate(`/project/${projectResult.id}/case/${projectResult.cases.find((o) => (
                o.name === caseName
            ))?.id}`)
        } catch (error) {
            console.error("[ProjectView] error while submitting form data", error)
        }
    }

    if (!project) return null

    return (
        <Wrapper>
            <Header>
                <Typography variant="h2">{project.name}</Typography>

                <EdsProvider density="compact">
                    <ActionsContainer>
                        <Tooltip title="Export to STEA">
                            <Button
                                variant="ghost_icon"
                                aria-label="Export to STEA"
                                onClick={submitToSTEA}
                            >
                                <Icon data={archive} />
                            </Button>
                        </Tooltip>
                        <Tooltip title="Add a case">
                            <Button variant="ghost_icon" aria-label="Add a case" onClick={toggleCreateCaseModal}>
                                <Icon data={add} />
                            </Button>
                        </Tooltip>
                    </ActionsContainer>
                </EdsProvider>
            </Header>

            <ChartsContainer>
                <BarChart data={chartData!} title="Capex / case" />
            </ChartsContainer>

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
        </Wrapper>
    )
}

export default ProjectView

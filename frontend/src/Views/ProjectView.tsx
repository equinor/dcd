// eslint-disable-next-line camelcase
import { add, delete_to_trash, edit } from "@equinor/eds-icons"
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

import { useTranslation } from "react-i18next"
import BarChart from "../Components/BarChart"

import { Project } from "../models/Project"
import { GetProjectService } from "../Services/ProjectService"

import { Modal } from "../Components/Modal"
import { CaseService } from "../Services/CaseService"

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
    const { t } = useTranslation()
    const navigate = useNavigate()
    const params = useParams()
    const [project, setProject] = useState<Project>()
    const [createCaseModalIsOpen, setCreateCaseModalIsOpen] = useState<boolean>(false)
    const [createCaseFormData, setCreateCaseFormData] = useState<Record<string, any>>({})
    const [submitIsDisabled, setSubmitIsDisabled] = useState<boolean>(false)

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

    const handleCreateCaseFormFieldChange: ChangeEventHandler<HTMLInputElement> = (e) => {
        setCreateCaseFormData({
            ...createCaseFormData,
            [e.target.name]: e.target.value,
        })
    }

    const submitCreateCaseForm: MouseEventHandler<HTMLButtonElement> = async (e) => {
        e.preventDefault()
        setSubmitIsDisabled(true)

        try {
            const projectResult = await CaseService.createCase({
                description: createCaseFormData.description,
                dG4Date: createCaseFormData.dg4Date,
                name: createCaseFormData.name,
                projectId: params.projectId,
            })
            setSubmitIsDisabled(false)
            toggleCreateCaseModal()
            navigate(`/project/${projectResult.id}/case/${projectResult.cases.find((o) => (
                o.name === createCaseFormData.name
            ))?.id}`)
        } catch (error) {
            setSubmitIsDisabled(false)
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
                        <Tooltip title={`${t("ProjectView.Edit")} ${project.name}`}>
                            <Button variant="ghost_icon" aria-label={`${t("ProjectView.Edit")} ${project.name}`}>
                                <Icon data={edit} />
                            </Button>
                        </Tooltip>
                        <Tooltip title={t("ProjectView.AddCase")}>
                            <Button variant="ghost_icon" aria-label="Add a case" onClick={toggleCreateCaseModal}>
                                <Icon data={add} />
                            </Button>
                        </Tooltip>
                        <Tooltip title={`Delete ${project.name}`}>
                            <Button variant="ghost_icon" color="danger" aria-label={`Delete ${project.name}`}>
                                {/* eslint-disable-next-line camelcase */}
                                <Icon data={delete_to_trash} />
                            </Button>
                        </Tooltip>
                    </ActionsContainer>
                </EdsProvider>
            </Header>

            <ChartsContainer>
                <BarChart data={chartData!} title={t("ProjectView.CapexCase")} />
            </ChartsContainer>

            <Modal isOpen={createCaseModalIsOpen} title={t("ProjectView.CreateCase")} shards={[]}>
                <CreateCaseForm>
                    <TextField
                        label={t("ProjectView.CreateCase")}
                        id="name"
                        name="name"
                        placeholder={t("ProjectView.EnterName")}
                        onChange={handleCreateCaseFormFieldChange}
                    />

                    <TextField
                        label={t("ProjectView.Description")}
                        id="description"
                        name="description"
                        placeholder={t("ProjectView.EnterDescription")}
                        onChange={handleCreateCaseFormFieldChange}
                    />

                    <div>
                        <Button
                            type="submit"
                            onClick={submitCreateCaseForm}
                            disabled={submitIsDisabled}
                        >
                            {t("ProjectView.CreateCase")}
                        </Button>
                        <Button
                            type="button"
                            color="secondary"
                            variant="ghost"
                            onClick={toggleCreateCaseModal}
                        >
                            {t("ProjectView.Cancel")}
                        </Button>
                    </div>
                </CreateCaseForm>
            </Modal>
        </Wrapper>
    )
}

export default ProjectView

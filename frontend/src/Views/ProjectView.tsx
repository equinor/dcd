/* eslint-disable @typescript-eslint/no-unused-vars */
// eslint-disable-next-line camelcase
import { add, archive } from "@equinor/eds-icons"
import {
    Button,
    Checkbox,
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

import {
    Chart as ChartJS,
    CategoryScale,
    LinearScale,
    PointElement,
    LineElement,
    Title,
    Tooltip as ChartTooltip,
    Legend,
} from "chart.js"
import { min, maxBy } from "lodash"
import { start } from "repl"
import BarChart from "../Components/BarChart"

import { Project } from "../models/Project"
import { GetProjectService } from "../Services/ProjectService"

import { Modal } from "../Components/Modal"
import { GetCaseService } from "../Services/CaseService"

import { GetSTEAService } from "../Services/STEAService"
import { unwrapProjectId, GetProjectCategoryName, GetProjectPhaseName } from "../Utils/common"
import { WrapperColumn } from "./Asset/StyledAssetComponents"
import PhysicalUnit from "../Components/PhysicalUnit"
import Currency from "../Components/Currency"
import { Case } from "../models/Case"
import ManniDataTable from "../Components/ManniDataTable"

const Wrapper = styled.div`
    margin: 2rem;
    display: flex;
    flex-direction: column;
`

const UnstyledList = styled.ul`
    margin: 0;
    padding: 0;
    list-style-type: none;
  `

const Header = styled.header`
    display: flex;
    align-items: center;

    > *:first-child {
        margin-right: 2rem;
    }
`

const ProjectDataFieldLabel = styled(Typography)`
    margin-top: 1rem;
    font-weight: bold;
    white-space: pre-wrap;
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
    const [physicalUnit, setPhysicalUnit] = useState<Components.Schemas.PhysUnit>(0)
    const [currency, setCurrency] = useState<Components.Schemas.PhysUnit>(0)
    const [x, setX] = useState<number[]>()
    const [y, setY] = useState<number[][]>()
    const [caseTitles, setCaseTitles] = useState<string[]>()

    useEffect(() => {
        (async () => {
            try {
                const projectId: string = unwrapProjectId(params.projectId)
                const res: Project = await GetProjectService().getProjectByID(projectId)
                if (res !== undefined) {
                    setPhysicalUnit(res?.physUnit)
                    setCurrency(res?.currency)
                }
                console.log("[ProjectView]", res)
                setProject(res)
            } catch (error) {
                console.error(`[ProjectView] Error while fetching project ${params.projectId}`, error)
            }
        })()
    }, [params.projectId])

    useEffect(() => {
        (async () => {
            try {
                if (project !== undefined) {
                    const projectDto = Project.Copy(project)
                    projectDto.physUnit = physicalUnit
                    projectDto.currency = currency
                    projectDto.projectId = params.projectId!
                    const cases: Case[] = []
                    project.cases.forEach((c) => cases.push(Case.Copy(c)))
                    projectDto.cases = cases
                    const res: Project = await GetProjectService().updateProject(projectDto)
                    setProject(res)
                }
            } catch (error) {
                console.error(`[ProjectView] Error while fetching project ${params.projectId}`, error)
            }
        })()
    }, [physicalUnit, currency])

    const generateChartData = useMemo(() => {
        const xx: number[] = []
        const yy: number[][] = []
        const caseeTitles: string[] = []
        project?.cases.forEach((casee) => {
            if (casee.capexYear?.startYear !== null) {
                xx.push(casee.capexYear?.startYear!)
            }
            if (casee.capexYear?.values?.length !== 0) {
                yy.push(casee.capexYear?.values!)
                caseeTitles?.push(casee.name!)
            }
        })

        setX(xx)
        setY(yy)
        setCaseTitles(caseeTitles)
    }, [project])

    const drainValues: number [] | undefined = project?.drainageStrategies[0].co2Emissions?.values

    const chartData = useMemo(() => (project ? {
        x: project?.cases.map((c) => c.name ?? ""),
        y: project?.cases.map((c) => c.capex ?? 0),
    } : { x: [], y: [] }), [project])

    // Data for grafen
    const chartDataManni = useMemo(() => (project ? {
        x: project?.cases.map((c) => c.capexYear?.startYear ?? undefined),
        y: project?.cases.map((c) => c.capexYear?.values ?? undefined),
    } : { x: [], y: [][0] }), [project])

    const facilitiesAvailabilityChartData = useMemo(() => (project ? {
        x: project?.cases.map((c) => c.name ?? ""),
        y: project?.cases.map((c) => c.facilitiesAvailability ?? 0),
    } : { x: [], y: [] }), [project])

    const gasInjectorChartData = useMemo(() => (project ? {
        x: project?.cases.map((c) => c.name ?? ""),
        y: project?.cases.map((c) => c.gasInjectorCount ?? 0),
    } : { x: [], y: [] }), [project])

    const producerCountChartData = useMemo(() => (project ? {
        x: project?.cases.map((c) => c.name ?? ""),
        y: project?.cases.map((c) => c.producerCount ?? 0),
    } : { x: [], y: [] }), [project])

    const waterInjectorChartData = useMemo(() => (project ? {
        x: project?.cases.map((c) => c.name ?? ""),
        y: project?.cases.map((c) => c.waterInjectorCount ?? 0),
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
            const projectId: string = unwrapProjectId(params.projectId)
            const projectResult: Project = await GetProjectService().getProjectByID(projectId)
            GetSTEAService().excelToSTEA(projectResult)
        } catch (error) {
            console.error("[ProjectView] error while submitting form data", error)
        }
    }

    const submitCreateCaseForm: MouseEventHandler<HTMLButtonElement> = async (e) => {
        e.preventDefault()

        try {
            const projectResult: Project = await GetCaseService().createCase({
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
            <WrapperColumn>
                <ProjectDataFieldLabel>Description:</ProjectDataFieldLabel>
                <Typography variant="h3">{project.description}</Typography>
            </WrapperColumn>
            <WrapperColumn>
                <ProjectDataFieldLabel>Project Phase:</ProjectDataFieldLabel>
                <Typography variant="h4" aria-label="Project phase">
                    {GetProjectPhaseName(project.phase)}
                </Typography>
            </WrapperColumn>
            <WrapperColumn>
                <ProjectDataFieldLabel>Project Category:</ProjectDataFieldLabel>
                <Typography variant="h4" aria-label="Project category">
                    {GetProjectCategoryName(project.category)}
                </Typography>
            </WrapperColumn>
            <WrapperColumn>
                <ProjectDataFieldLabel>Country:</ProjectDataFieldLabel>
                <Typography variant="h4" aria-label="Country">
                    {project.country ?? "Not defined in Common Library"}
                </Typography>
            </WrapperColumn>
            <PhysicalUnit
                currentValue={physicalUnit}
                setPhysicalUnit={setPhysicalUnit}
                setProject={setProject}
                project={project}
            />

            <Currency
                currentValue={currency}
                setCurrency={setCurrency}
                setProject={setProject}
                project={project}
            />

            <fieldset>
                <legend>
                    We are in this together!
                    <span role="img" aria-label="raising hands emoji">
                        🙌
                    </span>
                </legend>
                <UnstyledList>
                    <li>
                        <Checkbox label="Check me first" name="multiple" value="first" />
                    </li>
                    <li>
                        <Checkbox label="Check me second" name="multiple" value="second" />
                    </li>
                    <li>
                        <Checkbox label="Check me third" name="multiple" value="third" />
                    </li>
                </UnstyledList>
            </fieldset>
            <ManniDataTable x={x!} y={y!} caseTitles={caseTitles!} />

            <ChartsContainer>
                <BarChart data={chartData!} title="Capex / case" />
                <BarChart data={facilitiesAvailabilityChartData!} title="Facilities Availability / case" />
            </ChartsContainer>
            <ChartsContainer>
                <BarChart data={gasInjectorChartData!} title="Gas Injector Count / case" />
                <BarChart data={producerCountChartData!} title="Producer Count / case" />
            </ChartsContainer>
            <ChartsContainer>
                <BarChart data={waterInjectorChartData!} title="Water Injector Count / case" />
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

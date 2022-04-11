import {
    Button, Input, Label, Typography,
} from "@equinor/eds-core-react"
import { ChangeEventHandler, useEffect, useState } from "react"
import {
    useLocation, useNavigate, useParams,
} from "react-router"
import styled from "styled-components"
import { Exploration } from "../models/assets/exploration/Exploration"
import { Case } from "../models/Case"
import { Project } from "../models/Project"
import { GetProjectService } from "../Services/ProjectService"
import { GetExplorationService } from "../Services/ExplorationService"
import TimeSeriesEnum from "../models/assets/TimeSeriesEnum"
import TimeSeries from "../Components/TimeSeries"
import { emptyGuid } from "../Utils/constants"

const AssetHeader = styled.div`
    margin-bottom: 2rem;
    display: flex;

    > *:first-child {
        margin-right: 2rem;
    }
`

const AssetViewDiv = styled.div`
    margin: 2rem;
    display: flex;
    flex-direction: column;
`

const Wrapper = styled.div`
    display: flex;
    flex-direction: row;
`

const WrapperColumn = styled.div`
    display: flex;
    flex-direction: column;
`

const SaveButton = styled(Button)`
    margin-top: 5rem;
    margin-left: 2rem;
    &:disabled {
        margin-left: 2rem;
        margin-top: 5rem;
    }
`

const Dg4Field = styled.div`
    margin-left: 1rem;
    margin-bottom: 2rem;
    width: 10rem;
    display: flex;
`

const ExplorationView = () => {
    const [, setProject] = useState<Project>()
    const [caseItem, setCase] = useState<Case>()
    const [exploration, setExploration] = useState<Exploration>()
    const [hasChanges, setHasChanges] = useState(false)
    const [name, setName] = useState<string>("")
    const params = useParams()
    const navigate = useNavigate()
    const location = useLocation()

    useEffect(() => {
        (async () => {
            try {
                const projectResult = await GetProjectService().getProjectByID(params.projectId!)
                setProject(projectResult)
                const caseResult = projectResult.cases.find((o) => o.id === params.caseId)
                setCase(caseResult)
                let newExploration = projectResult.explorations.find((s) => s.id === params.explorationId)
                if (newExploration !== undefined) {
                    setExploration(newExploration)
                } else {
                    newExploration = new Exploration()
                    setExploration(newExploration)
                }
                setName(newExploration?.name!)
            } catch (error) {
                console.error(`[CaseView] Error while fetching project ${params.projectId}`, error)
            }
        })()
    }, [params.projectId, params.caseId])

    const handleSave = async () => {
        const explorationDto = new Exploration(exploration!)
        explorationDto.name = name
        if (exploration?.id === emptyGuid) {
            explorationDto.projectId = params.projectId
            const newProject = await GetExplorationService().createExploration(params.caseId!, explorationDto!)
            const newExploration = newProject.explorations.at(-1)
            const newUrl = location.pathname.replace(emptyGuid, newExploration!.id!)
            setProject(newProject)
            const newCase = newProject.cases.find((o) => o.id === params.caseId)
            setCase(newCase)
            setExploration(newExploration)
            navigate(`${newUrl}`, { replace: true })
        } else {
            const newProject = await GetExplorationService().updateExploration(explorationDto!)
            setProject(newProject)
            const newCase = newProject.cases.find((o) => o.id === params.caseId)
            setCase(newCase)
            const newExploration = newProject.explorations.find((s) => s.id === params.explorationId)
            setExploration(newExploration)
        }
        setHasChanges(false)
    }

    const handleNameChange: ChangeEventHandler<HTMLInputElement> = async (e) => {
        setName(e.target.value)
        if (e.target.value !== undefined && e.target.value !== "") {
            setHasChanges(true)
        } else {
            setHasChanges(false)
        }
    }

    return (
        <AssetViewDiv>
            <Typography variant="h2">Exploration</Typography>
            <AssetHeader>
                <WrapperColumn>
                    <Label htmlFor="name" label="Name" />
                    <Input
                        id="name"
                        name="name"
                        placeholder="Enter name"
                        value={name}
                        onChange={handleNameChange}
                    />
                </WrapperColumn>
            </AssetHeader>
            <Wrapper>
                <Typography variant="h4">DG4</Typography>
                <Dg4Field>
                    <Input disabled defaultValue={caseItem?.DG4Date?.toLocaleDateString("en-CA")} type="date" />
                </Dg4Field>
            </Wrapper>
            <TimeSeries
                timeSeries={exploration?.costProfile}
                caseItem={caseItem}
                setAsset={setExploration}
                setHasChanges={setHasChanges}
                asset={exploration}
                timeSeriesType={TimeSeriesEnum.costProfile}
                assetName={name}
                timeSeriesTitle="Cost profile"
            />
            <Wrapper><SaveButton disabled={!hasChanges} onClick={handleSave}>Save</SaveButton></Wrapper>
        </AssetViewDiv>
    )
}

export default ExplorationView

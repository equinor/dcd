import {
    Input, Label, Typography,
} from "@equinor/eds-core-react"
import { ChangeEventHandler, useEffect, useState } from "react"
import {
    useLocation, useNavigate, useParams,
} from "react-router"
import { Exploration } from "../models/assets/exploration/Exploration"
import { Case } from "../models/Case"
import { Project } from "../models/Project"
import { GetProjectService } from "../Services/ProjectService"
import { GetExplorationService } from "../Services/ExplorationService"
import TimeSeriesEnum from "../models/assets/TimeSeriesEnum"
import TimeSeries from "../Components/TimeSeries"
import { EMPTY_GUID } from "../Utils/constants"
import {
    AssetHeader, AssetViewDiv, Dg4Field, SaveButton, Wrapper, WrapperColumn,
} from "./Asset/StyledAssetComponents"

const ExplorationView = () => {
    const [project, setProject] = useState<Project>()
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
            } catch (error) {
                console.error(`[CaseView] Error while fetching project ${params.projectId}`, error)
            }
        })()
    }, [])

    useEffect(() => {
        (async () => {
            if (project !== undefined) {
                const caseResult = project.cases.find((o) => o.id === params.caseId)
                setCase(caseResult)
                let newExploration = project!.explorations.find((s) => s.id === params.explorationId)
                if (newExploration !== undefined) {
                    setExploration(newExploration)
                } else {
                    newExploration = new Exploration()
                    setExploration(newExploration)
                }
                setName(newExploration?.name!)
            }
        })()
    }, [project])

    const handleSave = async () => {
        const explorationDto = new Exploration(exploration!)
        explorationDto.name = name
        if (exploration?.id === EMPTY_GUID) {
            explorationDto.projectId = params.projectId
            const newProject = await GetExplorationService().createExploration(params.caseId!, explorationDto!)
            const newExploration = newProject.explorations.at(-1)
            const newUrl = location.pathname.replace(EMPTY_GUID, newExploration!.id!)
            navigate(`${newUrl}`)
            setProject(newProject)
        } else {
            const newProject = await GetExplorationService().updateExploration(explorationDto!)
            setProject(newProject)
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
                caseItem={caseItem}
                setAsset={setExploration}
                setHasChanges={setHasChanges}
                asset={exploration}
                timeSeriesType={TimeSeriesEnum.costProfile}
                assetName={name}
                timeSeriesTitle="Cost profile"
            />
            <Wrapper>
                <SaveButton disabled={!hasChanges} onClick={handleSave}>Save</SaveButton>
            </Wrapper>
        </AssetViewDiv>
    )
}

export default ExplorationView

import {
    Typography, Switch,
} from "@equinor/eds-core-react"
import { useEffect, useState } from "react"
import {
    useParams,
} from "react-router"
import { Exploration } from "../models/assets/exploration/Exploration"
import { Case } from "../models/case/Case"
import { Project } from "../models/Project"
import { GetProjectService } from "../Services/ProjectService"
import { GetExplorationService } from "../Services/ExplorationService"
import {
    AssetViewDiv, Wrapper,
} from "./Asset/StyledAssetComponents"
import Save from "../Components/Save"
import AssetName from "../Components/AssetName"
import { unwrapCase } from "../Utils/common"
import AssetTypeEnum from "../models/assets/AssetTypeEnum"
import { initializeFirstAndLastYear } from "./Asset/AssetHelper"
import NumberInput from "../Components/NumberInput"
import { ExplorationCostProfile } from "../models/assets/exploration/ExplorationCostProfile"
import { GAndGAdminCost } from "../models/assets/exploration/GAndAdminCost"
import TimeSeries from "../Components/TimeSeries"
import AssetCurrency from "../Components/AssetCurrency"
import { IAssetService } from "../Services/IAssetService"
import { Well } from "../models/Well"
import DrillingSchedules from "../Components/Well/DrillingSchedules"
import WellList from "../Components/Well/WellList"
import { ExplorationWell } from "../models/ExplorationWell"

const ExplorationView = () => {
    const [project, setProject] = useState<Project>()
    const [caseItem, setCase] = useState<Case>()
    const [exploration, setExploration] = useState<Exploration>()
    const [hasChanges, setHasChanges] = useState(false)
    const [name, setName] = useState<string>("")
    const { fusionProjectId, caseId, explorationId } = useParams<Record<string, string | undefined>>()
    const [firstTSYear, setFirstTSYear] = useState<number>()
    const [lastTSYear, setLastTSYear] = useState<number>()
    const [costProfile, setCostProfile] = useState<ExplorationCostProfile>()
    const [gAndGAdminCost, setGAndGAdminCost] = useState<GAndGAdminCost>()
    const [rigMobDemob, setRigMobDemob] = useState<number>()
    const [currency, setCurrency] = useState<Components.Schemas.Currency>(1)
    const [wellProjectWells, setWellProjectWells] = useState<ExplorationWell[] | null | undefined>()
    const [, setWells] = useState<Well[]>()

    const [explorationService, setExplorationService] = useState<IAssetService>()

    useEffect(() => {
        (async () => {
            try {
                const projectResult = await (await GetProjectService()).getProjectByID(fusionProjectId!)
                setProject(projectResult)
                const service = await GetExplorationService()
                setExplorationService(service)
            } catch (error) {
                console.error(`[CaseView] Error while fetching project ${fusionProjectId}`, error)
            }
        })()
    }, [])

    useEffect(() => {
        (async () => {
            if (project !== undefined) {
                const caseResult = unwrapCase(project.cases.find((o) => o.id === caseId))
                setCase(caseResult)
                setWells(project.wells)
                let newExploration = project.explorations.find((s) => s.id === explorationId)
                if (newExploration !== undefined) {
                    setExploration(newExploration)
                    setWellProjectWells(newExploration.explorationWells)
                } else {
                    newExploration = new Exploration()
                    newExploration.currency = project.currency
                    setExploration(newExploration)
                }
                setName(newExploration?.name!)
                setCurrency(newExploration.currency ?? 1)
                setRigMobDemob(newExploration.rigMobDemob)

                setCostProfile(newExploration.costProfile)
                setGAndGAdminCost(newExploration.gAndGAdminCost)

                if (caseResult?.DG4Date) {
                    initializeFirstAndLastYear(
                        caseResult?.DG4Date?.getFullYear(),
                        [newExploration.costProfile, newExploration.gAndGAdminCost],
                        setFirstTSYear,
                        setLastTSYear,
                    )
                }
            }
        })()
    }, [project])

    useEffect(() => {
        const newExploration: Exploration = { ...exploration }
        newExploration.rigMobDemob = rigMobDemob
        newExploration.costProfile = costProfile
        newExploration.gAndGAdminCost = gAndGAdminCost
        newExploration.currency = currency
        setExploration(newExploration)

        if (caseItem?.DG4Date) {
            initializeFirstAndLastYear(
                caseItem?.DG4Date?.getFullYear(),
                [costProfile, gAndGAdminCost],
                setFirstTSYear,
                setLastTSYear,
            )
        }
    }, [rigMobDemob, costProfile, gAndGAdminCost, currency])

    const overrideCostProfile = () => {
        if (costProfile) {
            const newCostProfile = { ...costProfile }
            newCostProfile.override = !costProfile?.override
            setCostProfile(newCostProfile)
            setHasChanges(true)
        }
    }

    if (!project) return null
    if (!exploration) return null

    return (
        <AssetViewDiv>
            <WellList project={project} asset={exploration} setProject={setProject} />

            <Wrapper>
                <Typography variant="h2">Exploration</Typography>
                <Save
                    name={name}
                    setHasChanges={setHasChanges}
                    hasChanges={hasChanges}
                    setAsset={setExploration}
                    setProject={setProject}
                    asset={exploration!}
                    assetService={explorationService!}
                    assetType={AssetTypeEnum.explorations}
                />
            </Wrapper>
            <AssetName
                setName={setName}
                name={name}
                setHasChanges={setHasChanges}
            />
            <AssetCurrency
                setCurrency={setCurrency}
                setHasChanges={setHasChanges}
                currentValue={currency}
            />
            <Wrapper>
                <NumberInput
                    setValue={setRigMobDemob}
                    value={rigMobDemob ?? 0}
                    setHasChanges={setHasChanges}
                    integer={false}
                    disabled={false}
                    label="Rig mob demob"
                />
            </Wrapper>
            <Switch
                label="Override generated cost profile"
                onClick={overrideCostProfile}
                checked={costProfile?.override ?? false}
            />
            <TimeSeries
                dG4Year={caseItem?.DG4Date?.getFullYear()}
                setTimeSeries={setCostProfile}
                setHasChanges={setHasChanges}
                timeSeries={costProfile}
                timeSeriesTitle={`Cost profile ${currency === 2 ? "(MUSD)" : "(MNOK)"}`}
                firstYear={firstTSYear!}
                lastYear={lastTSYear!}
                setFirstYear={setFirstTSYear!}
                setLastYear={setLastTSYear}
            />
            <TimeSeries
                dG4Year={caseItem?.DG4Date?.getFullYear()}
                setTimeSeries={setGAndGAdminCost}
                setHasChanges={setHasChanges}
                timeSeries={gAndGAdminCost}
                timeSeriesTitle={`G and g admin cost ${currency === 2 ? "(MUSD)" : "(MNOK)"}`}
                firstYear={firstTSYear!}
                lastYear={lastTSYear!}
                setFirstYear={setFirstTSYear!}
                setLastYear={setLastTSYear}
            />
            <Typography>Drilling schedules:</Typography>
            <DrillingSchedules
                setProject={setProject}
                assetWells={wellProjectWells}
                project={project}
                caseItem={caseItem!}
                firstYear={firstTSYear}
                lastYear={lastTSYear}
                setFirstYear={setFirstTSYear}
                setLastYear={setLastTSYear}
            />
        </AssetViewDiv>
    )
}

export default ExplorationView

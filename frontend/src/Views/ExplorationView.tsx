import {
    Typography,
} from "@equinor/eds-core-react"
import { useEffect, useState } from "react"
import {
    useParams,
} from "react-router"
import { Exploration } from "../models/assets/exploration/Exploration"
import { Case } from "../models/Case"
import { Project } from "../models/Project"
import { GetProjectService } from "../Services/ProjectService"
import { GetExplorationService } from "../Services/ExplorationService"
import {
    AssetViewDiv, Wrapper,
} from "./Asset/StyledAssetComponents"
import Save from "../Components/Save"
import AssetName from "../Components/AssetName"
import AssetTypeEnum from "../models/assets/AssetTypeEnum"
import { initializeFirstAndLastYear } from "./Asset/AssetHelper"
import NumberInput from "../Components/NumberInput"
import TimeSeriesNoAsset from "../Components/TimeSeriesNoAsset"
import { ExplorationCostProfile } from "../models/assets/exploration/ExplorationCostProfile"
import { ExplorationDrillingScheduleDto } from "../models/assets/exploration/ExplorationDrillingScheduleDto"
import { GAndGAdminCostDto } from "../models/assets/exploration/GAndAdminCostDto"

const ExplorationView = () => {
    const [project, setProject] = useState<Project>()
    const [caseItem, setCase] = useState<Case>()
    const [exploration, setExploration] = useState<Exploration>()
    const [hasChanges, setHasChanges] = useState(false)
    const [name, setName] = useState<string>("")
    const params = useParams()
    const [firstTSYear, setFirstTSYear] = useState<number>()
    const [lastTSYear, setLastTSYear] = useState<number>()
    const [costProfile, setCostProfile] = useState<ExplorationCostProfile>()
    const [drillingSchedule, setDrillingSchedule] = useState<ExplorationDrillingScheduleDto>()
    const [gAndGAdminCost, setGAndGAdminCost] = useState<GAndGAdminCostDto>()
    const [rigMobDemob, setRigMobDemob] = useState<number>()

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
                let newExploration: Exploration = project!.explorations.find((s) => s.id === params.explorationId)
                if (newExploration !== undefined) {
                    setExploration(newExploration)
                } else {
                    newExploration = new Exploration()
                    setExploration(newExploration)
                }
                setName(newExploration?.name!)

                setRigMobDemob(newExploration.rigMobDemob)

                setCostProfile(newExploration.costProfile)
                setDrillingSchedule(newExploration.drillingSchedule)
                setGAndGAdminCost(newExploration.gAndGAdminCost)

                if (caseResult?.DG4Date) {
                    initializeFirstAndLastYear(
                        caseResult?.DG4Date?.getFullYear(),
                        [newExploration.costProfile, newExploration.drillingSchedule, newExploration.gAndGAdminCost],
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
        newExploration.drillingSchedule = drillingSchedule
        newExploration.gAndGAdminCost = gAndGAdminCost
        setExploration(newExploration)

        if (caseItem?.DG4Date) {
            initializeFirstAndLastYear(
                caseItem?.DG4Date?.getFullYear(),
                [costProfile, drillingSchedule, gAndGAdminCost],
                setFirstTSYear,
                setLastTSYear,
            )
        }
    }, [rigMobDemob, costProfile, drillingSchedule, gAndGAdminCost])

    return (
        <AssetViewDiv>
            <Typography variant="h2">Exploration</Typography>
            <AssetName
                setName={setName}
                name={name}
                setHasChanges={setHasChanges}
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
            <TimeSeriesNoAsset
                dG4Year={caseItem?.DG4Date?.getFullYear()}
                setTimeSeries={setCostProfile}
                setHasChanges={setHasChanges}
                timeSeries={costProfile}
                timeSeriesTitle="Cost profile"
                firstYear={firstTSYear!}
                lastYear={lastTSYear!}
                setFirstYear={setFirstTSYear!}
                setLastYear={setLastTSYear}
            />
            <TimeSeriesNoAsset
                dG4Year={caseItem?.DG4Date?.getFullYear()}
                setTimeSeries={setDrillingSchedule}
                setHasChanges={setHasChanges}
                timeSeries={drillingSchedule}
                timeSeriesTitle="Drilling schedule"
                firstYear={firstTSYear!}
                lastYear={lastTSYear!}
                setFirstYear={setFirstTSYear!}
                setLastYear={setLastTSYear}
            />
            <TimeSeriesNoAsset
                dG4Year={caseItem?.DG4Date?.getFullYear()}
                setTimeSeries={setGAndGAdminCost}
                setHasChanges={setHasChanges}
                timeSeries={gAndGAdminCost}
                timeSeriesTitle="G and g admin cost"
                firstYear={firstTSYear!}
                lastYear={lastTSYear!}
                setFirstYear={setFirstTSYear!}
                setLastYear={setLastTSYear}
            />
            <Save
                name={name}
                setHasChanges={setHasChanges}
                hasChanges={hasChanges}
                setAsset={setExploration}
                setProject={setProject}
                asset={exploration!}
                assetService={GetExplorationService()}
                assetType={AssetTypeEnum.explorations}
            />
        </AssetViewDiv>
    )
}

export default ExplorationView

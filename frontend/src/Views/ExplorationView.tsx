import {
    Input, NativeSelect, Typography,
} from "@equinor/eds-core-react"
import { ChangeEvent, useEffect, useState } from "react"
import {
    useParams,
} from "react-router"
import { Exploration } from "../models/assets/exploration/Exploration"
import { Case } from "../models/Case"
import { Project } from "../models/Project"
import { GetProjectService } from "../Services/ProjectService"
import { GetExplorationService } from "../Services/ExplorationService"
import {
    AssetViewDiv, Dg4Field, Wrapper,
} from "./Asset/StyledAssetComponents"
import Save from "../Components/Save"
import AssetName from "../Components/AssetName"
import AssetTypeEnum from "../models/assets/AssetTypeEnum"
import { initializeFirstAndLastYear, TimeSeriesYears } from "./Asset/AssetHelper"
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
    const [earliestTimeSeriesYear, setEarliestTimeSeriesYear] = useState<number>()
    const [latestTimeSeriesYear, setLatestTimeSeriesYear] = useState<number>()
    const [wellType, setWellType] = useState<Components.Schemas.WellType>()
    const [costProfile, setCostProfile] = useState<ExplorationCostProfile>()
    const [drillingSchedule, setDrillingSchedule] = useState<ExplorationDrillingScheduleDto>()
    const [gAndGAdminCost, setGAndGAdminCost] = useState<GAndGAdminCostDto>()

    useEffect(() => {
        (async () => {
            try {
                const projectResult = await GetProjectService().getProjectByID(params.projectId!)
                setProject(projectResult)
                // (earliest, latest) = GetYears([]: ITimeseries)
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
                    newExploration.rigMobDemob = caseResult?.rigMobDemob
                    setExploration(newExploration)
                }
                setName(newExploration?.name!)

                setWellType(newExploration.wellType)

                setCostProfile(newExploration.costProfile)
                setDrillingSchedule(newExploration.drillingSchedule)
                setGAndGAdminCost(newExploration.gAndGAdminCost)

                if (caseResult?.DG4Date) {
                    console.log("Entered if in asset")
                    initializeFirstAndLastYear(
                        caseResult?.DG4Date?.getFullYear(),
                        [newExploration.costProfile, newExploration.drillingSchedule, newExploration.gAndGAdminCost],
                        setEarliestTimeSeriesYear,
                        setLatestTimeSeriesYear,
                    )
                }

                // TimeSeriesYears(
                //     newExploration,
                //     caseResult!.DG4Date!.getFullYear(),
                //     setEarliestTimeSeriesYear,
                //     setLatestTimeSeriesYear,
                // )
            }
        })()
    }, [project])

    useEffect(() => {
        const newExploration: Exploration = { ...exploration }
        newExploration.wellType = wellType
        newExploration.costProfile = costProfile
        newExploration.drillingSchedule = drillingSchedule
        newExploration.gAndGAdminCost = gAndGAdminCost
        setExploration(newExploration)
    }, [wellType, costProfile, drillingSchedule, gAndGAdminCost])

    const onWellTypeChange = (event: ChangeEvent<HTMLSelectElement>) => {
        switch (event.currentTarget.selectedOptions[0].value) {
        case "0":
            setWellType(0)
            setHasChanges(true)
            break
        case "1":
            setWellType(1)
            setHasChanges(true)
            break
        default:
            break
        }
    }

    return (
        <AssetViewDiv>
            <Typography variant="h2">Exploration</Typography>
            <AssetName
                setName={setName}
                name={name}
                setHasChanges={setHasChanges}
            />
            <Wrapper>
                <Typography variant="h4">DG4</Typography>
                <Dg4Field>
                    <Input disabled defaultValue={caseItem?.DG4Date?.toLocaleDateString("en-CA")} type="date" />
                </Dg4Field>
            </Wrapper>
            <NativeSelect
                id="wellType"
                label="Well type"
                onChange={(event: ChangeEvent<HTMLSelectElement>) => onWellTypeChange(event)}
                value={wellType}
            >
                <option value={0}>Oil</option>
                <option value={1}>Gas</option>
            </NativeSelect>
            <Wrapper>
                <NumberInput
                    value={exploration?.rigMobDemob ?? 0}
                    setHasChanges={setHasChanges}
                    integer={false}
                    disabled
                    label="Rig Mob Demob"
                />
            </Wrapper>
            <TimeSeriesNoAsset
                dG4Year={caseItem?.DG4Date?.getFullYear()}
                setTimeSeries={setCostProfile}
                setHasChanges={setHasChanges}
                timeSeries={costProfile}
                timeSeriesTitle="Cost profile"
                earliestYear={earliestTimeSeriesYear!}
                latestYear={latestTimeSeriesYear!}
                setEarliestYear={setEarliestTimeSeriesYear!}
                setLatestYear={setLatestTimeSeriesYear}
            />
            <TimeSeriesNoAsset
                dG4Year={caseItem?.DG4Date?.getFullYear()}
                setTimeSeries={setDrillingSchedule}
                setHasChanges={setHasChanges}
                timeSeries={drillingSchedule}
                timeSeriesTitle="Drilling schedule"
                earliestYear={earliestTimeSeriesYear!}
                latestYear={latestTimeSeriesYear!}
                setEarliestYear={setEarliestTimeSeriesYear!}
                setLatestYear={setLatestTimeSeriesYear}
            />
            <TimeSeriesNoAsset
                dG4Year={caseItem?.DG4Date?.getFullYear()}
                setTimeSeries={setGAndGAdminCost}
                setHasChanges={setHasChanges}
                timeSeries={gAndGAdminCost}
                timeSeriesTitle="G and g admin cost"
                earliestYear={earliestTimeSeriesYear!}
                latestYear={latestTimeSeriesYear!}
                setEarliestYear={setEarliestTimeSeriesYear!}
                setLatestYear={setLatestTimeSeriesYear}
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

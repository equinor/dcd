import {
    Typography,
} from "@equinor/eds-core-react"
import { useEffect, useState } from "react"
import {
    useParams,
} from "react-router"
import styled from "styled-components"
import { useCurrentContext } from "@equinor/fusion"
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
import { GAndGAdminCost } from "../models/assets/exploration/GAndGAdminCost"
import TimeSeries from "../Components/TimeSeries"
import AssetCurrency from "../Components/AssetCurrency"
import ExplorationCaseAsset from "./ExplorationCaseAsset"
import { IAssetService } from "../Services/IAssetService"
import { DrillingSchedule } from "../models/assets/wellproject/DrillingSchedule"
import { GetCaseService } from "../Services/CaseService"
import ReadOnlyCostProfile from "../Components/ReadOnlyCostProfile"

const RowWrapper = styled.div`
    margin: 1rem;
    display: flex;
    flex-direction: row;
    justify-content: space-between;
`

interface Params {
    _project: Project,
    _case: Case
}

const ExplorationViewTab = ({
    _project,
    _case,
}: Params) => {
    const [project, setProject] = useState<Project>()
    const [caseItem, setCase] = useState<Case>()
    const [exploration, setExploration] = useState<Exploration>()
    const [hasChanges, setHasChanges] = useState(false)
    const [name, setName] = useState<string>("")
    const { fusionContextId, caseId, explorationId } = useParams<Record<string, string | undefined>>()
    const currentProject = useCurrentContext()
    const [firstTSYear, setFirstTSYear] = useState<number>()
    const [lastTSYear, setLastTSYear] = useState<number>()
    const [costProfile, setCostProfile] = useState<ExplorationCostProfile>()
    const [rigMobDemob, setRigMobDemob] = useState<number>()
    const [currency, setCurrency] = useState<Components.Schemas.Currency>(1)
    const [gAndGAdminCost, setGAndGAdminCost] = useState<GAndGAdminCost>()

    const [explorationService, setExplorationService] = useState<IAssetService>()
    useEffect(() => {
        (async () => {
            try {
                const projectResult = await (await GetProjectService()).getProjectByID(currentProject?.externalId!)
                setProject(projectResult)
                const service = await GetExplorationService()
                setExplorationService(service)
            } catch (error) {
                console.error(`[CaseView] Error while fetching project ${currentProject?.externalId}`, error)
            }
        })()
    }, [])

    useEffect(() => {
        (async () => {
            if (project !== undefined) {
                const caseResult = unwrapCase(project.cases.find((o) => o.id === caseId))
                setCase(caseResult)
                let newExploration = project.explorations.find((s) => s.id === explorationId)
                if (newExploration !== undefined) {
                    setExploration(newExploration)
                } else {
                    newExploration = new Exploration()
                    newExploration.currency = project.currency
                    setExploration(newExploration)
                }
                const generatedGAndGAdminCost = await (await GetCaseService()).generateGAndGAdminCost(caseResult.id!)
                setGAndGAdminCost(generatedGAndGAdminCost)
                setName(newExploration?.name!)
                setCurrency(newExploration.currency ?? 1)
                setRigMobDemob(newExploration.rigMobDemob)

                setCostProfile(newExploration.costProfile)

                if (caseResult?.DG4Date) {
                    initializeFirstAndLastYear(
                        caseResult?.DG4Date?.getFullYear(),
                        [newExploration.costProfile, gAndGAdminCost],
                        setFirstTSYear,
                        setLastTSYear,
                    )
                }
            }
        })()
    }, [project])

    useEffect(() => {
        (async () => {
            if (project !== undefined) {
                const caseResult = unwrapCase(project.cases.find((o) => o.id === caseId))
                setCase(caseResult)
                let newExploration = project.explorations.find((s) => s.id === caseResult.explorationLink)
                if (newExploration !== undefined) {
                    setExploration(newExploration)
                } else {
                    newExploration = new Exploration()
                    newExploration.currency = project.currency
                    setExploration(newExploration)
                }
                setName(newExploration?.name!)
                setCurrency(newExploration.currency ?? 1)
                setRigMobDemob(newExploration.rigMobDemob)

                setCostProfile(newExploration.costProfile)

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
    }, [_project, _case, project])

    useEffect(() => {
        const newExploration: Exploration = { ...exploration }
        newExploration.rigMobDemob = rigMobDemob
        newExploration.costProfile = costProfile
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

    if (!project || !exploration || !caseItem) { return null }

    return (
        <RowWrapper>
            <AssetViewDiv>
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
                <TimeSeries
                    dG4Year={caseItem.DG4Date!.getFullYear()}
                    setTimeSeries={setCostProfile}
                    setHasChanges={setHasChanges}
                    timeSeries={[costProfile]}
                    firstYear={firstTSYear!}
                    lastYear={lastTSYear!}
                    profileName={["Cost profile"]}
                    profileEnum={project?.currency!}
                    profileType="Cost"
                />
                <ReadOnlyCostProfile
                    dG4Year={caseItem?.DG4Date?.getFullYear()}
                    timeSeries={gAndGAdminCost}
                    title="G &amp; G and admin cost (MUSD)"
                />
            </AssetViewDiv>
            <Wrapper>
                <ExplorationCaseAsset
                    project={project!}
                    setProject={setProject}
                    caseItem={caseItem}
                    caseId={caseItem?.id}
                    setCase={setCase}
                />
            </Wrapper>
        </RowWrapper>
    )
}

export default ExplorationViewTab

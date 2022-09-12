import {
    Input, Switch, Typography,
} from "@equinor/eds-core-react"
import { useEffect, useState } from "react"
import {
    useParams,
} from "react-router"
import { useCurrentContext } from "@equinor/fusion"
import Save from "../Components/Save"
import AssetName from "../Components/AssetName"
import TimeSeries from "../Components/TimeSeries"
import { WellProject } from "../models/assets/wellproject/WellProject"
import { Case } from "../models/case/Case"
import { Project } from "../models/Project"
import { GetProjectService } from "../Services/ProjectService"
import { GetWellProjectService } from "../Services/WellProjectService"
import { unwrapCase, unwrapProjectId } from "../Utils/common"
import { initializeFirstAndLastYear } from "./Asset/AssetHelper"
import {
    AssetViewDiv, Dg4Field, Wrapper, WrapperColumn,
} from "./Asset/StyledAssetComponents"
import AssetTypeEnum from "../models/assets/AssetTypeEnum"
import NumberInput from "../Components/NumberInput"
import { WellProjectCostProfile } from "../models/assets/wellproject/WellProjectCostProfile"
import AssetCurrency from "../Components/AssetCurrency"
import { IAssetService } from "../Services/IAssetService"
import ArtificialLiftInherited from "../Components/ArtificialLiftInherited"
import { WellProjectWell } from "../models/WellProjectWell"
import { Well } from "../models/Well"
import DrillingSchedules from "../Components/Well/DrillingSchedules"
import WellList from "../Components/Well/WellList"

function WellProjectView() {
    const [project, setProject] = useState<Project>()
    const [caseItem, setCase] = useState<Case>()
    const [wellProject, setWellProject] = useState<WellProject>()
    const [hasChanges, setHasChanges] = useState(false)
    const [wellProjectName, setWellProjectName] = useState<string>("")
    const { fusionContextId, caseId, wellProjectId } = useParams<Record<string, string | undefined>>()
    const currentProject = useCurrentContext()

    const [firstTSYear, setFirstTSYear] = useState<number>()
    const [lastTSYear, setLastTSYear] = useState<number>()
    const [annualWellInterventionCost, setAnnualWellInterventionCost] = useState<number>()
    const [pluggingAndAbandonment, setPluggingAndAbandonment] = useState<number>()
    const [rigMobDemob, setRigMobDemob] = useState<number>()
    const [costProfile, setCostProfile] = useState<WellProjectCostProfile>()
    const [currency, setCurrency] = useState<Components.Schemas.Currency>(1)
    const [wellProjectService, setWellProjectService] = useState<IAssetService>()
    const [artificialLift, setArtificialLift] = useState<Components.Schemas.ArtificialLift | undefined>()
    const [wellProjectWells, setWellProjectWells] = useState<WellProjectWell[] | null | undefined>()
    const [, setWells] = useState<Well[]>()

    useEffect(() => {
        (async () => {
            try {
                const projectId = unwrapProjectId(currentProject?.externalId)
                const projectResult = await (await GetProjectService()).getProjectByID(projectId)
                setProject(projectResult)
                const service = await GetWellProjectService()
                setWellProjectService(service)
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
                setWells(project.wells)
                let newWellProject = project?.wellProjects.find((s) => s.id === wellProjectId)
                if (newWellProject !== undefined) {
                    setWellProject(newWellProject)
                    setWellProjectWells(newWellProject.wellProjectWells)
                } else {
                    newWellProject = new WellProject()
                    newWellProject.artificialLift = caseResult?.artificialLift
                    newWellProject.currency = project.currency
                    setWellProject(newWellProject)
                }
                setWellProjectName(newWellProject?.name!)

                setAnnualWellInterventionCost(newWellProject.annualWellInterventionCost)
                setPluggingAndAbandonment(newWellProject.pluggingAndAbandonment)
                setRigMobDemob(newWellProject.rigMobDemob)
                setCurrency(newWellProject.currency ?? 1)

                setCostProfile(newWellProject.costProfile)
                setArtificialLift(newWellProject.artificialLift)

                if (caseResult?.DG4Date) {
                    initializeFirstAndLastYear(
                        caseResult?.DG4Date?.getFullYear(),
                        [newWellProject.costProfile],
                        setFirstTSYear,
                        setLastTSYear,
                    )
                }
            }
        })()
    }, [project])

    useEffect(() => {
        const newWellProject: WellProject = { ...wellProject }
        newWellProject.annualWellInterventionCost = annualWellInterventionCost
        newWellProject.pluggingAndAbandonment = pluggingAndAbandonment
        newWellProject.rigMobDemob = rigMobDemob
        newWellProject.costProfile = costProfile
        newWellProject.currency = currency
        newWellProject.artificialLift = artificialLift
        if (caseItem?.DG4Date) {
            initializeFirstAndLastYear(
                caseItem?.DG4Date?.getFullYear(),
                [costProfile],
                setFirstTSYear,
                setLastTSYear,
            )
        }
        setWellProject(newWellProject)
    }, [annualWellInterventionCost, pluggingAndAbandonment, rigMobDemob, costProfile, currency,
        artificialLift])

    const overrideCostProfile = () => {
        if (costProfile) {
            const newCostProfile = { ...costProfile }
            newCostProfile.override = !costProfile?.override
            setCostProfile(newCostProfile)
            setHasChanges(true)
        }
    }

    if (!project) return null
    if (!wellProject) return null
    return (
        <AssetViewDiv>
            <WellList project={project} wellProject={wellProject} setProject={setProject} />

            <Wrapper>
                <Typography variant="h2">WellProject</Typography>
                <Save
                    name={wellProjectName}
                    setHasChanges={setHasChanges}
                    hasChanges={hasChanges}
                    setAsset={setWellProject}
                    setProject={setProject}
                    asset={wellProject!}
                    assetService={wellProjectService!}
                    assetType={AssetTypeEnum.wellProjects}
                />
            </Wrapper>
            <AssetName
                setName={setWellProjectName}
                name={wellProjectName}
                setHasChanges={setHasChanges}
            />
            <Wrapper>
                <Typography variant="h4">DG3</Typography>
                <Dg4Field>
                    <Input
                        disabled
                        defaultValue={caseItem?.DG3Date?.toLocaleDateString("en-CA")}
                        type="date"
                    />
                </Dg4Field>
                <Typography variant="h4">DG4</Typography>
                <Dg4Field>
                    <Input
                        disabled
                        defaultValue={caseItem?.DG4Date?.toLocaleDateString("en-CA")}
                        type="date"
                    />
                </Dg4Field>
            </Wrapper>
            <AssetCurrency
                setCurrency={setCurrency}
                setHasChanges={setHasChanges}
                currentValue={currency}
            />
            <Wrapper>
                <WrapperColumn>
                    <ArtificialLiftInherited
                        currentValue={artificialLift}
                        setArtificialLift={setArtificialLift}
                        setHasChanges={setHasChanges}
                        caseArtificialLift={caseItem?.artificialLift}
                    />
                </WrapperColumn>
            </Wrapper>
            <Wrapper>
                <NumberInput
                    setValue={setRigMobDemob}
                    value={rigMobDemob ?? 0}
                    setHasChanges={setHasChanges}
                    integer={false}
                    label="Rig mob demob"
                />
                <NumberInput
                    setValue={setAnnualWellInterventionCost}
                    value={annualWellInterventionCost ?? 0}
                    setHasChanges={setHasChanges}
                    integer={false}
                    label="Annual well intervention cost"
                />
                <NumberInput
                    setValue={setPluggingAndAbandonment}
                    value={pluggingAndAbandonment ?? 0}
                    setHasChanges={setHasChanges}
                    integer={false}
                    label="Plugging and abandonment"
                />
            </Wrapper>
            <Wrapper>
                <Switch
                    label="Override generated cost profile"
                    onClick={overrideCostProfile}
                    checked={costProfile?.override ?? false}
                />
            </Wrapper>
            <TimeSeries
                dG4Year={caseItem?.DG4Date?.getFullYear()}
                setTimeSeries={setCostProfile}
                setHasChanges={setHasChanges}
                timeSeries={[costProfile!]}
                firstYear={firstTSYear!}
                lastYear={lastTSYear!}
                profileName={["Cost profile"]}
                profileEnum={project?.physUnit!}
            />
            <Typography>Drilling schedules:</Typography>
            <DrillingSchedules
                setProject={setProject}
                wellProjectWells={wellProjectWells}
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

export default WellProjectView

import {
    Input, Label, Typography,
} from "@equinor/eds-core-react"
import { useEffect, useState } from "react"
import {
    useParams,
} from "react-router"
import Save from "../Components/Save"
import AssetName from "../Components/AssetName"
import TimeSeries from "../Components/TimeSeries"
import { WellProject } from "../models/assets/wellproject/WellProject"
import { Case } from "../models/Case"
import { Project } from "../models/Project"
import { GetProjectService } from "../Services/ProjectService"
import { GetWellProjectService } from "../Services/WellProjectService"
import { unwrapCase, unwrapProjectId } from "../Utils/common"
import { GetArtificialLiftName, initializeFirstAndLastYear } from "./Asset/AssetHelper"
import {
    AssetViewDiv, Dg4Field, Wrapper, WrapperColumn,
} from "./Asset/StyledAssetComponents"
import AssetTypeEnum from "../models/assets/AssetTypeEnum"
import NumberInput from "../Components/NumberInput"
import { DrillingSchedule } from "../models/assets/wellproject/DrillingSchedule"
import { WellProjectCostProfile } from "../models/assets/wellproject/WellProjectCostProfile"
import AssetCurrency from "../Components/AssetCurrency"

function WellProjectView() {
    const [project, setProject] = useState<Project>()
    const [caseItem, setCase] = useState<Case>()
    const [wellProject, setWellProject] = useState<WellProject>()
    const [hasChanges, setHasChanges] = useState(false)
    const [wellProjectName, setWellProjectName] = useState<string>("")
    const params = useParams()
    const [firstTSYear, setFirstTSYear] = useState<number>()
    const [lastTSYear, setLastTSYear] = useState<number>()
    const [annualWellInterventionCost, setAnnualWellInterventionCost] = useState<number>()
    const [pluggingAndAbandonment, setPluggingAndAbandonment] = useState<number>()
    const [rigMobDemob, setRigMobDemob] = useState<number>()
    const [costProfile, setCostProfile] = useState<WellProjectCostProfile>()
    const [drillingSchedule, setDrillingSchedule] = useState<DrillingSchedule>()
    const [currency, setCurrency] = useState<Components.Schemas.Currency>(0)

    useEffect(() => {
        (async () => {
            try {
                const projectId: string = unwrapProjectId(params.projectId)
                const projectResult: Project = await GetProjectService().getProjectByID(projectId)
                setProject(projectResult)
            } catch (error) {
                console.error(`[CaseView] Error while fetching project ${params.projectId}`, error)
            }
        })()
    }, [])

    useEffect(() => {
        (async () => {
            if (project !== undefined) {
                const caseResult: Case = unwrapCase(project.cases.find((o) => o.id === params.caseId))
                setCase(caseResult)
                // eslint-disable-next-line max-len
                let newWellProject: WellProject | undefined = project?.wellProjects.find((s) => s.id === params.wellProjectId)
                if (newWellProject !== undefined) {
                    setWellProject(newWellProject)
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
                setCurrency(newWellProject.currency ?? 0)

                setCostProfile(newWellProject.costProfile)
                setDrillingSchedule(newWellProject.drillingSchedule)

                if (caseResult?.DG4Date) {
                    initializeFirstAndLastYear(
                        caseResult?.DG4Date?.getFullYear(),
                        [newWellProject.costProfile, newWellProject.drillingSchedule],
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
        newWellProject.drillingSchedule = drillingSchedule
        newWellProject.currency = currency
        if (caseItem?.DG4Date) {
            initializeFirstAndLastYear(
                caseItem?.DG4Date?.getFullYear(),
                [costProfile, drillingSchedule],
                setFirstTSYear,
                setLastTSYear,
            )
        }
        setWellProject(newWellProject)
    }, [annualWellInterventionCost, pluggingAndAbandonment, rigMobDemob, costProfile, drillingSchedule, currency])

    return (
        <AssetViewDiv>
            <Typography variant="h2">WellProject</Typography>
            <AssetName
                setName={setWellProjectName}
                name={wellProjectName}
                setHasChanges={setHasChanges}
            />
            <Wrapper>
                <Typography variant="h4">DG3</Typography>
                <Dg4Field>
                    <Input disabled defaultValue={caseItem?.DG3Date?.toLocaleDateString("en-CA")} type="date" />
                </Dg4Field>
                <Typography variant="h4">DG4</Typography>
                <Dg4Field>
                    <Input disabled defaultValue={caseItem?.DG4Date?.toLocaleDateString("en-CA")} type="date" />
                </Dg4Field>
            </Wrapper>
            <AssetCurrency
                setCurrency={setCurrency}
                setHasChanges={setHasChanges}
                currentValue={currency}
            />
            <Wrapper>
                <WrapperColumn>
                    <Label htmlFor="name" label="Artificial lift" />
                    <Input
                        id="artificialLift"
                        disabled
                        defaultValue={GetArtificialLiftName(wellProject?.artificialLift)}
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
            <TimeSeries
                dG4Year={caseItem?.DG4Date?.getFullYear()}
                setTimeSeries={setCostProfile}
                setHasChanges={setHasChanges}
                timeSeries={costProfile}
                timeSeriesTitle={`Cost profile ${currency === 0 ? "(MUSD)" : "(MNOK)"}`}
                firstYear={firstTSYear!}
                lastYear={lastTSYear!}
                setFirstYear={setFirstTSYear!}
                setLastYear={setLastTSYear}
            />
            <TimeSeries
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
            <Save
                name={wellProjectName}
                setHasChanges={setHasChanges}
                hasChanges={hasChanges}
                setAsset={setWellProject}
                setProject={setProject}
                asset={wellProject!}
                assetService={GetWellProjectService()}
                assetType={AssetTypeEnum.wellProjects}
            />
        </AssetViewDiv>
    )
}

export default WellProjectView

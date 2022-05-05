import {
    Input, Label, Typography,
} from "@equinor/eds-core-react"
import { useEffect, useState } from "react"
import {
    useParams,
} from "react-router"
import Save from "../Components/Save"
import AssetName from "../Components/AssetName"
import { WellProject } from "../models/assets/wellproject/WellProject"
import { Case } from "../models/Case"
import { Project } from "../models/Project"
import { GetProjectService } from "../Services/ProjectService"
import { GetWellProjectService } from "../Services/WellProjectService"
import { GetArtificialLiftName, initializeFirstAndLastYear } from "./Asset/AssetHelper"
import {
    AssetViewDiv, Dg4Field, Wrapper, WrapperColumn,
} from "./Asset/StyledAssetComponents"
import AssetTypeEnum from "../models/assets/AssetTypeEnum"
import NumberInput from "../Components/NumberInput"
import { WellProjectCostProfile } from "../models/assets/wellproject/WellProjectCostProfile"
import { DrillingSchedule } from "../models/assets/wellproject/DrillingSchedule"
import TimeSeries from "../Components/TimeSeries"

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
                let newWellProject = project!.wellProjects.find((s) => s.id === params.wellProjectId)
                if (newWellProject !== undefined) {
                    setWellProject(newWellProject)
                } else {
                    newWellProject = new WellProject()
                    newWellProject.artificialLift = caseResult?.artificialLift
                    newWellProject.producerCount = caseResult?.producerCount
                    newWellProject.gasInjectorCount = caseResult?.gasInjectorCount
                    newWellProject.waterInjectorCount = caseResult?.waterInjectorCount
                    setWellProject(newWellProject)
                }
                setWellProjectName(newWellProject?.name!)

                setAnnualWellInterventionCost(newWellProject.annualWellInterventionCost)
                setPluggingAndAbandonment(newWellProject.pluggingAndAbandonment)
                setRigMobDemob(newWellProject.rigMobDemob)

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
        setWellProject(newWellProject)
    }, [annualWellInterventionCost, pluggingAndAbandonment, rigMobDemob])

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
                <NumberInput
                    value={wellProject?.producerCount ?? 0}
                    integer
                    disabled
                    label="Producer count"
                />
                <NumberInput
                    value={wellProject?.gasInjectorCount ?? 0}
                    integer
                    disabled
                    label="Gas injector count"
                />
                <NumberInput
                    value={wellProject?.waterInjectorCount ?? 0}
                    integer
                    disabled
                    label="Water injector count"
                />
            </Wrapper>
            <TimeSeries
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

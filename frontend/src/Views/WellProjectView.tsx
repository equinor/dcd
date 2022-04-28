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
import TimeSeriesEnum from "../models/assets/TimeSeriesEnum"
import { WellProject } from "../models/assets/wellproject/WellProject"
import { Case } from "../models/Case"
import { Project } from "../models/Project"
import { GetProjectService } from "../Services/ProjectService"
import { GetWellProjectService } from "../Services/WellProjectService"
import { GetArtificialLiftName, TimeSeriesYears } from "./Asset/AssetHelper"
import {
    AssetViewDiv, Dg4Field, Wrapper, WrapperColumn,
} from "./Asset/StyledAssetComponents"
import AssetTypeEnum from "../models/assets/AssetTypeEnum"
import NumberInput from "../Components/NumberInput"

function WellProjectView() {
    const [project, setProject] = useState<Project>()
    const [caseItem, setCase] = useState<Case>()
    const [wellProject, setWellProject] = useState<WellProject>()
    const [hasChanges, setHasChanges] = useState(false)
    const [wellProjectName, setWellProjectName] = useState<string>("")
    const params = useParams()
    const [earliestTimeSeriesYear, setEarliestTimeSeriesYear] = useState<number>()
    const [latestTimeSeriesYear, setLatestTimeSeriesYear] = useState<number>()
    const [rigMobDemob, setRigMobDemob] = useState<number>()
    const [annualWellInterventionCost, setAnnualWellInterventionCost] = useState<number>()
    const [pluggingAndAbandonment, setPluggingAndAbandonment] = useState<number>()

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
                    setWellProject(newWellProject)
                }
                setWellProjectName(newWellProject?.name!)

                setRigMobDemob(newWellProject.rigMobDemob)
                setAnnualWellInterventionCost(newWellProject.annualWellInterventionCost)
                setPluggingAndAbandonment(newWellProject.pluggingAndAbandonment)

                TimeSeriesYears(
                    newWellProject,
                    caseResult!.DG4Date!.getFullYear(),
                    setEarliestTimeSeriesYear,
                    setLatestTimeSeriesYear,
                )
            }
        })()
    }, [project])

    useEffect(() => {
        const newWellProject = { ...wellProject }
        newWellProject.rigMobDemob = rigMobDemob
        newWellProject.annualWellInterventionCost = annualWellInterventionCost
        newWellProject.pluggingAndAbandonment = pluggingAndAbandonment
        setWellProject(newWellProject)
    }, [rigMobDemob, annualWellInterventionCost, pluggingAndAbandonment])

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
                    <Label htmlFor="name" label="Artificial Lift" />
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
                    label="Rig Mob Demob"
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
                caseItem={caseItem}
                setAsset={setWellProject}
                setHasChanges={setHasChanges}
                asset={wellProject}
                timeSeriesType={TimeSeriesEnum.costProfile}
                assetName={wellProjectName}
                timeSeriesTitle="Cost profile"
                earliestYear={earliestTimeSeriesYear!}
                latestYear={latestTimeSeriesYear!}
                setEarliestYear={setEarliestTimeSeriesYear!}
                setLatestYear={setLatestTimeSeriesYear}
            />
            <TimeSeries
                caseItem={caseItem}
                setAsset={setWellProject}
                setHasChanges={setHasChanges}
                asset={wellProject}
                timeSeriesType={TimeSeriesEnum.drillingSchedule}
                assetName={wellProjectName}
                timeSeriesTitle="Drilling schedule"
                earliestYear={earliestTimeSeriesYear!}
                latestYear={latestTimeSeriesYear!}
                setEarliestYear={setEarliestTimeSeriesYear!}
                setLatestYear={setLatestTimeSeriesYear}
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

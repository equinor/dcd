import {
    Input, Switch, Typography,
} from "@equinor/eds-core-react"
import { useEffect, useState } from "react"
import {
    useParams,
} from "react-router-dom"
import { useCurrentContext } from "@equinor/fusion"
import Save from "../Components/Save"
import AssetName from "../Components/AssetName"
import TimeSeries from "../Components/TimeSeries"
import { WellProject } from "../models/assets/wellproject/WellProject"
import { Case } from "../models/case/Case"
import { Project } from "../models/Project"
import { GetProjectService } from "../Services/ProjectService"
import { GetWellProjectService } from "../Services/WellProjectService"
import { ToMonthDate, unwrapCase, unwrapProjectId } from "../Utils/common"
import { initializeFirstAndLastYear } from "./Asset/AssetHelper"
import {
    AssetViewDiv, Dg4Field, Wrapper, WrapperColumn,
} from "./Asset/StyledAssetComponents"
import AssetTypeEnum from "../models/assets/AssetTypeEnum"
import NumberInput from "../Components/NumberInput"
import AssetCurrency from "../Components/AssetCurrency"
import { IAssetService } from "../Services/IAssetService"
import ArtificialLiftInherited from "../Components/ArtificialLiftInherited"
import { WellProjectWell } from "../models/WellProjectWell"
import { Well } from "../models/Well"

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
                setCurrency(newWellProject.currency ?? 1)

                setArtificialLift(newWellProject.artificialLift)
            }
        })()
    }, [project])

    useEffect(() => {
        const newWellProject: WellProject = { ...wellProject }
        newWellProject.currency = currency
        newWellProject.artificialLift = artificialLift
        setWellProject(newWellProject)
    }, [annualWellInterventionCost, pluggingAndAbandonment, rigMobDemob, currency,
        artificialLift])

    if (!project) return null
    if (!wellProject || !caseItem) return null
    return (
        <AssetViewDiv>
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
                        defaultValue={ToMonthDate(caseItem?.DG3Date)}
                        type="month"
                    />
                </Dg4Field>
                <Typography variant="h4">DG4</Typography>
                <Dg4Field>
                    <Input
                        disabled
                        defaultValue={ToMonthDate(caseItem?.DG4Date)}
                        type="month"
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
        </AssetViewDiv>
    )
}

export default WellProjectView

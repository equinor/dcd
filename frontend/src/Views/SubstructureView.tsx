import {
    Input, Typography,
} from "@equinor/eds-core-react"
import { useEffect, useState } from "react"
import {
    useParams,
} from "react-router"
import TimeSeries from "../Components/TimeSeries"
import TimeSeriesEnum from "../models/assets/TimeSeriesEnum"
import { Substructure } from "../models/assets/substructure/Substructure"
import { Case } from "../models/Case"
import { Project } from "../models/Project"
import { GetProjectService } from "../Services/ProjectService"
import { GetSubstructureService } from "../Services/SubstructureService"
import {
    AssetViewDiv, Dg4Field, Wrapper,
} from "./Asset/StyledAssetComponents"
import Save from "../Components/Save"
import AssetName from "../Components/AssetName"
import { unwrapCase, unwrapCaseId, unwrapProjectId } from "../Utils/common"
import AssetTypeEnum from "../models/assets/AssetTypeEnum"
import { TimeSeriesYears } from "./Asset/AssetHelper"

const SubstructureView = () => {
    const [project, setProject] = useState<Project>()
    const [caseItem, setCase] = useState<Case>()
    const [substructure, setSubstructure] = useState<Substructure>()

    const [hasChanges, setHasChanges] = useState(false)
    const [substructureName, setSubstructureName] = useState<string>("")
    const params = useParams()
    const [earliestTimeSeriesYear, setEarliestTimeSeriesYear] = useState<number>()
    const [latestTimeSeriesYear, setLatestTimeSeriesYear] = useState<number>()

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
                let newSubstructure: Substructure | undefined = project.substructures.find((s) => s.id === params.substructureId)
                if (newSubstructure !== undefined) {
                    setSubstructure(newSubstructure)
                } else {
                    newSubstructure = new Substructure()
                    setSubstructure(newSubstructure)
                }
                setSubstructureName(newSubstructure?.name!)

                TimeSeriesYears(
                    newSubstructure,
                    caseResult?.DG4Date!.getFullYear(),
                    setEarliestTimeSeriesYear,
                    setLatestTimeSeriesYear,
                )
            }
        })()
    }, [project])

    return (
        <AssetViewDiv>
            <Typography variant="h2">Substructure</Typography>
            <AssetName
                setName={setSubstructureName}
                name={substructureName}
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
            <TimeSeries
                caseItem={caseItem}
                setAsset={setSubstructure}
                setHasChanges={setHasChanges}
                asset={substructure}
                timeSeriesType={TimeSeriesEnum.costProfile}
                assetName={substructureName}
                timeSeriesTitle="Cost profile"
                earliestYear={earliestTimeSeriesYear!}
                latestYear={latestTimeSeriesYear!}
                setEarliestYear={setEarliestTimeSeriesYear!}
                setLatestYear={setLatestTimeSeriesYear}
            />
            <TimeSeries
                caseItem={caseItem}
                setAsset={setSubstructure}
                setHasChanges={setHasChanges}
                asset={substructure}
                timeSeriesType={TimeSeriesEnum.substructureCessationCostProfileDto}
                assetName={substructureName}
                timeSeriesTitle="Cessation Cost profile"
                earliestYear={earliestTimeSeriesYear!}
                latestYear={latestTimeSeriesYear!}
                setEarliestYear={setEarliestTimeSeriesYear!}
                setLatestYear={setLatestTimeSeriesYear}
            />
            <Save
                name={substructureName}
                setHasChanges={setHasChanges}
                hasChanges={hasChanges}
                setAsset={setSubstructure}
                setProject={setProject}
                asset={substructure!}
                assetService={GetSubstructureService()}
                assetType={AssetTypeEnum.substructures}
            />
        </AssetViewDiv>
    )
}

export default SubstructureView

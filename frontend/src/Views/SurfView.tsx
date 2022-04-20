import { useEffect, useState } from "react"
import {
    Input, Typography,
} from "@equinor/eds-core-react"

import { useParams } from "react-router"
import { Surf } from "../models/assets/surf/Surf"
import { Case } from "../models/Case"
import { Project } from "../models/Project"
import { GetProjectService } from "../Services/ProjectService"
import { GetSurfService } from "../Services/SurfService"
import TimeSeriesEnum from "../models/assets/TimeSeriesEnum"
import TimeSeries from "../Components/TimeSeries"
import {
    AssetViewDiv, Dg4Field, Wrapper,
} from "./Asset/StyledAssetComponents"
import Save from "../Components/Save"
import AssetName from "../Components/AssetName"
import AssetTypeEnum from "../models/assets/AssetTypeEnum"

const SurfView = () => {
    const [project, setProject] = useState<Project>()
    const [caseItem, setCase] = useState<Case>()
    const [surf, setSurf] = useState<Surf>()
    const [hasChanges, setHasChanges] = useState(false)
    const [surfName, setSurfName] = useState<string>("")
    const params = useParams()

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
                let newSurf = project.surfs.find((s) => s.id === params.surfId)
                if (newSurf !== undefined) {
                    setSurf(newSurf)
                } else {
                    newSurf = new Surf()
                    setSurf(newSurf)
                }
                setSurfName(newSurf?.name!)
            }
        })()
    }, [project])

    return (
        <AssetViewDiv>
            <Typography variant="h2">Surf</Typography>
            <AssetName
                setName={setSurfName}
                name={surfName}
                setHasChanges={setHasChanges}
            />
            <Wrapper>
                <Typography variant="h4">DG4</Typography>
                <Dg4Field>
                    <Input disabled defaultValue={caseItem?.DG4Date?.toLocaleDateString("en-CA")} type="date" />
                </Dg4Field>
            </Wrapper>
            <TimeSeries
                caseItem={caseItem}
                setAsset={setSurf}
                setHasChanges={setHasChanges}
                asset={surf}
                timeSeriesType={TimeSeriesEnum.costProfile}
                assetName={surfName}
                timeSeriesTitle="Cost profile"
            />

            <TimeSeries
                caseItem={caseItem}
                setAsset={setSurf}
                setHasChanges={setHasChanges}
                asset={surf}
                timeSeriesType={TimeSeriesEnum.surfCessationCostProfileDto}
                assetName={surfName}
                timeSeriesTitle="Cessation Cost profile"
            />
            <Wrapper>
                <Save
                    name={surfName}
                    setHasChanges={setHasChanges}
                    hasChanges={hasChanges}
                    setAsset={setSurf}
                    setProject={setProject}
                    asset={surf!}
                    assetService={GetSurfService()}
                    assetType={AssetTypeEnum.surfs}
                />
            </Wrapper>
        </AssetViewDiv>
    )
}

export default SurfView

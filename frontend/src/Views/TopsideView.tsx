import {
    Input, Typography,
} from "@equinor/eds-core-react"
import { useEffect, useState } from "react"
import {
    useParams,
} from "react-router"
import Save from "../Components/Save"
import AssetName from "../Components/AssetName"
import TimeSeries from "../Components/TimeSeries"
import TimeSeriesEnum from "../models/assets/TimeSeriesEnum"
import { Topside } from "../models/assets/topside/Topside"
import { Case } from "../models/Case"
import { Project } from "../models/Project"
import { GetProjectService } from "../Services/ProjectService"
import { GetTopsideService } from "../Services/TopsideService"
import {
    AssetViewDiv, Dg4Field, Wrapper,
} from "./Asset/StyledAssetComponents"
import AssetTypeEnum from "../models/assets/AssetTypeEnum"

const TopsideView = () => {
    const [project, setProject] = useState<Project>()
    const [caseItem, setCase] = useState<Case>()
    const [topside, setTopside] = useState<Topside>()
    const [hasChanges, setHasChanges] = useState(false)
    const [topsideName, setTopsideName] = useState<string>("")
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
                let newTopside = project.topsides.find((s) => s.id === params.topsideId)
                if (newTopside !== undefined) {
                    setTopside(newTopside)
                } else {
                    newTopside = new Topside()
                    setTopside(newTopside)
                }
                setTopsideName(newTopside?.name!)
            }
        })()
    }, [project])

    return (
        <AssetViewDiv>
            <Typography variant="h2">Topside</Typography>
            <AssetName
                setName={setTopsideName}
                name={topsideName}
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
                setAsset={setTopside}
                setHasChanges={setHasChanges}
                asset={topside}
                timeSeriesType={TimeSeriesEnum.costProfile}
                assetName={topsideName}
                timeSeriesTitle="Cost profile"
            />
            <Save
                name={topsideName}
                setHasChanges={setHasChanges}
                hasChanges={hasChanges}
                setAsset={setTopside}
                setProject={setProject}
                asset={topside!}
                assetService={GetTopsideService()}
                assetType={AssetTypeEnum.topsides}
            />
            <TimeSeries
                caseItem={caseItem}
                setAsset={setTopside}
                setHasChanges={setHasChanges}
                asset={topside}
                timeSeriesType={TimeSeriesEnum.topsideCessationCostProfileDto}
                assetName={topsideName}
                timeSeriesTitle="Cessation Cost profile"
            />
            <Wrapper>
                <Save
                    name={topsideName}
                    setHasChanges={setHasChanges}
                    hasChanges={hasChanges}
                    setAsset={setTopside}
                    setProject={setProject}
                    asset={topside!}
                    assetService={GetTopsideService()}
                    assetType={AssetTypeEnum.topsides}
                />
            </Wrapper>
        </AssetViewDiv>
    )
}

export default TopsideView

import {
    Input, Typography,
} from "@equinor/eds-core-react"
import { useEffect, useState } from "react"
import {
    useLocation, useNavigate, useParams,
} from "react-router"
import AssetName from "../Components/AssetName"
import TimeSeries from "../Components/TimeSeries"
import TimeSeriesEnum from "../models/assets/TimeSeriesEnum"
import { Topside } from "../models/assets/topside/Topside"
import { Case } from "../models/Case"
import { Project } from "../models/Project"
import { GetProjectService } from "../Services/ProjectService"
import { GetTopsideService } from "../Services/TopsideService"
import { EMPTY_GUID } from "../Utils/constants"
import {
    AssetViewDiv, Dg4Field, SaveButton, Wrapper,
} from "./Asset/StyledAssetComponents"

const TopsideView = () => {
    const [project, setProject] = useState<Project>()
    const [caseItem, setCase] = useState<Case>()
    const [topside, setTopside] = useState<Topside>()
    const [hasChanges, setHasChanges] = useState(false)
    const [topsideName, setTopsideName] = useState<string>("")
    const params = useParams()
    const navigate = useNavigate()
    const location = useLocation()
    const [earliestTimeSeriesYear, setEarliestTimeSeriesYear] = useState<number>()
    const [latestTimeSeriesYear, setLatestTimeSeriesYear] = useState<number>()

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
                // eslint-disable-next-line max-len
                setEarliestTimeSeriesYear(Math.min(newTopside!.costProfile!.startYear!) + caseResult!.DG4Date!.getFullYear())
                // eslint-disable-next-line max-len
                setLatestTimeSeriesYear(Math.max(newTopside!.costProfile!.startYear! + newTopside.costProfile!.values!.length!) + caseResult!.DG4Date!.getFullYear())
            }
        })()
    }, [project])

    const handleSave = async () => {
        const topsideDto = new Topside(topside!)
        topsideDto.name = topsideName
        if (topside?.id === EMPTY_GUID) {
            topsideDto.projectId = params.projectId
            const newProject: Project = await GetTopsideService().createTopside(params.caseId!, topsideDto!)
            const newTopside = newProject.topsides.at(-1)
            const newUrl = location.pathname.replace(EMPTY_GUID, newTopside!.id!)
            navigate(`${newUrl}`, { replace: true })
            setProject(newProject)
        } else {
            const newProject = await GetTopsideService().updateTopside(topsideDto!)
            setProject(newProject)
        }
        setHasChanges(false)
    }

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
                earliestYear={earliestTimeSeriesYear!}
                latestYear={latestTimeSeriesYear!}
                setEarliestYear={setEarliestTimeSeriesYear!}
                setLatestYear={setLatestTimeSeriesYear}
            />
            <Wrapper><SaveButton disabled={!hasChanges} onClick={handleSave}>Save</SaveButton></Wrapper>
        </AssetViewDiv>
    )
}

export default TopsideView

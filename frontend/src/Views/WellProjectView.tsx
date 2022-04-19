/* eslint-disable max-len */
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
import { WellProject } from "../models/assets/wellproject/WellProject"
import { Case } from "../models/Case"
import { Project } from "../models/Project"
import { GetProjectService } from "../Services/ProjectService"
import { GetWellProjectService } from "../Services/WellProjectService"
import { EMPTY_GUID } from "../Utils/constants"
import {
    AssetViewDiv, Dg4Field, SaveButton, Wrapper,
} from "./Asset/StyledAssetComponents"

function WellProjectView() {
    const [project, setProject] = useState<Project>()
    const [caseItem, setCase] = useState<Case>()
    const [wellProject, setWellProject] = useState<WellProject>()
    const [hasChanges, setHasChanges] = useState(false)
    const [wellProjectName, setWellProjectName] = useState<string>("")
    const [earliestStartYear, setEarliestStartYear] = useState<string>("")
    const [latestEndYear, setLatestEndYear] = useState<string>("")
    const [drillingStartYear, setDrillingStartYear] = useState<string>("")
    const [drillingEndYear, setDrillingEndYear] = useState<string>("")
    const [costProfileStartYear, setCostProfileStartYear] = useState<string>("")
    const [costProfileEndYear, setCostProfileEndYear] = useState<string>("")
    const params = useParams()
    const navigate = useNavigate()
    const location = useLocation()

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
                let newWellProject = project.wellProjects.find((s) => s.id === params.wellProjectId)
                if (newWellProject !== undefined) {
                    setWellProject(newWellProject)
                } else {
                    newWellProject = new WellProject()
                    setWellProject(newWellProject)
                }
                setWellProjectName(newWellProject?.name!)
            }
        })()
    }, [project])

    const handleSave = async () => {
        const wellProjectDto = new WellProject(wellProject!)
        wellProjectDto.name = wellProjectName
        if (wellProject?.id === EMPTY_GUID) {
            wellProjectDto.projectId = params.projectId
            const updatedProject: Project = await
            GetWellProjectService().createWellProject(params.caseId!, wellProjectDto!)
            const newWellProject = updatedProject.wellProjects.at(-1)
            const newUrl = location.pathname.replace(EMPTY_GUID, newWellProject!.id!)
            navigate(`${newUrl}`, { replace: true })
            setWellProject(newWellProject)
        } else {
            const newProject = await GetWellProjectService().updateWellProject(wellProjectDto!)
            setProject(newProject)
        }
        setHasChanges(false)
    }

    return (
        <AssetViewDiv>
            <Typography variant="h2">WellProject</Typography>
            <AssetName
                setName={setWellProjectName}
                name={wellProjectName}
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
                setAsset={setWellProject}
                setHasChanges={setHasChanges}
                asset={wellProject}
                timeSeriesType={TimeSeriesEnum.costProfile}
                assetName={wellProjectName}
                timeSeriesTitle="Cost profile"
            />
            <TimeSeries
                caseItem={caseItem}
                setAsset={setWellProject}
                setHasChanges={setHasChanges}
                asset={wellProject}
                timeSeriesType={TimeSeriesEnum.drillingSchedule}
                assetName={wellProjectName}
                timeSeriesTitle="Drilling schedule"
            />
            <Wrapper><SaveButton disabled={!hasChanges} onClick={handleSave}>Save</SaveButton></Wrapper>

        </AssetViewDiv>
    )
}

export default WellProjectView

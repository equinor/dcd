import {
    Button, Input, Typography, Label,
} from "@equinor/eds-core-react"
import { ChangeEventHandler, useEffect, useState } from "react"
import {
    useLocation, useNavigate, useParams,
} from "react-router"
import styled from "styled-components"
import TimeSeries from "../Components/TimeSeries"
import CostProfile from "../models/assets/CostProfile"
import { WellProject } from "../models/assets/wellproject/WellProject"
import { Case } from "../models/Case"
import { Project } from "../models/Project"
import { GetProjectService } from "../Services/ProjectService"
import { GetWellProjectService } from "../Services/WellProjectService"
import { emptyGuid } from "../Utils/constants"

const AssetHeader = styled.div`
    margin-bottom: 2rem;
    display: flex;

    > *:first-child {
        margin-right: 2rem;
    }
`

const AssetViewDiv = styled.div`
    margin: 2rem;
    display: flex;
    flex-direction: column;
`

const Wrapper = styled.div`
    display: flex;
    flex-direction: row;
    margin-top: 1rem;
`

const WrapperColumn = styled.div`
    display: flex;
    flex-direction: column;
`

const SaveButton = styled(Button)`
    margin-top: 5rem;
    margin-left: 2rem;
    &:disabled {
        margin-left: 2rem;
        margin-top: 5rem;
    }
`

const Dg4Field = styled.div`
    margin-left: 1rem;
    margin-bottom: 1rem;
    width: 10rem;
    display: flex;
`

function WellProjectView() {
    const [, setProject] = useState<Project>()
    const [caseItem, setCase] = useState<Case>()
    const [wellProject, setWellProject] = useState<WellProject>()
    const [hasChanges, setHasChanges] = useState(false)
    const [wellProjectName, setWellProjectName] = useState<string>("")
    const params = useParams()
    const navigate = useNavigate()
    const location = useLocation()

    useEffect(() => {
        (async () => {
            try {
                const projectResult = await GetProjectService().getProjectByID(params.projectId!)
                setProject(projectResult)
                const caseResult = projectResult.cases.find((o) => o.id === params.caseId)
                setCase(caseResult)
                let newWellProject = projectResult.wellProjects.find((s) => s.id === params.wellProjectId)
                if (newWellProject !== undefined) {
                    setWellProject(newWellProject)
                } else {
                    newWellProject = new WellProject()
                    setWellProject(newWellProject)
                }
                setWellProjectName(newWellProject.name!)
            } catch (error) {
                console.error(`[CaseView] Error while fetching project ${params.projectId}`, error)
            }
        })()
    }, [params.projectId, params.caseId])

    const handleSave = async () => {
        const wellProjectDto = new WellProject(wellProject!)
        wellProjectDto.name = wellProjectName
        if (wellProject?.id === emptyGuid) {
            wellProjectDto.projectId = params.projectId
            const updatedProject: Project = await
            GetWellProjectService().createWellProject(params.caseId!, wellProjectDto!)
            const updatedCase = updatedProject.cases.find((o) => o.id === params.caseId)
            const newWellProject = updatedProject.wellProjects.at(-1)
            const newUrl = location.pathname.replace(emptyGuid, newWellProject!.id!)
            setCase(updatedCase)
            setWellProject(newWellProject)
            navigate(`${newUrl}`, { replace: true })
        } else {
            wellProjectDto.projectId = params.projectId
            const newProject = await GetWellProjectService().updateWellProject(wellProjectDto!)
            setProject(newProject)
            const newCase = newProject.cases.find((o) => o.id === params.caseId)
            setCase(newCase)
            const newWellProject = newProject.wellProjects.find((s) => s.id === params.wellProjectId)
            setWellProject(newWellProject)
        }
        setHasChanges(false)
    }

    const handleWellProjectNameFieldChange: ChangeEventHandler<HTMLInputElement> = async (e) => {
        setWellProjectName(e.target.value)
        if (e.target.value !== undefined && e.target.value !== "") {
            setHasChanges(true)
        } else {
            setHasChanges(false)
        }
    }

    return (
        <AssetViewDiv>
            <Typography variant="h2">WellProject</Typography>
            <AssetHeader>
                <WrapperColumn>
                    <Label htmlFor="wellProjectName" label="Name" />
                    <Input
                        id="wellProjectName"
                        name="wellProjectName"
                        placeholder="Enter wellproject name"
                        value={wellProjectName}
                        onChange={handleWellProjectNameFieldChange}
                    />
                </WrapperColumn>
            </AssetHeader>
            <Wrapper>
                <Typography variant="h4">DG4</Typography>
                <Dg4Field>
                    <Input disabled defaultValue={caseItem?.DG4Date?.toLocaleDateString("en-CA")} type="date" />
                </Dg4Field>
            </Wrapper>
            <TimeSeries
                timeSeries={wellProject?.costProfile}
                caseItem={caseItem}
                setAsset={setWellProject}
                setHasChanges={setHasChanges}
                asset={wellProject}
                costProfile={CostProfile.costProfile}
                assetName={wellProjectName}
                timeSeriesTitle="Cost profile"
            />
            <TimeSeries
                timeSeries={wellProject?.drillingSchedule}
                caseItem={caseItem}
                setAsset={setWellProject}
                setHasChanges={setHasChanges}
                asset={wellProject}
                costProfile={CostProfile.drillingSchedule}
                assetName={wellProjectName}
                timeSeriesTitle="Drilling schedule"
            />
            <Wrapper><SaveButton disabled={!hasChanges} onClick={handleSave}>Save</SaveButton></Wrapper>

        </AssetViewDiv>
    )
}

export default WellProjectView

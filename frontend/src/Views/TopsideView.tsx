import {
    Button, Input, Label, Typography,
} from "@equinor/eds-core-react"
import { ChangeEventHandler, useEffect, useState } from "react"
import {
    useLocation, useNavigate, useParams,
} from "react-router"
import styled from "styled-components"
import TimeSeries from "../Components/TimeSeries"
import TimeSeriesEnum from "../models/assets/TimeSeriesEnum"
import { Topside } from "../models/assets/topside/Topside"
import { Case } from "../models/Case"
import { Project } from "../models/Project"
import { GetProjectService } from "../Services/ProjectService"
import { GetTopsideService } from "../Services/TopsideService"
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
    margin-bottom: 2rem;
    width: 10rem;
    display: flex;
`

const TopsideView = () => {
    const [, setProject] = useState<Project>()
    const [caseItem, setCase] = useState<Case>()
    const [topside, setTopside] = useState<Topside>()
    const [hasChanges, setHasChanges] = useState(false)
    const [topsideName, setTopsideName] = useState<string>("")
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
                let newTopside = projectResult.topsides.find((s) => s.id === params.topsideId)
                if (newTopside !== undefined) {
                    setTopside(newTopside)
                } else {
                    newTopside = new Topside()
                    setTopside(newTopside)
                }
                setTopsideName(newTopside.name!)
            } catch (error) {
                console.error(`[CaseView] Error while fetching project ${params.projectId}`, error)
            }
        })()
    }, [params.projectId, params.caseId])

    const handleSave = async () => {
        const topsideDto = new Topside(topside!)
        topsideDto.name = topsideName
        if (topside?.id === emptyGuid) {
            topsideDto.projectId = params.projectId
            const updatedProject: Project = await GetTopsideService().createTopside(params.caseId!, topsideDto!)
            const updatedCase = updatedProject.cases.find((o) => o.id === params.caseId)
            const newTopside = updatedProject.topsides.at(-1)
            const newUrl = location.pathname.replace(emptyGuid, newTopside!.id!)
            setTopside(newTopside)
            setCase(updatedCase)
            navigate(`${newUrl}`, { replace: true })
        } else {
            topsideDto.projectId = params.projectId
            const newProject = await GetTopsideService().updateTopside(topsideDto!)
            setProject(newProject)
            const newCase = newProject.cases.find((o) => o.id === params.caseId)
            setCase(newCase)
            const newTopside = newProject.topsides.find((s) => s.id === params.topsideId)
            setTopside(newTopside)
        }
        setHasChanges(false)
    }

    const handleTopsideNameFieldChange: ChangeEventHandler<HTMLInputElement> = async (e) => {
        setTopsideName(e.target.value)
        if (e.target.value !== undefined && e.target.value !== "") {
            setHasChanges(true)
        } else {
            setHasChanges(false)
        }
    }

    return (
        <AssetViewDiv>
            <Typography variant="h2">Topside</Typography>
            <AssetHeader>
                <WrapperColumn>
                    <Label htmlFor="topsideName" label="Name" />
                    <Input
                        id="topsideName"
                        name="topsideName"
                        placeholder="Enter topside name"
                        value={topsideName}
                        onChange={handleTopsideNameFieldChange}
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
                timeSeries={topside?.costProfile}
                caseItem={caseItem}
                setAsset={setTopside}
                setHasChanges={setHasChanges}
                asset={topside}
                timeSeriesType={TimeSeriesEnum.costProfile}
                assetName={topsideName}
                timeSeriesTitle="Cost profile"
            />
            <Wrapper><SaveButton disabled={!hasChanges} onClick={handleSave}>Save</SaveButton></Wrapper>
        </AssetViewDiv>
    )
}

export default TopsideView

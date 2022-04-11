import { ChangeEventHandler, useEffect, useState } from "react"
import styled from "styled-components"
import {
    Button, Input, Label, Typography,
} from "@equinor/eds-core-react"

import { useNavigate, useLocation, useParams } from "react-router"
import { Surf } from "../models/assets/surf/Surf"
import { Case } from "../models/Case"
import { Project } from "../models/Project"
import { GetProjectService } from "../Services/ProjectService"
import { GetSurfService } from "../Services/SurfService"
import TimeSeriesEnum from "../models/assets/TimeSeriesEnum"
import TimeSeries from "../Components/TimeSeries"
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

const SurfView = () => {
    const [, setProject] = useState<Project>()
    const [caseItem, setCase] = useState<Case>()
    const [surf, setSurf] = useState<Surf>()
    const [hasChanges, setHasChanges] = useState(false)
    const [surfName, setSurfName] = useState<string>("")
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
                let newSurf = projectResult.surfs.find((s) => s.id === params.surfId)
                if (newSurf !== undefined) {
                    setSurf(newSurf)
                } else {
                    newSurf = new Surf()
                    setSurf(newSurf)
                }
                setSurfName(newSurf.name!)
            } catch (error) {
                console.error(`[CaseView] Error while fetching project ${params.projectId}`, error)
            }
        })()
    }, [params.projectId, params.caseId])

    const handleSave = async () => {
        const surfDto = new Surf(surf!)
        surfDto.name = surfName
        if (surf?.id === emptyGuid) {
            surfDto.projectId = params.projectId
            const updatedProject = await GetSurfService().createSurf(params.caseId!, surfDto!)
            const updatedCase = updatedProject.cases.find((o) => o.id === params.caseId)
            const newSurf = updatedProject.surfs.at(-1)
            const newUrl = location.pathname.replace(emptyGuid, newSurf!.id!)
            setProject(updatedProject)
            setCase(updatedCase)
            setSurf(newSurf)
            navigate(`${newUrl}`, { replace: true })
        } else {
            const newProject = await GetSurfService().updateSurf(surfDto)
            setProject(newProject)
            const newCase = newProject.cases.find((o) => o.id === params.caseId)
            setCase(newCase)
            const newSurf = newProject.surfs.find((a) => a.id === params.surfId)
            setSurf(newSurf)
        }
        setHasChanges(false)
    }

    const handleSurfNameFieldChange: ChangeEventHandler<HTMLInputElement> = async (e) => {
        setSurfName(e.target.value)
        if (e.target.value !== undefined && e.target.value !== "") {
            setHasChanges(true)
        } else {
            setHasChanges(false)
        }
    }

    return (
        <AssetViewDiv>
            <Typography variant="h2">Surf</Typography>
            <AssetHeader>
                <WrapperColumn>
                    <Label htmlFor="surfName" label="Name" />
                    <Input
                        id="surfName"
                        name="surfName"
                        placeholder="Enter surf name"
                        value={surfName}
                        onChange={handleSurfNameFieldChange}
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
                timeSeries={surf?.costProfile}
                caseItem={caseItem}
                setAsset={setSurf}
                setHasChanges={setHasChanges}
                asset={surf}
                timeSeriesType={TimeSeriesEnum.costProfile}
                assetName={surfName}
                timeSeriesTitle="Cost profile"
            />
            <Wrapper><SaveButton disabled={!hasChanges} onClick={handleSave}>Save</SaveButton></Wrapper>
        </AssetViewDiv>
    )
}

export default SurfView

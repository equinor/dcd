import {
    Input, Typography, Label,
} from "@equinor/eds-core-react"
import { ChangeEventHandler, useEffect, useState } from "react"
import {
    useLocation, useNavigate, useParams,
} from "react-router"
import TimeSeries from "../Components/TimeSeries"
import TimeSeriesEnum from "../models/assets/TimeSeriesEnum"
import { Substructure } from "../models/assets/substructure/Substructure"
import { Case } from "../models/Case"
import { Project } from "../models/Project"
import { GetProjectService } from "../Services/ProjectService"
import { GetSubstructureService } from "../Services/SubstructureService"
import { emptyGuid } from "../Utils/constants"
import {
    AssetHeader, AssetViewDiv, Dg4Field, SaveButton, Wrapper, WrapperColumn,
   } from "./Asset/StyledAssetComponents"

const SubstructureView = () => {
    const [, setProject] = useState<Project>()
    const [caseItem, setCase] = useState<Case>()
    const [substructure, setSubstructure] = useState<Substructure>()
    const [hasChanges, setHasChanges] = useState(false)
    const [substructureName, setSubstructureName] = useState<string>("")
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
                let newSubstructure = projectResult.substructures.find((s) => s.id === params.substructureId)
                if (newSubstructure !== undefined) {
                    setSubstructure(newSubstructure)
                } else {
                    newSubstructure = new Substructure()
                    setSubstructure(newSubstructure)
                }
                setSubstructureName(newSubstructure.name!)
            } catch (error) {
                console.error(`[CaseView] Error while fetching project ${params.projectId}`, error)
            }
        })()
    }, [params.projectId, params.caseId])

    const handleSave = async () => {
        const substructureDto = new Substructure(substructure!)
        substructureDto.name = substructureName
        if (substructure?.id === emptyGuid) {
            substructureDto.projectId = params.projectId
            const updatedProject: Project = await
            GetSubstructureService().createSubstructure(params.caseId!, substructureDto!)
            const updatedCase = updatedProject.cases.find((o) => o.id === params.caseId)
            const newSubstructure = updatedProject.substructures.at(-1)
            const newUrl = location.pathname.replace(emptyGuid, newSubstructure!.id!)
            setSubstructure(newSubstructure)
            setCase(updatedCase)
            navigate(`${newUrl}`, { replace: true })
        } else {
            substructureDto.projectId = params.projectId
            const newProject = await GetSubstructureService().updateSubstructure(substructureDto!)
            setProject(newProject)
            const newCase = newProject.cases.find((o) => o.id === params.caseId)
            setCase(newCase)
            const newSubstructure = newProject.substructures.find((s) => s.id === params.substructureId)
            setSubstructure(newSubstructure)
        }
        setHasChanges(false)
    }

    const handleSubstructureNameFieldChange: ChangeEventHandler<HTMLInputElement> = async (e) => {
        setSubstructureName(e.target.value)
        if (e.target.value !== undefined && e.target.value !== "") {
            setHasChanges(true)
        } else {
            setHasChanges(false)
        }
    }

    return (
        <AssetViewDiv>
            <Typography variant="h2">Substructure</Typography>
            <AssetHeader>
                <WrapperColumn>
                    <Label htmlFor="substructureName" label="Name" />
                    <Input
                        id="substructureName"
                        name="substructureName"
                        placeholder="Enter substructure name"
                        defaultValue={substructure?.name}
                        onChange={handleSubstructureNameFieldChange}
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
                timeSeries={substructure?.costProfile}
                caseItem={caseItem}
                setAsset={setSubstructure}
                setHasChanges={setHasChanges}
                asset={substructure}
                timeSeriesType={TimeSeriesEnum.costProfile}
                assetName={substructureName}
                timeSeriesTitle="Cost profile"
            />
            <Wrapper>
                <SaveButton disabled={!hasChanges} onClick={handleSave}>
                    Save
                </SaveButton>
            </Wrapper>
        </AssetViewDiv>
    )
}

export default SubstructureView

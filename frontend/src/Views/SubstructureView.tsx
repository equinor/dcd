import {
    Input, Typography,
} from "@equinor/eds-core-react"
import { useEffect, useState } from "react"
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
import { EMPTY_GUID } from "../Utils/constants"
import {
    AssetViewDiv, Dg4Field, SaveButton, Wrapper,
} from "./Asset/StyledAssetComponents"
import AssetName from "../Components/AssetName"
import { unwrapCase, unwrapCaseId, unwrapProjectId } from "../Utils/common"

const SubstructureView = () => {
    const [project, setProject] = useState<Project>()
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
            }
        })()
    }, [project])

    const handleSave = async () => {
        const substructureDto: Substructure = new Substructure(substructure)
        substructureDto.name = substructureName
        if (substructure?.id === EMPTY_GUID) {
            substructureDto.projectId = params.projectId
            const caseId: string = unwrapCaseId(params.caseId)
            const newProject = await GetSubstructureService()
                .createSubstructure(caseId, substructureDto)
            const newSubstructure: Substructure | undefined = newProject.substructures.at(-1)
            const newUrl: string = location.pathname.replace(EMPTY_GUID, newSubstructure?.id!)
            navigate(`${newUrl}`)
            setSubstructure(newSubstructure)
        } else {
            const newProject: Project = await GetSubstructureService().updateSubstructure(substructureDto)
            setProject(newProject)
        }
        setHasChanges(false)
    }

    return (
        <AssetViewDiv>
            <Typography variant="h2">Substructure</Typography>
            <AssetName
                setName={setSubstructureName}
                name={substructureName}
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
                setAsset={setSubstructure}
                setHasChanges={setHasChanges}
                asset={substructure}
                timeSeriesType={TimeSeriesEnum.costProfile}
                assetName={substructureName}
                timeSeriesTitle="Cost profile"
            />
            <TimeSeries
                caseItem={caseItem}
                setAsset={setSubstructure}
                setHasChanges={setHasChanges}
                asset={substructure}
                timeSeriesType={TimeSeriesEnum.substructureCessationCostProfileDto}
                assetName={substructureName}
                timeSeriesTitle="Cessation Cost profile"
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

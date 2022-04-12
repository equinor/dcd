import {
    Input, Label, Typography,
} from "@equinor/eds-core-react"
import { ChangeEventHandler, useEffect, useState } from "react"
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
    AssetHeader,
    AssetViewDiv, Dg4Field, Wrapper, WrapperColumn,
} from "./Asset/StyledAssetComponents"
import Save from "../Components/Save"

const SubstructureView = () => {
    const [project, setProject] = useState<Project>()
    const [caseItem, setCase] = useState<Case>()
    const [substructure, setSubstructure] = useState<Substructure>()
    const [hasChanges, setHasChanges] = useState(false)
    const [substructureName, setSubstructureName] = useState<string>("")
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
                let newSubstructure = project.substructures.find((s) => s.id === params.substructureId)
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
                        value={substructureName}
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
                caseItem={caseItem}
                setAsset={setSubstructure}
                setHasChanges={setHasChanges}
                asset={substructure}
                timeSeriesType={TimeSeriesEnum.costProfile}
                assetName={substructureName}
                timeSeriesTitle="Cost profile"
            />
            <Save
                name={substructureName}
                setHasChanges={setHasChanges}
                hasChanges={hasChanges}
                setAsset={setSubstructure}
                setProject={setProject}
                asset={substructure!}
                assetService={GetSubstructureService()}
            />
        </AssetViewDiv>
    )
}

export default SubstructureView

import {
    Input, Typography,
} from "@equinor/eds-core-react"
import { useEffect, useState } from "react"
import {
    useParams,
} from "react-router"
import TimeSeries from "../Components/TimeSeries"
import { Substructure } from "../models/assets/substructure/Substructure"
import { Case } from "../models/Case"
import { Project } from "../models/Project"
import { GetProjectService } from "../Services/ProjectService"
import { GetSubstructureService } from "../Services/SubstructureService"
import {
    AssetViewDiv, Dg4Field, Wrapper,
} from "./Asset/StyledAssetComponents"
import Save from "../Components/Save"
import AssetName from "../Components/AssetName"
import { unwrapCase, unwrapProjectId } from "../Utils/common"
import AssetTypeEnum from "../models/assets/AssetTypeEnum"
import { initializeFirstAndLastYear } from "./Asset/AssetHelper"
import Maturity from "../Components/Maturity"
import NumberInput from "../Components/NumberInput"
import { SubstructureCostProfile } from "../models/assets/substructure/SubstructureCostProfile"
import { SubstructureCessationCostProfile } from "../models/assets/substructure/SubstructureCessationCostProfile"
import AssetCurrency from "../Components/AssetCurrency"

const SubstructureView = () => {
    const [project, setProject] = useState<Project>()
    const [caseItem, setCase] = useState<Case>()
    const [substructure, setSubstructure] = useState<Substructure>()

    const [hasChanges, setHasChanges] = useState(false)
    const [substructureName, setSubstructureName] = useState<string>("")
    const params = useParams()
    const [firstTSYear, setFirstTSYear] = useState<number>()
    const [lastTSYear, setLastTSYear] = useState<number>()
    const [maturity, setMaturity] = useState<Components.Schemas.Maturity | undefined>()
    const [dryWeight, setDryWeight] = useState<number | undefined>()
    const [costProfile, setCostProfile] = useState<SubstructureCostProfile>()
    const [cessationCostProfile, setCessationCostProfile] = useState<SubstructureCessationCostProfile>()
    const [currency, setCurrency] = useState<Components.Schemas.Currency>(0)

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
                    newSubstructure.currency = project.currency
                    setSubstructure(newSubstructure)
                }
                setSubstructureName(newSubstructure?.name!)
                setMaturity(newSubstructure.maturity)
                setDryWeight(newSubstructure.dryweight)
                setCurrency(newSubstructure.currency ?? 0)

                setCostProfile(newSubstructure.costProfile)
                setCessationCostProfile(newSubstructure.cessationCostProfile)

                if (caseResult?.DG4Date) {
                    initializeFirstAndLastYear(
                        caseResult?.DG4Date?.getFullYear(),
                        [newSubstructure.costProfile, newSubstructure.cessationCostProfile],
                        setFirstTSYear,
                        setLastTSYear,
                    )
                }
            }
        })()
    }, [project])

    useEffect(() => {
        if (substructure !== undefined) {
            const newSubstructure: Substructure = { ...substructure }
            newSubstructure.maturity = maturity
            newSubstructure.dryweight = dryWeight
            newSubstructure.costProfile = costProfile
            newSubstructure.cessationCostProfile = cessationCostProfile
            newSubstructure.currency = currency

            if (caseItem?.DG4Date) {
                initializeFirstAndLastYear(
                    caseItem?.DG4Date?.getFullYear(),
                    [costProfile, cessationCostProfile],
                    setFirstTSYear,
                    setLastTSYear,
                )
            }
            setSubstructure(newSubstructure)
        }
    }, [maturity, dryWeight, costProfile, cessationCostProfile, currency])

    return (
        <AssetViewDiv>
            <Typography variant="h2">Substructure</Typography>
            <AssetName
                setName={setSubstructureName}
                name={substructureName}
                setHasChanges={setHasChanges}
            />
            <Wrapper>
                <Typography variant="h4">DG3</Typography>
                <Dg4Field>
                    <Input disabled defaultValue={caseItem?.DG3Date?.toLocaleDateString("en-CA")} type="date" />
                </Dg4Field>
                <Typography variant="h4">DG4</Typography>
                <Dg4Field>
                    <Input disabled defaultValue={caseItem?.DG4Date?.toLocaleDateString("en-CA")} type="date" />
                </Dg4Field>
            </Wrapper>
            <AssetCurrency
                setCurrency={setCurrency}
                setHasChanges={setHasChanges}
                currentValue={currency}
            />
            <Wrapper>
                <NumberInput
                    setHasChanges={setHasChanges}
                    setValue={setDryWeight}
                    value={dryWeight ?? 0}
                    integer={false}
                    label={`Substructure dry weight ${project?.physUnit === 0 ? "(tonnes)" : "(Oilfield)"}`}
                />
            </Wrapper>
            <Maturity
                setMaturity={setMaturity}
                currentValue={maturity}
                setHasChanges={setHasChanges}
            />
            <TimeSeries
                dG4Year={caseItem?.DG4Date?.getFullYear()}
                setTimeSeries={setCostProfile}
                setHasChanges={setHasChanges}
                timeSeries={costProfile}
                timeSeriesTitle={`Cost profile ${currency === 0 ? "(MUSD)" : "(MNOK)"}`}
                firstYear={firstTSYear!}
                lastYear={lastTSYear!}
                setFirstYear={setFirstTSYear!}
                setLastYear={setLastTSYear}
            />
            <TimeSeries
                dG4Year={caseItem?.DG4Date?.getFullYear()}
                setTimeSeries={setCessationCostProfile}
                setHasChanges={setHasChanges}
                timeSeries={cessationCostProfile}
                timeSeriesTitle={`Cessation cost profile ${currency === 0 ? "(MUSD)" : "(MNOK)"}`}
                firstYear={firstTSYear!}
                lastYear={lastTSYear!}
                setFirstYear={setFirstTSYear!}
                setLastYear={setLastTSYear}
            />
            <Save
                name={substructureName}
                setHasChanges={setHasChanges}
                hasChanges={hasChanges}
                setAsset={setSubstructure}
                setProject={setProject}
                asset={substructure!}
                assetService={GetSubstructureService()}
                assetType={AssetTypeEnum.substructures}
            />
        </AssetViewDiv>
    )
}

export default SubstructureView

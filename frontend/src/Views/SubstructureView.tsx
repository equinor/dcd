import {
    Typography,
} from "@equinor/eds-core-react"
import { useEffect, useState } from "react"
import {
    useParams,
} from "react-router"
import { useCurrentContext } from "@equinor/fusion"
import TimeSeries from "../Components/TimeSeries"
import { Substructure } from "../models/assets/substructure/Substructure"
import { Case } from "../models/case/Case"
import { Project } from "../models/Project"
import { GetProjectService } from "../Services/ProjectService"
import { GetSubstructureService } from "../Services/SubstructureService"
import {
    AssetViewDiv, Wrapper, WrapperColumn,
} from "./Asset/StyledAssetComponents"
import Save from "../Components/Save"
import AssetName from "../Components/AssetName"
import { IsInvalidDate, unwrapCase, unwrapProjectId } from "../Utils/common"
import AssetTypeEnum from "../models/assets/AssetTypeEnum"
import { initializeFirstAndLastYear } from "./Asset/AssetHelper"
import Maturity from "../Components/Maturity"
import NumberInput from "../Components/NumberInput"
import { SubstructureCostProfile } from "../models/assets/substructure/SubstructureCostProfile"
import { SubstructureCessationCostProfile } from "../models/assets/substructure/SubstructureCessationCostProfile"
import AssetCurrency from "../Components/AssetCurrency"
import ApprovedBy from "../Components/ApprovedBy"
import Concept from "../Components/Concept"
import DGDateInherited from "../Components/DGDateInherited"
import { IAssetService } from "../Services/IAssetService"

const SubstructureView = () => {
    const [project, setProject] = useState<Project>()
    const [caseItem, setCase] = useState<Case>()
    const [substructure, setSubstructure] = useState<Substructure>()

    const [hasChanges, setHasChanges] = useState(false)
    const [substructureName, setSubstructureName] = useState<string>("")
    const { fusionContextId, caseId, substructureId } = useParams<Record<string, string | undefined>>()
    const currentProject = useCurrentContext()

    const [firstTSYear, setFirstTSYear] = useState<number>()
    const [lastTSYear, setLastTSYear] = useState<number>()
    const [maturity, setMaturity] = useState<Components.Schemas.Maturity | undefined>()
    const [dryWeight, setDryWeight] = useState<number | undefined>()
    const [costProfile, setCostProfile] = useState<SubstructureCostProfile>()
    const [cessationCostProfile, setCessationCostProfile] = useState<SubstructureCessationCostProfile>()
    const [currency, setCurrency] = useState<Components.Schemas.Currency>(1)
    const [approvedBy, setApprovedBy] = useState<string>("")
    const [costYear, setCostYear] = useState<number | undefined>()
    const [concept, setConcept] = useState<Components.Schemas.Concept | undefined>()
    const [dG3Date, setDG3Date] = useState<Date>()
    const [dG4Date, setDG4Date] = useState<Date>()

    const [substructureService, setSubstructureService] = useState<IAssetService>()

    useEffect(() => {
        (async () => {
            try {
                const projectId = unwrapProjectId(currentProject?.externalId)
                const projectResult = await (await GetProjectService()).getProjectByID(projectId)
                setProject(projectResult)
            } catch (error) {
                console.error(`[CaseView] Error while fetching project ${currentProject?.externalId}`, error)
            }
        })()
    }, [])

    useEffect(() => {
        (async () => {
            if (project !== undefined) {
                const caseResult = unwrapCase(project.cases.find((o) => o.id === caseId))
                setCase(caseResult)
                // eslint-disable-next-line max-len
                let newSubstructure = project.substructures.find((s) => s.id === substructureId)
                if (newSubstructure !== undefined) {
                    if (newSubstructure.DG3Date === null
                        || IsInvalidDate(newSubstructure.DG3Date)) {
                        newSubstructure.DG3Date = caseResult?.DG3Date
                    }
                    if (newSubstructure.DG4Date === null
                        || IsInvalidDate(newSubstructure.DG4Date)) {
                        newSubstructure.DG4Date = caseResult?.DG4Date
                    }
                    setSubstructure(newSubstructure)
                } else {
                    newSubstructure = new Substructure()
                    newSubstructure.currency = project.currency
                    newSubstructure.DG3Date = caseResult?.DG3Date
                    newSubstructure.DG4Date = caseResult?.DG4Date
                    setSubstructure(newSubstructure)
                }
                setSubstructureName(newSubstructure?.name!)
                setMaturity(newSubstructure.maturity)
                setDryWeight(newSubstructure.dryweight)
                setCurrency(newSubstructure.currency ?? 1)
                setApprovedBy(newSubstructure?.approvedBy!)
                setCostYear(newSubstructure?.costYear)
                setConcept(newSubstructure.concept)
                setDG3Date(newSubstructure.DG3Date ?? undefined)
                setDG4Date(newSubstructure.DG4Date ?? undefined)

                setCostProfile(newSubstructure.costProfile)
                setCessationCostProfile(newSubstructure.cessationCostProfile)

                if (caseResult?.DG4Date) {
                    const dg4 = newSubstructure?.source === 1 ? newSubstructure.DG4Date?.getFullYear()
                        : caseResult.DG4Date.getFullYear()
                    initializeFirstAndLastYear(
                        dg4!,
                        [newSubstructure.costProfile, newSubstructure.cessationCostProfile],
                        setFirstTSYear,
                        setLastTSYear,
                    )
                }
                const service = await GetSubstructureService()
                setSubstructureService(service)
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
            newSubstructure.approvedBy = approvedBy
            newSubstructure.costYear = costYear
            newSubstructure.concept = concept
            newSubstructure.DG3Date = dG3Date
            newSubstructure.DG4Date = dG4Date

            if (caseItem?.DG4Date) {
                const dg4 = newSubstructure?.source === 1 ? newSubstructure.DG4Date?.getFullYear()
                    : caseItem.DG4Date.getFullYear()
                initializeFirstAndLastYear(
                    dg4!,
                    [costProfile, cessationCostProfile],
                    setFirstTSYear,
                    setLastTSYear,
                )
            }
            setSubstructure(newSubstructure)
        }
    }, [maturity, dryWeight, costProfile, cessationCostProfile, currency, approvedBy, costYear, concept,
        dG3Date, dG4Date])

    const setAllStates = (timeSeries: any) => {
        if (timeSeries) {
            if (timeSeries.name === "Cost profile") {
                setCostProfile(timeSeries)
            }
            if (timeSeries.name === "Cessation cost profile") {
                setCessationCostProfile(timeSeries)
            }
        }
    }

    if (!substructure || !caseItem) {
        return null
    }

    return (
        <AssetViewDiv>
            <Wrapper>
                <Typography variant="h2">Substructure</Typography>
                <Save
                    name={substructureName}
                    setHasChanges={setHasChanges}
                    hasChanges={hasChanges}
                    setAsset={setSubstructure}
                    setProject={setProject}
                    asset={substructure!}
                    assetService={substructureService!}
                    assetType={AssetTypeEnum.substructures}
                />
                <Typography variant="h6">
                    {substructure?.LastChangedDate?.toLocaleString()
                        ? `Last changed: ${substructure?.LastChangedDate?.toLocaleString()}` : ""}
                </Typography>
            </Wrapper>
            <AssetName
                setName={setSubstructureName}
                name={substructureName}
                setHasChanges={setHasChanges}
            />
            <ApprovedBy
                setApprovedBy={setApprovedBy}
                approvedBy={approvedBy}
                setHasChanges={setHasChanges}
            />
            <Wrapper>
                <WrapperColumn>
                    <NumberInput
                        setHasChanges={setHasChanges}
                        setValue={setCostYear}
                        value={costYear ?? 0}
                        integer
                        label="Cost year"
                    />
                </WrapperColumn>
            </Wrapper>
            <Typography>
                {`Prosp version: ${substructure?.ProspVersion
                    ? substructure?.ProspVersion.toLocaleDateString() : "N/A"}`}
            </Typography>
            <Typography>
                {`Source: ${substructure?.source === 0 || substructure?.source === undefined ? "ConceptApp" : "Prosp"}`}
            </Typography>
            <Wrapper>
                <DGDateInherited
                    setHasChanges={setHasChanges}
                    setValue={setDG3Date}
                    dGName="DG3"
                    value={dG3Date}
                    caseValue={caseItem?.DG3Date}
                    disabled={substructure?.source === 1}
                />
                <DGDateInherited
                    setHasChanges={setHasChanges}
                    setValue={setDG4Date}
                    dGName="DG4"
                    value={dG4Date}
                    caseValue={caseItem?.DG4Date}
                    disabled={substructure?.source === 1}
                />
            </Wrapper>
            <AssetCurrency
                setCurrency={setCurrency}
                setHasChanges={setHasChanges}
                currentValue={currency}
            />
            <Wrapper>
                <WrapperColumn>
                    <NumberInput
                        setHasChanges={setHasChanges}
                        setValue={setCostYear}
                        value={costYear ?? 0}
                        integer
                        label="Cost year"
                    />
                </WrapperColumn>
            </Wrapper>
            <Wrapper>
                <NumberInput
                    setHasChanges={setHasChanges}
                    setValue={setDryWeight}
                    value={dryWeight ?? 0}
                    integer={false}
                    label="Substructure dry weight (tonnes)"
                />
            </Wrapper>
            <Maturity
                setMaturity={setMaturity}
                currentValue={maturity}
                setHasChanges={setHasChanges}
            />
            <Concept
                setHasChanges={setHasChanges}
                currentValue={concept}
                setConcept={setConcept}
            />
            <TimeSeries
                dG4Year={substructure.source === 1 ? substructure.DG4Date!.getFullYear()
                    : caseItem.DG4Date!.getFullYear()}
                setTimeSeries={setAllStates}
                setHasChanges={setHasChanges}
                timeSeries={[costProfile, cessationCostProfile]}
                firstYear={firstTSYear!}
                lastYear={lastTSYear!}
                profileName={["Cost profile", "Cessation cost profile"]}
                profileEnum={project?.physUnit!}
                profileType="Cost"
            />
        </AssetViewDiv>
    )
}

export default SubstructureView

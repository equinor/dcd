import { useEffect, useState } from "react"
import { Typography } from "@equinor/eds-core-react"

import { useParams } from "react-router-dom"
import { useCurrentContext } from "@equinor/fusion"
import { Surf } from "../models/assets/surf/Surf"
import { Case } from "../models/case/Case"
import { Project } from "../models/Project"
import { GetProjectService } from "../Services/ProjectService"
import { GetSurfService } from "../Services/SurfService"
import TimeSeries from "../Components/TimeSeries"
import {
    AssetViewDiv, Wrapper, WrapperColumn,
} from "./Asset/StyledAssetComponents"
import Save from "../Components/Save"
import AssetName from "../Components/AssetName"
import { IsInvalidDate, unwrapCase, unwrapProjectId } from "../Utils/common"
import AssetTypeEnum from "../models/assets/AssetTypeEnum"
import { initializeFirstAndLastYear } from "./Asset/AssetHelper"
import NumberInput from "../Components/NumberInput"
import Maturity from "../Components/Maturity"
import ProductionFlowline from "../Components/ProductionFlowline"
import { SurfCostProfile } from "../models/assets/surf/SurfCostProfile"
import { SurfCessationCostProfile } from "../models/assets/surf/SurfCessationCostProfile"
import AssetCurrency from "../Components/AssetCurrency"
import NumberInputInherited from "../Components/NumberInputInherited"
import ArtificialLiftInherited from "../Components/ArtificialLiftInherited"
import ApprovedBy from "../Components/ApprovedBy"
import DGDateInherited from "../Components/DGDateInherited"
import { IAssetService } from "../Services/IAssetService"

const SurfView = () => {
    const [project, setProject] = useState<Project>()
    const [caseItem, setCase] = useState<Case>()
    const [surf, setSurf] = useState<Surf>()
    const [hasChanges, setHasChanges] = useState(false)
    const [surfName, setSurfName] = useState<string>("")
    const { fusionContextId, caseId, surfId } = useParams<Record<string, string | undefined>>()
    const currentProject = useCurrentContext()

    const [firstTSYear, setFirstTSYear] = useState<number>()
    const [lastTSYear, setLastTSYear] = useState<number>()
    const [riserCount, setRiserCount] = useState<number | undefined>()
    const [templateCount, setTemplateCount] = useState<number | undefined>()
    const [producerCount, setProducerCount] = useState<number | undefined>()
    const [gasInjectorCount, setGasInjectorCount] = useState<number | undefined>()
    const [waterInjectorCount, setWaterInjectorCount] = useState<number | undefined>()
    const [infieldPipelineSystemLength, setInfieldPipelineSystemLength] = useState<number | undefined>()
    const [umbilicalSystemLength, setUmbilicalSystemLength] = useState<number | undefined>()
    const [maturity, setMaturity] = useState<Components.Schemas.Maturity | undefined>()
    const [productionFlowline, setProductionFlowline] = useState<Components.Schemas.ProductionFlowline | undefined>()
    const [costProfile, setCostProfile] = useState<SurfCostProfile>()
    const [cessationCostProfile, setCessationCostProfile] = useState<SurfCessationCostProfile>()
    const [currency, setCurrency] = useState<Components.Schemas.Currency>(1)
    const [artificialLift, setArtificialLift] = useState<Components.Schemas.ArtificialLift | undefined>()
    const [costYear, setCostYear] = useState<number | undefined>()
    const [cessationCost, setCessationCost] = useState<number | undefined>()
    const [approvedBy, setApprovedBy] = useState<string>("")
    const [dG3Date, setDG3Date] = useState<Date>()
    const [dG4Date, setDG4Date] = useState<Date>()
    const [surfService, setSurfService] = useState<IAssetService>()

    useEffect(() => {
        (async () => {
            try {
                const projectId = unwrapProjectId(currentProject?.externalId)
                const projectResult = await (await GetProjectService()).getProjectByID(projectId)
                setProject(projectResult)
                const service = await GetSurfService()
                setSurfService(service)
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
                let newSurf = project.surfs.find((s) => s.id === surfId)
                if (newSurf !== undefined) {
                    if (newSurf.DG3Date === null
                        || IsInvalidDate(newSurf.DG3Date)) {
                        newSurf.DG3Date = caseResult?.DG3Date
                    }
                    if (newSurf.DG4Date === null
                        || IsInvalidDate(newSurf.DG4Date)) {
                        newSurf.DG4Date = caseResult?.DG4Date
                    }
                    setSurf(newSurf)
                } else {
                    newSurf = new Surf()
                    newSurf.artificialLift = caseResult?.artificialLift
                    newSurf.producerCount = caseResult?.producerCount
                    newSurf.gasInjectorCount = caseResult?.gasInjectorCount
                    newSurf.waterInjectorCount = caseResult?.waterInjectorCount
                    newSurf.currency = project.currency
                    newSurf.DG3Date = caseResult?.DG3Date
                    newSurf.DG4Date = caseResult?.DG4Date
                    setSurf(newSurf)
                }
                setSurfName(newSurf?.name!)
                setRiserCount(newSurf?.riserCount)
                setTemplateCount(newSurf?.templateCount)
                setProducerCount(newSurf?.producerCount)
                setCostYear(newSurf?.costYear)
                setCessationCost(newSurf?.cessationCost ?? 0)
                setGasInjectorCount(newSurf?.gasInjectorCount)
                setWaterInjectorCount(newSurf?.waterInjectorCount)
                setInfieldPipelineSystemLength(newSurf?.infieldPipelineSystemLength)
                setUmbilicalSystemLength(newSurf?.umbilicalSystemLength)
                setMaturity(newSurf.maturity ?? undefined)
                setProductionFlowline(newSurf.productionFlowline ?? 0)
                setCurrency(newSurf.currency ?? 1)
                setArtificialLift(newSurf.artificialLift)
                setApprovedBy(newSurf?.approvedBy!)
                setDG3Date(newSurf.DG3Date ?? undefined)
                setDG4Date(newSurf.DG4Date ?? undefined)

                setCostProfile(newSurf.costProfile)
                setCessationCostProfile(newSurf.cessationCostProfile)

                if (caseResult?.DG4Date) {
                    const dg4 = newSurf?.source === 1 ? newSurf.DG4Date?.getFullYear()
                        : caseResult.DG4Date.getFullYear()
                    initializeFirstAndLastYear(
                        dg4!,
                        [newSurf.costProfile, newSurf.cessationCostProfile],
                        setFirstTSYear,
                        setLastTSYear,
                    )
                }
            }
        })()
    }, [project])

    useEffect(() => {
        if (surf !== undefined) {
            const newSurf: Surf = { ...surf }
            newSurf.riserCount = riserCount
            newSurf.templateCount = templateCount
            newSurf.producerCount = producerCount
            newSurf.gasInjectorCount = gasInjectorCount
            newSurf.waterInjectorCount = waterInjectorCount
            newSurf.infieldPipelineSystemLength = infieldPipelineSystemLength
            newSurf.umbilicalSystemLength = umbilicalSystemLength
            newSurf.costYear = costYear
            newSurf.cessationCost = cessationCost
            newSurf.maturity = maturity
            newSurf.productionFlowline = productionFlowline
            newSurf.currency = currency
            newSurf.artificialLift = artificialLift
            newSurf.approvedBy = approvedBy
            newSurf.DG3Date = dG3Date
            newSurf.DG4Date = dG4Date
            newSurf.costProfile = costProfile
            newSurf.cessationCostProfile = cessationCostProfile

            if (caseItem?.DG4Date) {
                const dg4 = newSurf?.source === 1 ? newSurf.DG4Date?.getFullYear()
                    : caseItem.DG4Date.getFullYear()
                initializeFirstAndLastYear(
                    dg4!,
                    [costProfile, cessationCostProfile],
                    setFirstTSYear,
                    setLastTSYear,
                )
            }

            setSurf(newSurf)
        }
    }, [riserCount, templateCount, producerCount, gasInjectorCount, waterInjectorCount,
        infieldPipelineSystemLength, umbilicalSystemLength, maturity, productionFlowline,
        costProfile, cessationCostProfile, currency, costYear, cessationCost, approvedBy, artificialLift,
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

    if (!surf || !caseItem) {
        return null
    }

    return (
        <AssetViewDiv>
            <Wrapper>
                <Typography variant="h2">Surf</Typography>
                <Save
                    name={surfName}
                    setHasChanges={setHasChanges}
                    hasChanges={hasChanges}
                    setAsset={setSurf}
                    setProject={setProject}
                    asset={surf!}
                    assetService={surfService!}
                    assetType={AssetTypeEnum.surfs}
                />
                <Typography variant="h6">
                    {surf?.LastChangedDate?.toLocaleString()
                        ? `Last changed: ${surf?.LastChangedDate?.toLocaleString()}` : ""}
                </Typography>
            </Wrapper>
            <AssetName
                setName={setSurfName}
                name={surfName}
                setHasChanges={setHasChanges}
            />
            <ApprovedBy
                setApprovedBy={setApprovedBy}
                approvedBy={approvedBy}
                setHasChanges={setHasChanges}
            />
            <Wrapper>
                <DGDateInherited
                    setHasChanges={setHasChanges}
                    setValue={setDG3Date}
                    dGName="DG3"
                    value={dG3Date}
                    caseValue={caseItem?.DG3Date}
                    disabled={surf?.source === 1}
                />
                <DGDateInherited
                    setHasChanges={setHasChanges}
                    setValue={setDG4Date}
                    dGName="DG4"
                    value={dG4Date}
                    caseValue={caseItem?.DG4Date}
                    disabled={surf?.source === 1}
                />
            </Wrapper>
            <AssetCurrency
                setCurrency={setCurrency}
                setHasChanges={setHasChanges}
                currentValue={currency}
            />
            <Typography>
                {`Prosp version: ${surf?.ProspVersion ? surf?.ProspVersion.toLocaleDateString() : "N/A"}`}
            </Typography>
            <Typography>
                {`Source: ${surf?.source === 0 || surf?.source === undefined ? "ConceptApp" : "Prosp"}`}
            </Typography>
            <Wrapper>
                <WrapperColumn>
                    <ArtificialLiftInherited
                        currentValue={artificialLift}
                        setArtificialLift={setArtificialLift}
                        setHasChanges={setHasChanges}
                        caseArtificialLift={caseItem?.artificialLift}
                    />
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
                <NumberInputInherited
                    setHasChanges={setHasChanges}
                    setValue={setProducerCount}
                    value={producerCount ?? 0}
                    integer
                    label="Producer count"
                    caseValue={caseItem?.producerCount}
                />
                <NumberInputInherited
                    setHasChanges={setHasChanges}
                    setValue={setGasInjectorCount}
                    value={gasInjectorCount ?? 0}
                    integer
                    label="Gas injector count"
                    caseValue={caseItem?.gasInjectorCount}
                />
                <NumberInputInherited
                    setHasChanges={setHasChanges}
                    setValue={setWaterInjectorCount}
                    value={waterInjectorCount ?? 0}
                    integer
                    label="Water injector count"
                    caseValue={caseItem?.waterInjectorCount}
                />
            </Wrapper>
            <Wrapper>
                <NumberInput
                    setHasChanges={setHasChanges}
                    setValue={setRiserCount}
                    value={riserCount ?? 0}
                    integer
                    label="Riser count"
                />
                <NumberInput
                    setHasChanges={setHasChanges}
                    setValue={setTemplateCount}
                    value={templateCount ?? 0}
                    integer
                    label="Template count"
                />
                <NumberInput
                    setHasChanges={setHasChanges}
                    setValue={setInfieldPipelineSystemLength}
                    value={infieldPipelineSystemLength ?? 0}
                    integer={false}
                    label="Length of production lines (km)"
                />
                <NumberInput
                    setHasChanges={setHasChanges}
                    setValue={setUmbilicalSystemLength}
                    value={umbilicalSystemLength ?? 0}
                    integer={false}
                    label="Length of umbilical system (km)"
                />
                <NumberInput
                    setHasChanges={setHasChanges}
                    setValue={setCessationCost}
                    value={cessationCost ?? 0}
                    integer={false}
                    label="Cessation cost"
                />
            </Wrapper>
            <Maturity
                setMaturity={setMaturity}
                currentValue={maturity}
                setHasChanges={setHasChanges}
            />
            <ProductionFlowline
                setHasChanges={setHasChanges}
                currentValue={productionFlowline}
                setProductionFlowline={setProductionFlowline}
            />
            <TimeSeries
                dG4Year={surf.source === 1 ? surf.DG4Date!.getFullYear() : caseItem.DG4Date!.getFullYear()}
                setTimeSeries={setAllStates}
                setHasChanges={setHasChanges}
                timeSeries={[costProfile, cessationCostProfile]}
                firstYear={firstTSYear!}
                lastYear={lastTSYear!}
                profileName={["Cost profile", "Cessation cost profile"]}
                profileEnum={currency}
                profileType="Cost"
                readOnlyTimeSeries={[]}
                readOnlyName={[]}
            />
        </AssetViewDiv>
    )
}

export default SurfView

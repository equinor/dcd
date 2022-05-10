import { useEffect, useState } from "react"
import {
    Input, Label, Typography,
} from "@equinor/eds-core-react"

import { useParams } from "react-router"
import { Surf } from "../models/assets/surf/Surf"
import { Case } from "../models/Case"
import { Project } from "../models/Project"
import { GetProjectService } from "../Services/ProjectService"
import { GetSurfService } from "../Services/SurfService"
import TimeSeries from "../Components/TimeSeries"
import {
    AssetViewDiv, Dg4Field, Wrapper, WrapperColumn,
} from "./Asset/StyledAssetComponents"
import Save from "../Components/Save"
import AssetName from "../Components/AssetName"
import AssetTypeEnum from "../models/assets/AssetTypeEnum"
import { GetArtificialLiftName, initializeFirstAndLastYear } from "./Asset/AssetHelper"
import NumberInput from "../Components/NumberInput"
import Maturity from "../Components/Maturity"
import ProductionFlowline from "../Components/ProductionFlowline"
import { SurfCostProfile } from "../models/assets/surf/SurfCostProfile"
import { SurfCessationCostProfile } from "../models/assets/surf/SurfCessationCostProfile"
import AssetCurrency from "../Components/AssetCurrency"

const SurfView = () => {
    const [project, setProject] = useState<Project>()
    const [caseItem, setCase] = useState<Case>()
    const [surf, setSurf] = useState<Surf>()
    const [hasChanges, setHasChanges] = useState(false)
    const [surfName, setSurfName] = useState<string>("")
    const params = useParams()
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
    const [currency, setCurrency] = useState<Components.Schemas.Currency>(0)

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
                let newSurf = project.surfs.find((s) => s.id === params.surfId)
                if (newSurf !== undefined) {
                    setSurf(newSurf)
                } else {
                    newSurf = new Surf()
                    newSurf.artificialLift = caseResult?.artificialLift
                    newSurf.producerCount = caseResult?.producerCount
                    newSurf.gasInjectorCount = caseResult?.gasInjectorCount
                    newSurf.waterInjectorCount = caseResult?.waterInjectorCount
                    newSurf.currency = project.currency
                    setSurf(newSurf)
                }
                setSurfName(newSurf?.name!)
                setRiserCount(newSurf?.riserCount)
                setTemplateCount(newSurf?.templateCount)
                setProducerCount(newSurf?.producerCount)
                setGasInjectorCount(newSurf?.gasInjectorCount)
                setWaterInjectorCount(newSurf?.waterInjectorCount)
                setInfieldPipelineSystemLength(newSurf?.infieldPipelineSystemLength)
                setUmbilicalSystemLength(newSurf?.umbilicalSystemLength)
                setMaturity(newSurf.maturity ?? undefined)
                setProductionFlowline(newSurf.productionFlowline ?? 0)
                setCurrency(newSurf.currency ?? 0)

                setCostProfile(newSurf.costProfile)
                setCessationCostProfile(newSurf.cessationCostProfile)

                if (caseResult?.DG4Date) {
                    initializeFirstAndLastYear(
                        caseResult?.DG4Date?.getFullYear(),
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
            newSurf.maturity = maturity
            newSurf.productionFlowline = productionFlowline
            newSurf.currency = currency

            newSurf.costProfile = costProfile
            newSurf.cessationCostProfile = cessationCostProfile

            if (caseItem?.DG4Date) {
                initializeFirstAndLastYear(
                    caseItem?.DG4Date?.getFullYear(),
                    [costProfile, cessationCostProfile],
                    setFirstTSYear,
                    setLastTSYear,
                )
            }

            setSurf(newSurf)
        }
    }, [riserCount, templateCount, producerCount, gasInjectorCount, waterInjectorCount,
        infieldPipelineSystemLength, umbilicalSystemLength, maturity, productionFlowline,
        costProfile, cessationCostProfile, currency])

    return (
        <AssetViewDiv>
            <Typography variant="h2">Surf</Typography>
            <AssetName
                setName={setSurfName}
                name={surfName}
                setHasChanges={setHasChanges}
            />
            <AssetCurrency
                setCurrency={setCurrency}
                setHasChanges={setHasChanges}
                currentValue={currency}
            />
            <Wrapper>
                <WrapperColumn>
                    <Label htmlFor="name" label="Artificial lift" />
                    <Input
                        id="artificialLift"
                        disabled
                        defaultValue={GetArtificialLiftName(surf?.artificialLift)}
                    />
                </WrapperColumn>
            </Wrapper>

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
            <Wrapper>
                <NumberInput
                    setHasChanges={setHasChanges}
                    setValue={setProducerCount}
                    value={producerCount ?? 0}
                    integer
                    disabled
                    label="Producer count"
                />
                <NumberInput
                    setHasChanges={setHasChanges}
                    setValue={setGasInjectorCount}
                    value={gasInjectorCount ?? 0}
                    integer
                    disabled
                    label="Gas injector count"
                />
                <NumberInput
                    setHasChanges={setHasChanges}
                    setValue={setWaterInjectorCount}
                    value={waterInjectorCount ?? 0}
                    integer
                    disabled
                    label="Water injector count"
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
                    integer
                    label={`Length of production lines ${project?.physUnit === 0 ? "(km)" : "(Oilfield)"}`}
                />
                <NumberInput
                    setHasChanges={setHasChanges}
                    setValue={setUmbilicalSystemLength}
                    value={umbilicalSystemLength ?? 0}
                    integer
                    label={`Length of umbilical system ${project?.physUnit === 0 ? "(km)" : "(Oilfield)"}`}
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
                name={surfName}
                setHasChanges={setHasChanges}
                hasChanges={hasChanges}
                setAsset={setSurf}
                setProject={setProject}
                asset={surf!}
                assetService={GetSurfService()}
                assetType={AssetTypeEnum.surfs}
            />
        </AssetViewDiv>
    )
}

export default SurfView

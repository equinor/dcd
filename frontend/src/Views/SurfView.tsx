/* eslint-disable linebreak-style */
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
import TimeSeriesEnum from "../models/assets/TimeSeriesEnum"
import TimeSeries from "../Components/TimeSeries"
import {
    AssetViewDiv, Dg4Field, Wrapper, WrapperColumn,
} from "./Asset/StyledAssetComponents"
import Save from "../Components/Save"
import AssetName from "../Components/AssetName"
import AssetTypeEnum from "../models/assets/AssetTypeEnum"
import { GetArtificialLiftName, TimeSeriesYears } from "./Asset/AssetHelper"
import NumberInput from "../Components/NumberInput"
import Maturity from "../Components/Maturity"
import ProductionFlowline from "../Components/ProductionFlowline"

const SurfView = () => {
    const [project, setProject] = useState<Project>()
    const [caseItem, setCase] = useState<Case>()
    const [surf, setSurf] = useState<Surf>()
    const [hasChanges, setHasChanges] = useState(false)
    const [surfName, setSurfName] = useState<string>("")
    const params = useParams()
    const [earliestTimeSeriesYear, setEarliestTimeSeriesYear] = useState<number>()
    const [latestTimeSeriesYear, setLatestTimeSeriesYear] = useState<number>()
    const [riserCount, setRiserCount] = useState<number | undefined>()
    const [templateCount, setTemplateCount] = useState<number | undefined>()
    const [producerCount, setProducerCount] = useState<number | undefined>()
    const [gasInjectorCount, setGasInjectorCount] = useState<number | undefined>()
    const [waterInjectorCount, setWaterInjectorCount] = useState<number | undefined>()
    const [infieldPipelineSystemLength, setInfieldPipelineSystemLength] = useState<number | undefined>()
    const [umbilicalSystemLength, setUmbilicalSystemLength] = useState<number | undefined>()
    const [maturity, setMaturity] = useState<Components.Schemas.Maturity | undefined>()
    const [productionFlowline, setProductionFlowline] = useState<Components.Schemas.ProductionFlowline | undefined>()

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

                TimeSeriesYears(
                    newSurf,
                    caseResult!.DG4Date!.getFullYear(),
                    setEarliestTimeSeriesYear,
                    setLatestTimeSeriesYear,
                )
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
            setSurf(newSurf)
        }
    }, [riserCount, templateCount, producerCount, gasInjectorCount, waterInjectorCount,
        infieldPipelineSystemLength, umbilicalSystemLength, maturity, productionFlowline])

    return (
        <AssetViewDiv>
            <Typography variant="h2">Surf</Typography>
            <AssetName
                setName={setSurfName}
                name={surfName}
                setHasChanges={setHasChanges}
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
                    label="Length of production lines"
                />
                <NumberInput
                    setHasChanges={setHasChanges}
                    setValue={setUmbilicalSystemLength}
                    value={umbilicalSystemLength ?? 0}
                    integer
                    label="Length of umbilical system"
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
                caseItem={caseItem}
                setAsset={setSurf}
                setHasChanges={setHasChanges}
                asset={surf}
                timeSeriesType={TimeSeriesEnum.costProfile}
                assetName={surfName}
                timeSeriesTitle="Cost profile"
                earliestYear={earliestTimeSeriesYear!}
                latestYear={latestTimeSeriesYear!}
                setEarliestYear={setEarliestTimeSeriesYear!}
                setLatestYear={setLatestTimeSeriesYear}
            />
            <TimeSeries
                caseItem={caseItem}
                setAsset={setSurf}
                setHasChanges={setHasChanges}
                asset={surf}
                timeSeriesType={TimeSeriesEnum.surfCessationCostProfileDto}
                assetName={surfName}
                timeSeriesTitle="Cessation Cost profile"
                earliestYear={earliestTimeSeriesYear!}
                latestYear={latestTimeSeriesYear!}
                setEarliestYear={setEarliestTimeSeriesYear!}
                setLatestYear={setLatestTimeSeriesYear}
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

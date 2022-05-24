import {
    Input, Label, Typography,
} from "@equinor/eds-core-react"
import { useEffect, useState } from "react"
import {
    useParams,
} from "react-router"
import Save from "../Components/Save"
import AssetName from "../Components/AssetName"
import TimeSeries from "../Components/TimeSeries"
import { Topside } from "../models/assets/topside/Topside"
import { Case } from "../models/Case"
import { Project } from "../models/Project"
import { GetProjectService } from "../Services/ProjectService"
import { GetTopsideService } from "../Services/TopsideService"
import { unwrapCase, unwrapProjectId } from "../Utils/common"
import { GetArtificialLiftName, initializeFirstAndLastYear } from "./Asset/AssetHelper"
import {
    AssetViewDiv, Dg4Field, Wrapper, WrapperColumn,
} from "./Asset/StyledAssetComponents"
import AssetTypeEnum from "../models/assets/AssetTypeEnum"
import Maturity from "../Components/Maturity"
import NumberInput from "../Components/NumberInput"
import { TopsideCostProfile } from "../models/assets/topside/TopsideCostProfile"
import { TopsideCessationCostProfile } from "../models/assets/topside/TopsideCessationCostProfile"
import AssetCurrency from "../Components/AssetCurrency"

const TopsideView = () => {
    const [project, setProject] = useState<Project>()
    const [caseItem, setCase] = useState<Case>()
    const [topside, setTopside] = useState<Topside>()
    const [hasChanges, setHasChanges] = useState(false)
    const [topsideName, setTopsideName] = useState<string>("")
    const params = useParams()
    const [firstTSYear, setFirstTSYear] = useState<number>()
    const [lastTSYear, setLastTSYear] = useState<number>()
    const [oilCapacity, setOilCapacity] = useState<number | undefined>()
    const [gasCapacity, setGasCapacity] = useState<number | undefined>()
    const [dryweight, setDryweight] = useState<number | undefined>()
    const [maturity, setMaturity] = useState<Components.Schemas.Maturity | undefined>()
    const [costProfile, setCostProfile] = useState<TopsideCostProfile>()
    const [cessationCostProfile, setCessationCostProfile] = useState<TopsideCessationCostProfile>()
    const [currency, setCurrency] = useState<Components.Schemas.Currency>(0)
    const [cO2ShareOilProfile, setCO2ShareOilProfile] = useState<number | undefined>()
    const [cO2ShareGasProfile, setCO2ShareGasProfile] = useState<number | undefined>()
    const [cO2ShareWaterInjectionProfile, setCO2ShareWaterInjectionProfile] = useState<number | undefined>()
    const [cO2OnMaxOilProfile, setCO2OnMaxOilProfile] = useState<number | undefined>()
    const [cO2OnMaxGasProfile, setCO2OnMaxGasProfile] = useState<number | undefined>()
    const [cO2OnMaxWaterInjectionProfile, setCO2OnMaxWaterInjectionProfile] = useState<number | undefined>()

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
                let newTopside: Topside | undefined = project.topsides.find((s) => s.id === params.topsideId)
                if (newTopside !== undefined) {
                    setTopside(newTopside)
                } else {
                    newTopside = new Topside()
                    newTopside.artificialLift = caseResult?.artificialLift
                    newTopside.currency = project.currency
                    setTopside(newTopside)
                }
                setTopsideName(newTopside?.name!)
                setDryweight(newTopside?.dryWeight)
                setOilCapacity(newTopside?.oilCapacity)
                setGasCapacity(newTopside?.gasCapacity)
                setMaturity(newTopside?.maturity ?? undefined)
                setCurrency(newTopside.currency ?? 0)

                setCostProfile(newTopside.costProfile)
                setCessationCostProfile(newTopside.cessationCostProfile)

                if (caseResult?.DG4Date) {
                    initializeFirstAndLastYear(
                        caseResult?.DG4Date?.getFullYear(),
                        [newTopside.costProfile, newTopside.cessationCostProfile],
                        setFirstTSYear,
                        setLastTSYear,
                    )
                }
            }
        })()
    }, [project])

    useEffect(() => {
        if (topside !== undefined) {
            const newTopside: Topside = { ...topside }
            newTopside.dryWeight = dryweight
            newTopside.oilCapacity = oilCapacity
            newTopside.gasCapacity = gasCapacity
            newTopside.maturity = maturity
            newTopside.costProfile = costProfile
            newTopside.cessationCostProfile = cessationCostProfile
            newTopside.currency = currency
            if (caseItem?.DG4Date) {
                initializeFirstAndLastYear(
                    caseItem?.DG4Date?.getFullYear(),
                    [costProfile, cessationCostProfile],
                    setFirstTSYear,
                    setLastTSYear,
                )
            }
            setTopside(newTopside)
        }
    }, [dryweight, oilCapacity, gasCapacity, maturity, costProfile, cessationCostProfile, currency])

    return (
        <AssetViewDiv>
            <Wrapper>
                <Typography variant="h2">Topside</Typography>
                <Save
                    name={topsideName}
                    setHasChanges={setHasChanges}
                    hasChanges={hasChanges}
                    setAsset={setTopside}
                    setProject={setProject}
                    asset={topside!}
                    assetService={GetTopsideService()}
                    assetType={AssetTypeEnum.topsides}
                />
            </Wrapper>
            <AssetName
                setName={setTopsideName}
                name={topsideName}
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
                <WrapperColumn>
                    <Label htmlFor="name" label="Artificial lift" />
                    <Input
                        id="artificialLift"
                        disabled
                        defaultValue={GetArtificialLiftName(topside?.artificialLift)}
                    />
                </WrapperColumn>
            </Wrapper>
            <Wrapper>
                <NumberInput
                    setHasChanges={setHasChanges}
                    setValue={setDryweight}
                    value={dryweight ?? 0}
                    integer
                    label={`Topside dry weight ${project?.physUnit === 0 ? "(tonnes)" : "(Oilfield)"}`}
                />
                <NumberInput
                    setHasChanges={setHasChanges}
                    setValue={setOilCapacity}
                    value={oilCapacity ?? 0}
                    integer={false}
                    label={`Capacity oil ${project?.physUnit === 0 ? "(Sm³/sd)" : "(Oilfield)"}`}
                />
                <NumberInput
                    setHasChanges={setHasChanges}
                    setValue={setGasCapacity}
                    value={gasCapacity ?? 0}
                    integer={false}
                    label={`Capacity gas ${project?.physUnit === 0 ? "(MSm³/sd)" : "(Oilfield)"}`}
                />
                <NumberInput
                    value={caseItem?.facilitiesAvailability ?? 0}
                    integer={false}
                    disabled
                    label={`Facilities availability ${project?.physUnit === 0 ? "(%)" : "(Oilfield)"}`}
                />
            </Wrapper>
            <Wrapper>
                <NumberInput
                    setHasChanges={setHasChanges}
                    setValue={setCO2ShareOilProfile}
                    value={cO2ShareOilProfile ?? 0}
                    integer
                    label="CO2 Share Oil Profile (%)"
                />
                <NumberInput
                    setHasChanges={setHasChanges}
                    setValue={setCO2ShareGasProfile}
                    value={cO2ShareGasProfile ?? 0}
                    integer
                    label="CO2 Share Gas Profile (%)"
                />
                <NumberInput
                    setHasChanges={setHasChanges}
                    setValue={setCO2ShareWaterInjectionProfile}
                    value={cO2ShareWaterInjectionProfile ?? 0}
                    integer
                    label="CO2 Share Water Injection Profile (%)"
                />
            </Wrapper>
            <Wrapper>
                <NumberInput
                    setHasChanges={setHasChanges}
                    setValue={setCO2OnMaxOilProfile}
                    value={cO2OnMaxOilProfile ?? 0}
                    integer
                    label="CO2 On Max Oil Profile (%)"
                />
                <NumberInput
                    setHasChanges={setHasChanges}
                    setValue={setCO2OnMaxGasProfile}
                    value={cO2OnMaxGasProfile ?? 0}
                    integer
                    label="CO2 On Max Gas Profile (%)"
                />
                <NumberInput
                    setHasChanges={setHasChanges}
                    setValue={setCO2OnMaxWaterInjectionProfile}
                    value={cO2OnMaxWaterInjectionProfile ?? 0}
                    integer
                    label="CO2 On Max Water Injection Profile (%)"
                />
            </Wrapper>
            <Maturity
                setMaturity={setMaturity}
                currentValue={maturity}
                setHasChanges={setHasChanges}
            />
            <Wrapper>
                <Input
                    defaultValue={topside?.prospVersion}
                    key={topside?.prospVersion}
                    type="string"
                    name="Prosp version"
                />
                <Input
                    defaultValue={topside?.lastChanged?.toLocaleDateString("en-CA")}
                    key={topside?.lastChanged?.toLocaleDateString("en-CA")}
                    type="date"
                    name="Last changed"
                />
                <Input
                    defaultValue={topside?.costYear?.toLocaleDateString("en-CA")}
                    key={topside?.costYear?.toLocaleDateString("en-CA")}
                    type="date"
                    name="Cost year"
                />
            </Wrapper>
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
        </AssetViewDiv>
    )
}

export default TopsideView

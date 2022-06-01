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
import ApprovedBy from "../Components/ApprovedBy"

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
    const [costYear, setCostYear] = useState<number | undefined>()
    const [approvedBy, setApprovedBy] = useState<string>("")

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
                setCostYear(newTopside?.costYear)
                setCO2ShareOilProfile(newTopside?.cO2ShareOilProfile)
                setCO2ShareGasProfile(newTopside?.cO2ShareGasProfile)
                setCO2ShareWaterInjectionProfile(newTopside?.cO2ShareWaterInjectionProfile)
                setCO2OnMaxOilProfile(newTopside?.cO2OnMaxOilProfile)
                setCO2OnMaxGasProfile(newTopside?.cO2OnMaxGasProfile)
                setCO2OnMaxWaterInjectionProfile(newTopside?.cO2OnMaxWaterInjectionProfile)
                setApprovedBy(newTopside?.approvedBy!)
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
            newTopside.costYear = costYear
            newTopside.cO2ShareOilProfile = cO2ShareOilProfile
            newTopside.cO2ShareGasProfile = cO2ShareGasProfile
            newTopside.cO2ShareWaterInjectionProfile = cO2ShareWaterInjectionProfile
            newTopside.cO2OnMaxOilProfile = cO2OnMaxOilProfile
            newTopside.cO2OnMaxGasProfile = cO2OnMaxGasProfile
            newTopside.cO2OnMaxWaterInjectionProfile = cO2OnMaxWaterInjectionProfile
            newTopside.approvedBy = approvedBy

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
    }, [dryweight, oilCapacity, gasCapacity, maturity, costProfile, cessationCostProfile, currency, costYear,
        cO2ShareOilProfile, cO2ShareGasProfile, cO2ShareWaterInjectionProfile, cO2OnMaxOilProfile, cO2OnMaxGasProfile,
        cO2OnMaxWaterInjectionProfile, approvedBy])

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
                <Typography variant="h6">
                    {topside?.LastChangedDate?.toLocaleString()
                        ? `Last changed: ${topside?.LastChangedDate?.toLocaleString()}` : ""}
                </Typography>
            </Wrapper>
            <AssetName
                setName={setTopsideName}
                name={topsideName}
                setHasChanges={setHasChanges}
            />
            <ApprovedBy
                setApprovedBy={setApprovedBy}
                approvedBy={approvedBy}
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
            <Typography>
                {`Prosp version: ${topside?.ProspVersion ? topside?.ProspVersion.toLocaleDateString("en-CA") : "N/A"}`}
            </Typography>
            <Typography>
                {`Source: ${topside?.source === 0 || topside?.source === undefined ? "ConceptApp" : "Prosp"}`}
            </Typography>
            <Wrapper>
                <WrapperColumn>
                    <Label htmlFor="name" label="Artificial lift" />
                    <Input
                        id="artificialLift"
                        disabled
                        defaultValue={GetArtificialLiftName(topside?.artificialLift)}
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

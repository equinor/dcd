import { Typography } from "@equinor/eds-core-react"
import { useEffect, useState } from "react"
import {
    useParams,
} from "react-router-dom"
import { useCurrentContext } from "@equinor/fusion"
import Save from "../Components/Save"
import AssetName from "../Components/AssetName"
import TimeSeries from "../Components/TimeSeries"
import { Topside } from "../models/assets/topside/Topside"
import { Case } from "../models/case/Case"
import { Project } from "../models/Project"
import { GetProjectService } from "../Services/ProjectService"
import { GetTopsideService } from "../Services/TopsideService"
import { IsInvalidDate, unwrapCase, unwrapProjectId } from "../Utils/common"
import { initializeFirstAndLastYear } from "./Asset/AssetHelper"
import {
    AssetViewDiv, Wrapper, WrapperColumn,
} from "./Asset/StyledAssetComponents"
import AssetTypeEnum from "../models/assets/AssetTypeEnum"
import Maturity from "../Components/Maturity"
import NumberInput from "../Components/NumberInput"
import { TopsideCostProfile } from "../models/assets/topside/TopsideCostProfile"
import { TopsideCessationCostProfile } from "../models/assets/topside/TopsideCessationCostProfile"
import AssetCurrency from "../Components/AssetCurrency"
import NumberInputInherited from "../Components/NumberInputInherited"
import ArtificialLiftInherited from "../Components/ArtificialLiftInherited"
import ApprovedBy from "../Components/ApprovedBy"
import DGDateInherited from "../Components/DGDateInherited"
import { IAssetService } from "../Services/IAssetService"

const TopsideView = () => {
    const [project, setProject] = useState<Project>()
    const [caseItem, setCase] = useState<Case>()
    const [topside, setTopside] = useState<Topside>()
    const [hasChanges, setHasChanges] = useState(false)
    const [topsideName, setTopsideName] = useState<string>("")
    const { fusionContextId, caseId, topsideId } = useParams<Record<string, string | undefined>>()
    const currentProject = useCurrentContext()
    const [firstTSYear, setFirstTSYear] = useState<number>()
    const [lastTSYear, setLastTSYear] = useState<number>()
    const [oilCapacity, setOilCapacity] = useState<number | undefined>()
    const [gasCapacity, setGasCapacity] = useState<number | undefined>()
    const [dryweight, setDryweight] = useState<number | undefined>()
    const [maturity, setMaturity] = useState<Components.Schemas.Maturity | undefined>()
    const [costProfile, setCostProfile] = useState<TopsideCostProfile>()
    const [cessationCostProfile, setCessationCostProfile] = useState<TopsideCessationCostProfile>()
    const [currency, setCurrency] = useState<Components.Schemas.Currency>(1)
    const [facilitiesAvailability, setFacilitiesAvailability] = useState<number>()
    const [artificialLift, setArtificialLift] = useState<Components.Schemas.ArtificialLift | undefined>()
    const [cO2ShareOilProfile, setCO2ShareOilProfile] = useState<number | undefined>()
    const [cO2ShareGasProfile, setCO2ShareGasProfile] = useState<number | undefined>()
    const [cO2ShareWaterInjectionProfile, setCO2ShareWaterInjectionProfile] = useState<number | undefined>()
    const [cO2OnMaxOilProfile, setCO2OnMaxOilProfile] = useState<number | undefined>()
    const [cO2OnMaxGasProfile, setCO2OnMaxGasProfile] = useState<number | undefined>()
    const [cO2OnMaxWaterInjectionProfile, setCO2OnMaxWaterInjectionProfile] = useState<number | undefined>()
    const [costYear, setCostYear] = useState<number | undefined>()
    const [approvedBy, setApprovedBy] = useState<string>("")
    const [producerCount, setProducerCount] = useState<number | undefined>()
    const [gasInjectorCount, setGasInjectorCount] = useState<number | undefined>()
    const [waterInjectorCount, setWaterInjectorCount] = useState<number | undefined>()
    const [fuelConsumption, setFuelConsumption] = useState<number | undefined>()
    const [flaredGas, setFlaredGas] = useState<number | undefined>()
    const [dG3Date, setDG3Date] = useState<Date>()
    const [dG4Date, setDG4Date] = useState<Date>()
    const [topsideService, setTopsideService] = useState<IAssetService>()
    const [facilityOpex, setFacilityOpex] = useState<number | undefined>()

    useEffect(() => {
        (async () => {
            try {
                const projectId = unwrapProjectId(currentProject?.externalId)
                const projectResult = await (await GetProjectService()).getProjectByID(projectId)
                setProject(projectResult)
                const service = await GetTopsideService()
                setTopsideService(service)
            } catch (error) {
                console.error(`[CaseView] Error while fetching project ${currentProject}`, error)
            }
        })()
    }, [])

    useEffect(() => {
        (async () => {
            if (project !== undefined) {
                const caseResult = unwrapCase(project.cases.find((o) => o.id === caseId))
                setCase(caseResult)
                let newTopside = project.topsides.find((s) => s.id === topsideId)
                if (newTopside !== undefined) {
                    if (newTopside.DG3Date === null
                        || IsInvalidDate(newTopside.DG3Date)) {
                        newTopside.DG3Date = caseResult?.DG3Date
                    }
                    if (newTopside.DG4Date === null
                        || IsInvalidDate(newTopside.DG4Date)) {
                        newTopside.DG4Date = caseResult?.DG4Date
                    }
                    setTopside(newTopside)
                } else {
                    newTopside = new Topside()
                    newTopside.artificialLift = caseResult?.artificialLift
                    newTopside.currency = project.currency
                    newTopside.facilitiesAvailability = caseResult?.facilitiesAvailability
                    newTopside.producerCount = caseResult?.producerCount
                    newTopside.gasInjectorCount = caseResult?.gasInjectorCount
                    newTopside.waterInjectorCount = caseResult?.waterInjectorCount
                    newTopside.DG3Date = caseResult?.DG3Date
                    newTopside.DG4Date = caseResult?.DG4Date
                    setTopside(newTopside)
                }
                setTopsideName(newTopside?.name!)
                setDryweight(newTopside?.dryWeight)
                setOilCapacity(newTopside?.oilCapacity)
                setGasCapacity(newTopside?.gasCapacity)
                setMaturity(newTopside?.maturity ?? undefined)
                setCurrency(newTopside.currency ?? 1)
                setFacilitiesAvailability(newTopside?.facilitiesAvailability)
                setArtificialLift(newTopside.artificialLift)
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
                setProducerCount(newTopside?.producerCount)
                setGasInjectorCount(newTopside?.gasInjectorCount)
                setWaterInjectorCount(newTopside?.waterInjectorCount)
                setFuelConsumption(newTopside?.fuelConsumption)
                setFlaredGas(newTopside?.flaredGas)
                setDG3Date(newTopside.DG3Date ?? undefined)
                setDG4Date(newTopside.DG4Date ?? undefined)
                setFacilityOpex(newTopside?.facilityOpex)

                if (caseResult?.DG4Date) {
                    const dg4 = newTopside?.source === 1 ? newTopside.DG4Date?.getFullYear()
                        : caseResult.DG4Date.getFullYear()
                    initializeFirstAndLastYear(
                        dg4!,
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
            newTopside.facilitiesAvailability = facilitiesAvailability
            newTopside.artificialLift = artificialLift
            newTopside.costYear = costYear
            newTopside.cO2ShareOilProfile = cO2ShareOilProfile
            newTopside.cO2ShareGasProfile = cO2ShareGasProfile
            newTopside.cO2ShareWaterInjectionProfile = cO2ShareWaterInjectionProfile
            newTopside.cO2OnMaxOilProfile = cO2OnMaxOilProfile
            newTopside.cO2OnMaxGasProfile = cO2OnMaxGasProfile
            newTopside.cO2OnMaxWaterInjectionProfile = cO2OnMaxWaterInjectionProfile
            newTopside.approvedBy = approvedBy
            newTopside.producerCount = producerCount
            newTopside.gasInjectorCount = gasInjectorCount
            newTopside.waterInjectorCount = waterInjectorCount
            newTopside.fuelConsumption = fuelConsumption
            newTopside.flaredGas = flaredGas
            newTopside.DG3Date = dG3Date
            newTopside.DG4Date = dG4Date
            newTopside.facilityOpex = facilityOpex

            if (caseItem?.DG4Date) {
                const dg4 = newTopside?.source === 1 ? newTopside.DG4Date?.getFullYear()
                    : caseItem.DG4Date.getFullYear()
                initializeFirstAndLastYear(
                    dg4!,
                    [costProfile, cessationCostProfile],
                    setFirstTSYear,
                    setLastTSYear,
                )
            }
            setTopside(newTopside)
        }
    }, [dryweight, oilCapacity, gasCapacity, maturity, costProfile, cessationCostProfile, currency, costYear,
        cO2ShareOilProfile, cO2ShareGasProfile, cO2ShareWaterInjectionProfile, cO2OnMaxOilProfile, cO2OnMaxGasProfile,
        cO2OnMaxWaterInjectionProfile, approvedBy, facilitiesAvailability, artificialLift,
        producerCount, gasInjectorCount, waterInjectorCount, fuelConsumption, flaredGas, dG3Date, dG4Date,
        facilityOpex])

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

    if (!topside || !caseItem) {
        return null
    }

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
                    assetService={topsideService!}
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
                <DGDateInherited
                    setHasChanges={setHasChanges}
                    setValue={setDG3Date}
                    dGName="DG3"
                    value={dG3Date}
                    caseValue={caseItem?.DG3Date}
                    disabled={topside?.source === 1}
                />
                <DGDateInherited
                    setHasChanges={setHasChanges}
                    setValue={setDG4Date}
                    dGName="DG4"
                    value={dG4Date}
                    caseValue={caseItem?.DG4Date}
                    disabled={topside?.source === 1}
                />
            </Wrapper>
            <AssetCurrency
                setCurrency={setCurrency}
                setHasChanges={setHasChanges}
                currentValue={currency}
            />
            <Typography>
                {`Prosp version: ${topside?.ProspVersion ? topside?.ProspVersion.toLocaleDateString() : "N/A"}`}
            </Typography>
            <Typography>
                {`Source: ${topside?.source === 0 || topside?.source === undefined ? "ConceptApp" : "Prosp"}`}
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
                <NumberInputInherited
                    setHasChanges={setHasChanges}
                    setValue={setFacilitiesAvailability}
                    value={facilitiesAvailability ?? 0}
                    integer
                    disabled={false}
                    label="Facilities availability (%)"
                    caseValue={caseItem?.facilitiesAvailability}
                />
            </Wrapper>
            <Wrapper>
                <NumberInput
                    setHasChanges={setHasChanges}
                    setValue={setFacilityOpex}
                    value={facilityOpex ?? 0}
                    integer={false}
                    label="Facility opex"
                />
            </Wrapper>
            <Wrapper>
                <NumberInput
                    setHasChanges={setHasChanges}
                    setValue={setFuelConsumption}
                    value={fuelConsumption ?? 0}
                    integer
                    label="Fuel consumption (MSm³ gas/sd)"
                />
                <NumberInput
                    setHasChanges={setHasChanges}
                    setValue={setFlaredGas}
                    value={flaredGas ?? 0}
                    integer={false}
                    label="Flared gas (MSm³ gas/sd)"
                />
            </Wrapper>
            <Wrapper>
                <NumberInput
                    setHasChanges={setHasChanges}
                    setValue={setDryweight}
                    value={dryweight ?? 0}
                    integer
                    label="Topside dry weight (tonnes)"
                />
                <NumberInput
                    setHasChanges={setHasChanges}
                    setValue={setOilCapacity}
                    value={oilCapacity ?? 0}
                    integer={false}
                    label="Capacity oil (Sm³/sd)"
                />
                <NumberInput
                    setHasChanges={setHasChanges}
                    setValue={setGasCapacity}
                    value={gasCapacity ?? 0}
                    integer={false}
                    label="Capacity gas (MSm³/sd)"
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
                dG4Year={topside.source === 1 ? topside.DG4Date!.getFullYear() : caseItem.DG4Date!.getFullYear()}
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

export default TopsideView

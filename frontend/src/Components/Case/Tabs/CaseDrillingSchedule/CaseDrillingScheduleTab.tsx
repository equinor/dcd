import {
    useState,
    useEffect,
} from "react"
import { Typography } from "@equinor/eds-core-react"
import Grid from "@mui/material/Grid2"
import { useMediaQuery } from "@mui/material"

import CaseProductionProfilesTabSkeleton from "@/Components/LoadingSkeletons/CaseProductionProfilesTabSkeleton"
import { SetTableYearsFromProfiles } from "@/Components/Tables/CaseTables/CaseTabTableHelper"
import SwitchableNumberInput from "@/Components/Input/SwitchableNumberInput"
import DateRangePicker from "@/Components/Input/TableDateRangePicker"
import { useAppNavigation } from "@/Hooks/useNavigate"
import { useCaseStore } from "@/Store/CaseStore"
import { useDataFetch, useCaseApiData } from "@/Hooks"
import { getYearFromDateString } from "@/Utils/DateUtils"
import Campaign from "./Components/Campaign"
import {
    CampaignHeader,
    CampaignHeaderTexts,
    CampaignLink,
    FieldsAndDatePickerContainer,
    LinkText,
} from "./Components/SharedCampaignStyles"
import { WellCategory } from "@/Models/enums"

const CaseDrillingScheduleTab = () => {
    const { activeTabCase } = useCaseStore()
    const revisionAndProjectData = useDataFetch()
    const { navigateToProjectTab } = useAppNavigation()
    const { apiData } = useCaseApiData()

    const [startYear, setStartYear] = useState<number>(2020)
    const [endYear, setEndYear] = useState<number>(2030)
    const [tableYears, setTableYears] = useState<[number, number]>([2020, 2030])
    const [yearRangeSetFromProfiles, setYearRangeSetFromProfiles] = useState<boolean>(false)
    const isSmallScreen = useMediaQuery("(max-width: 768px)")

    // DevelopmentWell
    const [oilProducerCount, setOilProducerCount] = useState<number>(0)
    const [gasProducerCount, setGasProducerCount] = useState<number>(0)
    const [waterInjectorCount, setWaterInjectorCount] = useState<number>(0)
    const [gasInjectorCount, setGasInjectorCount] = useState<number>(0)

    // ExplorationWell
    const [explorationWellCount, setExplorationWellCount] = useState<number>(0)
    const [appraisalWellCount, setAppraisalWellCount] = useState<number>(0)

    const [, setSidetrackCount] = useState<number>(0)

    const wells = revisionAndProjectData?.commonProjectAndRevisionData.wells

    useEffect(() => {
        if (activeTabCase === 3 && apiData && !yearRangeSetFromProfiles) {
            const explorationDrillingSchedule = apiData.explorationCampaigns.flatMap((ew) => ew.campaignWells) ?? []
            const developmentDrillingSchedule = apiData.developmentCampaigns.flatMap((ew) => ew.campaignWells) ?? []
            SetTableYearsFromProfiles(
                [...explorationDrillingSchedule, ...developmentDrillingSchedule],
                getYearFromDateString(apiData.case.dG4Date),
                setStartYear,
                setEndYear,
                setTableYears,
            )
            setYearRangeSetFromProfiles(true)
        }
    }, [activeTabCase, apiData])

    const sumWellsForWellCategory = (category: WellCategory): number => {
        if (!apiData) { return 0 }

        if (wells && wells.length > 0) {
            if ([WellCategory.ExplorationWell, WellCategory.AppraisalWell, WellCategory.Sidetrack, WellCategory.RigMobDemob].includes(category)) {
                const filteredWells = wells.filter((w) => w.wellCategory === category)
                let sum = 0
                filteredWells.forEach((fw) => {
                    apiData.explorationCampaigns.flatMap((x) => x.campaignWells).filter((few) => few.wellId === fw.id).forEach((ew) => {
                        if (ew.values && ew.values.length > 0) {
                            sum += ew.values.reduce((a, b) => a + b, 0)
                        }
                    })
                })
                return sum
            }
            const filteredWells = wells.filter((w) => w.wellCategory === category)
            let sum = 0
            filteredWells.forEach((fw) => {
                apiData.developmentCampaigns.flatMap((x) => x.campaignWells).filter((fwpw) => fwpw.wellId === fw.id).forEach((ew) => {
                    if (ew.values && ew.values.length > 0) {
                        sum += ew.values.reduce((a, b) => a + b, 0)
                    }
                })
            })
            return sum
        }
        return 0
    }

    useEffect(() => {
        if (activeTabCase === 3) {
            setOilProducerCount(sumWellsForWellCategory(WellCategory.OilProducer))
            setGasProducerCount(sumWellsForWellCategory(WellCategory.GasProducer))
            setWaterInjectorCount(sumWellsForWellCategory(WellCategory.WaterInjector))
            setGasInjectorCount(sumWellsForWellCategory(WellCategory.GasInjector))
            setExplorationWellCount(sumWellsForWellCategory(WellCategory.ExplorationWell))
            setAppraisalWellCount(sumWellsForWellCategory(WellCategory.AppraisalWell))
            setSidetrackCount(sumWellsForWellCategory(WellCategory.Sidetrack))
        }
    }, [apiData, wells, activeTabCase])

    if (!apiData) { return (<CaseProductionProfilesTabSkeleton />) }

    const {
        case: caseData,
        developmentCampaigns,
        explorationCampaigns,
    } = apiData

    const developmentWellsData = developmentCampaigns.flatMap((x) => x.campaignWells)
    const explorationWellsData = explorationCampaigns.flatMap((x) => x.campaignWells)

    if (
        activeTabCase !== 3
        || !caseData
        || !developmentWellsData
        || !explorationWellsData
    ) { return (<CaseProductionProfilesTabSkeleton />) }

    const handleTableYearsClick = () => {
        setTableYears([startYear, endYear])
    }

    return (
        <Grid container spacing={2}>
            <CampaignHeader $isSmallScreen={isSmallScreen}>
                <CampaignHeaderTexts>
                    <Typography variant="h2">Drilling Schedule</Typography>
                    <LinkText>
                        <Typography variant="ingress">
                            To edit the well costs go to
                            {" "}
                        </Typography>
                        <CampaignLink variant="body_short_link" onClick={() => navigateToProjectTab(2)}>Technical input</CampaignLink>
                    </LinkText>
                    <FieldsAndDatePickerContainer>
                        <Grid container size={12} justifyContent="flex-start">
                            <Grid container size={{ xs: 12, md: 10, lg: 8 }} spacing={2}>
                                <Grid size={{ xs: 12, sm: 6, md: 4 }}>
                                    <SwitchableNumberInput
                                        resourceName="case"
                                        resourcePropertyKey="producerCount"
                                        label="Exploration wells"
                                        previousResourceObject={caseData}
                                        value={explorationWellCount}
                                        integer
                                        disabled
                                    />
                                </Grid>
                                <Grid size={{ xs: 12, sm: 6, md: 4 }}>
                                    <SwitchableNumberInput
                                        resourceName="case"
                                        resourcePropertyKey="producerCount"
                                        label="Oil producer wells"
                                        previousResourceObject={caseData}
                                        value={oilProducerCount}
                                        integer
                                        disabled
                                    />
                                </Grid>
                                <Grid size={{ xs: 12, sm: 6, md: 4 }}>
                                    <SwitchableNumberInput
                                        resourceName="case"
                                        resourcePropertyKey="producerCount"
                                        label="Water injector wells"
                                        previousResourceObject={caseData}
                                        value={waterInjectorCount}
                                        integer
                                        disabled
                                    />
                                </Grid>
                                <Grid size={{ xs: 12, sm: 6, md: 4 }}>
                                    <SwitchableNumberInput
                                        resourceName="case"
                                        resourcePropertyKey="producerCount"
                                        label="Appraisal wells"
                                        previousResourceObject={caseData}
                                        value={appraisalWellCount}
                                        integer
                                        disabled
                                    />
                                </Grid>
                                <Grid size={{ xs: 12, sm: 6, md: 4 }}>
                                    <SwitchableNumberInput
                                        resourceName="case"
                                        resourcePropertyKey="producerCount"
                                        label="Gas producer wells"
                                        previousResourceObject={caseData}
                                        value={gasProducerCount}
                                        integer
                                        disabled
                                    />
                                </Grid>
                                <Grid size={{ xs: 12, sm: 6, md: 4 }}>
                                    <SwitchableNumberInput
                                        resourceName="case"
                                        resourcePropertyKey="producerCount"
                                        label="Gas injector wells"
                                        previousResourceObject={caseData}
                                        value={gasInjectorCount}
                                        integer
                                        disabled
                                    />
                                </Grid>
                            </Grid>
                        </Grid>
                        <DateRangePicker
                            setStartYear={setStartYear}
                            setEndYear={setEndYear}
                            startYear={startYear}
                            endYear={endYear}
                            handleTableYearsClick={handleTableYearsClick}
                        />
                    </FieldsAndDatePickerContainer>
                </CampaignHeaderTexts>
            </CampaignHeader>
            {apiData?.explorationCampaigns?.map((campaign) => (
                <Campaign
                    key={campaign.campaignId}
                    campaign={campaign}
                    tableYears={tableYears}
                    title="Exploration"
                />
            ))}
            {apiData?.developmentCampaigns?.map((campaign) => (
                <Campaign
                    key={campaign.campaignId}
                    campaign={campaign}
                    tableYears={tableYears}
                    title="Development"
                />
            ))}
        </Grid>
    )
}

export default CaseDrillingScheduleTab

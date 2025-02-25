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
// import CaseDrillingScheduleTable from "./CaseDrillingScheduleTable"
import Campaign from "./Components/Campaign"
import {
    CampaignHeader,
    CampaignHeaderTexts,
    CampaignLink,
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
            const explorationDrillingSchedule = apiData.explorationWells?.map((ew) => ew.drillingSchedule) ?? []
            const developmentDrillingSchedule = apiData.developmentWells?.map((ew) => ew.drillingSchedule) ?? []
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
            if ([WellCategory.Exploration_Well, WellCategory.Appraisal_Well, WellCategory.Sidetrack, WellCategory.RigMobDemob].includes(category)) {
                const filteredWells = wells.filter((w) => w.wellCategory === category)
                let sum = 0
                filteredWells.forEach((fw) => {
                    apiData.explorationWells?.filter((few) => few.wellId === fw.id).forEach((ew) => {
                        if (ew.drillingSchedule
                            && ew.drillingSchedule.values
                            && ew.drillingSchedule.values.length > 0) {
                            sum += ew.drillingSchedule.values.reduce((a, b) => a + b, 0)
                        }
                    })
                })
                return sum
            }
            const filteredWells = wells.filter((w) => w.wellCategory === category)
            let sum = 0
            filteredWells.forEach((fw) => {
                apiData.developmentWells?.filter((fwpw) => fwpw.wellId === fw.id).forEach((ew) => {
                    if (ew.drillingSchedule && ew.drillingSchedule.values && ew.drillingSchedule.values.length > 0) {
                        sum += ew.drillingSchedule.values.reduce((a, b) => a + b, 0)
                    }
                })
            })
            return sum
        }
        return 0
    }

    useEffect(() => {
        if (activeTabCase === 3) {
            setOilProducerCount(sumWellsForWellCategory(WellCategory.Oil_Producer))
            setGasProducerCount(sumWellsForWellCategory(WellCategory.Gas_Producer))
            setWaterInjectorCount(sumWellsForWellCategory(WellCategory.Water_Injector))
            setGasInjectorCount(sumWellsForWellCategory(WellCategory.Gas_Injector))
            setExplorationWellCount(sumWellsForWellCategory(WellCategory.Exploration_Well))
            setAppraisalWellCount(sumWellsForWellCategory(WellCategory.Appraisal_Well))
            setSidetrackCount(sumWellsForWellCategory(WellCategory.Sidetrack))
        }
    }, [apiData, wells, activeTabCase])

    if (!apiData) { return (<CaseProductionProfilesTabSkeleton />) }

    const {
        case: caseData,
        explorationId,
        wellProjectId,
        developmentWells: developmentWellsData,
        explorationWells: explorationWellsData,
    } = apiData

    if (
        activeTabCase !== 3
        || !explorationWellsData
        || !caseData
        || !developmentWellsData
        || !explorationId
        || !wellProjectId
    ) { return (<CaseProductionProfilesTabSkeleton />) }

    const handleTableYearsClick = () => {
        setTableYears([startYear, endYear])
    }

    return (
        <Grid container spacing={2}>
            <Grid size={12}>
                <Typography>Create wells in technical input in order to see them in the list below.</Typography>
            </Grid>
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
                </CampaignHeaderTexts>
                <DateRangePicker
                    setStartYear={setStartYear}
                    setEndYear={setEndYear}
                    startYear={startYear}
                    endYear={endYear}
                    handleTableYearsClick={handleTableYearsClick}
                />
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
            {/* <Grid size={12}>
                <CaseDrillingScheduleTable
                    assetWells={explorationWellsData}
                    dg4Year={getYearFromDateString(caseData.dG4Date)}
                    tableName="Exploration wells"
                    tableYears={tableYears}
                    resourceId={explorationId}
                    wells={wells}
                    isExplorationTable
                    gridRef={explorationWellsGridRef}
                    alignedGridsRef={[developmentWellsGridRef]}
                />
            </Grid>
            <Grid size={12}>
                <CaseDrillingScheduleTable
                    assetWells={developmentWellsData}
                    dg4Year={getYearFromDateString(caseData.dG4Date)}
                    tableName="Development wells"
                    tableYears={tableYears}
                    resourceId={wellProjectId}
                    wells={wells}
                    isExplorationTable={false}
                    gridRef={developmentWellsGridRef}
                    alignedGridsRef={[explorationWellsGridRef]}
                />
            </Grid> */}
        </Grid>
    )
}

export default CaseDrillingScheduleTab

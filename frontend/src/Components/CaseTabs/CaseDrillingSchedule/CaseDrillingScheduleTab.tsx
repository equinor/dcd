import { Button, Typography } from "@equinor/eds-core-react"
import { useMediaQuery } from "@mui/material"
import Grid from "@mui/material/Grid2"
import { useQueryClient } from "@tanstack/react-query"
import {
    useState,
    useEffect,
    useMemo,
} from "react"
import { styled } from "styled-components"

import Campaign from "./Components/Campaign"
import {
    CampaignHeader,
    CampaignHeaderTexts,
    CampaignLink,
    FieldsAndDatePickerContainer,
    LinkText,
} from "./Components/SharedCampaignStyles"

import SwitchableNumberInput from "@/Components/Input/SwitchableNumberInput"
import DateRangePicker from "@/Components/Input/TableDateRangePicker"
import CaseProductionProfilesTabSkeleton from "@/Components/LoadingSkeletons/CaseProductionProfilesTabSkeleton"
import { useDataFetch, useCaseApiData, useCanUserEdit } from "@/Hooks"
import { useAppNavigation } from "@/Hooks/useNavigate"
import { CampaignType, WellCategory } from "@/Models/enums"
import { GetDrillingCampaignsService } from "@/Services/DrillingCampaignsService"
import { useAppStore } from "@/Store/AppStore"
import { useCaseStore } from "@/Store/CaseStore"
import { DEFAULT_DRILLING_SCHEDULE_YEARS } from "@/Utils/Config/constants"
import { getYearFromDateString } from "@/Utils/DateUtils"
import { calculateTableYears } from "@/Utils/TableUtils"

const InputGroup = styled.div`
    display: flex;
    align-items: center;
    margin-bottom: 24px;
    gap: 10px;
`

const CaseDrillingScheduleTab = (): React.ReactNode => {
    const { canEdit } = useCanUserEdit()
    const { editMode } = useAppStore()
    const { activeTabCase } = useCaseStore()
    const revisionAndProjectData = useDataFetch()
    const { navigateToProjectTab } = useAppNavigation()
    const { apiData } = useCaseApiData()
    const queryClient = useQueryClient()
    const { setSnackBarMessage, isSaving, setIsSaving } = useAppStore()

    const [startYear, setStartYear] = useState<number>(DEFAULT_DRILLING_SCHEDULE_YEARS[0])
    const [endYear, setEndYear] = useState<number>(DEFAULT_DRILLING_SCHEDULE_YEARS[1])
    const [tableYears, setTableYears] = useState<[number, number]>(DEFAULT_DRILLING_SCHEDULE_YEARS)
    const isSmallScreen = useMediaQuery("(max-width: 768px)")

    const canUserEdit = useMemo(() => canEdit(), [canEdit, activeTabCase, editMode])

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
        if (activeTabCase === 3 && apiData) {
            const explorationDrillingSchedule = apiData.explorationCampaigns.flatMap((ew) => ew.campaignWells) ?? []
            const developmentDrillingSchedule = apiData.developmentCampaigns.flatMap((ew) => ew.campaignWells) ?? []
            const profiles = [...explorationDrillingSchedule, ...developmentDrillingSchedule]

            const dg4Year = getYearFromDateString(apiData.case.dg4Date)
            const years = calculateTableYears(profiles, dg4Year)

            if (years) {
                const [firstYear, lastYear] = years

                setStartYear(firstYear)
                setEndYear(lastYear)
                setTableYears([firstYear, lastYear])
            } else {
                setStartYear(DEFAULT_DRILLING_SCHEDULE_YEARS[0])
                setEndYear(DEFAULT_DRILLING_SCHEDULE_YEARS[1])
                setTableYears(DEFAULT_DRILLING_SCHEDULE_YEARS)
            }
        }
    }, [activeTabCase, apiData])

    const sumWellsForWellCategory = (category: WellCategory): number => {
        if (!apiData) { return 0 }

        if (wells && wells.length > 0) {
            if ([WellCategory.ExplorationWell, WellCategory.AppraisalWell, WellCategory.Sidetrack].includes(category)) {
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

    const developmentWellsData = developmentCampaigns?.flatMap((x) => x.campaignWells)
    const explorationWellsData = explorationCampaigns?.flatMap((x) => x.campaignWells)

    const handleTableYearsClick = (): void => {
        setTableYears([startYear, endYear])
    }

    const createCampaign = async (campaignType: CampaignType): Promise<void> => {
        setIsSaving(true)

        await GetDrillingCampaignsService().createCampaign(
            caseData.projectId,
            caseData.caseId,
            { campaignType },
        ).then(() => queryClient.invalidateQueries({ queryKey: ["caseApiData", caseData.projectId, caseData.caseId] }))
            .catch(() => { setSnackBarMessage("Unable to create campaign") })

        setIsSaving(false)
    }

    if (
        !caseData
        || !developmentWellsData
        || !explorationWellsData
    ) { return (<CaseProductionProfilesTabSkeleton />) }

    if (activeTabCase !== 3) { return null }

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
                        <CampaignLink variant="body_short_link" onClick={(): number => navigateToProjectTab(2)}>Technical input</CampaignLink>
                    </LinkText>
                    <FieldsAndDatePickerContainer>
                        <Grid container size={12} justifyContent="flex-start">
                            <Grid container size={{ xs: 12, md: 10, lg: 8 }} spacing={2}>
                                <Grid size={{ xs: 12, sm: 6, md: 4 }}>
                                    <SwitchableNumberInput
                                        label="Exploration wells"
                                        value={explorationWellCount}
                                        integer
                                        disabled
                                        id={`case-exploration-wells-${caseData.caseId}`}
                                        onSubmit={():void => { }}
                                    />
                                </Grid>
                                <Grid size={{ xs: 12, sm: 6, md: 4 }}>
                                    <SwitchableNumberInput
                                        label="Oil producer wells"
                                        value={oilProducerCount}
                                        integer
                                        disabled
                                        id={`case-oil-producer-wells-${caseData.caseId}`}
                                        onSubmit={():void => { }}
                                    />
                                </Grid>
                                <Grid size={{ xs: 12, sm: 6, md: 4 }}>
                                    <SwitchableNumberInput
                                        label="Water injector wells"
                                        value={waterInjectorCount}
                                        integer
                                        disabled
                                        id={`case-water-injector-wells-${caseData.caseId}`}
                                        onSubmit={():void => { }}
                                    />
                                </Grid>
                                <Grid size={{ xs: 12, sm: 6, md: 4 }}>
                                    <SwitchableNumberInput
                                        label="Appraisal wells"
                                        value={appraisalWellCount}
                                        integer
                                        disabled
                                        id={`case-appraisal-wells-${caseData.caseId}`}
                                        onSubmit={():void => { }}
                                    />
                                </Grid>
                                <Grid size={{ xs: 12, sm: 6, md: 4 }}>
                                    <SwitchableNumberInput
                                        label="Gas producer wells"
                                        value={gasProducerCount}
                                        integer
                                        disabled
                                        id={`case-gas-producer-wells-${caseData.caseId}`}
                                        onSubmit={():void => { }}
                                    />
                                </Grid>
                                <Grid size={{ xs: 12, sm: 6, md: 4 }}>
                                    <SwitchableNumberInput
                                        label="Gas injector wells"
                                        value={gasInjectorCount}
                                        integer
                                        disabled
                                        id={`case-gas-injector-wells-${caseData.caseId}`}
                                        onSubmit={():void => { }}
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
                    {canUserEdit && (
                        <Grid>
                            <Grid>
                                <InputGroup>
                                    <Button onClick={(): Promise<void> => createCampaign(CampaignType.ExplorationCampaign)} variant="contained" disabled={isSaving}>
                                        Create exploration campaign
                                    </Button>
                                    <Button onClick={(): Promise<void> => createCampaign(CampaignType.DevelopmentCampaign)} variant="contained" disabled={isSaving}>
                                        Create development campaign
                                    </Button>
                                </InputGroup>
                            </Grid>
                        </Grid>
                    )}
                </CampaignHeaderTexts>
            </CampaignHeader>
            {apiData.explorationCampaigns.map((campaign) => (
                <Campaign
                    key={campaign.campaignId}
                    campaign={campaign}
                    tableYears={tableYears}
                    title="Exploration"
                />
            ))}
            {apiData.developmentCampaigns.map((campaign) => (
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

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
import { getYearFromDateString } from "@/Utils/DateUtils"
import { SetTableYearsFromProfiles } from "@/Utils/TableUtils"

const InputGroup = styled.div`
    display: flex;
    align-items: center;
    margin-bottom: 24px;
    gap: 10px;
`

const CaseDrillingScheduleTab = () => {
    const { canEdit } = useCanUserEdit()
    const { editMode } = useAppStore()
    const { activeTabCase } = useCaseStore()
    const revisionAndProjectData = useDataFetch()
    const { navigateToProjectTab } = useAppNavigation()
    const { apiData } = useCaseApiData()
    const queryClient = useQueryClient()
    const { setSnackBarMessage, isSaving, setIsSaving } = useAppStore()

    const [startYear, setStartYear] = useState<number>(2020)
    const [endYear, setEndYear] = useState<number>(2030)
    const [tableYears, setTableYears] = useState<[number, number]>([2020, 2030])
    const [yearRangeSetFromProfiles, setYearRangeSetFromProfiles] = useState<boolean>(false)
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
        if (activeTabCase === 3 && apiData && !yearRangeSetFromProfiles) {
            const explorationDrillingSchedule = apiData.explorationCampaigns.flatMap((ew) => ew.campaignWells) ?? []
            const developmentDrillingSchedule = apiData.developmentCampaigns.flatMap((ew) => ew.campaignWells) ?? []

            SetTableYearsFromProfiles(
                [...explorationDrillingSchedule, ...developmentDrillingSchedule],
                getYearFromDateString(apiData.case.dg4Date),
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

    const createCampaign = async (campaignType: CampaignType) => {
        setIsSaving(true)

        await GetDrillingCampaignsService().createCampaign(
            caseData.projectId,
            caseData.caseId,
            { campaignType },
        ).then(() => queryClient.invalidateQueries({ queryKey: ["caseApiData", caseData.projectId, caseData.caseId] }))
            .catch(() => { setSnackBarMessage("Unable to create campaign") })

        setIsSaving(false)
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
                                        label="Exploration wells"
                                        value={explorationWellCount}
                                        integer
                                        disabled
                                        id={`case-exploration-wells-${caseData.caseId}`}
                                        onSubmit={() => { }}
                                    />
                                </Grid>
                                <Grid size={{ xs: 12, sm: 6, md: 4 }}>
                                    <SwitchableNumberInput
                                        label="Oil producer wells"
                                        value={oilProducerCount}
                                        integer
                                        disabled
                                        id={`case-oil-producer-wells-${caseData.caseId}`}
                                        onSubmit={() => { }}
                                    />
                                </Grid>
                                <Grid size={{ xs: 12, sm: 6, md: 4 }}>
                                    <SwitchableNumberInput
                                        label="Water injector wells"
                                        value={waterInjectorCount}
                                        integer
                                        disabled
                                        id={`case-water-injector-wells-${caseData.caseId}`}
                                        onSubmit={() => { }}
                                    />
                                </Grid>
                                <Grid size={{ xs: 12, sm: 6, md: 4 }}>
                                    <SwitchableNumberInput
                                        label="Appraisal wells"
                                        value={appraisalWellCount}
                                        integer
                                        disabled
                                        id={`case-appraisal-wells-${caseData.caseId}`}
                                        onSubmit={() => { }}
                                    />
                                </Grid>
                                <Grid size={{ xs: 12, sm: 6, md: 4 }}>
                                    <SwitchableNumberInput
                                        label="Gas producer wells"
                                        value={gasProducerCount}
                                        integer
                                        disabled
                                        id={`case-gas-producer-wells-${caseData.caseId}`}
                                        onSubmit={() => { }}
                                    />
                                </Grid>
                                <Grid size={{ xs: 12, sm: 6, md: 4 }}>
                                    <SwitchableNumberInput
                                        label="Gas injector wells"
                                        value={gasInjectorCount}
                                        integer
                                        disabled
                                        id={`case-gas-injector-wells-${caseData.caseId}`}
                                        onSubmit={() => { }}
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
                                    <Button onClick={() => createCampaign(CampaignType.ExplorationCampaign)} variant="contained" disabled={isSaving}>
                                        Create exploration campaign
                                    </Button>
                                    <Button onClick={() => createCampaign(CampaignType.DevelopmentCampaign)} variant="contained" disabled={isSaving}>
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

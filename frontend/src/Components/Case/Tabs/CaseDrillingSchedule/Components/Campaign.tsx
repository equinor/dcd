import { useMemo, useRef, useEffect } from "react"
import Grid from "@mui/material/Grid2"

import CaseTabTable from "@/Components/Tables/CaseTables/CaseTabTable"
import { ITimeSeriesTableData, ItimeSeriesTableDataWithWell } from "@/Models/ITimeSeries"
import { getYearFromDateString } from "@/Utils/DateUtils"
import { useCaseApiData } from "@/Hooks/useCaseApiData"
import ProjectSkeleton from "@/Components/LoadingSkeletons/ProjectSkeleton"
import SwitchableNumberInput from "@/Components/Input/SwitchableNumberInput"
import { CampaignFullWidthContainer, CampaignInputsContainer, CampaignTableContainer } from "./SharedCampaignStyles"
import { useDataFetch } from "@/Hooks/useDataFetch"
import { filterWells } from "@/Utils/common"
import useCanUserEdit from "@/Hooks/useCanUserEdit"
import { useAppStore } from "@/Store/AppStore"
import { useCaseStore } from "@/Store/CaseStore"
import { useGridRef } from "@/Store/GridRefContext"

export interface CampaignProps {
    campaign: Components.Schemas.CampaignDto
    title: string
    tableYears: [number, number]
}

const Campaign = ({
    tableYears,
    campaign,
    title,
}: CampaignProps) => {
    const { apiData } = useCaseApiData()
    const campaignGridRef = useRef<any>(null)
    const { addGridRef, gridRefs } = useGridRef()
    const revisionAndProjectData = useDataFetch()
    const { canEdit } = useCanUserEdit()
    const { editMode } = useAppStore()
    const { activeTabCase } = useCaseStore()

    const allWells = useMemo(() => filterWells(revisionAndProjectData?.commonProjectAndRevisionData.wells ?? []), [revisionAndProjectData])
    const canUserEdit = useMemo(() => canEdit(), [canEdit, activeTabCase, editMode])

    useEffect(() => {
        addGridRef(campaignGridRef)
    }, [])

    const generateRowData = (): ItimeSeriesTableDataWithWell[] => {
        const rows: ITimeSeriesTableData[] = []

        const upgradingRow: ITimeSeriesTableData = {
            profileName: "Rig upgrading",
            unit: "Percentage in decimals",
            profile: {
                startYear: campaign.rigUpgradingProfile.startYear,
                values: campaign.rigUpgradingProfile.values || [],
            },
            resourceName: "rigUpgrading",
            resourceId: campaign.campaignId,
            resourcePropertyKey: "rigUpgradingProfile",
            overridable: false,
            editable: true,
        }
        rows.push(upgradingRow)

        const mobDemobRow: ITimeSeriesTableData = {
            profileName: "Rig mob/demob",
            unit: "Percentage in decimals",
            profile: {
                startYear: campaign.rigMobDemobProfile.startYear,
                values: campaign.rigMobDemobProfile.values || [],
            },
            resourceName: "rigMobDemob",
            resourceId: campaign.campaignId,
            resourcePropertyKey: "rigMobDemobProfile",
            overridable: false,
            editable: true,
        }
        rows.push(mobDemobRow)

        const campaignWellsMap = new Map(
            campaign.campaignWells?.map((well) => [well.wellId, well]) || [],
        )

        if (canUserEdit) {
            let availableWells: Components.Schemas.WellOverviewDto[] = []
            if (title === "Exploration") {
                availableWells = allWells.explorationWells
            } else if (title === "Development") {
                availableWells = allWells.developmentWells
            }

            // In edit mode, show all wells in their original order
            availableWells.forEach((well: Components.Schemas.WellOverviewDto) => {
                const campaignWell = campaignWellsMap.get(well.id)
                const wellRow: ItimeSeriesTableDataWithWell = {
                    profileName: well.name,
                    unit: "Well",
                    profile: {
                        startYear: campaignWell?.startYear || 0,
                        values: campaignWell?.values || [],
                    },
                    resourceName: "campaignWells",
                    resourceId: campaign.campaignId,
                    wellId: well.id,
                    resourcePropertyKey: "campaignWells",
                    overridable: false,
                    editable: true,
                }
                rows.push(wellRow)
            })
        } else {
            // In view mode, only show campaign wells
            campaign.campaignWells?.forEach((well: Components.Schemas.CampaignWellDto) => {
                const wellRow: ItimeSeriesTableDataWithWell = {
                    profileName: well.wellName,
                    unit: "Well",
                    profile: {
                        startYear: well.startYear,
                        values: well.values || [],
                    },
                    resourceName: "campaignWells",
                    resourceId: campaign.campaignId,
                    wellId: well.wellId,
                    resourcePropertyKey: "campaignWells",
                    overridable: false,
                    editable: true,
                }
                rows.push(wellRow)
            })
        }

        return rows
    }

    const rowData = useMemo(() => generateRowData(), [campaign, tableYears, canUserEdit])

    if (!apiData) {
        return <ProjectSkeleton />
    }

    return (
        <CampaignFullWidthContainer container spacing={2}>
            <CampaignInputsContainer container size={12} spacing={2}>
                <Grid size={{ xs: 12, sm: 6, md: 5 }}>
                    <SwitchableNumberInput
                        resourceName="rigUpgradingCost"
                        resourcePropertyKey="rigUpgradingCost"
                        label={`Rig upgrading cost - ${title}`}
                        resourceId={campaign.campaignId}
                        previousResourceObject={campaign}
                        value={campaign.rigUpgradingCost}
                        unit="MUSD"
                        integer
                    />
                </Grid>
                <Grid size={{ xs: 12, sm: 6, md: 5 }}>
                    <SwitchableNumberInput
                        resourceName="rigMobDemobCost"
                        resourcePropertyKey="rigMobDemobCost"
                        label={`Rig mob/demob cost - ${title}`}
                        resourceId={campaign.campaignId}
                        previousResourceObject={campaign}
                        value={campaign.rigMobDemobCost}
                        unit="MUSD"
                        integer
                    />
                </Grid>
            </CampaignInputsContainer>
            <Grid size={12}>
                <CampaignTableContainer>
                    <CaseTabTable
                        timeSeriesData={rowData}
                        dg4Year={getYearFromDateString(apiData.case.dg4Date ?? "")}
                        tableYears={tableYears}
                        tableName={`${title} campaign`}
                        totalRowName="Total"
                        gridRef={campaignGridRef}
                        alignedGridsRef={gridRefs}
                        includeFooter={false}
                    />
                </CampaignTableContainer>
            </Grid>
        </CampaignFullWidthContainer>
    )
}

export default Campaign

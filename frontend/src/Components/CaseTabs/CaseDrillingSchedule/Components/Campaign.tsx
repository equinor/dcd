import Grid from "@mui/material/Grid2"
import { useMemo, useRef } from "react"

import { CampaignFullWidthContainer, CampaignInputsContainer, CampaignTableContainer } from "./SharedCampaignStyles"

import SwitchableNumberInput from "@/Components/Input/SwitchableNumberInput"
import ProjectSkeleton from "@/Components/LoadingSkeletons/ProjectSkeleton"
import CaseBaseTable from "@/Components/Tables/CaseBaseTable"
import { useCampaignMutation } from "@/Hooks/Mutations/useCampaignMutation"
import useCanUserEdit from "@/Hooks/useCanUserEdit"
import { useCaseApiData } from "@/Hooks/useCaseApiData"
import { useDataFetch } from "@/Hooks/useDataFetch"
import { ITimeSeriesTableData, ItimeSeriesTableDataWithWell } from "@/Models/ITimeSeries"
import { CampaignProfileType } from "@/Models/enums"
import { useAppStore } from "@/Store/AppStore"
import { useCaseStore } from "@/Store/CaseStore"
import { developmentWellOptions, explorationWellOptions } from "@/Utils/Config/constants"
import { getYearFromDateString } from "@/Utils/DateUtils"
import { isExplorationWell } from "@/Utils/TableUtils"

export interface CampaignProps {
    campaign: Components.Schemas.CampaignDto
    title: string
    tableYears: [number, number]
}

const filterWells = (wells: Components.Schemas.WellOverviewDto[]) => {
    if (!wells) {
        return {
            explorationWells: [],
            developmentWells: [],
            developmentWellOptions,
            explorationWellOptions,
        }
    }

    return {
        explorationWells: wells.filter((well) => isExplorationWell(well)),
        developmentWells: wells.filter((well) => !isExplorationWell(well)),
        developmentWellOptions,
        explorationWellOptions,
    }
}

/**
 * Campaign component displays and manages campaign data including:
 * - Standalone cost input fields (rig upgrading cost, rig mob/demob cost)
 * - Table-based profile edits (rig upgrading profile, rig mob/demob profile, campaign wells)
 */
const Campaign = ({ tableYears, campaign, title }: CampaignProps) => {
    const { apiData } = useCaseApiData()
    const campaignGridRef = useRef<any>(null)
    const revisionAndProjectData = useDataFetch()
    const { canEdit } = useCanUserEdit()
    const { editMode } = useAppStore()
    const { activeTabCase } = useCaseStore()
    const { updateRigUpgradingCost, updateRigMobDemobCost } = useCampaignMutation()

    const allWells = useMemo(() => filterWells(revisionAndProjectData?.commonProjectAndRevisionData.wells ?? []), [revisionAndProjectData])
    const canUserEdit = useMemo(() => canEdit(), [canEdit, activeTabCase, editMode])

    /**
     * Generate table row data for the campaign profiles and wells
     */
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
            resourcePropertyKey: CampaignProfileType.RigUpgrading,
            overridable: false,
            editable: true,
        }

        rows.push(upgradingRow)

        // Add rig mob/demob profile row (table edit)
        const mobDemobRow: ITimeSeriesTableData = {
            profileName: "Rig mob/demob",
            unit: "Percentage in decimals",
            profile: {
                startYear: campaign.rigMobDemobProfile.startYear,
                values: campaign.rigMobDemobProfile.values || [],
            },
            resourceName: "rigMobDemob",
            resourceId: campaign.campaignId,
            resourcePropertyKey: CampaignProfileType.RigMobDemob,
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
                        label={`Rig upgrading cost - ${title}`}
                        value={campaign.rigUpgradingCost}
                        id={`campaign-rig-upgrading-cost-${campaign.campaignId}`}
                        unit="MUSD"
                        integer
                        onSubmit={(newValue) => updateRigUpgradingCost(
                            campaign.campaignId,
                            newValue,
                        )}
                    />
                </Grid>
                <Grid size={{ xs: 12, sm: 6, md: 5 }}>
                    <SwitchableNumberInput
                        label={`Rig mob/demob cost - ${title}`}
                        value={campaign.rigMobDemobCost}
                        id={`campaign-rig-mobdemob-cost-${campaign.campaignId}`}
                        unit="MUSD"
                        integer
                        onSubmit={(newValue) => updateRigMobDemobCost(
                            campaign.campaignId,
                            newValue,
                        )}
                    />
                </Grid>
            </CampaignInputsContainer>
            <Grid size={12}>
                <CampaignTableContainer>
                    <CaseBaseTable
                        timeSeriesData={rowData}
                        dg4Year={getYearFromDateString(apiData.case.dg4Date ?? "")}
                        tableYears={tableYears}
                        tableName={`${title} campaign`}
                        totalRowName="Total"
                        gridRef={campaignGridRef}
                        includeFooter={false}
                    />
                </CampaignTableContainer>
            </Grid>
        </CampaignFullWidthContainer>
    )
}

export default Campaign

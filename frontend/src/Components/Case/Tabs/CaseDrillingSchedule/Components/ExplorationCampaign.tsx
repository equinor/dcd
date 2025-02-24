import { useMemo, useRef } from "react"
import useStyles from "@equinor/fusion-react-ag-grid-styles"
import Grid from "@mui/material/Grid2"

import {
    DrillingCampaignProps,
    Well,
} from "@/Models/ICampaigns"
import CaseTabTable from "@/Components/Tables/CaseTables/CaseTabTable"
import { ITimeSeriesTableData, ItimeSeriesTableDataWithWell } from "@/Models/ITimeSeries"
import { getYearFromDateString } from "@/Utils/DateUtils"
import { useCaseApiData } from "@/Hooks/useCaseApiData"
import ProjectSkeleton from "@/Components/LoadingSkeletons/ProjectSkeleton"

interface ExplorationCampaignProps extends DrillingCampaignProps {
    campaign: Components.Schemas.CampaignDto
}

const ExplorationCampaign = ({ tableYears, campaign }: ExplorationCampaignProps) => {
    const styles = useStyles()
    const { apiData } = useCaseApiData()
    const ExplorationCampaignGridRef = useRef<any>(null)

    const generateRowData = (): ItimeSeriesTableDataWithWell[] => {
        const rows: ITimeSeriesTableData[] = []

        // Add rig mob/demob profile row
        if (campaign.rigMobDemobProfile) {
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
        }

        // Add rig upgrading profile row
        if (campaign.rigUpgradingProfile) {
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
        }

        // Add wells from campaignWells
        campaign.campaignWells?.forEach((well: Well) => {
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

        return rows
    }

    const rowData = useMemo(() => generateRowData(), [campaign, tableYears])

    if (!apiData) {
        return <ProjectSkeleton />
    }

    return (
        <Grid container spacing={2} style={{ width: "100%" }}>
            <Grid container size={12} spacing={2} style={{ maxWidth: "600px" }}>
                {/* <Grid size={{ xs: 12, sm: 6, md: 5 }}>
                    <SwitchableNumberInput
                        resourceName="campaign"
                        resourcePropertyKey="rigUpgradingCost"
                        label="Rig upgrading cost - Exploration"
                        resourceId={campaign.campaignId}
                        previousResourceObject={campaign}
                        value={campaign.rigUpgradingCost}
                        unit="MUSD"
                        integer
                    />
                </Grid>
                <Grid size={{ xs: 12, sm: 6, md: 5 }}>
                    <SwitchableNumberInput
                        resourceName="campaign"
                        resourcePropertyKey="rigMobDemobCost"
                        label="Rig mob/demob cost - Exploration"
                        resourceId={campaign.campaignId}
                        previousResourceObject={campaign}
                        value={campaign.rigMobDemobCost}
                        unit="MUSD"
                        integer
                    />
                </Grid> */}
            </Grid>
            <Grid size={12} style={{ width: "100%" }}>
                <div className={styles.root} style={{ width: "100%" }}>
                    <div style={{
                        display: "flex",
                        flexDirection: "column",
                        width: "100%",
                        height: "400px",
                    }}
                    >
                        <CaseTabTable
                            timeSeriesData={rowData}
                            dg4Year={getYearFromDateString(apiData?.case.dG4Date ?? "")}
                            tableYears={tableYears}
                            tableName="Exploration campaign"
                            includeFooter
                            totalRowName="Total"
                            gridRef={ExplorationCampaignGridRef}
                        />
                    </div>
                </div>
            </Grid>
        </Grid>
    )
}

export default ExplorationCampaign

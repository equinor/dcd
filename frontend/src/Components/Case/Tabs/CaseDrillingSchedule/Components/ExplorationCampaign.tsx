import { useMemo, useRef } from "react"
import useStyles from "@equinor/fusion-react-ag-grid-styles"
import Grid from "@mui/material/Grid2"

import SwitchableNumberInput from "@/Components/Input/SwitchableNumberInput"
import {
    DrillingCampaignProps,
    Well,
} from "@/Models/ICampaigns"
import CaseTabTable from "@/Components/Tables/CaseTables/CaseTabTable"
import { ITimeSeriesTableData } from "@/Models/ITimeSeries"
import { getYearFromDateString } from "@/Utils/DateUtils"
import { ProfileTypes } from "@/Models/enums"

interface ExplorationCampaignProps extends DrillingCampaignProps {
    campaign: Components.Schemas.CampaignDto
}

const ExplorationCampaign = ({ tableYears, campaign }: ExplorationCampaignProps) => {
    const styles = useStyles()
    const ExplorationCampaignGridRef = useRef<any>(null)

    const generateRowData = (): ITimeSeriesTableData[] => {
        const rows: ITimeSeriesTableData[] = []

        // Add rig mob/demob profile row
        if (campaign.rigMobDemobProfile) {
            const mobDemobRow: ITimeSeriesTableData = {
                profileName: "Rig mob/demob",
                unit: "Percentage in decimals",
                profile: {
                    id: campaign.campaignId,
                    startYear: campaign.rigMobDemobProfile.startYear,
                    values: campaign.rigMobDemobProfile.values,
                },
                resourceName: ProfileTypes.ExplorationWellCostProfile,
                resourceId: campaign.campaignId,
                resourcePropertyKey: "rigMobDemobProfile",
                overridable: true,
                overrideProfile: undefined,
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
                    id: campaign.campaignId,
                    startYear: campaign.rigUpgradingProfile.startYear,
                    values: campaign.rigUpgradingProfile.values,
                },
                resourceName: ProfileTypes.ExplorationWellCostProfile,
                resourceId: campaign.campaignId,
                resourcePropertyKey: "rigUpgradingProfile",
                overridable: true,
                overrideProfile: undefined,
                editable: true,
            }
            rows.push(upgradingRow)
        }

        // Add wells from campaignWells
        campaign.campaignWells?.forEach((well: Well) => {
            const wellRow: ITimeSeriesTableData = {
                profileName: well.wellName,
                unit: "Well",
                profile: {
                    id: campaign.campaignId,
                    startYear: well.startYear,
                    values: well.values,
                },
                resourceName: ProfileTypes.ExplorationWellCostProfile,
                resourceId: campaign.campaignId,
                resourcePropertyKey: "campaignWell",
                overridable: true,
                editable: true,
            }
            console.log("wellRow fra explorationCampaign: ", wellRow)
            rows.push(wellRow)
        })

        return rows
    }

    const rowData = useMemo(() => generateRowData(), [campaign, tableYears])
    console.log("rowData fra campaign: ", rowData)

    return (
        <Grid container spacing={2} style={{ width: "100%" }}>
            <Grid container size={12} spacing={2} style={{ maxWidth: "600px" }}>
                <Grid size={{ xs: 12, sm: 6, md: 5 }}>
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
                </Grid>
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
                            dg4Year={getYearFromDateString(campaign.rigMobDemobProfile.startYear.toString())}
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

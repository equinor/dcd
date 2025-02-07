import { useMemo } from "react"
import { AgGridReact } from "@ag-grid-community/react"
import useStyles from "@equinor/fusion-react-ag-grid-styles"
import { ColDef } from "@ag-grid-community/core"
import Grid from "@mui/material/Grid2"

import SwitchableNumberInput from "@/Components/Input/SwitchableNumberInput"
import { cellStyleRightAlign } from "@/Utils/common"
import {
    NameCell,
    MainText,
    SubText,
} from "./SharedCampaignStyles"
import {
    DrillingCampaignProps,
    TableRow,
    Well,
} from "@/Models/ICampaigns"
import useEditCase from "@/Hooks/useEditCase"

const NameCellRenderer = (params: any) => {
    const { data } = params
    return (
        <NameCell>
            <MainText>{data.name}</MainText>
            <SubText>{data.subheader}</SubText>
        </NameCell>
    )
}

interface ExplorationCampaignProps extends DrillingCampaignProps {
    campaign: Components.Schemas.CampaignDto
}

const ExplorationCampaign = ({ tableYears, campaign }: ExplorationCampaignProps) => {
    const styles = useStyles()
    const { addEdit } = useEditCase()
    console.log("campaign", campaign)
    console.log(campaign.campaignId)

    const generateRowData = (): TableRow[] => {
        const rows: TableRow[] = []

        // Add rig mob/demob profile row
        if (campaign.rigMobDemobProfile) {
            const mobDemobRow: TableRow = {
                name: "Rig mob/demob",
                subheader: "Percentage in decimals",
            }
            // Add year columns dynamically
            for (let year = tableYears[0]; year <= tableYears[1]; year += 1) {
                const yearIndex = year - campaign.rigMobDemobProfile.startYear
                mobDemobRow[year] = yearIndex >= 0 && yearIndex < campaign.rigMobDemobProfile.values.length
                    ? campaign.rigMobDemobProfile.values[yearIndex]
                    : 0
            }
            rows.push(mobDemobRow)
        }

        // Add rig upgrading profile row
        if (campaign.rigUpgradingProfile) {
            const upgradingRow: TableRow = {
                name: "Rig upgrading",
                subheader: "Percentage in decimals",
            }
            // Add year columns dynamically
            for (let year = tableYears[0]; year <= tableYears[1]; year += 1) {
                const yearIndex = year - campaign.rigUpgradingProfile.startYear
                upgradingRow[year] = yearIndex >= 0 && yearIndex < campaign.rigUpgradingProfile.values.length
                    ? campaign.rigUpgradingProfile.values[yearIndex]
                    : 0
            }
            rows.push(upgradingRow)
        }

        // Add wells from campaignWells
        campaign.campaignWells?.forEach((well: Well) => {
            const wellRow: TableRow = {
                name: well.wellName,
                subheader: "Well",
            }
            // Add year columns dynamically
            for (let year = tableYears[0]; year <= tableYears[1]; year += 1) {
                const yearIndex = year - well.startYear
                wellRow[year] = yearIndex >= 0 && yearIndex < well.values.length
                    ? well.values[yearIndex]
                    : 0
            }
            rows.push(wellRow)
        })

        return rows
    }

    const rowData = useMemo(() => generateRowData(), [campaign, tableYears])

    const columnDefs = useMemo<ColDef[]>(() => {
        const yearColumns = Array.from(
            { length: tableYears[1] - tableYears[0] + 1 },
            (_, index) => {
                const year = tableYears[0] + index
                return {
                    field: year.toString(),
                    width: 100,
                    cellStyle: {
                        ...cellStyleRightAlign,
                        display: "flex",
                        alignItems: "center",
                        justifyContent: "flex-end",
                        padding: "4px 8px",
                    },
                }
            },
        )

        return [
            {
                field: "name",
                headerName: "Exploration campaign",
                width: 250,
                editable: false,
                pinned: "left",
                cellRenderer: NameCellRenderer,
            },
            ...yearColumns,
            {
                field: "total",
                headerName: "Total",
                width: 100,
                editable: false,
                pinned: "right",
                cellStyle: {
                    fontWeight: "bold",
                    textAlign: "right",
                    display: "flex",
                    alignItems: "center",
                    justifyContent: "flex-end",
                    padding: "4px 8px",
                },
            },
        ]
    }, [tableYears])

    const defaultColDef = useMemo(() => ({
        sortable: true,
        filter: true,
        resizable: true,
        suppressHeaderMenuButton: true,
    }), [])

    return (
        <Grid container spacing={2} style={{ width: "100%" }}>
            <Grid container size={12} spacing={2} style={{ maxWidth: "600px" }}>
                <Grid size={{ xs: 12, sm: 6, md: 5 }}>
                    <SwitchableNumberInput
                        addEdit={addEdit}
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
                        addEdit={addEdit}
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
                        <AgGridReact
                            rowData={rowData}
                            columnDefs={columnDefs}
                            defaultColDef={defaultColDef}
                            animateRows
                            domLayout="autoHeight"
                            suppressMovableColumns
                            enableCharts
                            stopEditingWhenCellsLoseFocus
                            suppressLastEmptyLineOnPaste
                            rowHeight={44}
                        />
                    </div>
                </div>
            </Grid>
        </Grid>
    )
}

export default ExplorationCampaign

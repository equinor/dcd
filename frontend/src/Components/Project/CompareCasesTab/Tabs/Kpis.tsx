import React from "react"
import Grid from "@mui/material/Grid"

import { AgChartsCompareCases } from "@/Components/AgGrid/AgChartsCompareCases"

interface KpisProps {
    npvChartData?: object
    breakEvenChartData?: object
}

const Kpis: React.FC<KpisProps> = ({ npvChartData, breakEvenChartData }) => {
    if (!npvChartData || !breakEvenChartData) { return <div>No data available</div> }
    return (
        <Grid container spacing={2}>
            <Grid item xs={12} md={6}>
                <AgChartsCompareCases
                    data={npvChartData}
                    chartTitle="NPV before tax"
                    barColors={["#005F57", "#B4260D"]}
                    barProfiles={["npv", "npvOverride"]}
                    barNames={["Calculated NPV", "Manually set NPV"]}
                    unit="MUSD"
                    enableLegend={false}
                />
            </Grid>
            <Grid item xs={12} md={6}>
                <AgChartsCompareCases
                    data={breakEvenChartData}
                    chartTitle="Break even before tax"
                    barColors={["#00977B", "#FF6347"]}
                    barProfiles={["breakEven", "breakEvenOverride"]}
                    barNames={["Calculated break even", "Manually set break even"]}
                    unit="USD/bbl"
                    enableLegend={false}
                />
            </Grid>
        </Grid>
    )
}

export default Kpis

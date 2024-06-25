import React from "react"
import Grid from "@mui/material/Grid"
import { AgChartsCompareCases } from "../../../AgGrid/AgChartsCompareCases"

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
                    barColors={["#005F57"]}
                    barProfiles={["npv"]}
                    barNames={["NPV"]}
                    unit="mill USD"
                    height={400}
                    width="100%"
                    enableLegend={false}
                />
            </Grid>
            <Grid item xs={12} md={6}>
                <AgChartsCompareCases
                    data={breakEvenChartData}
                    chartTitle="Break even before tax"
                    barColors={["#00977B"]}
                    barProfiles={["breakEven"]}
                    barNames={["Break even"]}
                    unit="USD/bbl"
                    width="100%"
                    height={400}
                    enableLegend={false}
                />
            </Grid>
        </Grid>
    )
}

export default Kpis

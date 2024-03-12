import React from "react"
import Grid from "@mui/material/Grid"
import { AgChartsCompareCases } from "../../../AgGrid/AgChartsCompareCases"

interface ProductionProfilesProps {
    totalCo2EmissionsChartData?: object
    co2IntensityChartData?: object
}

const ProductionProfiles: React.FC<ProductionProfilesProps> = ({ totalCo2EmissionsChartData, co2IntensityChartData }) => {
    if (!totalCo2EmissionsChartData || !co2IntensityChartData) return <div>No data available</div>

    return (
        <Grid container spacing={2}>
            <Grid item xs={12} md={6}>
                <AgChartsCompareCases
                    data={totalCo2EmissionsChartData}
                    chartTitle="Total CO2 emissions"
                    barColors={["#E24973"]}
                    barProfiles={["totalCO2Emissions"]}
                    barNames={["Total CO2 emissions"]}
                    unit="mill tonnes"
                    width="100%"
                    height={400}
                    enableLegend={false}
                />
            </Grid>

            <Grid item xs={12} md={6}>
                <AgChartsCompareCases
                    data={co2IntensityChartData}
                    chartTitle="CO2 intensity"
                    barColors={["#FF92A8"]}
                    barProfiles={["cO2Intensity"]}
                    barNames={["CO2 intensity"]}
                    unit="kg CO2/boe"
                    width="100%"
                    height={400}
                    enableLegend={false}
                />
            </Grid>
        </Grid>
    )
}

export default ProductionProfiles

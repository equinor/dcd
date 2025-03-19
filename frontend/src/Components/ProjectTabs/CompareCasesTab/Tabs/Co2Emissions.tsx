import Grid from "@mui/material/Grid2"
import React from "react"

import { CompareCasesChart } from "@/Components/Charts/CompareCasesChart"

interface ProductionProfilesProps {
    totalCo2EmissionsChartData?: object
    co2IntensityChartData?: object
}

const ProductionProfiles: React.FC<ProductionProfilesProps> = ({ totalCo2EmissionsChartData, co2IntensityChartData }) => {
    if (!totalCo2EmissionsChartData || !co2IntensityChartData) { return <div>No data available</div> }

    return (
        <Grid container spacing={2}>
            <Grid size={{ xs: 12, md: 6 }}>
                <CompareCasesChart
                    data={totalCo2EmissionsChartData}
                    chartTitle="Total CO2 emissions"
                    barColors={["#E24973"]}
                    barProfiles={["totalCO2Emissions"]}
                    barNames={["Total CO2 emissions"]}
                    unit="mill tonnes"
                    enableLegend={false}
                />
            </Grid>

            <Grid size={{ xs: 12, md: 6 }}>
                <CompareCasesChart
                    data={co2IntensityChartData}
                    chartTitle="CO2 intensity"
                    barColors={["#FF92A8"]}
                    barProfiles={["co2Intensity"]}
                    barNames={["CO2 intensity"]}
                    unit="kg CO2/boe"
                    enableLegend={false}
                />
            </Grid>
        </Grid>
    )
}

export default ProductionProfiles

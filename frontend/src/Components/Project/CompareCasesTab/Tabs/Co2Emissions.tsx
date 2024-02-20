import React from "react"
import styled from "styled-components"
import { AgChartsCompareCases } from "../../../AgGrid/AgChartsCompareCases"

const ChartContainer = styled.div`
    display: flex;
    flex-direction: column;
    gap: 20px;
`

interface ProductionProfilesProps {
    totalCo2EmissionsChartData?: object
    co2IntensityChartData?: object
}

const ProductionProfiles: React.FC<ProductionProfilesProps> = ({ totalCo2EmissionsChartData, co2IntensityChartData }) => {
    if (!totalCo2EmissionsChartData || !co2IntensityChartData) return <div>No data available</div>

    return (
        <ChartContainer>
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
        </ChartContainer>
    )
}

export default ProductionProfiles

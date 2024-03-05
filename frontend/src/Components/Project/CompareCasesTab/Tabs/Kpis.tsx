import React from "react"
import styled from "styled-components"
import { AgChartsCompareCases } from "../../../AgGrid/AgChartsCompareCases"

const ChartContainer = styled.div`
    display: flex;
    flex-direction: column;
    gap: 20px;
`

interface KpisProps {
    npvChartData?: object
    breakEvenChartData?: object
}

const Kpis: React.FC<KpisProps> = ({ npvChartData, breakEvenChartData }) => {
    if (!npvChartData || !breakEvenChartData) return <div>No data available</div>

    return (
        <ChartContainer>
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
        </ChartContainer>
    )
}

export default Kpis

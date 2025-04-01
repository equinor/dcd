import React from "react"

import { CompareCasesChart } from "@/Components/Charts/CompareCasesChart"

interface ProductionProfilesProps {
    productionProfilesChartData?: object
}

const ProductionProfiles: React.FC<ProductionProfilesProps> = ({ productionProfilesChartData }) => {
    if (!productionProfilesChartData) { return <div>No data available</div> }

    return (
        <CompareCasesChart
            data={productionProfilesChartData}
            chartTitle="Production profiles"
            barColors={["#243746", "#281457", "#EB0037", "#FF5733", "#8C1159"]}
            barProfiles={["oilProduction", "additionalOilProduction", "gasProduction", "additionalGasProduction", "totalExportedVolumes"]}
            barNames={[
                "Oil production (MSm3)",
                "Additional oil production (MSm3)",
                "Rich gas production (GSm3)",
                "Additional rich gas production (GSm3)",
                "Production & sales volumes (MSm3)",
            ]}
        />
    )
}

export default ProductionProfiles

import React from "react"
import { AgChartsCompareCases } from "../../../AgGrid/AgChartsCompareCases"

interface ProductionProfilesProps {
    productionProfilesChartData?: object
}

const ProductionProfiles: React.FC<ProductionProfilesProps> = ({ productionProfilesChartData }) => {
    if (!productionProfilesChartData) { return <div>No data available</div> }

    return (
        <AgChartsCompareCases
            data={productionProfilesChartData}
            chartTitle="Production profiles"
            barColors={["#243746", "#EB0037", "#8C1159"]}
            barProfiles={["oilProduction", "gasProduction", "totalExportedVolumes"]}
            barNames={[
                "Oil production (MSm3)",
                "Gas production (GSm3)",
                "Total exported volumes (MSm3)",
            ]}
            height={432}
        />
    )
}

export default ProductionProfiles

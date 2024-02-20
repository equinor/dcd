import React from "react"
import { AgChartsCompareCases } from "../../../AgGrid/AgChartsCompareCases"
import { useAppContext } from "../../../../Context/AppContext"

interface InvestmentProfilesProps {
    investmentProfilesChartData?: object
}

const InvestmentProfiles: React.FC<InvestmentProfilesProps> = ({ investmentProfilesChartData }) => {
    const { project } = useAppContext()

    if (!investmentProfilesChartData) return <div>No data available</div>

    return (
        <AgChartsCompareCases
            data={investmentProfilesChartData}
            chartTitle="Investment profiles"
            barColors={["#005F57", "#00977B", "#40D38F"]}
            barProfiles={["offshorePlusOnshoreFacilityCosts",
                "developmentCosts", "explorationWellCosts"]}
            barNames={[
                "Offshore + Onshore facility costs",
                "Development well costs",
                "Exploration well costs",
            ]}
            unit={`${project?.currency === 1 ? "mill NOK" : "mill USD"}`}
            height={432}
        />
    )
}

export default InvestmentProfiles

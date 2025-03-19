import React from "react"

import { CompareCasesChart } from "../../../Charts/CompareCasesChart"

import { useDataFetch } from "@/Hooks"
import { Currency } from "@/Models/enums"

interface InvestmentProfilesProps {
    investmentProfilesChartData?: object
}

const InvestmentProfiles: React.FC<InvestmentProfilesProps> = ({ investmentProfilesChartData }) => {
    const revisionAndProjectData = useDataFetch()

    if (!investmentProfilesChartData) { return <div>No data available</div> }

    return (
        <CompareCasesChart
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
            unit={`${revisionAndProjectData?.commonProjectAndRevisionData.currency === Currency.Nok ? "mill NOK" : "mill USD"}`}
        />
    )
}

export default InvestmentProfiles

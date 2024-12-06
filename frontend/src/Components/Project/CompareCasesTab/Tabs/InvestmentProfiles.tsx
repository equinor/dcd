import React from "react"
import { useModuleCurrentContext } from "@equinor/fusion-framework-react-module-context"
import { useQuery } from "@tanstack/react-query"
import { AgChartsCompareCases } from "../../../AgGrid/AgChartsCompareCases"
import { projectQueryFn } from "@/Services/QueryFunctions"

interface InvestmentProfilesProps {
    investmentProfilesChartData?: object
}

const InvestmentProfiles: React.FC<InvestmentProfilesProps> = ({ investmentProfilesChartData }) => {
    const { currentContext } = useModuleCurrentContext()
    const externalId = currentContext?.externalId

    const { data: apiData } = useQuery({
        queryKey: ["projectApiData", externalId],
        queryFn: () => projectQueryFn(externalId),
        enabled: !!externalId,
    })

    if (!investmentProfilesChartData) { return <div>No data available</div> }

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
            unit={`${apiData?.commonProjectAndRevisionData.currency === 1 ? "mill NOK" : "mill USD"}`}
        />
    )
}

export default InvestmentProfiles

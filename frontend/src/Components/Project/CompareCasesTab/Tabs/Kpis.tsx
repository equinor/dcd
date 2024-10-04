import React from "react"
import Grid from "@mui/material/Grid"
import { useQuery } from "@tanstack/react-query"
import { useModuleCurrentContext } from "@equinor/fusion-framework-react-module-context"
import { AgChartsCompareCases } from "@/Components/AgGrid/AgChartsCompareCases"
import { projectQueryFn } from "@/Services/QueryFunctions"

interface KpisProps {
    npvChartData?: object
    breakEvenChartData?: object
}

const Kpis: React.FC<KpisProps> = ({ npvChartData, breakEvenChartData }) => {
    if (!npvChartData || !breakEvenChartData) { return <div>No data available</div> }
    const { currentContext } = useModuleCurrentContext()
    const externalId = currentContext?.externalId
    const { data: apiData } = useQuery({
        queryKey: ["projectApiData", externalId],
        queryFn: () => projectQueryFn(externalId),
        enabled: !!externalId,
    })
    return (
        <Grid container spacing={2}>
            <Grid item xs={12} md={6}>
                <AgChartsCompareCases
                    data={npvChartData}
                    chartTitle="NPV before tax"
                    barColors={["#005F57", "#b4260d"]}
                    barProfiles={["npv", "npvOverride"]}
                    barNames={["Calculated NPV", "Manually set NPV"]}
                    unit={apiData?.currency === 1 ? "MNOK" : "MUSD"}
                    enableLegend={false}
                />
            </Grid>
            <Grid item xs={12} md={6}>
                <AgChartsCompareCases
                    data={breakEvenChartData}
                    chartTitle="Break even before tax"
                    barColors={["#00977B", "#FF6347"]}
                    barProfiles={["breakEven", "breakEvenOverride"]}
                    barNames={["Calculated Break Even", "Manually set Break Even"]}
                    unit={apiData?.currency === 1 ? "NOK/bbl" : "USD/bbl"}
                    enableLegend={false}
                />
            </Grid>
        </Grid>
    )
}

export default Kpis

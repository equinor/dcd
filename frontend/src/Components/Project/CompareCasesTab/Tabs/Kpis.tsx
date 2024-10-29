import React from "react"
import Grid from "@mui/material/Grid"
import { useQuery } from "@tanstack/react-query"
import { useModuleCurrentContext } from "@equinor/fusion-framework-react-module-context"
import { AgChartsCompareCases } from "@/Components/AgGrid/AgChartsCompareCases"
import { projectQueryFn } from "@/Services/QueryFunctions"
import { useProjectContext } from "@/Context/ProjectContext"

interface KpisProps {
    npvChartData?: object
    breakEvenChartData?: object
}

const Kpis: React.FC<KpisProps> = ({ npvChartData, breakEvenChartData }) => {
    if (!npvChartData || !breakEvenChartData) { return <div>No data available</div> }
    const { currentContext } = useModuleCurrentContext()
    const { projectId } = useProjectContext()
    const { data: apiData } = useQuery({
        queryKey: ["projectApiData", projectId],
        queryFn: () => projectQueryFn(projectId),
        enabled: !!currentContext?.externalId,
    })
    return (
        <Grid container spacing={2}>
            <Grid item xs={12} md={6}>
                <AgChartsCompareCases
                    data={npvChartData}
                    chartTitle="NPV before tax"
                    barColors={["#005F57", "#B4260D"]}
                    barProfiles={["npv", "npvOverride"]}
                    barNames={["Calculated NPV", "Manually set NPV"]}
                    unit="MUSD"
                    enableLegend={false}
                />
            </Grid>
            <Grid item xs={12} md={6}>
                <AgChartsCompareCases
                    data={breakEvenChartData}
                    chartTitle="Break even before tax"
                    barColors={["#00977B", "#FF6347"]}
                    barProfiles={["breakEven", "breakEvenOverride"]}
                    barNames={["Calculated break even", "Manually set break even"]}
                    unit="USD/bbl"
                    enableLegend={false}
                />
            </Grid>
        </Grid>
    )
}

export default Kpis

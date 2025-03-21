import Grid from "@mui/material/Grid2"
import { useState } from "react"

import { SecondaryTabs } from "../Components/SecondaryTabs"

import Co2Emissions from "./Tabs/Co2Emissions"
import InvestmentProfiles from "./Tabs/InvestmentProfiles"
import Kpis from "./Tabs/Kpis"
import ProductionProfiles from "./Tabs/ProductionProfiles"

import CompareCasesTable from "@/Components/Tables/ProjectTables/CompareCasesTable"
import { useProjectChartData } from "@/Hooks/useProjectChartData"

const ProjectCompareCasesTab = () => {
    const {
        rowData,
        npvChartData,
        breakEvenChartData,
        productionProfilesChartData,
        investmentProfilesChartData,
        totalCo2EmissionsChartData,
        co2IntensityChartData,
    } = useProjectChartData()
    const [value, setValue] = useState(0)

    const tabs = [
        {
            label: "KPIs",
            content: <Kpis npvChartData={npvChartData} breakEvenChartData={breakEvenChartData} />,
        },
        {
            label: "Production profiles",
            content: <ProductionProfiles productionProfilesChartData={productionProfilesChartData} />,
        },
        {
            label: "Investment profiles",
            content: <InvestmentProfiles investmentProfilesChartData={investmentProfilesChartData} />,
        },
        {
            label: "CO2 emissions",
            content: (
                <Co2Emissions
                    totalCo2EmissionsChartData={totalCo2EmissionsChartData}
                    co2IntensityChartData={co2IntensityChartData}
                />
            ),
        },
    ]

    return (
        <Grid container spacing={6}>
            <Grid size={12}>
                <SecondaryTabs
                    tabs={tabs}
                    value={value}
                    onChange={(_, newValue) => setValue(newValue)}
                />
            </Grid>
            <Grid size={12}>
                <CompareCasesTable rowData={rowData} />
            </Grid>
        </Grid>
    )
}

export default ProjectCompareCasesTab

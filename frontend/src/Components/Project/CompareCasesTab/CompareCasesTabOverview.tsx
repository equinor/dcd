import { useState } from "react"
import { Tabs } from "@equinor/eds-core-react"
import Grid from "@mui/material/Grid"
import Kpis from "./Tabs/Kpis"
import ProductionProfiles from "./Tabs/ProductionProfiles"
import InvestmentProfiles from "./Tabs/InvestmentProfiles"
import Co2Emissions from "./Tabs/Co2Emissions"
import { useProjectChartData } from "../../../Hooks/useProjectChartData"
import ProjectAgGridTable from "./ProjectAgGridTable"

const {
    List, Tab, Panels, Panel,
} = Tabs

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

    const [activeTab, setActiveTab] = useState(0)

    return (
        <Grid container spacing={2}>
            <Grid item xs={12}>
                <Tabs activeTab={activeTab} onChange={setActiveTab}>
                    <List>
                        <Tab>KPIs</Tab>
                        <Tab>Production profiles</Tab>
                        <Tab>Investment profiles</Tab>
                        <Tab>CO2 emissions</Tab>
                    </List>
                    <Panels>
                        <Panel>
                            <Kpis
                                npvChartData={npvChartData}
                                breakEvenChartData={breakEvenChartData}
                            />
                        </Panel>
                        <Panel>
                            <ProductionProfiles
                                productionProfilesChartData={productionProfilesChartData}
                            />
                        </Panel>
                        <Panel>
                            <InvestmentProfiles
                                investmentProfilesChartData={investmentProfilesChartData}
                            />
                        </Panel>
                        <Panel>
                            <Co2Emissions
                                totalCo2EmissionsChartData={totalCo2EmissionsChartData}
                                co2IntensityChartData={co2IntensityChartData}
                            />
                        </Panel>
                    </Panels>
                </Tabs>
            </Grid>
            <Grid item xs={12}>
                <ProjectAgGridTable rowData={rowData} />
            </Grid>
        </Grid>
    )
}

export default ProjectCompareCasesTab

import styled from "styled-components"
import { useState } from "react"
import { Tabs } from "@equinor/eds-core-react"
import { tokens } from "@equinor/eds-tokens"
import Kpis from "./Tabs/Kpis"
import ProductionProfiles from "./Tabs/ProductionProfiles"
import InvestmentProfiles from "./Tabs/InvestmentProfiles"
import Co2Emissions from "./Tabs/Co2Emissions"
import { useProjectChartData } from "../../../Hooks/useProjectChartData"
import ProjectAgGridTable from "./ProjectAgGridTable"

const {
    List, Tab, Panels, Panel,
} = Tabs

const StyledTabPanel = styled(Panel)`
    padding-top: 0px;
`

const StyledList = styled(List)`
    border-bottom: 1px solid ${tokens.colors.ui.background__medium.rgba};
`

const WrapperTabs = styled.div`
width: 100%;
display: flex;
float: left;
flex-direction: column;
`

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
        <>
            <WrapperTabs>
                <Tabs activeTab={activeTab} onChange={setActiveTab}>
                    <StyledList>
                        <Tab>KPIs</Tab>
                        <Tab>Production profiles</Tab>
                        <Tab>Investment profiles</Tab>
                        <Tab>CO2 emissions</Tab>
                    </StyledList>
                    <Panels>
                        <StyledTabPanel>
                            <Kpis
                                npvChartData={npvChartData}
                                breakEvenChartData={breakEvenChartData}
                            />
                        </StyledTabPanel>
                        <StyledTabPanel>
                            <ProductionProfiles
                                productionProfilesChartData={productionProfilesChartData}
                            />
                        </StyledTabPanel>
                        <StyledTabPanel>
                            <InvestmentProfiles
                                investmentProfilesChartData={investmentProfilesChartData}
                            />
                        </StyledTabPanel>
                        <StyledTabPanel>
                            <Co2Emissions
                                totalCo2EmissionsChartData={totalCo2EmissionsChartData}
                                co2IntensityChartData={co2IntensityChartData}
                            />
                        </StyledTabPanel>
                    </Panels>
                </Tabs>
            </WrapperTabs>
            <ProjectAgGridTable rowData={rowData} />
        </>
    )
}

export default ProjectCompareCasesTab

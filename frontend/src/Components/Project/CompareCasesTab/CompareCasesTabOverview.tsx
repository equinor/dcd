import { useState } from "react"
import Tabs from "@mui/material/Tabs"
import Tab from "@mui/material/Tab"
import Grid from "@mui/material/Grid"
import { useMediaQuery } from "@mui/material"
import styled from "styled-components"

import Kpis from "./Tabs/Kpis"
import { useProjectChartData } from "@/Hooks/useProjectChartData"
import ProductionProfiles from "./Tabs/ProductionProfiles"
import InvestmentProfiles from "./Tabs/InvestmentProfiles"
import Co2Emissions from "./Tabs/Co2Emissions"
import ProjectAgGridTable from "./ProjectAgGridTable"

const TabContainer = styled.div<{ $isSmallScreen?: boolean }>`
    display: flex;
    flex-direction: ${({ $isSmallScreen }) => ($isSmallScreen ? "column" : "row")};
    border-radius: 8px;
`

const StyledTabs = styled(Tabs)<{ $isSmallScreen?: boolean }>`
    &.MuiTabs-root {
        min-height: unset;
        min-width: ${({ $isSmallScreen }) => ($isSmallScreen ? "unset" : "200px")};
    }

    .MuiTabs-indicator {
        display: none;
    }
`

const StyledTab = styled(Tab)`
    &.MuiTab-root {
        margin: 0;
        padding: 12px 24px;
        background: rgba(220, 220, 220, 0.5);
        border: 1px solid #DCDCDC;
        min-height: unset;
        text-transform: none;
        font-family: Equinor;
        font-size: 16px;
        color: rgba(61, 61, 61, 1);
        align-items: flex-start;

        .MuiTabs-vertical & {
            border-radius: 4px 0 0 4px;
            border-right: none;
            border-bottom: 1px solid #DCDCDC;
        }

        .MuiTabs-horizontal & {
            border-radius: 4px 4px 0 0;
            border-right: 1px solid #DCDCDC;
            border-bottom: none;
        }
        
        &.Mui-selected {
            background: white;
            position: relative;
            z-index: 1;
            color: rgba(0, 79, 85, 1);

            &:after {
                content: "";
                position: absolute;
                background: white;
            }

            .MuiTabs-vertical &:after {
                right: -1px;
                top: 0;
                bottom: 0;
                width: 1px;
            }

            .MuiTabs-horizontal &:after {
                bottom: -1px;
                left: 0;
                right: 0;
                height: 1px;
            }
        }

        &:hover:not(.Mui-selected) {
            background: rgba(220, 220, 220, 0.8);
        }
    }
`

const TabPanel = styled.div<{ $isSmallScreen?: boolean }>`
    background: white;
    border-radius: ${({ $isSmallScreen }) => ($isSmallScreen ? "0 0 4px 4px" : "0 4px 4px 0")};
    border: 1px solid #DCDCDC;
    padding: 24px;
    flex-grow: 1;
    position: relative;
`

interface TabPanelProps {
    children?: React.ReactNode;
    index: number;
    value: number;
    isSmallScreen?: boolean;
}

const CustomTabPanel = (props: TabPanelProps) => {
    const { children, value, index, isSmallScreen, ...other } = props;

    return (
        <TabPanel
            role="tabpanel"
            hidden={value !== index}
            id={`vertical-tabpanel-${index}`}
            aria-labelledby={`vertical-tab-${index}`}
            $isSmallScreen={isSmallScreen}
            {...other}
        >
            {value === index && children}
        </TabPanel>
    );
};

function a11yProps(index: number) {
    return {
        id: `vertical-tab-${index}`,
        "aria-controls": `vertical-tabpanel-${index}`,
    }
}

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
    const isSmallScreen = useMediaQuery("(max-width: 968px)")
    const [value, setValue] = useState(0)

    const handleChange = (_: React.SyntheticEvent, newValue: number) => {
        setValue(newValue)
    }

    return (
        <Grid container spacing={6}>
            <Grid item xs={12}>
                <TabContainer $isSmallScreen={isSmallScreen}>
                    <StyledTabs
                        $isSmallScreen={isSmallScreen}
                        orientation={isSmallScreen ? "horizontal" : "vertical"}
                        value={value}
                        onChange={handleChange}
                    >
                        <StyledTab label="KPIs" {...a11yProps(0)} />
                        <StyledTab label="Production profiles" {...a11yProps(1)} />
                        <StyledTab label="Investment profiles" {...a11yProps(2)} />
                        <StyledTab label="CO2 emissions" {...a11yProps(3)} />
                    </StyledTabs>
                    <CustomTabPanel value={value} index={0} isSmallScreen={isSmallScreen}>
                        <Kpis
                            npvChartData={npvChartData}
                            breakEvenChartData={breakEvenChartData}
                        />
                    </CustomTabPanel>
                    <CustomTabPanel value={value} index={1} isSmallScreen={isSmallScreen}>
                        <ProductionProfiles
                            productionProfilesChartData={productionProfilesChartData}
                        />
                    </CustomTabPanel>
                    <CustomTabPanel value={value} index={2} isSmallScreen={isSmallScreen}>
                        <InvestmentProfiles
                            investmentProfilesChartData={investmentProfilesChartData}
                        />
                    </CustomTabPanel>
                    <CustomTabPanel value={value} index={3} isSmallScreen={isSmallScreen}>
                        <Co2Emissions
                            totalCo2EmissionsChartData={totalCo2EmissionsChartData}
                            co2IntensityChartData={co2IntensityChartData}
                        />
                    </CustomTabPanel>
                </TabContainer>
            </Grid>
            <Grid item xs={12}>
                <ProjectAgGridTable rowData={rowData} />
            </Grid>
        </Grid>
    )
}

export default ProjectCompareCasesTab

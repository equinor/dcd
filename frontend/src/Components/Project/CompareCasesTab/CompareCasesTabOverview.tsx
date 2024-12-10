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

const MuiTabs = styled(Tabs)<{ $isSmallScreen?: boolean }>`
    margin-bottom: ${({ $isSmallScreen }) => ($isSmallScreen ? "30px" : "10px")};
    &.MuiTabs-vertical {
        border: none;
        flex-wrap: wrap;
        &:after {
            content: "";
            position: relative;
            left: -2px;
            height: 100%;
            width: 2px;
            z-index: -2;
            background-color: var(--eds_ui_background__medium,rgba(220,220,220,1));
        }
        .MuiTabs-scroller {
            height: 100%;
            .MuiTabs-flexContainerVertical {
                height: 100%;
                justify-content: center;
            }
        }
    }
`

const MuiTab = styled(Tab)`
    &.MuiTab-root {
        font-family: Equinor;
        text-transform: none;
        font-size: 16px;
        letter-spacing: 0;
        border-right: 1px solid rgba(222, 237, 238, 1);
        color: var(--eds_navigation__menu_tabs_color,rgba(61,61,61,1));
        &:hover,
        &.Mui-selected {
            background: var(--eds_interactive_primary__hover_alt, rgba(222, 237, 238, 1));
        }
        &.Mui-selected {
            color: var(--eds_interactive_primary__hover, rgba(0, 79, 85, 1));
        }
    }
`

interface TabPanelProps {
    children?: React.ReactNode;
    index: number;
    value: number;
}

export const TabPanel = (props: TabPanelProps) => {
    const {
        children, value, index, ...other
    } = props

    return (
        <div
            role="tabpanel"
            hidden={value !== index}
            id={`vertical-tabpanel-${index}`}
            aria-labelledby={`vertical-tab-${index}`}
            {...other}
        >
            {value === index && <div>{children}</div>}
        </div>
    )
}

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
    const isSmallScreen = useMediaQuery("(max-width: 1080px)")

    const [value, setValue] = useState(0)

    const handleChange = (event: React.SyntheticEvent, newValue: number) => {
        setValue(newValue)
    }

    return (
        <Grid container spacing={6}>
            <Grid item xs={12} container display="flex" flexDirection="row">
                <Grid item container sx={{ width: "fit-content", marginRight: "10px" }}>
                    <MuiTabs
                        $isSmallScreen={isSmallScreen}
                        orientation={isSmallScreen ? "horizontal" : "vertical"}
                        value={value}
                        onChange={handleChange}
                        TabIndicatorProps={{
                            style: {
                                backgroundColor: "rgba(0,112,121,1)",
                            },
                        }}
                    >
                        <MuiTab label="KPIs" {...a11yProps(0)} />
                        <MuiTab label="Production profiles" {...a11yProps(1)} />
                        <MuiTab label="Investment profiles" {...a11yProps(2)} />
                        <MuiTab label="CO2 emissions" {...a11yProps(3)} />
                    </MuiTabs>
                </Grid>
                <Grid item sx={{ flexGrow: 1 }}>
                    <TabPanel index={0} value={value}>
                        <Kpis
                            npvChartData={npvChartData}
                            breakEvenChartData={breakEvenChartData}
                        />
                    </TabPanel>
                    <TabPanel index={1} value={value}>
                        <ProductionProfiles
                            productionProfilesChartData={productionProfilesChartData}
                        />
                    </TabPanel>
                    <TabPanel index={2} value={value}>
                        <InvestmentProfiles
                            investmentProfilesChartData={investmentProfilesChartData}
                        />
                    </TabPanel>
                    <TabPanel index={3} value={value}>
                        <Co2Emissions
                            totalCo2EmissionsChartData={totalCo2EmissionsChartData}
                            co2IntensityChartData={co2IntensityChartData}
                        />
                    </TabPanel>
                </Grid>
            </Grid>
            <Grid item xs={12}>
                <ProjectAgGridTable rowData={rowData} />
            </Grid>
        </Grid>
    )
}

export default ProjectCompareCasesTab

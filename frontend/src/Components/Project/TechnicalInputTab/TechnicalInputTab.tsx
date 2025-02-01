import { useState } from "react"
import { Tabs, Tab, Box } from "@mui/material"
import Grid from "@mui/material/Grid2"
import styled from "styled-components"

import { useDataFetch } from "@/Hooks/useDataFetch"
import WellCostsTab from "./WellCostTab/WellCostsTab"
import PROSPTab from "./PROSPTab/PROSPTab"
import CO2Tab from "./CO2Tab/CO2Tab"
import ProjectSkeleton from "@/Components/LoadingSkeletons/ProjectSkeleton"

const TabWrapper = styled.div`
    margin-top: 24px;
    border-radius: 8px;
`

const StyledTabs = styled(Tabs)`
    &.MuiTabs-root {
        min-height: unset;
    }

    .MuiTabs-indicator {
        display: none;
    }
`

const StyledTab = styled(Tab)`
    &.MuiTab-root {
        border-radius: 4px 4px 0 0;
        margin: 0;
        padding: 12px 24px;
        background: rgba(220, 220, 220, 0.5);
        border: 1px solid #DCDCDC;
        border-bottom: none;
        min-height: unset;
        text-transform: none;
        font-family: Equinor;
        
        &.Mui-selected {
            background: white;
            position: relative;
            z-index: 1;

            &:after {
                content: "";
                position: absolute;
                bottom: -1px;
                left: 0;
                right: 0;
                height: 1px;
                background: white;
            }
        }

        &:hover:not(.Mui-selected) {
            background: rgba(220, 220, 220, 0.8);
        }
    }
`

const TabPanel = styled.div`
    background: white;
    border-radius: 0 0 4px 4px;
    border: 1px solid #DCDCDC;
    padding: 24px;
    margin-top: -1px;
    position: relative;
`

interface TabPanelProps {
    children?: React.ReactNode;
    index: number;
    value: number;
}

const CustomTabPanel = (props: TabPanelProps) => {
    const {
        children,
        value,
        index,
        ...other
    } = props

    return (
        <TabPanel
            role="tabpanel"
            hidden={value !== index}
            id={`technical-tabpanel-${index}`}
            aria-labelledby={`technical-tab-${index}`}
            {...other}
        >
            {value === index && children}
        </TabPanel>
    )
}

const TechnicalInputTab = () => {
    const revisionAndProjectData = useDataFetch()
    const [activeTab, setActiveTab] = useState(0)

    if (!revisionAndProjectData) {
        return (<ProjectSkeleton />)
    }

    const handleChange = (_: React.SyntheticEvent, newValue: number) => {
        setActiveTab(newValue)
    }

    return (
        <TabWrapper>
            <Grid container>
                <Grid size={12}>
                    <Box>
                        <StyledTabs value={activeTab} onChange={handleChange}>
                            <StyledTab label="Well Costs" />
                            <StyledTab label="PROSP" />
                            <StyledTab label="CO2" />
                        </StyledTabs>
                        <CustomTabPanel value={activeTab} index={0}>
                            <WellCostsTab />
                        </CustomTabPanel>
                        <CustomTabPanel value={activeTab} index={1}>
                            <PROSPTab />
                        </CustomTabPanel>
                        <CustomTabPanel value={activeTab} index={2}>
                            <CO2Tab />
                        </CustomTabPanel>
                    </Box>
                </Grid>
            </Grid>
        </TabWrapper>
    )
}

export default TechnicalInputTab

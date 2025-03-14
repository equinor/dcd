import { Tab, Tabs, Box } from "@mui/material"
import styled from "styled-components"
import { ReactNode } from "react"

export const TabWrapper = styled.div`
    border-radius: 8px;
`

export const StyledTabs = styled(Tabs)`
    &.MuiTabs-root {
        min-height: unset;
    }

    .MuiTabs-indicator {
        display: none;
    }
`

export const StyledTab = styled(Tab)`
    &.MuiTab-root {
        border-radius: 4px 4px 0 0;
        margin: 0;
        padding: 12px 24px;
        background: rgba(220, 220, 220, 0.4);
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

export const TabPanel = styled.div`
    background: white;
    border-radius: 0 0 4px 4px;
    border: 1px solid #DCDCDC; 
    padding: 24px;
    margin-top: -1px;
    position: relative;
`

export interface TabPanelProps {
    children?: ReactNode;
    index: number;
    value: number;
}

export const CustomTabPanel = (props: TabPanelProps) => {
    const {
        children, value, index, ...other
    } = props

    return (
        <TabPanel
            role="tabpanel"
            hidden={value !== index}
            id={`secondary-tabpanel-${index}`}
            aria-labelledby={`secondary-tab-${index}`}
            {...other}
            {...other}
        >
            {value === index && children}
        </TabPanel>
    )
}

export interface CustomTab {
    label: string;
    content: ReactNode;
}

export interface SecondaryTabsProps {
    tabs: CustomTab[];
    value: number;
    onChange: (event: React.SyntheticEvent, newValue: number) => void;
}

export const SecondaryTabs = ({ tabs, value, onChange }: SecondaryTabsProps) => (
    <TabWrapper>
        <Box>
            <StyledTabs value={value} onChange={onChange}>
                {tabs.map((tab, index) => (
                    // eslint-disable-next-line react/no-array-index-key
                    <StyledTab key={index} label={tab.label} />
                ))}
            </StyledTabs>

            {tabs.map((tab, index) => (
                // eslint-disable-next-line react/no-array-index-key
                <CustomTabPanel key={index} value={value} index={index}>
                    {tab.content}
                </CustomTabPanel>
            ))}
        </Box>
    </TabWrapper>
)

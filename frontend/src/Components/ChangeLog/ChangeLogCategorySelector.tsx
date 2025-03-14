import { useState } from "react"
import { Box } from "@mui/material"
import { ChangeLogCategory } from "@/Models/enums"
import ChangeLogView from "./ChangeLog"
import {
    CustomTabPanel,
    StyledTab,
    StyledTabs,
    TabWrapper,
} from "../Project/Components/SecondaryTabs"

const ChangeLogCategorySelector = () => {
    const [activeTab, setActiveTab] = useState(ChangeLogCategory.None)

    const tabs = [
        { label: "All changes", content: <ChangeLogView changeLogCategory={ChangeLogCategory.None} /> },
        { label: "Project overview", content: <ChangeLogView changeLogCategory={ChangeLogCategory.ProjectOverviewTab} /> },
        { label: "Project settings", content: <ChangeLogView changeLogCategory={ChangeLogCategory.SettingsTab} /> },
        { label: "CO2", content: <ChangeLogView changeLogCategory={ChangeLogCategory.Co2Tab} /> },
        { label: "Wells", content: <ChangeLogView changeLogCategory={ChangeLogCategory.WellCostTab} /> },
        { label: "Access management", content: <ChangeLogView changeLogCategory={ChangeLogCategory.AccessManagementTab} /> },
    ]

    return (
        <TabWrapper>
            <Box>
                <StyledTabs value={activeTab} onChange={(_, newValue) => setActiveTab(newValue)}>
                    {tabs.map((tab, index) => (
                        // eslint-disable-next-line react/no-array-index-key
                        <StyledTab key={index} label={tab.label} />
                    ))}
                </StyledTabs>

                {tabs.map((tab, index) => (
                    // eslint-disable-next-line react/no-array-index-key
                    <CustomTabPanel key={index} value={activeTab} index={index}>
                        {tab.content}
                    </CustomTabPanel>
                ))}
            </Box>
        </TabWrapper>
    )
}

export default ChangeLogCategorySelector

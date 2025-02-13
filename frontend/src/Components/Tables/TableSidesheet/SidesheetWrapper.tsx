import { Drawer, Tab, Tabs } from "@mui/material"
import styled from "styled-components"
import { useState, useMemo } from "react"
import { tokens } from "@equinor/eds-tokens"
import SidesheetHeader from "./SidesheetHeader"
import CalculationsTab from "./Tabs/CalculationsTab/CalculationsTab"
import EditHistoryTab from "./Tabs/EditHistoryTab"
import MetadataTab from "./Tabs/MetadataTab"
import TimeSeriesTab from "./Tabs/TimeSeriesTab"
import { useAppStore } from "@/Store/AppStore"
import { formatFullDate } from "@/Utils/DateUtils"

const DrawerContent = styled.div`
    width: 580px;
    height: 100%;
    display: flex;
    flex-direction: column;
    background: ${tokens.colors.ui.background__default.rgba};
`

const TabsContainer = styled.div`
    border-bottom: 1px solid ${tokens.colors.ui.background__medium.rgba};
    padding: 0 16px;
    
    .MuiTabs-root {
        min-height: 48px;
    }

    .MuiTab-root {
        min-height: 48px;
        text-transform: none;
        font-weight: 500;
        font-size: 14px;
        padding: 12px 16px;
        min-width: unset;
        color: ${tokens.colors.text.static_icons__tertiary.rgba};

        &.Mui-selected {
            color: #577865;
        }
    }

    .MuiTabs-indicator {
        background-color: #577865;
        height: 2px;
    }
`

interface Props {
    isOpen: boolean
    onClose: () => void
    rowData: any
    dg4Year: number
    allTimeSeriesData?: any[]
    isProsp?: boolean
}

interface TabItem {
    label: string;
    content: JSX.Element;
}

const SidesheetWrapper = ({
    isOpen,
    onClose,
    rowData,
    dg4Year,
    isProsp,
}: Props) => {
    const { developerMode } = useAppStore()
    const [activeTab, setActiveTab] = useState(0)

    const handleTabChange = (_: React.SyntheticEvent, newValue: number) => {
        setActiveTab(newValue)
    }

    const headerData = useMemo(() => {
        if (!rowData) {
            return {
                title: "Unknown row name",
                value: 0,
                year: "N/A",
                lastUpdated: "Not available",
                source: "N/A",
            }
        }

        const isOverridable = rowData.overridable
        const isOverridden = rowData.overrideProfile.override === true

        let source = "Manual input"

        if (isOverridable && !isOverridden) {
            source = isProsp ? "PROSP file" : "Calculated input"
        }

        const clickedYearValue = rowData?.[rowData?.clickedYear] ?? (rowData?.profile?.values?.[0] ?? rowData?.overrideProfile?.values?.[0] ?? 0)
        const lastUpdated = rowData?.profile?.updatedUtc ?? "Not available"
        const formattedDate = lastUpdated !== "Not available" ? formatFullDate(lastUpdated) : "Not available"

        return {
            title: rowData?.profileName || "Unknown row name",
            value: rowData?.unit ? `${clickedYearValue} ${rowData.unit}` : clickedYearValue,
            year: rowData?.clickedYear || (rowData?.profile?.startYear ? dg4Year + rowData.profile.startYear : "N/A"),
            lastUpdated: formattedDate,
            source,
        }
    }, [rowData, dg4Year, isProsp])

    if (!rowData) {
        return null
    }

    const hasTimeSeriesData = rowData?.profile?.values?.length > 0 || rowData?.overrideProfile?.values?.length > 0

    const tabs = [
        hasTimeSeriesData && {
            label: "Time Series",
            content: <TimeSeriesTab rowData={rowData} dg4Year={dg4Year} />,
        },
        {
            label: "Calculations",
            content: <CalculationsTab profileName={headerData.title} rowData={rowData} />,
        },
        {
            label: "Edit history",
            content: <EditHistoryTab />,
        },
        developerMode && {
            label: "Metadata",
            content: <MetadataTab rowData={rowData} />,
        },
    ].filter((tab): tab is TabItem => Boolean(tab))

    return (
        <Drawer
            anchor="right"
            open={isOpen}
            onClose={onClose}
        >
            <DrawerContent>
                <SidesheetHeader {...headerData} onClose={onClose} />
                <TabsContainer>
                    <Tabs
                        value={activeTab}
                        onChange={handleTabChange}
                        variant="scrollable"
                        scrollButtons="auto"
                    >
                        {tabs.map((tab) => (
                            <Tab key={tab.label} label={tab.label} />
                        ))}
                    </Tabs>
                </TabsContainer>

                {tabs[activeTab]?.content}
            </DrawerContent>
        </Drawer>
    )
}

export default SidesheetWrapper

import { Drawer, Tab, Tabs } from "@mui/material"
import styled from "styled-components"
import { useState, useMemo } from "react"
import { tokens } from "@equinor/eds-tokens"
import SidesheetHeader from "./SidesheetHeader"
import CalculationsTab from "./Tabs/CalculationsTab/CalculationsTab"
import EditHistoryTab from "./Tabs/EditHistoryTab"
import MetadataTab from "./Tabs/MetadataTab"
import TimeSeriesTab from "./Tabs/TimeSeriesTab"
import { useAppContext } from "@/Context/AppContext"
import { formatDate } from "@/Utils/DateUtils"

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
        padding: 0 5px;
    }

    .MuiTab-root {
        min-height: 48px;
        text-transform: none;
        font-weight: 500;
        font-size: 14px;
        padding: 0;
        margin-right: 24px;
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
}: Props) => {
    const { developerMode } = useAppContext()
    const [activeTab, setActiveTab] = useState(0)

    const handleTabChange = (_: React.SyntheticEvent, newValue: number) => {
        setActiveTab(newValue)
    }

    const headerData = useMemo(() => {
        const clickedYearValue = rowData?.[rowData?.clickedYear]
            ?? (rowData?.profile?.values?.[0] ?? rowData?.overrideProfile?.values?.[0] ?? 0)

        const formatValue = (value: number, unit?: string) => {
            if (unit?.toLowerCase().includes("sm3")) {
                return `${value.toLocaleString("en-US", { maximumFractionDigits: 2 })} ${unit}`
            }
            if (unit?.toLowerCase().includes("mnok")) {
                return `${value.toLocaleString("en-US", { maximumFractionDigits: 1 })} ${unit}`
            }
            if (unit?.toLowerCase().includes("co2")) {
                return `${value.toLocaleString("en-US", { maximumFractionDigits: 0 })} ${unit}`
            }
            return `${value.toLocaleString("en-US", { maximumFractionDigits: 2 })} ${unit ?? ""}`
        }

        const lastUpdated = rowData?.overrideProfile?.lastUpdated ?? rowData?.profile?.lastUpdated ?? "Not available"
        const formattedDate = formatDate(lastUpdated)

        return {
            title: rowData?.profileName?.replace("Override", "")
                   || rowData?.resourceName?.replace("Override", "")
                   || "Unknown Profile",

            value: formatValue(clickedYearValue, rowData?.unit),

            year: rowData?.clickedYear
                  || (rowData?.profile?.startYear ? dg4Year + rowData.profile.startYear : "N/A"),

            lastUpdated: formattedDate,

            source: rowData?.overrideProfile?.override ? "Manual input" : "Calculated",
        }
    }, [rowData, dg4Year])

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
            PaperProps={{
                style: { boxShadow: "0px 8px 24px rgba(0, 0, 0, 0.15)" },
            }}
        >
            <DrawerContent>
                <SidesheetHeader {...headerData} onClose={onClose} />

                <TabsContainer>
                    <Tabs
                        value={activeTab}
                        onChange={handleTabChange}
                        variant="scrollable"
                        scrollButtons="auto"
                        TabIndicatorProps={{
                            style: { transition: "none" },
                        }}
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

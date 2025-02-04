import { Drawer, Tab, Tabs } from "@mui/material"
import styled from "styled-components"
import { useState, useMemo } from "react"
import { tokens } from "@equinor/eds-tokens"
import SidesheetHeader from "./SidesheetHeader"
import CalculationsTab from "./Tabs/CalculationsTab"
import EditHistoryTab from "./Tabs/EditHistoryTab"
import MetadataTab from "./Tabs/MetadataTab"
import TimeSeriesTab from "./Tabs/TimeSeriesTab"
import EnvironmentalImpactTab from "./Tabs/EnvironmentalImpactTab"
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
        // Get the clicked year's value, or fall back to the first available value
        const clickedYearValue = rowData?.[rowData?.clickedYear]
            ?? (rowData?.profile?.values?.[0] ?? rowData?.overrideProfile?.values?.[0] ?? 0)

        // Format the value based on the unit type
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

        // Get the last updated timestamp from the profile or override
        const lastUpdated = rowData?.overrideProfile?.lastUpdated ?? rowData?.profile?.lastUpdated ?? "Not available"
        const formattedDate = formatDate(lastUpdated)

        return {
            // Use the profile name or resource name, removing any 'Override' suffix
            title: rowData?.profileName?.replace("Override", "")
                   || rowData?.resourceName?.replace("Override", "")
                   || "Unknown Profile",

            // Format the value with its unit
            value: formatValue(clickedYearValue, rowData?.unit),

            // Use the clicked year or the first year from the profile
            year: rowData?.clickedYear
                  || (rowData?.profile?.startYear ? dg4Year + rowData.profile.startYear : "N/A"),

            // Format the last updated date
            lastUpdated: formattedDate,

            // Determine the source based on override status
            source: rowData?.overrideProfile?.override ? "Manual input" : "Calculated",
        }
    }, [rowData, dg4Year])

    const hasTimeSeriesData = rowData?.profile?.values?.length > 0 || rowData?.overrideProfile?.values?.length > 0

    // Check if this is an emissions-related profile
    const isEmissionsProfile = useMemo(() => {
        const emissionsProfiles = ["Co2Emissions", "FuelFlaringAndLosses", "ImportedElectricity"]
        return emissionsProfiles.some((p) => rowData?.resourceName?.includes(p))
    }, [rowData])

    // Calculate tab indices based on available tabs
    const getTabIndex = (baseIndex: number) => {
        let index = baseIndex
        if (!hasTimeSeriesData) { index -= 1 }
        if (!isEmissionsProfile) { index -= 1 }
        return Math.max(0, index)
    }

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
                        {hasTimeSeriesData && <Tab label="Time Series" />}
                        {isEmissionsProfile && <Tab label="Environmental Impact" />}
                        {developerMode && <Tab label="Calculations" />}
                        <Tab label="Edit history" />
                        {developerMode && <Tab label="Metadata" />}
                    </Tabs>
                </TabsContainer>

                {hasTimeSeriesData && activeTab === 0 && (
                    <TimeSeriesTab rowData={rowData} dg4Year={dg4Year} />
                )}

                {isEmissionsProfile && activeTab === getTabIndex(1) && (
                    <EnvironmentalImpactTab
                        rowData={rowData}
                    />
                )}

                {activeTab === getTabIndex(2) && (
                    <CalculationsTab
                        profileName={headerData.title}
                        rowData={rowData}
                    />
                )}

                {activeTab === getTabIndex(3) && (
                    <EditHistoryTab />
                )}

                {developerMode && activeTab === getTabIndex(4) && (
                    <MetadataTab rowData={rowData} />
                )}
            </DrawerContent>
        </Drawer>
    )
}

export default SidesheetWrapper

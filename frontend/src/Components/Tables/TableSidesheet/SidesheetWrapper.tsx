import { Drawer, Tab, Tabs } from "@mui/material"
import styled from "styled-components"
import { useState, useMemo } from "react"
import SidesheetHeader from "./SidesheetHeader"
import CalculationsTab from "./Tabs/CalculationsTab"
import EditHistoryTab from "./Tabs/EditHistoryTab"
import MetadataTab from "./Tabs/MetadataTab"
import TimeSeriesTab from "./Tabs/TimeSeriesTab"
import ComparativeAnalysisTab from "./Tabs/ComparativeAnalysisTab"
import EnvironmentalImpactTab from "./Tabs/EnvironmentalImpactTab"
import { useAppContext } from "@/Context/AppContext"

const DrawerContent = styled.div`
    width: 480px;
    height: 100%;
    display: flex;
    flex-direction: column;
    background: #FFFFFF;
`

const TabsContainer = styled.div`
    border-bottom: 1px solid #E0E0E0;
    
    .MuiTabs-root {
        min-height: 48px;
        padding: 0 32px;
    }

    .MuiTab-root {
        min-height: 48px;
        text-transform: none;
        font-weight: 500;
        font-size: 14px;
        padding: 0;
        margin-right: 24px;
        min-width: unset;
        color: #6F6F6F;

        &.Mui-selected {
            color: #007079;
        }
    }

    .MuiTabs-indicator {
        background-color: #007079;
        height: 2px;
    }
`

const TabContent = styled.div`
    padding: 24px 32px;
    flex: 1;
    overflow-y: auto;
`

const CalculationContainer = styled.div`
    font-size: 14px;
    color: #1A1A1A;

    .calculation-row {
        margin-bottom: 12px;
        line-height: 1.5;
    }
    
    .formula {
        margin: 16px 0;
        padding: 16px;
        background: #F7F7F7;
        border-radius: 4px;
        font-family: monospace;
        line-height: 1.5;
    }
    
    .result {
        color: #1A1A1A;
        font-weight: 500;
        margin-top: 12px;
        font-family: monospace;
    }

    .calculation-title {
        font-weight: 500;
        margin-bottom: 8px;
    }

    .calculation-description {
        color: #6F6F6F;
        margin-bottom: 24px;
    }
`

interface Props {
    isOpen: boolean
    onClose: () => void
    rowData: any
    dg4Year: number
    allTimeSeriesData?: any[]
}

const SidesheetWrapper = ({ isOpen, onClose, rowData, dg4Year, allTimeSeriesData }: Props) => {
    const { developerMode } = useAppContext()
    const [activeTab, setActiveTab] = useState(0)

    const handleTabChange = (_: React.SyntheticEvent, newValue: number) => {
        setActiveTab(newValue)
    }

    const headerData = useMemo(() => {
        // Get the clicked year's value, or fall back to the first available value
        const clickedYearValue = rowData?.[rowData?.clickedYear] ?? 
            (rowData?.profile?.values?.[0] ?? rowData?.overrideProfile?.values?.[0] ?? 0)

        // Format the value based on the unit type
        const formatValue = (value: number, unit?: string) => {
            if (unit?.toLowerCase().includes('sm3')) {
                return `${value.toLocaleString('en-US', { maximumFractionDigits: 2 })} ${unit}`
            }
            if (unit?.toLowerCase().includes('mnok')) {
                return `${value.toLocaleString('en-US', { maximumFractionDigits: 1 })} ${unit}`
            }
            if (unit?.toLowerCase().includes('co2')) {
                return `${value.toLocaleString('en-US', { maximumFractionDigits: 0 })} ${unit}`
            }
            return `${value.toLocaleString('en-US', { maximumFractionDigits: 2 })} ${unit ?? ''}`
        }

        // Get the last updated timestamp from the profile or override
        const lastUpdated = rowData?.overrideProfile?.lastUpdated ?? rowData?.profile?.lastUpdated ?? 'Not available'
        const formattedDate = lastUpdated !== 'Not available' 
            ? new Date(lastUpdated).toLocaleDateString('en-GB', {
                day: 'numeric',
                month: 'long',
                year: 'numeric'
              })
            : lastUpdated

        return {
            // Use the profile name or resource name, removing any 'Override' suffix
            title: rowData?.profileName?.replace('Override', '') || 
                   rowData?.resourceName?.replace('Override', '') || 
                   'Unknown Profile',
            
            // Format the value with its unit
            value: formatValue(clickedYearValue, rowData?.unit),
            
            // Use the clicked year or the first year from the profile
            year: rowData?.clickedYear || 
                  (rowData?.profile?.startYear ? dg4Year + rowData.profile.startYear : 'N/A'),
            
            // Format the last updated date
            lastUpdated: formattedDate,
            
            // Determine the source based on override status
            source: rowData?.overrideProfile?.override ? "Manual input" : "Calculated"
        }
    }, [rowData, dg4Year])

    const hasTimeSeriesData = rowData?.profile?.values?.length > 0 || rowData?.overrideProfile?.values?.length > 0

    // Find related profiles based on the current profile type
    const relatedProfiles = useMemo(() => {
        if (!rowData) return []

        // Group related profile types based on business logic and data relationships
        const profileGroups = {
            // Production profiles are grouped together because:
            // 1. They all represent volumetric flow rates over time
            // 2. They share common units (Sm3, Sm3/day)
            // 3. They are interdependent - e.g., water injection affects oil/gas production
            // 4. They include both base and additional production for comprehensive analysis
            // 5. They directly affect the net sales gas calculations
            production: [
                'ProductionProfileOil',
                'AdditionalProductionProfileOil',
                'ProductionProfileGas',
                'AdditionalProductionProfileGas',
                'ProductionProfileWater',
                'ProductionProfileWaterInjection',
                'ProductionProfileNetSalesGasOverride',
                'DeferredOilProduction',
                'DeferredGasProduction',
                'NetSalesGasOverride'
            ],

            // Emissions profiles are grouped because:
            // 1. They all contribute to environmental impact calculations
            // 2. They are used together in the EnvironmentalImpactTab component
            // 3. CO2 emissions are directly related to fuel/flaring/losses
            // 4. Imported electricity indirectly affects emissions through power consumption
            // 5. They share common units (tonnes CO2)
            emissions: [
                'Co2Emissions',
                'Co2EmissionsOverride',
                'Co2Intensity',
                'FuelFlaringAndLosses',
                'ImportedElectricity',
                'ProductionProfileFuelFlaringAndLossesOverride',
                'ProductionProfileImportedElectricityOverride'
            ],

            // Well costs are grouped because:
            // 1. They all relate to well operations and interventions
            // 2. They share common cost calculation methodologies
            // 3. They are managed together in the WellProject component
            // 4. They have similar timing and scheduling dependencies
            // 5. They use the same cost basis and currency units
            wellCosts: [
                'WellProjectOilProducerCost',
                'WellProjectGasProducerCost',
                'WellProjectWaterInjectorCost',
                'WellProjectGasInjectorCost',
                'OilProducerCostProfile',
                'GasProducerCostProfile',
                'WaterInjectorCostProfile',
                'GasInjectorCostProfile',
                'WellInterventionCostProfile'
            ],

            // Facilities costs are grouped because:
            // 1. They represent physical infrastructure costs
            // 2. They are typically managed as part of CAPEX planning
            // 3. They share dependencies in the infrastructure lifecycle
            // 4. They have similar cost estimation methodologies
            // 5. They are often tendered and contracted together
            facilitiesCosts: [
                'TopsideCost',
                'SurfCost',
                'TransportCost',
                'SubstructureCost',
                'OffshoreFacilitiesOperationsCost',
                'OnshoreRelatedOPEXCost',
                'AdditionalOPEXCost',
                'OnshorePowerSupplyCost'
            ],

            // Cessation costs are grouped because:
            // 1. They all relate to end-of-life activities
            // 2. They share timing dependencies (all occur during decommissioning)
            // 3. They are calculated using similar methodologies
            // 4. They are often planned and executed as a single project
            // 5. They share regulatory and environmental requirements
            cessationCosts: [
                'CessationWellsCost',
                'CessationOffshoreFacilitiesCost',
                'CessationOnshoreFacilitiesCost'
            ],

            // Study costs are grouped because:
            // 1. They represent pre-project and planning costs
            // 2. They occur early in the project lifecycle
            // 3. They share similar approval and governance processes
            // 4. They are typically managed by the same team
            // 5. They have similar cost estimation uncertainties
            studyCosts: [
                'TotalFeasibilityAndConceptStudies',
                'TotalFEEDStudies',
                'TotalOtherStudiesCost',
                'SeismicAcquisitionAndProcessing',
                'CountryOfficeCost'
            ],

            // Exploration costs are grouped because:
            // 1. They all relate to early-phase exploration activities
            // 2. They share high uncertainty and risk profiles
            // 3. They are managed under exploration licenses
            // 4. They have similar cost approval processes
            // 5. They often use the same resources and equipment
            explorationCosts: [
                'ExplorationWellCostProfile',
                'AppraisalWellCostProfile',
                'SidetrackCostProfile'
            ],

            // Historic costs are grouped because:
            // 1. They represent actual spent costs rather than estimates
            // 2. They are used as reference for future cost estimates
            // 3. They share similar data quality and audit requirements
            // 4. They are typically managed by finance/accounting teams
            // 5. They are used in variance analysis
            historicCosts: [
                'HistoricCostCostProfile',
                'GAndGAdminCost'
            ]
        }

        // Find which group the current profile belongs to
        let currentGroup = null
        for (const [group, profiles] of Object.entries(profileGroups)) {
            if (profiles.some(p => rowData.resourceName?.includes(p))) {
                currentGroup = group
                break
            }
        }

        if (!currentGroup) return []

        // Return other profiles from the same group that have data
        return profileGroups[currentGroup as keyof typeof profileGroups]
            .filter(profileType => 
                rowData.resourceName?.includes(profileType) &&
                (rowData.profile?.values?.length > 0 || rowData.overrideProfile?.values?.length > 0)
            )
            .map(profileType => ({
                profileName: profileType,
                profile: rowData.profile,
                overrideProfile: rowData.overrideProfile
            }))
    }, [rowData])

    const hasRelatedProfiles = relatedProfiles.length > 0

    // Check if this is an emissions-related profile
    const isEmissionsProfile = useMemo(() => {
        const emissionsProfiles = ['Co2Emissions', 'FuelFlaringAndLosses', 'ImportedElectricity']
        return emissionsProfiles.some(p => rowData?.resourceName?.includes(p))
    }, [rowData])

    // Calculate tab indices based on available tabs
    const getTabIndex = (baseIndex: number) => {
        let index = baseIndex
        if (!hasTimeSeriesData) index--
        if (!hasRelatedProfiles) index--
        if (!isEmissionsProfile) index--
        return Math.max(0, index)
    }

    return (
        <Drawer
            anchor="right"
            open={isOpen}
            onClose={onClose}
            PaperProps={{
                style: { boxShadow: '0px 8px 24px rgba(0, 0, 0, 0.15)' }
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
                            style: { transition: 'none' }
                        }}
                    >
                        {hasTimeSeriesData && <Tab label="Time Series" />}
                        {hasRelatedProfiles && <Tab label="Comparative Analysis" />}
                        {isEmissionsProfile && <Tab label="Environmental Impact" />}
                        {developerMode && <Tab label="Calculations" />}
                        <Tab label="Edit history" />
                        {developerMode && <Tab label="Metadata" />}
                    </Tabs>
                </TabsContainer>

                {hasTimeSeriesData && activeTab === 0 && (
                    <TimeSeriesTab rowData={rowData} dg4Year={dg4Year} />
                )}

                {hasRelatedProfiles && activeTab === getTabIndex(1) && (
                    <ComparativeAnalysisTab 
                        rowData={rowData} 
                        dg4Year={dg4Year}
                        relatedProfiles={relatedProfiles}
                    />
                )}

                {isEmissionsProfile && activeTab === getTabIndex(2) && (
                    <EnvironmentalImpactTab
                        rowData={rowData}
                        dg4Year={dg4Year}
                    />
                )}

                {activeTab === getTabIndex(3) && (
                    <CalculationsTab 
                        profileName={headerData.title} 
                        rowData={rowData}
                        allTimeSeriesData={allTimeSeriesData}
                    />
                )}

                {activeTab === getTabIndex(4) && (
                    <EditHistoryTab />
                )}

                {developerMode && activeTab === getTabIndex(5) && (
                    <MetadataTab rowData={rowData} />
                )}
            </DrawerContent>
        </Drawer>
    )
}

export default SidesheetWrapper

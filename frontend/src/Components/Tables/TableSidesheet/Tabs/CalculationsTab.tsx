import { Typography } from "@mui/material"
import styled from "styled-components"
import { ProfileNames } from "@/Models/Interfaces"
import { createLogger } from "@/Utils/logger"
import GAndGAdmin from "./Calculations/GAndGAdmin"
import FuelFlaringLosses from "./Calculations/FuelFlaringLosses"
import CO2Emissions from "./Calculations/CO2Emissions"
import NetSalesGas from "./Calculations/NetSalesGas"
import FeasibilityStudies from "./Calculations/FeasibilityStudies"
import FeedStudies from "./Calculations/FeedStudies"
import WellIntervention from "./Calculations/WellIntervention"
import OffshoreFacilitiesOperations from "./Calculations/OffshoreFacilitiesOperations"
import CessationWells from "./Calculations/CessationWells"
import OilProducerWell from "./Calculations/OilProducerWell"
import GasProducerWell from "./Calculations/GasProducerWell"
import WaterInjectorWell from "./Calculations/WaterInjectorWell"
import GasInjectorWell from "./Calculations/GasInjectorWell"
import ImportedElectricity from "./Calculations/ImportedElectricity"
import { useAppContext } from "@/Context/AppContext"

const developerMode: boolean = true // Toggle developer information visibility

const logger = createLogger({ 
    name: "CalculationsTab",
    enabled: true
})

interface Props {
    profileName: ProfileNames
    rowData?: {
        resourceName?: string
        resourceType?: string
        [key: string]: any
    }
    allTimeSeriesData?: any[]
}

const Container = styled.div`
    padding: 24px;
    max-width: 100%;
    margin: 0 auto;
`

const Section = styled.div`
    margin-bottom: 24px;
    background: #fff;
    border-radius: 4px;
    box-shadow: 0 1px 2px rgba(0, 0, 0, 0.1);
    padding: 24px;
`

const Formula = styled.div`
    font-family: 'Roboto Mono', monospace;
    background-color: #f5f6f7;
    padding: 20px;
    border-radius: 4px;
    margin: 16px 0;
    line-height: 1.5;
`

const MainFormula = styled.div`
    font-weight: 500;
    color: #0D47A1;
    font-size: 1.1em;
    margin-bottom: 20px;
    padding: 12px 16px;
    background: #E3F2FD;
    border-radius: 4px;
    border-left: 4px solid #1976D2;
`

const FormulaSection = styled.div`
    margin: 16px 0;
    
    h4 {
        color: #1976D2;
        font-size: 1em;
        margin: 0 0 8px 0;
        font-weight: 500;
    }
`

const SubFormula = styled.div`
    margin: 8px 0;
    padding: 8px 12px;
    background: #FAFAFA;
    border-radius: 4px;
    color: #424242;
    font-size: 0.95em;
`

const FormulaList = styled.ul`
    margin: 8px 0;
    padding: 0;
    list-style-type: none;

    & li {
        margin: 6px 0;
        padding-left: 16px;
        position: relative;
        color: #424242;

        &:before {
            content: "•";
            color: #1976D2;
            position: absolute;
            left: 0;
        }

        & ul {
            margin: 4px 0 4px 16px;
            padding: 0;
            list-style-type: none;

            & li {
                margin: 4px 0;
                font-size: 0.95em;
                color: #616161;

                &:before {
                    content: "○";
                }
            }
        }
    }
`

const SectionTitle = styled(Typography)`
    color: #1976d2;
    font-weight: 500;
    margin-bottom: 12px !important;
`

const Note = styled.div`
    font-size: 13px;
    color: #757575;
    margin-top: 20px;
    padding: 16px;
    background: #FAFAFA;
    border-radius: 4px;
    line-height: 1.5;

    & strong {
        color: #424242;
    }

    & ul {
        margin: 8px 0;
        padding-left: 20px;
    }

    & li {
        margin: 4px 0;
    }
`

const SpecialNote = styled(Typography)`
    margin-top: 16px !important;
    padding: 12px 16px;
    background-color: #FFF3E0;
    border-left: 4px solid #FB8C00;
    border-radius: 4px;
    padding: 12px;
    background-color: #fff3e0;
    border-left: 4px solid #ff9800;
    border-radius: 4px;
`

const CalculationsTab: React.FC<Props> = ({ profileName, rowData, allTimeSeriesData = [] }: Props) => {
    const { developerMode } = useAppContext()
    // Log only essential information for formula identification
    console.group("🔍 Formula Identification Data")
    
    // Profile info
    const profileData = {
        name: profileName,
        resourceName: rowData?.resourceName ?? "N/A",
        unit: rowData?.unit ?? "N/A"
    }
    console.log("Profile:", profileData)

    // Sample data point for verification
    const sampleValue = rowData?.profile?.values?.[0]
    if (sampleValue !== undefined && rowData?.profile?.startYear !== undefined) {
        console.log("Sample Value:", {
            year: rowData.profile.startYear,
            value: sampleValue,
            unit: rowData?.unit ?? "N/A"
        })
    }

    // Log any override status
    if (rowData?.profile?.override !== undefined) {
        console.log("Override Status:", rowData.profile.override)
    }

    console.groupEnd()

    // Helper function to determine which calculation to show
    const getCalculationType = () => {
        // Check resource name first
        if (rowData?.resourceName === "gAndGAdminCost" || rowData?.resourceName === "gAndGAdminCostOverride") {
            return "gAndGAdmin"
        }
        if (rowData?.resourceName === "productionProfileFuelFlaringAndLossesOverride") {
            return "fuelFlaringLosses"
        }
        if (rowData?.resourceName === "co2EmissionsOverride") {
            return "co2Emissions"
        }
        if (rowData?.resourceName === "netSalesGasOverride" || rowData?.resourceName === "productionProfileNetSalesGasOverride") {
            return "netSalesGas"
        }
        if (rowData?.resourceName === "totalFeasibilityAndConceptStudiesOverride") {
            return "feasibilityStudies"
        }
        if (rowData?.resourceName === "totalFEEDStudiesOverride") {
            return "feedStudies"
        }
        if (rowData?.resourceName === "wellInterventionCostProfileOverride") {
            return "wellIntervention"
        }
        if (rowData?.resourceName === "offshoreFacilitiesOperationsCostProfileOverride") {
            return "offshoreFacilitiesOperations"
        }
        if (rowData?.resourceName === "cessationWellsCostOverride") {
            return "cessationWells"
        }
        if (rowData?.resourceName === "wellProjectOilProducerCostOverride") {
            return "oilProducerWell"
        }
        if (rowData?.resourceName === "wellProjectGasProducerCostOverride") {
            return "gasProducerWell"
        }
        if (rowData?.resourceName === "wellProjectWaterInjectorCostOverride") {
            return "waterInjectorWell"
        }
        if (rowData?.resourceName === "wellProjectGasInjectorCostOverride") {
            return "gasInjectorWell"
        }
        if (rowData?.resourceName === "productionProfileImportedElectricityOverride") {
            return "importedElectricity"
        }

        // Fallback to profile name checks
        switch (profileName) {
            case "gAndGAdminCost":
                return "gAndGAdmin"
            case "productionProfileFuelFlaringAndLossesOverride":
                return "fuelFlaringLosses"
            case "co2EmissionsOverride":
                return "co2Emissions"
            case "netSalesGasOverride":
                return "netSalesGas"
            case "totalFeasibilityAndConceptStudiesOverride":
                return "feasibilityStudies"
            case "totalFEEDStudiesOverride":
                return "feedStudies"
            case "wellInterventionCostProfileOverride":
                return "wellIntervention"
            case "offshoreFacilitiesOperationsCostProfileOverride":
                return "offshoreFacilitiesOperations"
            case "cessationWellsCostOverride":
                return "cessationWells"
            case "wellProjectOilProducerCostOverride":
                return "oilProducerWell"
            case "wellProjectGasProducerCostOverride":
                return "gasProducerWell"
            case "wellProjectWaterInjectorCostOverride":
                return "waterInjectorWell"
            case "wellProjectGasInjectorCostOverride":
                return "gasInjectorWell"
            case "productionProfileImportedElectricityOverride":
                return "importedElectricity"
            default:
                return "unknown"
        }
    }

    // Render different calculations based on type
    switch (getCalculationType()) {
        case "gAndGAdmin":
            return <GAndGAdmin developerMode={developerMode} hasOverride={!!rowData?.overrideProfile?.override} />
        case "fuelFlaringLosses":
            return <FuelFlaringLosses developerMode={developerMode} hasOverride={!!rowData?.overrideProfile?.override} />
        case "co2Emissions":
            return <CO2Emissions developerMode={developerMode} hasOverride={!!rowData?.overrideProfile?.override} />
        case "netSalesGas":
            return <NetSalesGas developerMode={developerMode} hasOverride={!!rowData?.overrideProfile?.override} />
        case "feasibilityStudies":
            return <FeasibilityStudies developerMode={developerMode} hasOverride={!!rowData?.overrideProfile?.override} />
        case "feedStudies":
            return <FeedStudies developerMode={developerMode} hasOverride={!!rowData?.overrideProfile?.override} />
        case "wellIntervention":
            return <WellIntervention developerMode={developerMode} hasOverride={!!rowData?.overrideProfile?.override} />
        case "offshoreFacilitiesOperations":
            return <OffshoreFacilitiesOperations developerMode={developerMode} hasOverride={!!rowData?.overrideProfile?.override} />
        case "cessationWells":
            return <CessationWells developerMode={developerMode} hasOverride={!!rowData?.overrideProfile?.override} />
        case "oilProducerWell":
            return <OilProducerWell developerMode={developerMode} hasOverride={!!rowData?.overrideProfile?.override} />
        case "gasProducerWell":
            return <GasProducerWell developerMode={developerMode} hasOverride={!!rowData?.overrideProfile?.override} />
        case "waterInjectorWell":
            return <WaterInjectorWell developerMode={developerMode} hasOverride={!!rowData?.overrideProfile?.override} />
        case "gasInjectorWell":
            return <GasInjectorWell developerMode={developerMode} hasOverride={!!rowData?.overrideProfile?.override} />
        case "importedElectricity":
            return <ImportedElectricity developerMode={developerMode} hasOverride={!!rowData?.overrideProfile?.override} />
        default:
            return (
                <Container>
                    <Section>
                        <SectionTitle variant="h6">Calculation Details</SectionTitle>
                        <FormulaSection>
                            <Typography variant="body1" gutterBottom>
                                This profile type doesn't have specific calculation details available yet:
                            </Typography>
                            <Note>
                                <Typography variant="body2" component="div">
                                    <strong>What does this mean?</strong>
                                    <FormulaList>
                                        <li>This profile may be a direct input value that doesn't require calculations</li>
                                        <li>The calculation documentation might be pending</li>
                                    </FormulaList>
                                </Typography>
                            </Note>
                        </FormulaSection>
                        
                        

                        {developerMode && (
                            <SpecialNote variant="body2">
                                Developer Note: To add calculations for this profile type, create a new component in the 
                                Calculations folder and update the getCalculationType() function to include the mapping.
                                <br />
                                <br />
                                <strong>Profile Name:</strong> {profileName}
                                {rowData?.resourceName && (
                                    <>
                                        <br />
                                        <strong>Resource Name:</strong> {rowData.resourceName}
                                    </>
                                )}
                                {rowData?.resourceType && (
                                    <>
                                        <br />
                                        <strong>Resource Type:</strong> {rowData.resourceType}
                                    </>
                                )}
                            </SpecialNote>
                        )}
                    </Section>
                </Container>
            )
    }
}

export default CalculationsTab

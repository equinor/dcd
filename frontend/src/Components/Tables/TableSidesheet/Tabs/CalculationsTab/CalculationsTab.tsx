import { Typography } from "@mui/material"
import { ProfileNames } from "@/Models/Interfaces"
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
import CessationOffshoreFacilities from "./Calculations/CessationOffshoreFacilities"
import { useAppStore } from "@/Store/AppStore"
import SubmitMistakes from "./submitMistakes"
import {
    Container,
    Section,
    FormulaSection,
    FormulaList,
    SectionTitle,
    Note,
    SpecialNote,
} from "../../shared.styles"

interface Props {
    profileName: ProfileNames
    rowData?: {
        resourceName?: string
        resourceType?: string
        [key: string]: any
    }
}

const CalculationsTab: React.FC<Props> = ({ profileName, rowData = [] }: Props) => {
    const { developerMode } = useAppStore()
    // determine which calculation to show
    const getCalculationType = () => {
        // Check resource name first
        if (rowData?.resourceName === "gAndGAdminCost" || rowData?.resourceName === "gAndGAdminCostOverride") {
            return "gAndGAdmin"
        }
        if (rowData?.resourceName === "cessationOffshoreFacilitiesCostOverride") {
            return "cessationOffshoreFacilities"
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
        case "cessationOffshoreFacilitiesCostOverride":
            return "cessationOffshoreFacilities"
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

    return (
        <>
            <div style={{ fontStyle: "italic" }}>
                <SubmitMistakes
                    profileName={profileName}
                    rowData={rowData}
                />
            </div>
            {(() => {
                switch (getCalculationType()) {
                case "gAndGAdmin":
                    return <GAndGAdmin developerMode={developerMode} hasOverride={!!rowData?.overrideProfile?.override} />
                case "cessationOffshoreFacilities":
                    return <CessationOffshoreFacilities developerMode={developerMode} hasOverride={!!rowData?.overrideProfile?.override} />
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
                                        This profile type doesn&apos;t have specific calculation details available yet:
                                    </Typography>
                                    <Note>
                                        <Typography variant="body2" component="div">
                                            <strong>What does this mean?</strong>
                                            <FormulaList>
                                                <li>This profile may be a direct input value that doesn&apos;t require calculations</li>
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
                                        <strong>Profile Name:</strong>
                                        {" "}
                                        {profileName}
                                        {rowData?.resourceName && (
                                            <>
                                                <br />
                                                <strong>Resource Name:</strong>
                                                {" "}
                                                {rowData.resourceName}
                                            </>
                                        )}
                                        {rowData?.resourceType && (
                                            <>
                                                <br />
                                                <strong>Resource Type:</strong>
                                                {" "}
                                                {rowData.resourceType}
                                            </>
                                        )}
                                    </SpecialNote>
                                )}
                            </Section>
                        </Container>
                    )
                }
            })()}
        </>
    )
}

export default CalculationsTab

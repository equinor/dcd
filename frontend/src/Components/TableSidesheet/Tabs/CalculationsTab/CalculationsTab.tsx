import { Typography } from "@mui/material"

import {
    Container,
    Section,
    FormulaSection,
    FormulaList,
    SectionTitle,
    Note,
    SpecialNote,
} from "../../shared.styles"

import CO2Emissions from "./Calculations/CO2Emissions"
import CessationOffshoreFacilities from "./Calculations/CessationOffshoreFacilities"
import CessationWells from "./Calculations/CessationWells"
import CondensateProduction from "./Calculations/CondensateProduction"
import FeasibilityStudies from "./Calculations/FeasibilityStudies"
import FeedStudies from "./Calculations/FeedStudies"
import FuelFlaringLosses from "./Calculations/FuelFlaringLosses"
import GAndGAdmin from "./Calculations/GAndGAdmin"
import GasInjectorWell from "./Calculations/GasInjectorWell"
import GasProducerWell from "./Calculations/GasProducerWell"
import ImportedElectricity from "./Calculations/ImportedElectricity"
import NetSalesGas from "./Calculations/NetSalesGas"
import OffshoreFacilitiesOperations from "./Calculations/OffshoreFacilitiesOperations"
import OilProducerWell from "./Calculations/OilProducerWell"
import ProductionProfileNgl from "./Calculations/ProductionProfileNgl"
import TotalExportedVolumes from "./Calculations/TotalExportedVolumes"
import WaterInjectorWell from "./Calculations/WaterInjectorWell"
import WellIntervention from "./Calculations/WellIntervention"
import SubmitMistakes from "./submitMistakes"

import { ProfileTypes } from "@/Models/enums"
import { useAppStore } from "@/Store/AppStore"

interface Props {
    profileType: ProfileTypes
    rowData?: {
        resourceName?: string
        resourceType?: string
        [key: string]: any
    }
}

const CalculationsTab: React.FC<Props> = ({ profileType, rowData = [] }: Props) => {
    const { developerMode } = useAppStore()
    // determine which calculation to show
    const getCalculationType = () => {
        // Check resource name first
        if (rowData?.resourceName === ProfileTypes.GAndGAdminCost || rowData?.resourceName === ProfileTypes.GAndGAdminCostOverride) {
            return "gAndGAdmin"
        }
        if (rowData?.resourceName === ProfileTypes.CessationOffshoreFacilitiesCostOverride) {
            return "cessationOffshoreFacilities"
        }
        if (rowData?.resourceName === ProfileTypes.FuelFlaringAndLossesOverride) {
            return "fuelFlaringLosses"
        }
        if (rowData?.resourceName === ProfileTypes.Co2EmissionsOverride) {
            return "co2Emissions"
        }
        if (rowData?.resourceName === ProfileTypes.NetSalesGasOverride) {
            return "netSalesGas"
        }
        if (rowData?.resourceName === ProfileTypes.TotalExportedVolumesOverride) {
            return "totalExportedVolumes"
        }
        if (rowData?.resourceName === ProfileTypes.TotalFeasibilityAndConceptStudiesOverride) {
            return "feasibilityStudies"
        }
        if (rowData?.resourceName === ProfileTypes.TotalFeedStudiesOverride) {
            return "feedStudies"
        }
        if (rowData?.resourceName === ProfileTypes.WellInterventionCostProfileOverride) {
            return "wellIntervention"
        }
        if (rowData?.resourceName === ProfileTypes.OffshoreFacilitiesOperationsCostProfileOverride) {
            return "offshoreFacilitiesOperations"
        }
        if (rowData?.resourceName === ProfileTypes.CessationWellsCostOverride) {
            return "cessationWells"
        }
        if (rowData?.resourceName === ProfileTypes.OilProducerCostProfileOverride) {
            return "oilProducerWell"
        }
        if (rowData?.resourceName === ProfileTypes.GasProducerCostProfileOverride) {
            return "gasProducerWell"
        }
        if (rowData?.resourceName === ProfileTypes.WaterInjectorCostProfileOverride) {
            return "waterInjectorWell"
        }
        if (rowData?.resourceName === ProfileTypes.GasInjectorCostProfileOverride) {
            return "gasInjectorWell"
        }
        if (rowData?.resourceName === ProfileTypes.ImportedElectricityOverride) {
            return "importedElectricity"
        }
        if (rowData?.resourceName === ProfileTypes.ProductionProfileNglOverride) {
            return "productionProfileNgl"
        }
        if (rowData?.resourceName === ProfileTypes.CondensateProductionOverride) {
            return "condensateProduction"
        }

        // Fallback to profile name checks
        switch (profileType) {
        case ProfileTypes.GAndGAdminCostOverride:
            return "gAndGAdmin"
        case ProfileTypes.CessationOffshoreFacilitiesCostOverride:
            return "cessationOffshoreFacilities"
        case ProfileTypes.FuelFlaringAndLossesOverride:
            return "fuelFlaringLosses"
        case ProfileTypes.Co2EmissionsOverride:
            return "co2Emissions"
        case ProfileTypes.NetSalesGasOverride:
            return "netSalesGas"
        case ProfileTypes.TotalExportedVolumesOverride:
            return "totalExportedVolumes"
        case ProfileTypes.TotalFeasibilityAndConceptStudiesOverride:
            return "feasibilityStudies"
        case ProfileTypes.TotalFeedStudiesOverride:
            return "feedStudies"
        case ProfileTypes.WellInterventionCostProfileOverride:
            return "wellIntervention"
        case ProfileTypes.OffshoreFacilitiesOperationsCostProfileOverride:
            return "offshoreFacilitiesOperations"
        case ProfileTypes.CessationWellsCostOverride:
            return "cessationWells"
        case ProfileTypes.OilProducerCostProfileOverride:
            return "oilProducerWell"
        case ProfileTypes.GasProducerCostProfileOverride:
            return "gasProducerWell"
        case ProfileTypes.WaterInjectorCostProfileOverride:
            return "waterInjectorWell"
        case ProfileTypes.GasInjectorCostProfileOverride:
            return "gasInjectorWell"
        case ProfileTypes.ImportedElectricityOverride:
            return "importedElectricity"
        case ProfileTypes.ProductionProfileNglOverride:
            return "productionProfileNgl"
        case ProfileTypes.CondensateProductionOverride:
            return "condensateProduction"
        default:
            return "unknown"
        }
    }

    return (
        <>
            <div style={{ fontStyle: "italic" }}>
                <SubmitMistakes
                    profileType={profileType}
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
                case "totalExportedVolumes":
                    return <TotalExportedVolumes developerMode={developerMode} hasOverride={!!rowData?.overrideProfile?.override} />
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
                case "productionProfileNgl":
                    return <ProductionProfileNgl developerMode={developerMode} hasOverride={!!rowData?.overrideProfile?.override} />
                case "condensateProduction":
                    return <CondensateProduction developerMode={developerMode} hasOverride={!!rowData?.overrideProfile?.override} />
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
                                        {profileType}
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

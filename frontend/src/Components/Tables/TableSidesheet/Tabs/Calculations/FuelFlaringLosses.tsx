import { Typography } from "@mui/material"
import {
    Container,
    Section,
    Formula,
    MainFormula,
    FormulaSection,
    SubFormula,
    FormulaList,
    Note,
    SpecialNote,
} from "../../shared.styles"

interface Props {
    developerMode: boolean
    hasOverride: boolean
}

const FuelFlaringLosses: React.FC<Props> = ({ developerMode, hasOverride }) => (
    <Container>
        <Section>
            <Typography variant="h6">Fuel, Flaring and Losses Calculation</Typography>
            <Typography variant="body1" style={{ marginTop: 12, marginBottom: 24 }}>
                The total value (in GSm³/yr) is the sum of three components:
            </Typography>

            {/* Fuel Consumption Section */}
            <Formula>
                <MainFormula>
                    1. Fuel Consumption = TotalUseOfPower × BaseFactor
                </MainFormula>

                <FormulaSection>
                    <h4>Base Factor:</h4>
                    <SubFormula>
                        BaseFactor = FuelConsumption × 365.25 × FacilitiesAvailability/100 × 1,000,000
                    </SubFormula>
                </FormulaSection>

                <FormulaSection>
                    <h4>Power Components:</h4>
                    <FormulaList>
                        <li>
                            <strong>Oil Power:</strong>
                            <SubFormula>
                                Oil Power = BaseOilPower + DynamicOilPower
                            </SubFormula>
                            <ul>
                                <li>BaseOilPower = CO2ShareOilProfile × CO2OnMaxOilProfile</li>
                                <li>DynamicOilPower = (OilRate / (365.25 × FacilitiesAvailability/100 × OilCapacity)) × CO2ShareOilProfile × (1-CO2OnMaxOilProfile)</li>
                            </ul>
                        </li>
                        <li>
                            <strong>Gas Power:</strong>
                            <SubFormula>
                                Gas Power = BaseGasPower + DynamicGasPower
                            </SubFormula>
                            <ul>
                                <li>BaseGasPower = CO2ShareGasProfile × CO2OnMaxGasProfile</li>
                                <li>DynamicGasPower = (GasRate / (365.25 × FacilitiesAvailability/100 × GasCapacity × 1,000,000)) × CO2ShareGasProfile × (1-CO2OnMaxGasProfile)</li>
                            </ul>
                        </li>
                        <li>
                            <strong>Water Injection Power:</strong>
                            <SubFormula>
                                Water Injection Power = BaseWiPower + DynamicWiPower
                            </SubFormula>
                            <ul>
                                <li>BaseWiPower = CO2ShareWaterInjectionProfile × CO2OnMaxWaterInjectionProfile</li>
                                <li>
                                    DynamicWiPower = (WaterRate / (365.25 × FacilitiesAvailability/100 × WaterInjectionCapacity))
                                    × CO2ShareWaterInjectionProfile × (1-CO2OnMaxWaterInjectionProfile)
                                </li>
                            </ul>
                        </li>
                    </FormulaList>
                </FormulaSection>
            </Formula>

            {/* Flaring Section */}
            <Formula>
                <MainFormula>
                    2. Flaring = TotalProducedVolume × FlaredGasPerProducedVolume
                </MainFormula>

                <FormulaSection>
                    <h4>Total Produced Volume:</h4>
                    <SubFormula>
                        TotalProducedVolume = OilRate + GasRate/1000
                    </SubFormula>
                    <FormulaList>
                        <li>OilRate = ProductionProfileOil + AdditionalProductionProfileOil</li>
                        <li>GasRate = (ProductionProfileGas + AdditionalProductionProfileGas) / 1000 (converted from MSm³ to GSm³)</li>
                    </FormulaList>
                </FormulaSection>
            </Formula>

            {/* Losses Section */}
            <Formula>
                <MainFormula>
                    3. Losses = TotalGasProduction × CO2RemovedFromGas
                </MainFormula>

                <FormulaSection>
                    <h4>Total Gas Production:</h4>
                    <SubFormula>
                        TotalGasProduction = ProductionProfileGas + AdditionalProductionProfileGas
                    </SubFormula>
                </FormulaSection>
            </Formula>

            {hasOverride && (
                <SpecialNote variant="body2" color="textSecondary">
                    Note: Since this is a manually overridden profile, these calculations are not used and the values are manually set instead.
                </SpecialNote>
            )}
        </Section>

        {developerMode && (
            <Note>
                <strong>Developer Note:</strong>
                {" "}
                These calculations are implemented in:
                <ul>
                    <li>backend/api/Features/Cases/Recalculation/Types/Helpers/EmissionCalculationHelper.cs</li>
                    <li>Methods: CalculateTotalFuelConsumptions(), CalculateFlaring(), CalculateLosses()</li>
                    <li>The final values are merged in FuelFlaringLossesProfileService.cs</li>
                </ul>
            </Note>
        )}
    </Container>
)

export default FuelFlaringLosses

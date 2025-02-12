import { Typography } from "@mui/material"
import {
    Container,
    Section,
    Formula,
    MainFormula,
    FormulaSection,
    FormulaList,
    Note,
    SpecialNote,
} from "../../../shared.styles"

interface Props {
    developerMode: boolean
    hasOverride: boolean
}

const ImportedElectricity: React.FC<Props> = ({ developerMode, hasOverride }) => (
    <Container>
        <Section>
            <Typography variant="h6">Imported Electricity Calculation</Typography>
            <Typography variant="body1" style={{ marginTop: 12, marginBottom: 24 }}>
                The imported electricity (in GWh) is calculated based on power demand and facility availability:
            </Typography>

            <Formula>
                <MainFormula>
                    Imported Electricity = Total Power Demand × (FacilitiesAvailability / 100) × 8.76
                </MainFormula>

                <FormulaSection>
                    <h4>Total Power Demand Components:</h4>
                    <FormulaList>
                        <li>
                            Base Power Demand:
                            <ul>
                                <li>Oil: CO2ShareOilProfile × CO2OnMaxOilProfile × OilCapacity</li>
                                <li>Gas: CO2ShareGasProfile × CO2OnMaxGasProfile × GasCapacity</li>
                                <li>Water Injection: CO2ShareWaterInjectionProfile × CO2OnMaxWaterInjectionProfile × WaterInjectionCapacity</li>
                            </ul>
                        </li>
                        <li>
                            Variable Power Demand:
                            <ul>
                                <li>Oil: CO2ShareOilProfile × (1 - CO2OnMaxOilProfile) × OilRate</li>
                                <li>Gas: CO2ShareGasProfile × (1 - CO2OnMaxGasProfile) × GasRate</li>
                                <li>Water Injection: CO2ShareWaterInjectionProfile × (1 - CO2OnMaxWaterInjectionProfile) × WaterRate</li>
                            </ul>
                        </li>
                    </FormulaList>
                </FormulaSection>

                <FormulaSection>
                    <h4>Conversion Factors:</h4>
                    <FormulaList>
                        <li>8.76 = Conversion factor from MW to GWh per year (24 hours × 365 days / 1000)</li>
                        <li>FacilitiesAvailability is applied as a percentage (divided by 100)</li>
                    </FormulaList>
                </FormulaSection>

                <FormulaSection>
                    <h4>Special Cases:</h4>
                    <FormulaList>
                        <li>If DrainageStrategy.ArtificialLift is not set to ElectricSubmergedPumps, power demand is zero</li>
                        <li>If DrainageStrategy.GasInjection is false, gas injection power demand is zero</li>
                        <li>If DrainageStrategy.WaterInjection is false, water injection power demand is zero</li>
                        <li>If FacilitiesAvailability is not set, defaults to 100%</li>
                    </FormulaList>
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
                This calculation is implemented in:
                <ul>
                    <li>backend/api/Features/Cases/Recalculation/Types/ImportedElectricityProfile/ImportedElectricityProfileService.cs</li>
                    <li>Method: CalculateImportedElectricity()</li>
                    <li>Uses EmissionCalculationHelper.cs for power demand calculations</li>
                </ul>
            </Note>
        )}
    </Container>
)

export default ImportedElectricity

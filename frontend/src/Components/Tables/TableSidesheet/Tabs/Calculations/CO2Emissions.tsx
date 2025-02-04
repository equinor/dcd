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

const CO2Emissions: React.FC<Props> = ({ developerMode, hasOverride }) => (
    <Container>
        <Section>
            <Typography variant="h6">CO2 Emissions Calculation</Typography>
            <Typography variant="body1" style={{ marginTop: 12, marginBottom: 24 }}>
                The total CO2 emissions are calculated from multiple sources:
            </Typography>

            <Formula>
                <MainFormula>
                    CO2 Emissions = (Fuel Emissions + Flaring Emissions + Vented Emissions + Drilling Emissions) / 1000
                </MainFormula>

                <FormulaSection>
                    <h4>Component Calculations:</h4>
                    <SubFormula>
                        Fuel Emissions = Fuel Consumption × CO2EmissionFromFuelGas
                    </SubFormula>
                    <SubFormula>
                        Flaring Emissions = Flaring × CO2EmissionsFromFlaredGas
                    </SubFormula>
                    <SubFormula>
                        Vented Emissions = Losses × CO2Vented
                    </SubFormula>
                    <SubFormula>
                        Drilling Emissions = Drilling Days × DailyEmissionFromDrillingRig
                    </SubFormula>
                </FormulaSection>

                <FormulaSection>
                    <h4>Input Sources:</h4>
                    <FormulaList>
                        <li>Fuel Consumption: From FuelFlaringLossesProfile calculation</li>
                        <li>Flaring: From FuelFlaringLossesProfile calculation</li>
                        <li>Losses: From FuelFlaringLossesProfile calculation</li>
                        <li>Drilling Days: Sum from all drilling schedules</li>
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
                    <li>backend/api/Features/Cases/Recalculation/Types/Co2EmissionsProfile/Co2EmissionsProfileService.cs</li>
                    <li>Uses FuelFlaringLossesProfile components as inputs</li>
                    <li>Combines multiple emission sources with their respective conversion factors</li>
                </ul>
            </Note>
        )}
    </Container>
)

export default CO2Emissions

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
} from "../../../shared.styles"

interface Props {
    developerMode: boolean
    hasOverride: boolean
}

const TotalExportedVolumes: React.FC<Props> = ({ developerMode, hasOverride }) => (
    <Container>
        <Section>
            <Typography variant="h6">Net Sales Gas Calculation</Typography>
            <Typography variant="body1" style={{ marginTop: 12, marginBottom: 24 }}>
                Net sales gas (in GSm³/yr) is calculated as the difference between production and losses:
            </Typography>

            <Formula>
                <MainFormula>
                    Net Sales Gas = Total Gas Production - Total Fuel Flaring Losses
                </MainFormula>

                <FormulaSection>
                    <h4>Total Gas Production:</h4>
                    <SubFormula>
                        Total Gas Production = ProductionProfileGas + AdditionalProductionProfileGas
                    </SubFormula>
                </FormulaSection>

                <FormulaSection>
                    <h4>Total Fuel Flaring Losses Components:</h4>
                    <FormulaList>
                        <li>Fuel = From CalculateTotalFuelConsumptions()</li>
                        <li>Flaring = From CalculateFlaring()</li>
                        <li>Losses = From CalculateLosses()</li>
                    </FormulaList>
                </FormulaSection>

                <FormulaSection>
                    <h4>Special Cases:</h4>
                    <FormulaList>
                        <li>If ProductionProfileGas is null, Net Sales Gas = 0</li>
                        <li>If DrainageStrategy.GasSolution is Injection, Net Sales Gas = 0</li>
                        <li>If FuelFlaringAndLossesOverride is enabled, use those values instead of calculated ones</li>
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
                    <li>backend/api/Features/Cases/Recalculation/Types/NetSaleGasProfile/NetSaleGasProfileService.cs</li>
                    <li>Method: CalculateNetSaleGas()</li>
                    <li>Uses helper methods from EmissionCalculationHelper.cs for fuel, flaring, and losses calculations</li>
                </ul>
            </Note>
        )}
    </Container>
)

export default TotalExportedVolumes

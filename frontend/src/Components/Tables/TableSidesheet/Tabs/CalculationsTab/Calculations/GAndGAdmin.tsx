import { Typography } from "@mui/material"
import {
    Container,
    Section,
    Formula,
    MainFormula,
    FormulaSection,
    SubFormula,
    Note,
    SpecialNote,
} from "../../../shared.styles"

interface Props {
    developerMode: boolean
    hasOverride: boolean
}

const GAndGAdmin: React.FC<Props> = ({ developerMode, hasOverride }) => (
    <Container>
        <Section>
            <Typography variant="h6">G&G and Admin Cost Calculation</Typography>
            <Typography variant="body1" style={{ marginTop: 12 }}>
                The G&G (Geology & Geophysics) and Admin cost (in MUSD) is calculated based on the country and exploration timeline:
            </Typography>
            <Formula>
                <MainFormula>
                    G&G and Admin Cost = Country Cost Factor × Time Distribution
                </MainFormula>

                <FormulaSection>
                    <h4>Country Cost Factor (MUSD/year):</h4>
                    <ul>
                        <li>NORWAY: 1</li>
                        <li>UK: 1</li>
                        <li>BRAZIL: 3</li>
                        <li>CANADA: 3</li>
                        <li>UNITED STATES: 3</li>
                        <li>Other countries: 7</li>
                    </ul>
                </FormulaSection>

                <FormulaSection>
                    <h4>Time Distribution:</h4>
                    <ul>
                        <li>Starts from earliest exploration well drilling year</li>
                        <li>Continues until DG1 date</li>
                        <li>Full yearly cost for complete years</li>
                        <li>Last year is prorated based on DG1 date:</li>
                    </ul>
                    <SubFormula>
                        Last Year Cost = Country Cost × (DG1 Minutes / Total Minutes in Year)
                    </SubFormula>
                </FormulaSection>

                <FormulaSection>
                    <h4>Special Cases:</h4>
                    <ul>
                        <li>If no exploration well drilling schedules exist, no costs are calculated</li>
                        <li>If DG1 date is before earliest drilling year, no costs are calculated</li>
                        <li>StartYear is set to: EarliestDrillingYear - DG4Year</li>
                    </ul>
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
                    <li>backend/api/Features/Cases/Recalculation/Types/GenerateGAndGAdminCostProfile/GenerateGAndGAdminCostProfile.cs</li>
                    <li>Method: RunCalculation()</li>
                    <li>Uses MapCountry() for country-specific cost factors</li>
                </ul>
            </Note>
        )}
    </Container>
)

export default GAndGAdmin

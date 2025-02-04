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

const WellIntervention: React.FC<Props> = ({ developerMode, hasOverride }) => (
    <Container>
        <Section>
            <Typography variant="h6">Well Intervention Cost Calculation</Typography>
            <Typography variant="body1" style={{ marginTop: 12, marginBottom: 24 }}>
                The well intervention cost (in MUSD) is calculated based on the cumulative number of wells and intervention cost per well:
            </Typography>

            <Formula>
                <MainFormula>
                    Well Intervention Cost = Cumulative Number of Wells × Annual Well Intervention Cost Per Well
                </MainFormula>

                <FormulaSection>
                    <h4>Cumulative Well Calculation:</h4>
                    <SubFormula>
                        Cumulative Wells[Year] = Sum of all wells drilled up to that year
                    </SubFormula>
                    <FormulaList>
                        <li>For each drilling schedule, sum up the wells drilled over time</li>
                        <li>Example: If drilling schedule is [1, 2, 3, 4], cumulative is [1, 3, 6, 10]</li>
                    </FormulaList>
                </FormulaSection>

                <FormulaSection>
                    <h4>Cost Components:</h4>
                    <FormulaList>
                        <li>Annual Well Intervention Cost Per Well from Project.DevelopmentOperationalWellCosts</li>
                        <li>Applied to the cumulative number of wells for each year</li>
                    </FormulaList>
                </FormulaSection>

                <FormulaSection>
                    <h4>Time Distribution:</h4>
                    <FormulaList>
                        <li>Starts from the first year of drilling</li>
                        <li>Continues until the last year of production</li>
                        <li>After all wells are drilled, the cost remains constant</li>
                    </FormulaList>
                </FormulaSection>

                <FormulaSection>
                    <h4>Special Cases:</h4>
                    <FormulaList>
                        <li>If no drilling schedules exist, the profile is reset to zero</li>
                        <li>Values are extended to match the last year of production if needed</li>
                        <li>The last calculated value is repeated for any additional years needed</li>
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
                    <li>backend/api/Features/Cases/Recalculation/Types/OpexCostProfile/OpexCostProfileService.cs</li>
                    <li>Method: CalculateWellInterventionCostProfile()</li>
                    <li>Uses GetCumulativeDrillingSchedule() for well count calculation</li>
                </ul>
            </Note>
        )}
    </Container>
)

export default WellIntervention

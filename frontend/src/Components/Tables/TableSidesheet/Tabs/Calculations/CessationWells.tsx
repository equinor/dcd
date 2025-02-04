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
} from "./shared.styles"

interface Props {
    developerMode: boolean
    hasOverride: boolean
}

const CessationWells: React.FC<Props> = ({ developerMode, hasOverride }) => (
    <Container>
        <Section>
            <Typography variant="h6">Cessation Wells Cost Calculation</Typography>
            <Typography variant="body1" style={{ marginTop: 12, marginBottom: 24 }}>
                The total cessation wells cost is calculated based on the number and types of wells:
            </Typography>

            <Formula>
                <MainFormula>
                    Total Cost = (Number of Wells × Cost per Well) + Additional Costs
                </MainFormula>

                <FormulaSection>
                    <h4>Cost Components:</h4>
                    <FormulaList>
                        <li>Number of Wells: Total count from all drilling schedules</li>
                        <li>Cost per Well: Based on well type and complexity</li>
                        <li>Additional Costs: Any extra costs specific to the cessation project</li>
                    </FormulaList>
                </FormulaSection>

                <FormulaSection>
                    <h4>Time Distribution:</h4>
                    <SubFormula>
                        Year 1: Total Cost / 2
                    </SubFormula>
                    <SubFormula>
                        Year 2: Total Cost / 2
                    </SubFormula>
                    <FormulaList>
                        <li>Cost is split equally over two years after the last year of production</li>
                    </FormulaList>
                </FormulaSection>

                <FormulaSection>
                    <h4>Special Cases:</h4>
                    <FormulaList>
                        <li>If no drilling schedules exist, the profile is reset to zero</li>
                        <li>If last year of production is not available, the profile is reset to zero</li>
                        <li>StartYear is set to the last year of production</li>
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
                    <li>backend/api/Features/Cases/Recalculation/Types/CessationCostProfile/CessationCostProfileService.cs</li>
                    <li>Method: CalculateCessationWellsCost()</li>
                    <li>Uses GetCumulativeDrillingSchedule() for well count calculation</li>
                </ul>
            </Note>
        )}
    </Container>
)

export default CessationWells

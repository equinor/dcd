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

const FeasibilityStudies: React.FC<Props> = ({ developerMode, hasOverride }) => (
    <Container>
        <Section>
            <Typography variant="h6">Feasibility & Conceptual Studies Cost Calculation</Typography>
            <Typography variant="body1" style={{ marginTop: 12, marginBottom: 24 }}>
                The total cost (in MUSD) is calculated and distributed over time:
            </Typography>

            <Formula>
                <MainFormula>
                    Total Cost = (Total Facility Cost + Total Well Cost) × CapexFactorFeasibilityStudies
                </MainFormula>

                <FormulaSection>
                    <h4>Total Facility Cost Components:</h4>
                    <FormulaList>
                        <li>Substructure Cost</li>
                        <li>Surf Cost</li>
                        <li>Topside Cost</li>
                        <li>Transport Cost</li>
                        <li>Onshore Power Supply Cost</li>
                    </FormulaList>
                </FormulaSection>

                <FormulaSection>
                    <h4>Total Well Cost Components:</h4>
                    <FormulaList>
                        <li>Oil Producer Cost</li>
                        <li>Gas Producer Cost</li>
                        <li>Water Injector Cost</li>
                        <li>Gas Injector Cost</li>
                    </FormulaList>
                </FormulaSection>

                <FormulaSection>
                    <h4>Time Distribution:</h4>
                    <SubFormula>
                        Year Cost[i] = Total Cost × (Days in Year[i] / Total Days in Study Period)
                    </SubFormula>
                    <FormulaList>
                        <li>Study period: From DG0 to DG2</li>
                        <li>Cost is distributed based on the number of days in each year</li>
                    </FormulaList>
                </FormulaSection>

                <FormulaSection>
                    <h4>Special Cases:</h4>
                    <FormulaList>
                        <li>If DG0 or DG2 year is 1, the profile is reset to zero</li>
                        <li>If DG2 falls on January 1st, it&apos;s treated as December 31st of previous year</li>
                        <li>StartYear is calculated as: DG0.Year - DG4.Year</li>
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
                    <li>backend/api/Features/Cases/Recalculation/Types/StudyCostProfile/StudyCostProfileService.cs</li>
                    <li>Method: CalculateTotalFeasibilityAndConceptStudies()</li>
                    <li>Uses SumAllCostFacility() and SumWellCost() for cost components</li>
                </ul>
            </Note>
        )}
    </Container>
)

export default FeasibilityStudies

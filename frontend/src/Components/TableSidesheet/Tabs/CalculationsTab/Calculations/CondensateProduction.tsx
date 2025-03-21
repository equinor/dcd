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

const CondensateProduction: React.FC<Props> = ({ developerMode, hasOverride }) => (
    <Container>
        <Section>
            <Typography variant="h6">Condensate Production Calculation</Typography>
            <Typography variant="body1" style={{ marginTop: 12, marginBottom: 24 }}>
                The condensate production (in million Sm³) is derived from the gas production profile and the condensate yield:
            </Typography>

            <Formula>
                <MainFormula>
                    Condensate Production = Gas Production × Condensate Yield ÷ 1,000
                </MainFormula>

                <FormulaSection>
                    <h4>Gas Production Components:</h4>
                    <FormulaList>
                        <li>Production Profile Gas</li>
                        <li>Additional Production Profile Gas</li>
                    </FormulaList>
                </FormulaSection>

                <FormulaSection>
                    <h4>Time Distribution:</h4>
                    <SubFormula>
                        Yearly Condensate Production[i] = Yearly Gas Production[i] × Condensate Yield ÷ 1,000
                    </SubFormula>
                    <FormulaList>
                        <li>Gas production is sourced from the merged profiles of production and additional production gas</li>
                        <li>Condensate yield is a given factor from the drainage strategy</li>
                    </FormulaList>
                </FormulaSection>

                <FormulaSection>
                    <h4>Special Cases:</h4>
                    <FormulaList>
                        <li>If gas production is null, the condensate production profile is not calculated</li>
                        <li>If condensate yield is zero, the profile will have all zero values</li>
                        <li>Start year is taken from the gas production profile</li>
                    </FormulaList>
                </FormulaSection>
            </Formula>

            {hasOverride && (
                <SpecialNote variant="body2" color="textSecondary">
                    Note: Since this is a manually overridden profile, these calculations are not used, and the values are manually set instead.
                </SpecialNote>
            )}
        </Section>

        {developerMode && (
            <Note>
                <strong>Developer Note:</strong>
                {" "}
                This calculation is implemented in:
                <ul>
                    <li>backend/api/Features/Profiles/CondensateProductionProfileService.cs</li>
                    <li>Method: RunCalculation()</li>
                    <li>Uses getGasProduction() to retrieve merged gas production data</li>
                </ul>
            </Note>
        )}
    </Container>
)

export default CondensateProduction

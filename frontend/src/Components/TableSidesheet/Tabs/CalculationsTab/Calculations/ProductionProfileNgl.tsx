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
    developerMode: boolean;
    hasOverride: boolean;
}

const NglProduction: React.FC<Props> = ({ developerMode, hasOverride }) => (
    <Container>
        <Section>
            <Typography variant="h6">NGL Production Calculation</Typography>
            <Typography variant="body1" style={{ marginTop: 12, marginBottom: 24 }}>
                The total NGL production (in MBOE) is calculated as follows:
            </Typography>

            <Formula>
                <MainFormula>
                    Total NGL Production = Gas Production × NGL Yield Factor
                </MainFormula>

                <FormulaSection>
                    <h4>Input Components:</h4>
                    <FormulaList>
                        <li>Gas Production</li>
                        <li>NGL Yield Factor</li>
                    </FormulaList>
                </FormulaSection>

                <FormulaSection>
                    <h4>Time Distribution:</h4>
                    <SubFormula>
                        Yearly NGL Production[i] = Total NGL Production × (Gas Production[i] / Total Gas Production)
                    </SubFormula>
                    <FormulaList>
                        <li>Production is distributed proportionally based on yearly gas production</li>
                        <li>If total gas production is zero, NGL production is set to zero</li>
                    </FormulaList>
                </FormulaSection>

                <FormulaSection>
                    <h4>Special Cases:</h4>
                    <FormulaList>
                        <li>If NGL Yield Factor is zero, the production is set to zero</li>
                        <li>If gas production is missing for any year, that year&apos;s NGL production is also set to zero</li>
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
                    <li>backend/api/Features/Cases/Recalculation/Types/NglProductionProfile/NglProductionService.cs</li>
                    <li>Method: CalculateTotalNglProduction()</li>
                    <li>Uses SumGasProduction() to determine total gas available</li>
                </ul>
            </Note>
        )}
    </Container>
)

export default NglProduction

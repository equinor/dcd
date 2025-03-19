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

const OffshoreFacilitiesOperations: React.FC<Props> = ({ developerMode, hasOverride }) => (
    <Container>
        <Section>
            <Typography variant="h6">Offshore Facilities Operations Cost Calculation</Typography>
            <Typography variant="body1" style={{ marginTop: 12, marginBottom: 24 }}>
                The offshore facilities operations cost (in MUSD) is calculated based on the facility OPEX and production timeline:
            </Typography>

            <Formula>
                <MainFormula>
                    Offshore Facilities Operations Cost = FacilityOpex (with ramp-up period)
                </MainFormula>

                <FormulaSection>
                    <h4>Pre-Production Ramp-up Period:</h4>
                    <SubFormula>
                        Year -3: (FacilityOpex - 1) / 8
                    </SubFormula>
                    <SubFormula>
                        Year -2: (FacilityOpex - 1) / 4
                    </SubFormula>
                    <SubFormula>
                        Year -1: (FacilityOpex - 1) / 2
                    </SubFormula>
                </FormulaSection>

                <FormulaSection>
                    <h4>Production Period:</h4>
                    <SubFormula>
                        All years: FacilityOpex (full amount)
                    </SubFormula>
                </FormulaSection>

                <FormulaSection>
                    <h4>Time Distribution:</h4>
                    <FormulaList>
                        <li>Starts 3 years before first year of production</li>
                        <li>Continues until last year of production</li>
                        <li>Uses ramped costs for the first three years</li>
                        <li>Uses full FacilityOpex during production years</li>
                    </FormulaList>
                </FormulaSection>

                <FormulaSection>
                    <h4>Special Cases:</h4>
                    <FormulaList>
                        <li>If FacilityOpex is 0 or negative, all values are set to 0</li>
                        <li>If first or last year of production is not available, the profile is reset to zero</li>
                        <li>StartYear is calculated as: FirstYearOfProduction - 3</li>
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
                    <li>Method: CalculateOffshoreFacilitiesOperationsCostProfile()</li>
                    <li>Uses CalculationHelper.GetRelativeFirstYearOfProduction() and GetRelativeLastYearOfProduction()</li>
                </ul>
            </Note>
        )}
    </Container>
)

export default OffshoreFacilitiesOperations

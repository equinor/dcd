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
} from "../../shared.styles"

interface Props {
    developerMode: boolean
    hasOverride: boolean
    wellType: "Oil Producer" | "Gas Producer" | "Water Injector" | "Gas Injector"
    wellCategory: "Oil_Producer" | "Gas_Producer" | "Water_Injector" | "Gas_Injector"
}

const WellBase: React.FC<Props> = ({
    developerMode, hasOverride, wellType, wellCategory,
}) => (
    <Container>
        <Section>
            <Typography variant="h6">
                {wellType}
                {" "}
                Well Cost Calculation
            </Typography>
            <Typography variant="body1" style={{ marginTop: 12, marginBottom: 24 }}>
                The
                {" "}
                {wellType.toLowerCase()}
                {" "}
                well cost (in MUSD) is calculated based on the drilling schedule and well cost:
            </Typography>

            <Formula>
                <MainFormula>
                    {wellType}
                    {" "}
                    Well Cost = Number of Wells × Well Cost
                </MainFormula>

                <FormulaSection>
                    <h4>Well Count Components:</h4>
                    <FormulaList>
                        <li>Taken from the drilling schedule for each year</li>
                        <li>
                            Only includes wells categorized as
                            {wellCategory}
                        </li>
                    </FormulaList>
                </FormulaSection>

                <FormulaSection>
                    <h4>Cost Components:</h4>
                    <FormulaList>
                        <li>Individual well cost from Well.WellCost</li>
                        <li>Applied to each well in the drilling schedule</li>
                    </FormulaList>
                </FormulaSection>

                <FormulaSection>
                    <h4>Time Distribution:</h4>
                    <FormulaList>
                        <li>Follows the drilling schedule timeline</li>
                        <li>Cost is incurred in the year the well is drilled</li>
                        <li>StartYear matches the drilling schedule&apos;s start year</li>
                    </FormulaList>
                </FormulaSection>

                <FormulaSection>
                    <h4>Calculation Process:</h4>
                    <FormulaList>
                        <li>
                            Filter WellProjectWells for
                            {wellCategory}
                            {" "}
                            category
                        </li>
                        <li>
                            For each well with a drilling schedule:
                            <ul>
                                <li>Multiply schedule values by well cost</li>
                                <li>Create time series with appropriate start year</li>
                            </ul>
                        </li>
                        <li>Merge all well time series into final profile</li>
                    </FormulaList>
                </FormulaSection>

                <FormulaSection>
                    <h4>Special Cases:</h4>
                    <FormulaList>
                        <li>If drilling schedule is empty, no costs are calculated</li>
                        <li>Multiple wells can be drilled in the same year</li>
                        <li>Costs are distributed according to actual drilling timeline</li>
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
                    <li>backend/api/Features/Cases/Recalculation/Types/WellCostProfile/WellProjectWellCostProfileService.cs</li>
                    <li>
                        Method: RunCalculation() with WellCategory.
                        {wellCategory}
                    </li>
                    <li>Uses TimeSeriesMerger for combining multiple well schedules</li>
                </ul>
            </Note>
        )}
    </Container>
)

export default WellBase

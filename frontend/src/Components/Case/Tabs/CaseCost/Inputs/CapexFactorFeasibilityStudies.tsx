import React from "react"
import SwitchableNumberInput from "@/Components/Input/SwitchableNumberInput"

interface props {
    caseData: Components.Schemas.CaseOverviewDto
}
const CapexFactorFeasibilityStudies: React.FC<props> = ({ caseData }) => (
    <SwitchableNumberInput
        resourceName="case"
        resourcePropertyKey="capexFactorFeasibilityStudies"
        previousResourceObject={caseData}
        label="CAPEX factor feasibility studies"
        value={caseData.capexFactorFeasibilityStudies}
        integer={false}
        unit="%"
        min={0}
        max={100}

    />
)

export default CapexFactorFeasibilityStudies

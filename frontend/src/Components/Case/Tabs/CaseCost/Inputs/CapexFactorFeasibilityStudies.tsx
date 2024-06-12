import React from "react"
import SwitchableNumberInput from "../../../../Input/SwitchableNumberInput"

interface props {
    caseData: Components.Schemas.CaseDto
}
const CapexFactorFeasibilityStudies: React.FC<props> = ({ caseData }) => (
    <SwitchableNumberInput
        resourceName="case"
        resourcePropertyKey="capexFactorFeasibilityStudies"
        label="CAPEX factor feasibility studies"
        value={caseData.capexFactorFeasibilityStudies}
        integer={false}
        unit="%"
        min={0}
        max={100}
    />
)

export default CapexFactorFeasibilityStudies

import React from "react"
import SwitchableNumberInput from "../../../../Input/SwitchableNumberInput"

interface props {
    caseData: Components.Schemas.CaseOverviewDto
    addEdit: any
}
const CapexFactorFeasibilityStudies: React.FC<props> = ({ caseData, addEdit }) => (
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
        addEdit={addEdit}
    />
)

export default CapexFactorFeasibilityStudies

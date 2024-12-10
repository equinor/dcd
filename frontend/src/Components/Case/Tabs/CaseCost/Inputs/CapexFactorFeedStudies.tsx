import React from "react"
import SwitchableNumberInput from "../../../../Input/SwitchableNumberInput"

interface props {
    caseData: Components.Schemas.CaseOverviewDto
    addEdit: any
}
const CapexFactorFeedStudies: React.FC<props> = ({ caseData, addEdit }) => (
    <SwitchableNumberInput
        resourceName="case"
        resourcePropertyKey="capexFactorFEEDStudies"
        label="CAPEX factor FEED studies"
        previousResourceObject={caseData}
        value={caseData.capexFactorFEEDStudies}
        integer={false}
        unit="%"
        min={0}
        max={100}
        addEdit={addEdit}
    />
)

export default CapexFactorFeedStudies

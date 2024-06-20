import React from "react"
import SwitchableNumberInput from "../../../../Input/SwitchableNumberInput"

interface props {
    caseData: Components.Schemas.CaseDto
}
const CapexFactorFeedStudies: React.FC<props> = ({ caseData }) => (
    <SwitchableNumberInput
        resourceName="case"
        resourcePropertyKey="capexFactorFEEDStudies"
        label="CAPEX factor FEED studies"
        value={caseData.capexFactorFEEDStudies}
        integer={false}
        unit="%"
        min={0}
        max={100}
    />
)

export default CapexFactorFeedStudies

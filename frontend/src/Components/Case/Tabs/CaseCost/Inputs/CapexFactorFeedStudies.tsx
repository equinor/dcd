import React from "react"
import SwitchableNumberInput from "@/Components/Input/SwitchableNumberInput"

interface props {
    caseData: Components.Schemas.CaseOverviewDto
}
const CapexFactorFeedStudies: React.FC<props> = ({ caseData }) => (
    <SwitchableNumberInput
        resourceName="case"
        resourcePropertyKey="capexFactorFeedStudies"
        label="CAPEX factor FEED studies"
        previousResourceObject={caseData}
        value={caseData.capexFactorFeedStudies}
        integer={false}
        unit="%"
        min={0}
        max={100}
    />
)

export default CapexFactorFeedStudies

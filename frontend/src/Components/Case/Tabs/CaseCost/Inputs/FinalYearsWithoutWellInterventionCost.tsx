import React from "react"
import SwitchableDropdownInput from "@/Components/Input/SwitchableDropdownInput"

interface Props {
    caseData: Components.Schemas.CaseOverviewDto
}

const FinalYearsWithoutWellInterventionCost: React.FC<Props> = ({ caseData }) => (
    <SwitchableDropdownInput
        resourceName="case"
        resourcePropertyKey="finalYearsWithoutWellInterventionCost"
        label="Final Years Without Well Intervention Cost"
        previousResourceObject={caseData}
        value={caseData.finalYearsWithoutWellInterventionCost}
        options={{
            0: "0 years",
            1: "1 year",
            2: "2 years",
            3: "3 years",
        }}
    />
)

export default FinalYearsWithoutWellInterventionCost

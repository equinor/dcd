import React from "react"

import SwitchableDropdownInput from "@/Components/Input/SwitchableDropdownInput"
import { useCaseMutation } from "@/Hooks/Mutations"

interface Props {
    caseData: Components.Schemas.CaseOverviewDto
}

const InitialYearsWithoutWellInterventionCost: React.FC<Props> = ({ caseData }) => {
    const { updateInitialYearsWithoutWellInterventionCost } = useCaseMutation()

    return (
        <SwitchableDropdownInput
            label="Initial Years Without Well Intervention Cost"
            value={caseData.initialYearsWithoutWellInterventionCost}
            id={`case-initial-years-without-well-intervention-cost-${caseData.caseId}`}
            options={{
                0: "0 years",
                1: "1 year",
                2: "2 years",
                3: "3 years",
            }}
            onSubmit={(newValue) => updateInitialYearsWithoutWellInterventionCost(newValue)}
        />
    )
}

export default InitialYearsWithoutWellInterventionCost

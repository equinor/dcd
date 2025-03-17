import React from "react"
import SwitchableDropdownInput from "@/Components/Input/SwitchableDropdownInput"
import { useCaseMutation } from "@/Hooks/Mutations"

interface Props {
    caseData: Components.Schemas.CaseOverviewDto
}

const FinalYearsWithoutWellInterventionCost: React.FC<Props> = ({ caseData }) => {
    const { updateFinalYearsWithoutWellInterventionCost } = useCaseMutation()

    return (
        <SwitchableDropdownInput
            label="Final Years Without Well Intervention Cost"
            value={caseData.finalYearsWithoutWellInterventionCost}
            id={`case-final-years-without-well-intervention-cost-${caseData.caseId}`}
            options={{
                0: "0 years",
                1: "1 year",
                2: "2 years",
                3: "3 years",
            }}
            onSubmit={(newValue) => updateFinalYearsWithoutWellInterventionCost(newValue)}
        />
    )
}

export default FinalYearsWithoutWellInterventionCost

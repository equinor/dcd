import React from "react"

import SwitchableNumberInput from "@/Components/Input/SwitchableNumberInput"
import { useCaseMutation } from "@/Hooks/Mutations"

interface Props {
    caseData: Components.Schemas.CaseOverviewDto
}

const CapexFactorFeasibilityStudies: React.FC<Props> = ({ caseData }) => {
    const { updateCapexFactorFeasibilityStudies } = useCaseMutation()

    return (
        <SwitchableNumberInput
            label="CAPEX factor feasibility studies"
            value={caseData.capexFactorFeasibilityStudies}
            id={`case-capex-factor-feasibility-studies-${caseData.caseId}`}
            integer={false}
            unit="%"
            min={0}
            max={100}
            onSubmit={(newValue) => updateCapexFactorFeasibilityStudies(newValue)}
        />
    )
}

export default CapexFactorFeasibilityStudies

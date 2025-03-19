import React from "react"

import SwitchableNumberInput from "@/Components/Input/SwitchableNumberInput"
import { useCaseMutation } from "@/Hooks/Mutations"

interface Props {
    caseData: Components.Schemas.CaseOverviewDto
}

const CapexFactorFeedStudies: React.FC<Props> = ({ caseData }) => {
    const { updateCapexFactorFeedStudies } = useCaseMutation()

    return (
        <SwitchableNumberInput
            label="CAPEX factor FEED studies"
            value={caseData.capexFactorFeedStudies}
            id={`case-capex-factor-feed-studies-${caseData.caseId}`}
            integer={false}
            unit="%"
            min={0}
            max={100}
            onSubmit={(newValue) => updateCapexFactorFeedStudies(newValue)}
        />
    )
}

export default CapexFactorFeedStudies

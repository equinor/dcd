import React from "react"
import SwitchableNumberInput from "../../../../Input/SwitchableNumberInput"
import { useCaseContext } from "../../../../../Context/CaseContext"

interface props {
    updateCase: (key: any, value: any) => void;
}

const CapexFactorFeasibilityStudies: React.FC<props> = ({ updateCase }) => {
    const {
        projectCase,
        projectCaseEdited,
        setProjectCaseEdited,
    } = useCaseContext()

    const handleCaseFeasibilityChange = (value: number): void => {
        const newCase = { ...projectCaseEdited }
        const newCapexFactorFeasibilityStudies = value > 0
            ? Math.min(Math.max(value, 0), 100) : undefined
        if (newCapexFactorFeasibilityStudies !== undefined) {
            newCase.capexFactorFeasibilityStudies = newCapexFactorFeasibilityStudies / 100
        } else { newCase.capexFactorFeasibilityStudies = 0 }
        setProjectCaseEdited(newCase as Components.Schemas.CaseDto)
        updateCase("capexFactorFeasibilityStudies", newCase.capexFactorFeasibilityStudies)
    }
    return (
        <SwitchableNumberInput
            objectKey={projectCase?.capexFactorFeasibilityStudies}
            label="CAPEX factor feasibility studies"
            onSubmit={handleCaseFeasibilityChange}
            value={projectCase?.capexFactorFeasibilityStudies !== undefined ? (projectCase.capexFactorFeasibilityStudies ?? 0) * 100 : undefined}
            integer={false}
            unit="%"
            min={0}
            max={100}
        />
    )
}

export default CapexFactorFeasibilityStudies

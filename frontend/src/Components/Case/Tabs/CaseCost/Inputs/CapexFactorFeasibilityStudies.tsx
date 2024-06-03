import React from "react"
import SwitchableNumberInput from "../../../../Input/SwitchableNumberInput"
import { useCaseContext } from "../../../../../Context/CaseContext"

const CapexFactorFeasibilityStudies: React.FC = () => {
    const {
        projectCase,
        projectCaseEdited,
        setProjectCaseEdited,
    } = useCaseContext()

    // todo: the value is manipulated before submition. find out how to handle that with the service implementation
    const handleCaseFeasibilityChange = (value: number): void => {
        const newCase = { ...projectCaseEdited }
        const newCapexFactorFeasibilityStudies = value > 0
            ? Math.min(Math.max(value, 0), 100) : undefined
        if (newCapexFactorFeasibilityStudies !== undefined) {
            newCase.capexFactorFeasibilityStudies = newCapexFactorFeasibilityStudies / 100
        } else { newCase.capexFactorFeasibilityStudies = 0 }
        setProjectCaseEdited(newCase as Components.Schemas.CaseDto)
    }
    return (
        <SwitchableNumberInput
            serviceName="case"
            serviceKey="capexFactorFeasibilityStudies"
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

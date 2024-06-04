import React from "react"
import SwitchableNumberInput from "../../../../Input/SwitchableNumberInput"
import { useCaseContext } from "../../../../../Context/CaseContext"

const CapexFactorFeedStudies: React.FC = () => {
    const {
        projectCase,
        projectCaseEdited,
        setProjectCaseEdited,
    } = useCaseContext()

    if (!projectCase) { return null }

    const handleCaseFEEDChange = (value: number): void => {
        const newCase = { ...projectCaseEdited }
        const newCapexFactorFEEDStudies = value > 0
            ? Math.min(Math.max(value, 0), 100) : undefined
        if (newCapexFactorFEEDStudies !== undefined) {
            newCase.capexFactorFEEDStudies = newCapexFactorFEEDStudies / 100
        } else { newCase.capexFactorFEEDStudies = 0 }
        setProjectCaseEdited(newCase as Components.Schemas.CaseDto)
    }

    return (
        <SwitchableNumberInput
            resourceName="case"
            resourcePropertyKey="capexFactorFEEDStudies"
            label="CAPEX factor FEED studies"
            onSubmit={handleCaseFEEDChange}
            value={projectCase?.capexFactorFEEDStudies !== undefined ? (projectCase.capexFactorFEEDStudies ?? 0) * 100 : undefined}
            integer={false}
            unit="%"
            min={0}
            max={100}
        />
    )
}

export default CapexFactorFeedStudies

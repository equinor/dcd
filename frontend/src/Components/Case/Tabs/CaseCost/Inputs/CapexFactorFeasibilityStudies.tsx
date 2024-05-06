import React, { ChangeEventHandler } from "react"
import InputSwitcher from "../../../../Input/InputSwitcher"
import CaseNumberInput from "../../../../Input/CaseNumberInput"
import { useCaseContext } from "../../../../../Context/CaseContext"

const CapexFactorFeasibilityStudies: React.FC = () => {
    const {
        projectCase,
        projectCaseEdited,
        setProjectCaseEdited,
    } = useCaseContext()

    const handleCaseFeasibilityChange: ChangeEventHandler<HTMLInputElement> = async (e) => {
        const newCase = { ...projectCaseEdited }
        const newCapexFactorFeasibilityStudies = e.currentTarget.value.length > 0
            ? Math.min(Math.max(Number(e.currentTarget.value), 0), 100) : undefined
        if (newCapexFactorFeasibilityStudies !== undefined) {
            newCase.capexFactorFeasibilityStudies = newCapexFactorFeasibilityStudies / 100
        } else { newCase.capexFactorFeasibilityStudies = 0 }
        setProjectCaseEdited(newCase as Components.Schemas.CaseDto)
    }
    return (
        <InputSwitcher
            value={`${projectCase?.capexFactorFeasibilityStudies !== undefined ? ((projectCase.capexFactorFeasibilityStudies ?? 0) * 100).toFixed(2) : ""}%`}
            label="CAPEX factor feasibility studies"
        >
            <CaseNumberInput
                onChange={handleCaseFeasibilityChange}
                defaultValue={projectCase?.capexFactorFeasibilityStudies !== undefined ? (projectCase.capexFactorFeasibilityStudies ?? 0) * 100 : undefined}
                integer={false}
                unit="%"
                min={0}
                max={100}
            />
        </InputSwitcher>
    )
}

export default CapexFactorFeasibilityStudies

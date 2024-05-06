import React, { ChangeEventHandler } from "react"
import InputSwitcher from "../../../../Input/InputSwitcher"
import CaseNumberInput from "../../../../Input/CaseNumberInput"
import { useCaseContext } from "../../../../../Context/CaseContext"

const TotalStudyCosts: React.FC = () => {
    const {
        projectCase,
        projectCaseEdited,
        setProjectCaseEdited,
    } = useCaseContext()

    const handleCaseFEEDChange: ChangeEventHandler<HTMLInputElement> = async (e) => {
        const newCase = { ...projectCaseEdited }
        const newCapexFactorFEEDStudies = e.currentTarget.value.length > 0
            ? Math.min(Math.max(Number(e.currentTarget.value), 0), 100) : undefined
        if (newCapexFactorFEEDStudies !== undefined) {
            newCase.capexFactorFEEDStudies = newCapexFactorFEEDStudies / 100
        } else { newCase.capexFactorFEEDStudies = 0 }
        setProjectCaseEdited(newCase as Components.Schemas.CaseDto)
    }

    return (
        <InputSwitcher
            value={`${projectCase?.capexFactorFEEDStudies !== undefined ? ((projectCase.capexFactorFEEDStudies ?? 0) * 100).toFixed(2) : ""}%`}
            label="CAPEX factor FEED studies"
        >
            <CaseNumberInput
                onChange={handleCaseFEEDChange}
                defaultValue={projectCase?.capexFactorFEEDStudies !== undefined ? (projectCase.capexFactorFEEDStudies ?? 0) * 100 : undefined}
                integer={false}
                unit="%"
                min={0}
                max={100}
            />
        </InputSwitcher>
    )
}

export default TotalStudyCosts

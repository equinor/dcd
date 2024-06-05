import React from "react"
import NumberInputWithValidation from "./Components/NumberInputWithValidation"
import InputSwitcher from "./Components/InputSwitcher"
import useDataEdits from "../../Hooks/useDataEdits"
import { useCaseContext } from "../../Context/CaseContext"
import { useProjectContext } from "../../Context/ProjectContext"
import { ResourcePropertyKey, ResourceName } from "../../Models/Interfaces"

interface CaseEditInputProps {
    label: string;
    onSubmit?: (value: number) => void;
    value: number | undefined;
    resourceName: ResourceName;
    resourcePropertyKey: ResourcePropertyKey;
    resourceId?: string;
    integer: boolean;
    disabled?: boolean;
    unit?: string;
    allowNegative?: boolean;
    min?: number;
    max?: number;
}

const SwitchableNumberInput: React.FC<CaseEditInputProps> = ({
    label,
    onSubmit, // this will be obsolete when we introduce autosave.
    value,
    resourcePropertyKey,
    resourceName,
    resourceId,
    integer,
    disabled,
    unit,
    allowNegative,
    min,
    max,
}: CaseEditInputProps) => {
    const { addEdit } = useDataEdits()
    const { projectCase } = useCaseContext()
    const { project } = useProjectContext()

    const addToEditsAndSubmit = (insertedValue: number) => {
        if (onSubmit) {
            onSubmit(insertedValue) // this will be obsolete when we introduce autosave.
        }
        if (!projectCase || !project) { return }

        console.log("newValue: ", insertedValue)
        console.log("typeof newValue: ", typeof insertedValue)
        console.log("previousValue: ", value)
        console.log("inputLabel: ", label)
        console.log("projectId: ", project.id)
        console.log("resourceName: ", resourceName)
        console.log("resourcePropertyKey: ", resourcePropertyKey)
        console.log("resourceId: ", resourceId)
        console.log("caseId: ", projectCase.id)

        addEdit({
            newValue: insertedValue,
            previousValue: value,
            inputLabel: label,
            projectId: project.id,
            resourceName,
            resourcePropertyKey,
            resourceId,
            caseId: projectCase.id,
        })
    }

    return (
        <InputSwitcher
            label={label}
            value={`${value}`}
        >
            <NumberInputWithValidation
                onSubmit={addToEditsAndSubmit}
                defaultValue={value}
                integer={integer}
                disabled={disabled}
                unit={unit}
                allowNegative={allowNegative}
                min={min}
                max={max}
            />
        </InputSwitcher>
    )
}

export default SwitchableNumberInput

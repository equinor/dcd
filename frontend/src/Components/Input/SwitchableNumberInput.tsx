import React from "react"
import { useParams } from "react-router-dom"
import NumberInputWithValidation from "./Components/NumberInputWithValidation"
import InputSwitcher from "./Components/InputSwitcher"
import useDataEdits from "../../Hooks/useDataEdits"
import { useProjectContext } from "../../Context/ProjectContext"
import { ResourcePropertyKey, ResourceName, ResourceObject } from "../../Models/Interfaces"

interface CaseEditInputProps {
    label: string;
    onSubmit?: (value: number) => void;
    value: number | undefined;
    resourceName: ResourceName;
    resourcePropertyKey: ResourcePropertyKey;
    previousResourceObject: ResourceObject;
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
    previousResourceObject,
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
    const { project } = useProjectContext()
    const { caseId } = useParams()

    const addToEditsAndSubmit = (insertedValue: number) => {
        if (onSubmit) {
            onSubmit(insertedValue) // this will be obsolete when we introduce autosave.
        }
        if (!caseId || !project) { return }

        const newResourceObject: ResourceObject = structuredClone(previousResourceObject)
        newResourceObject[resourcePropertyKey as keyof ResourceObject] = String(insertedValue)

        addEdit({
            previousDisplayValue: value,
            newDisplayValue: insertedValue,
            newResourceObject,
            previousResourceObject,
            inputLabel: label,
            projectId: project.id,
            resourceName,
            resourcePropertyKey,
            resourceId,
            caseId,
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

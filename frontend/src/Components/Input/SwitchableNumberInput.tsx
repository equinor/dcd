import React from "react"
import { useParams } from "react-router-dom"

import { ResourcePropertyKey, ResourceName, ResourceObject } from "@/Models/Interfaces"
import { useProjectContext } from "@/Store/ProjectContext"
import InputSwitcher from "./Components/InputSwitcher"
import useEditCase from "@/Hooks/useEditCase"
import NumberInputWithValidation from "./Components/NumberInputWithValidation"

interface CaseEditInputProps {
    label: string;
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
    const { caseId } = useParams()
    const { projectId } = useProjectContext()
    const { addEdit } = useEditCase()

    const addToEditsAndSubmit = (insertedValue: number) => {
        if (!caseId || projectId === "") { return }

        const resourceObject: ResourceObject = structuredClone(previousResourceObject)
        resourceObject[resourcePropertyKey as keyof ResourceObject] = insertedValue as never

        addEdit({
            projectId,
            resourceName,
            resourcePropertyKey,
            resourceId,
            caseId,
            resourceObject,
        })
    }

    return (
        <InputSwitcher
            label={label}
            value={`${value ?? ""} ${unit ?? ""}`}
        >
            <NumberInputWithValidation
                label={label}
                id={`${resourceName}-${resourcePropertyKey}-${resourceId ?? ""}`}
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

import React from "react"
import { useParams } from "react-router-dom"
import { useModuleCurrentContext } from "@equinor/fusion-framework-react-module-context"
import NumberInputWithValidation from "./Components/NumberInputWithValidation"
import InputSwitcher from "./Components/InputSwitcher"
import { ResourcePropertyKey, ResourceName, ResourceObject } from "../../Models/Interfaces"
import { useProjectContext } from "../../Context/ProjectContext"

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
    addEdit: any;
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
    addEdit,
}: CaseEditInputProps) => {
    const { caseId, tab } = useParams()
    const { projectId } = useProjectContext()
    const addToEditsAndSubmit = (insertedValue: number) => {
        if (!caseId || projectId === "") { return }

        const newResourceObject: ResourceObject = structuredClone(previousResourceObject)
        newResourceObject[resourcePropertyKey as keyof ResourceObject] = insertedValue as never

        addEdit({
            previousDisplayValue: value,
            newDisplayValue: insertedValue,
            newResourceObject,
            previousResourceObject,
            inputLabel: label,
            projectId,
            resourceName,
            resourcePropertyKey,
            resourceId,
            caseId,
            tabName: tab,
            inputFieldId: `${resourceName}-${resourcePropertyKey}-${resourceId}`,
        })
    }

    return (
        <InputSwitcher
            label={label}
            value={`${value}`}
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

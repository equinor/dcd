import React, { ChangeEventHandler, useState } from "react"
import { NativeSelect } from "@equinor/eds-core-react"
import { useParams } from "react-router-dom"
import InputSwitcher from "./Components/InputSwitcher"
import { ResourcePropertyKey, ResourceName, ResourceObject } from "@/Models/Interfaces"
import { useProjectContext } from "@/Store/ProjectContext"

interface SwitchableDropdownInputProps {
    value: string | number;
    options: { [key: string]: string };
    resourceName: ResourceName;
    resourcePropertyKey: ResourcePropertyKey;
    previousResourceObject: ResourceObject
    resourceId?: string;
    label: string;
    addEdit: any;
}

const SwitchableDropdownInput: React.FC<SwitchableDropdownInputProps> = ({
    value,
    options,
    resourceName,
    resourcePropertyKey,
    previousResourceObject,
    resourceId,
    label,
    addEdit,
}: SwitchableDropdownInputProps) => {
    const { caseId, tab } = useParams()
    const { projectId } = useProjectContext()
    const [localValue, setLocalValue] = useState(value)

    const addToEditsAndSubmit: ChangeEventHandler<HTMLSelectElement> = async (e) => {
        if (!caseId || !projectId) { return }

        const newValue = e.currentTarget.value
        setLocalValue(Number(newValue))

        const newResourceObject: any = structuredClone(previousResourceObject)
        newResourceObject[resourcePropertyKey] = Number(newValue)

        addEdit({
            newResourceObject,
            previousResourceObject,
            inputLabel: label,
            projectId,
            resourceName,
            resourcePropertyKey,
            resourceId,
            caseId,
            newDisplayValue: options[newValue],
            previousDisplayValue: options[value],
            tabName: tab,
            inputFieldId: `${resourceName}-${resourcePropertyKey}-${resourceId}`,
        })
    }

    return (
        <InputSwitcher
            value={options[localValue]}
            label={label}
        >
            <NativeSelect
                id={`${resourceName}-${resourcePropertyKey}-${resourceId ?? ""}`}
                label=""
                value={localValue}
                onChange={addToEditsAndSubmit}
            >
                {Object.entries(options).map(([key, val]) => (
                    <option key={key} value={key}>{val}</option>
                ))}
            </NativeSelect>
        </InputSwitcher>
    )
}

export default SwitchableDropdownInput

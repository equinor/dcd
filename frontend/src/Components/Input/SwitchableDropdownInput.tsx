import React, { ChangeEventHandler, useState } from "react"
import { NativeSelect } from "@equinor/eds-core-react"
import { useParams } from "react-router-dom"
import InputSwitcher from "./Components/InputSwitcher"
import { ResourcePropertyKey, ResourceName, ResourceObject } from "@/Models/Interfaces"
import { useProjectContext } from "@/Store/ProjectContext"
import useEditCase from "@/Hooks/useEditCase"

interface SwitchableDropdownInputProps {
    value: string | number;
    options: { [key: string]: string };
    resourceName: ResourceName;
    resourcePropertyKey: ResourcePropertyKey;
    previousResourceObject: ResourceObject
    resourceId?: string;
    label: string;
}

const SwitchableDropdownInput: React.FC<SwitchableDropdownInputProps> = ({
    value,
    options,
    resourceName,
    resourcePropertyKey,
    previousResourceObject,
    resourceId,
    label,
}: SwitchableDropdownInputProps) => {
    const { caseId } = useParams()
    const { projectId } = useProjectContext()
    const [localValue, setLocalValue] = useState(value)
    const { addEdit } = useEditCase()

    const addToEditsAndSubmit: ChangeEventHandler<HTMLSelectElement> = async (e) => {
        if (!caseId || !projectId) { return }

        const newValue = e.currentTarget.value
        setLocalValue(Number(newValue))

        const resourceObject: any = structuredClone(previousResourceObject)
        resourceObject[resourcePropertyKey] = Number(newValue)

        addEdit({
            resourceObject,
            projectId,
            resourceName,
            resourcePropertyKey,
            resourceId,
            caseId,
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

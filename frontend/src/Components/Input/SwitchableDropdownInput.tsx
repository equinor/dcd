import React, { ChangeEventHandler } from "react"
import { NativeSelect } from "@equinor/eds-core-react"
import { useParams } from "react-router-dom"
import { useModuleCurrentContext } from "@equinor/fusion-framework-react-module-context"
import InputSwitcher from "./Components/InputSwitcher"
import { ResourcePropertyKey, ResourceName, ResourceObject } from "../../Models/Interfaces"

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

    const { currentContext } = useModuleCurrentContext()
    const projectId = currentContext?.externalId

    const addToEditsAndSubmit: ChangeEventHandler<HTMLSelectElement> = async (e) => {
        if (!caseId || !projectId) { return }

        const newResourceObject: any = structuredClone(previousResourceObject)
        newResourceObject[resourcePropertyKey] = Number(e.currentTarget.value)

        addEdit({
            newResourceObject,
            previousResourceObject,
            inputLabel: label,
            projectId,
            resourceName,
            resourcePropertyKey,
            resourceId,
            caseId,
            newDisplayValue: options[e.currentTarget.value],
            previousDisplayValue: options[value],
            tabName: tab,
            inputFieldId: `${resourceName}-${resourcePropertyKey}-${resourceId}`,
        })
    }

    return (
        <InputSwitcher
            value={options[value]}
            label={label}
        >
            <NativeSelect
                id={`${resourceName}-${resourcePropertyKey}-${resourceId ?? ""}`}
                label=""
                value={value}
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

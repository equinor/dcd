import React, { ChangeEventHandler } from "react"
import { NativeSelect } from "@equinor/eds-core-react"
import InputSwitcher from "./Components/InputSwitcher"
import useDataEdits from "../../Hooks/useDataEdits"
import { useCaseContext } from "../../Context/CaseContext"
import { useProjectContext } from "../../Context/ProjectContext"
import { ResourcePropertyKey, ResourceName } from "../../Models/Interfaces"

interface SwitchableDropdownInputProps {
    value: string | number;
    options: { [key: string]: string };
    resourceName: ResourceName;
    resourcePropertyKey: ResourcePropertyKey;
    resourceId?: string;
    label: string;
    onSubmit?: ChangeEventHandler<HTMLSelectElement>
}

const SwitchableDropdownInput: React.FC<SwitchableDropdownInputProps> = ({
    value,
    options,
    resourceName,
    resourcePropertyKey,
    resourceId,
    label,
    onSubmit,
}: SwitchableDropdownInputProps) => {
    const { projectCase } = useCaseContext()
    const { project } = useProjectContext()
    const { addEdit } = useDataEdits()

    const addToEditsAndSubmit: ChangeEventHandler<HTMLSelectElement> = async (e) => {
        if (!projectCase || !project) { return }
        if (onSubmit) { onSubmit(e) }

        addEdit({
            newValue: Number(e.currentTarget.value),
            previousValue: value,
            inputLabel: label,
            projectId: project.id,
            resourceName,
            resourcePropertyKey,
            resourceId,
            caseId: projectCase.id,
            newDisplayValue: options[e.currentTarget.value],
            previousDisplayValue: options[value],
        })
    }

    return (
        <InputSwitcher
            value={options[value]}
            label={label}
        >
            <NativeSelect
                label=""
                id={label}
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

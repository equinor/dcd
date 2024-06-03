import React, { ChangeEventHandler } from "react"
import { NativeSelect } from "@equinor/eds-core-react"
import InputSwitcher from "./Components/InputSwitcher"
import useDataEdits from "../../Hooks/useDataEdits"
import { useCaseContext } from "../../Context/CaseContext"
import { useProjectContext } from "../../Context/ProjectContext"
import { ServiceKey, ServiceName } from "../../Models/Interfaces"

interface SwitchableDropdownInputProps {
    value: string | number;
    options: { [key: string]: string };
    serviceName: ServiceName;
    serviceKey: ServiceKey;
    serviceId?: string;
    label: string;
    onSubmit: ChangeEventHandler<HTMLSelectElement>
}

const SwitchableDropdownInput: React.FC<SwitchableDropdownInputProps> = ({
    value,
    options,
    serviceName,
    serviceKey,
    serviceId,
    label,
    onSubmit,
}: SwitchableDropdownInputProps) => {
    const { projectCase } = useCaseContext()
    const { project } = useProjectContext()

    if (!projectCase || !project) { return null }

    const { addEdit } = useDataEdits()

    const addToEditsAndSubmit: ChangeEventHandler<HTMLSelectElement> = async (e) => {
        onSubmit(e)

        addEdit(
            e.currentTarget.value, // newValue
            value, // previousValue
            label, // inputLabel
            project.id, // projectId
            serviceName, // serviceName
            serviceKey, // serviceKey
            serviceId, // serviceId
            projectCase.id, // caseId
            options[e.currentTarget.value], // newDisplayValue
            options[value], // previousDisplayValue
        )
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

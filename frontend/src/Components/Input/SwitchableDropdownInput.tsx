import React, { ChangeEventHandler } from "react"
import { NativeSelect } from "@equinor/eds-core-react"
import InputSwitcher from "./Components/InputSwitcher"
import useDataEdits from "../../Hooks/useDataEdits"
import { useCaseContext } from "../../Context/CaseContext"
import { useProjectContext } from "../../Context/ProjectContext"

interface SwitchableDropdownInputProps {
    value: string | number;
    options: { [key: string]: string };
    objectKey: number;
    label: string;
    onSubmit: ChangeEventHandler<HTMLSelectElement>
}

const SwitchableDropdownInput: React.FC<SwitchableDropdownInputProps> = ({
    value,
    options,
    objectKey,
    label,
    onSubmit,
}: SwitchableDropdownInputProps) => {
    const { projectCase } = useCaseContext()
    const { project } = useProjectContext()

    const { addEdit } = useDataEdits(project!.id, projectCase!.id)

    const addToEditsAndSubmit: ChangeEventHandler<HTMLSelectElement> = async (e) => {
        if (!projectCase) {
            console.log("Case not found")
            return
        }

        const newValue = e.currentTarget.value
        const level = "case"
        const objectId = projectCase?.id

        onSubmit(e)
        addEdit(
            newValue,
            value,
            objectKey,
            label,
            level,
            objectId,
            options[newValue],
            options[value],
        )
    }

    return (
        <InputSwitcher
            value={options[objectKey]}
            label={label}
        >
            <NativeSelect
                label=""
                id={label}
                value={objectKey}
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

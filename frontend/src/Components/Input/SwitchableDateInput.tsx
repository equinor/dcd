import React, { ChangeEventHandler } from "react"
import { Input } from "@equinor/eds-core-react"
import InputSwitcher from "../Input/Components/InputSwitcher"
import { formatDate } from "../../Utils/common"
import useDataEdits from "../../Hooks/useDataEdits"
import { useCaseContext } from "../../Context/CaseContext"
import { useProjectContext } from "../../Context/ProjectContext"

interface SwitchableDateInputProps {
    objectKey: string
    value: string | undefined
    label: string
    onChange: ChangeEventHandler<HTMLInputElement>
    min?: string
    max?: string
}
const SwitchableDateInput: React.FC<SwitchableDateInputProps> = ({
    value,
    objectKey,
    label,
    onChange,
    min,
    max,
}) => {
    const { projectCase } = useCaseContext()
    const { project } = useProjectContext()

    if (!projectCase || !project) { return null }

    const { addEdit } = useDataEdits(project!.id, projectCase!.id)

    const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        onChange(e)

        addEdit(
            e.target.value,
            value,
            objectKey,
            label,
            "case",
            projectCase.id,
            formatDate(e.target.value),
            value && formatDate(value),
        )
    }
    return (
        <InputSwitcher
            value={formatDate(objectKey)}
            label={label}
        >
            <Input
                type="month"
                id="dgaDate"
                name="dgaDate"
                onChange={handleChange}
                value={value}
                min={min}
                max={max}
            />
        </InputSwitcher>
    )
}

export default SwitchableDateInput

import React, { ChangeEventHandler } from "react"
import { Input } from "@equinor/eds-core-react"
import InputSwitcher from "../Input/Components/InputSwitcher"
import { formatDate } from "../../Utils/common"
import useDataEdits from "../../Hooks/useDataEdits"
import { useCaseContext } from "../../Context/CaseContext"

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
    const { addEdit } = useDataEdits()
    const { projectCase } = useCaseContext()

    const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        if (!projectCase) { return }

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
            value={value ? formatDate(value) : ""}
            label={label}
        >
            <Input
                type="month"
                id={objectKey}
                name={objectKey}
                onChange={handleChange}
                value={value}
                min={min}
                max={max}
            />
        </InputSwitcher>
    )
}

export default SwitchableDateInput

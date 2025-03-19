import { TextField } from "@equinor/eds-core-react"
import React, { ChangeEventHandler, useState } from "react"

import InputSwitcher from "./Components/InputSwitcher"

interface SwitchableStringInputProps {
    value: string;
    label: string;
    id?: string;
    onSubmit: (newValue: string) => void;
    disabled?: boolean;
}

const SwitchableStringInput: React.FC<SwitchableStringInputProps> = ({
    value,
    label,
    id,
    onSubmit,
    disabled = false,
}: SwitchableStringInputProps) => {
    const [localValue, setLocalValue] = useState(value)

    const handleChange: ChangeEventHandler<HTMLInputElement> = (e) => {
        setLocalValue(e.target.value)
    }

    const handleBlur = () => {
        if (localValue !== value) {
            onSubmit(localValue)
        }
    }

    const handleKeyDown = (e: React.KeyboardEvent<HTMLInputElement>) => {
        if (e.key === "Enter") {
            e.currentTarget.blur()
        }
    }

    return (
        <InputSwitcher
            value={value}
            label={label}
        >
            <TextField
                id={id || `string-input-${label}`}
                value={localValue}
                onChange={handleChange}
                onBlur={handleBlur}
                onKeyDown={handleKeyDown}
                disabled={disabled}
            />
        </InputSwitcher>
    )
}

export default SwitchableStringInput

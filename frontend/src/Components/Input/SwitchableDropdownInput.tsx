import React, { ChangeEventHandler, useState } from "react"
import { NativeSelect } from "@equinor/eds-core-react"
import InputSwitcher from "./Components/InputSwitcher"

interface SwitchableDropdownInputProps {
    value: string | number;
    options: { [key: string]: string };
    label: string;
    id?: string;
    onSubmit: (newValue: number) => void;
    disabled?: boolean;
}

const SwitchableDropdownInput: React.FC<SwitchableDropdownInputProps> = ({
    value,
    options,
    label,
    id,
    onSubmit,
    disabled = false,
}: SwitchableDropdownInputProps) => {
    const [localValue, setLocalValue] = useState(value)

    const handleChange: ChangeEventHandler<HTMLSelectElement> = (e) => {
        const newValue = Number(e.currentTarget.value)
        setLocalValue(newValue)
        onSubmit(newValue)
    }

    return (
        <InputSwitcher
            value={options[localValue]}
            label={label}
        >
            <NativeSelect
                id={id || `dropdown-${label}`}
                label=""
                value={localValue}
                onChange={handleChange}
                disabled={disabled}
            >
                {Object.entries(options).map(([key, val]) => (
                    <option key={key} value={key}>{val}</option>
                ))}
            </NativeSelect>
        </InputSwitcher>
    )
}

export default SwitchableDropdownInput

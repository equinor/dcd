import React from "react"
import InputSwitcher from "./Components/InputSwitcher"
import NumberInputWithValidation from "./Components/NumberInputWithValidation"

interface SwitchableNumberInputProps {
    label: string;
    value: number | undefined;
    id?: string;
    integer: boolean;
    disabled?: boolean;
    unit?: string;
    allowNegative?: boolean;
    min?: number;
    max?: number;
    onSubmit: (value: number) => void;
}

const SwitchableNumberInput: React.FC<SwitchableNumberInputProps> = ({
    label,
    value,
    id,
    integer,
    disabled,
    unit,
    allowNegative,
    min,
    max,
    onSubmit,
}: SwitchableNumberInputProps) => (
    <InputSwitcher
        label={label}
        value={`${value ?? ""} ${unit ?? ""}`}
    >
        <NumberInputWithValidation
            label={label}
            id={id || `number-input-${label}`}
            onSubmit={onSubmit}
            defaultValue={value}
            integer={integer}
            disabled={disabled}
            unit={unit}
            allowNegative={allowNegative}
            min={min}
            max={max}
        />
    </InputSwitcher>
)

export default SwitchableNumberInput

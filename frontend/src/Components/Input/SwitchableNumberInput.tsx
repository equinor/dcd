import React from "react"
import NumberInputWithValidation from "./Components/NumberInputWithValidation"
import InputSwitcher from "./Components/InputSwitcher"
import useDataEdits from "../../Hooks/useDataEdits"
import { useCaseContext } from "../../Context/CaseContext"

interface CaseEditInputProps {
    label: string;
    object?: object;
    objectKey?: string | number
    onSubmit?: (value: number) => void;
    value: number | undefined;
    integer: boolean;
    disabled?: boolean;
    unit?: string;
    allowNegative?: boolean;
    min?: number;
    max?: number;
}

const SwitchableNumberInput: React.FC<CaseEditInputProps> = ({
    label,
    object,
    objectKey,
    onSubmit, // this will be obsolete when we introduce autosave.
    value,
    integer,
    disabled,
    unit,
    allowNegative,
    min,
    max,
}: CaseEditInputProps) => {
    const { addEdit } = useDataEdits()
    const { projectCase } = useCaseContext()

    const addToEditsAndSubmit = (insertedValue: number) => {
        if (!projectCase) {
            console.log("Case not found")
            return
        }

        if (!onSubmit) {
            console.log("onSubmit not defined")
            return
        }

        if (objectKey === undefined) {
            console.log("Object or objectKey not defined")
            return
        }

        onSubmit(insertedValue)
        addEdit(
            insertedValue,
            value,
            objectKey,
            label,
            "case",
            projectCase.id,
        )
    }

    return (
        <InputSwitcher
            label={label}
            value={`${value}`}
        >
            <NumberInputWithValidation
                onSubmit={addToEditsAndSubmit}
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
}

export default SwitchableNumberInput

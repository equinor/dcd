import React from "react"
import NumberInput from "./NumberInput"
import InputSwitcher from "./InputSwitcher"
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

const CaseEditInput: React.FC<CaseEditInputProps> = ({
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
        // logs can be removed once the solution has matured and we are confident that the edits are working as expected
        if (!projectCase) {
            console.error("Case not found")
            return
        }

        if (!onSubmit) {
            console.error("onSubmit not defined")
            return
        }

        if (!object || !objectKey) {
            console.error("Object or objectKey not defined")
            return
        }

        console.log("Adding edit", insertedValue, value, objectKey, label, "case", projectCase.id)

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
            <NumberInput
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

export default CaseEditInput

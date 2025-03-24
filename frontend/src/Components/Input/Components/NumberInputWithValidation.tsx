import { InputWrapper, Input, Icon } from "@equinor/eds-core-react"
import { error_filled } from "@equinor/eds-icons"
import { useState, useEffect } from "react"
import styled from "styled-components"

import { useAppStore } from "../../../Store/AppStore"

import { parseDecimalInput } from "@/Utils/FormatingUtils"

const ErrorIcon = styled(Icon)`
    margin-left: 8px;
`
const StyledInput = styled(Input)`
    && input[type='number']::-webkit-outer-spin-button,
    && input[type='number']::-webkit-inner-spin-button {
        -webkit-appearance: none;
        margin: 0;
    }

    && input[type='number'] {
        -moz-appearance: textfield;
        appearance: textfield;
    }
`

interface Props {
    label: string;
    id: string;
    onSubmit: (value: number) => void;
    defaultValue: number | undefined;
    integer: boolean;
    disabled?: boolean;
    unit?: string;
    allowNegative?: boolean;
    min?: number;
    max?: number;
}

const preventNonDigitInput = (e: React.KeyboardEvent<HTMLInputElement>): void => {
    if (!/\d/.test(e.key)) { e.preventDefault() }
}

const NumberInputWithValidation = ({
    label,
    id,
    onSubmit,
    defaultValue,
    integer,
    disabled,
    unit,
    allowNegative,
    min,
    max,
}: Props) => {
    const { setSnackBarMessage } = useAppStore()
    const [hasError, setHasError] = useState(false)
    const [inputValue, setInputValue] = useState<string>(defaultValue?.toString() ?? "0")
    const [helperText, setHelperText] = useState("\u200B")

    const validateInput = (value: string) => {
        if (value === "") {
            setInputValue("0")
            setHasError(false)
            setHelperText("\u200B")

            return true
        }

        const numValue = parseDecimalInput(value)

        if (min !== undefined && max !== undefined) {
            const isWithinRange = (num: number, upper: number, lower: number): boolean => num >= lower && num <= upper

            if (!isWithinRange(numValue, min, max)) {
                setHelperText(`(min: ${min}, max: ${max})`)
                setHasError(true)

                return false
            }
            setHasError(false)
            setHelperText("\u200B")

            return true
        }

        return true
    }

    const handleBlur = ({ target: { value } }: React.FocusEvent<HTMLInputElement>) => {
        if (value === "") {
            setInputValue("0")
            onSubmit(0)

            return
        }

        const numValue = parseDecimalInput(value)

        if (validateInput(value)) {
            onSubmit(numValue)
        } else {
            setSnackBarMessage(`The input for ${label} was not saved, because it's outside the allowed range (min: ${min}, max: ${max}).`)
        }
    }

    const handleChange = ({ target: { value } }: React.ChangeEvent<HTMLInputElement>) => {
        // If the input is "0" and user starts typing, clear it
        if (inputValue === "0" && value.length === 2 && value.startsWith("0")) {
            setInputValue(value.slice(1))

            return
        }
        setInputValue(value)
        validateInput(value)
    }

    const checkInput = (event: React.KeyboardEvent<HTMLInputElement>) => {
        if (integer) {
            preventNonDigitInput(event)
        }
    }

    useEffect(() => {
        if (defaultValue !== undefined) {
            setInputValue(defaultValue.toString())
            validateInput(defaultValue.toString())
        } else {
            setInputValue("0")
        }
    }, [defaultValue])

    return (
        <InputWrapper
            color={hasError ? "error" : undefined}
            helperProps={{
                text: helperText,
            }}
        >
            <StyledInput
                id={id}
                type="text"
                value={inputValue}
                disabled={disabled}
                onBlur={handleBlur}
                min={allowNegative ? undefined : 0}
                onInput={(event: React.FormEvent<HTMLInputElement>) => checkInput(event as unknown as React.KeyboardEvent<HTMLInputElement>)}
                rightAdornments={[unit, hasError ? <ErrorIcon size={16} data={error_filled} /> : undefined]}
                variant={hasError ? "error" : undefined}
                onChange={handleChange}
            />
        </InputWrapper>
    )
}

export default NumberInputWithValidation

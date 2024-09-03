import { InputWrapper, Input, Icon } from "@equinor/eds-core-react"
import { useState, useEffect } from "react"
import { error_filled } from "@equinor/eds-icons"
import styled from "styled-components"
import { preventNonDigitInput, isWithinRange } from "../../../Utils/common"
import { useAppContext } from "../../../Context/AppContext"

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
    const { setSnackBarMessage } = useAppContext()
    const [hasError, setHasError] = useState(false)
    const [inputValue, setInputValue] = useState(defaultValue)
    const [helperText, setHelperText] = useState("\u200B")
    const validateInput = (newValue: number) => {
        setInputValue(newValue)
        if (min !== undefined && max !== undefined) {
            if (!isWithinRange(newValue, min, max)) {
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

    const handleBlur = (event: React.FocusEvent<HTMLInputElement>) => {
        const newValue = Number(event.target.value)
        if (validateInput(newValue)) {
            onSubmit(newValue)
        } else {
            setSnackBarMessage(`The input for ${label} was not saved, because it's outside the allowed range (min: ${min}, max: ${max}).`)
        }
    }

    const checkInput = (event: React.KeyboardEvent<HTMLInputElement>) => {
        if (integer) {
            preventNonDigitInput(event)
        }
    }

    useEffect(() => {
        if (defaultValue !== undefined) {
            validateInput(defaultValue)
        } else {
            setInputValue(defaultValue)
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
                type="number"
                value={inputValue}
                disabled={disabled}
                onBlur={handleBlur}
                min={allowNegative ? undefined : 0}
                onInput={(event: React.FormEvent<HTMLInputElement>) => checkInput(event as unknown as React.KeyboardEvent<HTMLInputElement>)}
                rightAdornments={[unit, hasError ? <ErrorIcon size={16} data={error_filled} /> : undefined]}
                variant={hasError ? "error" : undefined}
                onChange={(e: any) => validateInput(Number(e.target.value))}
            />
        </InputWrapper>
    )
}

export default NumberInputWithValidation

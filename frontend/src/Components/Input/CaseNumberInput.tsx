import { InputWrapper, Input, Icon } from "@equinor/eds-core-react"
import { useState, useEffect } from "react"
import { error_filled } from "@equinor/eds-icons"
import styled from "styled-components"
import { preventNonDigitInput, isWithinRange } from "../../Utils/common"

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
    onChange: (value: number) => void
    defaultValue: number | undefined
    integer: boolean
    disabled?: boolean
    unit?: string
    allowNegative?: boolean
    min?: number
    max?: number
}

const CaseNumberInput = ({
    onChange,
    defaultValue,
    integer,
    disabled,
    unit,
    allowNegative,
    min,
    max,
}: Props) => {
    const [hasError, setHasError] = useState(false)
    const [inputValue, setInputValue] = useState(defaultValue)
    const [helperText, setHelperText] = useState("\u200B")

    const submitChange = (newValue: number) => {
        setInputValue(newValue)
        onChange(newValue)
        if (min !== undefined && max !== undefined) {
            if (!isWithinRange(newValue, min, max)) {
                setHelperText(`(min: ${min}, max: ${max})`)
                setHasError(true)
            } else {
                setHasError(false)
                setHelperText("\u200B")
            }
        }
    }

    const handleBlur = (event: React.FocusEvent<HTMLInputElement>) => {
        const newValue = Number(event.target.value)
        submitChange(newValue)
    }

    const checkInput = (event: React.KeyboardEvent<HTMLInputElement>) => {
        if (integer) {
            preventNonDigitInput(event)
        }
    }

    return (
        <InputWrapper
            color={hasError ? "error" : undefined}
            helperProps={{
                text: helperText,
            }}
        >
            <StyledInput
                type="number"
                defaultValue={inputValue}
                disabled={disabled}
                onBlur={handleBlur}
                min={allowNegative ? undefined : 0}
                onInput={(event: React.FormEvent<HTMLInputElement>) => checkInput(event as unknown as React.KeyboardEvent<HTMLInputElement>)}
                rightAdornments={[unit, hasError ? <ErrorIcon size={16} data={error_filled} /> : undefined]}
                variant={hasError ? "error" : undefined}
            />
        </InputWrapper>
    )
}

export default CaseNumberInput

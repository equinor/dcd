import { Input, Label } from "@equinor/eds-core-react"
import { useState, ChangeEventHandler, useEffect } from "react"
import styled from "styled-components"
import { preventNonDigitInput, isWithinRange } from "../../Utils/common"

const WrapperColumn = styled.div`
    display: flex;
    flex-direction: column;
`

const StyledLabel = styled(Label) <{ $variant: "error" | undefined }>`
    color: ${(props) => (props.$variant === "error" ? "red" : "black")};
`

interface Props {
    onChange: ChangeEventHandler<HTMLInputElement>
    defaultValue: number | undefined
    integer: boolean
    disabled?: boolean
    label?: string
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
    label,
    unit,
    allowNegative,
    min,
    max,
}: Props) => {
    const [hasError, setHasError] = useState(false)
    const [visibleLabel, setVisibleLabel] = useState(label)
    const [inputValue, setInputValue] = useState(defaultValue)

    const checkInput = (event: React.KeyboardEvent<HTMLInputElement>) => {
        if (integer) {
            preventNonDigitInput(event)
        }

        setInputValue(Number(event.currentTarget.value))
    }

    useEffect(() => {
        if (!inputValue) {
            setVisibleLabel(label)
            setHasError(false)
            return
        }

        if (min !== undefined && max !== undefined) {
            if (isWithinRange(Number(inputValue), min, max)) {
                setVisibleLabel(label)
                setHasError(false)
            } else {
                setVisibleLabel(`${label} (min: ${min}, max: ${max})`)
                setHasError(true)
            }
        }
    }, [inputValue, label, min, max])

    return (
        <WrapperColumn>
            <StyledLabel
                $variant={hasError ? "error" : undefined}
                htmlFor="numberInput"
                label={visibleLabel}
            />
            <Input
                id="numberInput"
                type="number"
                value={inputValue}
                disabled={disabled}
                onChange={onChange}
                min={allowNegative ? undefined : 0}
                onInput={(event: React.FormEvent<HTMLInputElement>) => checkInput(event as unknown as React.KeyboardEvent<HTMLInputElement>)}
                rightAdornments={unit}
                variant={hasError ? "error" : undefined}
            />
        </WrapperColumn>
    )
}

export default CaseNumberInput

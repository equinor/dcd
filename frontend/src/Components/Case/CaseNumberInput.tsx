import { Input, Label } from "@equinor/eds-core-react"
import { ChangeEventHandler } from "react"
import styled from "styled-components"

export const WrapperColumn = styled.div`
    display: flex;
    flex-direction: column;
`

interface Props {
    onChange: ChangeEventHandler<HTMLInputElement>
    defaultValue: number | undefined
    integer: boolean
    disabled?: boolean
    label: string
    unit?: string
    allowNegative?: boolean
}

const CaseNumberInput = ({
    onChange,
    defaultValue,
    integer,
    disabled,
    label,
    unit,
    allowNegative,
}: Props) => (
    <WrapperColumn>
        <Label htmlFor="numberInput" label={label} />
        <Input
            id="numberInput"
            type="number"
            defaultValue={defaultValue}
            value={defaultValue}
            disabled={disabled}
            onChange={onChange}
            min={allowNegative ? undefined : 0}
            onKeyPress={(event: React.KeyboardEvent<HTMLInputElement>) => {
                if (integer && !/\d/.test(event.key)) {
                    event.preventDefault()
                }
            }}
            rightAdornments={(
                // eslint-disable-next-line react/jsx-no-useless-fragment
                <>
                    {unit}
                </>
            )}
        />
    </WrapperColumn>
)

export default CaseNumberInput

import { Input, Label } from "@equinor/eds-core-react"
import { ChangeEventHandler } from "react"
import styled from "styled-components"
import { isDefaultDate, toMonthDate } from "../../Utils/common"

const ColumnWrapper = styled.div`
    display: flex;
    flex-direction: column;
`

interface Props {
    onChange: ChangeEventHandler<HTMLInputElement>
    value: Date
    label: string
    max?: string | undefined
    min?: string | undefined
}

const CaseDateField = ({
    onChange,
    value,
    label,
    max,
    min,
}: Props) => (
    <ColumnWrapper>
        <Label htmlFor="NumberInput" label={label} />
        <Input
            type="month"
            id="dgDate"
            name="dgDate"
            value={isDefaultDate(value) ? undefined : toMonthDate(value)}
            onChange={onChange}
            max={max}
            min={min}
        />
    </ColumnWrapper>
)

export default CaseDateField

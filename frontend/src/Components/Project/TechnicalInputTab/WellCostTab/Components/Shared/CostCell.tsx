import { Input, Table, Typography } from "@equinor/eds-core-react"
import {
    ChangeEventHandler,
} from "react"
import styled from "styled-components"

import { useAppContext } from "@/Context/AppContext"

interface Props {
    title: string
    setValue: (value: number | undefined) => void
    value: number
}

const StyledTypography = styled(Typography)`
    text-align: right;
`

const CostCell = ({
    title, setValue, value,
}: Props) => {
    const { editMode } = useAppContext()
    const onValueChange: ChangeEventHandler<HTMLInputElement> = (e) => {
        setValue(Number(e.target.value))
    }
    return (
        <Table.Row key={1}>
            <Table.Cell>
                {title}
            </Table.Cell>
            <Table.Cell>
                {editMode
                    ? (
                        <Input
                            id="WellCost"
                            type="number"
                            value={value}
                            onChange={onValueChange}
                        />
                    )
                    : <StyledTypography>{value}</StyledTypography>}
            </Table.Cell>
        </Table.Row>
    )
}

export default CostCell

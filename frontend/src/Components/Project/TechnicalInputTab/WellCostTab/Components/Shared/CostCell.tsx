import { Input, Table, Typography } from "@equinor/eds-core-react"
import {
    ChangeEventHandler,
} from "react"
import styled from "styled-components"

import { useAppStore } from "@/Store/AppStore"
import useEditDisabled from "@/Hooks/useEditDisabled"

interface Props {
    title: string
    setValue: (value: number) => void
    value: number
}

const StyledTypography = styled(Typography)`
    text-align: right;
`

const CostCell = ({
    title, setValue, value,
}: Props) => {
    const { editMode } = useAppStore()
    const { isEditDisabled } = useEditDisabled()
    const onValueChange: ChangeEventHandler<HTMLInputElement> = (e) => {
        setValue(Number(e.target.value))
    }
    return (
        <Table.Row key={1}>
            <Table.Cell>
                {title}
            </Table.Cell>
            <Table.Cell>
                {editMode && !isEditDisabled
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

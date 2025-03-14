import { Input, Table, Typography } from "@equinor/eds-core-react"
import {
    ChangeEventHandler,
} from "react"
import styled from "styled-components"
import useCanUserEdit from "@/Hooks/useCanUserEdit"

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
    const { canEdit } = useCanUserEdit()
    const onValueChange: ChangeEventHandler<HTMLInputElement> = (e) => {
        setValue(Number(e.target.value))
    }

    const handleWheel = (e: React.WheelEvent<HTMLInputElement>) => {
        if (e.currentTarget === document.activeElement) {
            e.currentTarget.blur()
        }
    }

    return (
        <Table.Row key={1}>
            <Table.Cell>
                {title}
            </Table.Cell>
            <Table.Cell>
                {canEdit()
                    ? (
                        <Input
                            id="WellCost"
                            type="number"
                            value={value}
                            onChange={onValueChange}
                            onWheel={handleWheel}
                        />
                    )
                    : <StyledTypography>{value}</StyledTypography>}
            </Table.Cell>
        </Table.Row>
    )
}

export default CostCell

import { Input, Table } from "@equinor/eds-core-react"
import {
    ChangeEventHandler,
    Dispatch, SetStateAction, useState,
} from "react"

interface Props {
    title: string
    setValue: Dispatch<SetStateAction<number | undefined>>
    value: number
}

function OperationalWellCost({
    title, setValue, value,
}: Props) {
    const onValueChange: ChangeEventHandler<HTMLInputElement> = (e) => {
        setValue(Number(e.target.value))
    }
    return (
        <Table.Row key={1}>
            <Table.Cell>
                {title}
            </Table.Cell>
            <Table.Cell>
                <Input
                    id="WellCost"
                    type="number"
                    value={value}
                    onChange={onValueChange}
                />
            </Table.Cell>
        </Table.Row>
    )
}

export default OperationalWellCost

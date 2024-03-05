import { Input, Table, Typography } from "@equinor/eds-core-react"
import {
    ChangeEventHandler,
    Dispatch, SetStateAction,
} from "react"
import { useModalContext } from "../../Context/ModalContext"
import { useAppContext } from "../../Context/AppContext"

interface Props {
    title: string
    setValue: Dispatch<SetStateAction<number | undefined>>
    value: number
}

const OperationalWellCost = ({
    title, setValue, value,
}: Props) => {
    const { editMode } = useAppContext()
    const { editTechnicalInput } = useModalContext()
    const onValueChange: ChangeEventHandler<HTMLInputElement> = (e) => {
        setValue(Number(e.target.value))
    }
    return (
        <Table.Row key={1}>
            <Table.Cell>
                {title}
            </Table.Cell>
            <Table.Cell>
                {editMode || editTechnicalInput
                    ? <Input
                        id="WellCost"
                        type="number"
                        value={value}
                        onChange={onValueChange}
                    />
                : <Typography>{value}</Typography>
                }
            </Table.Cell>
        </Table.Row>
    )
}

export default OperationalWellCost

import { Input, Label } from "@equinor/eds-core-react"
import { ChangeEventHandler, Dispatch, SetStateAction } from "react"
import { WrapperColumnTablePeriod } from "../Views/Asset/StyledAssetComponents"

interface Props {
    setValue?: Dispatch<SetStateAction<number>>
    value: number | undefined
    setHasChanges?: Dispatch<SetStateAction<boolean>>
    integer: boolean,
    disabled?: boolean
    label: string
}

const NumberInputTable = ({
    setValue,
    value,
    setHasChanges,
    integer,
    disabled,
    label,
}: Props) => {
    const onChange: ChangeEventHandler<HTMLInputElement> = async (e) => {
        if (setValue) {
            setValue(Number(e.target.value))
        }
        if (setHasChanges) {
            if (e.target.value !== undefined && e.target.value !== "") {
                setHasChanges(true)
            } else {
                setHasChanges(false)
            }
        }
    }

    return (
        <WrapperColumnTablePeriod>
            <Label htmlFor="NumberInputTable" label={label} />
            <Input
                id="NumberInputTable"
                type="number"
                value={value}
                disabled={disabled}
                onChange={onChange}
            />
        </WrapperColumnTablePeriod>
    )
}

export default NumberInputTable

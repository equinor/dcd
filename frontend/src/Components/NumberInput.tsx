import { Input, Label } from "@equinor/eds-core-react"
import { ChangeEventHandler, Dispatch, SetStateAction } from "react"
import {
    WrapperColumn,
} from "../Views/Asset/StyledAssetComponents"

interface Props {
    setValue?: Dispatch<SetStateAction<number | undefined>>
    value: number | undefined
    setHasChanges?: Dispatch<SetStateAction<boolean>>
    integer: boolean,
    label: string
}

const NumberInput = ({
    setValue,
    value,
    setHasChanges,
    integer,
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
        <WrapperColumn style={{ paddingLeft: 10 }}>
            <Label htmlFor="NumberInput" label={label} />
            <Input
                id="NumberInput"
                type="number"
                value={value}
                disabled={setValue === undefined}
                onChange={onChange}
                onKeyPress={(event) => {
                    if (integer && !/\d/.test(event.key)) {
                        event.preventDefault()
                    }
                }}
            />
        </WrapperColumn>
    )
}

export default NumberInput

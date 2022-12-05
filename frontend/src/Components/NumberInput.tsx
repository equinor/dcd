import { Input, Label } from "@equinor/eds-core-react"
import { ChangeEventHandler, Dispatch, SetStateAction } from "react"
import { WrapperColumn } from "../Views/Asset/StyledAssetComponents"

interface Props {
    setValue?: Dispatch<SetStateAction<number | undefined>>
    value: number | undefined
    setHasChanges?: Dispatch<SetStateAction<boolean>>
    integer: boolean,
    disabled?: boolean
    label: string
}

const NumberInput = ({
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
        <WrapperColumn>
            <Label htmlFor="NumberInput" label={label} />
            <Input
                id="NumberInput"
                type="number"
                value={value}
                disabled={disabled}
                onChange={onChange}
                onKeyPress={(event: any) => {
                    if (integer && !/\d/.test(event.key)) {
                        event.preventDefault()
                    }
                }}
            />
        </WrapperColumn>
    )
}

export default NumberInput

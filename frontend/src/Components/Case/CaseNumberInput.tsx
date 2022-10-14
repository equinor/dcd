import { Input, Label } from "@equinor/eds-core-react"
import { ChangeEventHandler, Dispatch, SetStateAction } from "react"
import { WrapperColumn } from "../../Views/Asset/StyledAssetComponents"

interface Props {
    onChange: ChangeEventHandler<HTMLInputElement>
    value: number | undefined
    integer: boolean,
    disabled?: boolean
    label: string
}

const CaseNumberInput = ({
    onChange,
    value,
    integer,
    disabled,
    label,
}: Props) => (
    <WrapperColumn>
        <Label htmlFor="NumberInput" label={label} />
        <Input
            id="NumberInput"
            type="number"
            value={value}
            disabled={disabled}
            onChange={onChange}
            onKeyPress={(event) => {
                    if (integer && !/\d/.test(event.key)) {
                        event.preventDefault()
                    }
                }}
        />
    </WrapperColumn>
    )

export default CaseNumberInput

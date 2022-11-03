import { Input, Label } from "@equinor/eds-core-react"
import { ChangeEventHandler } from "react"
import { WrapperColumn } from "../../Views/Asset/StyledAssetComponents"

interface Props {
    onChange: ChangeEventHandler<HTMLInputElement>
    defaultValue: number | undefined
    integer: boolean,
    disabled?: boolean
    label: string
}

const CaseNumberInput = ({
    onChange,
    defaultValue,
    integer,
    disabled,
    label,
}: Props) => (
    <WrapperColumn>
        <Label htmlFor="NumberInput" label={label} />
        <Input
            id="NumberInput"
            type="number"
            defaultValue={defaultValue}
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

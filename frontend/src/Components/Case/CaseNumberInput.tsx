import { Input, Label } from "@equinor/eds-core-react"
import { ChangeEventHandler } from "react"
import { WrapperColumn } from "../../Views/Asset/StyledAssetComponents"

interface Props {
    onChange: ChangeEventHandler<HTMLInputElement>
    value: number | undefined
    integer: boolean,
    disabled?: boolean
    label: string
    // nonNegativeValues: boolean
    // isPercentage: boolean
}

const CaseNumberInput = ({
    onChange,
    value,
    integer,
    disabled,
    label,
    // nonNegativeValues,
    // isPercentage,
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
            min={0} // nonNegativeValues
            max={100} // isPercentage
        />
    </WrapperColumn>
)

export default CaseNumberInput

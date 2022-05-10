import {
    NativeSelect,
} from "@equinor/eds-core-react"
import {
    ChangeEvent, Dispatch, SetStateAction,
} from "react"
import styled from "styled-components"

const CurrencyDropdown = styled(NativeSelect)`
width: 20rem;
padding-bottom: 2em;
`

interface Props {
    setCurrency: Dispatch<SetStateAction<Components.Schemas.Currency>>,
    setHasChanges: Dispatch<SetStateAction<boolean>>,
    currentValue: Components.Schemas.Currency,
}

const AssetCurrency = ({
    setCurrency,
    setHasChanges,
    currentValue,
}: Props) => {
    const onChange = async (event: ChangeEvent<HTMLSelectElement>) => {
        if (Number(event.currentTarget.selectedOptions[0].value) !== currentValue) {
            setHasChanges(true)
        }
        switch (event.currentTarget.selectedOptions[0].value) {
        case "0":
            setCurrency(0)
            break
        case "1":
            setCurrency(1)
            break
        default:
            setCurrency(0)
            break
        }
    }

    return (
        <CurrencyDropdown
            label="Currency"
            id="currency"
            placeholder="Choose a currency"
            onChange={(event: ChangeEvent<HTMLSelectElement>) => onChange(event)}
            value={currentValue}
            disabled={false}
        >
            <option key="0" value={0}>USD</option>
            <option key="1" value={1}>NOK</option>
        </CurrencyDropdown>
    )
}

export default AssetCurrency

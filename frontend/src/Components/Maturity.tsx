import {
    NativeSelect,
} from "@equinor/eds-core-react"
import {
    ChangeEvent, Dispatch, SetStateAction,
} from "react"
import styled from "styled-components"

const MaturityDropdown = styled(NativeSelect)`
width: 20rem;
padding-bottom: 2em;
`

interface Props {
    setMaturity: Dispatch<SetStateAction<Components.Schemas.Maturity | undefined>>,
    currentValue: Components.Schemas.Maturity | undefined,
    setHasChanges?: Dispatch<SetStateAction<boolean>>
}

const Maturity = ({
    setMaturity,
    currentValue,
    setHasChanges,
}: Props) => {
    const onChange = async (event: ChangeEvent<HTMLSelectElement>) => {
        let maturity:Components.Schemas.Maturity | undefined
        switch (event.currentTarget.selectedOptions[0].value) {
        case "0":
            setMaturity(0)
            maturity = 0
            break
        case "1":
            setMaturity(1)
            maturity = 1
            break
        case "2":
            setMaturity(2)
            maturity = 2
            break
        case "3":
            setMaturity(3)
            maturity = 3
            break
        default:
            setMaturity(undefined)
            maturity = undefined
            break
        }
        if (maturity !== currentValue && setHasChanges !== undefined) {
            setHasChanges(true)
        }
    }

    return (
        <MaturityDropdown
            label="Maturity"
            id="Maturity"
            placeholder="Choose maturity"
            onChange={(event: ChangeEvent<HTMLSelectElement>) => onChange(event)}
            value={currentValue}
        >
            <option key="0" value={0}>A</option>
            <option key="1" value={1}>B</option>
            <option key="2" value={2}>C</option>
            <option key="3" value={3}>D</option>
        </MaturityDropdown>
    )
}

export default Maturity

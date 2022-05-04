import {
    NativeSelect,
} from "@equinor/eds-core-react"
import {
    ChangeEvent, Dispatch, SetStateAction,
} from "react"
import styled from "styled-components"

const UnitDropdown = styled(NativeSelect)`
width: 7rem;
padding-bottom: 2em;
`

interface Props {
    setUnit: Dispatch<SetStateAction<Components.Schemas.Unit | undefined>>,
    currentValue: Components.Schemas.Unit | undefined,
    setHasChanges?: Dispatch<SetStateAction<boolean>>
}

const Unit = ({
    setUnit,
    currentValue,
    setHasChanges,
}: Props) => {
    const onChange = async (event: ChangeEvent<HTMLSelectElement>) => {
        let unit: Components.Schemas.Unit | undefined
        switch (event.currentTarget.selectedOptions[0].value) {
        case "0":
            setUnit(0)
            unit = 0
            break
        case "1":
            setUnit(1)
            unit = 1
            break
        case "2":
            setUnit(2)
            unit = 2
            break
        case "3":
            setUnit(3)
            unit = 3
            break
        case "4":
            setUnit(4)
            unit = 4
            break
        case "5":
            setUnit(5)
            unit = 5
            break
        case "6":
            setUnit(6)
            unit = 6
            break
        case "7":
            setUnit(7)
            unit = 7
            break
        case "8":
            setUnit(8)
            unit = 8
            break
        case "9":
            setUnit(9)
            unit = 9
            break
        default:
            setUnit(0)
            unit = 0
            break
        }
        if (unit !== currentValue && setHasChanges !== undefined) {
            setHasChanges(true)
        }
    }

    return (
        <UnitDropdown
            label="Unit (per year)"
            id="Unit"
            placeholder="Choose unit"
            onChange={(event: ChangeEvent<HTMLSelectElement>) => onChange(event)}
            value={currentValue}
        >
            <option key="0" value={0}>Sm3</option>
            <option key="1" value={1}>bbl</option>
            <option key="2" value={2}>scf</option>
            <option key="3" value={3}>J</option>
            <option key="4" value={4}>Wh</option>
            <option key="5" value={5}>kg</option>
            <option key="6" value={6}>tonnes</option>
            <option key="7" value={7}>prescript</option>
            <option key="8" value={8}>M</option>
            <option key="9" value={9}>G</option>
        </UnitDropdown>
    )
}

export default Unit

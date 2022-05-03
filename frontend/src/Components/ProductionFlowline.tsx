import {
    NativeSelect,
} from "@equinor/eds-core-react"
import {
    ChangeEvent, Dispatch, SetStateAction,
} from "react"
import styled from "styled-components"

const ProductionFlowlineDropdown = styled(NativeSelect)`
width: 20rem;
padding-bottom: 2em;
`

interface Props {
    setProductionFlowline: Dispatch<SetStateAction<Components.Schemas.ProductionFlowline | undefined>>,
    currentValue: Components.Schemas.ProductionFlowline | undefined,
    setHasChanges?: Dispatch<SetStateAction<boolean>>
}

const ProductionFlowline = ({
    setProductionFlowline,
    currentValue,
    setHasChanges,
}: Props) => {
    const onChange = async (event: ChangeEvent<HTMLSelectElement>) => {
        let selectedOptionAsValue: Components.Schemas.ProductionFlowline | undefined
        switch (event.currentTarget.selectedOptions[0].value) {
        case "0":
            setProductionFlowline(0)
            selectedOptionAsValue = 0
            break
        case "1":
            setProductionFlowline(1)
            selectedOptionAsValue = 1
            break
        case "2":
            setProductionFlowline(2)
            selectedOptionAsValue = 2
            break
        case "3":
            setProductionFlowline(3)
            selectedOptionAsValue = 3
            break
        case "4":
            setProductionFlowline(4)
            selectedOptionAsValue = 4
            break
        case "5":
            setProductionFlowline(5)
            selectedOptionAsValue = 5
            break
        case "6":
            setProductionFlowline(6)
            selectedOptionAsValue = 6
            break
        case "7":
            setProductionFlowline(7)
            selectedOptionAsValue = 7
            break
        case "8":
            setProductionFlowline(8)
            selectedOptionAsValue = 8
            break
        case "9":
            setProductionFlowline(9)
            selectedOptionAsValue = 9
            break
        case "10":
            setProductionFlowline(10)
            selectedOptionAsValue = 10
            break
        case "11":
            setProductionFlowline(11)
            selectedOptionAsValue = 11
            break
        case "12":
            setProductionFlowline(12)
            selectedOptionAsValue = 12
            break
        case "13":
            setProductionFlowline(13)
            selectedOptionAsValue = 13
            break
        default:
            setProductionFlowline(0)
            selectedOptionAsValue = 0
            break
        }
        if (selectedOptionAsValue !== currentValue && setHasChanges !== undefined) {
            setHasChanges(true)
        }
    }

    return (
        <ProductionFlowlineDropdown
            label="Production flowline"
            id="ProductionFlowline"
            placeholder="Choose Production flowline"
            onChange={(event: ChangeEvent<HTMLSelectElement>) => onChange(event)}
            value={currentValue}
        >
            <option key="0" value={0}>No production flowline</option>
            <option key="1" value={1}>Carbon</option>
            <option key="2" value={2}>SS Clad</option>
            <option key="3" value={3}>Cr13</option>
            <option key="4" value={4}>Carbon + insulation</option>
            <option key="5" value={5}>SS Clad + insulation</option>
            <option key="6" value={6}>Cr13 + insulation</option>
            <option key="7" value={7}>Carbon + insulation + DEH</option>
            <option key="8" value={8}>SS Clad + insulation + DEH</option>
            <option key="9" value={9}>Cr13 + insulation + DEH</option>
            <option key="10" value={10}>Carbon + PIP</option>
            <option key="11" value={11}>SS Clad + PIP</option>
            <option key="12" value={12}>Cr13 + PIP</option>
            <option key="13" value={13}>HDPE lined CS (Water injection only)</option>
        </ProductionFlowlineDropdown>
    )
}

export default ProductionFlowline

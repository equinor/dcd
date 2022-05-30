import {
    NativeSelect,
} from "@equinor/eds-core-react"
import {
    ChangeEvent, Dispatch, SetStateAction,
} from "react"
import styled from "styled-components"

const ConceptDropdown = styled(NativeSelect)`
width: 20rem;
padding-bottom: 2em;
`

interface Props {
    setConcept: Dispatch<SetStateAction<Components.Schemas.Concept | undefined>>,
    currentValue: Components.Schemas.Concept | undefined,
    setHasChanges?: Dispatch<SetStateAction<boolean>>
}

const Concept = ({
    setConcept,
    currentValue,
    setHasChanges,
}: Props) => {
    const onChange = async (event: ChangeEvent<HTMLSelectElement>) => {
        let selectedOptionAsValue: Components.Schemas.Concept | undefined
        switch (event.currentTarget.selectedOptions[0].value) {
        case "0":
            setConcept(0)
            selectedOptionAsValue = 0
            break
        case "1":
            setConcept(1)
            selectedOptionAsValue = 1
            break
        case "2":
            setConcept(2)
            selectedOptionAsValue = 2
            break
        case "3":
            setConcept(3)
            selectedOptionAsValue = 3
            break
        case "4":
            setConcept(4)
            selectedOptionAsValue = 4
            break
        case "5":
            setConcept(5)
            selectedOptionAsValue = 5
            break
        case "6":
            setConcept(6)
            selectedOptionAsValue = 6
            break
        case "7":
            setConcept(7)
            selectedOptionAsValue = 7
            break
        case "8":
            setConcept(8)
            selectedOptionAsValue = 8
            break
        case "9":
            setConcept(9)
            selectedOptionAsValue = 9
            break
        case "10":
            setConcept(10)
            selectedOptionAsValue = 10
            break
        case "11":
            setConcept(11)
            selectedOptionAsValue = 11
            break
        case "12":
            setConcept(12)
            selectedOptionAsValue = 12
            break
        default:
            setConcept(0)
            selectedOptionAsValue = 0
            break
        }
        if (selectedOptionAsValue !== currentValue && setHasChanges !== undefined) {
            setHasChanges(true)
        }
    }

    return (
        <ConceptDropdown
            label="Concept"
            id="Concept"
            placeholder="Choose Concept"
            onChange={(event: ChangeEvent<HTMLSelectElement>) => onChange(event)}
            value={currentValue}
        >
            <option key="0" value={0}>No Concept</option>
            <option key="1" value={1}>Tie-back to existing offshore platform</option>
            <option key="2" value={2}>JACKET - Fixed Steel Jacket</option>
            <option key="3" value={3}>GBS - Fixed Concrete Platform - Gravity Based Structure</option>
            <option key="4" value={4}>TLP - Tension Leg Platform - Steel</option>
            <option key="5" value={5}>SPAR Platform - Steel</option>
            <option key="6" value={6}>SEMI - Semi Submersible - Steel</option>
            <option key="7" value={7}>CIRCULAR BARGE - Sevan type FPSO</option>
            <option key="8" value={8}>BARGE - Barge shaped - Spread Moored FPSO</option>
            <option key="9" value={9}>FPSO - Ship shaped - TUrret Moored</option>
            <option key="10" value={10}>TANKER - converted tanker FPSO - Turret Moored</option>
            <option key="11" value={11}>JACK-UP Platform</option>
            <option key="12" value={12}>Subsea to shore</option>
        </ConceptDropdown>
    )
}

export default Concept

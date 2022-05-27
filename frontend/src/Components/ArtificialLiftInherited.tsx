/* eslint-disable camelcase */
import {
    Button,
    EdsProvider,
    Icon,
    NativeSelect,
    Tooltip,
} from "@equinor/eds-core-react"
import { info_circle } from "@equinor/eds-icons"
import {
    ChangeEvent, Dispatch, SetStateAction, useEffect, useState,
} from "react"
import styled from "styled-components"
import { WrapperInherited } from "../Views/Asset/StyledAssetComponents"

const ArtificialLiftDropdown = styled(NativeSelect)`
width: 20rem;
padding-bottom: 2em;
> *:not(:last-child) {
    color: blue;
}
`

const ActionsContainer = styled.div`
    margin-top: 1rem;
    margin-left: 0.1rem;
`

interface Props {
    setArtificialLift: Dispatch<SetStateAction<Components.Schemas.ArtificialLift | undefined>>,
    setHasChanges?: Dispatch<SetStateAction<boolean>>,
    currentValue: Components.Schemas.ArtificialLift | undefined,
    caseArtificialLift: Components.Schemas.ArtificialLift | undefined
}

const ArtificialLiftInherited = ({
    setArtificialLift,
    setHasChanges,
    currentValue,
    caseArtificialLift,
}: Props) => {
    const [isMismatchedToCase, setIsMismatchedToCase] = useState<boolean | undefined>()

    useEffect(() => {
        (async () => {
            if (caseArtificialLift !== currentValue) {
                return setIsMismatchedToCase(true)
            }
            return setIsMismatchedToCase(false)
        })()
    })

    const onChange = async (event: ChangeEvent<HTMLSelectElement>) => {
        let al: Components.Schemas.ArtificialLift | undefined
        switch (event.currentTarget.selectedOptions[0].value) {
        case "0":
            setArtificialLift(0)
            al = 0
            break
        case "1":
            setArtificialLift(1)
            al = 1
            break
        case "2":
            setArtificialLift(2)
            al = 2
            break
        case "3":
            setArtificialLift(3)
            al = 3
            break
        default:
            al = 0
            setArtificialLift(0)
            break
        }
        if (al !== currentValue && setHasChanges !== undefined) {
            setHasChanges(true)
        }
    }

    return (
        <WrapperInherited>
            <ArtificialLiftDropdown
                label="Artificial Lift"
                id="ArtificialLift"
                placeholder="Choose an artificial lift"
                onChange={(event: ChangeEvent<HTMLSelectElement>) => onChange(event)}
                value={currentValue}
                disabled={false}
            >
                <option key="0" value={0}>No lift</option>
                <option key="1" value={1}>Gas lift</option>
                <option key="2" value={2}>Electrical submerged pumps</option>
                <option key="3" value={3}>Subsea booster pumps</option>
            </ArtificialLiftDropdown>
            <EdsProvider density="compact">
                <ActionsContainer hidden={!isMismatchedToCase}>
                    <Tooltip
                        title="Data does not match data on case. Using this data will overwrite asset data."
                        hidden={!isMismatchedToCase}
                    >
                        <Button
                            variant="ghost_icon"
                            aria-label="Case mismatch"
                            color="danger"
                        >
                            <Icon data={info_circle} />
                        </Button>
                    </Tooltip>
                </ActionsContainer>
            </EdsProvider>
        </WrapperInherited>
    )
}

export default ArtificialLiftInherited

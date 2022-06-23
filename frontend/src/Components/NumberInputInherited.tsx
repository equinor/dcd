/* eslint-disable camelcase */
import {
    Button, EdsProvider, Icon, Input, Label, Tooltip,
} from "@equinor/eds-core-react"
import { info_circle } from "@equinor/eds-icons"
import {
    ChangeEventHandler, Dispatch, SetStateAction, useEffect, useState,
} from "react"
import styled from "styled-components"
import {
    WrapperColumn,
    WrapperInherited,
} from "../Views/Asset/StyledAssetComponents"

const ActionsContainer = styled.div`
    margin-top: -1rem;
`

interface Props {
    setValue?: Dispatch<SetStateAction<number | undefined>>
    value: number | undefined
    setHasChanges?: Dispatch<SetStateAction<boolean>>
    integer: boolean,
    disabled?: boolean
    label: string
    caseValue: number | undefined,
}

const NumberInputInherited = ({
    setValue,
    value,
    setHasChanges,
    integer,
    disabled,
    label,
    caseValue,
}: Props) => {
    const [isMismatchedToCase, setIsMismatchedToCase] = useState<boolean | undefined>()

    useEffect(() => {
        (async () => {
            if (caseValue !== null && caseValue !== undefined && caseValue !== value) {
                return setIsMismatchedToCase(true)
            }
            return setIsMismatchedToCase(false)
        })()
    })

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
            <WrapperInherited>
                <Label htmlFor="NumberInput" label={label} />
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
}

export default NumberInputInherited

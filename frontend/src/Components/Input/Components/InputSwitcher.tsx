import { Typography, InputWrapper } from "@equinor/eds-core-react"
import React, { memo } from "react"
import styled from "styled-components"

import useCanUserEdit from "@/Hooks/useCanUserEdit"
import { useAppStore } from "@/Store/AppStore"

const ViewValue = styled(Typography)`
    margin-top: 10px;
    font-size: 16px;
`

interface InputSwitcherProps {
    label?: string;
    value: string;
    children: React.ReactElement;
}

const InputSwitcher = memo(({ value, label, children }: InputSwitcherProps): JSX.Element => {
    const { isSaving } = useAppStore()
    const { canEdit } = useCanUserEdit()

    return (
        <InputWrapper
            labelProps={{
                label,
            }}
        >
            {canEdit() ? (
                <div key="input">
                    {React.cloneElement(children, {
                        disabled: children.props.disabled === true ? true : isSaving,
                    })}
                </div>
            ) : (
                <div key="text">
                    <ViewValue id={`${label}-${value}`}>{value}</ViewValue>
                </div>
            )}
        </InputWrapper>
    )
})

InputSwitcher.displayName = "InputSwitcher"

export default InputSwitcher

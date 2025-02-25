import { Typography, InputWrapper } from "@equinor/eds-core-react"
import styled from "styled-components"
import React from "react"

import { useAppStore } from "@/Store/AppStore"
import useEditDisabled from "@/Hooks/useEditDisabled"

const ViewValue = styled(Typography)`
    margin-top: 10px;
    font-size: 16px;
`
interface InputSwitcherProps {
    label?: string;
    value: string;
    children: React.ReactElement;
}

const InputSwitcher = ({ value, label, children }: InputSwitcherProps): JSX.Element => {
    const { editMode, isSaving } = useAppStore()
    const { isEditDisabled } = useEditDisabled()

    return (
        <InputWrapper
            labelProps={{
                label,
            }}
        >
            {editMode && !isEditDisabled ? (
                <div key="input">
                    {React.cloneElement(children, { disabled: isSaving })}
                </div>
            ) : (
                <div key="text">
                    <ViewValue id={`${label}-${value}`}>{value}</ViewValue>
                </div>
            )}
        </InputWrapper>
    )
}

export default InputSwitcher

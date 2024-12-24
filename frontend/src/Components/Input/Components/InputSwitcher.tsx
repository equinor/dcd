import { Typography, InputWrapper } from "@equinor/eds-core-react"
import styled from "styled-components"

import { useAppContext } from "@/Context/AppContext"

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
    const { editMode } = useAppContext()

    return (
        <InputWrapper
            labelProps={{
                label,
            }}
        >
            {editMode ? (
                <div key="input">
                    {children}
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

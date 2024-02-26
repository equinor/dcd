import { Typography } from "@equinor/eds-core-react"
import styled from "styled-components"
import { useAppContext } from "../../Context/AppContext"

const Wrapper = styled.div`
    margin-bottom: 8px;
`

interface InputSwitcherProps {
    label: string;
    value: string;
    children: React.ReactElement
}

const InputSwitcher = ({ value, label, children }: InputSwitcherProps): JSX.Element => {
    const { editMode } = useAppContext()

    if (editMode) {
        return children
    }
    return (
        <Wrapper>
            <Typography variant="meta">
                {label}
                :
            </Typography>
            <Typography>{value}</Typography>
        </Wrapper>
    )
}

export default InputSwitcher

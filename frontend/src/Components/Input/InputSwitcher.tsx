import { Typography, InputWrapper } from "@equinor/eds-core-react"
import { useAppContext } from "../../Context/AppContext"

interface InputSwitcherProps {
    label?: string;
    value: string;
    children: React.ReactElement
}

const InputSwitcher = ({ value, label, children }: InputSwitcherProps): JSX.Element => {
    const { editMode } = useAppContext()

    if (editMode) {
        return children
    }
    return (
        <InputWrapper
            labelProps={{
                label,
            }}
        >
            {editMode
                ? children
                : <Typography>{value}</Typography>}
        </InputWrapper>
    )
}

export default InputSwitcher

import { Typography } from "@equinor/eds-core-react"
import { useAppContext } from "../../Context/AppContext"

interface InputSwitcherProps {
    value: string;
    children: React.ReactElement
}

const InputSwitcher = ({ value, children }: InputSwitcherProps): JSX.Element => {
    const { editMode } = useAppContext()

    if (editMode) {
        return children
    }
    return <Typography>{value}</Typography>
}

export default InputSwitcher

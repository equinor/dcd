import { Typography, InputWrapper } from "@equinor/eds-core-react"
import { motion, AnimatePresence } from "framer-motion"
import styled from "styled-components"
import { useAppContext } from "../../../Context/AppContext"

const ViewValue = styled(Typography)`
    margin-top: 10px;
    font-size: 1rem;
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
            <AnimatePresence mode="popLayout" initial={false}>
                {editMode ? (
                    <motion.div
                        key="input"
                        initial={{ opacity: 0, y: -10 }}
                        animate={{ opacity: 1, y: 0 }}
                        transition={{ type: "spring", stiffness: 500, damping: 20 }}
                    >
                        {children}
                    </motion.div>
                ) : (
                    <motion.div
                        key="text"
                        initial={{ opacity: 0, y: -10 }}
                        animate={{ opacity: 1, y: 0 }}
                        transition={{ type: "spring", stiffness: 500, damping: 20 }}
                    >
                        <ViewValue>{value}</ViewValue>
                    </motion.div>
                )}
            </AnimatePresence>
        </InputWrapper>
    )
}

export default InputSwitcher

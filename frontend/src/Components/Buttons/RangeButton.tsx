import React from "react"
import styled from "styled-components"
import { Button } from "@equinor/eds-core-react"

interface RangeButtonProps {
    onClick: () => void;
}

const StyledButton = styled(Button)`
    position: relative;
    top: 25px;
`

const RangeButton: React.FC<RangeButtonProps> = ({ onClick }) => <StyledButton onClick={onClick}>Apply</StyledButton>

export default RangeButton

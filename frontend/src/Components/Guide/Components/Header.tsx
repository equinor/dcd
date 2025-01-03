import React from "react"
import styled from "styled-components"
import { Typography } from "@equinor/eds-core-react"

const HeaderText = styled(Typography).attrs({ variant: "h1" })`
    font-size: 32px;
    padding-bottom: 15px;
    padding-top: 35px;
 
`

interface HeaderProps {
    children: React.ReactNode
    }

const Header = ({ children }: HeaderProps) => (
    <HeaderText>{children}</HeaderText>
)

export default Header

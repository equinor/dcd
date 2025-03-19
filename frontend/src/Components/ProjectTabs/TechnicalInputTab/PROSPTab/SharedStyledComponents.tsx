import { Typography, NativeSelect, Input } from "@equinor/eds-core-react"
import styled from "styled-components"

export const Container = styled.div`
    display: flex;
    align-items: center;
    width: 100%;
    padding: 8px 0;
    border-bottom: 1px solid #E6E6E6;
`

export const HeaderContainer = styled(Container)`
    padding: 16px 0 8px 0;
    border-bottom: 2px solid #E6E6E6;
`

export const LeftColumn = styled.div`
    width: 25%;
    padding-right: 16px;
    font-weight: 700;
`

export const MiddleColumn = styled.div`
    flex: 1;
    padding-right: 16px;
`

export const RightColumn = styled.div`
    width: 100px;
    text-align: right;
`

export const StyledSelect = styled(NativeSelect)`
    width: 100%;
`

export const StyledInput = styled(Input)`
    width: 100%;
`

export const TextDisplay = styled(Typography)`
    padding: 8px 0;
`

export const UrlLink = styled.a`
    color: #007079;
    text-decoration: underline;
    cursor: pointer;
    padding: 8px 0;
    display: inline-block;
    font-size: 16px;
    word-break: break-all;
    
    &:hover {
        text-decoration: none;
    }
`

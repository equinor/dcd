import styled from "styled-components"
import Grid from "@mui/material/Grid2"
import { Icon, Typography } from "@equinor/eds-core-react"

export const EditorViewerContainer = styled(Grid) <{ $isSmallScreen: boolean }>`
    display: flex;
    justify-content: center;
    padding: 15px;
    margin-top: 35px;
    min-height: 500px;
    && {
        flex-direction: ${(props) => (props.$isSmallScreen ? "column" : "row")};
    }
`

export const EditorViewerContent = styled.div<{ $right?: boolean; $isSmallScreen?: boolean; }>`
    display: flex;
    flex-direction: column;
    justify-content: space-between;
    width: 100%;
    margin: ${(props) => (props.$right ? "0 0 0 50px" : "0 50px 0 0")};
    margin: ${(props) => (props.$isSmallScreen && "0")};
`

export const EditorViewerHeading = styled.div<{ $smallGap?: boolean; }>`
    display: flex;
    align-items: center;
    width: 100%;
    gap: ${(props) => (props.$smallGap ? "3px" : "10px")};
    margin-bottom: 15px;
`

export const PeopleContainer = styled.div<{ $orgChart?: boolean; }>`
    display: flex;
    margin: ${(props) => (props.$orgChart ? "0 0 0 0" : "20px 0 100px 0")};
    flex-direction: column;
    gap: 20px;
`

export const ClickableHeading = styled(Grid)`
    display: flex;
    align-items: center;
    gap: 10px;
    cursor: pointer;
    && {
        margin-top: 100px;
    }
`

export const ExternalAccessHeader = styled(Grid)`
    cursor: pointer;
    display: flex;
    align-items: center;
    gap: 8px;
    margin-top: 80px;
`

export const AccessGroupContainer = styled(Grid)`
    margin-top: 20px;
    display: flex;
    flex-direction: column;
    gap: 8px;
    
    && {
        min-width: 250px;
        width: 100%;
    }
`

export const ExternalLinkIcon = styled(Icon)`
    color: #007079;
    font-size: 16px;
`

export const AccessDescription = styled(Typography)`
    margin-bottom: 25px;
`

export const AccessLinkContainer = styled(Typography)`
    display: flex;
    align-items: center;
    gap: 4px;

`

export const OptionsContainer = styled(Grid)`
    width: 100%;
    margin-bottom: 80px;
`

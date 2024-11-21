import styled from "styled-components"
import Grid from "@mui/material/Grid"

export const EditorViewerContainer = styled(Grid) <{ $isSmallScreen: boolean }>`
    display: flex;
    justify-content: center;
    padding: 15px;
    margin-top: 35px;
    flex-direction: ${(props) => (props.$isSmallScreen ? "column" : "row")}!important;
`

export const EditorViewerContent = styled.div<{ $right?: boolean; $isSmallScreen?: boolean; }>`
    display: flex;
    flex-direction: column;
    // justify-content: space-between;
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
    margin-top: 100px!important;
    cursor: pointer;
`

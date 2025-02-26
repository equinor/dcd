import { Typography } from "@equinor/eds-core-react"
import Grid from "@mui/material/Grid2"
import { styled } from "styled-components"

export const CampaignHeader = styled.div <{ $isSmallScreen: boolean }>`
    display: flex;
    flex-direction: ${({ $isSmallScreen }) => ($isSmallScreen ? "column" : "row")};
    justify-content: space-between;
    align-items: center;
    margin: 10px;
    width: 100%;
`

export const LinkText = styled.div`
    display: flex;
    flex-direction: row;
    gap: 10px;
    margin-bottom: 10px;
`

export const CampaignLink = styled(Typography)`
    font-size: 18px;
    margin: 0;
    cursor: pointer;
`

export const CampaignHeaderTexts = styled.div`
    display: flex;
    flex-direction: column;
    width: 100%;
    gap: 20px;
`

export const FieldsAndDatePickerContainer = styled.div`
    display: flex;
    flex-direction: row;
    gap: 30px;
`

export const NameCell = styled.div`
    display: flex;
    flex-direction: column;
    gap: 2px;
    padding: 4px 0;
    height: 100%;
    justify-content: center;
`

export const CampaignTableContainer = styled.div`
    display: flex;
    flex-direction: column;
    width: 100%;
    margin-bottom: 75px;
`

export const CampaignFullWidthContainer = styled(Grid)`
    width: 100%;
`

export const CampaignInputsContainer = styled(Grid)`
    width: 100%;
    max-width: 600px;
`

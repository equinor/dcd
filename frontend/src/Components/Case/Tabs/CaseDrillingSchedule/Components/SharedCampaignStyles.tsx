import { Typography } from "@equinor/eds-core-react"
import { styled } from "styled-components"

export const CampaignHeader = styled.div <{ $isSmallScreen: boolean }>`
    display: flex;
    flex-direction: ${({ $isSmallScreen }) => ($isSmallScreen ? "column" : "row")};
    justify-content: space-between;
    align-items: center;
    margin: 20px 0;
    width: 100%;
`

export const LinkText = styled.div`
    display: flex;
    flex-direction: row;
    gap: 10px;
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

export const NameCell = styled.div`
    display: flex;
    flex-direction: column;
    gap: 2px;
    padding: 4px 0;
    height: 100%;
    justify-content: center;
`

export const MainText = styled.div`
    font-weight: 500;
    line-height: 1.2;
`

export const SubText = styled.div`
    font-size: 12px;
    color: #6F6F6F;
    font-weight: 400;
    line-height: 1.2;
`

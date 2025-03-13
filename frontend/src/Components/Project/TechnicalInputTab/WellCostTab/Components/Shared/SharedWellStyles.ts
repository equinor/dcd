import styled from "styled-components"
import { Table } from "@equinor/eds-core-react"

export const {
    Head, Body, Row, Cell,
} = Table

export const FullwidthTable = styled(Table)`
    width: 100%;
`

export const CostWithCurrency = styled.div`
    display: flex;
    align-items: baseline;
    gap: 5px;

    div {
        font-weight: normal;
        font-size: 10px;
        margin-top: -4px;
        color: #6F6F6F;
        letter-spacing: 0.5px;
    }
`

export const Section = styled.section`
    display: flex;
    flex-direction: column;
    gap: 30px;

    &:not(:first-child) {
        margin-top: 100px;
    }
`

export const SectionHeader = styled.div`
    display: flex;
    gap: 20px;
    align-items: center;
    justify-content: space-between;
`

import { Button } from "@equinor/eds-core-react"
import styled from "styled-components"

export const AssetHeader = styled.div`
    margin-bottom: 2rem;
    display: flex;

    > *:first-child {
        margin-right: 2rem;
    }
`

export const AssetViewDiv = styled.div`
    margin: 2rem;
    display: flex;
    flex-direction: column;
`

export const Wrapper = styled.div`
    display: flex;
    flex-direction: row;
`

export const WrapperColumn = styled.div`
    display: flex;
    flex-direction: column;
`

export const SaveButton = styled(Button)`
    margin-top: 5rem;
    margin-left: 2rem;
    &:disabled {
        margin-left: 2rem;
        margin-top: 5rem;
    }
`

export const Dg4Field = styled.div`
    margin-left: 1rem;
    margin-bottom: 2rem;
    width: 10rem;
    display: flex;
`
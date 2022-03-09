import styled from "styled-components"

export const AssetFormContainer = styled.form`
    width: 30rem;

    > *:not(:last-child) {
        margin-bottom: 1rem;
    }
`

export const AssetFormActionsContainer = styled.div`
    display: flex;
    align-items: baseline;

    > *:not(:last-child) {
        margin-right: 1rem;
    }
`

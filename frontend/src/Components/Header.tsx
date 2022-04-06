import styled from "styled-components"
import { Typography } from "@equinor/eds-core-react"

const Wrapper = styled.div`
    display: flex;
    flex-direction: row;
    border-bottom: 1px solid lightgrey;
    padding: 1.5rem 2rem;
`

const PageTitle = styled(Typography)`
    flex-grow: 1;
`

interface Props {
    name: string | undefined
}

function Header({ name }: Props) {
    return (
        <Wrapper>
            <PageTitle>DCD - Digital Concept Development</PageTitle>
            <Typography>
                Welcome to DCD
                {" "}
                <b>{name}</b>
                !
            </Typography>
        </Wrapper>
    )
}

export default Header

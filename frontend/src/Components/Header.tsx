import { useState } from "react"
import styled from "styled-components"
import { Typography, Button } from "@equinor/eds-core-react"
import CreateProjectView from "../Views/CreateProjectView"

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
    const [isOpen, setIsOpen] = useState(false)
    const openModal = () => {
        setIsOpen(true)
    }
    const closeModal = () => {
        setIsOpen(false)
    }
    return (
        <>
            <Wrapper>
                <PageTitle>DCD - Digital Concept Development</PageTitle>
                <Button onClick={openModal}>Create Project</Button>
                <Typography>
                    Welcome to DCD
                    {" "}
                    <b>{name}</b>
                    !
                </Typography>
            </Wrapper>
            <CreateProjectView isOpen={isOpen} closeModal={closeModal} />
        </>
    )
}

export default Header

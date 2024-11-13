import { Button, Typography } from "@equinor/eds-core-react"
import { PersonAvatar } from "@equinor/fusion-react-person"
import styled from "styled-components"

import { PersonDetails } from "@/Models/AccessManagement"

interface PersonProps {
    person: PersonDetails,
    handleRemovePerson: (person: PersonDetails) => void
}

const PersonInfo = styled.div`
    display: flex;
    flex-direction: row;
    gap: 10px;
    align-items: center;
    justify-content: space-between;
    border-bottom: 1px solid #e0e0e0;
    padding-bottom: 10px;
`

const Person = ({
    person,
    handleRemovePerson,
}: PersonProps) => (
    <PersonInfo style={{ marginBottom: 10 }}>
        <PersonAvatar azureId={person.azureId} />
        <Typography>{person.name}</Typography>
        <Button
            onClick={() => {
                console.log("clicked")
                handleRemovePerson(person)
            }}
        >
            Remove
        </Button>
    </PersonInfo>
)

export default Person

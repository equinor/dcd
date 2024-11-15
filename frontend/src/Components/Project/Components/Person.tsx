import { Button, Typography } from "@equinor/eds-core-react"
import { PersonAvatar, PersonInfo } from "@equinor/fusion-react-person"
import styled from "styled-components"

interface PersonProps {
    person: PersonInfo,
    handleRemovePerson: (person: PersonInfo) => void
}

const PersonInfoContainer = styled.div`
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
    <PersonInfoContainer style={{ marginBottom: 10 }}>
        <PersonAvatar azureId={person.azureId} />
        <Typography>{person.name}</Typography>
        <Button
            onClick={() => {
                handleRemovePerson(person)
            }}
        >
            Remove
        </Button>
    </PersonInfoContainer>
)

export default Person

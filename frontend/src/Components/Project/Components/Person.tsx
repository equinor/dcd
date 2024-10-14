import { Avatar, Button, Typography } from "@equinor/eds-core-react"
import { Grid } from "@mui/material"
import styled from "styled-components"

interface PersonProps {
    name: string
    email: string
    hideAction?: boolean
    action?: (name: string, email: string) => void
}

const StyledPerson = styled(Grid)`
    display: flex;
    align-items: center;
    justify-content: space-between;
    & > div {
        display: flex;
        align-items: center;
        & > div {
            margin-left: 10px;
        }
    }
`

const Person = ({
    name,
    email,
    hideAction,
    action,
}: PersonProps) => (
    <StyledPerson item>
        <div>
            <Avatar size={48} alt="person1" src="https://picsum.photos/400/400" />
            <div>
                <Typography variant="h6">
                    {name}
                </Typography>
                <Typography variant="body_short">
                    {email}
                </Typography>
            </div>
        </div>
        {!hideAction && action && <Button onClick={() => action(name, email)} color="danger" variant="ghost">Remove</Button>}
    </StyledPerson>
)

export default Person

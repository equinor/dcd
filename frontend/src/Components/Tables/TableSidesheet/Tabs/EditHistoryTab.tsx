import { Stack, Typography } from "@mui/material"
import styled from "styled-components"

const Container = styled.div`
    padding: 20px;
`

interface Props {

}

const EditHistoryTab: React.FC<Props> = () => (
    <Container>
        <Stack spacing={2}>
            <Typography variant="body2" color="textSecondary">
                No edit history available
            </Typography>
        </Stack>
    </Container>
)

export default EditHistoryTab

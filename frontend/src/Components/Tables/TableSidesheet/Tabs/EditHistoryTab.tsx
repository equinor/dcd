import { Stack, Typography } from "@mui/material"
import styled from "styled-components"
import { tokens } from "@equinor/eds-tokens"

const Container = styled.div`
    padding: 24px;
`

const PlaceholderItem = styled.div`
    padding: 16px;
    border-radius: 4px;
    background: ${tokens.colors.ui.background__light.rgba};
    display: flex;
    gap: 16px;
    align-items: flex-start;
    opacity: 0.5;
`

const Circle = styled.div`
    width: 32px;
    height: 32px;
    border-radius: 50%;
    background: ${tokens.colors.ui.background__medium.rgba};
`

const ActionInfo = styled.div`
    flex: 1;
`

const Bar = styled.div<{ width: string }>`
    height: 12px;
    width: ${(props) => props.width};
    background: ${tokens.colors.ui.background__medium.rgba};
    border-radius: 2px;
    margin: 4px 0;
`

const ComingSoonBanner = styled.div`
    text-align: center;
    padding: 16px;
    margin-bottom: 24px;
    background: ${tokens.colors.ui.background__light.rgba};
    border-radius: 4px;
    border: 1px dashed ${tokens.colors.interactive.primary__resting.rgba};
`

const EditHistoryTab = () => {
    const placeholderItems = [
        { id: "recent" },
        { id: "today" },
        { id: "yesterday" },
        { id: "last-week" },
    ]

    return (
        <Container>
            <ComingSoonBanner>
                <Typography variant="h6" color="primary" gutterBottom>
                    Coming Soon
                </Typography>
                <Typography variant="body2" color="textSecondary">
                    Edit history tracking is under development. You&apos;ll soon be able to see all changes made to this data.
                </Typography>
            </ComingSoonBanner>
            <Stack spacing={2}>
                {placeholderItems.map((item) => (
                    <PlaceholderItem key={item.id}>
                        <Circle />
                        <ActionInfo>
                            <Bar width="180px" />
                            <Bar width="90%" />
                            <Bar width="120px" />
                        </ActionInfo>
                    </PlaceholderItem>
                ))}
            </Stack>
        </Container>
    )
}

export default EditHistoryTab

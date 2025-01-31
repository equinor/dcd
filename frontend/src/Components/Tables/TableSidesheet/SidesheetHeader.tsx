import { Box, IconButton, Stack, Typography } from "@mui/material"
import { Close } from "@mui/icons-material"
import { grey } from "@mui/material/colors"
import styled from "styled-components"

const HeaderContainer = styled(Box)`
    padding: 56px 32px 24px;
    border-bottom: 1px solid ${({ theme }) => theme.palette?.divider ?? '#E0E0E0'};
`

const Title = styled(Typography)`
    font-size: 20px;
`

const CloseIconButton = styled(IconButton)`
    color: ${grey[600]};
`

const InfoLabel = styled(Typography)`
    color: ${grey[600]};
    text-transform: uppercase;
    letter-spacing: 0.5px;
`

const InfoValue = styled(Typography)`
    font-weight: 500;
`

interface Props {
    onClose: () => void
    title: string
    value: string
    year: string
    lastUpdated: string
    source: string
}

const InfoItem = ({ label, value }: { label: string; value: string }) => (
    <Stack spacing={0.5} alignItems="center">
        <InfoLabel variant="caption">
            {label}
        </InfoLabel>
        <InfoValue variant="body2">
            {value}
        </InfoValue>
    </Stack>
)

const SidesheetHeader = ({
    onClose,
    title,
    value,
    year,
    lastUpdated,
    source,
}: Props) => {
    return (
        <HeaderContainer>
            <Stack 
                direction="row" 
                justifyContent="space-between" 
                alignItems="center"
                sx={{ mb: 3 }}
            >
                <Title variant="h6">
                    {title}
                </Title>
                <CloseIconButton
                    onClick={onClose}
                    aria-label="Close"
                >
                    <Close />
                </CloseIconButton>
            </Stack>

            <Stack 
                direction="row" 
                justifyContent="space-between" 
                alignItems="center" 
                spacing={2}
            >
                <InfoItem label="Value" value={value} />
                <InfoItem label="Year" value={year} />
                <InfoItem label="Last updated" value={lastUpdated} />
                <InfoItem label="Source" value={source} />
            </Stack>
        </HeaderContainer>
    )
}

export default SidesheetHeader
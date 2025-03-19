import React from "react"
import styled from "styled-components"
import { Box, Button } from "@mui/material"
import { Typography, Card, Icon } from "@equinor/eds-core-react"
import { home } from "@equinor/eds-icons"
import { useNavigate } from "react-router-dom"
import { tokens } from "@equinor/eds-tokens"

const Container = styled.div`
    max-width: 800px;
    margin: 120px auto 0 auto;
    padding: 20px;
    text-align: center;
`

const StyledCard = styled(Card)`
    padding: 64px 32px;
    background: ${tokens.colors.ui.background__default.rgba};
    transition: transform 0.2s ease-in-out;

    &:hover {
        transform: translateY(-2px);
    }
`

const ErrorCode = styled(Typography)`
    font-size: 120px;
    font-weight: 700;
    line-height: 1;
    color: ${tokens.colors.interactive.danger__resting.rgba};
    margin-bottom: 16px;
`

const SubHeader = styled(Typography)`
    font-size: 32px;
    color: ${tokens.colors.text.static_icons__default.rgba};
    margin-bottom: 32px;
`

const ButtonContainer = styled.div`
    display: flex;
    justify-content: center;
    margin-top: 40px;
`

const StyledButton = styled(Button)`
    background-color: ${tokens.colors.interactive.primary__resting.rgba};
    padding: 12px 24px;
    border-radius: 4px;
    font-size: 16px;
    font-weight: 500;
    text-transform: none;
    transition: all 0.2s ease-in-out;
    box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1);
    min-width: 200px;
    display: inline-flex;
    align-items: center;
    gap: 8px;

    &:hover {
        background-color: ${tokens.colors.interactive.primary__hover.rgba};
        box-shadow: 0 4px 8px rgba(0, 0, 0, 0.15);
        transform: translateY(-1px);
    }

    &:active {
        transform: translateY(0);
        box-shadow: 0 1px 2px rgba(0, 0, 0, 0.1);
    }
`

const NotFoundView: React.FC = () => {
    const navigate = useNavigate()

    return (
        <Container>
            <StyledCard>
                <ErrorCode>404</ErrorCode>
                <SubHeader>Page Not Found</SubHeader>
                <Box mt={3}>
                    <Typography variant="body_long" style={{ color: tokens.colors.text.static_icons__tertiary.rgba }}>
                        The page you are looking for might have been removed, had its name changed, or is temporarily unavailable.
                    </Typography>
                </Box>
                <ButtonContainer>
                    <StyledButton
                        variant="contained"
                        onClick={() => navigate("/")}
                    >
                        <Icon data={home} size={24} />
                        Return to Home
                    </StyledButton>
                </ButtonContainer>
            </StyledCard>
        </Container>
    )
}

export default NotFoundView

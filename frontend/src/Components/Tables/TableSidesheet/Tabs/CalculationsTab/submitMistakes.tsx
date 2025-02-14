import React, { useState } from "react"
import {
    Box, Button, Typography, TextField, Collapse,
} from "@mui/material"
import { Icon } from "@equinor/eds-core-react"
import { info_circle } from "@equinor/eds-icons"
import styled from "styled-components"

// Create a div instead of extending Box to avoid MUI styling system conflicts
const InfoBar = styled.div`
    display: flex;
    align-items: center;
    gap: 8px;
    padding: 8px 20px;
    margin-bottom: "16px";
    border-radius: 4px;
    background: #f2f2f2;
    cursor: pointer;
    transition: transform 0.2s ease-in-out;

    &:hover {
        transform: scale(1.01) 
    }
`

const FeedbackForm = styled.div`
    background: #f2f2f2;
    padding: 20px;
    margin-bottom: 16px;
    border-radius: 0 0 4px 4px;
`
const ButtonContainer = styled.div`
    display: flex;
    justify-content: flex-end;
    gap: 8px;
    margin-top: 16px;
`

interface SubmitMistakesProps {
    profileType?: string
    rowData?: {
        resourceName?: string
        resourceType?: string
        [key: string]: any
    }
}

const SubmitMistakes: React.FC<SubmitMistakesProps> = ({ profileType, rowData }) => {
    const [showForm, setShowForm] = useState(false)
    const [explanation, setExplanation] = useState("")

    const handleClose = () => {
        setShowForm(false)
        setExplanation("")
    }

    const handleSendMail = () => {
        if (!explanation.trim()) {
            return
        }
        const subject = encodeURIComponent("Possible mistake in code explanation")
        const body = encodeURIComponent(
            `I believe there might be an issue with the AI-generated explanation.\nProfile: ${profileType || "N/A"}\n`
            + `Resource Name: ${rowData?.resourceName || "N/A"}\nResource Type: ${rowData?.resourceType || "N/A"}\n\nDetails:\n${explanation}\n`,
        )
        window.location.href = `mailto:agars@equinor.com?subject=${subject}&body=${body}`
        handleClose()
    }

    return (
        <Box>
            <InfoBar onClick={() => setShowForm(!showForm)}>
                <Icon
                    data={info_circle}
                    size={16}
                    color="#6F6F6F"
                />
                <Typography variant="body2" color="textSecondary">
                    AI-generated explanation - Click to report inaccuracies
                </Typography>
            </InfoBar>

            <Collapse in={showForm}>
                <FeedbackForm>
                    <TextField
                        label="What seems incorrect?"
                        placeholder="Please describe the potential issue..."
                        multiline
                        rows={3}
                        variant="outlined"
                        value={explanation}
                        onChange={(e) => setExplanation(e.target.value)}
                        fullWidth
                        size="small"
                    />
                    <ButtonContainer>
                        <Button
                            variant="outlined"
                            size="small"
                            onClick={handleClose}
                        >
                            Cancel
                        </Button>
                        <Button
                            variant="contained"
                            color="primary"
                            onClick={handleSendMail}
                            disabled={!explanation.trim()}
                            size="small"
                        >
                            Send Feedback
                        </Button>
                    </ButtonContainer>
                </FeedbackForm>
            </Collapse>
        </Box>
    )
}

export default SubmitMistakes

import {
    Button,
    Progress,
    Icon,
} from "@equinor/eds-core-react"
import React from "react"
import { cloud_download, check_circle_outlined, error_filled } from "@equinor/eds-icons"
import styled from "styled-components"

export type FeedbackStatus = "none" | "success" | "error";

export interface ActionButtonProps {
    isLoading: boolean;
    feedbackStatus: FeedbackStatus;
    onClick: (e?: React.MouseEvent<HTMLButtonElement>) => void;
    disabled: boolean;
    buttonText?: string;
}

const StyledButton = styled(Button)`
    white-space: nowrap;
    min-width: auto;
    padding: 0 8px;
`

const ButtonIcon = styled(Icon)`
    margin-right: 4px;
`

/**
 * ActionButton component that shows different states:
 * - Normal state: Primary button with cloud download icon and text
 * - Loading state: Button with spinner
 * - Success state: Button with green checkmark
 * - Error state: Button with red X
 * - Disabled state: Shows the button in disabled state
 */
const ActionButton: React.FC<ActionButtonProps> = ({
    isLoading,
    feedbackStatus,
    onClick,
    disabled,
    buttonText = "Import",
}) => {
    if (isLoading) {
        return (
            <Button
                variant="ghost_icon"
                disabled
                title="Importing data..."
            >
                <Progress.Circular size={24} />
            </Button>
        )
    }

    if (feedbackStatus === "success") {
        return (
            <Button
                variant="ghost_icon"
                disabled
                title="Success"
            >
                <Icon data={check_circle_outlined} size={24} color="#4BB748" />
            </Button>
        )
    }

    if (feedbackStatus === "error") {
        return (
            <Button
                variant="ghost_icon"
                disabled
                title="Error"
            >
                <Icon data={error_filled} size={24} color="#EB0000" />
            </Button>
        )
    }

    return (
        <StyledButton
            onClick={onClick}
            disabled={disabled}
            variant="contained"
            color="primary"
            title="Import data"
        >
            <ButtonIcon data={cloud_download} size={16} />
            {buttonText}
        </StyledButton>
    )
}

export default ActionButton

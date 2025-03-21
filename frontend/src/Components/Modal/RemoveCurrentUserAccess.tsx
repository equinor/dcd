import { Button, Typography } from "@equinor/eds-core-react"
import React from "react"
import styled from "styled-components"

import BaseModal from "./BaseModal"

const ModalContent = styled.div`
  padding: 1rem;
`

const ModalActions = styled.div`
  display: flex;
  justify-content: flex-end;
`

const ConfirmButton = styled(Button)`
  margin-left: 0.5rem;
`

interface RemoveCurrentUserAccessProps {
  isOpen: boolean
  onClose: () => void
  onConfirm: () => void
  isSwitch?: boolean
}

const RemoveCurrentUserAccess: React.FC<RemoveCurrentUserAccessProps> = ({
    isOpen,
    onClose,
    onConfirm,
    isSwitch = false,
}) => (
    <BaseModal
        isOpen={isOpen}
        title="Warning"
        size="sm"
        content={(
            <ModalContent>
                <Typography variant="body_long">
                    {isSwitch
                        ? "Changing your role to viewer will restrict your ability to edit this project. Are you sure you want to continue?"
                        : "Removing yourself from this project will result in loss of access. Are you sure you want to continue?"}
                </Typography>
            </ModalContent>
        )}
        actions={(
            <ModalActions>
                <Button
                    type="button"
                    variant="outlined"
                    onClick={onClose}
                >
                    No, cancel
                </Button>
                <ConfirmButton color="danger" onClick={onConfirm}>
                    Yes, continue
                </ConfirmButton>
            </ModalActions>
        )}
    />
)

export default RemoveCurrentUserAccess

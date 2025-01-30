import React from "react"
import { Button } from "@equinor/eds-core-react"
import styled from "styled-components"
import Modal from "./Modal"

const ModalContent = styled.div`
  padding: 1rem;
`

const ModalActions = styled.div`
  display: flex;
  justify-content: flex-end;
`

const DeleteButton = styled(Button)`
  margin-left: 0.5rem;
`

interface DeleteWellInUseModalProps {
  isOpen: boolean
  onClose: () => void
  onConfirm: () => void
}

const DeleteWellInUseModal: React.FC<DeleteWellInUseModalProps> = ({
  isOpen,
  onClose,
  onConfirm,
}) => {
  return (
    <Modal
      isOpen={isOpen}
      title="Warning"
      size="sm"
      content={
        <ModalContent>
          This well is used in one or more cases. Are you sure you want
          to delete it?
        </ModalContent>
      }
      actions={
        <ModalActions>
          <Button
            type="button"
            variant="outlined"
            onClick={onClose}
          >
            No, cancel
          </Button>
          <DeleteButton onClick={onConfirm}>
            Yes, delete
          </DeleteButton>
        </ModalActions>
      }
    />
  )
}

export default DeleteWellInUseModal

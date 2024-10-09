import React from "react"
import {
    Typography, Icon, Button,
} from "@equinor/eds-core-react"
import { exit_to_app } from "@equinor/eds-icons"
import Modal from "../Modal/Modal"

type CaseDropMenuProps = {
    isMenuOpen: boolean;
    setIsMenuOpen: React.Dispatch<React.SetStateAction<boolean>>;
};

const CaseDropMenu: React.FC<CaseDropMenuProps> = ({ isMenuOpen, setIsMenuOpen }) => {
    const exitRevisionView = () => {
        console.log("Exiting revision view")
    }

    const closeMenu = () => {
        setIsMenuOpen(false)
    }

    return (
        <Modal
            title="APx Rev 1. Revision details"
            size="sm"
            isOpen={isMenuOpen}
            content={(
                <Typography variant="body_short">
                    Revisions are copies of a project at a given point in time. Revisions are locked for editing.
                </Typography>
            )}
            actions={(
                <div>
                    <Button variant="ghost" onClick={() => exitRevisionView()}>
                        <Icon data={exit_to_app} />
                        Exit revision
                    </Button>
                    <Button onClick={() => closeMenu()}> Close details</Button>
                </div>
            )}
        />
    )
}

export default CaseDropMenu

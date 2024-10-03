import React, { useState } from "react"
import {
    Menu, Typography, Icon, Button,
} from "@equinor/eds-core-react"
import { add, exit_to_app } from "@equinor/eds-icons"
import { useProjectContext } from "../../Context/ProjectContext"
import Modal from "../Modal/Modal"

type RevisionsDropMenuProps = {
    isMenuOpen: boolean;
    setIsMenuOpen: React.Dispatch<React.SetStateAction<boolean>>;
};

const RevisionsDropMenu: React.FC<RevisionsDropMenuProps> = ({ isMenuOpen, setIsMenuOpen }) => {
    const { isRevision } = useProjectContext()

    const [creatingRevision, setCreatingRevision] = useState(false)

    const mockRevisionInfo = [
        {
            id: "1",
            name: "Revision 1",
            description: "This is the first revision",
            date: "2021-10-01",
        },
        {
            id: "2",
            name: "Revision 2",
            description: "This is the second revision",
            date: "2021-10-02",
        },
        {
            id: "3",
            name: "Revision 3",
            description: "This is the third revision",
            date: "2021-10-03",
        },
    ]

    const createRevision = () => {
        console.log("Creating revision")
        setCreatingRevision(true)
    }

    const exitRevisionView = () => {
        console.log("Exiting revision view")
    }

    return (
        <>
            <Modal
                title="Create revision"
                size="sm"
                isOpen={creatingRevision}
                content={(
                    <Typography variant="body_short">
                        Create revision
                    </Typography>
                )}
                actions={(
                    <div>
                        <Button variant="ghost" onClick={() => setCreatingRevision(false)}>Cancel</Button>
                        <Button onClick={() => createRevision()}> Create Reision </Button>
                    </div>
                )}
            />
            <Menu
                id="menu-complex"
                open={isMenuOpen}
                onClose={() => setIsMenuOpen(false)}
                placement="bottom"
            >
                {
                    mockRevisionInfo.map((revision) => (
                        <Menu.Item onClick={() => createRevision()}>
                            <Typography group="navigation" variant="menu_title" as="span">
                                {revision.name}
                                {" "}
                                -
                                {" "}
                                {revision.date}
                            </Typography>
                        </Menu.Item>
                    ))
                }
                {!isRevision ? (
                    <Menu.Item onClick={() => createRevision()}>
                        <Icon data={add} size={16} />
                        <Typography group="navigation" variant="menu_title" as="span">
                            Create new revision
                        </Typography>
                    </Menu.Item>
                ) : (
                    <Menu.Item onClick={() => exitRevisionView()}>
                        <Icon data={exit_to_app} size={16} />
                        <Typography group="navigation" variant="menu_title" as="span">
                            Exit revision view
                        </Typography>
                    </Menu.Item>
                )}
            </Menu>
        </>
    )
}

export default RevisionsDropMenu

import React, { useEffect, useState } from "react"
import {
    Menu, Typography, Icon,
} from "@equinor/eds-core-react"
import { add, exit_to_app } from "@equinor/eds-icons"
import { useModuleCurrentContext } from "@equinor/fusion-framework-react-module-context"
import { useQuery } from "@tanstack/react-query"

import { useProjectContext } from "@/Context/ProjectContext"
import { projectQueryFn } from "@/Services/QueryFunctions"
import { formatFullDate, truncateText } from "@/Utils/common"
import useEditDisabled from "@/Hooks/useEditDisabled"
import CreateRevisionModal from "../Modal/CreateRevisionModal"
import { useRevisions } from "@/Hooks/useRevision"

type RevisionsDropMenuProps = {
    isMenuOpen: boolean
    setIsMenuOpen: (isMenuOpen: boolean) => void
    menuAnchorEl: HTMLElement | null
    setIsRevisionMenuOpen?: React.Dispatch<React.SetStateAction<boolean>>
    isCaseMenu: boolean
}

interface Revision {
    id: string
    name: string
    description: string
    date: string
}

const RevisionsDropMenu: React.FC<RevisionsDropMenuProps> = ({
    isMenuOpen, setIsMenuOpen, menuAnchorEl, isCaseMenu, setIsRevisionMenuOpen,
}) => {
    const { isRevision } = useProjectContext()
    const {
        navigateToRevision,
        exitRevisionView,
        disableCurrentRevision,
    } = useRevisions()
    const { isEditDisabled } = useEditDisabled()

    const { currentContext } = useModuleCurrentContext()
    const externalId = currentContext?.externalId

    const [isRevisionModalOpen, setIsRevisionModalOpen] = useState(false)
    const [revisions, setRevisions] = useState<Revision[]>([])

    const { data: apiData } = useQuery({
        queryKey: ["projectApiData", externalId],
        queryFn: () => projectQueryFn(externalId),
        enabled: !!externalId,
    })

    useEffect(() => {
        if (apiData) {
            const revisionsResult = apiData.revisions.map((r: Components.Schemas.ProjectDto) => ({
                id: r.id,
                name: r.name,
                description: r.description,
                date: r.createDate,
            }))
            setRevisions(revisionsResult)
        }
    }, [apiData])

    const navToRevision = (revision: Revision) => {
        setIsMenuOpen(false)
        navigateToRevision(revision.id)
    }

    const exitRevision = () => {
        setIsMenuOpen(false)
        exitRevisionView()
    }

    return (
        <>
            <CreateRevisionModal
                isOpen={isRevisionModalOpen}
                setIsOpen={setIsRevisionModalOpen}
            />
            <Menu
                id="menu-complex"
                open={isMenuOpen}
                anchorEl={menuAnchorEl}
                onClose={() => (isCaseMenu && setIsRevisionMenuOpen ? setIsRevisionMenuOpen(false) : setIsMenuOpen(false))}
                placement={isCaseMenu ? "left" : "bottom"}
            >
                {
                    revisions.map((revision) => (
                        <Menu.Item
                            key={revision.id}
                            onClick={() => (isCaseMenu ? navToRevision(revision) : navigateToRevision(revision.id))}
                            disabled={disableCurrentRevision(revision.id)}
                        >
                            <Typography group="navigation" variant="menu_title" as="span">
                                {truncateText(revision.name, 50)}
                                {" "}
                                -
                                {" "}
                                {formatFullDate(revision.date)}
                            </Typography>
                        </Menu.Item>
                    ))
                }
                <Menu.Item
                    onClick={() => setIsRevisionModalOpen(true)}
                    disabled={isEditDisabled}
                >
                    <Icon data={add} size={16} />
                    <Typography group="navigation" variant="menu_title" as="span">
                        Create new revision
                    </Typography>
                </Menu.Item>
                <Menu.Item
                    onClick={() => (isCaseMenu ? exitRevision() : exitRevisionView())}
                    disabled={!isRevision}
                >
                    <Icon data={exit_to_app} size={16} />
                    <Typography group="navigation" variant="menu_title" as="span">
                        Exit revision view
                    </Typography>
                </Menu.Item>
            </Menu>
        </>
    )
}

export default RevisionsDropMenu

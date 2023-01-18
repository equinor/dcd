/* eslint-disable camelcase */
import React, { useEffect, useState } from "react"
import styled from "styled-components"
import { Link, useParams } from "react-router-dom"
import { bookmark_filled, bookmark_outlined, IconData } from "@equinor/eds-icons"

import { useCurrentContext } from "@equinor/fusion"
import { ProjectMenuItemType } from "./ProjectMenu"
import MenuItem from "./MenuItem"
import { CasePath } from "../../Utils/common"
import { Project } from "../../models/Project"
import { Icon } from "@equinor/eds-core-react"

const ExpandableDiv = styled.div`
    display: flex;
    flex-direction: column;
    padding: 0.25rem 0 0.25rem 2rem;
    cursor: pointer;
`

const SubItems = styled.ul`
    padding: 0;
    margin: 0;
    width: 100%;
`

const SubItem = styled.li`
    list-style-type: none;
    margin: 0;
    display: flex;
    flex-direction: column;
    padding: 0;
    cursor: pointer;
`

const LinkWithoutStyle = styled(Link)`
    text-decoration: none;
`

export type ProjectMenuItem = {
    name: string
    icon: IconData
}

interface Props {
    project: Project
    item: ProjectMenuItem
    projectId: string
    subItems?: Components.Schemas.CaseDto[]
}

function ProjectMenuItemComponent({
    item, projectId, subItems, project,
}: Props) {
    const { fusionContextId, caseId } = useParams<Record<string, string | undefined>>()
    const currentProject = useCurrentContext()

    // eslint-disable-next-line max-len
    const isSelectedProjectMenuItem = (item.name === ProjectMenuItemType.OVERVIEW && caseId === undefined)
        || (item.name === ProjectMenuItemType.CASES && caseId !== undefined)
    const isSelected = currentProject?.externalId === projectId && isSelectedProjectMenuItem
    const [isOpen, setIsOpen] = useState<boolean>(isSelected)

    useEffect(() => {
        setIsOpen(isSelected)
    }, [isSelected])

    const withReferenceCase = (c: any) => {
        if (project.referenceCaseId === c.id) {
            return bookmark_filled
        }
        return undefined
    }

    return (
        <ExpandableDiv>
            <MenuItem
                title={item.name}
                isSelected={isSelected}
                icon={item.icon}
                isOpen={isOpen}
                onClick={subItems ? () => setIsOpen(!isOpen) : undefined}
            />
            {subItems && isOpen && (
                <SubItems>
                    {subItems.map((subItem, index) => (
                        <SubItem key={`menu-sub-item-${index + 1}`}>
                            <nav>
                                <LinkWithoutStyle to={CasePath(
                                    currentProject?.id!,
                                    subItem.id ? subItem.id : "",
                                )}
                                >
                                    <MenuItem
                                        // eslint-disable-next-line max-len
                                        title={`${subItem.name}`}
                                        isSelected={isSelected && caseId === subItem.id}
                                        padding="0.25rem 2rem"
                                        referenceCaseIcon={withReferenceCase(subItem)}
                                    />
                                </LinkWithoutStyle>
                            </nav>
                        </SubItem>
                    ))}
                </SubItems>
            )}
        </ExpandableDiv>
    )
}

export default ProjectMenuItemComponent

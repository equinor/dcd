import React from 'react'
import styled from 'styled-components'
import { chevron_down, chevron_right, IconData } from '@equinor/eds-icons'
import { Icon, Typography } from '@equinor/eds-core-react'
import { tokens } from '@equinor/eds-tokens'

const Wrapper = styled.div<{ padding?: string }>`
    display: flex;
    flex-direction: row;
    align-items: center;
    padding: ${props => props.padding};
`

const MenuTitle = styled.div`
    display: flex;
    flex-direction: row;
    flex-grow: 1;
    align-items: center;
    user-select: none;
    padding: 0.5rem 0.5rem;
`

const MenuIcon = styled(Icon)`
    color: ${tokens.colors.text.static_icons__secondary.rgba};
    margin-right: 0.5rem;
`

interface Props {
    title: string
    isSelected: boolean
    icon?: IconData
    isOpen?: boolean
    onClick?: () => void
    padding?: string
}

const MenuItem = ({ title, isSelected, icon, isOpen, onClick, padding }: Props) => {
    const selectedColor = tokens.colors.infographic.primary__moss_green_100.rgba

    return (
        <Wrapper onClick={onClick} padding={padding}>
            <MenuTitle>
                {icon && <MenuIcon data={icon} color={isSelected ? selectedColor : ''}></MenuIcon>}
                <Typography color={isSelected ? selectedColor : ''}>{title}</Typography>
            </MenuTitle>
            {onClick !== undefined && <MenuIcon data={isOpen ? chevron_down : chevron_right}></MenuIcon>}
        </Wrapper>
    )
}

export default MenuItem

import { tokens } from "@equinor/eds-tokens"
import {
    useCallback, useEffect, FunctionComponent, useState,
} from "react"
import { FocusOn } from "react-focus-on"
import styled, { css } from "styled-components"
import { Portal } from "./Portal"

type ContainerProps = {
    anchorRect: DOMRect
}

const Container = styled.div<ContainerProps>`
    position: absolute;
    background-color: ${tokens.colors.ui.background__default};
    box-shadow: ${tokens.elevation.raised};
    border-radius: 4px;
    ${({ anchorRect }) => css`
        top: ${anchorRect.y + anchorRect.height}px;
        left: ${anchorRect.x}px;
    `}
`

type Props = {
    anchor: HTMLElement | null
    onDismiss?: (e: Event) => void
}

export const Popover: FunctionComponent<Props> = ({ anchor, onDismiss, children }) => {
    const [storedAnchor, setStoredAnchor] = useState<HTMLElement | null>(null)

    useEffect(() => {
        if (anchor) setStoredAnchor(anchor)

        return () => setStoredAnchor(null)
    }, [anchor])

    const getAnchorClientRect = useCallback(() => {
        if (!storedAnchor) return null
        return storedAnchor.getBoundingClientRect()
    }, [storedAnchor])

    if (!storedAnchor) return null

    return (
        <Portal>
            <FocusOn onClickOutside={onDismiss} onEscapeKey={onDismiss}>
                <Container anchorRect={getAnchorClientRect()!}>
                    {children}
                </Container>
            </FocusOn>
        </Portal>
    )
}

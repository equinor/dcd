import { FunctionComponent } from "react"
import { FocusOn } from "react-focus-on"
import styled from "styled-components"
import { Divider, Typography } from "@equinor/eds-core-react"
import { tokens } from "@equinor/eds-tokens"
import { Portal } from "./Portal"

export const ModalActionsContainer = styled.div`
    display: flex;
    align-items: baseline;

    > button:not(:last-child),
    > a:not(:last-child) {
        margin-right: 1rem;
    }
`

const Backdrop = styled.div`
    position: absolute;
    top: 0;
    left: 0;
    width: 100vw;
    height: 100vh;
    display: flex;
    justify-content: center;
    align-items: center;
    background-color: rgba(0, 0, 0, 0.4);
`

const Container = styled.div`
    background-color: ${tokens.colors.ui.background__default.hex};
    border-radius: 4px;
    box-shadow: ${tokens.elevation.above_scrim};
`

const Header = styled.div`
    padding: 1rem 1rem 0;
`

const Content = styled.div`
    padding: 0 1rem 1rem;
`

type Props = {
    title: string;
    onDismiss?: (e: Event) => void
}

export const Modal: FunctionComponent<Props> = ({ title, onDismiss, children }) => (
    <Portal>
        <Backdrop>
            <FocusOn onClickOutside={onDismiss} onEscapeKey={onDismiss}>
                <Container>
                    {title && (
                        <>
                            <Header>
                                <Typography variant="body_short_bold">{title}</Typography>
                            </Header>
                            <Divider />
                        </>
                    )}
                    <Content>{children}</Content>
                </Container>
            </FocusOn>
        </Backdrop>
    </Portal>
)

import { StarProgress } from "@equinor/eds-core-react"
import styled from "styled-components"
import { useEffect } from "react"

const Container = styled.div`
    display: flex;
    align-items: center;
    justify-content: center;
    gap: 5px;
    position: fixed;
    top: 0;
    left: 0;
    background-color: #ffffff7a;
    z-index: 1000;
    width: 100dvw;
    height: 100dvh;

`

const FullPageLoading = () => {
    // workaround to disable all focusable elements when full page loading is active.
    // todo: add project edits to queue system to avoid conflicting edits
    useEffect(() => {
        const activeElement = document.activeElement as HTMLElement
        if (activeElement && typeof activeElement.blur === "function") {
            activeElement.blur()
        }

        const disableFocusableElements = () => {
            const focusableElements = document.querySelectorAll(
                "input, select, textarea, button, [tabindex]:not([tabindex=\"-1\"])",
            )
            focusableElements.forEach((el) => {
                (el as HTMLElement).setAttribute("disabled", "true")
            })
        }

        const enableFocusableElements = () => {
            const focusableElements = document.querySelectorAll(
                "input, select, textarea, button, [tabindex]:not([tabindex=\"-1\"])",
            )
            focusableElements.forEach((el) => {
                (el as HTMLElement).removeAttribute("disabled")
            })
        }

        disableFocusableElements()

        const handleKeyDown = (event: any) => {
            event.preventDefault()
            event.stopPropagation()
        }

        const handleFocus = (event: any) => {
            event.preventDefault()
            event.stopPropagation()
            const active = document.activeElement as HTMLElement
            if (active && typeof active.blur === "function") {
                active.blur()
            }
        }

        document.addEventListener("keydown", handleKeyDown)
        document.addEventListener("focus", handleFocus, true)

        return () => {
            document.removeEventListener("keydown", handleKeyDown)
            document.removeEventListener("focus", handleFocus, true)
            enableFocusableElements()
        }
    }, [])

    return (
        <Container>
            <StarProgress size={48} />
        </Container>
    )
}

export default FullPageLoading

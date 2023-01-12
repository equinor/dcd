
import {
    Dispatch, SetStateAction, useEffect, useState, FunctionComponent,
} from "react"
import styled from "styled-components"
import {
    Button, Icon, Progress, Tabs, Typography,
} from "@equinor/eds-core-react"

const ModalDiv = styled.div`
    position: fixed;
    top: 50%;
    left: 50%;
    transform: translate(-50%, -50%);
    padding: 30px;
    z-index: 1000;
    background-color: white;
    border: 2px solid gray;
`

type Props = {
    profileName: string;
    isOpen: boolean;
    setIsOpen: Dispatch<SetStateAction<boolean>>;
}

export const OverrideTimeSeriesPrompt: FunctionComponent<Props> = ({
    isOpen, setIsOpen, profileName, children,
}) => {
    if (!isOpen) return null
    const toggleIsOpen = () => {
        // eslint-disable-next-line no-param-reassign
        setIsOpen(!isOpen)
    }
    return (
        <>
            <div style={{
                position: "fixed",
                top: 0,
                left: 0,
                right: 0,
                bottom: 0,
                backgroundColor: "rgba(0,0,0, .7)",
                zIndex: 1000,
            }}
            />
            <ModalDiv>
                {profileName && <Typography variant="h6">{profileName}</Typography>}
                <div>{children}</div>
                <p>
                    Are you sure you want to unlock the
                    <br />
                    {profileName}
                    ?
                    The cost will
                    <br />
                    no longer be calculated automatically
                </p>
                <Button
                    type="button"
                    variant="outlined"
                    onClick={toggleIsOpen}
                >
                    No, cancel
                </Button>
                <Button onClick={toggleIsOpen}>Yes, unlock</Button>
            </ModalDiv>
        </>
    )
}

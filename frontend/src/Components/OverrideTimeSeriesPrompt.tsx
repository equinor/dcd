
import {
    Dispatch, SetStateAction, FunctionComponent,
} from "react"
import styled from "styled-components"
import {
    Button, Typography,
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

const LockButton = styled(Button)`
    margin-left: 4rem;
`

type Props = {
    profileName: string;
    isOpen: boolean;
    setIsOpen: Dispatch<SetStateAction<boolean>>;
    setProfile: Dispatch<SetStateAction<any>> | undefined;
    profile: any
}

export const OverrideTimeSeriesPrompt: FunctionComponent<Props> = ({
    isOpen, setIsOpen, profileName, children, setProfile, profile,
}) => {
    if (!isOpen) return null
    const toggleIsOpen = () => {
        setIsOpen(!isOpen)
    }
    const toggleLock = () => {
        if (profile !== undefined && setProfile !== undefined) {
            const newProfile = { ...profile }
            newProfile.override = !profile.override
            console.log(newProfile)
            setProfile(newProfile)
        }
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
                backgroundColor: "rgba(0,0,0, 0.3)",
                zIndex: 1000,
            }}
            />
            <ModalDiv>
                {profileName && <Typography variant="h6">{profileName}</Typography>}
                <div>{children}</div>
                <p>
                    Are you sure you want to
                    {profile.override ? " lock " : " unlock "}
                    <br />
                    {profileName.toLowerCase()}
                    ?
                    The time series
                    <br />
                    {profile.override ? "be " : "no longer be "}
                    calculated automatically
                </p>
                <Button
                    type="button"
                    variant="outlined"
                    onClick={toggleIsOpen}
                >
                    No, cancel
                </Button>
                <LockButton onClick={toggleLock}>
                    Yes,
                    {profile.override ? " lock" : " unlock"}
                </LockButton>
            </ModalDiv>
        </>
    )
}

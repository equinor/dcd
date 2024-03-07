import { Dispatch, SetStateAction, FunctionComponent } from "react"
import { Button, Typography } from "@equinor/eds-core-react"
import Grid from "@mui/material/Grid"
import Modal from "./Modal/Modal"

type Props = {
    isOpen: boolean
    setIsOpen: Dispatch<SetStateAction<boolean>>
    profileName: string;
    setProfile: Dispatch<SetStateAction<any>> | undefined
    profile: any
}

export const OverrideTimeSeriesPrompt: FunctionComponent<Props> = ({
    isOpen,
    setIsOpen,
    profileName,
    setProfile,
    profile,
}) => {
    if (!isOpen) return null
    const toggleIsOpen = () => {
        setIsOpen(!isOpen)
    }
    const toggleLock = () => {
        if (profile !== undefined && setProfile !== undefined) {
            const newProfile = { ...profile }
            newProfile.override = !profile.override
            setProfile(newProfile)
        }
        setIsOpen(!isOpen)
    }
    return (
        <Modal isOpen={isOpen} title="Warning">
            <Grid container spacing={1}>
                <Grid item xs={12}>
                    <Typography>
                        Are you sure you want to
                        {profile.override ? " lock " : " unlock "}
                        <br />
                        {profileName.toLowerCase()}
                        ?
                        The time series will
                        <br />
                        {profile.override ? "be " : "no longer be "}
                        calculated automatically
                    </Typography>
                </Grid>
            </Grid>
            <Grid container spacing={1} justifyContent="flex-end">
                <Grid item>
                    <Button
                        type="button"
                        variant="outlined"
                        onClick={toggleIsOpen}
                    >
                        No, cancel
                    </Button>
                </Grid>
                <Grid item>
                    <Button onClick={toggleLock}>
                        {`Yes, ${profile.override ? "lock" : "unlock"}`}
                    </Button>
                </Grid>
            </Grid>
        </Modal>
    )
}

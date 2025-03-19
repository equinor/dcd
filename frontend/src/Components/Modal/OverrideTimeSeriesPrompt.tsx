import { Dispatch, SetStateAction } from "react"
import { Button, Typography } from "@equinor/eds-core-react"
import Grid from "@mui/material/Grid2"
import BaseModal from "./BaseModal"
import { useProjectContext } from "@/Store/ProjectContext"
import { ProfileTypes } from "@/Models/enums"
import { useTimeSeriesMutation } from "@/Hooks/Mutations"

type Props = {
    isOpen: boolean
    setIsOpen: Dispatch<SetStateAction<boolean>>
    profileType: ProfileTypes | undefined;
    profile: any
}

export const OverrideTimeSeriesPrompt: React.FC<Props> = ({
    isOpen,
    setIsOpen,
    profileType,
    profile,
}) => {
    const { updateProfileOverride } = useTimeSeriesMutation()
    const { projectId } = useProjectContext()

    if (!isOpen || !projectId || !profileType) { return null }
    const toggleIsOpen = () => {
        setIsOpen(!isOpen)
    }
    const toggleLock = () => {
        if (profile !== undefined) {
            updateProfileOverride(profile)
        }
        setIsOpen(!isOpen)
    }
    return (
        <BaseModal
            isOpen={isOpen}
            title="Warning"
            size="sm"
            content={(
                <Grid container spacing={1}>
                    <Grid size={12}>
                        <Typography>
                            Are you sure you want to
                            {profile.override ? " lock " : " unlock "}
                            <br />
                            {profileType.toLowerCase()}
                            ?
                            The time series will
                            <br />
                            {profile.override ? "be " : "no longer be "}
                            calculated automatically
                        </Typography>
                    </Grid>
                </Grid>
            )}
            actions={(
                <Grid container spacing={1} justifyContent="flex-end">
                    <Grid size={12}>
                        <Button
                            type="button"
                            variant="outlined"
                            onClick={toggleIsOpen}
                        >
                            No, cancel
                        </Button>
                    </Grid>
                    <Grid size={12}>
                        <Button onClick={toggleLock}>
                            {`Yes, ${profile.override ? "lock" : "unlock"}`}
                        </Button>
                    </Grid>
                </Grid>
            )}
        />

    )
}

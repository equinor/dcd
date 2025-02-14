import { Dispatch, SetStateAction } from "react"
import { Button, Typography } from "@equinor/eds-core-react"
import Grid from "@mui/material/Grid2"
import { useParams } from "react-router-dom"
import Modal from "./Modal"
import useEditCase from "@/Hooks/useEditCase"
import { useProjectContext } from "@/Store/ProjectContext"
import { ProfileTypes } from "@/Models/enums"

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
    const { addEdit } = useEditCase()
    const { caseId } = useParams()
    const { projectId } = useProjectContext()

    if (!isOpen || !projectId || !profileType) { return null }
    const toggleIsOpen = () => {
        setIsOpen(!isOpen)
    }
    const toggleLock = () => {
        const newResourceObject = structuredClone(profile)
        newResourceObject.override = !profile.override
        if (profile !== undefined) {
            addEdit({
                newDisplayValue: (!profile.override).toString(),
                previousDisplayValue: profile.override.toString(),
                inputLabel: profileType,
                projectId,
                resourceName: profile.resourceName,
                resourcePropertyKey: "override",
                caseId,
                resourceId: profile.resourceId,
                newResourceObject,
                previousResourceObject: profile,
            })
        }
        setIsOpen(!isOpen)
    }
    return (
        <Modal
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

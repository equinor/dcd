import { Dispatch, SetStateAction, FunctionComponent } from "react"
import { Button, Typography } from "@equinor/eds-core-react"
import Grid from "@mui/material/Grid"
import { useParams } from "react-router-dom"
import Modal from "./Modal"
import useDataEdits from "../../Hooks/useDataEdits"
import { useProjectContext } from "../../Context/ProjectContext"
import { ProfileNames } from "../../Models/Interfaces"

type Props = {
    isOpen: boolean
    setIsOpen: Dispatch<SetStateAction<boolean>>
    profileName: ProfileNames | undefined;
    profile: any
}

export const OverrideTimeSeriesPrompt: FunctionComponent<Props> = ({
    isOpen,
    setIsOpen,
    profileName,
    profile,
}) => {
    const { addEdit } = useDataEdits()
    const { project } = useProjectContext()
    const { caseId } = useParams()

    if (!isOpen || !project || !profileName) { return null }
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
                inputLabel: profileName,
                projectId: project.id,
                resourceName: profile.resourceName,
                resourcePropertyKey: "override",
                caseId,
                resourceId: profile.resourceId,
                newResourceObject,
                previousResourceObject: profile,
                resourceProfileId: profile.id,
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
            )}
            actions={(
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
            )}
        />

    )
}

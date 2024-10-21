import React, {
    ChangeEventHandler, FunctionComponent, useState,
} from "react"
import {
    Divider, Icon, Typography, Button,
    InputWrapper,
    TextField,
    NativeSelect,
    Chip,
} from "@equinor/eds-core-react"
import Dialog from "@mui/material/Dialog"
import DialogTitle from "@mui/material/DialogTitle"
import DialogContent from "@mui/material/DialogContent"
import DialogActions from "@mui/material/DialogActions"
import { checkbox_outline, info_circle } from "@equinor/eds-icons"
import styled from "styled-components"
import { Grid } from "@mui/material"
import { useQuery } from "@tanstack/react-query"
import { useModuleCurrentContext } from "@equinor/fusion-framework-react-module-context"
import { createRevision } from "@/Utils/RevisionUtils"
import { useProjectContext } from "@/Context/ProjectContext"
import { INTERNAL_PROJECT_PHASE, PROJECT_CLASSIFICATION } from "@/Utils/constants"
import { projectQueryFn } from "@/Services/QueryFunctions"

const Wrapper = styled.div`
    flex-direction: row;
    display: flex;
    margin-bottom: 10px;
`

const InfoIcon = styled(Icon)`
    margin-right: 5px;
    margin-top: -2px;
`

const ColumnWrapper = styled.div`
    flex-direction: column;
    margin-bottom: 15px;
`

type Props = {
    isOpen: boolean;
    size?: false | "xs" | "sm" | "md" | "lg" | "xl" | undefined;
    onClose?: () => void;
    setCreatingRevision: React.Dispatch<React.SetStateAction<boolean>>
}

const CreateRevisionModal: FunctionComponent<Props> = ({
    isOpen,
    size,
    onClose,
    setCreatingRevision,
}) => {
    const { projectId } = useProjectContext()
    const { currentContext } = useModuleCurrentContext()

    const [revisionName, setRevisionName] = useState<string>("")
    const [classification, setClassification] = useState<Components.Schemas.ProjectClassification>()
    const [internalProjectPhase, setInternalProjectPhase] = useState<Components.Schemas.InternalProjectPhase | undefined>()

    const externalId = currentContext?.externalId

    const { data: apiData } = useQuery({
        queryKey: ["projectApiData", externalId],
        queryFn: () => projectQueryFn(externalId),
        enabled: !!externalId,
    })

    const handleNameChange: ChangeEventHandler<HTMLInputElement> = async (e) => {
        setRevisionName(e.currentTarget.value)
    }

    const handleClassificationChange: ChangeEventHandler<HTMLSelectElement> = async (e) => {
        if ([0, 1, 2, 3].indexOf(Number(e.currentTarget.value)) !== -1) {
            const newClassification: Components.Schemas.ProjectClassification = Number(e.currentTarget.value) as unknown as Components.Schemas.ProjectClassification
            setClassification(newClassification)
        }
    }

    const handleInternalProjectPhaseChange: ChangeEventHandler<HTMLSelectElement> = async (e) => {
        if ([0, 1, 2].indexOf(Number(e.currentTarget.value)) !== -1) {
            const newInternalProjectPhase: Components.Schemas.InternalProjectPhase = Number(e.currentTarget.value) as unknown as Components.Schemas.InternalProjectPhase
            setInternalProjectPhase(newInternalProjectPhase)
        }
    }

    const internalProjectPhaseOptions = Object.entries(INTERNAL_PROJECT_PHASE).map(([key, value]) => (
        <option key={key} value={key}>{value.label}</option>
    ))

    const classificationOptions = Object.entries(PROJECT_CLASSIFICATION).map(([key, value]) => (
        <option key={key} value={key}>{value.label}</option>
    ))

    if (!apiData || !isOpen) { return null }

    const disableAfterDG0 = () => apiData?.projectPhase! >= 3

    const submitRevision = () => {
        const newRevision: Components.Schemas.CreateRevisionDto = {
            name: revisionName,
            internalProjectPhase: internalProjectPhase || apiData.internalProjectPhase,
            classification: classification || apiData.classification,
        }
        return newRevision
    }

    return (
        <Dialog
            open={isOpen}
            fullWidth
            maxWidth={size || "sm"}
            className="ConceptApp ag-theme-alpine-fusion"
            onClose={onClose}
        >
            <DialogTitle>
                <Typography variant="h5" as="p">Create new project revision</Typography>
                <Divider
                    style={{
                        width: "100%",
                        marginBottom: 4,
                        marginTop: 10,
                    }}
                />
            </DialogTitle>
            <DialogContent>
                <Wrapper>
                    <InfoIcon data={info_circle} size={18} />
                    <Typography group="ui" variant="chart">
                        Revisions are copies of a project at a given point in time. Revisions are locked for editing.
                    </Typography>
                </Wrapper>
                <Grid item xs={12} md={8}>
                    <ColumnWrapper>
                        <ColumnWrapper>
                            <InputWrapper labelProps={{ label: "Revision name" }}>
                                <TextField
                                    id="name"
                                    name="name"
                                    placeholder="Give the revision a fitting name"
                                    onChange={handleNameChange}
                                    value={revisionName}
                                />
                            </InputWrapper>
                        </ColumnWrapper>
                        <ColumnWrapper>
                            <NativeSelect
                                id="internalProjectPhase"
                                label="Project phase"
                                onChange={handleInternalProjectPhaseChange}
                                value={internalProjectPhase}
                                disabled={disableAfterDG0()}
                            >
                                {internalProjectPhaseOptions}
                            </NativeSelect>
                        </ColumnWrapper>
                        <ColumnWrapper>
                            <NativeSelect
                                id="projectClassification"
                                label="Project classification"
                                onChange={handleClassificationChange}
                                value={classification}
                            >
                                {classificationOptions}
                            </NativeSelect>
                        </ColumnWrapper>
                    </ColumnWrapper>
                </Grid>
                <Grid item xs={12} md={8}>
                    <ColumnWrapper>
                        <Typography>
                            Quality checks performed
                        </Typography>
                    </ColumnWrapper>
                    <Wrapper>
                        <Chip disabled>
                            <Icon data={checkbox_outline} />
                            MDQC
                        </Chip>
                        <Chip disabled>
                            <Icon data={checkbox_outline} />
                            Arena
                        </Chip>
                    </Wrapper>
                </Grid>
            </DialogContent>
            <DialogActions>
                <Grid container spacing={1} justifyContent="flex-end">
                    <Grid item>
                        <Button variant="ghost" onClick={() => setCreatingRevision(false)}>Cancel</Button>
                    </Grid>
                    <Grid item>
                        <Button onClick={() => createRevision(
                            projectId,
                            submitRevision() as Components.Schemas.CreateRevisionDto,
                            setCreatingRevision,
                        )}
                        >
                            Create revision
                        </Button>
                    </Grid>
                </Grid>
            </DialogActions>
        </Dialog>
    )
}

export default CreateRevisionModal

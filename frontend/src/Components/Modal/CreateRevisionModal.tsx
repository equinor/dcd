import React, {
    ChangeEventHandler, useEffect, useState,
} from "react"
import {
    Divider, Icon, Typography, Button,
    InputWrapper,
    TextField,
    NativeSelect,
    Chip,
    Progress,
} from "@equinor/eds-core-react"
import Dialog from "@mui/material/Dialog"
import DialogTitle from "@mui/material/DialogTitle"
import DialogContent from "@mui/material/DialogContent"
import DialogActions from "@mui/material/DialogActions"
import { checkbox, checkbox_outline, info_circle } from "@equinor/eds-icons"
import styled from "styled-components"
import Grid from "@mui/material/Grid2"

import { INTERNAL_PROJECT_PHASE, PROJECT_CLASSIFICATION } from "@/Utils/Config/constants"
import { useRevisions } from "@/Hooks/useRevision"
import { getProjectPhaseName } from "@/Utils/commonUtils"
import { useProjectContext } from "@/Store/ProjectContext"
import { useDataFetch } from "@/Hooks"

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
    size?: false | "xs" | "sm" | "md" | "lg" | "xl" | undefined;
}

const CreateRevisionModal: React.FC<Props> = ({
    size,
}) => {
    const revisionAndProjectData = useDataFetch()
    const { isCreateRevisionModalOpen, setIsCreateRevisionModalOpen } = useProjectContext()
    const {
        isRevisionsLoading,
        createRevision,
    } = useRevisions()

    const [revisionName, setRevisionName] = useState<string>("")
    const [classification, setClassification] = useState<Components.Schemas.ProjectClassification>(1)
    const [internalProjectPhase, setInternalProjectPhase] = useState<Components.Schemas.InternalProjectPhase>(
        revisionAndProjectData?.commonProjectAndRevisionData.internalProjectPhase ?? 0,
    )
    const [mdqc, setMdqc] = useState(false)
    const [arena, setArena] = useState(false)

    useEffect(() => {
        if (revisionAndProjectData?.commonProjectAndRevisionData.classification) {
            setClassification(revisionAndProjectData.commonProjectAndRevisionData.classification)
        }
    }, [revisionAndProjectData])

    const handleNameChange: ChangeEventHandler<HTMLInputElement> = async (e) => {
        setRevisionName(e.currentTarget.value.trimStart())
    }

    const handleClassificationChange: ChangeEventHandler<HTMLSelectElement> = async (e) => {
        if ([1, 2, 3].indexOf(Number(e.currentTarget.value)) !== -1) {
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

    if (!revisionAndProjectData || !isCreateRevisionModalOpen) { return null }

    const disableAfterDG0 = () => [3, 4, 5, 6, 7, 8].includes(revisionAndProjectData.commonProjectAndRevisionData.projectPhase)

    const internalProjectPhaseOptions = Object.entries(INTERNAL_PROJECT_PHASE).map(([key, value]) => {
        if (disableAfterDG0()) {
            return <option key={key}>{getProjectPhaseName(revisionAndProjectData.commonProjectAndRevisionData.projectPhase)}</option>
        }
        return <option key={key} value={key}>{value.label}</option>
    })

    const classificationOptions = Object.entries(PROJECT_CLASSIFICATION)
        .filter(([key]) => key !== "0")
        .map(([key, value]) => (
            <option key={key} value={key}>{value.label}</option>
        ))

    const submitRevision = () => {
        const newRevision: Components.Schemas.CreateRevisionDto = {
            name: revisionName,
            internalProjectPhase,
            classification,
            mdqc,
            arena,
        }
        createRevision(newRevision)
        setRevisionName("")
    }

    const closeModal = () => {
        setIsCreateRevisionModalOpen(false)
        setRevisionName("")
    }

    return (
        <Dialog
            open={isCreateRevisionModalOpen}
            fullWidth
            maxWidth={size || "sm"}
            className="ConceptApp ag-theme-alpine-fusion"
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
                <Grid size={{ xs: 12, md: 8 }}>
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
                            <InputWrapper
                                disabled
                                helperProps={disableAfterDG0() ? {
                                    icon: <Icon data={info_circle} size={16} />,
                                    text: "Project phases after DG0 are set in Common Library",
                                } : undefined}
                            >
                                <NativeSelect
                                    id="internalProjectPhase"
                                    label="Project phase"
                                    onChange={handleInternalProjectPhaseChange}
                                    value={internalProjectPhase}
                                    disabled={disableAfterDG0()}
                                >
                                    {internalProjectPhaseOptions}
                                </NativeSelect>
                            </InputWrapper>
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
                <Grid size={{ xs: 12, md: 8 }}>
                    <ColumnWrapper>
                        <Typography group="ui" variant="chart">
                            Quality checks performed
                        </Typography>
                    </ColumnWrapper>
                    <Wrapper>
                        <Grid container spacing={1} justifyContent="flex-start">
                            <Grid>
                                <Chip
                                    onClick={() => setMdqc(!mdqc)}
                                    variant={mdqc ? "active" : "default"}
                                >
                                    <Icon
                                        data={mdqc ? checkbox : checkbox_outline}
                                    />
                                    MDQC
                                </Chip>
                            </Grid>
                            <Grid>
                                <Chip
                                    onClick={() => setArena(!arena)}
                                    variant={arena ? "active" : "default"}
                                >
                                    <Icon
                                        data={arena ? checkbox : checkbox_outline}
                                    />
                                    Arena
                                </Chip>
                            </Grid>
                        </Grid>
                    </Wrapper>
                </Grid>
            </DialogContent>
            <DialogActions>
                <Grid container spacing={1} justifyContent="flex-end">
                    <Grid>
                        {!isRevisionsLoading ? (<Button variant="ghost" onClick={() => closeModal()}>Cancel</Button>) : null}
                    </Grid>
                    <Grid>
                        <Button disabled={isRevisionsLoading || revisionName === ""} onClick={() => submitRevision()}>
                            {isRevisionsLoading ? <Progress.Dots /> : "Create revision"}
                        </Button>
                    </Grid>
                </Grid>
            </DialogActions>
        </Dialog>
    )
}

export default CreateRevisionModal

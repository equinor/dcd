import React, {
    ChangeEventHandler, FunctionComponent, ReactNode, useState,
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
import { createRevision } from "@/Utils/RevisionUtils"
import { useProjectContext } from "@/Context/ProjectContext"

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
    if (!isOpen) { return null }
    const { projectId } = useProjectContext()

    const [revisionName, setRevisionName] = useState<string>("")

    const handleNameChange: ChangeEventHandler<HTMLInputElement> = async (e) => {
        setRevisionName(e.currentTarget.value)
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
                                id="projectPhase"
                                label="Project phase"
                            >
                                <option key={0} value={0}>APx1</option>
                                <option key={1} value={1}>APx2</option>
                            </NativeSelect>
                        </ColumnWrapper>
                        <ColumnWrapper>
                            <NativeSelect
                                id="projectClassification"
                                label="Project classification"
                            >
                                <option key={0} value={0}>Internal</option>
                                <option key={1} value={1}>Open</option>
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
                        <Chip>
                            <Icon data={checkbox_outline} />
                            MDQC
                        </Chip>
                        <Chip>
                            <Icon data={checkbox_outline} />
                            Arena
                        </Chip>
                    </Wrapper>
                </Grid>
            </DialogContent>
            <DialogActions>
                <div>
                    <Button variant="ghost" onClick={() => setCreatingRevision(false)}>Cancel</Button>
                    <Button onClick={() => createRevision(projectId, setCreatingRevision)}>Create revision</Button>
                </div>
            </DialogActions>
        </Dialog>
    )
}

export default CreateRevisionModal

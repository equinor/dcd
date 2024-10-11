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
            maxWidth={size || "lg"}
            className="ConceptApp ag-theme-alpine-fusion"
            onClose={onClose}
        >
            <DialogTitle><Typography variant="h2" as="p">Create new project revision revision</Typography></DialogTitle>
            <DialogContent>
                <Divider
                    style={{
                        width: "100%",
                    }}
                />
                <Wrapper>
                    <Icon data={info_circle} size={18} />
                    <Typography variant="body_short">
                        Revisions are copies of a project at a given point in time. Revisions are locked for editing.
                    </Typography>
                </Wrapper>
                <Grid item xs={12} md={8}>
                    <InputWrapper labelProps={{ label: "Revision name" }}>
                        <TextField
                            id="name"
                            name="name"
                            placeholder="Give the revision a fitting name"
                            onChange={handleNameChange}
                            value={revisionName}
                        />
                    </InputWrapper>
                    <NativeSelect
                        id="projectPhase"
                        label="Project phase"
                        // onChange={handleProductionStrategyChange}
                        // value={productionStrategy}
                    >
                        <option key={0} value={0}>APx1</option>
                        <option key={1} value={1}>APx2</option>
                    </NativeSelect>
                    <NativeSelect
                        id="projectClassification"
                        label="Project classification"
                        // onChange={handleProductionStrategyChange}
                        // value={productionStrategy}
                    >
                        <option key={0} value={0}>Internal</option>
                        <option key={1} value={1}>Open</option>
                    </NativeSelect>
                </Grid>
                <Grid item xs={12} md={8}>
                    <Typography>
                        Quality checks performed
                    </Typography>
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

import React, {
    ChangeEventHandler, useEffect, useState,
} from "react"
import {
    Typography,
    Icon,
    Button,
    InputWrapper,
    TextField,
    Chip,
    Divider,
} from "@equinor/eds-core-react"
import {
    checkbox, checkbox_outline, info_circle, close as closeIcon,
} from "@equinor/eds-icons"
import { useQueryClient } from "@tanstack/react-query"
import { useModuleCurrentContext } from "@equinor/fusion-framework-react-module-context"
import styled from "styled-components"
import { useParams } from "react-router-dom"
import DialogContent from "@mui/material/DialogContent"
import DialogTitle from "@mui/material/DialogTitle"
import DialogActions from "@mui/material/DialogActions"
import Dialog from "@mui/material/Dialog"
import Grid from "@mui/material/Grid2"

import { PROJECT_CLASSIFICATION, INTERNAL_PROJECT_PHASE } from "@/Utils/constants"
import { getProjectPhaseName } from "@/Utils/common"
import { formatFullDate } from "@/Utils/DateUtils"
import { GetProjectService } from "@/Services/ProjectService"
import { useProjectContext } from "@/Store/ProjectContext"
import { useDataFetch } from "@/Hooks"

type RevisionDetailsModalProps = {
    isMenuOpen: boolean;
    setIsMenuOpen: React.Dispatch<React.SetStateAction<boolean>>
};

const Wrapper = styled.div`
    display: flex;
    flex-direction: row;
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

const CloseRevision = styled.div`
    position: absolute;
    top: 8px;
    right: 8px;
    cursor: pointer;
    color: #007079;
`

const RevisionDetailsModal: React.FC<RevisionDetailsModalProps> = ({
    isMenuOpen,
    setIsMenuOpen,
}) => {
    const { currentContext } = useModuleCurrentContext()
    const externalId = currentContext?.externalId
    const { projectId } = useProjectContext()
    const queryClient = useQueryClient()
    const revisionAndProjectData = useDataFetch()
    const { revisionId } = useParams()

    const [revisionDetails, setRevisionDetails] = useState({
        revisionName: "",
        mdqc: false,
        arena: false,
    })

    const revisionData = revisionAndProjectData?.dataType === "revision"
        ? (revisionAndProjectData as Components.Schemas.RevisionDataDto)
        : null

    const [savedRevisionName, setSavedRevisionName] = useState<string>("")
    const [savedMdqc, setSavedMdqc] = useState<boolean>(false)
    const [savedArena, setSavedArena] = useState<boolean>(false)
    const [isRevisionModified, setIsRevisionModified] = useState(false)

    useEffect(() => {
        if (revisionData?.revisionDetails) {
            const { revisionName, mdqc, arena } = revisionData.revisionDetails
            setRevisionDetails({ revisionName, mdqc, arena })
            setSavedRevisionName(revisionName || "")
            setSavedMdqc(mdqc || false)
            setSavedArena(arena || false)
        }
    }, [revisionAndProjectData])

    const closeMenu = () => {
        setIsMenuOpen(false)
        setRevisionDetails({
            revisionName: savedRevisionName,
            mdqc: savedMdqc,
            arena: savedArena,
        })
        setIsRevisionModified(false)
    }

    const updateRevisionName = async () => {
        const { revisionName, mdqc, arena } = revisionDetails
        const updateRevisionDto = { name: revisionName, mdqc, arena }
        const updatedRevision = await GetProjectService().updateRevision(
            projectId,
            revisionId ?? "",
            updateRevisionDto,
        )
        if (updatedRevision) {
            setSavedRevisionName(revisionName)
            setSavedMdqc(mdqc)
            setSavedArena(arena)
            queryClient.invalidateQueries({ queryKey: ["revisionApiData", revisionId] })
            queryClient.invalidateQueries({ queryKey: ["projectApiData", externalId] })
            closeMenu()
        }
    }

    const checkIfModified = (
        newRevisionName: string,
        newMdqc: boolean,
        newArena: boolean,
    ) => {
        setIsRevisionModified(
            newRevisionName !== savedRevisionName
            || newMdqc !== savedMdqc
            || newArena !== savedArena,
        )
    }

    const handleRevisionNameChange: ChangeEventHandler<HTMLInputElement> = (e) => {
        const { value } = e.currentTarget
        setRevisionDetails((prevDetails) => ({
            ...prevDetails,
            revisionName: value,
        }))
        checkIfModified(value, revisionDetails.mdqc, revisionDetails.arena)
    }

    const handleQualityCheckToggle = (check: "mdqc" | "arena") => {
        setRevisionDetails((prevDetails) => ({
            ...prevDetails,
            [check]: !prevDetails[check],
        }))
        checkIfModified(
            revisionDetails.revisionName,
            check === "mdqc" ? !revisionDetails.mdqc : revisionDetails.mdqc,
            check === "arena" ? !revisionDetails.arena : revisionDetails.arena,
        )
    }

    if (!revisionAndProjectData) { return null }
    const isAfterDG0 = () => [3, 4, 5, 6, 7, 8].includes(revisionAndProjectData?.commonProjectAndRevisionData.projectPhase)

    const displayedPhase = isAfterDG0()
        ? getProjectPhaseName(revisionAndProjectData.commonProjectAndRevisionData.projectPhase)
        : INTERNAL_PROJECT_PHASE[revisionAndProjectData.commonProjectAndRevisionData.internalProjectPhase]?.label ?? "N/A"

    return (
        <Dialog
            open={isMenuOpen}
            onClose={closeMenu}
            maxWidth="sm"
            fullWidth
            className="ConceptApp ag-theme-alpine-fusion"
        >
            <DialogTitle>
                <Typography variant="h5" as="p">{`${savedRevisionName} revision details`}</Typography>
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
                <CloseRevision onClick={closeMenu}>
                    <Icon data={closeIcon} size={32} />
                </CloseRevision>
                <Grid size={{ xs: 12, md: 8 }}>
                    <ColumnWrapper>
                        <ColumnWrapper>
                            <InputWrapper labelProps={{ label: "Revision name" }}>
                                <TextField
                                    id="name"
                                    name="name"
                                    onChange={handleRevisionNameChange}
                                    value={revisionDetails.revisionName}
                                />
                            </InputWrapper>
                        </ColumnWrapper>
                        <ColumnWrapper>
                            <InputWrapper labelProps={{ label: "Created date" }}>
                                <Typography variant="body_short">
                                    {revisionData?.revisionDetails?.revisionDate
                                        ? formatFullDate(revisionData.revisionDetails.revisionDate)
                                        : "N/A"}
                                </Typography>
                            </InputWrapper>
                        </ColumnWrapper>
                        <ColumnWrapper>
                            <InputWrapper labelProps={{ label: "Project phase" }}>
                                <Typography variant="body_short">
                                    {displayedPhase}
                                </Typography>
                            </InputWrapper>
                        </ColumnWrapper>
                        <ColumnWrapper>
                            <InputWrapper labelProps={{ label: "Project classification" }}>
                                <Typography variant="body_short">
                                    {PROJECT_CLASSIFICATION[revisionAndProjectData?.commonProjectAndRevisionData.classification]?.label ?? "N/A"}
                                </Typography>
                            </InputWrapper>
                        </ColumnWrapper>
                    </ColumnWrapper>
                </Grid>
                <Grid size={{ xs: 12, md: 8 }}>
                    <ColumnWrapper>
                        <Typography>Quality checks performed</Typography>
                    </ColumnWrapper>
                    <Wrapper>
                        <Grid container spacing={1} justifyContent="flex-start">
                            <Grid>
                                <Chip
                                    onClick={() => handleQualityCheckToggle("mdqc")}
                                    variant={revisionDetails.mdqc ? "active" : "default"}
                                >
                                    <Icon data={revisionDetails.mdqc ? checkbox : checkbox_outline} />
                                    MDQC
                                </Chip>
                            </Grid>
                            <Grid>
                                <Chip
                                    onClick={() => handleQualityCheckToggle("arena")}
                                    variant={revisionDetails.arena ? "active" : "default"}
                                >
                                    <Icon data={revisionDetails.arena ? checkbox : checkbox_outline} />
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
                        <Button variant="outlined" onClick={closeMenu}>Cancel</Button>
                    </Grid>
                    <Grid>
                        <Button
                            disabled={!isRevisionModified}
                            onClick={updateRevisionName}
                        >
                            Close and Save
                        </Button>
                    </Grid>
                </Grid>
            </DialogActions>
        </Dialog>
    )
}

export default RevisionDetailsModal

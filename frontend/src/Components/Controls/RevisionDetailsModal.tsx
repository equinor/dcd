import React, {
    ChangeEventHandler, useEffect, useRef, useState,
} from "react"
import {
    Typography,
    Icon,
    Button,
    InputWrapper,
    TextField,
    Chip,
} from "@equinor/eds-core-react"
import { checkbox, checkbox_outline, info_circle } from "@equinor/eds-icons"
import { useQuery, useQueryClient } from "@tanstack/react-query"
import { useModuleCurrentContext } from "@equinor/fusion-framework-react-module-context"
import styled from "styled-components"
import { useParams } from "react-router-dom"
import DialogContent from "@mui/material/DialogContent"
import { Grid } from "@mui/material"
import DialogActions from "@mui/material/DialogActions"
import Modal from "../Modal/Modal"

import { formatFullDate } from "@/Utils/common"
import { revisionQueryFn } from "@/Services/QueryFunctions"
import { useProjectContext } from "@/Context/ProjectContext"
import { PROJECT_CLASSIFICATION, INTERNAL_PROJECT_PHASE } from "@/Utils/constants"
import { GetProjectService } from "@/Services/ProjectService"

type RevisionDetailsModalProps = {
    isMenuOpen: boolean;
    setIsMenuOpen: React.Dispatch<React.SetStateAction<boolean>>;
};

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
    display: flex;
    flex-direction: column;
    margin-bottom: 15px;
    gap: 8px;
`

const RevisionDetailsModal: React.FC<RevisionDetailsModalProps> = ({
    isMenuOpen,
    setIsMenuOpen,
}) => {
    const { currentContext } = useModuleCurrentContext()
    const externalId = currentContext?.externalId
    const { projectId } = useProjectContext()
    const queryClient = useQueryClient()
    const { revisionId } = useParams()

    const [revisionDetails, setRevisionDetails] = useState({
        revisionName: "",
        mdqc: false,
        arena: false,
    })

    const [savedRevisionName, setSavedRevisionName] = useState<string>("")
    const [savedMdqc, setSavedMdqc] = useState<boolean>(false)
    const [savedArena, setSavedArena] = useState<boolean>(false)
    const [isRevisionModified, setIsRevisionModified] = useState(false)

    const { data: revisionApiData } = useQuery({
        queryKey: ["revisionApiData", revisionId],
        queryFn: () => revisionQueryFn(projectId, revisionId),
        enabled: !!revisionId,
    })

    useEffect(() => {
        if (revisionApiData?.revisionDetails) {
            const { revisionName, mdqc, arena } = revisionApiData.revisionDetails
            setRevisionDetails({ revisionName, mdqc, arena })
            setSavedRevisionName(revisionName || "")
            setSavedMdqc(mdqc || false)
            setSavedArena(arena || false)
        }
    }, [revisionApiData])

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
        const projectService = await GetProjectService()
        const { revisionName, mdqc, arena } = revisionDetails
        const updateRevisionDto = { name: revisionName, mdqc, arena }
        const updatedRevision = await projectService.updateRevision(
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

    if (!revisionApiData) { return null }

    return (
        <Modal
            title={`${savedRevisionName} revision details`}
            size="sm"
            isOpen={isMenuOpen}
            content={(
                <DialogContent>
                    <Wrapper>
                        <InfoIcon data={info_circle} size={18} />
                        <Typography group="ui" variant="chart">
                            Revisions are copies of a project at a given point in time.
                            Revisions are locked for editing.
                        </Typography>
                    </Wrapper>
                    <Grid item xs={12} md={8}>
                        <ColumnWrapper>
                            <InputWrapper labelProps={{ label: "Revision name" }}>
                                <TextField
                                    id="name"
                                    name="name"
                                    onChange={handleRevisionNameChange}
                                    value={revisionDetails.revisionName}
                                />
                            </InputWrapper>
                            <InputWrapper labelProps={{ label: "Created Date" }}>
                                <Typography variant="body_short">
                                    {revisionApiData.revisionDetails?.revisionDate
                                        ? formatFullDate(revisionApiData.revisionDetails?.revisionDate)
                                        : "N/A"}
                                </Typography>
                            </InputWrapper>
                            <InputWrapper labelProps={{ label: "Project Phase" }}>
                                <Typography variant="body_short">
                                    {INTERNAL_PROJECT_PHASE[revisionApiData?.internalProjectPhase]?.label ?? "N/A"}
                                </Typography>
                            </InputWrapper>
                            <InputWrapper labelProps={{ label: "Project Classification" }}>
                                <Typography variant="body_short">
                                    {PROJECT_CLASSIFICATION[revisionApiData?.classification]?.label ?? "N/A"}
                                </Typography>
                            </InputWrapper>
                        </ColumnWrapper>
                    </Grid>

                    <Grid item xs={12} md={8}>
                        <ColumnWrapper>
                            <Typography>Quality checks performed</Typography>
                        </ColumnWrapper>
                        <Wrapper>
                            <Grid container spacing={1} justifyContent="flex-start">
                                <Grid item>
                                    <Chip
                                        onClick={() => handleQualityCheckToggle("mdqc")}
                                        variant={revisionDetails.mdqc ? "active" : "default"}
                                    >
                                        <Icon data={revisionDetails.mdqc ? checkbox : checkbox_outline} />
                                        MDQC
                                    </Chip>
                                </Grid>
                                <Grid item>
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
            )}
            actions={(
                <DialogActions>
                    <Button onClick={closeMenu}>Close details</Button>
                    <Button
                        disabled={!isRevisionModified}
                        onClick={updateRevisionName}
                    >
                        Close and Save
                    </Button>
                </DialogActions>
            )}
        />
    )
}

export default RevisionDetailsModal

import React, {
    ChangeEventHandler, useEffect, useRef, useState,
} from "react"
import {
    Typography, Icon, Button,
    Divider,
    InputWrapper,
    TextField,
    Chip,
} from "@equinor/eds-core-react"
import { checkbox_outline, exit_to_app, info_circle } from "@equinor/eds-icons"
import { useQuery, useQueryClient } from "@tanstack/react-query"
import { useModuleCurrentContext } from "@equinor/fusion-framework-react-module-context"
import styled from "styled-components"
import { useNavigate, useParams } from "react-router-dom"
import DialogContent from "@mui/material/DialogContent"
import { Grid } from "@mui/material"
import DialogActions from "@mui/material/DialogActions"
import Modal from "../Modal/Modal"

import { formatFullDate } from "@/Utils/common"
import { projectQueryFn, revisionQueryFn } from "@/Services/QueryFunctions"
import { exitRevisionView, updateRevisionName } from "@/Utils/RevisionUtils"
import { useProjectContext } from "@/Context/ProjectContext"
import useEditProject from "@/Hooks/useEditProject"
import { GetProjectService } from "@/Services/ProjectService"
import { PROJECT_CLASSIFICATION, INTERNAL_PROJECT_PHASE } from "@/Utils/constants"

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
    const [isNameChanged, setIsNameChanged] = useState(false)
    const { addProjectEdit } = useEditProject()
    const [revisionName, setRevisionName] = useState<string>("")

    const { revisionId } = useParams()

    const { data: projectApiData } = useQuery({
        queryKey: ["projectApiData", externalId],
        queryFn: () => projectQueryFn(externalId),
        enabled: !!externalId,
    })

    const { data: revisionApiData } = useQuery({
        queryKey: ["revisionApiData", revisionId],
        queryFn: () => revisionQueryFn(projectId, revisionId),
        enabled: !!revisionId,
    })

    useEffect(() => {
        setRevisionName(revisionApiData?.name || "")
    }, [revisionApiData])

    const closeMenu = () => {
        setIsMenuOpen(false)
    }

    const handleRevisionNameChange = async () => {
        if (revisionApiData && projectId && revisionId) {
            const updatedRevision = await updateRevisionName(projectId, revisionId, revisionName)
            if (updatedRevision) {
                addProjectEdit(updatedRevision.id, updatedRevision)
                closeMenu()
            }
        }
    }

    const handleNameInputChange: ChangeEventHandler<HTMLInputElement> = async (e) => {
        setRevisionName(e.currentTarget.value)
        setIsNameChanged(true)
    }

    const newClassification = PROJECT_CLASSIFICATION[revisionApiData?.classification as keyof typeof PROJECT_CLASSIFICATION]?.label
    const newInternalProjectPhase = revisionApiData?.internalProjectPhase as keyof typeof INTERNAL_PROJECT_PHASE

    if (!projectApiData && !revisionApiData) { return null }

    return (
        <Modal
            title={`${revisionName} revision details`}
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
                                    onChange={handleNameInputChange}
                                    value={revisionName}
                                />
                            </InputWrapper>
                            <InputWrapper labelProps={{ label: "Created Date" }}>
                                <Typography variant="body_short">
                                    {projectApiData?.createDate ? formatFullDate(projectApiData.createDate) : "N/A"}
                                </Typography>
                            </InputWrapper>
                            <InputWrapper labelProps={{ label: "Project Phase" }}>
                                <Typography variant="body_short">
                                    {INTERNAL_PROJECT_PHASE[newInternalProjectPhase]?.label ?? "N/A"}
                                </Typography>
                            </InputWrapper>

                            <InputWrapper labelProps={{ label: "Project Classification" }}>
                                <Typography variant="body_short">
                                    {newClassification ?? "N/A"}
                                </Typography>
                            </InputWrapper>
                        </ColumnWrapper>
                    </Grid>

                    <Grid item xs={12} md={8}>
                        <ColumnWrapper>
                            <Typography>Quality checks performed</Typography>
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
            )}
            actions={(
                <DialogActions>
                    <Button onClick={closeMenu}>Close details</Button>
                    <Button
                        disabled={!isNameChanged}
                        onClick={handleRevisionNameChange}
                    >
                        Close and Save
                    </Button>
                </DialogActions>
            )}
        />
    )
}

export default RevisionDetailsModal

/* eslint-disable camelcase */
import { add, delete_to_trash, edit } from "@equinor/eds-icons"
import {
    Button,
    EdsProvider,
    Icon,
    Tooltip,
    Typography,
} from "@equinor/eds-core-react"
import { useEffect, useRef, useState } from "react"
import { useParams } from "react-router-dom"
import styled from "styled-components"

import { Case } from "../../models/Case"
import { GetProjectService } from "../../Services/ProjectService"
import { AssetTypes } from "../../Components/CreateAssetForm/types"
import { Modal } from "../../Components/Modal"
import { CreateAssetForm } from "../../Components/CreateAssetForm"
import { CreateAssetPopover } from "./components/CreateAssetPopover"

const Wrapper = styled.div`
    margin: 2rem;
    display: flex;
    flex-direction: column;
`

const Header = styled.header`
    display: flex;
    align-items: center;

    > *:first-child {
        margin-right: 2rem;
    }
`

const ActionsContainer = styled.div`
    > *:not(:last-child) {
        margin-right: 0.5rem;
    }
`

function CaseView() {
    const params = useParams()

    const [caseItem, setCase] = useState<Case>()
    const [createAssetPopoverIsOpen, setCreateAssetPopoverIsOpen] = useState<boolean>(false)
    const [createAssetModalIsOpen, setCreateAssetModalIsOpen] = useState<boolean>(false)
    const [assetType, setAssetType] = useState<AssetTypes>()

    const popoverAnchorRef = useRef<HTMLButtonElement>(null)

    useEffect(() => {
        (async () => {
            try {
                const projectResult = await GetProjectService().getProjectByID(params.projectId!)
                const caseResult = projectResult.cases.find((o) => o.id === params.caseId)
                setCase(caseResult)
            } catch (error) {
                console.error(`[CaseView] Error while fetching project ${params.projectId}`, error)
            }
        })()
    }, [params.projectId, params.caseId])

    const openCreateAssetModal = (asset: AssetTypes) => {
        setAssetType(asset)
        setCreateAssetPopoverIsOpen(false)
        setCreateAssetModalIsOpen(true)
    }

    const closeCreateAssetModal = () => {
        setAssetType(undefined)
    }

    const toggleCreateAssetPopOver = () => setCreateAssetPopoverIsOpen(!createAssetPopoverIsOpen)

    if (!caseItem) return null

    return (
        <Wrapper>
            <Header>
                <Typography variant="h2">{caseItem.name}</Typography>

                <EdsProvider density="compact">
                    <ActionsContainer>
                        <Tooltip title={`Edit ${caseItem.name}`}>
                            <Button variant="ghost_icon" aria-label={`Edit ${caseItem.name}`}>
                                <Icon data={edit} />
                            </Button>
                        </Tooltip>
                        <Tooltip title="Add an asset">
                            <Button
                                variant="ghost_icon"
                                aria-label="Add an asset"
                                onClick={toggleCreateAssetPopOver}
                                ref={popoverAnchorRef}
                            >
                                <Icon data={add} />
                            </Button>
                        </Tooltip>
                        {createAssetPopoverIsOpen && (
                            <CreateAssetPopover
                                onAssetTypeClick={openCreateAssetModal}
                                onDismiss={closeCreateAssetModal}
                                popoverAnchor={popoverAnchorRef.current}
                            />
                        )}
                        <Tooltip title={`Delete ${caseItem.name}`}>
                            <Button
                                variant="ghost_icon"
                                color="danger"
                                aria-label={`Delete ${caseItem.name}`}
                            >
                                <Icon data={delete_to_trash} />
                            </Button>
                        </Tooltip>
                    </ActionsContainer>
                </EdsProvider>
            </Header>

            {(createAssetModalIsOpen && assetType != null) && (
                <Modal title="Create an asset">
                    <CreateAssetForm asset={assetType} onCancel={closeCreateAssetModal} />
                </Modal>
            )}
        </Wrapper>
    )
}

export default CaseView

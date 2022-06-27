import {
    ChangeEventHandler, MouseEventHandler, useEffect, useState,
} from "react"
import styled from "styled-components"
import {
    Button, Icon, TextField, Tooltip, Typography,
} from "@equinor/eds-core-react"
import { useNavigate, useParams } from "react-router"
import { add } from "@equinor/eds-icons"
import { Project } from "../models/Project"
import { unwrapProjectId } from "../Utils/common"
import { GetProjectService } from "../Services/ProjectService"
import { Modal } from "../Components/Modal"
import { GetWellService } from "../Services/WellService"
import { Well } from "../models/Well"
// import { Well } from "../models/Well"
// import { GetWellService } from "../Services/WellService"

const Wrapper = styled.div`
    display: flex;
    flex-direction: column;
`

const CreateWellForm = styled.form`
    width: 30rem;

    > * {
        margin-bottom: 1.5rem;
    }
`

function WellView() {
    const navigate = useNavigate()
    const [currentProject, setProject] = useState<Project>()
    const [createWellModalIsOpen, setCreateWellModalIsOpen] = useState<boolean>(false)
    const [wellName, setWellName] = useState<string>("")
    const [wells, setWells] = useState<Well[]>()
    const [hasChanges, setHasChanges] = useState<boolean>()
    const params = useParams()

    useEffect(() => {
        (async () => {
            try {
                const projectId: string = unwrapProjectId(params.projectId)
                const projectResult: Project = await GetProjectService().getProjectByID(projectId)
                setProject(projectResult)
            } catch (error) {
                console.error(`[WellView] Error while fetching project ${params.projectId}`, error)
            }
        })()
    }, [params.projectId])

    useEffect(() => {
        (async () => {
            try {
                const allWells = await GetWellService().getWellsByProjectId(params?.projectId!)
                setWells(allWells)
            } catch (error) {
                console.error(`[WellView] Error while fetching project ${params.projectId}`, error)
            }
        })()
    }, [hasChanges === true])

    const handleWellNameChange: ChangeEventHandler<HTMLInputElement> = (e) => {
        const { value } = e.target
        setWellName(value)
    }

    const toggleCreateWellModal = () => setCreateWellModalIsOpen(!createWellModalIsOpen)

    const submitCreateWellForm: MouseEventHandler<HTMLButtonElement> = async (e) => {
        e.preventDefault()

        try {
            // eslint-disable-next-line no-underscore-dangle
            const _wellService = GetWellService()
            const projectResult: Project = await _wellService.createWell({
                name: wellName,
                projectId: params.projectId,
                wellType: undefined,
                explorationWellType: undefined,
                wellInterventionCost: 0,
                plugingAndAbandonmentCost: 0,
                project: currentProject,
            })
            setHasChanges(true)
            toggleCreateWellModal()
            setHasChanges(false)
            navigate(`/project/${projectResult.id}/wells/`)
        } catch (error) {
            console.error("[ProjectView] error while submitting form data", error)
        }
    }

    return (
        <Wrapper>
            <Tooltip title="Add a well">
                <Button variant="ghost_icon" aria-label="Add a well" onClick={toggleCreateWellModal}>
                    <Icon data={add} />
                </Button>
            </Tooltip>
            <Typography variant="h3">Implementation of Wellview in progress</Typography>
            <Typography variant="h3">{currentProject?.id}</Typography>
            <Typography variant="h3">List of Wells:</Typography>
            {wells?.map((well) => (
                <Typography>
                    {well.name}
                </Typography>
            ))}
            <Modal isOpen={createWellModalIsOpen} title="Create a well" shards={[]}>
                <CreateWellForm>
                    <TextField
                        label="Name"
                        id="name"
                        name="name"
                        placeholder="Enter a name"
                        onChange={handleWellNameChange}
                    />

                    <div>
                        <Button
                            type="submit"
                            onClick={submitCreateWellForm}
                            disabled={wellName === ""}
                        >
                            Create well
                        </Button>
                        <Button
                            type="button"
                            color="secondary"
                            variant="ghost"
                            onClick={toggleCreateWellModal}
                        >
                            Cancel
                        </Button>
                    </div>
                </CreateWellForm>
            </Modal>
        </Wrapper>
    )
}

export default WellView

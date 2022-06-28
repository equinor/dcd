/* eslint-disable react/no-array-index-key */
import {
    ChangeEventHandler, MouseEventHandler, useEffect, useState,
} from "react"
import styled from "styled-components"
import {
    Button, Icon, Input, TextField, Tooltip, Typography,
} from "@equinor/eds-core-react"
import { useNavigate, useParams } from "react-router"
import { add } from "@equinor/eds-icons"
import { Project } from "../models/Project"
import { unwrapProjectId } from "../Utils/common"
import { GetProjectService } from "../Services/ProjectService"
import { Modal } from "../Components/Modal"
import { GetWellService } from "../Services/WellService"
import { Well } from "../models/Well"
import WellTypeCategory from "../Components/WellTypeCategory"
// import NumberInput from "../Components/NumberInput"
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
    const [wellTypeName, setWellTypeName] = useState<string>("")
    const [wellInterventionCost, setWellInterventionCost] = useState<number | undefined>()
    const [wellTypeCost, setWellTypeCost] = useState<number | undefined>()
    const [wellTypeDrillingDays, setWellTypeDrillingDays] = useState<number | undefined>()
    const [plugingAndAbandonmentCost, setPlugingAndAbandonmentCost] = useState<number | undefined>()
    const [wells, setWells] = useState<Well[]>()
    const [hasChanges, setHasChanges] = useState<boolean>()
    const [wellTypeCategory, setWellTypeCategory] = useState<Components.Schemas.WellTypeCategory | undefined>()
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

    const handleWellTypeNameChange: ChangeEventHandler<HTMLInputElement> = (e) => {
        const { value } = e.target
        setWellTypeName(value)
    }

    const onChangeWellTypeDrillingDays: ChangeEventHandler<HTMLInputElement> = async (e) => {
        setWellTypeDrillingDays(Number(e.target.value))
    }

    const onChangeWellTypeCost: ChangeEventHandler<HTMLInputElement> = async (e) => {
        setWellTypeCost(Number(e.target.value))
    }

    const onChangePlugingAndAbandonmentCost: ChangeEventHandler<HTMLInputElement> = async (e) => {
        setPlugingAndAbandonmentCost(Number(e.target.value))
    }

    const onChangeWellInterventionCost: ChangeEventHandler<HTMLInputElement> = async (e) => {
        setWellInterventionCost(Number(e.target.value))
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
                wellType: {
                    name: wellTypeName,
                    category: wellTypeCategory,
                    drillingDays: wellTypeDrillingDays,
                    wellCost: wellTypeCost,
                    description: undefined,
                },
                explorationWellType: undefined,
                wellInterventionCost: wellInterventionCost ?? 0,
                plugingAndAbandonmentCost: plugingAndAbandonmentCost ?? 0,
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
            {wells?.map((well, index) => (
                // eslint-disable-next-line react/jsx-no-comment-textnodes
                <>
                    <li key={index}>{well.name}</li>
                    <Wrapper>
                        <Typography>{`Well intervention cost: ${well.wellInterventionCost}`}</Typography>
                        <Typography>{`Pluging and abandonment cost: ${well.plugingAndAbandonmentCost}`}</Typography>
                        <Typography variant="h4">Well type:</Typography>
                        <Typography>{`Well type name: ${well.wellType?.name}`}</Typography>
                        <Typography>{`Well type cost: ${well.wellType?.wellCost}`}</Typography>
                        <Typography>{`Well type drilling days: ${well.wellType?.drillingDays}`}</Typography>
                    </Wrapper>
                </>
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
                        <label htmlFor="wellinterventioncost">Well intervention cost</label>
                        <Input
                            id="wellinterventioncost"
                            type="number"
                            onChange={onChangeWellInterventionCost}
                            value={wellInterventionCost ?? 0}
                        />
                        <label htmlFor="plugingandabandonmentcost">Pluging and abandonment cost</label>
                        <Input
                            id="plugingandabandonmentcost"
                            type="number"
                            onChange={onChangePlugingAndAbandonmentCost}
                            value={plugingAndAbandonmentCost ?? 0}
                        />
                    </div>
                    <Typography variant="h3">Well type</Typography>
                    <WellTypeCategory
                        // label="Welltype category"
                        // id="welltypecategory"
                        // placeholder="pick a category"
                        // onChange
                        currentValue={wellTypeCategory}
                        setWellCategory={setWellTypeCategory}
                    />
                    <TextField
                        label="Well type name"
                        id="name"
                        name="name"
                        placeholder="Enter a name"
                        onChange={handleWellTypeNameChange}
                    />
                    <div>
                        <label htmlFor="welltypewellcost">Well type cost</label>
                        <Input
                            id="welltypewellcost"
                            type="number"
                            onChange={onChangeWellTypeCost}
                            value={wellTypeCost ?? 0}
                        />
                        <label htmlFor="welltypedrillingdays">Well type drilling days</label>
                        <Input
                            id="welltypedrillingdays"
                            type="number"
                            onChange={onChangeWellTypeDrillingDays}
                            value={wellTypeDrillingDays ?? 0}
                        />
                    </div>
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

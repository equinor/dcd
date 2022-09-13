import {
    Button,
    TextField,
    Input,
    Label,
} from "@equinor/eds-core-react"
import {
    useState,
    Dispatch,
    SetStateAction,
    ChangeEventHandler,
    MouseEventHandler,
} from "react"
import { useParams } from "react-router-dom"
import styled from "styled-components"
import { Project } from "../../models/Project"
import { Case } from "../../models/case/Case"
import { ModalNoFocus } from "../ModalNoFocus"
import { ToMonthDate } from "../../Utils/common"
import { GetCaseService } from "../../Services/CaseService"

const CreateCaseForm = styled.form`
    width: 50rem;
`

interface Props {
    setProject: Dispatch<SetStateAction<Project | undefined>>
    project: Project
    caseItem: Case | undefined,
    isOpen: boolean
    toggleModal: () => void
}

const EditCaseModal = ({
    setProject,
    project,
    caseItem,
    isOpen,
    toggleModal,
}: Props) => {
    const { fusionContextId } = useParams<Record<string, string | undefined>>()
    const [caseNameModalIsOpen, setCaseNameModalIsOpen] = useState<boolean>(false)
    const [caseName, setCaseName] = useState<string>(caseItem?.name ?? "")
    const [dG4Date, setDG4Date] = useState<Date>()
    const [description, setDescription] = useState<string>()
    const [productionStrategy, setProductionStrategy] = useState<Components.Schemas.ProductionStrategyOverview>()
    const [producerCount, setProducerWells] = useState<number>()
    const [gasInjectorCount, setGasInjectorWells] = useState<number>()
    const [waterInjectorCount, setWaterInjectorWells] = useState<number>()

    // const toggleEditCaseNameModal = () => setCaseNameModalIsOpen(!caseNameModalIsOpen)

    // const handleCaseNameChange: ChangeEventHandler<HTMLInputElement> = async (e) => {
    //     setCaseName(e.target.value)
    // }

    // const submitUpdateCaseName = async () => {
    //     const unwrappedCase: Case = unwrapCase(caseItem)
    //     const caseDto = Case.Copy(unwrappedCase)
    //     caseDto.name = caseName
    //     const newProject = await (await GetCaseService()).updateCase(caseDto)
    //     setProject(newProject)
    //     const caseResult = unwrapCase(newProject.cases.find((o) => o.id === _caseId))
    //     setCase(caseResult)
    // }

    // const submitEditCaseNameForm: MouseEventHandler<HTMLButtonElement> = async (e) => {
    //     e.preventDefault()

    //     try {
    //         submitUpdateCaseName()
    //         toggleEditCaseNameModal()
    //     } catch (error) {
    //         console.error("[CaseView] error while submitting form data", error)
    //     }
    // }

    const submitCreateCaseForm: MouseEventHandler<HTMLButtonElement> = async (e) => {
        e.preventDefault()

        try {
            const projectResult: Project = await (await GetCaseService()).createCase({
                projectId: project.projectId,
                name: caseName,
                description,
                dG4Date: dG4Date?.toJSON(),
                producerCount,
                gasInjectorCount,
                waterInjectorCount,
            })
            setProject(projectResult)
            toggleModal()
        } catch (error) {
            console.error("[ProjectView] error while submitting form data", error)
        }
    }

    const handleNameChange: ChangeEventHandler<HTMLInputElement> = async (e) => {
        setCaseName(e.currentTarget.value)
    }

    const handleDescriptionChange: ChangeEventHandler<HTMLInputElement> = async (e) => {
        setDescription(e.currentTarget.value)
    }

    return (
        <ModalNoFocus isOpen={isOpen} title="Create a case">
            <CreateCaseForm>
                <TextField
                    label="Name"
                    id="name"
                    name="name"
                    placeholder="Enter a name"
                    onChange={handleNameChange}
                />

                <Input
                    type="month"
                    defaultValue={ToMonthDate(dG4Date)}
                    id="dgDate"
                    name="dgDate"
                    onChange={(e) => setDG4Date(new Date(e.currentTarget.value))}
                />

                <TextField
                    label="Description"
                    id="description"
                    name="description"
                    placeholder="Enter a description"
                    onChange={handleDescriptionChange}
                />
                <Label htmlFor="producerWells" label="Producer wells" />
                <Input
                    id="producerWells"
                    type="number"
                    value={producerCount}
                    disabled={false}
                    onChange={(e) => setProducerWells(Number(e.currentTarget.value))}
                    onKeyPress={(e) => {
                        if (!/\d/.test(e.key)) {
                            e.preventDefault()
                        }
                    }}
                />
                <Label htmlFor="gasInjector" label="Gas injector wells" />
                <Input
                    id="gasInjector"
                    type="number"
                    value={gasInjectorCount}
                    disabled={false}
                    onChange={(e) => setGasInjectorWells(Number(e.currentTarget.value))}
                    onKeyPress={(e) => {
                        if (!/\d/.test(e.key)) {
                            e.preventDefault()
                        }
                    }}
                />
                <Label htmlFor="waterInjector" label="Water injector wells" />
                <Input
                    id="waterInjector"
                    type="number"
                    value={waterInjectorCount}
                    disabled={false}
                    onChange={(e) => setWaterInjectorWells(Number(e.currentTarget.value))}
                    onKeyPress={(e) => {
                        if (!/\d/.test(e.key)) {
                            e.preventDefault()
                        }
                    }}
                />
                <div>
                    <Button
                        type="submit"
                        onClick={submitCreateCaseForm}
                        disabled={false}
                    >
                        Create case
                    </Button>
                    <Button
                        type="button"
                        color="secondary"
                        variant="ghost"
                        onClick={() => console.log("yee")}
                    >
                        Cancel
                    </Button>
                </div>
            </CreateCaseForm>
        </ModalNoFocus>
    )
}

export default EditCaseModal

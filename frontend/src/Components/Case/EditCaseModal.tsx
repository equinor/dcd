import {
    Button,
    TextField,
    Input,
    Label,
    NativeSelect,
} from "@equinor/eds-core-react"
import {
    useState,
    Dispatch,
    SetStateAction,
    ChangeEventHandler,
    MouseEventHandler,
    useEffect,
} from "react"
import { useHistory, useParams } from "react-router-dom"
import styled from "styled-components"
import TextArea from "@equinor/fusion-react-textarea/dist/TextArea"
import { Project } from "../../models/Project"
import { Case } from "../../models/case/Case"
import { ModalNoFocus } from "../ModalNoFocus"
import { DefaultDate, ToMonthDate } from "../../Utils/common"
import { GetCaseService } from "../../Services/CaseService"

const CreateCaseForm = styled.form`
    width: 596px;
`

const NameField = styled.div`
    width: 412px;
    margin-bottom: 21px;
    margin-right: 20px;
`

const DateField = styled.div`
    width: 120;
`

const RowWrapper = styled.div`
    display: flex;
    flex-direction: row;
`

const ColumnWrapper = styled.div`
    display: flex;
    flex-direction: column;
`

const ProductionStrategyOverviewField = styled.div`
    margin-top: 27px;
    margin-bottom: 34px;
`

const WellCountField = styled.div`
    margin-right: 25px;
    margin-bottom: 45px;
`

const CreateButton = styled(Button)`
    width: 150px;
`

const CreateButtonWrapper = styled.div`
    margin-left: 20px;
`

const ButtonsWrapper = styled.div`
    display: flex;
    flex-direction: row;
    margin-left: 350px;
`

interface Props {
    setProject: Dispatch<SetStateAction<Project | undefined>>
    project: Project
    caseId?: string,
    isOpen: boolean
    toggleModal: () => void
    editMode: boolean
}

const EditCaseModal = ({
    setProject,
    project,
    caseId,
    isOpen,
    toggleModal,
    editMode,
}: Props) => {
    const { fusionContextId } = useParams<Record<string, string | undefined>>()
    const [caseName, setCaseName] = useState<string | undefined>()
    const [dG4Date, setDG4Date] = useState<Date>(DefaultDate())
    const [description, setDescription] = useState<string | undefined>()
    const [productionStrategy, setProductionStrategy] = useState<Components.Schemas.ProductionStrategyOverview>()
    const [producerCount, setProducerWells] = useState<number>()
    const [gasInjectorCount, setGasInjectorWells] = useState<number>()
    const [waterInjectorCount, setWaterInjectorWells] = useState<number>()

    const [caseItem, setCaseItem] = useState<Case>()

    const history = useHistory()

    useEffect(() => {
        const dG4DefaultDate = new Date(Date.UTC(2030, 0, 1))

        setCaseName(caseItem?.name)
        setDG4Date(caseItem?.DG4Date ?? dG4DefaultDate)
        setDescription(caseItem?.description)
        setProductionStrategy(caseItem?.productionStrategyOverview ?? 0)
        setProducerWells(caseItem?.producerCount ?? 0)
        setGasInjectorWells(caseItem?.gasInjectorCount ?? 0)
        setWaterInjectorWells(caseItem?.waterInjectorCount ?? 0)
    }, [isOpen, caseId])

    useEffect(() => {
        const newCase = project.cases.find((c) => c.id === caseId)
        setCaseItem(newCase)
    }, [project, caseId])

    const handleNameChange: ChangeEventHandler<HTMLInputElement> = async (e) => {
        setCaseName(e.currentTarget.value)
    }

    const handleDescriptionChange: ChangeEventHandler<HTMLInputElement> = async (e) => {
        setDescription(e.currentTarget.value)
    }

    const handleProductionStrategyChange: ChangeEventHandler<HTMLSelectElement> = async (e) => {
        if ([0, 1, 2, 3, 4].indexOf(Number(e.currentTarget.value)) !== -1) {
            // eslint-disable-next-line max-len
            const newProductionStrategy: Components.Schemas.ProductionStrategyOverview = Number(e.currentTarget.value) as Components.Schemas.ProductionStrategyOverview
            setProductionStrategy(newProductionStrategy)
        }
    }

    const submitCaseForm: MouseEventHandler<HTMLButtonElement> = async (e) => {
        e.preventDefault()

        try {
            let projectResult: Project
            if (editMode && caseItem) {
                const newCase = Case.Copy(caseItem)
                newCase.name = caseName
                newCase.description = description
                newCase.DG4Date = dG4Date
                newCase.producerCount = producerCount
                newCase.gasInjectorCount = gasInjectorCount
                newCase.waterInjectorCount = waterInjectorCount
                newCase.productionStrategyOverview = productionStrategy ?? 0
                projectResult = await (await GetCaseService()).updateCase(
                    newCase,
                )
            } else {
                projectResult = await (await GetCaseService()).createCase({
                    projectId: project.projectId,
                    name: caseName,
                    description,
                    dG4Date: dG4Date.toJSON(),
                    producerCount,
                    gasInjectorCount,
                    waterInjectorCount,
                    productionStrategyOverview: productionStrategy,
                })
                history.push(`/${fusionContextId}/case/${projectResult.cases.find((o) => (
                    o.name === caseName
                ))?.id}`)
            }
            setProject(projectResult)
            toggleModal()
        } catch (error) {
            console.error("[ProjectView] error while submitting form data", error)
        }
    }

    const disableCreateButton = () => caseName && caseName !== "" && description && description !== ""

    return (
        <ModalNoFocus isOpen={isOpen} title={editMode ? "Edit case" : "Add new case"}>
            <CreateCaseForm>
                <RowWrapper>
                    <NameField>
                        <TextField
                            label="Name"
                            id="name"
                            name="name"
                            placeholder="Enter a name"
                            onChange={handleNameChange}
                            value={caseName}
                        />
                    </NameField>
                    <DateField>
                        <Label htmlFor="dgDate" label="DG4" />

                        <Input
                            type="month"
                            id="dgDate"
                            name="dgDate"
                            value={ToMonthDate(dG4Date) ?? ToMonthDate(caseItem?.DG4Date)}
                            onChange={(e) => setDG4Date(new Date(e.currentTarget.value))}
                        />
                    </DateField>
                </RowWrapper>
                <Label htmlFor="description" label="Description" />
                <TextArea
                    id="description"
                    placeholder="Enter a description"
                    onInput={handleDescriptionChange}
                    value={description ?? ""}
                    cols={110}
                    rows={4}
                />
                <ProductionStrategyOverviewField>
                    <NativeSelect
                        id="productionStrategy"
                        label="Production strategy overview"
                        onChange={handleProductionStrategyChange}
                        value={productionStrategy}
                    >
                        <option key={undefined} value={undefined}> </option>
                        <option key={0} value={0}>Depletion</option>
                        <option key={1} value={1}>Water injection</option>
                        <option key={2} value={2}>Gas injection</option>
                        <option key={3} value={3}>WAG</option>
                        <option key={4} value={4}>Mixed</option>
                    </NativeSelect>
                </ProductionStrategyOverviewField>
                <RowWrapper>
                    <WellCountField>
                        <ColumnWrapper>
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
                        </ColumnWrapper>
                    </WellCountField>
                    <WellCountField>
                        <ColumnWrapper>
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
                        </ColumnWrapper>
                    </WellCountField>
                    <ColumnWrapper>
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
                    </ColumnWrapper>
                </RowWrapper>
                <ButtonsWrapper>
                    <Button
                        type="button"
                        variant="outlined"
                        onClick={toggleModal}
                    >
                        Cancel
                    </Button>
                    <CreateButtonWrapper>
                        <CreateButton
                            type="submit"
                            onClick={submitCaseForm}
                            disabled={!disableCreateButton()}
                        >
                            {editMode ? "Save changes" : "Create case"}
                        </CreateButton>
                    </CreateButtonWrapper>
                </ButtonsWrapper>
            </CreateCaseForm>
        </ModalNoFocus>
    )
}

export default EditCaseModal

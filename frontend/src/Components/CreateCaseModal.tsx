import {
    Button,
    TextField,
    Input,
    Label,
    NativeSelect,
    Progress,
} from "@equinor/eds-core-react"
import {
    useState,
    ChangeEventHandler,
    MouseEventHandler,
    useEffect,
    FormEventHandler,
} from "react"
import { useNavigate, useParams } from "react-router-dom"
import styled from "styled-components"
import TextArea from "@equinor/fusion-react-textarea/dist/TextArea"
import { ModalNoFocus } from "./ModalNoFocus"
import {
    defaultDate, isDefaultDate, toMonthDate,
} from "../Utils/common"
import { GetCaseService } from "../Services/CaseService"
import { useAppContext } from "../Context/AppContext"
import { useModalContext } from "../Context/ModalContext"

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

const CreateCaseModal = () => {
    const {
        project,
        setProject,
    } = useAppContext()

    const {
        caseModalIsOpen,
        setCaseModalIsOpen,
        caseModalEditMode,
        caseModalShouldNavigate,
        modalCaseId,
    } = useModalContext()

    const { fusionContextId } = useParams<Record<string, string | undefined>>()
    const [caseName, setCaseName] = useState<string>("")
    const [dG4Date, setDG4Date] = useState<Date>(defaultDate())
    const [description, setDescription] = useState<string>("")
    const [productionStrategy, setProductionStrategy] = useState<Components.Schemas.ProductionStrategyOverview>(0)
    const [producerCount, setProducerWells] = useState<number>(0)
    const [gasInjectorCount, setGasInjectorWells] = useState<number>(0)
    const [waterInjectorCount, setWaterInjectorWells] = useState<number>(0)
    const [isLoading, setIsLoading] = useState<boolean>(false)
    const [caseItem, setCaseItem] = useState<Components.Schemas.CaseDto>()

    const navigate = useNavigate()

    const resetForm = () => {
        setCaseName("")
        setDG4Date(defaultDate())
        setDescription("")
        setProductionStrategy(0)
        setProducerWells(0)
        setGasInjectorWells(0)
        setWaterInjectorWells(0)
    }

    useEffect(() => {
        if (project) {
            const selectedCase = project.cases?.find((c) => c.id === modalCaseId)

            if (selectedCase) {
                setCaseItem(selectedCase)
                setCaseName(selectedCase.name)
                setDG4Date(selectedCase.dG4Date ? new Date(selectedCase.dG4Date) : defaultDate())
                setDescription(selectedCase.description)
                setProductionStrategy(selectedCase.productionStrategyOverview ?? 0)
                setProducerWells(selectedCase.producerCount ?? 0)
                setGasInjectorWells(selectedCase.gasInjectorCount ?? 0)
                setWaterInjectorWells(selectedCase.waterInjectorCount ?? 0)
            } else {
                resetForm()
            }
        }
    }, [project, modalCaseId])

    const handleNameChange: ChangeEventHandler<HTMLInputElement> = async (e) => {
        setCaseName(e.currentTarget.value)
    }

    const handleDescriptionChange: FormEventHandler<any> = async (e) => {
        setDescription(e.currentTarget.value)
    }

    const handleProductionStrategyChange: ChangeEventHandler<HTMLSelectElement> = async (e) => {
        if ([0, 1, 2, 3, 4].indexOf(Number(e.currentTarget.value)) !== -1) {
            const newProductionStrategy: Components.Schemas.ProductionStrategyOverview = Number(e.currentTarget.value) as Components.Schemas.ProductionStrategyOverview
            setProductionStrategy(newProductionStrategy)
        }
    }

    const handleDG4Change: ChangeEventHandler<HTMLInputElement> = async (e) => {
        let newDate = new Date(e.target.value)
        if (Number.isNaN(newDate.getTime())) {
            newDate = defaultDate()
        } else {
            newDate = new Date(e.target.value)
        }
        setDG4Date(newDate)
    }

    const getDG4Value = () => {
        if (!isDefaultDate(dG4Date)) {
            return toMonthDate(dG4Date)
        }
        return !isDefaultDate(caseItem?.dG4Date ? new Date(caseItem?.dG4Date) : new Date()) ? toMonthDate(caseItem?.dG4Date ? new Date(caseItem?.dG4Date) : new Date()) : undefined
    }

    const submitCaseForm: MouseEventHandler<HTMLButtonElement> = async (e) => {
        e.preventDefault()

        try {
            setIsLoading(true)
            if (!project) {
                throw new Error("No project found")
            }

            let projectResult: Components.Schemas.ProjectDto
            if (caseModalEditMode && caseItem && caseItem.id) {
                const newCase = { ...caseItem }
                newCase.name = caseName
                newCase.description = description
                newCase.dG4Date = dG4Date.toISOString()
                newCase.producerCount = producerCount
                newCase.gasInjectorCount = gasInjectorCount
                newCase.waterInjectorCount = waterInjectorCount
                newCase.productionStrategyOverview = productionStrategy ?? 0
                console.log("submitting for edit: ", newCase)
                projectResult = await (await GetCaseService()).updateCase(
                    project.id,
                    caseItem.id,
                    newCase,
                )
                setIsLoading(false)
            } else {
                projectResult = await (await GetCaseService()).create(
                    project.id,
                    {
                        name: caseName,
                        description,
                        dG4Date: dG4Date.toJSON(),
                        producerCount,
                        gasInjectorCount,
                        waterInjectorCount,
                        productionStrategyOverview: productionStrategy,
                    },
                )
                setIsLoading(false)
                navigate(`/${projectResult.cases.find((o) => (
                    o.name === caseName
                ))?.id}`)
            }
            setProject(projectResult)
            setCaseModalIsOpen(false)
            if (caseModalShouldNavigate) {
                navigate(fusionContextId!)
            }
        } catch (error) {
            console.error("[ProjectView] error while submitting form data", error)
            setIsLoading(false)
        }
    }

    const disableCreateButton = () => caseName && caseName !== ""

    return (
        <ModalNoFocus isOpen={caseModalIsOpen} title={caseModalEditMode ? "Edit case" : "Add new case"}>
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
                            value={getDG4Value()}
                            onChange={handleDG4Change}
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
                        <option key={undefined} value={undefined} aria-label="None"> </option>
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
                                onChange={(e: React.ChangeEvent<HTMLInputElement>) => setProducerWells(Number(e.currentTarget.value))}
                                onKeyPress={(e: React.KeyboardEvent<HTMLInputElement>) => {
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
                                onChange={(e: any) => setGasInjectorWells(Number(e.currentTarget.value))}
                                onKeyPress={(e: any) => {
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
                            onChange={(e: any) => setWaterInjectorWells(Number(e.currentTarget.value))}
                            onKeyPress={(e: any) => {
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
                        onClick={() => setCaseModalIsOpen(false)}
                    >
                        Cancel
                    </Button>
                    <CreateButtonWrapper>
                        {isLoading ? (
                            <CreateButton>
                                <Progress.Dots />
                            </CreateButton>
                        ) : (
                            <CreateButton
                                type="submit"
                                onClick={submitCaseForm}
                                disabled={!disableCreateButton()}
                            >
                                {caseModalEditMode ? "Save changes" : "Create case"}
                            </CreateButton>
                        )}

                    </CreateButtonWrapper>
                </ButtonsWrapper>
            </CreateCaseForm>
        </ModalNoFocus>
    )
}

export default CreateCaseModal

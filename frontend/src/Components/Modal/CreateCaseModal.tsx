import {
    Button,
    TextField,
    Input,
    NativeSelect,
    Progress,
    InputWrapper,
} from "@equinor/eds-core-react"
import {
    useState,
    ChangeEventHandler,
    MouseEventHandler,
    useEffect,
} from "react"
import { MarkdownEditor } from "@equinor/fusion-react-markdown"
import Grid from "@mui/material/Grid"
import Modal from "./Modal"
import { defaultDate, isDefaultDate, toMonthDate } from "../../Utils/common"
import { GetCaseService } from "../../Services/CaseService"
import { useProjectContext } from "../../Context/ProjectContext"
import { useModalContext } from "../../Context/ModalContext"
import { useAppContext } from "../../Context/AppContext"

const CreateCaseModal = () => {
    const { isLoading, setIsLoading } = useAppContext()
    const {
        project,
        setProject,
    } = useProjectContext()

    const {
        caseModalIsOpen,
        setCaseModalIsOpen,
        caseModalEditMode,
        modalCaseId,
    } = useModalContext()

    const [caseName, setCaseName] = useState<string>("")
    const [dG4Date, setDG4Date] = useState<Date>(defaultDate())
    const [description, setDescription] = useState<string>("")
    const [productionStrategy, setProductionStrategy] = useState<Components.Schemas.ProductionStrategyOverview>(0)
    const [producerCount, setProducerWells] = useState<number>(0)
    const [gasInjectorCount, setGasInjectorWells] = useState<number>(0)
    const [waterInjectorCount, setWaterInjectorWells] = useState<number>(0)
    const [projectCase, setCaseItem] = useState<Components.Schemas.CaseDto>()

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

    function handleDescriptionChange(value: string) {
        setDescription(value)
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
        return toMonthDate(new Date("2030-01-01"))
    }

    const submitCaseForm: MouseEventHandler<HTMLButtonElement> = async (e) => {
        e.preventDefault()

        try {
            setIsLoading(true)
            if (!project) {
                throw new Error("No project found")
            }

            let projectResult: Components.Schemas.ProjectDto
            if (caseModalEditMode && projectCase && projectCase.id) {
                const newCase = { ...projectCase }
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
                    projectCase.id,
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
            }
            setProject(projectResult)
            setCaseModalIsOpen(false)
        } catch (error) {
            console.error("[ProjectView] error while submitting form data", error)
            setIsLoading(false)
        }
    }

    const disableCreateButton = () => caseName && caseName !== ""

    return (
        <Modal
            isOpen={caseModalIsOpen}
            title={caseModalEditMode ? "Edit case" : "Add new case"}
        >
            <Grid container spacing={2}>
                <Grid item xs={12} md={8}>
                    <InputWrapper labelProps={{ label: "Name" }}>
                        <TextField
                            id="name"
                            name="name"
                            placeholder="Enter a name"
                            onChange={handleNameChange}
                            value={caseName}
                        />
                    </InputWrapper>
                </Grid>
                <Grid item xs={12} md={4}>
                    <InputWrapper labelProps={{ label: "DG4" }}>
                        <Input
                            type="month"
                            id="dgDate"
                            name="dgDate"
                            value={getDG4Value()}
                            onChange={handleDG4Change}
                        />
                    </InputWrapper>
                </Grid>
                <Grid item xs={12}>
                    <InputWrapper labelProps={{ label: "Description" }}>
                        <MarkdownEditor
                            minHeight="100px"
                            value={description}
                            menuItems={["strong", "em", "bullet_list", "ordered_list", "blockquote", "h1", "h2", "h3", "paragraph"]}
                            onInput={(markdown) => {
                                // eslint-disable-next-line no-underscore-dangle
                                const value = (markdown as any).target._value
                                handleDescriptionChange(value)
                            }}
                        />
                    </InputWrapper>
                </Grid>
                <Grid item xs={12}>
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
                </Grid>
                <Grid item xs={12} md={4}>
                    <InputWrapper labelProps={{ label: "Producer wells" }}>
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
                    </InputWrapper>
                </Grid>
                <Grid item xs={12} md={4}>
                    <InputWrapper labelProps={{ label: "Gas injector wells" }}>
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
                    </InputWrapper>
                </Grid>
                <Grid item xs={12} md={4}>
                    <InputWrapper labelProps={{ label: "Water injector wells" }}>
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
                    </InputWrapper>
                </Grid>
            </Grid>
            {/* ModalActions */}
            <Grid container spacing={1} justifyContent="flex-end">
                <Grid item>
                    <Button
                        type="button"
                        variant="outlined"
                        onClick={() => setCaseModalIsOpen(false)}
                    >
                        Cancel
                    </Button>
                </Grid>
                <Grid item>
                    {isLoading ? (
                        <Button>
                            <Progress.Dots />
                        </Button>
                    ) : (
                        <Button
                            type="submit"
                            onClick={submitCaseForm}
                            disabled={!disableCreateButton()}
                        >
                            {caseModalEditMode ? "Save changes" : "Create case"}
                        </Button>
                    )}
                </Grid>
            </Grid>
        </Modal>
    )
}

export default CreateCaseModal

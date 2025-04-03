import {
    Button,
    TextField,
    Input,
    NativeSelect,
    Progress,
    InputWrapper,
} from "@equinor/eds-core-react"
import { MarkdownEditor } from "@equinor/fusion-react-markdown"
import Grid from "@mui/material/Grid2"
import {
    useState,
    ChangeEventHandler,
    MouseEventHandler,
    useEffect,
} from "react"

import BaseModal from "./BaseModal"
import {
    ModalContent,
    ModalActions,
    FormSection,
    InputGroup,
} from "./styles"

import { useDataFetch } from "@/Hooks"
import { GetCaseService } from "@/Services/CaseService"
import { useAppStore } from "@/Store/AppStore"
import { useModalContext } from "@/Store/ModalContext"
import {
    dateStringToDateUtc,
    toMonthDate,
} from "@/Utils/DateUtils"

const CreateCaseModal = () => {
    const { isLoading, setIsLoading } = useAppStore()
    const revisionAndProjectData = useDataFetch()

    const {
        caseModalIsOpen,
        setCaseModalIsOpen,
        caseModalEditMode,
        modalCaseId,
    } = useModalContext()

    const [caseName, setCaseName] = useState<string>("")
    const [dg4Date, setDg4Date] = useState<Date | null>(null)
    const [description, setDescription] = useState<string>("")
    const [productionStrategy, setProductionStrategy] = useState<Components.Schemas.ProductionStrategyOverview>(0)
    const [producerCount, setProducerWells] = useState<number>(0)
    const [gasInjectorCount, setGasInjectorWells] = useState<number>(0)
    const [waterInjectorCount, setWaterInjectorWells] = useState<number>(0)
    const [projectCase, setCaseItem] = useState<Components.Schemas.CaseOverviewDto>()

    const resetForm = () => {
        setCaseName("")
        setDg4Date(null)
        setDescription("")
        setProductionStrategy(0)
        setProducerWells(0)
        setGasInjectorWells(0)
        setWaterInjectorWells(0)
    }

    useEffect(() => {
        if (revisionAndProjectData) {
            const selectedCase = revisionAndProjectData.commonProjectAndRevisionData.cases?.find((c) => c.caseId === modalCaseId)

            if (selectedCase) {
                setCaseItem(selectedCase)
                setCaseName(selectedCase.name)
                setDg4Date(dateStringToDateUtc(selectedCase.dg4Date))
                setDescription(selectedCase.description)
                setProductionStrategy(selectedCase.productionStrategyOverview ?? 0)
                setProducerWells(selectedCase.producerCount ?? 0)
                setGasInjectorWells(selectedCase.gasInjectorCount ?? 0)
                setWaterInjectorWells(selectedCase.waterInjectorCount ?? 0)
            } else {
                resetForm()
            }
        }
    }, [revisionAndProjectData, modalCaseId])

    const handleProductionStrategyChange: ChangeEventHandler<HTMLSelectElement> = async (e) => {
        if ([0, 1, 2, 3, 4].indexOf(Number(e.currentTarget.value)) !== -1) {
            const newProductionStrategy: Components.Schemas.ProductionStrategyOverview = Number(e.currentTarget.value) as Components.Schemas.ProductionStrategyOverview

            setProductionStrategy(newProductionStrategy)
        }
    }

    const handleDG4Change: ChangeEventHandler<HTMLInputElement> = async (e) => {
        let newDate: Date | null = dateStringToDateUtc(e.target.value)

        if (Number.isNaN(newDate.getTime())) {
            newDate = null
        } else {
            newDate = dateStringToDateUtc(e.target.value)
        }
        setDg4Date(newDate)
    }

    const submitCaseForm: MouseEventHandler<HTMLButtonElement> = async (e) => {
        e.preventDefault()

        try {
            if (!revisionAndProjectData) {
                throw new Error("No project found")
            }
            setIsLoading(true)

            if (caseModalEditMode && projectCase && projectCase.caseId) {
                const newCase = { ...projectCase }

                newCase.name = caseName
                newCase.description = description
                newCase.dg4Date = dg4Date!.toISOString()
                newCase.producerCount = producerCount
                newCase.gasInjectorCount = gasInjectorCount
                newCase.waterInjectorCount = waterInjectorCount
                newCase.productionStrategyOverview = productionStrategy ?? 0

                await GetCaseService().updateCase(
                    revisionAndProjectData.projectId,
                    projectCase.caseId,
                    newCase,
                )
                setIsLoading(false)
            } else {
                await GetCaseService().create(
                    revisionAndProjectData.projectId,
                    {
                        name: caseName,
                        description,
                        dg4Date: dg4Date!.toJSON(),
                        producerCount,
                        gasInjectorCount,
                        waterInjectorCount,
                        productionStrategyOverview: productionStrategy,
                    },
                )
                setIsLoading(false)
            }
            setCaseModalIsOpen(false)
        } catch (error) {
            console.error("[ProjectView] error while submitting form data", error)
            setIsLoading(false)
        }
    }

    const disableCreateButton = () => !dg4Date || !caseName || caseName.trim() === ""

    const getButtonText = () => {
        if (isLoading) { return <Progress.Dots /> }

        return caseModalEditMode ? "Save" : "Create case"
    }

    return (
        <BaseModal
            isOpen={caseModalIsOpen}
            title={caseModalEditMode ? "Edit case" : "Add new case"}
            content={(
                <ModalContent container spacing={2}>
                    <FormSection size={{ xs: 12, md: 8 }}>
                        <InputWrapper labelProps={{ label: "Name" }}>
                            <TextField
                                id="name"
                                name="name"
                                placeholder="Enter a name"
                                onChange={(e: React.ChangeEvent<HTMLInputElement>) => setCaseName(e.currentTarget.value.trimStart())}
                                value={caseName}
                            />
                        </InputWrapper>
                    </FormSection>
                    <FormSection size={{ xs: 12, md: 4 }}>
                        <InputWrapper labelProps={{ label: "DG4" }}>
                            <Input
                                type="month"
                                id="dgDate"
                                name="dgDate"
                                value={toMonthDate(dg4Date)}
                                onChange={handleDG4Change}
                            />
                        </InputWrapper>
                    </FormSection>
                    <FormSection size={12}>
                        <InputWrapper labelProps={{ label: "Description" }}>
                            <MarkdownEditor
                                minHeight="100px"
                                value={description}
                                menuItems={["strong", "em", "bullet_list", "ordered_list", "blockquote", "h1", "h2", "h3", "paragraph"]}
                                onInput={(e: any) => {
                                    // eslint-disable-next-line no-underscore-dangle
                                    setDescription(e.target._value)
                                }}
                            />
                        </InputWrapper>
                    </FormSection>
                    <FormSection size={12}>
                        <NativeSelect
                            id="productionStrategy"
                            label="Production strategy overview"
                            onChange={handleProductionStrategyChange}
                            value={productionStrategy}
                        >
                            <option key={0} value={0}>Depletion</option>
                            <option key={1} value={1}>Water injection</option>
                            <option key={2} value={2}>Gas injection</option>
                            <option key={3} value={3}>WAG</option>
                            <option key={4} value={4}>Mixed</option>
                        </NativeSelect>
                    </FormSection>
                    <InputGroup size={{ xs: 12, md: 4 }}>
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
                    </InputGroup>
                    <InputGroup size={{ xs: 12, md: 4 }}>
                        <InputWrapper labelProps={{ label: "Gas injector wells" }}>
                            <Input
                                id="gasInjector"
                                type="number"
                                value={gasInjectorCount}
                                disabled={false}
                                onChange={(e: React.ChangeEvent<HTMLInputElement>) => setGasInjectorWells(Number(e.currentTarget.value))}
                                onKeyPress={(e: React.KeyboardEvent<HTMLInputElement>) => {
                                    if (!/\d/.test(e.key)) {
                                        e.preventDefault()
                                    }
                                }}
                            />
                        </InputWrapper>
                    </InputGroup>
                    <InputGroup size={{ xs: 12, md: 4 }}>
                        <InputWrapper labelProps={{ label: "Water injector wells" }}>
                            <Input
                                id="waterInjector"
                                type="number"
                                value={waterInjectorCount}
                                disabled={false}
                                onChange={(e: React.ChangeEvent<HTMLInputElement>) => setWaterInjectorWells(Number(e.currentTarget.value))}
                                onKeyPress={(e: React.KeyboardEvent<HTMLInputElement>) => {
                                    if (!/\d/.test(e.key)) {
                                        e.preventDefault()
                                    }
                                }}
                            />
                        </InputWrapper>
                    </InputGroup>
                </ModalContent>
            )}
            actions={(
                <ModalActions
                    container
                    spacing={2}
                    justifyContent="flex-end"
                >
                    <Grid>
                        <Button
                            variant="outlined"
                            onClick={() => setCaseModalIsOpen(false)}
                        >
                            Cancel
                        </Button>
                    </Grid>
                    <Grid>
                        <Button
                            disabled={disableCreateButton()}
                            onClick={submitCaseForm}
                        >
                            {getButtonText()}
                        </Button>
                    </Grid>
                </ModalActions>
            )}
        />
    )
}

export default CreateCaseModal

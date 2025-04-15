import {
    Button,
    TextField as EdsTextField,
    Input,
    NativeSelect,
    Progress,
    InputWrapper,
} from "@equinor/eds-core-react"
import { MarkdownEditor } from "@equinor/fusion-react-markdown"
import Grid from "@mui/material/Grid2"
import { DatePicker } from "@mui/x-date-pickers"
import dayjs, { Dayjs } from "dayjs"
import {
    useState,
    ChangeEventHandler,
    MouseEventHandler,
    useEffect,
    useMemo,
    useCallback,
} from "react"
import styled from "styled-components"

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
import { GANTT_CHART_CONFIG } from "@/Utils/Config/constants"
import {
    dateStringToDateUtc,
    dateToUtcDateStringWithZeroTimePart,
} from "@/Utils/DateUtils"

const RequiredAsterisk = styled.span`
    color: red;
    margin-left: 2px;
`

const CreateCaseModal = (): JSX.Element => {
    const { isLoading, setIsLoading } = useAppStore()
    const revisionAndProjectData = useDataFetch()

    const {
        caseModalIsOpen,
        setCaseModalIsOpen,
        caseModalEditMode,
        modalCaseId,
    } = useModalContext()

    const minAllowedYear = GANTT_CHART_CONFIG.MIN_ALLOWED_YEAR
    const maxAllowedYear = GANTT_CHART_CONFIG.MAX_ALLOWED_YEAR
    const minDate = useMemo(() => dayjs().year(minAllowedYear).month(0).date(1), [minAllowedYear])
    const maxDate = useMemo(() => dayjs().year(maxAllowedYear).month(11).date(31), [maxAllowedYear])

    const getDefaultDg4Date = useCallback(() => {
        const today = dayjs()

        if (today.isBefore(minDate)) {
            return minDate
        }
        if (today.isAfter(maxDate)) {
            return maxDate
        }

        return today
    }, [minDate, maxDate])

    const [caseName, setCaseName] = useState<string | null>(null)
    const [dg4Date, setDg4Date] = useState<Dayjs | null>(getDefaultDg4Date())
    const [description, setDescription] = useState<string>("")
    const [productionStrategy, setProductionStrategy] = useState<Components.Schemas.ProductionStrategyOverview>(0)
    const [producerCount, setProducerWells] = useState<number>(0)
    const [gasInjectorCount, setGasInjectorWells] = useState<number>(0)
    const [waterInjectorCount, setWaterInjectorWells] = useState<number>(0)
    const [projectCase, setCaseItem] = useState<Components.Schemas.CaseOverviewDto>()

    useEffect(() => {
        const resetForm = (): void => {
            setCaseName("")
            setDg4Date(getDefaultDg4Date())
            setDescription("")
            setProductionStrategy(0)
            setProducerWells(0)
            setGasInjectorWells(0)
            setWaterInjectorWells(0)
        }

        if (revisionAndProjectData) {
            const selectedCase = revisionAndProjectData.commonProjectAndRevisionData.cases?.find((c) => c.caseId === modalCaseId)

            if (selectedCase) {
                setCaseItem(selectedCase)
                setCaseName(selectedCase.name)
                setDg4Date(dayjs(dateStringToDateUtc(selectedCase.dg4Date))) // Convert stored string back to Dayjs
                setDescription(selectedCase.description)
                setProductionStrategy(selectedCase.productionStrategyOverview ?? 0)
                setProducerWells(selectedCase.producerCount ?? 0)
                setGasInjectorWells(selectedCase.gasInjectorCount ?? 0)
                setWaterInjectorWells(selectedCase.waterInjectorCount ?? 0)
            } else {
                resetForm()
            }
        }
    }, [revisionAndProjectData, modalCaseId, getDefaultDg4Date])

    const handleProductionStrategyChange: ChangeEventHandler<HTMLSelectElement> = (e) => {
        if ([0, 1, 2, 3, 4].indexOf(Number(e.currentTarget.value)) !== -1) {
            const newProductionStrategy: Components.Schemas.ProductionStrategyOverview = Number(e.currentTarget.value) as Components.Schemas.ProductionStrategyOverview

            setProductionStrategy(newProductionStrategy)
        }
    }

    const handleDG4Change = (newValue: Dayjs | null): void => {
        setDg4Date(newValue)
    }

    const submitCaseForm: MouseEventHandler<HTMLButtonElement> = async (e) => {
        e.preventDefault()

        try {
            if (!revisionAndProjectData) {
                throw new Error("No project found")
            }

            if (!caseName || !dg4Date) {
                throw new Error("Missing required fields")
            }

            setIsLoading(true)

            const dg4DateForApi = dg4Date.toDate()
            const dg4DateString = dateToUtcDateStringWithZeroTimePart(dg4DateForApi)

            if (caseModalEditMode && projectCase && projectCase.caseId) {
                const newCase = { ...projectCase } as Components.Schemas.UpdateCaseDto

                newCase.name = caseName
                newCase.description = description
                newCase.dg4Date = dg4DateString
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
                        dg4Date: dg4DateString,
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

    const disableCreateButton = (): boolean => !dg4Date || !caseName || caseName.trim() === ""

    const getButtonText = (): JSX.Element | string => {
        if (isLoading) { return <Progress.Dots /> }

        return caseModalEditMode ? "Save" : "Create case"
    }

    return (
        <BaseModal
            isOpen={caseModalIsOpen}
            title={caseModalEditMode ? "Edit this case" : "Create a new case"}
            content={(
                <ModalContent container spacing={2}>
                    <FormSection size={{ xs: 12, md: 10 }}>
                        <InputWrapper labelProps={{
                            label: (
                                <>
                                    Name
                                    {" "}
                                    <RequiredAsterisk>*</RequiredAsterisk>
                                </>
                            ),
                        }}
                        >
                            <EdsTextField
                                id="name"
                                name="name"
                                placeholder="Enter a name"
                                onChange={(e: React.ChangeEvent<HTMLInputElement>) => setCaseName(e.currentTarget.value.trimStart())}
                                value={caseName ?? ""}
                            />
                        </InputWrapper>
                    </FormSection>
                    <FormSection size={{ xs: 12, md: 2 }}>
                        <InputWrapper labelProps={{
                            label: (
                                <>
                                    DG4
                                    {" "}
                                    <RequiredAsterisk>*</RequiredAsterisk>
                                </>
                            ),
                        }}
                        >
                            <DatePicker
                                value={dg4Date}
                                onChange={handleDG4Change}
                                minDate={minDate}
                                maxDate={maxDate}
                                slotProps={{
                                    textField: {
                                        size: "small",
                                        fullWidth: true,
                                        required: true, // Mark the text field as required
                                        placeholder: "Select DG4 date",
                                    },
                                }}
                                format="DD MMM YYYY"
                            />
                        </InputWrapper>
                    </FormSection>
                    <FormSection size={12}>
                        <InputWrapper labelProps={{ label: "Description" }}>
                            <MarkdownEditor
                                minHeight="100px"
                                value={description}
                                menuItems={["strong", "em", "bullet_list", "ordered_list", "blockquote", "h1", "h2", "h3", "paragraph"]}
                                onInput={(e: any): void => {
                                    // eslint-disable-next-line no-underscore-dangle
                                    setDescription(e.target._value)
                                }}
                            />
                        </InputWrapper>
                    </FormSection>
                    <InputGroup size={{ xs: 12, md: 3 }}>
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
                    </InputGroup>
                    <InputGroup size={{ xs: 12, md: 3 }}>
                        <InputWrapper labelProps={{ label: "Producer wells" }}>
                            <Input
                                id="producerWells"
                                type="number"
                                value={producerCount}
                                disabled={false}
                                onChange={(e: React.ChangeEvent<HTMLInputElement>): void => setProducerWells(Number(e.currentTarget.value))}
                                onKeyPress={(e: React.KeyboardEvent<HTMLInputElement>): void => {
                                    if (!/\d/.test(e.key)) {
                                        e.preventDefault()
                                    }
                                }}
                            />
                        </InputWrapper>
                    </InputGroup>
                    <InputGroup size={{ xs: 12, md: 3 }}>
                        <InputWrapper labelProps={{ label: "Gas injector wells" }}>
                            <Input
                                id="gasInjector"
                                type="number"
                                value={gasInjectorCount}
                                disabled={false}
                                onChange={(e: React.ChangeEvent<HTMLInputElement>): void => setGasInjectorWells(Number(e.currentTarget.value))}
                                onKeyPress={(e: React.KeyboardEvent<HTMLInputElement>): void => {
                                    if (!/\d/.test(e.key)) {
                                        e.preventDefault()
                                    }
                                }}
                            />
                        </InputWrapper>
                    </InputGroup>
                    <InputGroup size={{ xs: 12, md: 3 }}>
                        <InputWrapper labelProps={{ label: "Water injector wells" }}>
                            <Input
                                id="waterInjector"
                                type="number"
                                value={waterInjectorCount}
                                disabled={false}
                                onChange={(e: React.ChangeEvent<HTMLInputElement>): void => setWaterInjectorWells(Number(e.currentTarget.value))}
                                onKeyPress={(e: React.KeyboardEvent<HTMLInputElement>): void => {
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
                            onClick={(): void => setCaseModalIsOpen(false)}
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

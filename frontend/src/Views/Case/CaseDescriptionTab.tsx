import {
    Dispatch,
    SetStateAction,
    ChangeEventHandler,
    useState,
} from "react"
import styled from "styled-components"

import {
    Button, Label, NativeSelect, Progress, Typography,
} from "@equinor/eds-core-react"
import TextArea from "@equinor/fusion-react-textarea/dist/TextArea"
import { Project } from "../../models/Project"
import { Case } from "../../models/case/Case"
import { GetCaseService } from "../../Services/CaseService"
import CaseNumberInput from "../../Components/Case/CaseNumberInput"

const ColumnWrapper = styled.div`
    display: flex;
    flex-direction: column;
`
const RowWrapper = styled.div`
    display: flex;
    flex-direction: row;
    margin-bottom: 78px;
`
const TopWrapper = styled.div`
    display: flex;
    flex-direction: row;
    margin-top: 20px;
    margin-bottom: 20px;
`
const PageTitle = styled(Typography)`
    flex-grow: 1;
`
const DescriptionField = styled(TextArea)`
    margin-bottom: 50px;
`
const NativeSelectField = styled(NativeSelect)`
    width: 250px;
`
const InputWrapper = styled.div`
    margin-right: 20px;
`

interface Props {
    project: Project,
    setProject: Dispatch<SetStateAction<Project | undefined>>,
    caseItem: Case,
    setCase: Dispatch<SetStateAction<Case | undefined>>,
}

function CaseDescriptionTab({
    project,
    setProject,
    caseItem,
    setCase,
}: Props) {
    const [isSaving, setIsSaving] = useState<boolean>()

    const handleDescriptionChange: ChangeEventHandler<HTMLInputElement> = async (e) => {
        const newCase: Case = { ...caseItem }
        newCase.description = e.currentTarget.value
        setCase(newCase)
    }

    const handleFacilitiesAvailabilityChange: ChangeEventHandler<HTMLInputElement> = async (e) => {
        const newCase: Case = { ...caseItem }
        const newfacilitiesAvailability = Number(e.currentTarget.value)
        newCase.facilitiesAvailability = newfacilitiesAvailability / 100
        setCase(newCase)
    }

    const handleProducerCountChange: ChangeEventHandler<HTMLInputElement> = async (e) => {
        const newCase: Case = { ...caseItem }
        newCase.producerCount = Number(e.currentTarget.value)
        setCase(newCase)
    }

    const handleGasInjectorCountChange: ChangeEventHandler<HTMLInputElement> = async (e) => {
        const newCase: Case = { ...caseItem }
        newCase.gasInjectorCount = Number(e.currentTarget.value)
        setCase(newCase)
    }

    const handletWaterInjectorCountChange: ChangeEventHandler<HTMLInputElement> = async (e) => {
        const newCase: Case = { ...caseItem }
        newCase.waterInjectorCount = Number(e.currentTarget.value)
        setCase(newCase)
    }

    const handleProductionStrategyChange: ChangeEventHandler<HTMLSelectElement> = async (e) => {
        if ([0, 1, 2, 3, 4].indexOf(Number(e.currentTarget.value)) !== -1) {
            // eslint-disable-next-line max-len
            const newProductionStrategy: Components.Schemas.ProductionStrategyOverview = Number(e.currentTarget.value) as Components.Schemas.ProductionStrategyOverview
            const newCase: Case = { ...caseItem }
            newCase.productionStrategyOverview = newProductionStrategy
            setCase(newCase)
        }
    }

    const handleArtificialLiftChange: ChangeEventHandler<HTMLSelectElement> = async (e) => {
        if ([0, 1, 2, 3].indexOf(Number(e.currentTarget.value)) !== -1) {
            // eslint-disable-next-line max-len
            const newArtificialLift: Components.Schemas.ArtificialLift = Number(e.currentTarget.value) as Components.Schemas.ArtificialLift
            const newCase: Case = { ...caseItem }
            newCase.artificialLift = newArtificialLift
            setCase(newCase)
        }
    }

    const handleSave = async () => {
        setIsSaving(true)
        const updatedCase = Case.Copy(caseItem)
        const result = await (await GetCaseService()).update(updatedCase)
        setIsSaving(false)
        setCase(result)
    }

    return (
        <>
            <TopWrapper>
                <PageTitle variant="h3">Description</PageTitle>
                {!isSaving ? <Button onClick={handleSave}>Save</Button> : (
                    <Button>
                        <Progress.Dots />
                    </Button>
                )}
            </TopWrapper>
            <ColumnWrapper>

                <Label htmlFor="description" label="Case description" />
                <DescriptionField
                    id="description"
                    placeholder="Enter a description"
                    onInput={handleDescriptionChange}
                    value={caseItem.description}
                    cols={110}
                    rows={8}
                />
                <RowWrapper>
                    <InputWrapper>
                        <CaseNumberInput
                            onChange={handleProducerCountChange}
                            value={caseItem.producerCount}
                            integer
                            label="Oil producer wells"
                        />
                    </InputWrapper>
                    <InputWrapper>
                        <CaseNumberInput
                            onChange={handletWaterInjectorCountChange}
                            value={caseItem.waterInjectorCount}
                            integer
                            disabled={false}
                            label="Water injector count"
                        />
                    </InputWrapper>
                    <CaseNumberInput
                        onChange={handleGasInjectorCountChange}
                        value={caseItem.gasInjectorCount}
                        integer
                        label="Gas injector count"
                    />
                </RowWrapper>
                <RowWrapper>
                    <InputWrapper>
                        <NativeSelectField
                            id="productionStrategy"
                            label="Production strategy overview"
                            onChange={handleProductionStrategyChange}
                            value={caseItem.productionStrategyOverview}
                        >
                            <option key={undefined} value={undefined}> </option>
                            <option key={0} value={0}>Depletion</option>
                            <option key={1} value={1}>Water injection</option>
                            <option key={2} value={2}>Gas injection</option>
                            <option key={3} value={3}>WAG</option>
                            <option key={4} value={4}>Mixed</option>
                        </NativeSelectField>
                    </InputWrapper>
                    <InputWrapper>
                        <NativeSelectField
                            id="artificialLift"
                            label="Artificial lift"
                            onChange={handleArtificialLiftChange}
                            value={caseItem.artificialLift}
                        >
                            <option key="0" value={0}>No lift</option>
                            <option key="1" value={1}>Gas lift</option>
                            <option key="2" value={2}>Electrical submerged pumps</option>
                            <option key="3" value={3}>Subsea booster pumps</option>
                        </NativeSelectField>
                    </InputWrapper>
                    <CaseNumberInput
                        onChange={handleFacilitiesAvailabilityChange}
                        value={caseItem.facilitiesAvailability * 100}
                        integer={false}
                        label="Facilities availability (%)"
                    />
                </RowWrapper>
            </ColumnWrapper>
        </>
    )
}

export default CaseDescriptionTab

import {
    Dispatch,
    SetStateAction,
    ChangeEventHandler,
    FormEventHandler,
} from "react"
import styled from "styled-components"

import {
    Label, NativeSelect, Typography,
} from "@equinor/eds-core-react"
import TextArea from "@equinor/fusion-react-textarea/dist/TextArea"
import CaseNumberInput from "./CaseNumberInput"

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
    caseItem: Components.Schemas.CaseDto,
    setCase: Dispatch<SetStateAction<Components.Schemas.CaseDto | undefined>>,
    activeTab: number
}

const CaseDescriptionTab = ({
    caseItem,
    setCase,
    activeTab,
}: Props) => {
    const handleDescriptionChange: FormEventHandler<any> = async (e) => {
        const newCase = { ...caseItem }
        newCase.description = e.currentTarget.value
        setCase(newCase)
    }

    const handleFacilitiesAvailabilityChange: ChangeEventHandler<HTMLInputElement> = async (e) => {
        const newCase = { ...caseItem }
        const newfacilitiesAvailability = e.currentTarget.value.length > 0
            ? Math.min(Math.max(Number(e.currentTarget.value), 0), 100) : undefined
        if (newfacilitiesAvailability !== undefined) {
            newCase.facilitiesAvailability = newfacilitiesAvailability / 100
        } else { newCase.facilitiesAvailability = 0 }
        setCase(newCase)
    }

    const handleProducerCountChange: ChangeEventHandler<HTMLInputElement> = async (e) => {
        const newCase = { ...caseItem }
        newCase.producerCount = e.currentTarget.value.length > 0 ? Math.max(Number(e.currentTarget.value), 0) : 0
        setCase(newCase)
    }

    const handleGasInjectorCountChange: ChangeEventHandler<HTMLInputElement> = async (e) => {
        const newCase = { ...caseItem }
        newCase.gasInjectorCount = e.currentTarget.value.length > 0 ? Math.max(Number(e.currentTarget.value), 0) : 0
        setCase(newCase)
    }

    const handletWaterInjectorCountChange: ChangeEventHandler<HTMLInputElement> = async (e) => {
        const newCase = { ...caseItem }
        newCase.waterInjectorCount = e.currentTarget.value.length > 0 ? Math.max(Number(e.currentTarget.value), 0) : 0
        setCase(newCase)
    }

    const handleProductionStrategyChange: ChangeEventHandler<HTMLSelectElement> = async (e) => {
        if ([0, 1, 2, 3, 4].indexOf(Number(e.currentTarget.value)) !== -1) {
            const newProductionStrategy: Components.Schemas.ProductionStrategyOverview = Number(e.currentTarget.value) as Components.Schemas.ProductionStrategyOverview
            const newCase = { ...caseItem }
            newCase.productionStrategyOverview = newProductionStrategy
            setCase(newCase)
        }
    }

    const handleArtificialLiftChange: ChangeEventHandler<HTMLSelectElement> = async (e) => {
        if ([0, 1, 2, 3].indexOf(Number(e.currentTarget.value)) !== -1) {
            // eslint-disable-next-line max-len
            const newArtificialLift: Components.Schemas.ArtificialLift = Number(e.currentTarget.value) as Components.Schemas.ArtificialLift
            const newCase = { ...caseItem }
            newCase.artificialLift = newArtificialLift
            setCase(newCase)
        }
    }
    if (activeTab !== 0) { return null }

    return (
        <>
            <TopWrapper>
                <PageTitle variant="h3">Description</PageTitle>
            </TopWrapper>
            <ColumnWrapper>

                <Label htmlFor="description" label="Case description" />
                <DescriptionField
                    id="description"
                    placeholder="Enter a description"
                    onInput={handleDescriptionChange}
                    value={caseItem.description ?? ""}
                    cols={110}
                    rows={8}
                />
                <RowWrapper>
                    <InputWrapper>
                        <CaseNumberInput
                            onChange={handleProducerCountChange}
                            defaultValue={caseItem.producerCount}
                            integer
                            label="Production wells"
                        />
                    </InputWrapper>
                    <InputWrapper>
                        <CaseNumberInput
                            onChange={handletWaterInjectorCountChange}
                            defaultValue={caseItem.waterInjectorCount}
                            integer
                            disabled={false}
                            label="Water injector wells"
                        />
                    </InputWrapper>
                    <CaseNumberInput
                        onChange={handleGasInjectorCountChange}
                        defaultValue={caseItem.gasInjectorCount}
                        integer
                        label="Gas injector wells"
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
                        defaultValue={caseItem.facilitiesAvailability
                            !== undefined ? caseItem.facilitiesAvailability * 100 : undefined}
                        integer={false}
                        label="Facilities availability"
                        unit="%"
                    />
                </RowWrapper>
            </ColumnWrapper>
        </>
    )
}

export default CaseDescriptionTab

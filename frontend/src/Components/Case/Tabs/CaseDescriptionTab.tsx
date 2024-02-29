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
import CaseNumberInput from "../../Input/CaseNumberInput"
import InputContainer from "../../Input/Containers/InputContainer"
import InputSwitcher from "../../Input/InputSwitcher"

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
    width: 100%;
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
    const productionStrategyOptions = {
        0: "Depletion",
        1: "Water injection",
        2: "Gas injection",
        3: "WAG",
        4: "Mixed",
    }

    const artificialLiftOptions = {
        0: "No lift",
        1: "Gas lift",
        2: "Electrical submerged pumps",
        3: "Subsea booster pumps",
    }

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
            <InputSwitcher label="Description" value={caseItem.description ?? ""}>
                <>
                    <Label htmlFor="description" label="Case description" />
                    <DescriptionField
                        id="description"
                        placeholder="Enter a description"
                        onInput={handleDescriptionChange}
                        value={caseItem.description ?? ""}
                        cols={110}
                        rows={8}
                    />
                </>
            </InputSwitcher>
            <InputContainer desktopColumns={3} mobileColumns={1} breakPoint={850}>
                <InputSwitcher
                    label="Production wells"
                    value={caseItem.producerCount.toString()}
                >
                    <CaseNumberInput
                        onChange={handleProducerCountChange}
                        defaultValue={caseItem.producerCount}
                        integer
                        label="Production wells"
                    />
                </InputSwitcher>
                <InputSwitcher
                    label="Water injector wells"
                    value={caseItem.waterInjectorCount.toString()}
                >
                    <CaseNumberInput
                        onChange={handletWaterInjectorCountChange}
                        defaultValue={caseItem.waterInjectorCount}
                        integer
                        disabled={false}
                        label="Water injector wells"
                    />
                </InputSwitcher>
                <InputSwitcher
                    label="Gas injector wells"
                    value={caseItem.gasInjectorCount.toString()}
                >
                    <CaseNumberInput
                        onChange={handleGasInjectorCountChange}
                        defaultValue={caseItem.gasInjectorCount}
                        integer
                        label="Gas injector wells"
                    />
                </InputSwitcher>
            </InputContainer>
            <InputContainer desktopColumns={3} mobileColumns={1} breakPoint={850}>
                <InputSwitcher
                    label="Production strategy overview"
                    value={productionStrategyOptions[caseItem.productionStrategyOverview]}

                >
                    <NativeSelect
                        id="productionStrategy"
                        label="Production strategy overview"
                        onChange={handleProductionStrategyChange}
                        value={caseItem.productionStrategyOverview}
                    >
                        {Object.entries(productionStrategyOptions).map(([value, label]) => (
                            <option key={value} value={value}>{label}</option>
                        ))}
                    </NativeSelect>
                </InputSwitcher>
                <InputSwitcher
                    label="Artificial lift"
                    value={artificialLiftOptions[caseItem.artificialLift]}
                >
                    <NativeSelect
                        id="artificialLift"
                        label="Artificial lift"
                        onChange={handleArtificialLiftChange}
                        value={caseItem.artificialLift}
                    >
                        {Object.entries(artificialLiftOptions).map(([value, label]) => (
                            <option key={value} value={value}>{label}</option>
                        ))}
                    </NativeSelect>
                </InputSwitcher>
                <InputSwitcher
                    label="Facilities availability"
                    value={`${caseItem.facilitiesAvailability !== undefined ? (caseItem.facilitiesAvailability * 100).toFixed(2) : ""}%`}
                >
                    <CaseNumberInput
                        onChange={handleFacilitiesAvailabilityChange}
                        defaultValue={caseItem.facilitiesAvailability
                            !== undefined ? caseItem.facilitiesAvailability * 100 : undefined}
                        integer={false}
                        label="Facilities availability"
                        unit="%"
                    />
                </InputSwitcher>
            </InputContainer>
        </>
    )
}

export default CaseDescriptionTab

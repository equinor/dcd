import {
    Dispatch,
    SetStateAction,
    ChangeEventHandler,
} from "react"
import styled from "styled-components"

import {
    Button, Label, NativeSelect, Typography,
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
    justify-content: space-between;
    margin-bottom: 78px;
`
const TopWrapper = styled.div`
    display: flex;
    flex-direction: row;
    padding: 1.5rem 2rem;
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
    /*
    Description - Case
    ------------------
    Exploration wells - Exploration -> explorationWell -> well
    appraisal wells - Exploration -> explorationWell -> well
    oil producer wells - WellProject -> wellProjectWell -> well
    gas producer wells - WellProject -> wellprojectWll -> well
    ------------------
    water injector wells - case
    gas injector wells - case
    templates - case
    production strategy overview - case
    artificial lift - case
     facilities availability - case
    */

    const handleDescriptionChange: ChangeEventHandler<HTMLInputElement> = async (e) => {
        const newCase = { ...caseItem }
        newCase.description = e.currentTarget.value
        setCase(newCase)
    }

    const handleWellsChange: ChangeEventHandler<HTMLInputElement> = async (e) => {
        console.log("Wells changed")
        // const newCase = { ...caseItem }
        // newCase.facilitiesAvailability = Number(e.currentTarget.value)
        // setCase(newCase)
    }

    const handleFacilitiesAvailabilityChange: ChangeEventHandler<HTMLInputElement> = async (e) => {
        const newCase = { ...caseItem }
        newCase.facilitiesAvailability = Number(e.currentTarget.value)
        setCase(newCase)
    }

    const handleProducerCountChange: ChangeEventHandler<HTMLInputElement> = async (e) => {
        const newCase = { ...caseItem }
        newCase.producerCount = Number(e.currentTarget.value)
        setCase(newCase)
    }

    const handleGasInjectorCountChange: ChangeEventHandler<HTMLInputElement> = async (e) => {
        const newCase = { ...caseItem }
        newCase.gasInjectorCount = Number(e.currentTarget.value)
        setCase(newCase)
    }

    const handletWaterInjectorCountChange: ChangeEventHandler<HTMLInputElement> = async (e) => {
        const newCase = { ...caseItem }
        newCase.waterInjectorCount = Number(e.currentTarget.value)
        setCase(newCase)
    }

    const handleTemplateCountChange: ChangeEventHandler<HTMLInputElement> = async (e) => {
        const newCase = { ...caseItem }
        newCase.templateCount = Number(e.currentTarget.value)
        setCase(newCase)
    }

    const handleProductionStrategyChange: ChangeEventHandler<HTMLSelectElement> = async (e) => {
        if ([0, 1, 2, 3, 4].indexOf(Number(e.currentTarget.value)) !== -1) {
            // eslint-disable-next-line max-len
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

    const handleSave = async () => {
        const result = await (await GetCaseService()).update(caseItem)
        setCase(result)
    }

    return (
        <>
            <TopWrapper>
                <PageTitle variant="h3">Description</PageTitle>
                <Button onClick={handleSave}>Save</Button>
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
                    <CaseNumberInput
                        onChange={handleWellsChange}
                        value={caseItem.producerCount}
                        integer
                        label="Exploration wells"
                    />
                    <CaseNumberInput
                        onChange={handleWellsChange}
                        value={caseItem.producerCount}
                        integer
                        label="Appraisal wells"
                    />
                    <CaseNumberInput
                        onChange={handleProducerCountChange}
                        value={caseItem.producerCount}
                        integer
                        label="Producer count"
                    />
                    <CaseNumberInput
                        onChange={handleWellsChange}
                        value={caseItem.producerCount}
                        integer
                        label="Gas producer wells"
                    />
                    <CaseNumberInput
                        onChange={handletWaterInjectorCountChange}
                        value={caseItem.waterInjectorCount}
                        integer
                        disabled={false}
                        label="Water injector count"
                    />
                    <CaseNumberInput
                        onChange={handleGasInjectorCountChange}
                        value={caseItem.gasInjectorCount}
                        integer
                        label="Gas injector count"
                    />

                </RowWrapper>
                <RowWrapper>
                    <CaseNumberInput
                        onChange={handleTemplateCountChange}
                        value={caseItem.templateCount}
                        integer
                        label="Templates"
                    />
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
                    <NativeSelectField
                        id="artificialLift"
                        label="Artificial lidt"
                        onChange={handleArtificialLiftChange}
                        value={caseItem.artificialLift}
                    >
                        <option key="0" value={0}>No lift</option>
                        <option key="1" value={1}>Gas lift</option>
                        <option key="2" value={2}>Electrical submerged pumps</option>
                        <option key="3" value={3}>Subsea booster pumps</option>
                    </NativeSelectField>

                    <CaseNumberInput
                        onChange={handleFacilitiesAvailabilityChange}
                        value={caseItem.facilitiesAvailability}
                        integer={false}
                        label="Facilities availability (%)"
                    />
                </RowWrapper>
            </ColumnWrapper>
        </>
    )
}

export default CaseDescriptionTab

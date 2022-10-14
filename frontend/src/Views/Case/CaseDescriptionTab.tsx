import {
    MouseEventHandler, useState,
    Dispatch,
    SetStateAction,
    useEffect,
    ChangeEventHandler,
} from "react"
import styled from "styled-components"

import {
    Button, Label, NativeSelect, Switch, Typography,
} from "@equinor/eds-core-react"
import { useParams } from "react-router-dom"
import TextArea from "@equinor/fusion-react-textarea/dist/TextArea"
import { Project } from "../../models/Project"
import CaseArtificialLift from "../../Components/Case/CaseArtificialLift"
import CaseDescription from "../../Components/Case/CaseDescription"
import CaseDGDate from "../../Components/Case/CaseDGDate"
import ExcelUpload from "../../Components/ExcelUpload"
import ProductionStrategyOverview from "../../Components/ProductionStrategyOverview"
import DGEnum from "../../models/DGEnum"
import { Case } from "../../models/case/Case"
import NumberInput from "../../Components/NumberInput"
import { GetCaseService } from "../../Services/CaseService"
import CaseNumberInput from "../../Components/Case/CaseNumberInput"

const ColumnWrapper = styled.div`
    display: flex;
    flex-direction: column;
`

const RowWrapper = styled.div`
    display: flex;
    flex-direction: row;
`

const StyledButton = styled(Button)`
    color: white;
    background-color: #007079;
`

const TopWrapper = styled.div`
    display: flex;
    flex-direction: row;
    padding: 1.5rem 2rem;
`
const PageTitle = styled(Typography)`
    flex-grow: 1;
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
    const { caseId } = useParams<Record<string, string | undefined>>()

    // const [artificialLift, setArtificialLift] = useState<Components.Schemas.ArtificialLift>(0)
    // const [producerCount, setProducerCount] = useState<number>()
    // const [gasInjectorCount, setGasInjectorCount] = useState<number>()
    // const [waterInjectorCount, setWaterInjectorCount] = useState<number>()
    // const [facilitiesAvailability, setFacilitiesAvailability] = useState<number>()
    // const [productionStrategyOverview,
    //     setProductionStrategyOverview] = useState<Components.Schemas.ProductionStrategyOverview>(0)

    // const [description, setDescription] = useState<string | undefined>()

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
                <TextArea
                    id="description"
                    placeholder="Enter a description"
                    onInput={handleDescriptionChange}
                    value={caseItem.description}
                    cols={110}
                    rows={8}
                />
                <RowWrapper>
                    {/* <ProductionStrategyOverview
                    currentValue={productionStrategyOverview}
                    setProductionStrategyOverview={setProductionStrategyOverview}
                    setProject={setProject}
                    caseItem={caseItem}
                />
                <CaseArtificialLift
                    currentValue={artificialLift}
                    setArtificialLift={setArtificialLift}
                    setProject={setProject}
                    caseItem={caseItem}
                /> */}
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
                    <NativeSelect
                        id="productionStrategy"
                        label="Production strategy overview"
                        onChange={handleProductionStrategyChange}
                        value={caseItem.artificialLift}
                    >
                        <option key="0" value={0}>No lift</option>
                        <option key="1" value={1}>Gas lift</option>
                        <option key="2" value={2}>Electrical submerged pumps</option>
                        <option key="3" value={3}>Subsea booster pumps</option>
                    </NativeSelect>
                    <NativeSelect
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
                    </NativeSelect>
                    <CaseNumberInput
                        onChange={handleFacilitiesAvailabilityChange}
                        value={caseItem.facilitiesAvailability}
                        integer={false}
                        label="Facilities availability (%)"
                    />
                </RowWrapper>

                {/* <Switch
                onClick={switchReference}
                label="Reference case"
                readOnly
                checked={isReferenceCase ?? false}
            /> */}
                {/* <ExcelUpload setProject={setProject} setCase={setCase} /> */}
            </ColumnWrapper>
        </>
    )
}

export default CaseDescriptionTab

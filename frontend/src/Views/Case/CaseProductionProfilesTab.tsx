import {
    Dispatch,
    SetStateAction,
    ChangeEventHandler,
} from "react"
import styled from "styled-components"

import {
    Button, NativeSelect, Typography,
} from "@equinor/eds-core-react"
import { Project } from "../../models/Project"
import { Case } from "../../models/case/Case"
import CaseNumberInput from "../../Components/Case/CaseNumberInput"
import { DrainageStrategy } from "../../models/assets/drainagestrategy/DrainageStrategy"
import { GetDrainageStrategyService } from "../../Services/DrainageStrategyService"

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
const NativeSelectField = styled(NativeSelect)`
    width: 250px;
    padding-right: 20px;
`
const NumberInputField = styled.div`
    padding-right: 20px;
`

interface Props {
    project: Project,
    setProject: Dispatch<SetStateAction<Project | undefined>>,
    caseItem: Case,
    setCase: Dispatch<SetStateAction<Case | undefined>>,
    drainageStrategy: DrainageStrategy | undefined,
    setDrainageStrategy: Dispatch<SetStateAction<DrainageStrategy | undefined>>,
}

function CaseProductionProfilesTab({
    project, setProject,
    caseItem, setCase,
    drainageStrategy, setDrainageStrategy,
}: Props) {
    const handleDrainageStrategyNGLYieldChange: ChangeEventHandler<HTMLInputElement> = async (e) => {
        const newDrainageStrategy: DrainageStrategy = { ...drainageStrategy }
        newDrainageStrategy.nglYield = Number(e.currentTarget.value)
        setDrainageStrategy(newDrainageStrategy)
    }

    const handleSave = async () => {
        if (drainageStrategy) {
            const result = await (await GetDrainageStrategyService()).newUpdate(drainageStrategy)
            setDrainageStrategy(result)
        }
    }

    return (
        <>
            <TopWrapper>
                <PageTitle variant="h3">Production profiles</PageTitle>
                <Button onClick={handleSave}>Save</Button>
            </TopWrapper>
            <ColumnWrapper>
                <RowWrapper>
                    <NumberInputField>
                        <CaseNumberInput
                            onChange={() => console.log("Facilities availability")}
                            value={caseItem.facilitiesAvailability}
                            integer={false}
                            disabled
                            label="Facilities availability (%)"
                        />
                    </NumberInputField>
                    <NativeSelectField
                        id="gasSolution"
                        label="Gas solution"
                        onChange={() => console.log("Gas solution")}
                        value={0}
                    >
                        <option key={undefined} value={undefined}> </option>
                        <option key={0} value={0}>Export</option>
                        <option key={1} value={1}>Water injection</option>
                    </NativeSelectField>
                    <NumberInputField>
                        <CaseNumberInput
                            onChange={handleDrainageStrategyNGLYieldChange}
                            value={drainageStrategy?.nglYield}
                            integer={false}
                            label="NGL yield"
                        />
                    </NumberInputField>
                    <NumberInputField>
                        <CaseNumberInput
                            onChange={() => console.log("Condensate yield")}
                            value={drainageStrategy?.nglYield}
                            integer={false}
                            label="Condensate yield"
                        />
                    </NumberInputField>
                    <NativeSelectField
                        id="gasShrinkageFactor"
                        label="Gas shrinkage factor"
                        onChange={() => console.log("Gas shrinkage factor")}
                        disabled
                        value={0}
                    >
                        <option key={undefined} value={undefined}> </option>
                        <option key={0} value={0}>0</option>
                        <option key={1} value={1}>Water injection</option>
                    </NativeSelectField>
                </RowWrapper>
                <RowWrapper>
                    <CaseNumberInput
                        onChange={() => console.log("Sales gas GCV (gross calorific value)")}
                        value={drainageStrategy?.nglYield}
                        integer={false}
                        label="Sales gas GCV (gross calorific value)"
                    />
                </RowWrapper>
            </ColumnWrapper>
        </>
    )
}

export default CaseProductionProfilesTab

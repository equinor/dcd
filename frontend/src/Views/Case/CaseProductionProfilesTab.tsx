import {
    Dispatch,
    SetStateAction,
    ChangeEventHandler,
    useState,
    useEffect,
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
import CaseProductionProfilesTabTable from "./CaseProductionProfilesTabTable"
import { NetSalesGas } from "../../models/assets/drainagestrategy/NetSalesGas"
import { FuelFlaringAndLosses } from "../../models/assets/drainagestrategy/FuelFlaringAndLosses"
import { ProductionProfileGas } from "../../models/assets/drainagestrategy/ProductionProfileGas"
import { ProductionProfileOil } from "../../models/assets/drainagestrategy/ProductionProfileOil"
import { ProductionProfileWater } from "../../models/assets/drainagestrategy/ProductionProfileWater"
import { ProductionProfileNGL } from "../../models/assets/drainagestrategy/ProductionProfileNGL"
import { ProductionProfileWaterInjection } from "../../models/assets/drainagestrategy/ProductionProfileWaterInjection"

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
    width: 200px;
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
    const [netSalesGas, setNetSalesGas] = useState<NetSalesGas>()
    const [fuelFlaringAndLosses, setFuelFlaringAndLosses] = useState<FuelFlaringAndLosses>()
    const [gas, setGas] = useState<ProductionProfileGas>()
    const [oil, setOil] = useState<ProductionProfileOil>()
    const [water, setWater] = useState<ProductionProfileWater>()
    const [nGL, setNGL] = useState<ProductionProfileNGL>()
    const [waterInjection, setWaterInjection] = useState<ProductionProfileWaterInjection>()

    const [firstYear, setFirstYear] = useState<number>(2020)
    const [lastYear, setLastYear] = useState<number>(2030)

    const updateAndSetDraiangeStrategy = (drainage: DrainageStrategy) => {
        const newDrainageStrategy: DrainageStrategy = { ...drainage }
        newDrainageStrategy.netSalesGas = netSalesGas
        newDrainageStrategy.fuelFlaringAndLosses = fuelFlaringAndLosses
        newDrainageStrategy.productionProfileGas = gas
        newDrainageStrategy.productionProfileOil = oil
        newDrainageStrategy.productionProfileWater = water
        newDrainageStrategy.productionProfileNGL = nGL
        newDrainageStrategy.productionProfileWaterInjection = waterInjection
        setDrainageStrategy(newDrainageStrategy)
    }

    const handleDrainageStrategyNGLYieldChange: ChangeEventHandler<HTMLInputElement> = async (e) => {
        const newDrainageStrategy: DrainageStrategy = { ...drainageStrategy }
        newDrainageStrategy.nglYield = Number(e.currentTarget.value)
        updateAndSetDraiangeStrategy(newDrainageStrategy)
    }

    const timeSeriesData = [
        {
            profileName: "Net sales gas", unit: "GSm3/yr", set: setNetSalesGas, profile: netSalesGas,
        },
        {
            profileName: "Fuel flaring and losses",
            unit: "GSm3/yr",
            set: setFuelFlaringAndLosses,
            profile: fuelFlaringAndLosses,
        },
        {
            profileName: "Gas production", unit: "GSm3/yr", set: setGas, profile: gas,
        },
        {
            profileName: "Oil production", unit: "MSm3/yr", set: setOil, profile: oil,
        },
        {
            profileName: "Water production", unit: "MSm3/yr", set: setWater, profile: water,
        },
        {
            profileName: "Water injection", unit: "MSm3/yr", set: setWaterInjection, profile: waterInjection,
        },
    ]

    useEffect(() => {
        if (drainageStrategy) {
            setNetSalesGas(drainageStrategy.netSalesGas)
            setFuelFlaringAndLosses(drainageStrategy.fuelFlaringAndLosses)
            setGas(drainageStrategy.productionProfileGas)
            setOil(drainageStrategy.productionProfileOil)
            setWater(drainageStrategy.productionProfileWater)
            setNGL(drainageStrategy.productionProfileNGL)
            setWaterInjection(drainageStrategy.productionProfileWaterInjection)
        }
    }, [drainageStrategy])

    const handleSave = async () => {
        if (drainageStrategy) {
            const newDrainageStrategy: DrainageStrategy = { ...drainageStrategy }
            newDrainageStrategy.netSalesGas = netSalesGas
            newDrainageStrategy.fuelFlaringAndLosses = fuelFlaringAndLosses
            newDrainageStrategy.productionProfileGas = gas
            newDrainageStrategy.productionProfileOil = oil
            newDrainageStrategy.productionProfileWater = water
            newDrainageStrategy.productionProfileNGL = nGL
            newDrainageStrategy.productionProfileWaterInjection = waterInjection
            const result = await (await GetDrainageStrategyService()).newUpdate(newDrainageStrategy)
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
                        <option key={1} value={1}>Injection</option>
                    </NativeSelectField>
                    <NumberInputField>
                        <CaseNumberInput
                            onChange={handleDrainageStrategyNGLYieldChange}
                            value={drainageStrategy?.nglYield}
                            integer={false}
                            label="NGL yield"
                        />
                    </NumberInputField>
                </RowWrapper>
            </ColumnWrapper>
            {drainageStrategy && (
                <CaseProductionProfilesTabTable
                    caseItem={caseItem}
                    project={project}
                    setCase={setCase}
                    setProject={setProject}
                    timeSeriesData={timeSeriesData}
                    dg4Year={caseItem.DG4Date.getFullYear()}
                    firstYear={firstYear}
                    lastYear={lastYear}
                />
            )}
        </>
    )
}

export default CaseProductionProfilesTab

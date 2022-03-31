/* eslint-disable max-len */
import { Button, Input, Typography } from "@equinor/eds-core-react"
import { useEffect, useState } from "react"
import {
    useLocation, useNavigate, useParams,
} from "react-router"
import styled from "styled-components"
import DataTable, { CellValue } from "../Components/DataTable/DataTable"
import {
    buildGridData, getColumnAbsoluteYears, replaceOldData,
} from "../Components/DataTable/helpers"
import Import from "../Components/Import/Import"
import { DrainageStrategy } from "../models/assets/drainagestrategy/DrainageStrategy"
import { Project } from "../models/Project"
import { Case } from "../models/Case"

import { GetProjectService } from "../Services/ProjectService"
import { GetDrainageStrategyService } from "../Services/DrainageStrategyService"
import { Co2EmissionsCostProfile } from "../models/assets/drainagestrategy/Co2EmissionsCostProfile"

const AssetHeader = styled.div`
    margin-bottom: 2rem;
    display: flex;

    > *:first-child {
        margin-right: 2rem;
    }
`

const AssetViewDiv = styled.div`
    margin: 2rem;
    display: flex;
    flex-direction: column;
`

const Wrapper = styled.div`
    display: flex;
    flex-direction: row;
`

const WrapperColumn = styled.div`
    display: flex;
    flex-direction: column;
`

const ImportButton = styled(Button)`
    margin-left: 2rem;
    &:disabled {
        margin-left: 2rem;
    }
`

const SaveButton = styled(Button)`
    margin-top: 5rem;
    margin-left: 2rem;
    &:disabled {
        margin-left: 2rem;
        margin-top: 5rem;
    }
`

const Dg4Field = styled.div`
    margin-left: 1rem;
    margin-bottom: 2rem;
    width: 10rem;
    display: flex;
`

const DrainageStrategyView = () => {
    const [, setProject] = useState<Project>()
    const [caseItem, setCase] = useState<Case>()
    const [drainageStrategy, setDrainageStrategy] = useState<DrainageStrategy>()
    // Artificial Lift. Tror ikke dette er Grid Excel import...
    const [artificialLiftColumns, setArtificialLiftColumns] = useState<string[]>([""])
    const [artificialLiftGridData, setArtificialLiftGridData] = useState<CellValue[][]>([[]])
    const [artificialLiftCostProfileDialogOpen, setArtificialLiftCostProfileDialogOpen] = useState(false)

    // co2Emissions
    const [co2EmissionsColumns, setCo2EmissionsColumns] = useState<string[]>([""])
    const [co2EmissionsGridData, setCo2EmissionsGridData] = useState<CellValue[][]>([[]])
    const [co2EmissionsCostProfileDialogOpen, setCo2EmissionsCostProfileDialogOpen] = useState(false)

    // fuel flaring and losses
    const [fuelFlaringAndLossesColumns, setFuelFlaringAndLossesColumns] = useState<string[]>([""])
    const [fuelFlaringAndLossesGridData, setFuelFlaringAndLossesGridData] = useState<CellValue[][]>([[]])
    const [fuelFlaringAndLossesCostProfileDialogOpen, setFuelFlaringAndLossesCostProfileDialogOpen] = useState(false)

    // Netsalesgas
    const [netSalesGasColumns, setNetSalesGasColumns] = useState<string[]>([""])
    const [netSalesGasGridData, setNetSalesGasGridData] = useState<CellValue[][]>([[]])
    const [netSalesGasCostProfileDialogOpen, setNetSalesGasCostProfileDialogOpen] = useState(false)

    // ProductionProfileGas
    const [productionProfileGasColumns, setProductionProfileGasColumns] = useState<string[]>([""])
    const [productionProfileGasGridData, setProductionProfileGasGridData] = useState<CellValue[][]>([[]])
    const [productionProfileGasCostProfileDialogOpen, setProductionProfileGasCostProfileDialogOpen] = useState(false)

    // ProductionProfileOil
    const [productionProfileOilColumns, setProductionProfileOilColumns] = useState<string[]>([""])
    const [productionProfileOilGridData, setProductionProfileOilGridData] = useState<CellValue[][]>([[]])
    const [productionProfileOilCostProfileDialogOpen, setProductionProfileOilCostProfileDialogOpen] = useState(false)

    // ProductionProfileOil
    const [productionProfileWaterColumns, setProductionProfileWaterColumns] = useState<string[]>([""])
    const [productionProfileWaterGridData, setProductionProfileWaterGridData] = useState<CellValue[][]>([[]])
    const [productionProfileWaterCostProfileDialogOpen, setProductionProfileWaterCostProfileDialogOpen] = useState(false)

    // ProductionProfileOil
    const [productionProfileWaterInjectionColumns, setProductionProfileWaterInjectionColumns] = useState<string[]>([""])
    const [productionProfileWaterInjectionGridData, setProductionProfileWaterInjectionGridData] = useState<CellValue[][]>([[]])
    const [productionProfileWaterInjectionCostProfileDialogOpen, setProductionProfileWaterInjectionCostProfileDialogOpen] = useState(false)

    const [hasChanges, setHasChanges] = useState(false)
    const params = useParams()
    const navigate = useNavigate()
    const location = useLocation()

    const emptyGuid = "00000000-0000-0000-0000-000000000000"

    useEffect(() => {
        (async () => {
            try {
                const projectResult = await GetProjectService().getProjectByID(params.projectId!)
                setProject(projectResult)
                const caseResult = projectResult.cases.find((o) => o.id === params.caseId)
                setCase(caseResult)
                let newDrainageStrategy = projectResult.drainageStrategies.find((s) => s.id === params.drainageStrategyId)
                if (newDrainageStrategy !== undefined) {
                    setDrainageStrategy(newDrainageStrategy)
                } else {
                    newDrainageStrategy = new DrainageStrategy()
                    setDrainageStrategy(newDrainageStrategy)
                }
                const newCo2EmissionsColumnTitles = getColumnAbsoluteYears(caseResult, newDrainageStrategy?.co2Emissions)
                const newFuelFlaringAndLossesColumnTitles = getColumnAbsoluteYears(caseResult, newDrainageStrategy?.fuelFlaringAndLosses)
                const newNetSalesGasColumnTitles = getColumnAbsoluteYears(caseResult, newDrainageStrategy?.netSalesGas)
                const newProductionProfileGasColumnTitles = getColumnAbsoluteYears(caseResult, newDrainageStrategy?.productionProfileGas)
                const newProductionProfileOilColumnTitles = getColumnAbsoluteYears(caseResult, newDrainageStrategy?.productionProfileOil)
                const newProductionProfileWaterTitles = getColumnAbsoluteYears(caseResult, newDrainageStrategy?.productionProfileWater)
                const newProductionProfileWaterInjectionColumnTitles = getColumnAbsoluteYears(caseResult, newDrainageStrategy?.productionProfileWaterInjection)

                setCo2EmissionsColumns(newCo2EmissionsColumnTitles)
                setFuelFlaringAndLossesColumns(newFuelFlaringAndLossesColumnTitles)
                setNetSalesGasColumns(newNetSalesGasColumnTitles)
                setProductionProfileGasColumns(newProductionProfileGasColumnTitles)
                setProductionProfileOilColumns(newProductionProfileOilColumnTitles)
                setProductionProfileWaterColumns(newProductionProfileWaterTitles)
                setProductionProfileWaterInjectionColumns(newProductionProfileWaterInjectionColumnTitles)

                const newCo2EmissionsGridData = buildGridData(newDrainageStrategy?.co2Emissions)
                const newFuelAndFlaringLossesGridData = buildGridData(newDrainageStrategy?.fuelFlaringAndLosses)
                const newNetSalesGasGridData = buildGridData(newDrainageStrategy?.netSalesGas)
                const newProductionProfileGasGridData = buildGridData(newDrainageStrategy?.productionProfileGas)
                const newProductionProfileOilGridData = buildGridData(newDrainageStrategy?.productionProfileOil)
                const newProductionProfileWaterGridData = buildGridData(newDrainageStrategy?.productionProfileWater)
                const newProductionProileWaterInjectionGridData = buildGridData(newDrainageStrategy?.productionProfileWaterInjection)

                setCo2EmissionsGridData(newCo2EmissionsGridData)
                setFuelFlaringAndLossesGridData(newFuelAndFlaringLossesGridData)
                setNetSalesGasGridData(newNetSalesGasGridData)
                setProductionProfileGasGridData(newProductionProfileGasGridData)
                setProductionProfileOilGridData(newProductionProfileOilGridData)
                setProductionProfileWaterGridData(newProductionProfileWaterGridData)
                setProductionProfileWaterInjectionGridData(newProductionProileWaterInjectionGridData)
            } catch (error) {
                console.error(`[CaseView] Error while fetching project ${params.projectId}`, error)
            }
        })()
    }, [params.projectId, params.caseId])

    const onCo2EmissionsCellsChanged = (changes: { cell: { value: number }; col: number; row: number; value: string }[]) => {
        const newGridData = replaceOldData(co2EmissionsGridData, changes)
        setCo2EmissionsGridData(newGridData)
        setCo2EmissionsColumns(getColumnAbsoluteYears(caseItem, drainageStrategy?.co2Emissions))
        setHasChanges(true)
    }

    const onFuelFlaringAndLossesCellsChanged = (changes: { cell: { value: number }; col: number; row: number; value: string }[]) => {
        const newGridData = replaceOldData(fuelFlaringAndLossesGridData, changes)
        setFuelFlaringAndLossesGridData(newGridData)
        setFuelFlaringAndLossesColumns(getColumnAbsoluteYears(caseItem, drainageStrategy?.fuelFlaringAndLosses))
        setHasChanges(true)
    }

    const onNetSalesGasCellsChanged = (changes: { cell: { value: number }; col: number; row: number; value: string }[]) => {
        const newGridData = replaceOldData(netSalesGasGridData, changes)
        setNetSalesGasGridData(newGridData)
        setNetSalesGasColumns(getColumnAbsoluteYears(caseItem, drainageStrategy?.netSalesGas))
        setHasChanges(true)
    }

    const onProductionProfileGasCellsChanged = (changes: { cell: { value: number }; col: number; row: number; value: string }[]) => {
        const newGridData = replaceOldData(productionProfileGasGridData, changes)
        setProductionProfileGasGridData(newGridData)
        setProductionProfileGasColumns(getColumnAbsoluteYears(caseItem, drainageStrategy?.productionProfileGas))
        setHasChanges(true)
    }

    const onProductionProfileOilCellsChanged = (changes: { cell: { value: number }; col: number; row: number; value: string }[]) => {
        const newGridData = replaceOldData(productionProfileOilGridData, changes)
        setProductionProfileOilGridData(newGridData)
        setProductionProfileOilColumns(getColumnAbsoluteYears(caseItem, drainageStrategy?.productionProfileOil))
        setHasChanges(true)
    }

    const onProductionProfileWaterCellsChanged = (changes: { cell: { value: number }; col: number; row: number; value: string }[]) => {
        const newGridData = replaceOldData(productionProfileWaterGridData, changes)
        setProductionProfileWaterGridData(newGridData)
        setProductionProfileWaterColumns(getColumnAbsoluteYears(caseItem, drainageStrategy?.productionProfileWater))
        setHasChanges(true)
    }

    const onProductionProfileWaterInjectionCellsChanged = (changes: { cell: { value: number }; col: number; row: number; value: string }[]) => {
        const newGridData = replaceOldData(productionProfileWaterInjectionGridData, changes)
        setProductionProfileWaterInjectionGridData(newGridData)
        setProductionProfileWaterInjectionColumns(getColumnAbsoluteYears(caseItem, drainageStrategy?.productionProfileWaterInjection))
        setHasChanges(true)
    }

    const onImportCo2Emissions = (input: string, year: number) => {
        const newDrainageStrategy = DrainageStrategy.Copy(drainageStrategy!)
        if (newDrainageStrategy.co2Emissions === undefined) {
            newDrainageStrategy.co2Emissions = new Co2EmissionsCostProfile()
        }
        newDrainageStrategy.co2Emissions!.startYear = year
        newDrainageStrategy.co2Emissions!.values = input.replace(/(\r\n|\n|\r)/gm, "").split("\t").map((i) => parseFloat(i))
        setDrainageStrategy(newDrainageStrategy)
        const newCo2EmissionsTitles = getColumnAbsoluteYears(caseItem, newDrainageStrategy?.co2Emissions)
        setCo2EmissionsColumns(newCo2EmissionsTitles)
        const newGridData = buildGridData(newDrainageStrategy?.co2Emissions)
        setCo2EmissionsGridData(newGridData)
        setCo2EmissionsCostProfileDialogOpen(!co2EmissionsCostProfileDialogOpen)
        setHasChanges(true)
    }

    const onImportFuelFlaringAndLosses = (input: string, year: number) => {
        const newDrainageStrategy = DrainageStrategy.Copy(drainageStrategy!)
        newDrainageStrategy.fuelFlaringAndLosses!.startYear = year
        newDrainageStrategy.fuelFlaringAndLosses!.values = input.replace(/(\r\n|\n|\r)/gm, "").split("\t").map((i) => parseFloat(i))
        setDrainageStrategy(newDrainageStrategy)
        const newFuelFlaringAndLossesTitles = getColumnAbsoluteYears(caseItem, newDrainageStrategy?.fuelFlaringAndLosses)
        setFuelFlaringAndLossesColumns(newFuelFlaringAndLossesTitles)
        const newGridData = buildGridData(newDrainageStrategy?.fuelFlaringAndLosses)
        setFuelFlaringAndLossesGridData(newGridData)
        setFuelFlaringAndLossesCostProfileDialogOpen(!fuelFlaringAndLossesCostProfileDialogOpen)
        setHasChanges(true)
    }

    const onImportNetSalesGas = (input: string, year: number) => {
        const newDrainageStrategy = DrainageStrategy.Copy(drainageStrategy!)
        newDrainageStrategy.netSalesGas!.startYear = year
        newDrainageStrategy.netSalesGas!.values = input.replace(/(\r\n|\n|\r)/gm, "").split("\t").map((i) => parseFloat(i))
        setDrainageStrategy(newDrainageStrategy)
        const newNetSalesGasTitles = getColumnAbsoluteYears(caseItem, newDrainageStrategy?.netSalesGas)
        setNetSalesGasColumns(newNetSalesGasTitles)
        const newGridData = buildGridData(newDrainageStrategy?.netSalesGas)
        setNetSalesGasGridData(newGridData)
        setNetSalesGasCostProfileDialogOpen(!netSalesGasCostProfileDialogOpen)
        setHasChanges(true)
    }

    const onImportProductionProfileGas = (input: string, year: number) => {
        const newDrainageStrategy = DrainageStrategy.Copy(drainageStrategy!)
        newDrainageStrategy.productionProfileGas!.startYear = year
        newDrainageStrategy.productionProfileGas!.values = input.replace(/(\r\n|\n|\r)/gm, "").split("\t").map((i) => parseFloat(i))
        setDrainageStrategy(newDrainageStrategy)
        const newProductionProfileGasTitles = getColumnAbsoluteYears(caseItem, newDrainageStrategy?.productionProfileGas)
        setProductionProfileGasColumns(newProductionProfileGasTitles)
        const newGridData = buildGridData(newDrainageStrategy?.productionProfileGas)
        setProductionProfileGasGridData(newGridData)
        setProductionProfileGasCostProfileDialogOpen(!productionProfileGasCostProfileDialogOpen)
        setHasChanges(true)
    }

    const onImportProductionProfileOil = (input: string, year: number) => {
        const newDrainageStrategy = DrainageStrategy.Copy(drainageStrategy!)
        newDrainageStrategy.productionProfileOil!.startYear = year
        newDrainageStrategy.productionProfileOil!.values = input.replace(/(\r\n|\n|\r)/gm, "").split("\t").map((i) => parseFloat(i))
        setDrainageStrategy(newDrainageStrategy)
        const newProductionProfileOilTitles = getColumnAbsoluteYears(caseItem, newDrainageStrategy?.productionProfileOil)
        setProductionProfileOilColumns(newProductionProfileOilTitles)
        const newGridData = buildGridData(newDrainageStrategy?.productionProfileOil)
        setProductionProfileOilGridData(newGridData)
        setProductionProfileOilCostProfileDialogOpen(!productionProfileOilCostProfileDialogOpen)
        setHasChanges(true)
    }

    const onImportProductionProfileWater = (input: string, year: number) => {
        const newDrainageStrategy = DrainageStrategy.Copy(drainageStrategy!)
        newDrainageStrategy.productionProfileWater!.startYear = year
        newDrainageStrategy.productionProfileWater!.values = input.replace(/(\r\n|\n|\r)/gm, "").split("\t").map((i) => parseFloat(i))
        setDrainageStrategy(newDrainageStrategy)
        const newProductionProfileWaterTitles = getColumnAbsoluteYears(caseItem, newDrainageStrategy?.productionProfileWater)
        setProductionProfileWaterColumns(newProductionProfileWaterTitles)
        const newGridData = buildGridData(newDrainageStrategy?.productionProfileWater)
        setProductionProfileWaterGridData(newGridData)
        setProductionProfileWaterCostProfileDialogOpen(!productionProfileWaterCostProfileDialogOpen)
        setHasChanges(true)
    }

    const onImportProductionProfileWaterInjection = (input: string, year: number) => {
        const newDrainageStrategy = DrainageStrategy.Copy(drainageStrategy!)
        newDrainageStrategy.productionProfileWaterInjection!.startYear = year
        newDrainageStrategy.productionProfileWaterInjection!.values = input.replace(/(\r\n|\n|\r)/gm, "").split("\t").map((i) => parseFloat(i))
        setDrainageStrategy(newDrainageStrategy)
        const newProductionProfileWaterInjectionTitles = getColumnAbsoluteYears(caseItem, newDrainageStrategy?.productionProfileWaterInjection)
        setProductionProfileWaterInjectionColumns(newProductionProfileWaterInjectionTitles)
        const newGridData = buildGridData(newDrainageStrategy?.productionProfileWaterInjection)
        setProductionProfileWaterInjectionGridData(newGridData)
        setProductionProfileWaterInjectionCostProfileDialogOpen(!productionProfileWaterInjectionCostProfileDialogOpen)
        setHasChanges(true)
    }

    const handleSave = async () => {
        const drainageStrategyDto = DrainageStrategy.toDto(drainageStrategy!)
        if (drainageStrategyDto?.id === emptyGuid) {
            drainageStrategyDto.projectId = params.projectId
            const newProject: Project = await GetDrainageStrategyService().createDrainageStrategy(params.caseId!, drainageStrategyDto!)
            const newDrainageStrategy = newProject.drainageStrategies.at(-1)
            const newUrl = location.pathname.replace(emptyGuid, newDrainageStrategy!.id!)
            const newCase = newProject.cases.find((o) => o.id === params.caseId)
            setDrainageStrategy(newDrainageStrategy)
            setCase(newCase)
            navigate(`${newUrl}`, { replace: true })
        } else {
            drainageStrategyDto.projectId = params.projectId
            const newProject = await GetDrainageStrategyService().updateDrainageStrategy(drainageStrategyDto!)
            setProject(newProject)
            const newCase = newProject.cases.find((o) => o.id === params.caseId)
            setCase(newCase)
            const newDrainageStrategy = newProject.drainageStrategies.find((s) => s.id === params.drainageStrategyId)
            setDrainageStrategy(newDrainageStrategy)
        }
        setHasChanges(false)
    }

    return (
        <AssetViewDiv>
            <AssetHeader>
                <Typography variant="h2">{drainageStrategy?.name}</Typography>
            </AssetHeader>
            <Wrapper>
                <Typography variant="h4">DG4</Typography>
                <Dg4Field>
                    <Input disabled defaultValue={caseItem?.DG4Date?.toLocaleDateString("en-CA")} type="date" />
                </Dg4Field>
            </Wrapper>
            <Wrapper>
                <Typography variant="h4"> Co2 Emissions </Typography>
                <ImportButton onClick={() => { setCo2EmissionsCostProfileDialogOpen(true) }}>Import</ImportButton>
            </Wrapper>
            <WrapperColumn>
                <DataTable columns={co2EmissionsColumns} gridData={co2EmissionsGridData} onCellsChanged={onCo2EmissionsCellsChanged} />
            </WrapperColumn>
            {!co2EmissionsCostProfileDialogOpen ? null
                : <Import onClose={() => { setCo2EmissionsCostProfileDialogOpen(!co2EmissionsCostProfileDialogOpen) }} onImport={onImportCo2Emissions} />}
            <Wrapper><SaveButton disabled={!hasChanges} onClick={handleSave}>Save</SaveButton></Wrapper>
        </AssetViewDiv>
    )
}

export default DrainageStrategyView

/* eslint-disable max-len */
/* Ignored as we will be refactoring this code */
import {
    Button, Input, Typography, Label,
} from "@equinor/eds-core-react"
import { ChangeEventHandler, useEffect, useState } from "react"
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
import { FuelFlaringAndLossesCostProfile } from "../models/assets/drainagestrategy/FuelFlaringAndLossesCostProfile"
import { NetSalesGasCostProfile } from "../models/assets/drainagestrategy/NetSalesGasCostProfile"
import { ProductionProfileGasCostProfile } from "../models/assets/drainagestrategy/ProductionProfileGasCostProfile"
import { ProductionProfileOilCostProfile } from "../models/assets/drainagestrategy/ProductionProfileOilCostProfile"
import { ProductionProfileWaterCostProfile } from "../models/assets/drainagestrategy/ProductionProfileWaterCostProfile"
import { ProductionProfileWaterInjectionCostProfile } from "../models/assets/drainagestrategy/ProductionProfileWaterInjectionCostProfile"

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
    margin-top: 1rem;
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
    const [drainageStrategyName, setDrainageStrategyName] = useState<string>("")

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
                setDrainageStrategyName(newDrainageStrategy.name!)

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
        if (newDrainageStrategy.fuelFlaringAndLosses === undefined) {
            newDrainageStrategy.fuelFlaringAndLosses = new FuelFlaringAndLossesCostProfile()
        }
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
        if (newDrainageStrategy.netSalesGas === undefined) {
            newDrainageStrategy.netSalesGas = new NetSalesGasCostProfile()
        }
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
        if (newDrainageStrategy.productionProfileGas === undefined) {
            newDrainageStrategy.productionProfileGas = new ProductionProfileGasCostProfile()
        }
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
        if (newDrainageStrategy.productionProfileOil === undefined) {
            newDrainageStrategy.productionProfileOil = new ProductionProfileOilCostProfile()
        }
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
        if (newDrainageStrategy.productionProfileWater === undefined) {
            newDrainageStrategy.productionProfileWater = new ProductionProfileWaterCostProfile()
        }
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
        if (newDrainageStrategy.productionProfileWaterInjection === undefined) {
            newDrainageStrategy.productionProfileWaterInjection = new ProductionProfileWaterInjectionCostProfile()
        }
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
        drainageStrategyDto.name = drainageStrategyName
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

    const handleDrainageStrategyNameFieldChange: ChangeEventHandler<HTMLInputElement> = async (e) => {
        setDrainageStrategyName(e.target.value)
        if (e.target.value !== undefined && e.target.value !== "" && e.target.value !== drainageStrategy?.name) {
            setHasChanges(true)
        } else {
            setHasChanges(false)
        }
    }

    const deleteCo2EmissionsCostProfile = () => {
        const drainageCopy = new DrainageStrategy(drainageStrategy)
        drainageCopy.co2Emissions = undefined
        setHasChanges(true)
        setCo2EmissionsColumns([])
        setCo2EmissionsGridData([[]])
        setDrainageStrategy(drainageCopy)
    }

    const deleteFuelFlaringAndLossesCostProfile = () => {
        const drainageCopy = new DrainageStrategy(drainageStrategy)
        drainageCopy.fuelFlaringAndLosses = undefined
        setHasChanges(true)
        setFuelFlaringAndLossesColumns([])
        setFuelFlaringAndLossesGridData([[]])
        setDrainageStrategy(drainageCopy)
    }

    const deleteNetSalesGasCostProfile = () => {
        const drainageCopy = new DrainageStrategy(drainageStrategy)
        drainageCopy.netSalesGas = undefined
        setHasChanges(true)
        setNetSalesGasColumns([])
        setNetSalesGasGridData([[]])
        setDrainageStrategy(drainageCopy)
    }

    const deleteProductionProfileGasCostProfile = () => {
        const drainageCopy = new DrainageStrategy(drainageStrategy)
        drainageCopy.productionProfileGas = undefined
        setHasChanges(true)
        setProductionProfileGasColumns([])
        setProductionProfileGasGridData([[]])
        setDrainageStrategy(drainageCopy)
    }

    const deleteProductionProfileOilCostProfile = () => {
        const drainageCopy = new DrainageStrategy(drainageStrategy)
        drainageCopy.productionProfileOil = undefined
        setHasChanges(true)
        setProductionProfileOilColumns([])
        setProductionProfileOilGridData([[]])
        setDrainageStrategy(drainageCopy)
    }

    const deleteProductionProfileWaterCostProfile = () => {
        const drainageCopy = new DrainageStrategy(drainageStrategy)
        drainageCopy.productionProfileWater = undefined
        setHasChanges(true)
        setProductionProfileWaterColumns([])
        setProductionProfileWaterGridData([[]])
        setDrainageStrategy(drainageCopy)
    }

    const deleteProductionProfileWaterInjectionCostProfile = () => {
        const drainageCopy = new DrainageStrategy(drainageStrategy)
        drainageCopy.productionProfileWaterInjection = undefined
        setHasChanges(true)
        setProductionProfileWaterInjectionColumns([])
        setProductionProfileWaterInjectionGridData([[]])
        setDrainageStrategy(drainageCopy)
    }

    return (
        <AssetViewDiv>
            <AssetHeader>
                <WrapperColumn>
                    <Label htmlFor="drainagStrategyName" label="Name" />
                    <Input
                        id="drainagStrategyName"
                        name="drainagStrategyName"
                        placeholder="Enter Drainage Strategy name"
                        defaultValue={drainageStrategy?.name}
                        onChange={handleDrainageStrategyNameFieldChange}
                    />
                </WrapperColumn>
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
                <ImportButton disabled={drainageStrategy?.co2Emissions === undefined} color="danger" onClick={deleteCo2EmissionsCostProfile}>Delete</ImportButton>
            </Wrapper>
            <WrapperColumn>
                <DataTable columns={co2EmissionsColumns} gridData={co2EmissionsGridData} onCellsChanged={onCo2EmissionsCellsChanged} dG4Year={caseItem?.DG4Date?.getFullYear().toString()!} />
            </WrapperColumn>
            {!co2EmissionsCostProfileDialogOpen ? null
                : <Import onClose={() => { setCo2EmissionsCostProfileDialogOpen(!co2EmissionsCostProfileDialogOpen) }} onImport={onImportCo2Emissions} />}

            <Wrapper>
                <Typography variant="h4"> Fuel Flaring And Losses </Typography>
                <ImportButton onClick={() => { setFuelFlaringAndLossesCostProfileDialogOpen(true) }}>Import</ImportButton>
                <ImportButton disabled={drainageStrategy?.fuelFlaringAndLosses === undefined} color="danger" onClick={deleteFuelFlaringAndLossesCostProfile}>Delete</ImportButton>

            </Wrapper>
            <WrapperColumn>
                <DataTable columns={fuelFlaringAndLossesColumns} gridData={fuelFlaringAndLossesGridData} onCellsChanged={onFuelFlaringAndLossesCellsChanged} dG4Year={caseItem?.DG4Date?.getFullYear().toString()!} />
            </WrapperColumn>
            {!fuelFlaringAndLossesCostProfileDialogOpen ? null
                : <Import onClose={() => { setFuelFlaringAndLossesCostProfileDialogOpen(!fuelFlaringAndLossesCostProfileDialogOpen) }} onImport={onImportFuelFlaringAndLosses} />}

            <Wrapper>
                <Typography variant="h4"> Net Sales Gas </Typography>
                <ImportButton onClick={() => { setNetSalesGasCostProfileDialogOpen(true) }}>Import</ImportButton>
                <ImportButton disabled={drainageStrategy?.netSalesGas === undefined} color="danger" onClick={deleteNetSalesGasCostProfile}>Delete</ImportButton>

            </Wrapper>
            <WrapperColumn>
                <DataTable columns={netSalesGasColumns} gridData={netSalesGasGridData} onCellsChanged={onNetSalesGasCellsChanged} dG4Year={caseItem?.DG4Date?.getFullYear().toString()!} />
            </WrapperColumn>
            {!netSalesGasCostProfileDialogOpen ? null
                : <Import onClose={() => { setNetSalesGasCostProfileDialogOpen(!netSalesGasCostProfileDialogOpen) }} onImport={onImportNetSalesGas} />}

            <Wrapper>
                <Typography variant="h4"> Production Profile Gas </Typography>
                <ImportButton onClick={() => { setProductionProfileGasCostProfileDialogOpen(true) }}>Import</ImportButton>
                <ImportButton disabled={drainageStrategy?.productionProfileGas === undefined} color="danger" onClick={deleteProductionProfileGasCostProfile}>Delete</ImportButton>

            </Wrapper>
            <WrapperColumn>
                <DataTable columns={productionProfileGasColumns} gridData={productionProfileGasGridData} onCellsChanged={onProductionProfileGasCellsChanged} dG4Year={caseItem?.DG4Date?.getFullYear().toString()!} />
            </WrapperColumn>
            {!productionProfileGasCostProfileDialogOpen ? null
                : <Import onClose={() => { setProductionProfileGasCostProfileDialogOpen(!productionProfileGasCostProfileDialogOpen) }} onImport={onImportProductionProfileGas} />}

            <Wrapper>
                <Typography variant="h4"> Production Profile Oil </Typography>
                <ImportButton onClick={() => { setProductionProfileOilCostProfileDialogOpen(true) }}>Import</ImportButton>
                <ImportButton disabled={drainageStrategy?.productionProfileOil === undefined} color="danger" onClick={deleteProductionProfileOilCostProfile}>Delete</ImportButton>

            </Wrapper>
            <WrapperColumn>
                <DataTable columns={productionProfileOilColumns} gridData={productionProfileOilGridData} onCellsChanged={onProductionProfileOilCellsChanged} dG4Year={caseItem?.DG4Date?.getFullYear().toString()!} />
            </WrapperColumn>
            {!productionProfileOilCostProfileDialogOpen ? null
                : <Import onClose={() => { setProductionProfileOilCostProfileDialogOpen(!productionProfileOilCostProfileDialogOpen) }} onImport={onImportProductionProfileOil} />}

            <Wrapper>
                <Typography variant="h4"> Production Profile Water </Typography>
                <ImportButton onClick={() => { setProductionProfileWaterCostProfileDialogOpen(true) }}>Import</ImportButton>
                <ImportButton disabled={drainageStrategy?.productionProfileWater === undefined} color="danger" onClick={deleteProductionProfileWaterCostProfile}>Delete</ImportButton>

            </Wrapper>
            <WrapperColumn>
                <DataTable columns={productionProfileWaterColumns} gridData={productionProfileWaterGridData} onCellsChanged={onProductionProfileWaterCellsChanged} dG4Year={caseItem?.DG4Date?.getFullYear().toString()!} />
            </WrapperColumn>
            {!productionProfileWaterCostProfileDialogOpen ? null
                : <Import onClose={() => { setProductionProfileWaterCostProfileDialogOpen(!productionProfileWaterCostProfileDialogOpen) }} onImport={onImportProductionProfileWater} />}

            <Wrapper>
                <Typography variant="h4"> Production Profile Water Injection </Typography>
                <ImportButton onClick={() => { setProductionProfileWaterInjectionCostProfileDialogOpen(true) }}>Import</ImportButton>
                <ImportButton disabled={drainageStrategy?.productionProfileWaterInjection === undefined} color="danger" onClick={deleteProductionProfileWaterInjectionCostProfile}>Delete</ImportButton>

            </Wrapper>
            <WrapperColumn>
                <DataTable columns={productionProfileWaterInjectionColumns} gridData={productionProfileWaterInjectionGridData} onCellsChanged={onProductionProfileWaterInjectionCellsChanged} dG4Year={caseItem?.DG4Date?.getFullYear().toString()!} />
            </WrapperColumn>
            {!productionProfileWaterInjectionCostProfileDialogOpen ? null
                : <Import onClose={() => { setProductionProfileWaterInjectionCostProfileDialogOpen(!productionProfileWaterInjectionCostProfileDialogOpen) }} onImport={onImportProductionProfileWaterInjection} />}

            <Wrapper><SaveButton disabled={!hasChanges} onClick={handleSave}>Save</SaveButton></Wrapper>
        </AssetViewDiv>
    )
}

export default DrainageStrategyView

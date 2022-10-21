import {
    Button,
    Typography,
} from "@equinor/eds-core-react"
import { useEffect, useState } from "react"
import {
    useParams,
} from "react-router-dom"
import styled from "styled-components"
import { useCurrentContext } from "@equinor/fusion"
import { Exploration } from "../models/assets/exploration/Exploration"
import { Case } from "../models/case/Case"
import { Project } from "../models/Project"
import { GetProjectService } from "../Services/ProjectService"
import { GetExplorationService } from "../Services/ExplorationService"
import Save from "../Components/Save"
import { unwrapCase } from "../Utils/common"
import AssetTypeEnum from "../models/assets/AssetTypeEnum"
import { ExplorationCostProfile } from "../models/assets/exploration/ExplorationCostProfile"
import { GAndGAdminCost } from "../models/assets/exploration/GAndGAdminCost"
import { IAssetService } from "../Services/IAssetService"
import { GetCaseService } from "../Services/CaseService"
import { initializeFirstAndLastYear } from "./Asset/AssetHelper"
import { AssetViewDiv, ImportButton, WrapperTablePeriod } from "./Asset/StyledAssetComponents"
import TimeSeriesWells from "../Components/TimeSeriesWells"
import { ITimeSeries } from "../models/ITimeSeries"
import { ExplorationWell } from "../models/ExplorationWell"
import { WellProjectWell } from "../models/WellProjectWell"
import { WellProject } from "../models/assets/wellproject/WellProject"
import NumberInputTable from "../Components/NumberInputTable"
import { Well } from "../models/Well"
import { DrillingSchedule } from "../models/assets/wellproject/DrillingSchedule"

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

const ColumnWrapper = styled.div`
    display: flex;
    flex-direction: column;
`

interface Params {
    _project: Project,
    _case: Case
    _exploration: Exploration | undefined,
    _wellProject: WellProject | undefined,
    _wells: Well[] | undefined,
}

const DrillingScheduleViewTab = ({
    _project,
    _case,
    _exploration,
    _wellProject,
    _wells,
}: Params) => {
    const [project, setProject] = useState<Project>()
    const [caseItem, setCase] = useState<Case>()
    const [hasChanges, setHasChanges] = useState(false)
    const [name, setName] = useState<string>("")
    const { fusionContextId, caseId, explorationId } = useParams<Record<string, string | undefined>>()
    const currentProject = useCurrentContext()
    const [firstTSYear, setFirstTSYear] = useState<number>()
    const [lastTSYear, setLastTSYear] = useState<number>()
    const [costProfile, setCostProfile] = useState<ExplorationCostProfile>()
    const [rigMobDemob, setRigMobDemob] = useState<number>()
    const [currency, setCurrency] = useState<Components.Schemas.Currency>(1)
    const [gAndGAdminCost, setGAndGAdminCost] = useState<GAndGAdminCost>()

    const [explorationService, setExplorationService] = useState<IAssetService>()

    const [exploration, setExploration] = useState<Exploration>()
    const [wellProject, setWellProject] = useState<WellProject>()

    const [explorationWells, setExplorationWells] = useState<ExplorationWell[]>()
    const [developmentWells, setDevelopmentWells] = useState<WellProjectWell[]>()

    const [tableFirstYear, setTableFirstYear] = useState<number>(Number.MAX_SAFE_INTEGER)
    const [tableLastYear, setTableLastYear] = useState<number>(Number.MIN_SAFE_INTEGER)
    const [columns, setColumns] = useState<string[]>([""])

    const isValidYear = (year: number | undefined) => year?.toString().length === 4

    useEffect(() => {
        (async () => {
            try {
                const projectResult = await (await GetProjectService()).getProjectByID(currentProject?.externalId!)
                setProject(projectResult)
                const service = await GetExplorationService()
                setExplorationService(service)
            } catch (error) {
                console.error(`[CaseView] Error while fetching project ${currentProject?.externalId}`, error)
            }
        })()
    }, [])

    useEffect(() => {
        (async () => {
            if (project !== undefined) {
                const caseResult = unwrapCase(project.cases.find((o) => o.id === caseId))
                setCase(caseResult)
                let newExploration = project.explorations.find((s) => s.id === explorationId)
                if (newExploration !== undefined) {
                    setExploration(newExploration)
                } else {
                    newExploration = new Exploration()
                    newExploration.currency = project.currency
                    setExploration(newExploration)
                }
            }
        })()
    }, [project])

    useEffect(() => {
        (async () => {
            if (project !== undefined) {
                const caseResult = unwrapCase(project.cases.find((o) => o.id === caseId))
                setCase(caseResult)
                let newExploration = project.explorations.find((s) => s.id === caseResult.explorationLink)
                if (newExploration !== undefined) {
                    setExploration(newExploration)
                } else {
                    newExploration = new Exploration()
                    newExploration.currency = project.currency
                    setExploration(newExploration)
                }
            }
        })()
    }, [_project, _case, project])

    useEffect(() => {
        if (_exploration) {
            setExplorationWells(_exploration?.explorationWells!)
        }
        if (_wellProject) {
            setDevelopmentWells(_wellProject?.wellProjectWells!)
        }
        console.log(_exploration, _wellProject)
    }, [_exploration, _wellProject])

    const allDrillingSchedules = () => {
        const dS: DrillingSchedule[] = []
        if (_exploration?.explorationWells![0] !== undefined) {
            for (let i = 0; i < _exploration?.explorationWells.length; i += 1) {
                dS.push(_exploration?.explorationWells![i]?.drillingSchedule!)
            }
        }
        if (_wellProject?.wellProjectWells![0] !== undefined) {
            for (let i = 0; i < _wellProject?.wellProjectWells.length; i += 1) {
                dS.push(_wellProject?.wellProjectWells![i]?.drillingSchedule!)
            }
        }
        return dS
    }

    useEffect(() => {
        const newExploration: Exploration = { ...exploration }
        setExploration(newExploration)
        const newWellProject: WellProject = { ...wellProject }
        setWellProject(newWellProject)

        if (caseItem?.DG4Date) {
            initializeFirstAndLastYear(
                caseItem?.DG4Date?.getFullYear(),
                allDrillingSchedules(),
                setFirstTSYear,
                setLastTSYear,
            )
        }

        console.log(_exploration?.explorationWells![0]?.drillingSchedule)

        if (isValidYear(firstTSYear) && isValidYear(lastTSYear) && firstTSYear && lastTSYear
            && tableFirstYear === Number.MAX_SAFE_INTEGER && tableLastYear === Number.MIN_SAFE_INTEGER
            && (firstTSYear !== lastTSYear)) {
            setTableFirstYear(firstTSYear)
            setTableLastYear(lastTSYear - 1)
        }

        const colYears = []
        if (isValidYear(firstTSYear) && isValidYear(lastTSYear) && firstTSYear && lastTSYear) {
            for (let j = firstTSYear; j < lastTSYear; j += 1) {
                colYears.push(j.toString())
            }
            setColumns(colYears)
        }
    }, [explorationWells, developmentWells, firstTSYear, lastTSYear, caseItem])

    const tempSetTimeSeries = () => {
        const newTimeSeries: ITimeSeries = {}
        return newTimeSeries
    }

    const disableApplyButton = () => {
        if (firstTSYear === tableFirstYear && lastTSYear === (tableLastYear + 1)) {
            return true
        }
        return false
    }

    const addTimeSeries = () => {
        const colYears = []
        if (isValidYear(tableFirstYear) && isValidYear(tableLastYear)) {
            for (let j = tableFirstYear; j < tableLastYear + 1; j += 1) {
                colYears.push(j.toString())
            }
            setColumns(colYears)

            // add columns as props to timeSeriesWells
            // check in timeSeriesWells if column (first and last years)
            // doesn't match firstYear and lastYear. if unmatch => create grids

            // if (explorationWells![0] === undefined) {
            //     for (let i = 0; i < explorationWells?.length!; i += 1) {
            //         // createEmptyGrid(i)
            //     }
            // }
            // if (explorationWells![0] !== undefined) {
            //     for (let i = 0; i < explorationWells?.length!; i += 1) {
            //         // createNewGridWithData(i)
            //     }
            // }

            // if (developmentWells![0] === undefined) {
            //     for (let i = 0; i < developmentWells?.length!; i += 1) {
            //         // createEmptyGrid(i)
            //     }
            // }
            // if (developmentWells![0] !== undefined) {
            //     for (let i = 0; i < developmentWells?.length!; i += 1) {
            //         // createNewGridWithData(i)
            //     }
            // }
        }
    }

    const expWellNames: string[] = []
    const explorationWellName = (i: number) => {
        if (_wells !== undefined) {
            console.log(_wells)
            expWellNames.push(_wells[i]?.name!)
        }
    }

    const devWellNames: string[] = []
    const developmentWellName = (i: number) => {
        if (_wells !== undefined) {
            console.log(_wells)
            devWellNames.push(_wells[i]?.name!)
        }
    }

    console.log(explorationWells)

    const expWellTimeSeries: DrillingSchedule[] = []
    const devWellTimeSeries: DrillingSchedule[] = []

    const explorationAndDevlopmentWellTimeSeries = () => {
        if (explorationWells !== undefined) {
            for (let i = 0; i < Number(explorationWells?.length); i += 1) {
                expWellTimeSeries.push(explorationWells![i]?.drillingSchedule!)
            }
        }
        if (developmentWells !== undefined) {
            for (let i = 0; i < Number(developmentWells?.length); i += 1) {
                devWellTimeSeries.push(developmentWells![i]?.drillingSchedule!)
            }
        }
    }
    explorationAndDevlopmentWellTimeSeries()

    const timeSeriesDataExpWellLoop = () => {
        if (_wells !== undefined && _wells[0] !== undefined) {
            for (let i = 0; i < _wells.length; i += 1) {
                if (_wells[i]?.wellCategory! > 3) {
                    explorationWellName(i)
                }
                if (_wells[i]?.wellCategory! <= 3) {
                    developmentWellName(i)
                }
            }
        }
        return expWellNames
    }
    timeSeriesDataExpWellLoop()

    const expWellObjects: object[] = []

    const timeSeriesDataExpWellObjLoop = () => {
        if (expWellNames !== undefined) {
            for (let i = 0; i < expWellNames.length; i += 1) {
                const timeSeriesDataExpWellObj = {
                    "Exploration wells": expWellNames[i], Unit: "Wells", set: setExplorationWells, "Total wells": 0,
                }
                expWellObjects.push(timeSeriesDataExpWellObj)
            }
        }
    }
    timeSeriesDataExpWellObjLoop()
    console.log(expWellObjects)

    const devWellObjects: object[] = []

    const timeSeriesDataDevWellObjLoop = () => {
        if (devWellNames !== undefined) {
            for (let i = 0; i < devWellNames.length; i += 1) {
                const timeSeriesDataDevWellObj = {
                    "Development wells": devWellNames[i], Unit: "Wells", set: setExplorationWells, "Total wells": 0,
                }
                devWellObjects.push(timeSeriesDataDevWellObj)
            }
        }
    }
    timeSeriesDataDevWellObjLoop()

    // const timeSeriesDataExpWellObj = {
    //     "Exploration well": expWellNames, unit: "Wells", set: setExplorationWells,
    // }

    // console.log(timeSeriesDataExpWell)
    // const timeSeriesDataDevWell = [
    //     {
    //         "Development well": developmentWellName(), unit: "Wells", set: setDevelopmentWells,
    //     },
    // ]
    // console.log(timeSeriesDataDevWell)

    if (!project || !exploration || !caseItem) { return null }

    return (
        <>
            <TopWrapper>
                <PageTitle variant="h3">Drilling schedule</PageTitle>
                {/* <Button onClick={handleSave}>Save</Button> */}
            </TopWrapper>
            <ColumnWrapper>
                <AssetViewDiv>
                    <Typography>
                        To edit the well costs, go to Edit technical input
                    </Typography>
                    <WrapperTablePeriod>
                        <NumberInputTable
                            value={isValidYear(tableFirstYear) ? tableFirstYear : 2020}
                            setValue={setTableFirstYear}
                            integer
                            label="Start year"
                        />
                        <Typography variant="h2">-</Typography>
                        <NumberInputTable
                            value={isValidYear(tableLastYear) ? tableLastYear : 2030}
                            setValue={setTableLastYear}
                            integer
                            label="End year"
                        />
                        <ImportButton
                            onClick={addTimeSeries}
                            disabled={disableApplyButton()}
                        >
                            Apply
                        </ImportButton>
                    </WrapperTablePeriod>
                    <TimeSeriesWells
                        dG4Year={caseItem.DG4Date!.getFullYear()}
                        setTimeSeries={tempSetTimeSeries}
                        setHasChanges={setHasChanges}
                        firstYear={tableFirstYear}
                        lastYear={tableLastYear}
                        wellsTimeSeries={[]}
                        timeSeriesData={[expWellObjects, devWellObjects]}
                        columnsFromTableYears={columns}
                        expDrillingSchedules={expWellTimeSeries}
                        devDrillingSchedules={devWellTimeSeries}
                    />
                </AssetViewDiv>
            </ColumnWrapper>
        </>
    )
}

export default DrillingScheduleViewTab

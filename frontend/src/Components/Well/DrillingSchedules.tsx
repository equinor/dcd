import { Dispatch, SetStateAction } from "react"
import { Case } from "../../models/case/Case"
import { ExplorationWell } from "../../models/ExplorationWell"
import { Project } from "../../models/Project"
import { WellProjectWell } from "../../models/WellProjectWell"
import DrillingScheduleRow from "./DrillingScheduleRow"

interface Props {
    wellProjectWells?: WellProjectWell[] | null | undefined
    explorationWells?: ExplorationWell[] | null | undefined
    setProject: Dispatch<SetStateAction<Project | undefined>>
    project: Project
    caseItem: Case
    firstYear: number | undefined,
    lastYear: number | undefined,
    setFirstYear: Dispatch<SetStateAction<number | undefined>>,
    setLastYear: Dispatch<SetStateAction<number | undefined>>,
}

const DrillingSchedules = ({
    wellProjectWells,
    explorationWells,
    setProject,
    project,
    caseItem,
    firstYear,
    lastYear,
    setFirstYear,
    setLastYear,
}: Props) => {
    const GenerateDrillingSchedules = () => {
        const drillingSchedules: JSX.Element[] = []
        if (wellProjectWells) {
            wellProjectWells?.forEach((aw) => {
                const well = project.wells?.find((w) => w.id === aw.wellId)
                if (well && aw.count && aw.count > 0) {
                    drillingSchedules.push((
                        <DrillingScheduleRow
                            dG4Year={caseItem.DG4Date?.getFullYear()}
                            firstYear={firstYear}
                            lastYear={lastYear}
                            setFirstYear={setFirstYear}
                            setLastYear={setLastYear}
                            setProject={setProject}
                            timeSeriesTitle={well.name ?? ""}
                            wellProjectWell={aw}
                            key={well.id}
                        />
                    ))
                }
            })
        } else if (explorationWells) {
            explorationWells?.forEach((aw) => {
                const well = project.wells?.find((w) => w.id === aw.wellId)
                if (well && aw.count && aw.count > 0) {
                    drillingSchedules.push((
                        <DrillingScheduleRow
                            dG4Year={caseItem.DG4Date?.getFullYear()}
                            firstYear={firstYear}
                            lastYear={lastYear}
                            setFirstYear={setFirstYear}
                            setLastYear={setLastYear}
                            setProject={setProject}
                            timeSeriesTitle={well.name ?? ""}
                            explorationWell={aw}
                            key={well.id}
                        />
                    ))
                }
            })
        }

        return drillingSchedules
    }

    return (<>{GenerateDrillingSchedules()}</>)
}

export default DrillingSchedules

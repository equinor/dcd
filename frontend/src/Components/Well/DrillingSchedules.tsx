import { Dispatch, SetStateAction } from "react"
import { Case } from "../../models/case/Case"
import { ExplorationWell } from "../../models/ExplorationWell"
import { Project } from "../../models/Project"
import { WellProjectWell } from "../../models/WellProjectWell"
import DrillingScheduleRow from "./DrillingScheduleRow"

interface Props {
    assetWells: WellProjectWell[] | ExplorationWell[] | null | undefined
    setProject: Dispatch<SetStateAction<Project | undefined>>
    project: Project
    caseItem: Case
    firstYear: number | undefined,
    lastYear: number | undefined,
    setFirstYear: Dispatch<SetStateAction<number | undefined>>,
    setLastYear: Dispatch<SetStateAction<number | undefined>>,
}

const DrillingSchedules = ({
    assetWells,
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
        assetWells?.forEach((aw) => {
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
                        assetWell={aw}
                        key={well.id}
                    />
                ))
            }
        })
        return drillingSchedules
    }

    return (<>{GenerateDrillingSchedules()}</>)
}

export default DrillingSchedules

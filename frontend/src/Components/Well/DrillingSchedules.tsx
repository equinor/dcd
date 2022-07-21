import { Dispatch, SetStateAction } from "react"
import { Case } from "../../models/Case"
import { Project } from "../../models/Project"
import { WellProjectWell } from "../../models/WellProjectWell"
import DrillingScheduleRow from "./DrillingScheduleRow"

interface Props {
    wellProjectWells: WellProjectWell[] | null | undefined
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
        wellProjectWells?.forEach((wpw) => {
            const well = project.wells?.find((w) => w.id === wpw.wellId)
            if (well && wpw.count && wpw.count > 0) {
                drillingSchedules.push((
                    <DrillingScheduleRow
                        dG4Year={caseItem.DG4Date?.getFullYear()}
                        firstYear={firstYear}
                        lastYear={lastYear}
                        setFirstYear={setFirstYear}
                        setLastYear={setLastYear}
                        setProject={setProject}
                        timeSeriesTitle={well.name ?? ""}
                        wellProjectWell={wpw}
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

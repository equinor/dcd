import { Dispatch, SetStateAction } from "react"
import { Project } from "../../models/Project"
import { Well } from "../../models/Well"
import { WellProjectWell } from "../../models/WellProjectWell"
import DrillingScheduleTable from "./DrillingScheduleTable"

interface Props {
    wellProjectWells: WellProjectWell[] | null | undefined
    setWellCases: Dispatch<SetStateAction<WellProjectWell[] | null | undefined>>,
    setProject: Dispatch<SetStateAction<Project | undefined>>
    project: Project
}

const DrillingSchedules = ({
    wellProjectWells,
    setWellCases,
    setProject,
    project,
}: Props) => {
    const GenerateDrillingSchedules = () => {
        const drillingSchedules: JSX.Element[] = []
        wellProjectWells?.forEach((wpw) => {
            const well = project.wells?.find((w) => w.id === wpw.wellId)
            if (well && wpw.count && wpw.count > 0) {
                drillingSchedules.push((
                    <DrillingScheduleTable
                        key={`${wpw.wellProjectId}${wpw.wellId}`}
                        wellCase1={wpw}
                        setWellCases={setWellCases}
                        wellCases={wellProjectWells}
                        setProject={setProject}
                        project={project}
                        well={well}
                    />
                ))
            }
        })
        return drillingSchedules
    }

    return (<>{GenerateDrillingSchedules()}</>)
}

export default DrillingSchedules

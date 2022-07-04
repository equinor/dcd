import { Dispatch, SetStateAction } from "react"
import { Well } from "../../models/Well"
import { WellProjectWell } from "../../models/WellProjectWell"
import DrillingScheduleTable from "./DrillingScheduleTable"

interface Props {
    wellCases: WellProjectWell[] | null | undefined
    setWellCases: Dispatch<SetStateAction<WellProjectWell[] | null | undefined>>,
}

const DrillingSchedules = ({
    wellCases,
    setWellCases,
}: Props) => {
    const GenerateDrillingSchedules = () => {
        const drillingSchedules: JSX.Element[] = []
        wellCases?.forEach((wc) => {
            if (wc.count && wc.count > 0) {
                drillingSchedules.push((
                    <DrillingScheduleTable
                        key={`${wc.wellProjectId}${wc.wellId}`}
                        wellCase1={wc}
                        setWellCases={setWellCases}
                        wellCases={wellCases}
                        title="Test"
                    />
                ))
            }
        })
        return drillingSchedules
    }

    return (<>{GenerateDrillingSchedules()}</>)
}

export default DrillingSchedules

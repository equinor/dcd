import { Dispatch, SetStateAction } from "react"
import { Well } from "../../models/Well"
import { WellCase } from "../../models/WellCase"
import DrillingScheduleTable from "./DrillingScheduleTable"

interface Props {
    wellCases: WellCase[] | null | undefined
    setWellCases: Dispatch<SetStateAction<WellCase[] | null | undefined>>,
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
                        key={`${wc.caseId}${wc.wellId}`}
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

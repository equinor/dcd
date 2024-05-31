import Grid from "@mui/material/Grid"
import CapexFactorFeasibilityStudies from "./Inputs/CapexFactorFeasibilityStudies"
import CapexFactorFeedStudies from "./Inputs/CapexFactorFeedStudies"
import Maturity from "./Inputs/Maturity"
import DateRangePicker from "../../../Input/TableDateRangePicker"
import { useProjectContext } from "../../../../Context/ProjectContext"
import useQuery from "../../../../Hooks/useQuery"
import { GetCaseService } from "../../../../Services/CaseService"
import { useCaseContext } from "../../../../Context/CaseContext"

interface HeaderProps {
    startYear: number;
    setStartYear: (startYear: number) => void;
    endYear: number;
    setEndYear: (endYear: number) => void;
    setTableYears: (years: [number, number]) => void;
}

const Header: React.FC<HeaderProps> = ({
    startYear,
    setStartYear,
    endYear,
    setEndYear,
    setTableYears,
}) => {
    const { project } = useProjectContext()
    const { projectCase } = useCaseContext()

    if (!projectCase) { return null }

    const { updateData: updateCase } = useQuery({
        queryKey: ["caseData", project!.id, projectCase.id],
        mutationFn: async (updatedData: Components.Schemas.CaseDto) => {
            const caseService = await GetCaseService()
            return caseService.updateCase(project!.id, projectCase.id, updatedData)
        },
    })

    const handleTableYearsClick = () => {
        setTableYears([startYear, endYear])
    }

    const datePickerValue = (() => {
        if (project?.currency === 1) {
            return "MNOK"
        } if (project?.currency === 2) {
            return "MUSD"
        }
        return ""
    })()
    return (
        <>
            <Grid item xs={12} md={4}>
                <CapexFactorFeasibilityStudies updateCase={updateCase} />
            </Grid>
            <Grid item xs={12} md={4}>
                <CapexFactorFeedStudies updateCase={updateCase} />
            </Grid>
            <Grid item xs={12} md={4}>
                <Maturity />
            </Grid>
            <DateRangePicker
                startYear={startYear}
                endYear={endYear}
                setStartYear={setStartYear}
                setEndYear={setEndYear}
                handleTableYearsClick={handleTableYearsClick}
                labelText="Currency"
                labelValue={datePickerValue}
            />
        </>
    )
}

export default Header

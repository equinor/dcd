import Grid from "@mui/material/Grid"
import CapexFactorFeasibilityStudies from "./Inputs/CapexFactorFeasibilityStudies"
import CapexFactorFeedStudies from "./Inputs/CapexFactorFeedStudies"
import Maturity from "./Inputs/Maturity"
import DateRangePicker from "../../../Input/TableDateRangePicker"
import { useProjectContext } from "../../../../Context/ProjectContext"

interface HeaderProps {
    startYear: number;
    setStartYear: (startYear: number) => void;
    endYear: number;
    setEndYear: (endYear: number) => void;
    setTableYears: (years: [number, number]) => void;
    caseData: Components.Schemas.CaseDto;
    surfData: Components.Schemas.SurfWithProfilesDto
    addEdit: any
}

const Header: React.FC<HeaderProps> = ({
    startYear,
    setStartYear,
    endYear,
    setEndYear,
    setTableYears,
    caseData,
    surfData,
    addEdit,
}) => {
    const { project } = useProjectContext()
    const projectId = project?.id || null

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

    if (!projectId) {
        return null
    }

    return (
        <>
            <Grid item xs={12} md={4}>
                <CapexFactorFeasibilityStudies
                    caseData={caseData}
                    addEdit={addEdit}
                />
            </Grid>
            <Grid item xs={12} md={4}>
                <CapexFactorFeedStudies
                    caseData={caseData}
                    addEdit={addEdit}
                />
            </Grid>
            <Grid item xs={12} md={4}>
                <Maturity
                    surfData={surfData}
                    projectId={projectId}
                    addEdit={addEdit}
                />
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

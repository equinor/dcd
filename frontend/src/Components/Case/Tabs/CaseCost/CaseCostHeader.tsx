import Grid from "@mui/material/Grid"

import DateRangePicker from "@/Components/Input/TableDateRangePicker"
import { useDataFetch } from "@/Hooks/useDataFetch"
import CapexFactorFeasibilityStudies from "./Inputs/CapexFactorFeasibilityStudies"
import CapexFactorFeedStudies from "./Inputs/CapexFactorFeedStudies"
import Maturity from "./Inputs/Maturity"

interface HeaderProps {
    startYear: number;
    setStartYear: (startYear: number) => void;
    endYear: number;
    setEndYear: (endYear: number) => void;
    setTableYears: (years: [number, number]) => void;
    caseData: Components.Schemas.CaseOverviewDto;
    surfData: Components.Schemas.SurfOverviewDto
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
    const revisionAndProjectData = useDataFetch()

    const handleTableYearsClick = () => {
        setTableYears([startYear, endYear])
    }

    const datePickerValue = (() => {
        if (revisionAndProjectData?.commonProjectAndRevisionData.currency === 1) {
            return "MNOK"
        } if (revisionAndProjectData?.commonProjectAndRevisionData.currency === 2) {
            return "MUSD"
        }
        return ""
    })()

    if (!revisionAndProjectData) {
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
                    projectId={revisionAndProjectData.projectId}
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

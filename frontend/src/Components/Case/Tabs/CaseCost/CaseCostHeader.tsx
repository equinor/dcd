import Grid from "@mui/material/Grid2"

import DateRangePicker from "@/Components/Input/TableDateRangePicker"
import { useDataFetch } from "@/Hooks"
import CapexFactorFeasibilityStudies from "./Inputs/CapexFactorFeasibilityStudies"
import CapexFactorFeedStudies from "./Inputs/CapexFactorFeedStudies"
import Maturity from "./Inputs/Maturity"
import { Currency } from "@/Models/enums"

interface HeaderProps {
    startYear: number;
    setStartYear: (startYear: number) => void;
    endYear: number;
    setEndYear: (endYear: number) => void;
    setTableYears: (years: [number, number]) => void;
    caseData: Components.Schemas.CaseOverviewDto;
    surfData: Components.Schemas.SurfDto
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
        if (revisionAndProjectData?.commonProjectAndRevisionData.currency === Currency.NOK) {
            return "MNOK"
        } if (revisionAndProjectData?.commonProjectAndRevisionData.currency === Currency.USD) {
            return "MUSD"
        }
        return ""
    })()

    if (!revisionAndProjectData) {
        return null
    }

    return (
        <>
            <Grid container size={12} justifyContent="flex-start">
                <Grid container size={{ xs: 12, md: 10, lg: 8 }} spacing={2}>
                    <Grid size={{ xs: 12, sm: 6, md: 4 }}>
                        <CapexFactorFeasibilityStudies
                            caseData={caseData}
                            addEdit={addEdit}
                        />
                    </Grid>
                    <Grid size={{ xs: 12, sm: 6, md: 4 }}>
                        <CapexFactorFeedStudies
                            caseData={caseData}
                            addEdit={addEdit}
                        />
                    </Grid>
                    <Grid size={{ xs: 12, sm: 6, md: 4 }}>
                        <Maturity
                            surfData={surfData}
                            projectId={revisionAndProjectData.projectId}
                            addEdit={addEdit}
                        />
                    </Grid>
                </Grid>
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

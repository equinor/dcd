import Grid from "@mui/material/Grid2"

import CapexFactorFeasibilityStudies from "./Inputs/CapexFactorFeasibilityStudies"
import CapexFactorFeedStudies from "./Inputs/CapexFactorFeedStudies"
import FinalYearsWithoutWellInterventionCost from "./Inputs/FinalYearsWithoutWellInterventionCost"
import InitialYearsWithoutWellInterventionCost from "./Inputs/InitialYearsWithoutWellInterventionCost"
import Maturity from "./Inputs/Maturity"

import DateRangePicker from "@/Components/Input/TableDateRangePicker"
import { useDataFetch } from "@/Hooks"
import { Currency } from "@/Models/enums"

interface HeaderProps {
    startYear: number;
    setStartYear: (startYear: number) => void;
    endYear: number;
    setEndYear: (endYear: number) => void;
    setTableYears: (years: [number, number]) => void;
    caseData: Components.Schemas.CaseOverviewDto;
    surfData: Components.Schemas.SurfDto;
    handleTableYearsClick: () => Promise<void>;
}

const Header: React.FC<HeaderProps> = ({
    startYear,
    setStartYear,
    endYear,
    setEndYear,
    setTableYears,
    caseData,
    surfData,
    handleTableYearsClick,
}) => {
    const revisionAndProjectData = useDataFetch()

    const datePickerValue = (() => {
        if (revisionAndProjectData?.commonProjectAndRevisionData.currency === Currency.Nok) {
            return "MNOK"
        } if (revisionAndProjectData?.commonProjectAndRevisionData.currency === Currency.Usd) {
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

                        />
                    </Grid>
                    <Grid size={{ xs: 12, sm: 6, md: 4 }}>
                        <CapexFactorFeedStudies
                            caseData={caseData}

                        />
                    </Grid>
                    <Grid size={{ xs: 12, sm: 6, md: 4 }}>
                        <Maturity
                            surfData={surfData}
                        />
                    </Grid>
                    <Grid size={{ xs: 12, sm: 6, md: 4 }}>
                        <InitialYearsWithoutWellInterventionCost caseData={caseData} />
                    </Grid>
                    <Grid size={{ xs: 12, sm: 6, md: 4 }}>
                        <FinalYearsWithoutWellInterventionCost caseData={caseData} />
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

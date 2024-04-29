import Grid from "@mui/material/Grid"
import CapexFactorFeasibilityStudies from "./Inputs/CapexFactorFeasibilityStudies"
import CapexFactorFeedStudies from "./Inputs/CapexFactorFeedStudies"
import Maturity from "./Inputs/Maturity"
import YearRange from "./Inputs/YearRange"

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
}) => (
    <>
        <Grid item xs={12} md={4}>
            <CapexFactorFeasibilityStudies />
        </Grid>
        <Grid item xs={12} md={4}>
            <CapexFactorFeedStudies />
        </Grid>
        <Grid item xs={12} md={4}>
            <Maturity />
        </Grid>
        <Grid item xs={12} container spacing={1} justifyContent="flex-end" alignItems="baseline" marginTop={6}>
            <YearRange
                startYear={startYear}
                endYear={endYear}
                setStartYear={setStartYear}
                setEndYear={setEndYear}
                setTableYears={setTableYears}
            />
        </Grid>
    </>
)

export default Header

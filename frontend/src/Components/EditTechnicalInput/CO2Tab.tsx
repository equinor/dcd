import { Typography } from "@equinor/eds-core-react"
import Grid from "@mui/material/Grid"
import CO2ListTechnicalInput from "./CO2ListTechnicalInput"

const CO2Tab = () => (
    <Grid container spacing={2}>
        <Grid item xs={12}>
            <Typography>
                You can override these default assumption to customize the calculation made in CO2 emissions.
            </Typography>
        </Grid>
        <Grid item xs={12}>
            <CO2ListTechnicalInput />
        </Grid>
    </Grid>
)

export default CO2Tab

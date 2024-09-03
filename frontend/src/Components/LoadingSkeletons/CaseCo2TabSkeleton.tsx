import React from "react"
import { Grid, Skeleton } from "@mui/material"

const CaseCo2TabSkeleton: React.FC = () => (
    <Grid container spacing={2} sx={{ padding: "16px" }}>
        <Grid item xs={12}>
            <Skeleton variant="text" animation="wave" width="100%" height={60} />
            <Skeleton variant="text" animation="wave" width="40%" height={60} />
        </Grid>
        <Grid item xs={12} md={6} lg={3}>
            <Skeleton variant="text" animation="wave" width="80%" height={20} />
            <Skeleton animation="wave" height={60} />
        </Grid>
        <Grid item container spacing={2} xs={12}>
            <Grid item xs={7}>
                <Skeleton animation="wave" variant="rounded" height={440} />
            </Grid>
            <Grid item xs={5}>
                <Skeleton animation="wave" variant="circular" height={440} />
            </Grid>
        </Grid>
        <Grid item xs={12} container spacing={1} justifyContent="flex-end" alignItems="baseline" marginTop={6}>
            <Grid item xs={12} md={6} lg={3}>
                <Skeleton variant="text" animation="wave" width="80%" height={20} />
                <Skeleton animation="wave" height={60} />
            </Grid>
            <Grid item xs={12} md={6} lg={3}>
                <Skeleton variant="text" animation="wave" width="80%" height={20} />
                <Skeleton animation="wave" height={60} />
            </Grid>
        </Grid>
        <Grid item xs={12}>
            <Skeleton animation="wave" variant="rounded" height={400} />
        </Grid>
    </Grid>
)

export default CaseCo2TabSkeleton

import React from "react"
import { Grid, Skeleton } from "@mui/material"

const CaseProductionProfilesTabSkeleton: React.FC = () => (
    <Grid container spacing={2}>
        {[...Array(7)].map((_, index) => (
            <Grid item xs={12} md={6} lg={3} key={`menu-item-${index + 1}`}>
                <Skeleton variant="text" animation="wave" width="80%" height={20} />
                <Skeleton animation="wave" height={60} />
            </Grid>
        ))}
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
        <Grid item xs={12} sx={{ marginTop: -8 }}>
            <Skeleton animation="wave" height={400} />
        </Grid>
        <Grid item xs={12} sx={{ marginBottom: -2, marginTop: -15 }}>
            <Skeleton animation="wave" height={400} />
        </Grid>
    </Grid>
)

export default CaseProductionProfilesTabSkeleton

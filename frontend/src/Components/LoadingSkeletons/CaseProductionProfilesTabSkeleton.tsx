import { Grid, Skeleton } from "@mui/material"
import React from "react"

const CaseProductionProfilesTabSkeleton: React.FC = () => (
    <Grid container spacing={2} sx={{ padding: "16px" }}>
        <Grid item xs={12} container spacing={1} justifyContent="space-between" alignItems="baseline">
            <Grid item xs={12} md={6} lg={4}>
                <Skeleton animation="wave" height={60} />
            </Grid>
            <Grid item xs={12} md={6} lg={4}>
                <Skeleton animation="wave" height={60} />
            </Grid>
        </Grid>
        <Grid item xs={12} sx={{ marginBottom: 5, marginTop: -3 }}>
            <Skeleton animation="wave" height={60} />
        </Grid>
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
        <Grid item xs={12}>
            <Skeleton animation="wave" variant="rounded" height={400} />
        </Grid>
        <Grid item xs={12}>
            <Skeleton animation="wave" variant="rounded" height={400} />
        </Grid>
    </Grid>
)

export default CaseProductionProfilesTabSkeleton

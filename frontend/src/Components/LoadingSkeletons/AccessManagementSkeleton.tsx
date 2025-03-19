import { Grid, Skeleton } from "@mui/material"
import React from "react"

const AccessManagementSkeleton: React.FC = () => (
    <Grid container spacing={2} sx={{ padding: "16px" }}>
        <Grid item xs={9} md={10} container spacing={2}>
            <Grid item xs={12} container spacing={1} justifyContent="space-between" alignItems="baseline">
                <Grid item xs={10} md={10} lg={10}>
                    <Skeleton animation="wave" height={90} />
                </Grid>
                <Grid item xs={12}>
                    <Skeleton animation="wave" height={60} />
                </Grid>
                <Grid item xs={12} md={6} lg={4}>
                    <Skeleton animation="wave" height={60} />
                </Grid>
            </Grid>

            <Grid item xs={6}>
                <Skeleton variant="rounded" animation="wave" height={500} />
            </Grid>
            <Grid item xs={6}>
                <Skeleton variant="rounded" animation="wave" height={500} />
            </Grid>
            <Grid item xs={12} container spacing={1} justifyContent="space-between" alignItems="baseline">
                <Grid item xs={12}>
                    <Skeleton animation="wave" height={60} />
                </Grid>
                <Grid item xs={12} md={6} lg={4}>
                    <Skeleton animation="wave" height={60} />
                </Grid>
            </Grid>
        </Grid>
    </Grid>
)

export default AccessManagementSkeleton

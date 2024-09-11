import React from "react"
import { Grid, Skeleton } from "@mui/material"

const ProjectSkeleton: React.FC = () => (
    <Grid container spacing={2} sx={{ padding: "16px" }}>
        <Grid item xs={3} md={2}>
            <Skeleton variant="rounded" animation="wave" height={1230} sx={{ width: "100%" }} />
        </Grid>
        <Grid item xs={9} md={10} container spacing={2}>
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
            {[...Array(4)].map((_, index) => (
                <Grid item xs={12} md={6} lg={4} key={`menu-item-${index + 1}`}>
                    <Skeleton variant="text" animation="wave" width="80%" height={20} />
                    <Skeleton animation="wave" height={60} />
                </Grid>
            ))}
            <Grid item xs={12}>
                <Skeleton variant="rounded" animation="wave" height={870} />
            </Grid>
        </Grid>
    </Grid>
)

export default ProjectSkeleton

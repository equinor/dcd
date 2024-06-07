import React from "react"
import { Grid, Skeleton } from "@mui/material"

const CaseDescriptionTabSkeleton: React.FC = () => (
    <Grid container spacing={2}>
        <Grid item xs={12}>
            <Grid item xs={12}>
                <Skeleton variant="text" animation="wave" width="30%" height={20} />
                <Skeleton animation="wave" height={60} />
            </Grid>
        </Grid>
        {[...Array(6)].map((_, index) => (
            <Grid item xs={12} md={6} lg={3} key={`menu-item-${index + 1}`}>
                <Skeleton variant="text" animation="wave" width="80%" height={20} />
                <Skeleton animation="wave" height={60} />
            </Grid>
        ))}
    </Grid>
)

export default CaseDescriptionTabSkeleton

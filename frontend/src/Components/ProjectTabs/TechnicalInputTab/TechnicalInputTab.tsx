import Grid from "@mui/material/Grid2"
import { useState } from "react"

import { SecondaryTabs } from "../Components/SecondaryTabs"

import CO2Tab from "./CO2Tab/CO2Tab"
import PROSPTab from "./PROSPTab/PROSPTab"
import WellCostsTab from "./WellCostTab/WellCostsTab"

import ProjectSkeleton from "@/Components/LoadingSkeletons/ProjectSkeleton"
import { useDataFetch } from "@/Hooks"

const TechnicalInputTab = () => {
    const revisionAndProjectData = useDataFetch()
    const [activeTab, setActiveTab] = useState(0)

    if (!revisionAndProjectData) {
        return (<ProjectSkeleton />)
    }

    const tabs = [
        { label: "Well Costs", content: <WellCostsTab /> },
        { label: "PROSP", content: <PROSPTab /> },
        { label: "CO2", content: <CO2Tab /> },
    ]

    return (
        <Grid container>
            <Grid size={12}>
                <SecondaryTabs
                    tabs={tabs}
                    value={activeTab}
                    onChange={(_, newValue) => setActiveTab(newValue)}
                />
            </Grid>
        </Grid>
    )
}

export default TechnicalInputTab

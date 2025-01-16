import { useState } from "react"
import { Tabs } from "@equinor/eds-core-react"
import Grid from "@mui/material/Grid"

import { useDataFetch } from "@/Hooks/useDataFetch"
import WellCostsTab from "./WellCostTab/WellCostsTab"
import PROSPTab from "./PROSPTab/PROSPTab"
import CO2Tab from "./CO2Tab/CO2Tab"

const {
    List, Tab, Panels, Panel,
} = Tabs

const TechnicalInputTab = () => {
    const revisionAndProjectData = useDataFetch()
    const [activeTab, setActiveTab] = useState<number>(0)

    if (!revisionAndProjectData) {
        return (<div>Loading...</div>)
    }

    return (
        <div>
            <Grid container>
                <Grid item xs={12}>
                    <Tabs activeTab={activeTab} onChange={setActiveTab}>
                        <List>
                            <Tab>Well Costs</Tab>
                            <Tab>PROSP</Tab>
                            <Tab>CO2</Tab>
                        </List>
                        <Panels>
                            <Panel>
                                <WellCostsTab />
                            </Panel>
                            <Panel>
                                <PROSPTab />
                            </Panel>
                            <Panel>
                                <CO2Tab />
                            </Panel>
                        </Panels>
                    </Tabs>
                </Grid>
            </Grid>
        </div>
    )
}

export default TechnicalInputTab

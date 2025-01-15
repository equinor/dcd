import {
    useEffect,
    useState,
} from "react"
import { Typography, Button, Icon } from "@equinor/eds-core-react"
import styled from "styled-components"
import { add } from "@equinor/eds-icons"

import { useAppContext } from "@/Context/AppContext"
import { useDataFetch } from "@/Hooks/useDataFetch"
import { EMPTY_GUID } from "@/Utils/constants"
import ExplorationCosts from "./Components/Exploration/ExplorationCosts"
import DevelopmentCosts from "./Components/Development/DevelopmentCosts"
import ExplorationWells from "./Components/Exploration/ExplorationWells"
import DevelopmentWells from "./Components/Development/DevelopmentWells"

const Section = styled.section`
    margin-top: 56px;
    display: flex;
    flex-direction: column;
    gap: 30px;
`

const SectionHeader = styled.div`
    display: flex;
    gap: 20px;
    align-items: center;
    justify-content: space-between;
`

const WellCostsTab = () => {
    const revisionAndProjectData = useDataFetch()
    const { editMode } = useAppContext()
    const [deletedWells, setDeletedWells] = useState<string[]>([])

    const CreateWell = async (wells: any[], setWells: React.Dispatch<React.SetStateAction<any[]>>, category: number) => {
        const newWell: any = {
            wellCategory: category,
            name: "New well",
            projectId: revisionAndProjectData?.projectId,
        }
        if (wells) {
            const newWells = [...wells, newWell]
            setWells(newWells)
        } else {
            setWells([newWell])
        }
    }

    // useEffect(() => {
    //     const updateWellCosts = async () => {
    //         if (revisionAndProjectData
    //             && editMode
    //             && developmentOperationalWellCosts
    //             && explorationOperationalWellCosts
    //             && developmentWells
    //             && explorationWells
    //         ) {
    //             const wellDtos = [...explorationWells, ...developmentWells]
    //             const updateTechnicalInputDto = {
    //                 projectDto: { ...revisionAndProjectData.commonProjectAndRevisionData },
    //                 explorationOperationalWellCostsDto: explorationOperationalWellCosts || {},
    //                 developmentOperationalWellCostsDto: developmentOperationalWellCosts || {},
    //                 createWellDtos: wellDtos.filter((w) => w.id === EMPTY_GUID || w.id === undefined || w.id === null || w.id === ""),
    //                 updateWellDtos: wellDtos.filter((w) => w.id !== EMPTY_GUID && w.id !== undefined && w.id !== null && w.id !== ""),
    //                 deleteWellDtos: deletedWells.map((id) => ({ id })),
    //             }

    //             addTechnicalInputEdit(revisionAndProjectData.projectId, updateTechnicalInputDto)
    //         }
    //     }
    //     updateWellCosts()
    // }, [
    //     developmentOperationalWellCosts,
    //     explorationOperationalWellCosts,
    //     developmentWells,
    //     explorationWells,
    // ])

    return (
        <div>
            <Typography variant="body_long">
                This input is used to calculate each case&apos;s well costs based on their drilling schedules.
            </Typography>
            <Section>
                <SectionHeader>
                    <Typography variant="h2">Exploration Well Costs</Typography>
                    {/* {editMode && (
                        <Button
                            onClick={
                                () => CreateWell(explorationWells, setExplorationWells, 4)
                            }
                            variant="outlined"
                        >
                            <Icon data={add} />
                            Add new exploration well type
                        </Button>
                    )} */}
                </SectionHeader>
                {/* <ExplorationWells
                    setExplorationWells={setExplorationWells}
                    setDeletedWells={setDeletedWells}
                /> */}
                <ExplorationCosts />
            </Section>
            <Section>
                <SectionHeader>
                    <Typography variant="h2">Development Well Costs</Typography>
                    {/* {editMode && (
                        <Button
                            onClick={() => CreateWell(developmentWells, setDevelopmentWells, 0)}
                            variant="outlined"
                        >
                            <Icon data={add} />
                            Add new development/drilling well type
                        </Button>
                    )} */}
                </SectionHeader>
                {/* <DevelopmentWells
                    setDeletedWells={setDeletedWells}
                    setDevelopmentWells={setDevelopmentWells}
                /> */}
                <DevelopmentCosts />
            </Section>
        </div>
    )
}

export default WellCostsTab

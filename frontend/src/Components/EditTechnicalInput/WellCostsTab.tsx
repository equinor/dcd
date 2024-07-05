import { Dispatch, SetStateAction } from "react"
import { Typography, Button, Icon } from "@equinor/eds-core-react"
import styled from "styled-components"
import { add } from "@equinor/eds-icons"
import OperationalWellCosts from "./OperationalWellCosts"
import WellListEditTechnicalInput from "./WellListEditTechnicalInput"
import { useProjectContext } from "../../Context/ProjectContext"
import { useModalContext } from "../../Context/ModalContext"
import { useAppContext } from "../../Context/AppContext"

const Section = styled.section`
    margin-top: 56px;
    display: flex;
    flex-direction: column;
    gap: 26px;
`

const SectionHeader = styled.div`
    display: flex;
    gap: 16px;
    align-items: center;
    justify-content: space-between;
`
interface Props {
    developmentOperationalWellCosts: Components.Schemas.DevelopmentOperationalWellCostsDto | undefined
    setDevelopmentOperationalWellCosts: Dispatch<SetStateAction<Components.Schemas.DevelopmentOperationalWellCostsDto | undefined>>

    explorationOperationalWellCosts: Components.Schemas.ExplorationOperationalWellCostsDto | undefined
    setExplorationOperationalWellCosts: Dispatch<SetStateAction<Components.Schemas.ExplorationOperationalWellCostsDto | undefined>>

    wellProjectWells: Components.Schemas.WellDto[]
    setWellProjectWells: Dispatch<SetStateAction<Components.Schemas.WellDto[]>>

    explorationWells: Components.Schemas.WellDto[]
    setExplorationWells: Dispatch<SetStateAction<Components.Schemas.WellDto[]>>

    setDeletedWells: Dispatch<SetStateAction<string[]>>
}

const WellCostsTab = ({
    developmentOperationalWellCosts,
    setDevelopmentOperationalWellCosts,
    explorationOperationalWellCosts,
    setExplorationOperationalWellCosts,
    wellProjectWells,
    setWellProjectWells,
    explorationWells,
    setExplorationWells,
    setDeletedWells,
}: Props) => {
    const { project } = useProjectContext()
    const { editTechnicalInput } = useModalContext()
    const { editMode } = useAppContext()

    const CreateWell = async (wells: any[], setWells: React.Dispatch<React.SetStateAction<any[]>>, category: number) => {
        const newWell: any = {
            wellCategory: category,
            name: "New well",
            projectId: project?.id,
        }
        if (wells) {
            const newWells = [...wells, newWell]
            setWells(newWells)
        } else {
            setWells([newWell])
        }
    }

    return (
        <div>
            <Typography variant="body_long">
                This input is used to calculate each case&apos;s well costs based on their drilling schedules.
            </Typography>
            <Section>
                <SectionHeader>
                    <Typography variant="h2">Exploration Well Costs</Typography>
                    {(editMode || editTechnicalInput) && (
                        <Button
                            onClick={
                                () => CreateWell(explorationWells, setExplorationWells, 4)
                            }
                            variant="outlined"
                        >
                            <Icon data={add} />
                            Add new exploration well type
                        </Button>
                    )}
                </SectionHeader>
                <WellListEditTechnicalInput
                    explorationWells
                    setWells={setExplorationWells}
                    wells={explorationWells}
                    setDeletedWells={setDeletedWells}
                />
                <OperationalWellCosts
                    title="Exploration costs"
                    explorationOperationalWellCosts={explorationOperationalWellCosts}
                    setExplorationOperationalWellCosts={setExplorationOperationalWellCosts}
                />
            </Section>
            <Section>
                <SectionHeader>
                    <Typography variant="h2">Development Well Costs</Typography>
                    {(editMode || editTechnicalInput) && (
                        <Button
                            onClick={() => CreateWell(wellProjectWells, setWellProjectWells, 0)}
                            variant="outlined"
                        >
                            <Icon data={add} />
                            Add new development/drilling well type
                        </Button>
                    )}
                </SectionHeader>

                <WellListEditTechnicalInput
                    explorationWells={false}
                    setWells={setWellProjectWells}
                    wells={wellProjectWells}
                    setDeletedWells={setDeletedWells}
                />
                <OperationalWellCosts
                    title="Development costs"
                    developmentOperationalWellCosts={developmentOperationalWellCosts}
                    setDevelopmentOperationalWellCosts={setDevelopmentOperationalWellCosts}
                />
            </Section>
        </div>
    )
}

export default WellCostsTab

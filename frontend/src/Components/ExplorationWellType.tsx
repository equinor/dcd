/* eslint-disable max-len */
import { Typography } from "@equinor/eds-core-react"
import { useEffect, useState } from "react"
import styled from "styled-components"
import { Exploration } from "../models/assets/exploration/Exploration"
import { Case } from "../models/Case"
import LinkWellType from "./LinkWellType"

const WrapperColumn = styled.div`
    display: flex;
    flex-direction: column;
    margin-bottom: 2rem;
`

interface Props {
    caseItem: Case | undefined,
    exploration: Exploration | undefined,
}

const ExplorationWellType = ({
    caseItem,
    exploration,
}: Props) => {
    const [selectedWellType, setSelectedWellType] = useState<Components.Schemas.WellType>()

    const onSelectWellType = async (event: React.ChangeEvent<HTMLSelectElement>) => {
        try {
            const selectWell = exploration?.explorationWellTypes?.filter((w) => w.id === event.target.value).at(0)
            setSelectedWellType(selectWell)
        } catch (error) {
            console.error("[ExplorationView] error while fetching well type", error)
        }
    }

    useEffect(() => {
        (async () => {
            try {
                if (selectedWellType === (null || undefined)) {
                    setSelectedWellType(exploration?.explorationWellTypes?.find((w) => w.id))
                }
            } catch (error) {
                console.error("[ExplorationView] Error while fetching well type", error)
            }
        })()
    })

    enum wellTypeCategory {
        "Oil producer" = 0,
        "Gas producer" = 1,
        "Water injector" = 2,
        "Gas injector" = 3,
        "Exploration well" = 4,
        "Appraisal well" = 5,
        "Sidetrack" = 6
    }

    return (
        <WrapperColumn>
            <LinkWellType
                caseItem={caseItem}
                linkWellType={onSelectWellType}
                link="wellsLink"
                currentValue={selectedWellType?.id}
                values={exploration?.explorationWellTypes?.map((a) => <option key={a.id} value={a.id}>{a.name}</option>)}
            />
            <Typography>
                Description:
                {selectedWellType?.description}
            </Typography>
            <Typography>
                Category:
                {wellTypeCategory[selectedWellType?.category!]}
            </Typography>
            <Typography>
                Drilling days:
                {selectedWellType?.drillingDays}
            </Typography>
            <Typography>
                Well cost:
                {selectedWellType?.wellCost}
            </Typography>
        </WrapperColumn>
    )
}

export default ExplorationWellType

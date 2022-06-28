/* eslint-disable max-len */
import { Typography } from "@equinor/eds-core-react"
import { useEffect, useState } from "react"
import styled from "styled-components"
import { WellProject } from "../models/assets/wellproject/WellProject"
import { Case } from "../models/Case"
import LinkWellType from "./LinkWellType"

const WrapperColumn = styled.div`
    display: flex;
    flex-direction: column;
    margin-bottom: 2rem;
`

interface Props {
    caseItem: Case | undefined,
    wellProject: WellProject | undefined,
}

const WellType = ({
    caseItem,
    wellProject,
}: Props) => {
    const [selectedWellType, setSelectedWellType] = useState<Components.Schemas.WellType>()

    const onSelectWellType = async (event: React.ChangeEvent<HTMLSelectElement>) => {
        try {
            const selectWell = wellProject?.wellTypes?.filter((w) => w.id === event.target.value).at(0)
            setSelectedWellType(selectWell)
        } catch (error) {
            console.error("[CaseView] error while submitting form data", error)
        }
    }

    useEffect(() => {
        (async () => {
            try {
                if (selectedWellType === (null || undefined)) {
                    setSelectedWellType(wellProject?.wellTypes?.find((w) => w.id))
                }
            } catch (error) {
                console.error("[WellProjectView] Error while fetching well type", error)
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
                values={wellProject?.wellTypes?.map((a) => <option key={a.id} value={a.id}>{a.name}</option>)}
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

export default WellType

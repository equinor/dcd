import {
    Dispatch,
    SetStateAction,
    ChangeEventHandler,
} from "react"
import styled from "styled-components"

import {
    Button, Label, NativeSelect, Typography,
} from "@equinor/eds-core-react"
import TextArea from "@equinor/fusion-react-textarea/dist/TextArea"
import { Project } from "../../models/Project"
import { Case } from "../../models/case/Case"
import { GetCaseService } from "../../Services/CaseService"
import CaseNumberInput from "../../Components/Case/CaseNumberInput"
import { Topside } from "../../models/assets/topside/Topside"
import { Substructure } from "../../models/assets/substructure/Substructure"
import { Surf } from "../../models/assets/surf/Surf"
import { Transport } from "../../models/assets/transport/Transport"

const ColumnWrapper = styled.div`
    display: flex;
    flex-direction: column;
`
const RowWrapper = styled.div`
    display: flex;
    flex-direction: row;
    margin-bottom: 50px;
    margin-top: 20px;
`
const TopWrapper = styled.div`
    display: flex;
    flex-direction: row;
    margin-top: 20px;
    margin-bottom: 20px;
`
const PageTitle = styled(Typography)`
    flex-grow: 1;
`
const NumberInputField = styled.div`
    padding-right: 20px;
`

interface Props {
    project: Project,
    setProject: Dispatch<SetStateAction<Project | undefined>>,
    caseItem: Case,
    setCase: Dispatch<SetStateAction<Case | undefined>>,
    topside: Topside | undefined,
    setTopside: Dispatch<SetStateAction<Topside | undefined>>,
    surf: Surf | undefined,
    setSurf: Dispatch<SetStateAction<Surf | undefined>>,
    substructure: Substructure | undefined,
    setSubstrucutre: Dispatch<SetStateAction<Substructure | undefined>>,
    transport: Transport | undefined,
    setTransport: Dispatch<SetStateAction<Transport | undefined>>,
}

function CaseFacilitiesTab({
    project, setProject,
    caseItem, setCase,
    topside, setTopside,
    surf, setSurf,
    substructure, setSubstrucutre,
    transport, setTransport,
}: Props) {
    /*
    Topside
    Dry weight
    facilities availability
    oil capacity
    gas capacity
    water injection cap
    produced water treatment cap

    SURF
    drilling centres
    templates
    risers
    production lines length
    umbilical length
    production flowline

    Substructure
    dry weight
    substructure concept

    Transport
    oil export pipeline length
    gas export pipeline length
    */

    const handleTopsideDryWeightChange: ChangeEventHandler<HTMLInputElement> = async (e) => {
        const newTopside = { ...topside }
        newTopside.dryWeight = Number(e.currentTarget.value)
        setTopside(newTopside)
    }

    const handleTopsideFacilitiesAvailabilityChange: ChangeEventHandler<HTMLInputElement> = async (e) => {
        const newTopside = { ...topside }
        newTopside.facilitiesAvailability = Number(e.currentTarget.value)
        setTopside(newTopside)
    }

    const handleTopsideOilCapacityChange: ChangeEventHandler<HTMLInputElement> = async (e) => {
        const newTopside = { ...topside }
        newTopside.oilCapacity = Number(e.currentTarget.value)
        setTopside(newTopside)
    }

    const handleTopsideGasCapacityChange: ChangeEventHandler<HTMLInputElement> = async (e) => {
        const newTopside = { ...topside }
        newTopside.gasCapacity = Number(e.currentTarget.value)
        setTopside(newTopside)
    }

    const handleSave = async () => {
        const result = await (await GetCaseService()).update(caseItem)
        setCase(result)
    }

    return (
        <>
            <TopWrapper>
                <PageTitle variant="h2">Facilities</PageTitle>
                <Button onClick={handleSave}>Save</Button>
            </TopWrapper>
            <ColumnWrapper>
                <Typography variant="h4">Topside</Typography>
                <RowWrapper>
                    <NumberInputField>
                        <CaseNumberInput
                            onChange={handleTopsideDryWeightChange}
                            value={topside?.dryWeight}
                            integer={false}
                            label="Topside dry weight"
                        />
                    </NumberInputField>
                    <CaseNumberInput
                        onChange={handleTopsideFacilitiesAvailabilityChange}
                        value={topside?.facilitiesAvailability}
                        integer
                        disabled
                        label="Facilities availability"
                    />
                </RowWrapper>
                <RowWrapper>
                    <NumberInputField>
                        <CaseNumberInput
                            onChange={handleTopsideOilCapacityChange}
                            value={topside?.oilCapacity}
                            integer={false}
                            label="Oil capacity"
                        />
                    </NumberInputField>
                    <NumberInputField>
                        <CaseNumberInput
                            onChange={handleTopsideGasCapacityChange}
                            value={topside?.gasCapacity}
                            integer={false}
                            label="Gas capacity"
                        />
                    </NumberInputField>
                    <NumberInputField>
                        <CaseNumberInput
                            onChange={() => console.log("Water injection capacity change")}
                            value={topside?.dryWeight}
                            integer={false}
                            label="Water injection capacity"
                        />
                    </NumberInputField>
                    <NumberInputField>
                        <CaseNumberInput
                            onChange={() => console.log("Produced water treatment capacity change")}
                            value={topside?.dryWeight}
                            integer={false}
                            label="Produced water treatment capacity"
                        />
                    </NumberInputField>
                </RowWrapper>
            </ColumnWrapper>
        </>
    )
}

export default CaseFacilitiesTab

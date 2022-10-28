import {
    Dispatch,
    SetStateAction,
    ChangeEventHandler,
    useState,
} from "react"
import styled from "styled-components"

import {
    Button, NativeSelect, Typography, Input, Label, Progress,
} from "@equinor/eds-core-react"
import { Project } from "../../models/Project"
import { Case } from "../../models/case/Case"
import CaseNumberInput from "../../Components/Case/CaseNumberInput"
import { Topside } from "../../models/assets/topside/Topside"
import { Substructure } from "../../models/assets/substructure/Substructure"
import { Surf } from "../../models/assets/surf/Surf"
import { Transport } from "../../models/assets/transport/Transport"
import { GetTopsideService } from "../../Services/TopsideService"
import { GetSurfService } from "../../Services/SurfService"
import { GetSubstructureService } from "../../Services/SubstructureService"
import { GetTransportService } from "../../Services/TransportService"
import { GetCaseService } from "../../Services/CaseService"

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
const NativeSelectField = styled(NativeSelect)`
    width: 320px;
`
const HostWrapper = styled.div`
    margin-left: 20px;
    display: flex;
    flex-direction: column;
`

interface Props {
    project: Project,
    setProject: Dispatch<SetStateAction<Project | undefined>>,
    caseItem: Case,
    setCase: Dispatch<SetStateAction<Case | undefined>>,
    topside: Topside,
    setTopside: Dispatch<SetStateAction<Topside | undefined>>,
    surf: Surf,
    setSurf: Dispatch<SetStateAction<Surf | undefined>>,
    substructure: Substructure,
    setSubstrucutre: Dispatch<SetStateAction<Substructure | undefined>>,
    transport: Transport,
    setTransport: Dispatch<SetStateAction<Transport | undefined>>,
    activeTab: number
}

function CaseFacilitiesTab({
    project, setProject,
    caseItem, setCase,
    topside, setTopside,
    surf, setSurf,
    substructure, setSubstrucutre,
    transport, setTransport,
    activeTab,
}: Props) {
    const [isSaving, setIsSaving] = useState<boolean>()

    const handleTopsideDryWeightChange: ChangeEventHandler<HTMLInputElement> = async (e) => {
        const newTopside: Topside = { ...topside }
        newTopside.dryWeight = Number(e.currentTarget.value)
        setTopside(newTopside)
    }

    const handleTopsideOilCapacityChange: ChangeEventHandler<HTMLInputElement> = async (e) => {
        const newTopside: Topside = { ...topside }
        newTopside.oilCapacity = Number(e.currentTarget.value)
        setTopside(newTopside)
    }

    const handleTopsideGasCapacityChange: ChangeEventHandler<HTMLInputElement> = async (e) => {
        const newTopside: Topside = { ...topside }
        newTopside.gasCapacity = Number(e.currentTarget.value)
        setTopside(newTopside)
    }

    const handleSurfTemplateCountChange: ChangeEventHandler<HTMLInputElement> = async (e) => {
        const newSurf: Surf = { ...surf }
        newSurf.templateCount = Number(e.currentTarget.value)
        setSurf(newSurf)
    }

    const handleSurfRiserCountChange: ChangeEventHandler<HTMLInputElement> = async (e) => {
        const newSurf: Surf = { ...surf }
        newSurf.riserCount = Number(e.currentTarget.value)
        setSurf(newSurf)
    }

    const handleSurfInfieldPipelineSystemLengthChange: ChangeEventHandler<HTMLInputElement> = async (e) => {
        const newSurf: Surf = { ...surf }
        newSurf.infieldPipelineSystemLength = Number(e.currentTarget.value)
        setSurf(newSurf)
    }

    const handleSurfUmbilicalSystemLengthChange: ChangeEventHandler<HTMLInputElement> = async (e) => {
        const newSurf: Surf = { ...surf }
        newSurf.umbilicalSystemLength = Number(e.currentTarget.value)
        setSurf(newSurf)
    }

    const handleProductionFlowlineChange: ChangeEventHandler<HTMLSelectElement> = async (e) => {
        if ([0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13].indexOf(Number(e.currentTarget.value)) !== -1) {
            // eslint-disable-next-line max-len
            const newProductionFlowline: Components.Schemas.ProductionFlowline = Number(e.currentTarget.value) as Components.Schemas.ProductionFlowline
            const newSurf: Surf = { ...surf }
            newSurf.productionFlowline = newProductionFlowline
            setSurf(newSurf)
        }
    }

    const handleSubstructureDryweightChange: ChangeEventHandler<HTMLInputElement> = async (e) => {
        const newSubstructure: Substructure = { ...substructure }
        newSubstructure.dryWeight = Number(e.currentTarget.value)
        setSubstrucutre(newSubstructure)
    }

    const handleSubstructureConceptChange: ChangeEventHandler<HTMLSelectElement> = async (e) => {
        if ([0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12].indexOf(Number(e.currentTarget.value)) !== -1) {
            // eslint-disable-next-line max-len
            const newConcept: Components.Schemas.Concept = Number(e.currentTarget.value) as Components.Schemas.Concept
            const newSubstructure: Substructure = { ...substructure }
            newSubstructure.concept = newConcept
            if (newConcept !== 1) {
                const newCase: Case = { ...caseItem }
                newCase.host = ""
                setCase(newCase)
            }
            setSubstrucutre(newSubstructure)
        }
    }

    const handleHostChange: ChangeEventHandler<HTMLInputElement> = async (e) => {
        const newCase: Case = { ...caseItem }
        newCase.host = e.currentTarget.value
        setCase(newCase)
    }

    const handleTransportOilExportPipelineLengthChange: ChangeEventHandler<HTMLInputElement> = async (e) => {
        const newTransport: Transport = { ...transport }
        newTransport.oilExportPipelineLength = Number(e.currentTarget.value)
        setTransport(newTransport)
    }

    const handleTransportGasExportPipelineLengthChange: ChangeEventHandler<HTMLInputElement> = async (e) => {
        const newTransport: Transport = { ...transport }
        newTransport.gasExportPipelineLength = Number(e.currentTarget.value)
        setTransport(newTransport)
    }

    const handleSave = async () => {
        setIsSaving(true)
        if (topside) {
            const result = await (await GetTopsideService()).newUpdate(topside)
            setTopside(result)
        }
        if (surf) {
            const result = await (await GetSurfService()).newUpdate(surf)
            setSurf(result)
        }
        if (substructure) {
            const result = await (await GetSubstructureService()).newUpdate(substructure)
            setSubstrucutre(result)
        }
        if (transport) {
            const result = await (await GetTransportService()).newUpdate(transport)
            setTransport(result)
        }

        if (caseItem) {
            const result = await (await GetCaseService()).update(caseItem)
            setCase(result)
        }
        setIsSaving(false)
    }

    if (activeTab !== 4) { return null }

    return (
        <>
            <TopWrapper>
                <PageTitle variant="h2">Facilities</PageTitle>
                {!isSaving ? <Button onClick={handleSave}>Save</Button> : (
                    <Button>
                        <Progress.Dots />
                    </Button>
                )}
            </TopWrapper>
            <ColumnWrapper>
                <RowWrapper>
                    <NativeSelectField
                        id="platformConcept"
                        label="Platform concept"
                        onChange={handleSubstructureConceptChange}
                        value={substructure?.concept}
                    >
                        <option key="0" value={0}>No Concept</option>
                        <option key="1" value={1}>Tie-back to existing offshore platform</option>
                        <option key="2" value={2}>JACKET - Fixed Steel Jacket</option>
                        <option key="3" value={3}>GBS - Fixed Concrete Platform - Gravity Based Structure</option>
                        <option key="4" value={4}>TLP - Tension Leg Platform - Steel</option>
                        <option key="5" value={5}>SPAR Platform - Steel</option>
                        <option key="6" value={6}>SEMI - Semi Submersible - Steel</option>
                        <option key="7" value={7}>CIRCULAR BARGE - Sevan type FPSO</option>
                        <option key="8" value={8}>BARGE - Barge shaped - Spread Moored FPSO</option>
                        <option key="9" value={9}>FPSO - Ship shaped - TUrret Moored</option>
                        <option key="10" value={10}>TANKER - converted tanker FPSO - Turret Moored</option>
                        <option key="11" value={11}>JACK-UP Platform</option>
                        <option key="12" value={12}>Subsea to shore</option>
                    </NativeSelectField>
                    {substructure.concept === 1 && (
                        <HostWrapper>
                            <Label htmlFor="NumberInput" label="Host" />
                            <Input
                                id="NumberInput"
                                value={caseItem.host ?? ""}
                                disabled={false}
                                onChange={handleHostChange}
                            />
                        </HostWrapper>
                    )}
                </RowWrapper>
            </ColumnWrapper>
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
                        onChange={() => { }}
                        value={caseItem.facilitiesAvailability * 100}
                        integer
                        disabled
                        label="Facilities availability (%)"
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
                </RowWrapper>
            </ColumnWrapper>

            <ColumnWrapper>
                <Typography variant="h4">SURF</Typography>
                <RowWrapper>
                    <NumberInputField>
                        <CaseNumberInput
                            onChange={handleSurfTemplateCountChange}
                            value={surf?.templateCount}
                            integer
                            label="Templates"
                        />
                    </NumberInputField>
                    <CaseNumberInput
                        onChange={handleSurfRiserCountChange}
                        value={surf?.riserCount}
                        integer
                        label="Risers"
                    />
                </RowWrapper>
                <RowWrapper>
                    <NumberInputField>
                        <CaseNumberInput
                            onChange={handleSurfInfieldPipelineSystemLengthChange}
                            value={surf?.infieldPipelineSystemLength}
                            integer={false}
                            label="Production lines length"
                        />
                    </NumberInputField>
                    <NumberInputField>
                        <CaseNumberInput
                            onChange={handleSurfUmbilicalSystemLengthChange}
                            value={surf?.umbilicalSystemLength}
                            integer={false}
                            label="Umbilical system length"
                        />
                    </NumberInputField>
                    <NativeSelectField
                        id="productionFlowline"
                        label="Production flowline"
                        onChange={handleProductionFlowlineChange}
                        value={surf?.productionFlowline}
                    >
                        <option key="0" value={0}>No production flowline</option>
                        <option key="1" value={1}>Carbon</option>
                        <option key="2" value={2}>SS Clad</option>
                        <option key="3" value={3}>Cr13</option>
                        <option key="4" value={4}>Carbon + insulation</option>
                        <option key="5" value={5}>SS Clad + insulation</option>
                        <option key="6" value={6}>Cr13 + insulation</option>
                        <option key="7" value={7}>Carbon + insulation + DEH</option>
                        <option key="8" value={8}>SS Clad + insulation + DEH</option>
                        <option key="9" value={9}>Cr13 + insulation + DEH</option>
                        <option key="10" value={10}>Carbon + PIP</option>
                        <option key="11" value={11}>SS Clad + PIP</option>
                        <option key="12" value={12}>Cr13 + PIP</option>
                        <option key="13" value={13}>HDPE lined CS (Water injection only)</option>
                    </NativeSelectField>
                </RowWrapper>
            </ColumnWrapper>
            <ColumnWrapper>
                <Typography variant="h4">Substructure</Typography>
                <RowWrapper>
                    <NumberInputField>
                        <CaseNumberInput
                            onChange={handleSubstructureDryweightChange}
                            value={substructure?.dryWeight}
                            integer={false}
                            label="Substructure dry weight"
                        />
                    </NumberInputField>

                </RowWrapper>
            </ColumnWrapper>
            <ColumnWrapper>
                <Typography variant="h4">Transport</Typography>
                <RowWrapper>
                    <NumberInputField>
                        <CaseNumberInput
                            onChange={handleTransportOilExportPipelineLengthChange}
                            value={transport?.oilExportPipelineLength}
                            integer={false}
                            label="Oil export pipeline length"
                        />
                    </NumberInputField>
                    <NumberInputField>
                        <CaseNumberInput
                            onChange={handleTransportGasExportPipelineLengthChange}
                            value={transport?.gasExportPipelineLength}
                            integer={false}
                            label="Gas export pipeline length"
                        />
                    </NumberInputField>
                </RowWrapper>
            </ColumnWrapper>
        </>
    )
}

export default CaseFacilitiesTab

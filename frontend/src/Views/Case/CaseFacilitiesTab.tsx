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
    const handleFacilityOpexChange: ChangeEventHandler<HTMLInputElement> = async (e) => {
        const newTopside: Topside = { ...topside }
        newTopside.facilityOpex = Number(e.currentTarget.value)
        setTopside(newTopside)
    }

    const handleSurfCessationCostChange: ChangeEventHandler<HTMLInputElement> = async (e) => {
        const newSurf: Surf = { ...surf }
        newSurf.cessationCost = Number(e.currentTarget.value)
        setSurf(newSurf)
    }

    const handleTopsideDryWeightChange: ChangeEventHandler<HTMLInputElement> = async (e) => {
        const newTopside: Topside = { ...topside }
        newTopside.dryWeight = Math.max(Number(e.currentTarget.value), 0)
        setTopside(newTopside)
    }

    const handleTopsidePeakElectricityImportedChange: ChangeEventHandler<HTMLInputElement> = async (e) => {
        const newTopside: Topside = { ...topside }
        newTopside.peakElectricityImported = Math.max(Number(e.currentTarget.value), 0)
        setTopside(newTopside)
    }

    const handleTopsideProducerCountChange: ChangeEventHandler<HTMLInputElement> = async (e) => {
        const newTopside: Topside = { ...topside }
        newTopside.producerCount = Number(e.currentTarget.value)
        setTopside(newTopside)
    }

    const handleTopsideGasInjectorCountChange: ChangeEventHandler<HTMLInputElement> = async (e) => {
        const newTopside: Topside = { ...topside }
        newTopside.gasInjectorCount = Number(e.currentTarget.value)
        setTopside(newTopside)
    }

    const handleTopsideWaterInjectorCountChange: ChangeEventHandler<HTMLInputElement> = async (e) => {
        const newTopside: Topside = { ...topside }
        newTopside.waterInjectorCount = Number(e.currentTarget.value)
        setTopside(newTopside)
    }

    const handleTopsideOilCapacityChange: ChangeEventHandler<HTMLInputElement> = async (e) => {
        const newTopside: Topside = { ...topside }
        newTopside.oilCapacity = Math.max(Number(e.currentTarget.value), 0)
        setTopside(newTopside)
    }

    const handleTopsideGasCapacityChange: ChangeEventHandler<HTMLInputElement> = async (e) => {
        const newTopside: Topside = { ...topside }
        newTopside.gasCapacity = Math.max(Number(e.currentTarget.value), 0)
        setTopside(newTopside)
    }

    const handleTopsideWaterInjectionCapacityChange: ChangeEventHandler<HTMLInputElement> = async (e) => {
        const newTopside: Topside = { ...topside }
        newTopside.waterInjectionCapacity = Number(e.currentTarget.value)
        setTopside(newTopside)
    }

    const handleSurfTemplateCountChange: ChangeEventHandler<HTMLInputElement> = async (e) => {
        const newSurf: Surf = { ...surf }
        newSurf.templateCount = Math.max(Number(e.currentTarget.value), 0)
        setSurf(newSurf)
    }

    const handleSurfRiserCountChange: ChangeEventHandler<HTMLInputElement> = async (e) => {
        const newSurf: Surf = { ...surf }
        newSurf.riserCount = Math.max(Number(e.currentTarget.value), 0)
        setSurf(newSurf)
    }

    const handleSurfProducerCountChange: ChangeEventHandler<HTMLInputElement> = async (e) => {
        const newSurf: Surf = { ...surf }
        newSurf.producerCount = Number(e.currentTarget.value)
        setSurf(newSurf)
    }

    const handleSurfGasInjectorCountChange: ChangeEventHandler<HTMLInputElement> = async (e) => {
        const newSurf: Surf = { ...surf }
        newSurf.gasInjectorCount = Number(e.currentTarget.value)
        setSurf(newSurf)
    }

    const handleSurfWaterInjectorCountChange: ChangeEventHandler<HTMLInputElement> = async (e) => {
        const newSurf: Surf = { ...surf }
        newSurf.waterInjectorCount = Number(e.currentTarget.value)
        setSurf(newSurf)
    }

    const handleSurfInfieldPipelineSystemLengthChange: ChangeEventHandler<HTMLInputElement> = async (e) => {
        const newSurf: Surf = { ...surf }
        newSurf.infieldPipelineSystemLength = Math.max(Number(e.currentTarget.value), 0)
        setSurf(newSurf)
    }

    const handleSurfUmbilicalSystemLengthChange: ChangeEventHandler<HTMLInputElement> = async (e) => {
        const newSurf: Surf = { ...surf }
        newSurf.umbilicalSystemLength = Math.max(Number(e.currentTarget.value), 0)
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
        newSubstructure.dryWeight = Math.max(Number(e.currentTarget.value), 0)
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
        newTransport.oilExportPipelineLength = Math.max(Number(e.currentTarget.value), 0)
        setTransport(newTransport)
    }

    const handleTransportGasExportPipelineLengthChange: ChangeEventHandler<HTMLInputElement> = async (e) => {
        const newTransport: Transport = { ...transport }
        newTransport.gasExportPipelineLength = Math.max(Number(e.currentTarget.value), 0)
        setTransport(newTransport)
    }

    if (activeTab !== 4) { return null }

    return (
        <>
            <TopWrapper>
                <PageTitle variant="h2">Facilities</PageTitle>
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
                <RowWrapper>
                    <NumberInputField>
                        <CaseNumberInput
                            onChange={handleFacilityOpexChange}
                            defaultValue={Math.round(Number(topside?.facilityOpex) * 10) / 10}
                            integer={false}
                            label="Facility opex"
                            unit={`${project?.currency === 1 ? "MNOK" : "MUSD"}`}
                        />
                    </NumberInputField>
                    <CaseNumberInput
                        onChange={handleSurfCessationCostChange}
                        defaultValue={surf?.cessationCost}
                        integer={false}
                        label="Cessation cost"
                        unit={`${project?.currency === 1 ? "MNOK" : "MUSD"}`}
                    />
                </RowWrapper>
            </ColumnWrapper>
            <ColumnWrapper>
                <Typography variant="h4">Topside</Typography>
                <RowWrapper>
                    <NumberInputField>
                        <CaseNumberInput
                            onChange={handleTopsideDryWeightChange}
                            defaultValue={Math.round(Number(topside?.dryWeight) * 1) / 1}
                            integer
                            label="Topside dry weight"
                            unit="tonnes"
                        />
                    </NumberInputField>
                    <NumberInputField>
                        <CaseNumberInput
                            onChange={() => { }}
                            defaultValue={caseItem.facilitiesAvailability * 100}
                            integer
                            disabled
                            label="Facilities availability"
                            unit="%"
                        />
                    </NumberInputField>
                    <CaseNumberInput
                        onChange={handleTopsidePeakElectricityImportedChange}
                        defaultValue={Math.round(Number(topside?.peakElectricityImported) * 10) / 10}
                        integer={false}
                        label="Peak electricity imported"
                        unit="MW"
                    />
                </RowWrapper>
                <RowWrapper>
                    <NumberInputField>
                        <CaseNumberInput
                            onChange={handleTopsideOilCapacityChange}
                            defaultValue={Math.round(Number(topside?.oilCapacity) * 1) / 1}
                            integer
                            label="Oil capacity"
                            unit="Sm³/sd"
                        />
                    </NumberInputField>
                    <NumberInputField>
                        <CaseNumberInput
                            onChange={handleTopsideGasCapacityChange}
                            defaultValue={Math.round(Number(topside?.gasCapacity) * 10) / 10}
                            integer={false}
                            label="Gas capacity"
                            unit="MSm³/sd"

                        />
                    </NumberInputField>
                    <NumberInputField>
                        <CaseNumberInput
                            onChange={handleTopsideWaterInjectionCapacityChange}
                            defaultValue={Math.round(Number(topside?.waterInjectionCapacity) * 1) / 1}
                            integer
                            label="Water injection capacity"
                            unit="MSm³/sd"
                        />
                    </NumberInputField>
                </RowWrapper>
                <Typography variant="h5">Platform wells</Typography>
                <RowWrapper>
                    <NumberInputField>
                        <CaseNumberInput
                            onChange={handleTopsideProducerCountChange}
                            defaultValue={topside?.producerCount}
                            integer
                            label="Producer count"
                        />
                    </NumberInputField>
                    <NumberInputField>
                        <CaseNumberInput
                            onChange={handleTopsideGasInjectorCountChange}
                            defaultValue={topside?.gasInjectorCount}
                            integer
                            label="Gas injector count"
                        />
                    </NumberInputField>
                    <CaseNumberInput
                        onChange={handleTopsideWaterInjectorCountChange}
                        defaultValue={topside?.waterInjectorCount}
                        integer
                        label="Water injector count"
                    />
                </RowWrapper>
            </ColumnWrapper>

            <ColumnWrapper>
                <Typography variant="h4">SURF</Typography>
                <RowWrapper>
                    <NumberInputField>
                        <CaseNumberInput
                            onChange={handleSurfTemplateCountChange}
                            defaultValue={surf?.templateCount}
                            integer
                            label="Templates"
                        />
                    </NumberInputField>
                    <CaseNumberInput
                        onChange={handleSurfRiserCountChange}
                        defaultValue={surf?.riserCount}
                        integer
                        label="Risers"
                    />
                </RowWrapper>
                <RowWrapper>
                    <NumberInputField>
                        <CaseNumberInput
                            onChange={handleSurfInfieldPipelineSystemLengthChange}
                            defaultValue={Math.round(Number(surf?.infieldPipelineSystemLength) * 10) / 10}
                            integer={false}
                            label="Production lines length"
                            unit="km"
                        />
                    </NumberInputField>
                    <NumberInputField>
                        <CaseNumberInput
                            onChange={handleSurfUmbilicalSystemLengthChange}
                            defaultValue={Math.round(Number(surf?.umbilicalSystemLength) * 10) / 10}
                            integer={false}
                            label="Umbilical system length"
                            unit="km"
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
                <Typography variant="h5">Subsea wells</Typography>
                <RowWrapper>
                    <NumberInputField>
                        <CaseNumberInput
                            onChange={handleSurfProducerCountChange}
                            defaultValue={surf?.producerCount}
                            integer
                            label="Producer count"
                        />
                    </NumberInputField>
                    <NumberInputField>
                        <CaseNumberInput
                            onChange={handleSurfGasInjectorCountChange}
                            defaultValue={surf?.gasInjectorCount}
                            integer
                            label="Gas injector count"
                        />
                    </NumberInputField>
                    <CaseNumberInput
                        onChange={handleSurfWaterInjectorCountChange}
                        defaultValue={surf?.waterInjectorCount}
                        integer
                        label="Water injector count"
                    />
                </RowWrapper>
            </ColumnWrapper>
            <ColumnWrapper>
                <Typography variant="h4">Substructure</Typography>
                <RowWrapper>
                    <NumberInputField>
                        <CaseNumberInput
                            onChange={handleSubstructureDryweightChange}
                            defaultValue={Math.round(Number(substructure?.dryWeight) * 1) / 1}
                            integer
                            label="Substructure dry weight"
                            unit="tonnes"
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
                            defaultValue={Math.round(Number(transport?.oilExportPipelineLength) * 10) / 10}
                            integer={false}
                            label="Oil export pipeline length"
                            unit="km"
                        />
                    </NumberInputField>
                    <NumberInputField>
                        <CaseNumberInput
                            onChange={handleTransportGasExportPipelineLengthChange}
                            defaultValue={Math.round(Number(transport?.gasExportPipelineLength) * 10) / 10}
                            integer={false}
                            label="Gas export pipeline length"
                            unit="km"
                        />
                    </NumberInputField>
                </RowWrapper>
            </ColumnWrapper>
        </>
    )
}

export default CaseFacilitiesTab

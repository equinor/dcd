import {
    Dispatch,
    SetStateAction,
    ChangeEventHandler,
} from "react"
import {
    NativeSelect, Typography, Input,
} from "@equinor/eds-core-react"
import Grid from "@mui/material/Grid"
import CaseNumberInput from "../../Input/CaseNumberInput"
import InputSwitcher from "../../Input/InputSwitcher"
import { useProjectContext } from "../../../Context/ProjectContext"
import { useCaseContext } from "../../../Context/CaseContext"

interface Props {
    topside: Components.Schemas.TopsideDto,
    setTopside: Dispatch<SetStateAction<Components.Schemas.TopsideDto | undefined>>,
    surf: Components.Schemas.SurfDto,
    setSurf: Dispatch<SetStateAction<Components.Schemas.SurfDto | undefined>>,
    substructure: Components.Schemas.SubstructureDto,
    setSubstrucutre: Dispatch<SetStateAction<Components.Schemas.SubstructureDto | undefined>>,
    transport: Components.Schemas.TransportDto,
    setTransport: Dispatch<SetStateAction<Components.Schemas.TransportDto | undefined>>,
}

const CaseFacilitiesTab = ({
    topside, setTopside,
    surf, setSurf,
    substructure, setSubstrucutre,
    transport, setTransport,
}: Props) => {
    const { project } = useProjectContext()
    const { projectCase, setProjectCaseEdited } = useCaseContext()
    if (!projectCase) { return null }

    const platformConceptValues: { [key: number]: string } = {
        0: "No Concept",
        1: "Tie-back to existing offshore platform",
        2: "JACKET - Fixed Steel Jacket",
        3: "GBS - Fixed Concrete Platform - Gravity Based Structure",
        4: "TLP - Tension Leg Platform - Steel",
        5: "SPAR Platform - Steel",
        6: "SEMI - Semi Submersible - Steel",
        7: "CIRCULAR BARGE - Sevan type FPSO",
        8: "BARGE - Barge shaped - Spread Moored FPSO",
        9: "FPSO - Ship shaped - TUrret Moored",
        10: "TANKER - converted tanker FPSO - Turret Moored",
        11: "JACK-UP Platform",
        12: "Subsea to shore",
    }
    const productionFlowlineValues: { [key: number]: string } = {
        0: "No production flowline",
        1: "Carbon",
        2: "SS Clad",
        3: "Cr13",
        4: "Carbon + insulation",
        5: "SS Clad + insulation",
        6: "Cr13 + insulation",
        7: "Carbon + insulation + DEH",
        8: "SS Clad + insulation + DEH",
        9: "Cr13 + insulation + DEH",
        10: "Carbon + PIP",
        11: "SS Clad + PIP",
        12: "Cr13 + PIP",
        13: "HDPE lined CS (Water injection only)",
    }

    const handleFacilityOpexChange: ChangeEventHandler<HTMLInputElement> = async (e) => {
        const newTopside = { ...topside }
        newTopside.facilityOpex = e.currentTarget.value.length > 0 ? Number(e.currentTarget.value) : 0
        setTopside(newTopside)
    }

    const handleSurfCessationCostChange: ChangeEventHandler<HTMLInputElement> = async (e) => {
        const newSurf = { ...surf }
        newSurf.cessationCost = e.currentTarget.value.length > 0 ? Number(e.currentTarget.value) : 0
        setSurf(newSurf)
    }

    const handleTopsideDryWeightChange: ChangeEventHandler<HTMLInputElement> = async (e) => {
        const newTopside = { ...topside }
        newTopside.dryWeight = e.currentTarget.value.length > 0 ? Math.max(Number(e.currentTarget.value), 0) : 0
        setTopside(newTopside)
    }

    const handleTopsidePeakElectricityImportedChange: ChangeEventHandler<HTMLInputElement> = async (e) => {
        const newTopside = { ...topside }
        newTopside.peakElectricityImported = e.currentTarget.value.length > 0 ? Math.max(Number(e.currentTarget.value), 0) : 0
        setTopside(newTopside)
    }

    const handleTopsideProducerCountChange: ChangeEventHandler<HTMLInputElement> = async (e) => {
        const newTopside = { ...topside }
        newTopside.producerCount = e.currentTarget.value.length > 0 ? Number(e.currentTarget.value) : 0
        setTopside(newTopside)
    }

    const handleTopsideGasInjectorCountChange: ChangeEventHandler<HTMLInputElement> = async (e) => {
        const newTopside = { ...topside }
        newTopside.gasInjectorCount = e.currentTarget.value.length > 0 ? Number(e.currentTarget.value) : 0
        setTopside(newTopside)
    }

    const handleTopsideWaterInjectorCountChange: ChangeEventHandler<HTMLInputElement> = async (e) => {
        const newTopside = { ...topside }
        newTopside.waterInjectorCount = e.currentTarget.value.length > 0 ? Number(e.currentTarget.value) : 0
        setTopside(newTopside)
    }

    const handleTopsideOilCapacityChange: ChangeEventHandler<HTMLInputElement> = async (e) => {
        const newTopside = { ...topside }
        newTopside.oilCapacity = e.currentTarget.value.length > 0 ? Math.max(Number(e.currentTarget.value), 0) : 0
        setTopside(newTopside)
    }

    const handleTopsideGasCapacityChange: ChangeEventHandler<HTMLInputElement> = async (e) => {
        const newTopside = { ...topside }
        newTopside.gasCapacity = e.currentTarget.value.length > 0 ? Math.max(Number(e.currentTarget.value), 0) : 0
        setTopside(newTopside)
    }

    const handleTopsideWaterInjectionCapacityChange: ChangeEventHandler<HTMLInputElement> = async (e) => {
        const newTopside = { ...topside }
        newTopside.waterInjectionCapacity = e.currentTarget.value.length > 0 ? Number(e.currentTarget.value) : 0
        setTopside(newTopside)
    }

    const handleSurfTemplateCountChange: ChangeEventHandler<HTMLInputElement> = async (e) => {
        const newSurf = { ...surf }
        newSurf.templateCount = e.currentTarget.value.length > 0 ? Math.max(Number(e.currentTarget.value), 0) : 0
        setSurf(newSurf)
    }

    const handleSurfRiserCountChange: ChangeEventHandler<HTMLInputElement> = async (e) => {
        const newSurf = { ...surf }
        newSurf.riserCount = e.currentTarget.value.length > 0 ? Math.max(Number(e.currentTarget.value), 0) : 0
        setSurf(newSurf)
    }

    const handleSurfProducerCountChange: ChangeEventHandler<HTMLInputElement> = async (e) => {
        const newSurf = { ...surf }
        newSurf.producerCount = e.currentTarget.value.length > 0 ? Number(e.currentTarget.value) : 0
        setSurf(newSurf)
    }

    const handleSurfGasInjectorCountChange: ChangeEventHandler<HTMLInputElement> = async (e) => {
        const newSurf = { ...surf }
        newSurf.gasInjectorCount = e.currentTarget.value.length > 0 ? Number(e.currentTarget.value) : 0
        setSurf(newSurf)
    }

    const handleSurfWaterInjectorCountChange: ChangeEventHandler<HTMLInputElement> = async (e) => {
        const newSurf = { ...surf }
        newSurf.waterInjectorCount = e.currentTarget.value.length > 0 ? Number(e.currentTarget.value) : 0
        setSurf(newSurf)
    }

    const handleSurfInfieldPipelineSystemLengthChange: ChangeEventHandler<HTMLInputElement> = async (e) => {
        const newSurf = { ...surf }
        newSurf.infieldPipelineSystemLength = e.currentTarget.value.length > 0 ? Math.max(Number(e.currentTarget.value), 0) : 0
        setSurf(newSurf)
    }

    const handleSurfUmbilicalSystemLengthChange: ChangeEventHandler<HTMLInputElement> = async (e) => {
        const newSurf = { ...surf }
        newSurf.umbilicalSystemLength = e.currentTarget.value.length > 0 ? Math.max(Number(e.currentTarget.value), 0) : 0
        setSurf(newSurf)
    }

    const handleProductionFlowlineChange: ChangeEventHandler<HTMLSelectElement> = async (e) => {
        if ([0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13].indexOf(Number(e.currentTarget.value)) !== -1) {
            const newProductionFlowline: Components.Schemas.ProductionFlowline = Number(e.currentTarget.value) as Components.Schemas.ProductionFlowline
            const newSurf = { ...surf }
            newSurf.productionFlowline = newProductionFlowline
            setSurf(newSurf)
        }
    }

    const handleSubstructureDryweightChange: ChangeEventHandler<HTMLInputElement> = async (e) => {
        const newSubstructure = { ...substructure }
        newSubstructure.dryWeight = e.currentTarget.value.length > 0 ? Math.max(Number(e.currentTarget.value), 0) : 0
        setSubstrucutre(newSubstructure)
    }

    const handleSubstructureConceptChange: ChangeEventHandler<HTMLSelectElement> = async (e) => {
        if ([0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12].indexOf(Number(e.currentTarget.value)) !== -1) {
            const newConcept: Components.Schemas.Concept = Number(e.currentTarget.value) as Components.Schemas.Concept
            const newSubstructure = { ...substructure }
            newSubstructure.concept = newConcept
            if (newConcept !== 1) {
                const newCase = { ...projectCase }
                newCase.host = ""
                setProjectCaseEdited(newCase)
            }
            setSubstrucutre(newSubstructure)
        }
    }

    const handleHostChange: ChangeEventHandler<HTMLInputElement> = async (e) => {
        const newCase = { ...projectCase }
        newCase.host = e.currentTarget.value
        setProjectCaseEdited(newCase)
    }

    const handleTransportOilExportPipelineLengthChange: ChangeEventHandler<HTMLInputElement> = async (e) => {
        const newTransport = { ...transport }
        newTransport.oilExportPipelineLength = e.currentTarget.value.length > 0 ? Math.max(Number(e.currentTarget.value), 0) : 0
        setTransport(newTransport)
    }

    const handleTransportGasExportPipelineLengthChange: ChangeEventHandler<HTMLInputElement> = async (e) => {
        const newTransport = { ...transport }
        newTransport.gasExportPipelineLength = e.currentTarget.value.length > 0 ? Math.max(Number(e.currentTarget.value), 0) : 0
        setTransport(newTransport)
    }

    return (
        <Grid container spacing={2}>
            <Grid item xs={12} md={4}>
                <InputSwitcher
                    value={platformConceptValues[substructure?.concept]}
                    label="Platform concept"
                >
                    <NativeSelect
                        id="platformConcept"
                        label=""
                        onChange={handleSubstructureConceptChange}
                        value={substructure?.concept}
                    >
                        {
                            Object.keys(platformConceptValues).map((key) => (
                                <option key={key} value={key}>{platformConceptValues[Number(key)]}</option>
                            ))
                        }
                    </NativeSelect>
                </InputSwitcher>
            </Grid>
            {substructure.concept === 1 && (
                <Grid item xs={12} md={4}>
                    <InputSwitcher
                        value={projectCase.host ?? ""}
                        label="Host"
                    >
                        <Input
                            id="NumberInput"
                            value={projectCase.host ?? ""}
                            disabled={false}
                            onChange={handleHostChange}
                        />
                    </InputSwitcher>
                </Grid>
            )}
            <Grid item xs={12} md={4}>
                <InputSwitcher
                    value={`${Math.round(Number(topside?.facilityOpex) * 10) / 10} ${project?.currency === 1 ? "MNOK" : "MUSD"}`}
                    label="Facility opex"
                >
                    <CaseNumberInput
                        onChange={handleFacilityOpexChange}
                        defaultValue={Math.round(Number(topside?.facilityOpex) * 10) / 10}
                        integer={false}
                        unit={`${project?.currency === 1 ? "MNOK" : "MUSD"}`}
                    />
                </InputSwitcher>
            </Grid>
            <Grid item xs={12} md={4}>
                <InputSwitcher
                    value={`${Math.round(Number(surf?.cessationCost) * 10) / 10} ${project?.currency === 1 ? "MNOK" : "MUSD"}`}
                    label="Cessation cost"
                >
                    <CaseNumberInput
                        onChange={handleSurfCessationCostChange}
                        defaultValue={Math.round(Number(surf?.cessationCost) * 10) / 10}
                        integer={false}
                        unit={`${project?.currency === 1 ? "MNOK" : "MUSD"}`}
                    />
                </InputSwitcher>
            </Grid>
            <Grid item xs={12}>
                <Typography variant="h4">Topside</Typography>
            </Grid>
            <Grid item xs={12} md={4}>
                <InputSwitcher
                    value={`${Math.round(Number(topside?.dryWeight) * 1) / 1} tonnes`}
                    label="Topside dry weight"
                >
                    <CaseNumberInput
                        onChange={handleTopsideDryWeightChange}
                        defaultValue={Math.round(Number(topside?.dryWeight) * 1) / 1}
                        integer
                        unit="tonnes"
                        min={0}
                        max={1000000}
                    />
                </InputSwitcher>
            </Grid>
            <Grid item xs={12} md={4}>
                <InputSwitcher
                    value={`${projectCase.facilitiesAvailability ?? 0 * 100}%`}
                    label="Facilities availability"
                >
                    <CaseNumberInput
                        onChange={() => { }}
                        defaultValue={projectCase.facilitiesAvailability ?? 0 * 100}
                        integer
                        disabled
                        unit="%"
                    />
                </InputSwitcher>
            </Grid>
            <Grid item xs={12} md={4}>
                <InputSwitcher
                    value={`${Math.round(Number(topside?.peakElectricityImported) * 10) / 10} MW`}
                    label="Peak electricity imported"
                >
                    <CaseNumberInput
                        onChange={handleTopsidePeakElectricityImportedChange}
                        defaultValue={Math.round(Number(topside?.peakElectricityImported) * 10) / 10}
                        integer={false}
                        unit="MW"
                        min={0}
                        max={1000000}
                    />
                </InputSwitcher>
            </Grid>
            <Grid item xs={12} md={4}>
                <InputSwitcher
                    value={`${Math.round(Number(topside?.oilCapacity) * 1) / 1} Sm³/sd`}
                    label="Oil capacity"
                >
                    <CaseNumberInput
                        onChange={handleTopsideOilCapacityChange}
                        defaultValue={Math.round(Number(topside?.oilCapacity) * 1) / 1}
                        integer
                        unit="Sm³/sd"
                        min={0}
                        max={1000000}
                    />
                </InputSwitcher>
            </Grid>
            <Grid item xs={12} md={4}>
                <InputSwitcher
                    value={`${Math.round(Number(topside?.gasCapacity) * 10) / 10} MSm³/sd`}
                    label="Gas capacity"
                >
                    <CaseNumberInput
                        onChange={handleTopsideGasCapacityChange}
                        defaultValue={Math.round(Number(topside?.gasCapacity) * 10) / 10}
                        integer={false}
                        unit="MSm³/sd"
                        min={0}
                        max={1000000}
                    />
                </InputSwitcher>
            </Grid>
            <Grid item xs={12} md={4}>
                <InputSwitcher
                    value={`${Math.round(Number(topside?.waterInjectionCapacity) * 1) / 1} MSm³/sd`}
                    label="Water injection capacity"
                >
                    <CaseNumberInput
                        onChange={handleTopsideWaterInjectionCapacityChange}
                        defaultValue={Math.round(Number(topside?.waterInjectionCapacity) * 1) / 1}
                        integer
                        unit="MSm³/sd"
                    />
                </InputSwitcher>
            </Grid>
            <Grid item xs={12}>
                <Typography variant="h4">Platform wells</Typography>
            </Grid>
            <Grid item xs={12} md={4}>
                <InputSwitcher
                    value={`${topside?.producerCount}`}
                    label="Producer count"
                >
                    <CaseNumberInput
                        onChange={handleTopsideProducerCountChange}
                        defaultValue={topside?.producerCount}
                        integer
                        min={0}
                        max={1000000}
                    />
                </InputSwitcher>
            </Grid>
            <Grid item xs={12} md={4}>
                <InputSwitcher
                    value={`${topside?.gasInjectorCount}`}
                    label="Gas injector count"
                >
                    <CaseNumberInput
                        onChange={handleTopsideGasInjectorCountChange}
                        defaultValue={topside?.gasInjectorCount}
                        integer
                        min={0}
                        max={1000000}
                    />
                </InputSwitcher>
            </Grid>
            <Grid item xs={12} md={4}>
                <InputSwitcher
                    value={`${topside?.waterInjectorCount}`}
                    label="Water injector count"
                >
                    <CaseNumberInput
                        onChange={handleTopsideWaterInjectorCountChange}
                        defaultValue={topside?.waterInjectorCount}
                        integer
                        min={0}
                        max={1000000}
                    />
                </InputSwitcher>
            </Grid>
            <Grid item xs={12}>
                <Typography variant="h4">SURF</Typography>
            </Grid>
            <Grid item xs={12} md={4}>
                <InputSwitcher value={`${surf?.templateCount}`} label="Templates">
                    <CaseNumberInput
                        onChange={handleSurfTemplateCountChange}
                        defaultValue={surf?.templateCount}
                        integer
                        min={0}
                        max={1000000}
                    />
                </InputSwitcher>
            </Grid>
            <Grid item xs={12} md={4}>
                <InputSwitcher value={`${surf?.riserCount}`} label="Risers">
                    <CaseNumberInput
                        onChange={handleSurfRiserCountChange}
                        defaultValue={surf?.riserCount}
                        integer
                        min={0}
                        max={1000000}
                    />
                </InputSwitcher>
            </Grid>
            <Grid item xs={12} md={4}>
                <InputSwitcher value={`${Math.round(Number(surf?.infieldPipelineSystemLength) * 10) / 10} km`} label="Production lines length">
                    <CaseNumberInput
                        onChange={handleSurfInfieldPipelineSystemLengthChange}
                        defaultValue={Math.round(Number(surf?.infieldPipelineSystemLength) * 10) / 10}
                        integer={false}
                        unit="km"
                        min={0}
                        max={1000000}
                    />
                </InputSwitcher>
            </Grid>
            <Grid item xs={12} md={4}>
                <InputSwitcher value={`${Math.round(Number(surf?.umbilicalSystemLength) * 10) / 10} km`} label="Umbilical system length">
                    <CaseNumberInput
                        onChange={handleSurfUmbilicalSystemLengthChange}
                        defaultValue={Math.round(Number(surf?.umbilicalSystemLength) * 10) / 10}
                        integer={false}
                        unit="km"
                        min={0}
                        max={1000000}
                    />
                </InputSwitcher>
            </Grid>
            <Grid item xs={12} md={4}>
                <InputSwitcher
                    value={productionFlowlineValues[surf?.productionFlowline]}
                    label="Production flowline"
                >
                    <NativeSelect
                        id="productionFlowline"
                        label=""
                        onChange={handleProductionFlowlineChange}
                        value={surf?.productionFlowline}
                    >
                        {
                            Object.keys(productionFlowlineValues).map((key) => (
                                <option key={key} value={key}>{productionFlowlineValues[Number(key)]}</option>
                            ))
                        }
                    </NativeSelect>
                </InputSwitcher>
            </Grid>
            <Grid item xs={12}>
                <Typography variant="h4">Subsea wells</Typography>
            </Grid>
            <Grid item xs={12} md={4}>
                <InputSwitcher value={`${surf?.producerCount}`} label="Producer count">
                    <CaseNumberInput
                        onChange={handleSurfProducerCountChange}
                        defaultValue={surf?.producerCount}
                        integer
                    />
                </InputSwitcher>
            </Grid>
            <Grid item xs={12} md={4}>
                <InputSwitcher value={`${surf?.gasInjectorCount}`} label="Gas injector count">
                    <CaseNumberInput
                        onChange={handleSurfGasInjectorCountChange}
                        defaultValue={surf?.gasInjectorCount}
                        integer
                        min={0}
                        max={1000000}
                    />
                </InputSwitcher>
            </Grid>
            <Grid item xs={12} md={4}>
                <InputSwitcher value={`${surf?.waterInjectorCount}`} label="Water injector count">
                    <CaseNumberInput
                        onChange={handleSurfWaterInjectorCountChange}
                        defaultValue={surf?.waterInjectorCount}
                        integer
                        min={0}
                        max={1000000}
                    />
                </InputSwitcher>
            </Grid>
            <Grid item xs={12}>
                <Typography variant="h4">Transport</Typography>
            </Grid>
            <Grid item xs={12} md={4}>
                <InputSwitcher value={`${Math.round(Number(transport?.oilExportPipelineLength) * 10) / 10} km`} label="Oil export pipeline length">
                    <CaseNumberInput
                        onChange={handleTransportOilExportPipelineLengthChange}
                        defaultValue={Math.round(Number(transport?.oilExportPipelineLength) * 10) / 10}
                        integer={false}
                        unit="km"
                        min={0}
                        max={1000000}
                    />
                </InputSwitcher>
            </Grid>
            <Grid item xs={12} md={4}>
                <InputSwitcher value={`${Math.round(Number(transport?.gasExportPipelineLength) * 10) / 10} km`} label="Gas export pipeline length">
                    <CaseNumberInput
                        onChange={handleTransportGasExportPipelineLengthChange}
                        defaultValue={Math.round(Number(transport?.gasExportPipelineLength) * 10) / 10}
                        integer={false}
                        unit="km"
                        min={0}
                        max={1000000}
                    />
                </InputSwitcher>
            </Grid>
            <Grid item xs={12}>
                <Typography variant="h4">Substructure</Typography>
            </Grid>
            <Grid item xs={12} md={4}>
                <InputSwitcher value={`${Math.round(Number(substructure?.dryWeight) * 1) / 1} tonnes`} label="Substructure dry weight">
                    <CaseNumberInput
                        onChange={handleSubstructureDryweightChange}
                        defaultValue={Math.round(Number(substructure?.dryWeight) * 1) / 1}
                        integer
                        unit="tonnes"
                        min={0}
                        max={1000000}
                    />
                </InputSwitcher>
            </Grid>
        </Grid>
    )
}

export default CaseFacilitiesTab

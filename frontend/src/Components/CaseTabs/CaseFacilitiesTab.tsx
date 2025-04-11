import { Typography } from "@equinor/eds-core-react"
import Grid from "@mui/material/Grid2"
import styled from "styled-components"

import PROSPBar from "./Components/PROSPBar"

import SwitchableDropdownInput from "@/Components/Input/SwitchableDropdownInput"
import SwitchableNumberInput from "@/Components/Input/SwitchableNumberInput"
import SwitchableStringInput from "@/Components/Input/SwitchableStringInput"
import CaseFasilitiesTabSkeleton from "@/Components/LoadingSkeletons/CaseFacilitiesTabSkeleton"
import { useDataFetch, useCaseApiData } from "@/Hooks"
import {
    useTopsideMutation,
    useSurfMutation,
    useTransportMutation,
    useCaseMutation,
    useSubstructureMutation,
} from "@/Hooks/Mutations"
import { Concept, Source } from "@/Models/enums"
import { useProjectContext } from "@/Store/ProjectContext"
import { formatCurrencyUnit, roundToDecimals } from "@/Utils/FormatingUtils"

const TabContainer = styled(Grid)`
    max-width: 1000px;
`

const CaseFacilitiesTab = () => {
    const revisionAndProjectData = useDataFetch()
    const { projectId } = useProjectContext()
    const { apiData } = useCaseApiData()
    const {
        updateFacilityOpex,
        updateDryWeight: updateTopsideDryWeight,
        updatePeakElectricityImported,
        updateOilCapacity,
        updateGasCapacity,
        updateWaterInjectionCapacity,
        updateProducerCount: updateTopsideProducerCount,
        updateGasInjectorCount: updateTopsideGasInjectorCount,
        updateWaterInjectorCount: updateTopsideWaterInjectorCount,
    } = useTopsideMutation()

    const {
        updateProductionFlowline,
        updateCessationCost,
        updateTemplateCount,
        updateRiserCount,
        updateInfieldPipelineSystemLength,
        updateUmbilicalSystemLength,
        updateProducerCount: updateSurfProducerCount,
        updateGasInjectorCount: updateSurfGasInjectorCount,
        updateWaterInjectorCount: updateSurfWaterInjectorCount,
    } = useSurfMutation()

    const {
        updateOilExportPipelineLength,
        updateGasExportPipelineLength,
    } = useTransportMutation()

    const { updateHost } = useCaseMutation()

    const {
        updateConcept,
        updateDryWeight: updateSubstructureDryWeight,
    } = useSubstructureMutation()

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

    const sourceValues: { [key: number]: string } = {
        [Source.ConceptApp]: "Concept App",
        [Source.Prosp]: "Prosp",
    }

    const caseData = apiData?.case
    const topsideData = apiData?.topside
    const surfData = apiData?.surf
    const transportData = apiData?.transport
    const substructureData = apiData?.substructure
    const onshorePowerSupplyData = apiData?.onshorePowerSupply

    if (
        !caseData
        || !revisionAndProjectData
        || !topsideData
        || !surfData
        || !transportData
        || !substructureData
        || !onshorePowerSupplyData
    ) {
        return (<CaseFasilitiesTabSkeleton />)
    }

    return (
        <TabContainer container spacing={2}>
            <Grid size={{ xs: 12, md: 4 }}>
                <SwitchableDropdownInput
                    value={substructureData.concept}
                    options={platformConceptValues}
                    label="Platform concept"
                    id={`substructure-concept-${substructureData.id}`}
                    onSubmit={(newValue) => updateConcept(substructureData.id, newValue)}
                />
            </Grid>
            {substructureData.concept === Concept.TieBack && (
                <Grid size={{ xs: 12, md: 4 }}>
                    <SwitchableStringInput
                        label="Host"
                        value={caseData.host || ""}
                        id={`case-host-${caseData.caseId}`}
                        onSubmit={(newValue) => updateHost(newValue)}
                    />
                </Grid>
            )}
            <Grid size={{ xs: 12, md: 4 }}>
                <SwitchableNumberInput
                    label="Facility opex"
                    value={roundToDecimals(Number(topsideData.facilityOpex), 1)}
                    integer={false}
                    unit={formatCurrencyUnit(revisionAndProjectData.commonProjectAndRevisionData.currency)}
                    id={`topside-facility-opex-${topsideData.id}`}
                    onSubmit={(newValue) => updateFacilityOpex(topsideData.id, newValue)}
                />
            </Grid>
            <Grid size={{ xs: 12, md: 4 }}>
                <SwitchableNumberInput
                    label="Cessation cost"
                    value={roundToDecimals(Number(surfData?.cessationCost), 1)}
                    integer={false}
                    unit={formatCurrencyUnit(revisionAndProjectData.commonProjectAndRevisionData.currency)}
                    id={`surf-cessation-cost-${surfData.id}`}
                    onSubmit={(newValue) => updateCessationCost(newValue)}
                />
            </Grid>
            <Grid size={12}>
                <Typography variant="h4">Topside</Typography>
            </Grid>
            <Grid size={{ xs: 12, md: 4 }}>
                <SwitchableNumberInput
                    label="Topside dry weight"
                    value={roundToDecimals(Number(topsideData.dryWeight), 0)}
                    integer
                    unit="tonnes"
                    min={0}
                    max={1_000_000}
                    id={`topside-dry-weight-${topsideData.id}`}
                    onSubmit={(newValue) => updateTopsideDryWeight(topsideData.id, newValue)}
                />
            </Grid>
            <Grid size={{ xs: 12, md: 4 }}>
                <SwitchableNumberInput
                    label="Facilities availability"
                    value={caseData.facilitiesAvailability ?? 0 * 100}
                    integer
                    disabled
                    unit="%"
                    min={0}
                    max={100}
                    id="case-facilities-availability"
                    onSubmit={() => { }}
                />
            </Grid>
            <Grid size={{ xs: 12, md: 4 }}>
                <SwitchableNumberInput
                    label="Peak electricity imported"
                    value={roundToDecimals(Number(topsideData.peakElectricityImported), 1)}
                    integer={false}
                    unit="MW"
                    min={0}
                    max={1_000_000}
                    id={`topside-peak-electricity-imported-${topsideData.id}`}
                    onSubmit={(newValue) => updatePeakElectricityImported(topsideData.id, newValue)}
                />
            </Grid>
            <Grid size={{ xs: 12, md: 4 }}>
                <SwitchableNumberInput
                    label="Oil capacity"
                    value={roundToDecimals(Number(topsideData.oilCapacity), 0)}
                    integer
                    unit="Sm³/sd"
                    min={0}
                    max={1_000_000}
                    id={`topside-oil-capacity-${topsideData.id}`}
                    onSubmit={(newValue) => updateOilCapacity(topsideData.id, newValue)}
                />
            </Grid>
            <Grid size={{ xs: 12, md: 4 }}>
                <SwitchableNumberInput
                    label="Gas capacity"
                    value={roundToDecimals(Number(topsideData.gasCapacity), 1)}
                    integer={false}
                    unit="MSm³/sd"
                    min={0}
                    max={1_000_000}
                    id={`topside-gas-capacity-${topsideData.id}`}
                    onSubmit={(newValue) => updateGasCapacity(topsideData.id, newValue)}
                />
            </Grid>
            <Grid size={{ xs: 12, md: 4 }}>
                <SwitchableNumberInput
                    label="Water injection capacity"
                    value={roundToDecimals(Number(topsideData.waterInjectionCapacity), 0)}
                    integer
                    unit="MSm³/sd"
                    id={`topside-water-injection-capacity-${topsideData.id}`}
                    onSubmit={(newValue) => updateWaterInjectionCapacity(topsideData.id, newValue)}
                />
            </Grid>
            <Grid size={12}>
                <Typography variant="h4">Platform wells</Typography>
            </Grid>
            <Grid size={{ xs: 12, md: 4 }}>
                <SwitchableNumberInput
                    label="Producer count"
                    value={topsideData.producerCount}
                    integer
                    min={0}
                    max={1_000_000}
                    id={`topside-producer-count-${topsideData.id}`}
                    onSubmit={(newValue) => updateTopsideProducerCount(topsideData.id, newValue)}
                />
            </Grid>
            <Grid size={{ xs: 12, md: 4 }}>
                <SwitchableNumberInput
                    label="Gas injector count"
                    value={topsideData.gasInjectorCount}
                    integer
                    min={0}
                    max={1_000_000}
                    id={`topside-gas-injector-count-${topsideData.id}`}
                    onSubmit={(newValue) => updateTopsideGasInjectorCount(topsideData.id, newValue)}
                />
            </Grid>
            <Grid size={{ xs: 12, md: 4 }}>
                <SwitchableNumberInput
                    label="Water injector count"
                    value={topsideData.waterInjectorCount}
                    integer
                    min={0}
                    max={1_000_000}
                    id={`topside-water-injector-count-${topsideData.id}`}
                    onSubmit={(newValue) => updateTopsideWaterInjectorCount(topsideData.id, newValue)}
                />
            </Grid>
            <Grid size={12}>
                <Typography variant="h4">SURF</Typography>
            </Grid>
            <Grid size={{ xs: 12, md: 4 }}>
                <SwitchableNumberInput
                    label="Templates"
                    value={surfData.templateCount}
                    integer
                    min={0}
                    max={1_000_000}
                    id={`surf-template-count-${surfData.id}`}
                    onSubmit={(newValue) => updateTemplateCount(newValue)}
                />
            </Grid>
            <Grid size={{ xs: 12, md: 4 }}>
                <SwitchableNumberInput
                    label="Risers"
                    value={surfData.riserCount}
                    integer
                    min={0}
                    max={1_000_000}
                    id={`surf-riser-count-${surfData.id}`}
                    onSubmit={(newValue) => updateRiserCount(newValue)}
                />
            </Grid>
            <Grid size={{ xs: 12, md: 4 }}>
                <SwitchableNumberInput
                    label="Production lines length"
                    value={roundToDecimals(Number(surfData.infieldPipelineSystemLength), 1)}
                    integer={false}
                    unit="km"
                    min={0}
                    max={1_000_000}
                    id={`surf-infield-pipeline-system-length-${surfData.id}`}
                    onSubmit={(newValue) => updateInfieldPipelineSystemLength(newValue)}
                />
            </Grid>
            <Grid size={{ xs: 12, md: 4 }}>
                <SwitchableNumberInput
                    label="Umbilical system length"
                    value={roundToDecimals(Number(surfData.umbilicalSystemLength), 1)}
                    integer={false}
                    unit="km"
                    min={0}
                    max={1_000_000}
                    id={`surf-umbilical-system-length-${surfData.id}`}
                    onSubmit={(newValue) => updateUmbilicalSystemLength(newValue)}
                />
            </Grid>
            <Grid size={{ xs: 12, md: 4 }}>
                <SwitchableDropdownInput
                    value={surfData.productionFlowline}
                    options={productionFlowlineValues}
                    label="Production flowline"
                    id={`surf-production-flowline-${surfData.id}`}
                    onSubmit={(newValue) => updateProductionFlowline(newValue)}
                />
            </Grid>
            <Grid size={12}>
                <Typography variant="h4">Subsea wells</Typography>
            </Grid>
            <Grid size={{ xs: 12, md: 4 }}>
                <SwitchableNumberInput
                    label="Producer count"
                    value={surfData.producerCount}
                    integer
                    id={`surf-producer-count-${surfData.id}`}
                    onSubmit={(newValue) => updateSurfProducerCount(newValue)}
                />
            </Grid>
            <Grid size={{ xs: 12, md: 4 }}>
                <SwitchableNumberInput
                    label="Gas injector count"
                    value={surfData.gasInjectorCount}
                    integer
                    min={0}
                    max={1_000_000}
                    id={`surf-gas-injector-count-${surfData.id}`}
                    onSubmit={(newValue) => updateSurfGasInjectorCount(newValue)}
                />
            </Grid>
            <Grid size={{ xs: 12, md: 4 }}>
                <SwitchableNumberInput
                    label="Water injector count"
                    value={surfData.waterInjectorCount}
                    integer
                    min={0}
                    max={1_000_000}
                    id={`surf-water-injector-count-${surfData.id}`}
                    onSubmit={(newValue) => updateSurfWaterInjectorCount(newValue)}
                />
            </Grid>
            <Grid size={12}>
                <Typography variant="h4">Transport</Typography>
            </Grid>
            <Grid size={{ xs: 12, md: 4 }}>
                <SwitchableNumberInput
                    label="Oil export pipeline length"
                    value={roundToDecimals(Number(transportData.oilExportPipelineLength), 1)}
                    integer={false}
                    unit="km"
                    min={0}
                    max={1_000_000}
                    id={`transport-oil-export-pipeline-length-${transportData.id}`}
                    onSubmit={(newValue) => updateOilExportPipelineLength(transportData.id, newValue)}
                />
            </Grid>
            <Grid size={{ xs: 12, md: 4 }}>
                <SwitchableNumberInput
                    label="Gas export pipeline length"
                    value={roundToDecimals(Number(transportData.gasExportPipelineLength), 1)}
                    integer={false}
                    unit="km"
                    min={0}
                    max={1_000_000}
                    id={`transport-gas-export-pipeline-length-${transportData.id}`}
                    onSubmit={(newValue) => updateGasExportPipelineLength(transportData.id, newValue)}
                />
            </Grid>
            <Grid size={12}>
                <Typography variant="h4">Substructure</Typography>
            </Grid>
            <Grid size={{ xs: 12, md: 4 }}>
                <SwitchableNumberInput
                    label="Substructure dry weight"
                    value={roundToDecimals(Number(substructureData.dryWeight), 0)}
                    integer
                    unit="tonnes"
                    min={0}
                    max={1_000_000}
                    id={`substructure-dry-weight-${substructureData.id}`}
                    onSubmit={(newValue) => updateSubstructureDryWeight(substructureData.id, newValue)}
                />
            </Grid>
            <PROSPBar
                projectId={projectId}
                caseId={caseData.caseId}
                currentSharePointFileId={caseData.sharepointFileId || null}
            />
        </TabContainer>
    )
}

export default CaseFacilitiesTab

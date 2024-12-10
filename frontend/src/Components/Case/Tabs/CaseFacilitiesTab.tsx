import { Typography } from "@equinor/eds-core-react"
import Grid from "@mui/material/Grid"
import { useParams } from "react-router"
import { useQuery } from "@tanstack/react-query"
import { useModuleCurrentContext } from "@equinor/fusion-framework-react-module-context"
import SwitchableNumberInput from "../../Input/SwitchableNumberInput"
import SwitchableDropdownInput from "../../Input/SwitchableDropdownInput"
import CaseFasilitiesTabSkeleton from "../../LoadingSkeletons/CaseFacilitiesTabSkeleton"
import SwitchableStringInput from "../../Input/SwitchableStringInput"
import { caseQueryFn, projectQueryFn } from "../../../Services/QueryFunctions"
import { useProjectContext } from "../../../Context/ProjectContext"

const CaseFacilitiesTab = ({ addEdit }: { addEdit: any }) => {
    const { currentContext } = useModuleCurrentContext()
    const externalId = currentContext?.externalId
    const { caseId, revisionId } = useParams()
    const { projectId, isRevision } = useProjectContext()

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
    const { data: projectData } = useQuery({
        queryKey: ["projectApiData", externalId],
        queryFn: () => projectQueryFn(externalId),
        enabled: !!externalId,
    })

    const { data: apiData } = useQuery({
        queryKey: ["caseApiData", isRevision ? revisionId : projectId, caseId],
        queryFn: () => caseQueryFn(isRevision ? revisionId ?? "" : projectId, caseId),
        enabled: !!projectId && !!caseId,
    })

    const caseData = apiData?.case
    const topsideData = apiData?.topside
    const surfData = apiData?.surf
    const transportData = apiData?.transport
    const substructureData = apiData?.substructure

    if (
        !caseData
        || !projectData
        || !topsideData
        || !surfData
        || !transportData
        || !substructureData
    ) {
        return (<CaseFasilitiesTabSkeleton />)
    }

    return (
        <Grid container spacing={2}>
            <Grid item xs={12} md={4}>
                <SwitchableDropdownInput
                    addEdit={addEdit}
                    resourceName="substructure"
                    resourcePropertyKey="concept"
                    resourceId={substructureData.id}
                    previousResourceObject={substructureData}
                    value={substructureData.concept}
                    options={platformConceptValues}
                    label="Platform concept"
                />
            </Grid>
            {substructureData.concept === 1 && (
                <Grid item xs={12} md={4}>
                    <SwitchableStringInput
                        addEdit={addEdit}
                        label="Host"
                        resourceName="case"
                        resourcePropertyKey="host"
                        previousResourceObject={caseData}
                        value={caseData.host || ""}
                    />
                </Grid>
            )}
            <Grid item xs={12} md={4}>

                <SwitchableNumberInput
                    addEdit={addEdit}
                    resourceName="topside"
                    resourcePropertyKey="facilityOpex"
                    resourceId={topsideData.id}
                    previousResourceObject={topsideData}
                    label="Facility opex"
                    value={Math.round(Number(topsideData.facilityOpex) * 10) / 10}
                    integer={false}
                    unit={`${projectData.commonProjectAndRevisionData.currency === 1 ? "MNOK" : "MUSD"}`}
                />

            </Grid>
            <Grid item xs={12} md={4}>
                <SwitchableNumberInput
                    addEdit={addEdit}
                    resourcePropertyKey="cessationCost"
                    resourceName="surf"
                    resourceId={surfData.id}
                    previousResourceObject={surfData}
                    label="Cessation cost"
                    value={Math.round(Number(surfData?.cessationCost) * 10) / 10}
                    integer={false}
                    unit={`${projectData.commonProjectAndRevisionData.currency === 1 ? "MNOK" : "MUSD"}`}
                />
            </Grid>
            <Grid item xs={12}>
                <Typography variant="h4">Topside</Typography>
            </Grid>
            <Grid item xs={12} md={4}>
                <SwitchableNumberInput
                    addEdit={addEdit}
                    resourceName="topside"
                    resourcePropertyKey="dryWeight"
                    resourceId={topsideData.id}
                    previousResourceObject={topsideData}
                    label="Topside dry weight"
                    value={Math.round(Number(topsideData.dryWeight) * 1) / 1}
                    integer
                    unit="tonnes"
                    min={0}
                    max={1000000}
                />
            </Grid>
            <Grid item xs={12} md={4}>
                <SwitchableNumberInput
                    addEdit={addEdit}
                    resourceName="case"
                    resourcePropertyKey="facilitiesAvailability"
                    previousResourceObject={caseData}
                    label="Facilities availability"
                    value={caseData.facilitiesAvailability ?? 0 * 100}
                    integer
                    disabled
                    unit="%"
                    min={0}
                    max={100}
                />
            </Grid>
            <Grid item xs={12} md={4}>
                <SwitchableNumberInput
                    addEdit={addEdit}
                    resourceName="topside"
                    resourcePropertyKey="peakElectricityImported"
                    resourceId={topsideData.id}
                    previousResourceObject={topsideData}
                    label="Peak electricity imported"
                    value={Math.round(Number(topsideData.peakElectricityImported) * 10) / 10}
                    integer={false}
                    unit="MW"
                    min={0}
                    max={1000000}
                />
            </Grid>
            <Grid item xs={12} md={4}>

                <SwitchableNumberInput
                    addEdit={addEdit}
                    resourceName="topside"
                    resourcePropertyKey="oilCapacity"
                    resourceId={topsideData.id}
                    previousResourceObject={topsideData}
                    label="Oil capacity"
                    value={Math.round(Number(topsideData.oilCapacity) * 1) / 1}
                    integer
                    unit="Sm³/sd"
                    min={0}
                    max={1000000}
                />
            </Grid>
            <Grid item xs={12} md={4}>
                <SwitchableNumberInput
                    addEdit={addEdit}
                    resourceName="topside"
                    resourcePropertyKey="gasCapacity"
                    resourceId={topsideData.id}
                    previousResourceObject={topsideData}
                    label="Gas capacity"
                    value={Math.round(Number(topsideData.gasCapacity) * 10) / 10}
                    integer={false}
                    unit="MSm³/sd"
                    min={0}
                    max={1000000}
                />
            </Grid>
            <Grid item xs={12} md={4}>
                <SwitchableNumberInput
                    addEdit={addEdit}
                    resourceName="topside"
                    resourcePropertyKey="waterInjectionCapacity"
                    resourceId={topsideData.id}
                    previousResourceObject={topsideData}
                    label="Water injection capacity"
                    value={Math.round(Number(topsideData.waterInjectionCapacity) * 1) / 1}
                    integer
                    unit="MSm³/sd"
                />
            </Grid>
            <Grid item xs={12}>
                <Typography variant="h4">Platform wells</Typography>
            </Grid>
            <Grid item xs={12} md={4}>
                <SwitchableNumberInput
                    addEdit={addEdit}
                    resourceName="topside"
                    resourcePropertyKey="producerCount"
                    resourceId={topsideData.id}
                    previousResourceObject={topsideData}
                    label="Producer count"
                    value={topsideData.producerCount}
                    integer
                    min={0}
                    max={1000000}
                />
            </Grid>
            <Grid item xs={12} md={4}>
                <SwitchableNumberInput
                    addEdit={addEdit}
                    resourceName="topside"
                    resourcePropertyKey="gasInjectorCount"
                    resourceId={topsideData.id}
                    previousResourceObject={topsideData}
                    label="Gas injector count"
                    value={topsideData.gasInjectorCount}
                    integer
                    min={0}
                    max={1000000}
                />
            </Grid>
            <Grid item xs={12} md={4}>
                <SwitchableNumberInput
                    addEdit={addEdit}
                    resourceName="topside"
                    resourcePropertyKey="waterInjectorCount"
                    resourceId={topsideData.id}
                    previousResourceObject={topsideData}
                    label="Water injector count"
                    value={topsideData.waterInjectorCount}
                    integer
                    min={0}
                    max={1000000}
                />
            </Grid>
            <Grid item xs={12}>
                <Typography variant="h4">SURF</Typography>
            </Grid>
            <Grid item xs={12} md={4}>
                <SwitchableNumberInput
                    addEdit={addEdit}
                    resourceName="surf"
                    resourcePropertyKey="templateCount"
                    resourceId={surfData.id}
                    previousResourceObject={surfData}
                    label="Templates"
                    value={surfData.templateCount}
                    integer
                    min={0}
                    max={1000000}
                />
            </Grid>
            <Grid item xs={12} md={4}>
                <SwitchableNumberInput
                    addEdit={addEdit}
                    resourceName="surf"
                    resourcePropertyKey="riserCount"
                    resourceId={surfData.id}
                    previousResourceObject={surfData}
                    label="Risers"
                    value={surfData.riserCount}
                    integer
                    min={0}
                    max={1000000}
                />
            </Grid>
            <Grid item xs={12} md={4}>
                <SwitchableNumberInput
                    addEdit={addEdit}
                    resourceName="surf"
                    resourcePropertyKey="infieldPipelineSystemLength"
                    resourceId={surfData.id}
                    previousResourceObject={surfData}
                    label="Production lines length"
                    value={Math.round(Number(surfData.infieldPipelineSystemLength) * 10) / 10}
                    integer={false}
                    unit="km"
                    min={0}
                    max={1000000}
                />
            </Grid>
            <Grid item xs={12} md={4}>
                <SwitchableNumberInput
                    addEdit={addEdit}
                    resourceName="surf"
                    resourcePropertyKey="umbilicalSystemLength"
                    resourceId={surfData.id}
                    previousResourceObject={surfData}
                    label="Umbilical system length"
                    value={Math.round(Number(surfData.umbilicalSystemLength) * 10) / 10}
                    integer={false}
                    unit="km"
                    min={0}
                    max={1000000}
                />
            </Grid>
            <Grid item xs={12} md={4}>
                <SwitchableDropdownInput
                    addEdit={addEdit}
                    resourceName="surf"
                    resourcePropertyKey="productionFlowline"
                    resourceId={surfData.id}
                    previousResourceObject={surfData}
                    value={surfData.productionFlowline}
                    options={productionFlowlineValues}
                    label="Production flowline"
                />
            </Grid>
            <Grid item xs={12}>
                <Typography variant="h4">Subsea wells</Typography>
            </Grid>
            <Grid item xs={12} md={4}>
                <SwitchableNumberInput
                    addEdit={addEdit}
                    resourceName="surf"
                    resourcePropertyKey="producerCount"
                    resourceId={surfData.id}
                    previousResourceObject={surfData}
                    label="Producer count"
                    value={surfData.producerCount}
                    integer
                />
            </Grid>
            <Grid item xs={12} md={4}>
                <SwitchableNumberInput
                    addEdit={addEdit}
                    resourceName="surf"
                    resourcePropertyKey="gasInjectorCount"
                    resourceId={surfData.id}
                    previousResourceObject={surfData}
                    label="Gas injector count"
                    value={surfData.gasInjectorCount}
                    integer
                    min={0}
                    max={1000000}
                />
            </Grid>
            <Grid item xs={12} md={4}>
                <SwitchableNumberInput
                    addEdit={addEdit}
                    resourceName="surf"
                    resourcePropertyKey="waterInjectorCount"
                    resourceId={surfData.id}
                    previousResourceObject={surfData}
                    label="Water injector count"
                    value={surfData.waterInjectorCount}
                    integer
                    min={0}
                    max={1000000}
                />
            </Grid>
            <Grid item xs={12}>
                <Typography variant="h4">Transport</Typography>
            </Grid>
            <Grid item xs={12} md={4}>
                <SwitchableNumberInput
                    addEdit={addEdit}
                    resourceName="transport"
                    resourcePropertyKey="oilExportPipelineLength"
                    resourceId={transportData.id}
                    previousResourceObject={transportData}
                    label="Oil export pipeline length"
                    value={Math.round(Number(transportData.oilExportPipelineLength) * 10) / 10}
                    integer={false}
                    unit="km"
                    min={0}
                    max={1000000}
                />
            </Grid>
            <Grid item xs={12} md={4}>
                <SwitchableNumberInput
                    addEdit={addEdit}
                    resourceName="transport"
                    resourcePropertyKey="gasExportPipelineLength"
                    resourceId={transportData.id}
                    previousResourceObject={transportData}
                    label="Gas export pipeline length"
                    value={Math.round(Number(transportData.gasExportPipelineLength) * 10) / 10}
                    integer={false}
                    unit="km"
                    min={0}
                    max={1000000}
                />
            </Grid>
            <Grid item xs={12}>
                <Typography variant="h4">Substructure</Typography>
            </Grid>
            <Grid item xs={12} md={4}>
                <SwitchableNumberInput
                    addEdit={addEdit}
                    resourceName="substructure"
                    resourcePropertyKey="dryWeight"
                    resourceId={substructureData.id}
                    previousResourceObject={substructureData}
                    label="Substructure dry weight"
                    value={Math.round(Number(substructureData.dryWeight) * 1) / 1}
                    integer
                    unit="tonnes"
                    min={0}
                    max={1000000}
                />
            </Grid>
        </Grid>
    )
}

export default CaseFacilitiesTab

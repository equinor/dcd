import { Typography } from "@equinor/eds-core-react"
import Grid from "@mui/material/Grid"
import { useParams } from "react-router"
import { useQueryClient, useQuery } from "react-query"
import SwitchableNumberInput from "../../Input/SwitchableNumberInput"
import { useProjectContext } from "../../../Context/ProjectContext"
import SwitchableDropdownInput from "../../Input/SwitchableDropdownInput"
import CaseFasilitiesTabSkeleton from "./LoadingSkeletons/CaseFacilitiesTabSkeleton"

const CaseFacilitiesTab = () => {
    const queryClient = useQueryClient()
    const { project } = useProjectContext()
    const { caseId } = useParams()
    const projectId = project?.id || null

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

    const { data: apiData } = useQuery<Components.Schemas.CaseWithAssetsDto | undefined>(
        ["apiData", { projectId, caseId }],
        () => queryClient.getQueryData(["apiData", { projectId, caseId }]),
        {
            enabled: !!projectId && !!caseId,
            initialData: () => queryClient.getQueryData(["apiData", { projectId, caseId }]),
        },
    )

    const { data: caseData } = useQuery<Components.Schemas.CaseDto | undefined>(
        [{ projectId, caseId, resourceId: "" }],
        () => queryClient.getQueryData([{ projectId, caseId, resourceId: "" }]),
        {
            enabled: !!project && !!projectId,
            initialData: () => queryClient.getQueryData([{ projectId: project?.id, caseId, resourceId: "" }]) as Components.Schemas.CaseDto,
        },
    )
    const topsideData = apiData?.topside
    const surfData = apiData?.surf
    const transportData = apiData?.transport
    const substructureData = apiData?.substructure

    if (!caseData || !topsideData || !surfData || !transportData || !substructureData) {
        return (<CaseFasilitiesTabSkeleton />)
    }

    return (
        <Grid container spacing={2}>
            <Grid item xs={12} md={4}>
                <SwitchableDropdownInput
                    resourceName="substructure"
                    resourcePropertyKey="concept"
                    resourceId={substructureData.id}
                    value={substructureData.concept}
                    options={platformConceptValues}
                    label="Platform concept"
                />
            </Grid>
            {substructureData.concept === 1 && (
                <Grid item xs={12} md={4}>
                    <SwitchableNumberInput
                        resourceName="case"
                        resourcePropertyKey="host"
                        label="Host"
                        value={Number(caseData.host)}
                        integer
                    />
                </Grid>
            )}
            <Grid item xs={12} md={4}>

                <SwitchableNumberInput
                    resourceName="topside"
                    resourcePropertyKey="facilityOpex"
                    resourceId={topsideData.id}
                    label="Facility opex"
                    value={Math.round(Number(topsideData.facilityOpex) * 10) / 10}
                    integer={false}
                    unit={`${project?.currency === 1 ? "MNOK" : "MUSD"}`}
                />

            </Grid>
            <Grid item xs={12} md={4}>
                <SwitchableNumberInput
                    resourcePropertyKey="cessationCost"
                    resourceName="surf"
                    resourceId={surfData.id}
                    label="Cessation cost"
                    value={Math.round(Number(surfData?.cessationCost) * 10) / 10}
                    integer={false}
                    unit={`${project?.currency === 1 ? "MNOK" : "MUSD"}`}
                />
            </Grid>
            <Grid item xs={12}>
                <Typography variant="h4">Topside</Typography>
            </Grid>
            <Grid item xs={12} md={4}>
                <SwitchableNumberInput
                    resourceName="topside"
                    resourcePropertyKey="dryWeight"
                    resourceId={topsideData.id}
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
                    resourceName="case"
                    resourcePropertyKey="facilitiesAvailability"
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
                    resourceName="topside"
                    resourcePropertyKey="peakElectricityImported"
                    resourceId={topsideData.id}
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
                    resourceName="topside"
                    resourcePropertyKey="oilCapacity"
                    resourceId={topsideData.id}
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
                    resourceName="topside"
                    resourcePropertyKey="gasCapacity"
                    resourceId={topsideData.id}
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
                    resourceName="topside"
                    resourcePropertyKey="waterInjectionCapacity"
                    resourceId={topsideData.id}
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
                    resourceName="topside"
                    resourcePropertyKey="producerCount"
                    resourceId={topsideData.id}
                    label="Producer count"
                    value={topsideData.producerCount}
                    integer
                    min={0}
                    max={1000000}
                />
            </Grid>
            <Grid item xs={12} md={4}>
                <SwitchableNumberInput
                    resourceName="topside"
                    resourcePropertyKey="gasInjectorCount"
                    resourceId={topsideData.id}
                    label="Gas injector count"
                    value={topsideData.gasInjectorCount}
                    integer
                    min={0}
                    max={1000000}
                />
            </Grid>
            <Grid item xs={12} md={4}>
                <SwitchableNumberInput
                    resourceName="topside"
                    resourcePropertyKey="waterInjectorCount"
                    resourceId={topsideData.id}
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
                    resourceName="surf"
                    resourcePropertyKey="templateCount"
                    resourceId={surfData.id}
                    label="Templates"
                    value={surfData.templateCount}
                    integer
                    min={0}
                    max={1000000}
                />
            </Grid>
            <Grid item xs={12} md={4}>
                <SwitchableNumberInput
                    resourceName="surf"
                    resourcePropertyKey="riserCount"
                    resourceId={surfData.id}
                    label="Risers"
                    value={surfData.riserCount}
                    integer
                    min={0}
                    max={1000000}
                />
            </Grid>
            <Grid item xs={12} md={4}>
                <SwitchableNumberInput
                    resourceName="surf"
                    resourcePropertyKey="infieldPipelineSystemLength"
                    resourceId={surfData.id}
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
                    resourceName="surf"
                    resourcePropertyKey="umbilicalSystemLength"
                    resourceId={surfData.id}
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
                    resourceName="surf"
                    resourcePropertyKey="productionFlowline"
                    resourceId={surfData.id}
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
                    resourceName="surf"
                    resourcePropertyKey="producerCount"
                    resourceId={surfData.id}
                    label="Producer count"
                    value={surfData.producerCount}
                    integer
                />
            </Grid>
            <Grid item xs={12} md={4}>
                <SwitchableNumberInput
                    resourceName="surf"
                    resourcePropertyKey="gasInjectorCount"
                    resourceId={surfData.id}
                    label="Gas injector count"
                    value={surfData.gasInjectorCount}
                    integer
                    min={0}
                    max={1000000}
                />
            </Grid>
            <Grid item xs={12} md={4}>
                <SwitchableNumberInput
                    resourceName="surf"
                    resourcePropertyKey="waterInjectorCount"
                    resourceId={surfData.id}
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
                    resourceName="transport"
                    resourcePropertyKey="oilExportPipelineLength"
                    resourceId={transportData.id}
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
                    resourceName="transport"
                    resourcePropertyKey="gasExportPipelineLength"
                    resourceId={transportData.id}
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
                    resourceName="substructure"
                    resourcePropertyKey="dryWeight"
                    resourceId={substructureData.id}
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

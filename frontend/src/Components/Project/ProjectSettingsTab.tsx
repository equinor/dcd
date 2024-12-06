import { useState, ChangeEventHandler, useEffect } from "react"
import { useParams } from "react-router"
import { Input, NativeSelect } from "@equinor/eds-core-react"
import Grid from "@mui/material/Grid"
import { useQuery } from "@tanstack/react-query"
import { useModuleCurrentContext } from "@equinor/fusion-framework-react-module-context"

import InputSwitcher from "../Input/Components/InputSwitcher"
import { PROJECT_CLASSIFICATION } from "@/Utils/constants"
import { projectQueryFn, revisionQueryFn } from "@/Services/QueryFunctions"
import useEditProject from "@/Hooks/useEditProject"
import { useProjectContext } from "@/Context/ProjectContext"

const ProjectSettingsTab = () => {
    const { currentContext } = useModuleCurrentContext()
    const { addProjectEdit } = useEditProject()
    const { revisionId } = useParams()
    const { isRevision, projectId } = useProjectContext()
    const externalId = currentContext?.externalId

    const { data: apiData } = useQuery({
        queryKey: ["projectApiData", externalId],
        queryFn: () => projectQueryFn(externalId),
        enabled: !!externalId,
    })

    const { data: apiRevisionData } = useQuery({
        queryKey: ["revisionApiData", revisionId],
        queryFn: () => revisionQueryFn(projectId, revisionId),
        enabled: !!projectId && !!revisionId && isRevision,
    })

    const [dummyRole, setDummyRole] = useState(0) // TODO: Get role from user
    const [oilPriceUSD, setOilPriceUSD] = useState(apiData?.commonProjectAndRevisionData.oilPriceUSD || 0)
    const [gasPriceNOK, setGasPriceNOK] = useState(apiData?.commonProjectAndRevisionData.gasPriceNOK || 0)
    const [discountRate, setDiscountRate] = useState(apiData?.commonProjectAndRevisionData.discountRate || 0)

    useEffect(() => {
        if (apiData && !isRevision) {
            setOilPriceUSD(apiData.commonProjectAndRevisionData.oilPriceUSD)
            setGasPriceNOK(apiData.commonProjectAndRevisionData.gasPriceNOK)
            setDiscountRate(apiData.commonProjectAndRevisionData.discountRate)
        }
    }, [apiData, isRevision])

    useEffect(() => {
        if (apiRevisionData && isRevision) {
            setOilPriceUSD(apiRevisionData.commonProjectAndRevisionData.oilPriceUSD)
            setGasPriceNOK(apiRevisionData.commonProjectAndRevisionData.gasPriceNOK)
            setDiscountRate(apiRevisionData.commonProjectAndRevisionData.discountRate)
        }
    }, [isRevision, apiRevisionData, revisionId])

    const handlePhysicalUnitChange: ChangeEventHandler<HTMLSelectElement> = async (e) => {
        if ([0, 1].indexOf(Number(e.currentTarget.value)) !== -1 && apiData) {
            const newPhysicalUnit: Components.Schemas.PhysUnit = Number(e.currentTarget.value) as Components.Schemas.PhysUnit
            const newProject: Components.Schemas.UpdateProjectDto = { ...apiData.commonProjectAndRevisionData }
            newProject.physicalUnit = newPhysicalUnit
            addProjectEdit(apiData.projectId, newProject)
        }
    }

    const handleCurrencyChange: ChangeEventHandler<HTMLSelectElement> = async (e) => {
        if ([1, 2].indexOf(Number(e.currentTarget.value)) !== -1 && apiData) {
            const newCurrency: Components.Schemas.Currency = Number(e.currentTarget.value) as Components.Schemas.Currency
            const newProject: Components.Schemas.UpdateProjectDto = { ...apiData.commonProjectAndRevisionData }
            newProject.currency = newCurrency
            addProjectEdit(apiData.projectId, newProject)
        }
    }

    const handleClassificationChange: ChangeEventHandler<HTMLSelectElement> = async (e) => {
        if ([0, 1, 2, 3].indexOf(Number(e.currentTarget.value)) !== -1 && apiData) {
            const newClassification: Components.Schemas.ProjectClassification = Number(e.currentTarget.value) as unknown as Components.Schemas.ProjectClassification
            const newProject: Components.Schemas.UpdateProjectDto = { ...apiData.commonProjectAndRevisionData }
            newProject.classification = newClassification
            addProjectEdit(apiData.projectId, newProject)
        }
    }

    const handleOilPriceChange = () => {
        const newOilPrice = oilPriceUSD
        if (!Number.isNaN(newOilPrice) && apiData) {
            const newProject: Components.Schemas.UpdateProjectDto = { ...apiData.commonProjectAndRevisionData, oilPriceUSD: newOilPrice }
            addProjectEdit(apiData.projectId, newProject)
        }
    }

    const handleGasPriceChange = () => {
        const newGasPrice = gasPriceNOK
        if (!Number.isNaN(newGasPrice) && apiData) {
            const newProject: Components.Schemas.UpdateProjectDto = { ...apiData.commonProjectAndRevisionData, gasPriceNOK: newGasPrice }
            addProjectEdit(apiData.projectId, newProject)
        }
    }

    const handleDiscountRateChange = () => {
        const newDiscountRate = discountRate
        if (!Number.isNaN(newDiscountRate) && apiData) {
            const newProject: Components.Schemas.UpdateProjectDto = { ...apiData.commonProjectAndRevisionData, discountRate: newDiscountRate }
            addProjectEdit(apiData.projectId, newProject)
        }
    }

    if (!apiData) {
        return <div>Loading project data...</div>
    }

    return (
        <Grid container direction="column" spacing={2}>
            <Grid item>
                <InputSwitcher
                    value={isRevision ? apiRevisionData?.commonProjectAndRevisionData.physicalUnit === 0 ? "SI" : "Oil field" : apiData.commonProjectAndRevisionData.physicalUnit === 0 ? "SI" : "Oil field"}
                    label="Physical unit"
                >
                    <NativeSelect
                        id="physicalUnit"
                        label=""
                        onChange={handlePhysicalUnitChange}
                        value={apiData.commonProjectAndRevisionData.physicalUnit}
                    >
                        <option key={0} value={0}>SI</option>
                        <option key={1} value={1}>Oil field</option>
                    </NativeSelect>
                </InputSwitcher>
            </Grid>
            <Grid item>
                <InputSwitcher
                    value={isRevision ? apiRevisionData?.commonProjectAndRevisionData.currency === 1 ? "NOK" : "USD" : apiData.commonProjectAndRevisionData.currency === 1 ? "NOK" : "USD"}
                    label="Currency"
                >
                    <NativeSelect
                        id="currency"
                        label=""
                        onChange={handleCurrencyChange}
                        value={apiData.commonProjectAndRevisionData.currency}
                    >
                        <option key={1} value={1}>NOK</option>
                        <option key={2} value={2}>USD</option>
                    </NativeSelect>
                </InputSwitcher>
            </Grid>
            <Grid item>
                {dummyRole === 0 && (
                    <InputSwitcher
                        value={isRevision
                        && !!apiRevisionData?.commonProjectAndRevisionData.classification
                            ? PROJECT_CLASSIFICATION[apiRevisionData?.commonProjectAndRevisionData.classification].label
                            : PROJECT_CLASSIFICATION[apiData.commonProjectAndRevisionData.classification].label}
                        label="Classification"
                    >
                        <NativeSelect
                            id="classification"
                            label=""
                            onChange={(e) => handleClassificationChange(e)}
                            value={apiData ? apiData.commonProjectAndRevisionData.classification : undefined}
                        >
                            {Object.entries(PROJECT_CLASSIFICATION).map(([key, value]) => (
                                <option key={key} value={key}>{value.label}</option>
                            ))}
                        </NativeSelect>
                    </InputSwitcher>
                )}
            </Grid>
            <Grid item>
                <InputSwitcher
                    value={String(oilPriceUSD)}
                    label="Oil Price (USD)"
                >
                    <Input
                        type="number"
                        step="0.01"
                        value={oilPriceUSD}
                        onChange={(e: any) => setOilPriceUSD(Number(e.target.value))}
                        onBlur={handleOilPriceChange}
                    />
                </InputSwitcher>
            </Grid>
            <Grid item>
                <InputSwitcher
                    value={String(gasPriceNOK)}
                    label="Gas Price (NOK)"
                >
                    <Input
                        type="number"
                        step="0.01"
                        value={gasPriceNOK}
                        onChange={(e: any) => setGasPriceNOK(Number(e.target.value))}
                        onBlur={handleGasPriceChange}
                    />
                </InputSwitcher>
            </Grid>
            <Grid item>
                <InputSwitcher
                    value={String(discountRate)}
                    label="Discount Rate (%)"
                >
                    <Input
                        type="number"
                        step="0.01"
                        value={discountRate}
                        onChange={(e: any) => setDiscountRate(Number(e.target.value))}
                        onBlur={handleDiscountRateChange}
                    />
                </InputSwitcher>
            </Grid>
        </Grid>
    )
}

export default ProjectSettingsTab

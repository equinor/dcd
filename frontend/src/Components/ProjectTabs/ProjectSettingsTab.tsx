import {
    Icon, Input, NativeSelect, Typography,
} from "@equinor/eds-core-react"
import { help_outline } from "@equinor/eds-icons"
import Grid from "@mui/material/Grid2"
import { useState, ChangeEventHandler, useEffect } from "react"
import { styled } from "styled-components"

import ProjectSkeleton from "../LoadingSkeletons/ProjectSkeleton"

import InputSwitcher from "@/Components/Input/Components/InputSwitcher"
import { useDataFetch } from "@/Hooks"
import useEditProject from "@/Hooks/useEditProject"
import { Currency, PhysUnit } from "@/Models/enums"
import { PROJECT_CLASSIFICATION } from "@/Utils/Config/constants"

export const PhysicalUnitExplanation = styled.div`
    display: flex;
    align-items: center;
    width: 100%;
    gap: 10px;
    margin-top: 15px;
    margin-bottom: 15px;
`

const ProjectSettingsTab = () => {
    const { addProjectEdit } = useEditProject()
    const revisionAndProjectData = useDataFetch()

    const [dummyRole] = useState(0) // TODO: Get role from user
    const [oilPriceUsd, setOilPriceUsd] = useState(revisionAndProjectData?.commonProjectAndRevisionData.oilPriceUsd || 0)
    const [nglpriceUsd, setNglPriceUsd] = useState(revisionAndProjectData?.commonProjectAndRevisionData.nglPriceUsd || 0)
    const [gasPriceNok, setGasPriceNok] = useState(revisionAndProjectData?.commonProjectAndRevisionData.gasPriceNok || 0)
    const [discountRate, setDiscountRate] = useState(revisionAndProjectData?.commonProjectAndRevisionData.discountRate || 0)
    const [exchangeRateUsdToNok, setExchangeRateUsdToNok] = useState(revisionAndProjectData?.commonProjectAndRevisionData.exchangeRateUsdToNok || 0)
    const [npvYear, setNpvYear] = useState(revisionAndProjectData?.commonProjectAndRevisionData.npvYear || 0)

    useEffect(() => {
        if (revisionAndProjectData) {
            setOilPriceUsd(revisionAndProjectData.commonProjectAndRevisionData.oilPriceUsd)
            setNglPriceUsd(revisionAndProjectData.commonProjectAndRevisionData.nglPriceUsd)
            setGasPriceNok(revisionAndProjectData.commonProjectAndRevisionData.gasPriceNok)
            setDiscountRate(revisionAndProjectData.commonProjectAndRevisionData.discountRate)
            setExchangeRateUsdToNok(revisionAndProjectData.commonProjectAndRevisionData.exchangeRateUsdToNok)
            setNpvYear(revisionAndProjectData.commonProjectAndRevisionData.npvYear)
        }
    }, [revisionAndProjectData])

    const handlePhysicalUnitChange: ChangeEventHandler<HTMLSelectElement> = async (e) => {
        if ([0, 1].indexOf(Number(e.currentTarget.value)) !== -1 && revisionAndProjectData) {
            const newPhysicalUnit: Components.Schemas.PhysUnit = Number(e.currentTarget.value) as Components.Schemas.PhysUnit
            const newProject: Components.Schemas.UpdateProjectDto = { ...revisionAndProjectData.commonProjectAndRevisionData }

            newProject.physicalUnit = newPhysicalUnit
            addProjectEdit(revisionAndProjectData.projectId, newProject)
        }
    }

    const handleCurrencyChange: ChangeEventHandler<HTMLSelectElement> = async (e) => {
        if ([1, 2].indexOf(Number(e.currentTarget.value)) !== -1 && revisionAndProjectData) {
            const newCurrency: Components.Schemas.Currency = Number(e.currentTarget.value) as Components.Schemas.Currency
            const newProject: Components.Schemas.UpdateProjectDto = { ...revisionAndProjectData.commonProjectAndRevisionData }

            newProject.currency = newCurrency
            addProjectEdit(revisionAndProjectData.projectId, newProject)
        }
    }

    const handleClassificationChange: ChangeEventHandler<HTMLSelectElement> = async (e) => {
        if ([1, 2, 3].indexOf(Number(e.currentTarget.value)) !== -1 && revisionAndProjectData) {
            const newClassification: Components.Schemas.ProjectClassification = Number(e.currentTarget.value) as unknown as Components.Schemas.ProjectClassification
            const newProject: Components.Schemas.UpdateProjectDto = { ...revisionAndProjectData.commonProjectAndRevisionData }

            newProject.classification = newClassification
            addProjectEdit(revisionAndProjectData.projectId, newProject)
        }
    }

    const handleOilPriceChange = () => {
        const newOilPrice = oilPriceUsd

        if (!Number.isNaN(newOilPrice) && revisionAndProjectData) {
            const newProject: Components.Schemas.UpdateProjectDto = { ...revisionAndProjectData.commonProjectAndRevisionData, oilPriceUsd: newOilPrice }

            addProjectEdit(revisionAndProjectData.projectId, newProject)
        }
    }

    const handleNglPriceChange = () => {
        const newNglPrice = nglpriceUsd

        if (!Number.isNaN(newNglPrice) && revisionAndProjectData) {
            const newProject: Components.Schemas.UpdateProjectDto = { ...revisionAndProjectData.commonProjectAndRevisionData, nglPriceUsd: newNglPrice }

            addProjectEdit(revisionAndProjectData.projectId, newProject)
        }
    }

    const handleGasPriceChange = () => {
        const newGasPrice = gasPriceNok

        if (!Number.isNaN(newGasPrice) && revisionAndProjectData) {
            const newProject: Components.Schemas.UpdateProjectDto = { ...revisionAndProjectData.commonProjectAndRevisionData, gasPriceNok: newGasPrice }

            addProjectEdit(revisionAndProjectData.projectId, newProject)
        }
    }

    const handleDiscountRateChange = () => {
        const newDiscountRate = discountRate

        if (!Number.isNaN(newDiscountRate) && revisionAndProjectData) {
            const newProject: Components.Schemas.UpdateProjectDto = { ...revisionAndProjectData.commonProjectAndRevisionData, discountRate: newDiscountRate }

            addProjectEdit(revisionAndProjectData.projectId, newProject)
        }
    }

    const handleExchangeRateChange = () => {
        const newExchangeRate = exchangeRateUsdToNok

        if (!Number.isNaN(newExchangeRate) && revisionAndProjectData) {
            const newProject: Components.Schemas.UpdateProjectDto = { ...revisionAndProjectData.commonProjectAndRevisionData, exchangeRateUsdToNok: newExchangeRate }

            addProjectEdit(revisionAndProjectData.projectId, newProject)
        }
    }

    const handleNpvYearChange = () => {
        const newNpvYear = npvYear

        if (!Number.isNaN(newNpvYear) && revisionAndProjectData) {
            const newProject: Components.Schemas.UpdateProjectDto = { ...revisionAndProjectData.commonProjectAndRevisionData, npvYear: newNpvYear }

            addProjectEdit(revisionAndProjectData.projectId, newProject)
        }
    }

    const getPhysicalUnit = () => (revisionAndProjectData?.commonProjectAndRevisionData.physicalUnit === PhysUnit.Si ? "SI" : "Oil field")
    const getCurrency = () => (revisionAndProjectData?.commonProjectAndRevisionData.currency === Currency.Nok ? "NOK" : "USD")

    if (!revisionAndProjectData) {
        return <ProjectSkeleton />
    }

    return (
        <Grid container direction="column" spacing={2}>
            <Grid container size={12} justifyContent="flex-start">
                <Grid container size={{ xs: 12, md: 6, lg: 4 }} spacing={2}>
                    <Grid size={12}>
                        <InputSwitcher
                            value={getPhysicalUnit()}
                            label="Physical unit"
                        >
                            <NativeSelect
                                id="physicalUnit"
                                label=""
                                onChange={handlePhysicalUnitChange}
                                value={revisionAndProjectData.commonProjectAndRevisionData.physicalUnit}
                            >
                                <option key={0} value={0}>SI</option>
                                <option key={1} value={1}>Oil field</option>
                            </NativeSelect>
                        </InputSwitcher>

                        <PhysicalUnitExplanation>
                            <Icon data={help_outline} size={24} />
                            <Typography variant="caption">Switching physical unit will convert and display all applicable numbers in the selected unit</Typography>
                        </PhysicalUnitExplanation>
                    </Grid>
                    <Grid size={12}>
                        <InputSwitcher
                            value={getCurrency()}
                            label="Currency"
                        >
                            <NativeSelect
                                id="currency"
                                label=""
                                onChange={handleCurrencyChange}
                                value={revisionAndProjectData.commonProjectAndRevisionData.currency}
                            >
                                <option key={1} value={1}>NOK</option>
                                <option key={2} value={2}>USD</option>
                            </NativeSelect>
                        </InputSwitcher>
                    </Grid>
                    <Grid size={12}>
                        {dummyRole === 0 && (
                            <InputSwitcher
                                value={PROJECT_CLASSIFICATION[revisionAndProjectData.commonProjectAndRevisionData.classification].label}
                                label="Classification"
                            >
                                <NativeSelect
                                    id="classification"
                                    label=""
                                    onChange={(e) => handleClassificationChange(e)}
                                    value={revisionAndProjectData ? revisionAndProjectData.commonProjectAndRevisionData.classification : 1}
                                >
                                    {Object.entries(PROJECT_CLASSIFICATION).filter(([key]) => key !== "0").map(([key, value]) => (
                                        <option key={key} value={key}>{value.label}</option>
                                    ))}
                                </NativeSelect>
                            </InputSwitcher>
                        )}
                    </Grid>
                    <Grid size={12}>
                        <InputSwitcher
                            value={String(oilPriceUsd)}
                            label="Oil Price (USD/bbl)"
                        >
                            <Input
                                type="number"
                                step="0.01"
                                value={oilPriceUsd}
                                onChange={(e: any) => setOilPriceUsd(Number(e.target.value))}
                                onBlur={handleOilPriceChange}
                            />
                        </InputSwitcher>
                    </Grid>
                    <Grid size={12}>
                        <InputSwitcher
                            value={String(nglpriceUsd)}
                            label="NGL price (USD/tonn)"
                        >
                            <Input
                                type="number"
                                step="0.01"
                                value={nglpriceUsd}
                                onChange={(e: any) => setNglPriceUsd(Number(e.target.value))}
                                onBlur={handleNglPriceChange}
                            />
                        </InputSwitcher>
                    </Grid>
                    <Grid size={12}>
                        <InputSwitcher
                            value={String(gasPriceNok)}
                            label="Gas Price (NOK/Sm3)"
                        >
                            <Input
                                type="number"
                                step="0.01"
                                value={gasPriceNok}
                                onChange={(e: any) => setGasPriceNok(Number(e.target.value))}
                                onBlur={handleGasPriceChange}
                            />
                        </InputSwitcher>
                    </Grid>
                    <Grid size={12}>
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
                    <Grid size={12}>
                        <InputSwitcher
                            value={String(exchangeRateUsdToNok)}
                            label="Exchange Rate (USD to NOK)"
                        >
                            <Input
                                type="number"
                                step="0.01"
                                value={exchangeRateUsdToNok}
                                onChange={(e: any) => setExchangeRateUsdToNok(Number(e.target.value))}
                                onBlur={handleExchangeRateChange}
                            />
                        </InputSwitcher>
                    </Grid>
                    <Grid size={12}>
                        <InputSwitcher
                            value={String(npvYear)}
                            label="NPV Year"
                        >
                            <Input
                                type="number"
                                step="1"
                                value={npvYear}
                                onChange={(e: any) => setNpvYear(Number(e.target.value))}
                                onBlur={handleNpvYearChange}
                            />
                        </InputSwitcher>
                    </Grid>
                </Grid>
            </Grid>
        </Grid>
    )
}

export default ProjectSettingsTab

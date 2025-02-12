import {
    Table, TableBody, TableCell, TableContainer, TableRow, Typography, Chip, Accordion, AccordionSummary, AccordionDetails,
} from "@mui/material"
import styled from "styled-components"
import { grey } from "@mui/material/colors"
import { Icon } from "@equinor/eds-core-react"
import { chevron_down } from "@equinor/eds-icons"

const Container = styled.div`
    padding: 20px;
`

const Section = styled.div`
    margin-bottom: 32px;

    &:last-child {
        margin-bottom: 0;
    }
`

const SectionTitle = styled(Typography)`
    font-size: 14px;
    font-weight: 500;
    margin-bottom: 16px;
`

const StyledTableContainer = styled(TableContainer)`
    .MuiTableCell-root {
        border-bottom: 1px solid #E0E0E0;
        padding: 12px 0;
    }
`

const KeyCell = styled(TableCell)`
    color: ${grey[600]};
    font-weight: 500;
    width: 140px;
    vertical-align: top;
`

const ValueCell = styled(TableCell)`
    font-family: monospace;
    word-break: break-word;
`

const ChipsContainer = styled.div`
    display: flex;
    gap: 8px;
    flex-wrap: wrap;
    margin-bottom: 24px;
`

const JsonContainer = styled.pre`
    background: ${grey[100]};
    padding: 16px;
    border-radius: 4px;
    overflow-x: auto;
    font-size: 12px;
    margin: 0;
`

interface Props {
    rowData: any
}

const MetadataTab = ({ rowData }: Props) => {
    const profile = rowData.overrideProfile?.override ? rowData.overrideProfile : rowData.profile

    const getBasicInfo = () => [
        { key: "Profile Name", value: rowData.profileName },
        { key: "Resource Name", value: rowData.resourceName },
        { key: "Resource ID", value: rowData.resourceId },
        { key: "Resource Property Key", value: rowData.resourcePropertyKey },
        { key: "Unit", value: rowData.unit },
        { key: "Total", value: rowData.total },
    ]

    const getProfileInfo = () => {
        if (!profile) { return [] }

        return [
            { key: "Profile ID", value: profile.id },
            { key: "Start Year", value: profile.startYear },
            { key: "Values Length", value: profile.values?.length || 0 },
            { key: "Has Override", value: rowData.overrideProfile?.override ? "Yes" : "No" },
            { key: "Is Overridable", value: rowData.overridable ? "Yes" : "No" },
            { key: "Is Editable", value: rowData.editable ? "Yes" : "No" },
            { key: "Hide If Empty", value: rowData.hideIfEmpty ? "Yes" : "No" },
        ]
    }

    const getTimeSeriesStats = () => {
        if (!profile?.values?.length) { return [] }

        const { values } = profile
        const sum = values.reduce((a: number, b: number) => a + b, 0)
        const avg = sum / values.length
        const max = Math.max(...values)
        const min = Math.min(...values)
        const nonZeroValues = values.filter((v: number) => v !== 0)
        const zeroCount = values.length - nonZeroValues.length
        const firstNonZeroValue = nonZeroValues[0]
        const lastNonZeroValue = nonZeroValues[nonZeroValues.length - 1]
        const totalChange = firstNonZeroValue ? ((lastNonZeroValue - firstNonZeroValue) / firstNonZeroValue) * 100 : 0

        return [
            { key: "Sum", value: sum.toFixed(2) },
            { key: "Average", value: avg.toFixed(2) },
            { key: "Maximum", value: max.toFixed(2) },
            { key: "Minimum", value: min.toFixed(2) },
            { key: "Data Points", value: values.length },
            { key: "Non-zero Points", value: nonZeroValues.length },
            { key: "Zero/Missing Points", value: zeroCount },
            { key: "First Non-zero Value", value: firstNonZeroValue?.toFixed(2) || "N/A" },
            { key: "Last Non-zero Value", value: lastNonZeroValue?.toFixed(2) || "N/A" },
            { key: "Total Change", value: `${totalChange.toFixed(1)}%` },
        ]
    }

    const getYearlyData = (): Array<{ key: string, value: string }> => {
        if (!profile?.values) { return [] }

        return profile.values.map((value: number, index: number) => ({
            key: `Year ${profile.startYear + index}`,
            value: value.toFixed(2),
        }))
    }

    const getFlags = () => [
        { label: "Editable", active: rowData.editable },
        { label: "Overridable", active: rowData.overridable },
        { label: "Has Override", active: rowData.overrideProfile?.override },
        { label: "Hide If Empty", active: rowData.hideIfEmpty },
        { label: "Has Data", active: profile?.values?.length > 0 },
        { label: "Has Non-zero Values", active: profile?.values?.some((v: number) => v !== 0) },
    ]

    const getDataChanges = (): Array<{ key: string, value: string }> => {
        if (!profile?.values || profile.values.length < 2) { return [] }

        return profile.values.slice(1).map((value: number, index: number) => {
            const year = profile.startYear + index + 1
            const prevValue = profile.values[index]
            let change = ""
            if (prevValue === 0) {
                change = value === 0 ? "0%" : "New entry"
            } else {
                change = `${(((value - prevValue) / prevValue) * 100).toFixed(1)}%`
            }

            return {
                key: `${year} vs ${year - 1}`,
                value: `${value.toFixed(2)} vs ${prevValue.toFixed(2)} (${change})`,
            }
        })
    }

    const renderSection = (title: string, data: { key: string, value: any }[]) => (
        <Section>
            <SectionTitle variant="subtitle2">
                {title}
            </SectionTitle>
            <StyledTableContainer>
                <Table size="small">
                    <TableBody>
                        {data.map(({ key, value }) => (
                            <TableRow key={key}>
                                <KeyCell>{key}</KeyCell>
                                <ValueCell>
                                    <pre style={{ margin: 0 }}>
                                        {typeof value === "object" ? JSON.stringify(value, null, 2) : String(value)}
                                    </pre>
                                </ValueCell>
                            </TableRow>
                        ))}
                    </TableBody>
                </Table>
            </StyledTableContainer>
        </Section>
    )

    return (
        <Container>
            <ChipsContainer>
                {getFlags().map(({ label, active }) => (
                    <Chip
                        key={label}
                        label={label}
                        color={active ? "primary" : "default"}
                        variant={active ? "filled" : "outlined"}
                        size="small"
                    />
                ))}
            </ChipsContainer>

            {renderSection("Basic Information", getBasicInfo())}
            {renderSection("Profile Information", getProfileInfo())}
            {renderSection("Statistical Analysis", getTimeSeriesStats())}

            <Accordion>
                <AccordionSummary expandIcon={
                    <Icon data={chevron_down} />
                }
                >
                    <Typography>Year-over-Year Changes</Typography>
                </AccordionSummary>
                <AccordionDetails>
                    <StyledTableContainer>
                        <Table size="small">
                            <TableBody>
                                {getDataChanges().map(({ key, value }) => (
                                    <TableRow key={key}>
                                        <KeyCell>{key}</KeyCell>
                                        <ValueCell>{value}</ValueCell>
                                    </TableRow>
                                ))}
                            </TableBody>
                        </Table>
                    </StyledTableContainer>
                </AccordionDetails>
            </Accordion>

            <Accordion>
                <AccordionSummary expandIcon={
                    <Icon data={chevron_down} />
                }
                >
                    <Typography>Raw Yearly Data</Typography>
                </AccordionSummary>
                <AccordionDetails>
                    <StyledTableContainer>
                        <Table size="small">
                            <TableBody>
                                {getYearlyData().map(({ key, value }) => (
                                    <TableRow key={key}>
                                        <KeyCell>{key}</KeyCell>
                                        <ValueCell>{value}</ValueCell>
                                    </TableRow>
                                ))}
                            </TableBody>
                        </Table>
                    </StyledTableContainer>
                </AccordionDetails>
            </Accordion>

            <Accordion>
                <AccordionSummary expandIcon={
                    <Icon data={chevron_down} />
                }
                >
                    <Typography>Raw JSON Data</Typography>
                </AccordionSummary>
                <AccordionDetails>
                    <JsonContainer>
                        {JSON.stringify(rowData, null, 2)}
                    </JsonContainer>
                </AccordionDetails>
            </Accordion>
        </Container>
    )
}

export default MetadataTab

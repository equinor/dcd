import { Stack, Typography } from "@mui/material"
import styled from "styled-components"
import { grey } from "@mui/material/colors"
import { AgChartsTimeseries } from "@/Components/AgGrid/AgChartsTimeseries"

const Container = styled.div`
    padding: 24px 32px;
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

const ValueGrid = styled.div`
    display: grid;
    grid-template-columns: repeat(auto-fill, minmax(120px, 1fr));
    gap: 16px;
`

const ValueCard = styled.div`
    background: ${grey[50]};
    padding: 12px;
    border-radius: 4px;
`

const ValueLabel = styled(Typography)`
    color: ${grey[600]};
    font-size: 12px;
    margin-bottom: 4px;
`

const Value = styled(Typography)`
    font-family: monospace;
    font-size: 14px;
    font-weight: 500;
`

const ComparisonChart = styled.div`
    height: 300px;
    margin-bottom: 24px;
`

interface Props {
    rowData: any
    dg4Year: number
    relatedProfiles?: any[]
}

interface ComparisonItem {
    label: string
    value: string
    difference: number
}

const ComparativeAnalysisTab = ({ rowData, dg4Year, relatedProfiles = [] }: Props) => {
    const profile = rowData.overrideProfile?.override ? rowData.overrideProfile : rowData.profile
    if (!profile) return null

    const getComparisonData = () => {
        const mainProfile = {
            name: rowData.profileName,
            values: profile.values,
            startYear: profile.startYear
        }

        const comparisonData: any[] = []
        const maxLength = Math.max(
            profile.values.length,
            ...relatedProfiles.map(p => p.profile.values.length)
        )

        for (let i = 0; i < maxLength; i++) {
            const year = dg4Year + profile.startYear + i
            const dataPoint: any = { year }

            // Add main profile value
            if (i < profile.values.length) {
                dataPoint[mainProfile.name] = profile.values[i]
            }

            // Add related profiles values
            relatedProfiles.forEach(rp => {
                const relatedProfile = rp.overrideProfile?.override ? rp.overrideProfile : rp.profile
                if (i < relatedProfile.values.length) {
                    dataPoint[rp.profileName] = relatedProfile.values[i]
                }
            })

            comparisonData.push(dataPoint)
        }

        return comparisonData
    }

    const getProfileComparisons = (): ComparisonItem[] => {
        if (!relatedProfiles.length) return []

        return relatedProfiles.map(rp => {
            const relatedProfile = rp.overrideProfile?.override ? rp.overrideProfile : rp.profile
            const mainSum = profile.values.reduce((a: number, b: number) => a + b, 0)
            const relatedSum = relatedProfile.values.reduce((a: number, b: number) => a + b, 0)
            const difference = ((mainSum - relatedSum) / relatedSum) * 100

            return {
                label: rp.profileName,
                value: `${Math.abs(difference).toFixed(1)}% ${difference > 0 ? 'higher' : 'lower'}`,
                difference
            }
        })
    }

    const getOverrideComparison = () => {
        if (!rowData.overrideProfile?.override) return null

        const originalValues = rowData.profile.values
        const overrideValues = rowData.overrideProfile.values
        const originalSum = originalValues.reduce((a: number, b: number) => a + b, 0)
        const overrideSum = overrideValues.reduce((a: number, b: number) => a + b, 0)
        const difference = ((overrideSum - originalSum) / originalSum) * 100

        return {
            label: 'Original vs Override',
            value: `${Math.abs(difference).toFixed(1)}% ${difference > 0 ? 'higher' : 'lower'}`,
            difference
        }
    }

    return (
        <Container>
            <Section>
                <SectionTitle>
                    Profile Comparison
                </SectionTitle>
                <ComparisonChart>
                    <AgChartsTimeseries
                        data={getComparisonData()}
                        chartTitle="Profile Comparison"
                        barColors={["#007079", "#FF1243", "#00C8F5", "#FFB800"]}
                        barProfiles={[rowData.profileName, ...relatedProfiles.map(rp => rp.profileName)]}
                        barNames={[rowData.profileName, ...relatedProfiles.map(rp => rp.profileName)]}
                        unit={rowData.unit}
                        height="300px"
                    />
                </ComparisonChart>
            </Section>

            {relatedProfiles.length > 0 && (
                <Section>
                    <SectionTitle>
                        Related Profile Comparisons
                    </SectionTitle>
                    <ValueGrid>
                        {getProfileComparisons().map(({ label, value }) => (
                            <ValueCard key={label}>
                                <ValueLabel variant="caption">
                                    {label}
                                </ValueLabel>
                                <Value>
                                    {value}
                                </Value>
                            </ValueCard>
                        ))}
                    </ValueGrid>
                </Section>
            )}

            {rowData.overrideProfile?.override && (
                <Section>
                    <SectionTitle>
                        Override Analysis
                    </SectionTitle>
                    <ValueGrid>
                        {getOverrideComparison() && (
                            <ValueCard>
                                <ValueLabel variant="caption">
                                    {getOverrideComparison()?.label}
                                </ValueLabel>
                                <Value>
                                    {getOverrideComparison()?.value}
                                </Value>
                            </ValueCard>
                        )}
                    </ValueGrid>
                </Section>
            )}
        </Container>
    )
}

export default ComparativeAnalysisTab 
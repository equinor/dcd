import { GetCaseService } from "./CaseService"
import { ProfileTypes } from "@/Models/enums"

interface TimeSeriesEntry {
    profileType: string;
    startYear: number;
    values: number[];
}

interface SaveTimeSeriesOverrideDto extends TimeSeriesEntry {
    override: boolean;
}

interface SaveTimeSeriesListDto {
    timeSeries: TimeSeriesEntry[];
    overrideTimeSeries: SaveTimeSeriesOverrideDto[];
}

export interface TimeSeriesData {
    profileType: string;
    startYear: number;
    values: number[];
    override?: boolean;
}

export class TimeSeriesService {
    /**
     * Normalizes profile type names to match the expected format on the server
     */
    static normalizeProfileType(profileType: string): string {
        if (profileType.toLowerCase() === "onshorerelatedopexcostprofile") {
            return "OnshoreRelatedOpexCostProfile"
        }

        if (profileType.toLowerCase() === "additionalopexcostprofile") {
            return "AdditionalOpexCostProfile"
        }

        const enumKey = Object.keys(ProfileTypes).find(
            (key) => key.toLowerCase() === profileType.toLowerCase(),
        )
        if (enumKey) {
            return ProfileTypes[enumKey as keyof typeof ProfileTypes]
        }
        return profileType
    }

    /**
     * Saves time series profiles to the server
     */
    static async saveProfiles(
        projectId: string,
        caseId: string,
        timeSeriesData: TimeSeriesData[],
    ): Promise<any> {
        const service = GetCaseService()

        // Add debugging to help diagnose toggle issues
        console.log("TimeSeriesService.saveProfiles - Input data:", timeSeriesData)

        // Separate regular time series from overrides
        const regularEntries: TimeSeriesEntry[] = []
        const overrideEntries: SaveTimeSeriesOverrideDto[] = []

        timeSeriesData.forEach((data) => {
            const entry = {
                profileType: this.normalizeProfileType(data.profileType),
                startYear: data.startYear,
                values: data.values,
            }

            if (data.override) {
                overrideEntries.push({
                    ...entry,
                    override: true,
                })
            } else {
                regularEntries.push(entry)
            }
        })

        const saveTimeSeriesDto: SaveTimeSeriesListDto = {
            timeSeries: regularEntries,
            overrideTimeSeries: overrideEntries,
        }

        console.log("TimeSeriesService.saveProfiles - Sending to API:", {
            regularEntries: regularEntries.length,
            overrideEntries: overrideEntries.length,
            dtoPaths: Object.keys(saveTimeSeriesDto),
        })

        return service.saveProfiles(projectId, caseId, saveTimeSeriesDto)
    }
}

declare namespace Components {
    namespace Schemas {
        export type ArtificialLift = 0 | 1 | 2 | 3; // int32
        export type CampaignCostType = 0 | 1; // int32
        export interface CampaignDto {
            campaignId: string; // uuid
            campaignType: CampaignType /* int32 */;
            rigUpgradingCost: number; // double
            rigMobDemobCost: number; // double
            rigUpgradingProfile: RigProfileDto;
            rigMobDemobProfile: RigProfileDto;
            campaignWells: CampaignWellDto[];
        }
        export type CampaignType = 1 | 2; // int32
        export interface CampaignWellDto {
            wellId: string; // uuid
            wellName: string;
            wellCategory: WellCategory /* int32 */;
            startYear: number; // int32
            values: number /* int32 */[];
        }
        export interface CaseOverviewDto {
            caseId: string; // uuid
            projectId: string; // uuid
            name: string;
            description: string;
            archived: boolean;
            productionStrategyOverview: ProductionStrategyOverview /* int32 */;
            artificialLift: ArtificialLift /* int32 */;
            producerCount: number; // int32
            gasInjectorCount: number; // int32
            waterInjectorCount: number; // int32
            npv: number; // double
            npvOverride: number | null; // double
            breakEven: number; // double
            breakEvenOverride: number | null; // double
            facilitiesAvailability: number; // double
            capexFactorFeasibilityStudies: number; // double
            capexFactorFeedStudies: number; // double
            initialYearsWithoutWellInterventionCost: number; // double
            finalYearsWithoutWellInterventionCost: number; // double
            host: string | null;
            averageCo2Intensity: number; // double
            discountedCashflow: number; // double
            co2RemovedFromGas: number; // double
            co2EmissionFromFuelGas: number; // double
            flaredGasPerProducedVolume: number; // double
            co2EmissionsFromFlaredGas: number; // double
            co2Vented: number; // double
            dailyEmissionFromDrillingRig: number; // double
            averageDevelopmentDrillingDays: number; // double
            dgaDate: string | null; // date-time
            dgbDate: string | null; // date-time
            dgcDate: string | null; // date-time
            apboDate: string | null; // date-time
            borDate: string | null; // date-time
            vpboDate: string | null; // date-time
            dg0Date: string | null; // date-time
            dg1Date: string | null; // date-time
            dg2Date: string | null; // date-time
            dg3Date: string | null; // date-time
            dg4Date: string; // date-time
            createdUtc: string; // date-time
            updatedUtc: string; // date-time
            surfId: string; // uuid
            substructureId: string; // uuid
            topsideId: string; // uuid
            transportId: string; // uuid
            onshorePowerSupplyId: string; // uuid
            sharepointFileId: string | null;
            sharepointFileName: string | null;
            sharepointFileUrl: string | null;
            sharepointUrl: string | null;
            sharepointUpdatedTimestampUtc: string | null; // date-time
        }
        export interface CaseWithAssetsDto {
            case: CaseOverviewDto;
            cessationWellsCost: TimeSeriesDto;
            cessationWellsCostOverride: TimeSeriesOverrideDto;
            cessationOffshoreFacilitiesCost: TimeSeriesDto;
            cessationOffshoreFacilitiesCostOverride: TimeSeriesOverrideDto;
            cessationOnshoreFacilitiesCostProfile: TimeSeriesDto;
            totalFeasibilityAndConceptStudies: TimeSeriesDto;
            totalFeasibilityAndConceptStudiesOverride: TimeSeriesOverrideDto;
            totalFeedStudies: TimeSeriesDto;
            totalFeedStudiesOverride: TimeSeriesOverrideDto;
            totalOtherStudiesCostProfile: TimeSeriesDto;
            historicCostCostProfile: TimeSeriesDto;
            wellInterventionCostProfile: TimeSeriesDto;
            wellInterventionCostProfileOverride: TimeSeriesOverrideDto;
            offshoreFacilitiesOperationsCostProfile: TimeSeriesDto;
            offshoreFacilitiesOperationsCostProfileOverride: TimeSeriesOverrideDto;
            onshoreRelatedOpexCostProfile: TimeSeriesDto;
            additionalOpexCostProfile: TimeSeriesDto;
            calculatedTotalIncomeCostProfileUsd: TimeSeriesDto;
            calculatedTotalCostCostProfileUsd: TimeSeriesDto;
            calculatedDiscountedCashflowService: TimeSeriesDto;
            topside: TopsideDto;
            topsideCostProfile: TimeSeriesDto;
            topsideCostProfileOverride: TimeSeriesOverrideDto;
            topsideCessationCostProfile: TimeSeriesDto;
            drainageStrategy: DrainageStrategyDto;
            productionProfileOil: TimeSeriesDto;
            additionalProductionProfileOil: TimeSeriesDto;
            productionProfileGas: TimeSeriesDto;
            additionalProductionProfileGas: TimeSeriesDto;
            productionProfileWater: TimeSeriesDto;
            productionProfileWaterInjection: TimeSeriesDto;
            fuelFlaringAndLosses: TimeSeriesDto;
            fuelFlaringAndLossesOverride: TimeSeriesOverrideDto;
            netSalesGas: TimeSeriesDto;
            netSalesGasOverride: TimeSeriesOverrideDto;
            totalExportedVolumes: TimeSeriesDto;
            totalExportedVolumesOverride: TimeSeriesOverrideDto;
            co2Emissions: TimeSeriesDto;
            co2EmissionsOverride: TimeSeriesOverrideDto;
            productionProfileNgl: TimeSeriesDto;
            productionProfileNglOverride: TimeSeriesOverrideDto;
            condensateProduction: TimeSeriesDto;
            condensateProductionOverride: TimeSeriesOverrideDto;
            importedElectricity: TimeSeriesDto;
            importedElectricityOverride: TimeSeriesOverrideDto;
            co2Intensity: TimeSeriesDto;
            co2IntensityOverride: TimeSeriesOverrideDto;
            deferredOilProduction: TimeSeriesDto;
            deferredGasProduction: TimeSeriesDto;
            substructure: SubstructureDto;
            substructureCostProfile: TimeSeriesDto;
            substructureCostProfileOverride: TimeSeriesOverrideDto;
            substructureCessationCostProfile: TimeSeriesDto;
            surf: SurfDto;
            surfCostProfile: TimeSeriesDto;
            surfCostProfileOverride: TimeSeriesOverrideDto;
            surfCessationCostProfile: TimeSeriesDto;
            transport: TransportDto;
            transportCostProfile: TimeSeriesDto;
            transportCostProfileOverride: TimeSeriesOverrideDto;
            transportCessationCostProfile: TimeSeriesDto;
            onshorePowerSupply: OnshorePowerSupplyDto;
            onshorePowerSupplyCostProfile: TimeSeriesDto;
            onshorePowerSupplyCostProfileOverride: TimeSeriesOverrideDto;
            explorationWellCostProfile: TimeSeriesDto;
            explorationWellCostProfileOverride: TimeSeriesOverrideDto;
            appraisalWellCostProfile: TimeSeriesDto;
            appraisalWellCostProfileOverride: TimeSeriesOverrideDto;
            sidetrackCostProfile: TimeSeriesDto;
            sidetrackCostProfileOverride: TimeSeriesOverrideDto;
            gAndGAdminCost: TimeSeriesDto;
            gAndGAdminCostOverride: TimeSeriesOverrideDto;
            seismicAcquisitionAndProcessing: TimeSeriesDto;
            countryOfficeCost: TimeSeriesDto;
            projectSpecificDrillingCostProfile: TimeSeriesDto;
            explorationRigUpgradingCostProfile: TimeSeriesDto;
            explorationRigUpgradingCostProfileOverride: TimeSeriesOverrideDto;
            explorationRigMobDemob: TimeSeriesDto;
            explorationRigMobDemobOverride: TimeSeriesOverrideDto;
            developmentCampaigns: CampaignDto[];
            explorationCampaigns: CampaignDto[];
            oilProducerCostProfile: TimeSeriesDto;
            oilProducerCostProfileOverride: TimeSeriesOverrideDto;
            gasProducerCostProfile: TimeSeriesDto;
            gasProducerCostProfileOverride: TimeSeriesOverrideDto;
            waterInjectorCostProfile: TimeSeriesDto;
            waterInjectorCostProfileOverride: TimeSeriesOverrideDto;
            gasInjectorCostProfile: TimeSeriesDto;
            gasInjectorCostProfileOverride: TimeSeriesOverrideDto;
            developmentRigUpgradingCostProfile: TimeSeriesDto;
            developmentRigUpgradingCostProfileOverride: TimeSeriesOverrideDto;
            developmentRigMobDemob: TimeSeriesDto;
            developmentRigMobDemobOverride: TimeSeriesOverrideDto;
        }
        export type ChangeLogCategory = 0 | 1 | 2 | 3 | 4 | 5; // int32
        export interface Co2DrillingFlaringFuelTotalsDto {
            co2Drilling: number; // double
            co2Fuel: number; // double
            co2Flaring: number; // double
        }
        export interface CommonProjectAndRevisionDto {
            updatedUtc: string; // date-time
            classification: ProjectClassification /* int32 */;
            name: string;
            fusionProjectId: string; // uuid
            referenceCaseId: string; // uuid
            description: string;
            country: string;
            currency: Currency /* int32 */;
            physicalUnit: PhysUnit /* int32 */;
            projectPhase: ProjectPhase /* int32 */;
            internalProjectPhase: InternalProjectPhase /* int32 */;
            projectCategory: ProjectCategory /* int32 */;
            sharepointSiteUrl: string | null;
            oilPriceUsd: number; // double
            gasPriceNok: number; // double
            nglPriceUsd: number; // double
            discountRate: number; // double
            exchangeRateUsdToNok: number; // double
            npvYear: number; // int32
            cases: CaseOverviewDto[];
            wells: WellOverviewDto[];
        }
        export interface CompareCasesDto {
            caseId: string; // uuid
            totalOilProduction: number; // double
            additionalOilProduction: number; // double
            totalGasProduction: number; // double
            additionalGasProduction: number; // double
            totalExportedVolumes: number; // double
            totalStudyCostsPlusOpex: number; // double
            totalCessationCosts: number; // double
            offshorePlusOnshoreFacilityCosts: number; // double
            developmentWellCosts: number; // double
            explorationWellCosts: number; // double
            totalCo2Emissions: number; // double
            co2Intensity: number; // double
        }
        export type Concept = 0 | 1 | 2 | 3 | 4 | 5 | 6 | 7 | 8 | 9 | 10 | 11 | 12; // int32
        export interface CreateCampaignDto {
            campaignType: CampaignType /* int32 */;
        }
        export interface CreateCaseDto {
            name: string;
            description: string;
            productionStrategyOverview: ProductionStrategyOverview /* int32 */;
            producerCount: number; // int32
            gasInjectorCount: number; // int32
            waterInjectorCount: number; // int32
            dg4Date: string; // date-time
        }
        export interface CreateProjectMemberDto {
            role: ProjectMemberRole /* int32 */;
            azureAdUserId: string; // uuid
        }
        export interface CreateRevisionDto {
            name: string;
            internalProjectPhase: InternalProjectPhase /* int32 */;
            classification: ProjectClassification /* int32 */;
            arena: boolean;
            mdqc: boolean;
        }
        export interface CreateWellDto {
            name: string;
            wellCategory: WellCategory /* int32 */;
            wellInterventionCost: number; // double
            plugingAndAbandonmentCost: number; // double
            wellCost: number; // double
            drillingDays: number; // double
        }
        export type Currency = 1 | 2; // int32
        export interface DeleteWellDto {
            id: string; // uuid
        }
        export interface DrainageStrategyDto {
            id: string; // uuid
            nglYield: number; // double
            condensateYield: number; // double
            gasShrinkageFactor: number; // double
            producerCount: number; // int32
            gasInjectorCount: number; // int32
            waterInjectorCount: number; // int32
            artificialLift: ArtificialLift /* int32 */;
            gasSolution: GasSolution /* int32 */;
        }
        export interface FeatureToggleDto {
            revisionEnabled?: boolean;
            environmentName?: string | null;
        }
        export type GasSolution = 0 | 1; // int32
        export interface ImageDto {
            imageId: string; // uuid
            createTime: string; // date-time
            description: string | null;
            caseId: string; // uuid
            projectId: string; // uuid
            imageData: string;
        }
        export type InternalProjectPhase = 0 | 1 | 2; // int32
        export type Maturity = 0 | 1 | 2 | 3; // int32
        export type NoAccessReason = 1 | 2 | 3 | 4; // int32
        export interface OnshorePowerSupplyDto {
            id: string; // uuid
            lastChangedDate: string | null; // date-time
            costYear: number; // int32
            source: Source /* int32 */;
            prospVersion: string | null; // date-time
        }
        export type PhysUnit = 0 | 1; // int32
        export type ProductionFlowline = 0 | 1 | 2 | 3 | 4 | 5 | 6 | 7 | 8 | 9 | 10 | 11 | 12 | 13; // int32
        export type ProductionStrategyOverview = 0 | 1 | 2 | 3 | 4; // int32
        export type ProjectCategory = 0 | 1 | 2 | 3 | 4 | 5 | 6 | 7 | 8 | 9 | 10 | 11 | 12 | 13 | 14 | 15 | 16 | 17 | 18 | 19 | 20 | 21; // int32
        export interface ProjectChangeLogDto {
            entityDescription: string | null;
            entityId: string; // uuid
            entityName: string;
            propertyName: string | null;
            oldValue: string | null;
            newValue: string | null;
            username: string | null;
            timestampUtc: string; // date-time
            entityState: string;
            category: ChangeLogCategory /* int32 */;
        }
        export type ProjectClassification = 0 | 1 | 2 | 3; // int32
        export interface ProjectDataDto {
            projectId: string; // uuid
            dataType: string;
            userActions: UserActionsDto;
            projectMembers: ProjectMemberDto[];
            revisionDetailsList: RevisionDetailsDto[];
            commonProjectAndRevisionData: CommonProjectAndRevisionDto;
        }
        export interface ProjectExistsDto {
            projectExists: boolean;
            canCreateProject: boolean;
            noAccessReason: NoAccessReason /* int32 */;
        }
        export interface ProjectMemberDto {
            projectId: string; // uuid
            azureAdUserId: string; // uuid
            role: ProjectMemberRole /* int32 */;
            isPmt: boolean;
        }
        export type ProjectMemberRole = 0 | 1; // int32
        export type ProjectPhase = 0 | 1 | 2 | 3 | 4 | 5 | 6 | 7 | 8 | 9; // int32
        export interface RevisionDataDto {
            projectId: string; // uuid
            revisionId: string; // uuid
            dataType: string;
            userActions: UserActionsDto;
            revisionDetails: RevisionDetailsDto;
            revisionDetailsList: RevisionDetailsDto[];
            commonProjectAndRevisionData: CommonProjectAndRevisionDto;
        }
        export interface RevisionDetailsDto {
            revisionId: string; // uuid
            revisionName: string;
            revisionDate: string; // date-time
            arena: boolean;
            mdqc: boolean;
        }
        export interface RigProfileDto {
            startYear: number; // int32
            values: number /* double */[];
        }
        export interface SaveCampaignWellDto {
            wellId: string; // uuid
            startYear: number; // int32
            values: number /* int32 */[];
        }
        export interface SaveTimeSeriesDto {
            profileType: string;
            startYear: number; // int32
            values: number /* double */[];
        }
        export interface SaveTimeSeriesListDto {
            timeSeries: SaveTimeSeriesDto[];
            overrideTimeSeries: SaveTimeSeriesOverrideDto[];
        }
        export interface SaveTimeSeriesOverrideDto {
            profileType: string;
            startYear: number; // int32
            values: number /* double */[];
            override: boolean;
        }
        export interface SharePointFileDto {
            name: string;
            id: string;
        }
        export interface SharePointImportDto {
            caseId: string; // uuid
            sharePointFileId: string;
            sharePointFileName: string;
            sharePointSiteUrl: string;
        }
        export interface SharePointSiteUrlDto {
            url: string;
        }
        export type Source = 0 | 1; // int32
        export interface SubstructureDto {
            id: string; // uuid
            dryWeight: number; // double
            maturity: Maturity /* int32 */;
            approvedBy: string;
            costYear: number; // int32
            prospVersion: string | null; // date-time
            source: Source /* int32 */;
            lastChangedDate: string | null; // date-time
            concept: Concept /* int32 */;
        }
        export interface SurfDto {
            id: string; // uuid
            cessationCost: number; // double
            maturity: Maturity /* int32 */;
            infieldPipelineSystemLength: number; // double
            umbilicalSystemLength: number; // double
            artificialLift: ArtificialLift /* int32 */;
            riserCount: number; // int32
            templateCount: number; // int32
            producerCount: number; // int32
            gasInjectorCount: number; // int32
            waterInjectorCount: number; // int32
            productionFlowline: ProductionFlowline /* int32 */;
            lastChangedDate: string; // date-time
            costYear: number; // int32
            source: Source /* int32 */;
            prospVersion: string | null; // date-time
            approvedBy: string;
        }
        export interface TimeSeriesDto {
            startYear: number; // int32
            values: number /* double */[];
            updatedUtc: string; // date-time
        }
        export interface TimeSeriesOverrideDto {
            startYear: number; // int32
            values: number /* double */[];
            override: boolean;
            updatedUtc: string; // date-time
        }
        export interface TopsideDto {
            id: string; // uuid
            dryWeight: number; // double
            oilCapacity: number; // double
            gasCapacity: number; // double
            waterInjectionCapacity: number; // double
            artificialLift: ArtificialLift /* int32 */;
            maturity: Maturity /* int32 */;
            fuelConsumption: number; // double
            flaredGas: number; // double
            producerCount: number; // int32
            gasInjectorCount: number; // int32
            waterInjectorCount: number; // int32
            co2ShareOilProfile: number; // double
            co2ShareGasProfile: number; // double
            co2ShareWaterInjectionProfile: number; // double
            co2OnMaxOilProfile: number; // double
            co2OnMaxGasProfile: number; // double
            co2OnMaxWaterInjectionProfile: number; // double
            costYear: number; // int32
            prospVersion: string | null; // date-time
            lastChangedDate: string | null; // date-time
            source: Source /* int32 */;
            approvedBy: string;
            facilityOpex: number; // double
            peakElectricityImported: number; // double
        }
        export interface TransportDto {
            id: string; // uuid
            maturity: Maturity /* int32 */;
            gasExportPipelineLength: number; // double
            oilExportPipelineLength: number; // double
            lastChangedDate: string | null; // date-time
            costYear: number; // int32
            source: Source /* int32 */;
            prospVersion: string | null; // date-time
        }
        export interface UpdateCampaignDto {
            campaignCostType: CampaignCostType /* int32 */;
            startYear: number; // int32
            values: number /* double */[];
        }
        export interface UpdateCaseDto {
            name: string;
            description: string;
            archived: boolean;
            artificialLift: ArtificialLift /* int32 */;
            productionStrategyOverview: ProductionStrategyOverview /* int32 */;
            producerCount: number; // int32
            gasInjectorCount: number; // int32
            waterInjectorCount: number; // int32
            facilitiesAvailability: number; // double
            capexFactorFeasibilityStudies: number; // double
            capexFactorFeedStudies: number; // double
            initialYearsWithoutWellInterventionCost: number; // double
            finalYearsWithoutWellInterventionCost: number; // double
            npv: number; // double
            npvOverride: number | null; // double
            breakEven: number; // double
            breakEvenOverride: number | null; // double
            host: string | null;
            averageCo2Intensity: number; // double
            discountedCashflow: number; // double
            co2RemovedFromGas: number; // double
            co2EmissionFromFuelGas: number; // double
            flaredGasPerProducedVolume: number; // double
            co2EmissionsFromFlaredGas: number; // double
            co2Vented: number; // double
            dailyEmissionFromDrillingRig: number; // double
            averageDevelopmentDrillingDays: number; // double
            dgaDate: string | null; // date-time
            dgbDate: string | null; // date-time
            dgcDate: string | null; // date-time
            apboDate: string | null; // date-time
            borDate: string | null; // date-time
            vpboDate: string | null; // date-time
            dg0Date: string | null; // date-time
            dg1Date: string | null; // date-time
            dg2Date: string | null; // date-time
            dg3Date: string | null; // date-time
            dg4Date: string; // date-time
            sharepointFileId: string | null;
            sharepointFileName: string | null;
            sharepointFileUrl: string | null;
            sharepointUrl: string | null;
        }
        export interface UpdateDrainageStrategyDto {
            nglYield: number; // double
            condensateYield: number; // double
            gasShrinkageFactor: number; // double
            producerCount: number; // int32
            gasInjectorCount: number; // int32
            waterInjectorCount: number; // int32
            artificialLift: ArtificialLift /* int32 */;
            gasSolution: GasSolution /* int32 */;
        }
        export interface UpdateImageDto {
            description: string | null;
        }
        export interface UpdateOnshorePowerSupplyDto {
            costYear: number; // int32
            source: Source /* int32 */;
        }
        export interface UpdateProjectDto {
            name: string;
            referenceCaseId: string | null; // uuid
            description: string;
            country: string;
            currency: Currency /* int32 */;
            physicalUnit: PhysUnit /* int32 */;
            classification: ProjectClassification /* int32 */;
            projectPhase: ProjectPhase /* int32 */;
            internalProjectPhase: InternalProjectPhase /* int32 */;
            projectCategory: ProjectCategory /* int32 */;
            sharepointSiteUrl: string | null;
            oilPriceUsd: number; // double
            gasPriceNok: number; // double
            nglPriceUsd: number; // double
            discountRate: number; // double
            exchangeRateUsdToNok: number; // double
            npvYear: number; // int32
        }
        export interface UpdateProjectMemberDto {
            role: ProjectMemberRole /* int32 */;
            azureAdUserId: string; // uuid
        }
        export interface UpdateRevisionDto {
            name: string;
            arena: boolean;
            mdqc: boolean;
        }
        export interface UpdateRigMobDemobCostDto {
            cost: number; // double
        }
        export interface UpdateRigUpgradingCostDto {
            cost: number; // double
        }
        export interface UpdateSubstructureDto {
            dryWeight: number; // double
            costYear: number; // int32
            source: Source /* int32 */;
            concept: Concept /* int32 */;
            maturity: Maturity /* int32 */;
            approvedBy: string;
        }
        export interface UpdateSurfDto {
            cessationCost: number; // double
            infieldPipelineSystemLength: number; // double
            umbilicalSystemLength: number; // double
            artificialLift: ArtificialLift /* int32 */;
            riserCount: number; // int32
            templateCount: number; // int32
            producerCount: number; // int32
            gasInjectorCount: number; // int32
            waterInjectorCount: number; // int32
            productionFlowline: ProductionFlowline /* int32 */;
            costYear: number; // int32
            source: Source /* int32 */;
            approvedBy: string;
            maturity: Maturity /* int32 */;
        }
        export interface UpdateTopsideDto {
            dryWeight: number; // double
            oilCapacity: number; // double
            gasCapacity: number; // double
            waterInjectionCapacity: number; // double
            artificialLift: ArtificialLift /* int32 */;
            fuelConsumption: number; // double
            flaredGas: number; // double
            producerCount: number; // int32
            gasInjectorCount: number; // int32
            waterInjectorCount: number; // int32
            co2ShareOilProfile: number; // double
            co2ShareGasProfile: number; // double
            co2ShareWaterInjectionProfile: number; // double
            co2OnMaxOilProfile: number; // double
            co2OnMaxGasProfile: number; // double
            co2OnMaxWaterInjectionProfile: number; // double
            costYear: number; // int32
            facilityOpex: number; // double
            peakElectricityImported: number; // double
            source: Source /* int32 */;
            maturity: Maturity /* int32 */;
            approvedBy: string;
        }
        export interface UpdateTransportDto {
            gasExportPipelineLength: number; // double
            oilExportPipelineLength: number; // double
            costYear: number; // int32
            source: Source /* int32 */;
            maturity: Maturity /* int32 */;
        }
        export interface UpdateWellDto {
            id: string; // uuid
            name: string;
            wellInterventionCost: number; // double
            plugingAndAbandonmentCost: number; // double
            wellCategory: WellCategory /* int32 */;
            wellCost: number; // double
            drillingDays: number; // double
        }
        export interface UpdateWellsDto {
            updateWellDtos: UpdateWellDto[];
            createWellDtos: CreateWellDto[];
            deleteWellDtos: DeleteWellDto[];
        }
        export interface UserActionsDto {
            canView: boolean;
            canCreateRevision: boolean;
            canEditProjectData: boolean;
            canEditProjectMembers: boolean;
        }
        export interface VideoDto {
            videoName: string;
            base64EncodedData: string;
        }
        export type WellCategory = 0 | 1 | 2 | 3 | 4 | 5 | 6; // int32
        export interface WellOverviewDto {
            id: string; // uuid
            name: string;
            wellCategory: WellCategory /* int32 */;
            wellCost: number; // double
            drillingDays: number; // double
            plugingAndAbandonmentCost: number; // double
            wellInterventionCost: number; // double
        }
    }
}
declare namespace Paths {
    namespace FeatureToggles {
        namespace Get {
            namespace Responses {
                export type $200 = Components.Schemas.FeatureToggleDto;
            }
        }
    }
    namespace Projects {
        namespace Post {
            namespace Parameters {
                export type ContextId = string; // uuid
            }
            export interface QueryParameters {
                contextId?: Parameters.ContextId /* uuid */;
            }
            namespace Responses {
                export type $200 = Components.Schemas.ProjectDataDto;
            }
        }
    }
    namespace Projects$ProjectId {
        namespace Get {
            namespace Parameters {
                export type ProjectId = string; // uuid
            }
            export interface PathParameters {
                projectId: Parameters.ProjectId /* uuid */;
            }
            namespace Responses {
                export type $200 = Components.Schemas.ProjectDataDto;
            }
        }
        namespace Put {
            namespace Parameters {
                export type ProjectId = string; // uuid
            }
            export interface PathParameters {
                projectId: Parameters.ProjectId /* uuid */;
            }
            export type RequestBody = Components.Schemas.UpdateProjectDto;
            namespace Responses {
                export type $200 = Components.Schemas.ProjectDataDto;
            }
        }
    }
    namespace Projects$ProjectIdCaseComparison {
        namespace Get {
            namespace Parameters {
                export type ProjectId = string; // uuid
            }
            export interface PathParameters {
                projectId: Parameters.ProjectId /* uuid */;
            }
            namespace Responses {
                export type $200 = Components.Schemas.CompareCasesDto[];
            }
        }
    }
    namespace Projects$ProjectIdCases {
        namespace Post {
            namespace Parameters {
                export type ProjectId = string; // uuid
            }
            export interface PathParameters {
                projectId: Parameters.ProjectId /* uuid */;
            }
            export type RequestBody = Components.Schemas.CreateCaseDto;
            namespace Responses {
                export type $200 = Components.Schemas.ProjectDataDto;
            }
        }
    }
    namespace Projects$ProjectIdCases$CaseId {
        namespace Delete {
            namespace Parameters {
                export type CaseId = string; // uuid
                export type ProjectId = string; // uuid
            }
            export interface PathParameters {
                projectId: Parameters.ProjectId /* uuid */;
                caseId: Parameters.CaseId /* uuid */;
            }
            namespace Responses {
                export type $200 = Components.Schemas.ProjectDataDto;
            }
        }
        namespace Put {
            namespace Parameters {
                export type CaseId = string; // uuid
                export type ProjectId = string; // uuid
            }
            export interface PathParameters {
                projectId: Parameters.ProjectId /* uuid */;
                caseId: Parameters.CaseId /* uuid */;
            }
            export type RequestBody = Components.Schemas.UpdateCaseDto;
            namespace Responses {
                export interface $200 {
                }
            }
        }
    }
    namespace Projects$ProjectIdCases$CaseIdCampaigns {
        namespace Post {
            namespace Parameters {
                export type CaseId = string; // uuid
                export type ProjectId = string; // uuid
            }
            export interface PathParameters {
                projectId: Parameters.ProjectId /* uuid */;
                caseId: Parameters.CaseId /* uuid */;
            }
            export type RequestBody = Components.Schemas.CreateCampaignDto;
            namespace Responses {
                export type $200 = Components.Schemas.CampaignDto;
            }
        }
    }
    namespace Projects$ProjectIdCases$CaseIdCampaigns$CampaignId {
        namespace Delete {
            namespace Parameters {
                export type CampaignId = string; // uuid
                export type CaseId = string; // uuid
                export type ProjectId = string; // uuid
            }
            export interface PathParameters {
                projectId: Parameters.ProjectId /* uuid */;
                caseId: Parameters.CaseId /* uuid */;
                campaignId: Parameters.CampaignId /* uuid */;
            }
            namespace Responses {
                export interface $200 {
                }
            }
        }
        namespace Put {
            namespace Parameters {
                export type CampaignId = string; // uuid
                export type CaseId = string; // uuid
                export type ProjectId = string; // uuid
            }
            export interface PathParameters {
                projectId: Parameters.ProjectId /* uuid */;
                caseId: Parameters.CaseId /* uuid */;
                campaignId: Parameters.CampaignId /* uuid */;
            }
            export type RequestBody = Components.Schemas.UpdateCampaignDto;
            namespace Responses {
                export interface $200 {
                }
            }
        }
    }
    namespace Projects$ProjectIdCases$CaseIdCampaigns$CampaignIdRigMobdemobCost {
        namespace Put {
            namespace Parameters {
                export type CampaignId = string; // uuid
                export type CaseId = string; // uuid
                export type ProjectId = string; // uuid
            }
            export interface PathParameters {
                projectId: Parameters.ProjectId /* uuid */;
                caseId: Parameters.CaseId /* uuid */;
                campaignId: Parameters.CampaignId /* uuid */;
            }
            export type RequestBody = Components.Schemas.UpdateRigMobDemobCostDto;
            namespace Responses {
                export interface $200 {
                }
            }
        }
    }
    namespace Projects$ProjectIdCases$CaseIdCampaigns$CampaignIdRigUpgradingCost {
        namespace Put {
            namespace Parameters {
                export type CampaignId = string; // uuid
                export type CaseId = string; // uuid
                export type ProjectId = string; // uuid
            }
            export interface PathParameters {
                projectId: Parameters.ProjectId /* uuid */;
                caseId: Parameters.CaseId /* uuid */;
                campaignId: Parameters.CampaignId /* uuid */;
            }
            export type RequestBody = Components.Schemas.UpdateRigUpgradingCostDto;
            namespace Responses {
                export interface $200 {
                }
            }
        }
    }
    namespace Projects$ProjectIdCases$CaseIdCampaigns$CampaignIdWells {
        namespace Put {
            namespace Parameters {
                export type CampaignId = string; // uuid
                export type CaseId = string; // uuid
                export type ProjectId = string; // uuid
            }
            export interface PathParameters {
                projectId: Parameters.ProjectId /* uuid */;
                caseId: Parameters.CaseId /* uuid */;
                campaignId: Parameters.CampaignId /* uuid */;
            }
            export type RequestBody = Components.Schemas.SaveCampaignWellDto[];
            namespace Responses {
                export interface $200 {
                }
            }
        }
    }
    namespace Projects$ProjectIdCases$CaseIdCaseWithAssets {
        namespace Get {
            namespace Parameters {
                export type CaseId = string; // uuid
                export type ProjectId = string; // uuid
            }
            export interface PathParameters {
                projectId: Parameters.ProjectId /* uuid */;
                caseId: Parameters.CaseId /* uuid */;
            }
            namespace Responses {
                export type $200 = Components.Schemas.CaseWithAssetsDto;
            }
        }
    }
    namespace Projects$ProjectIdCases$CaseIdCo2DrillingFlaringFuelTotals {
        namespace Get {
            namespace Parameters {
                export type CaseId = string; // uuid
                export type ProjectId = string; // uuid
            }
            export interface PathParameters {
                projectId: Parameters.ProjectId /* uuid */;
                caseId: Parameters.CaseId /* uuid */;
            }
            namespace Responses {
                export type $200 = Components.Schemas.Co2DrillingFlaringFuelTotalsDto;
            }
        }
    }
    namespace Projects$ProjectIdCases$CaseIdDrainageStrategy {
        namespace Put {
            namespace Parameters {
                export type CaseId = string; // uuid
                export type ProjectId = string; // uuid
            }
            export interface PathParameters {
                projectId: Parameters.ProjectId /* uuid */;
                caseId: Parameters.CaseId /* uuid */;
            }
            export type RequestBody = Components.Schemas.UpdateDrainageStrategyDto;
            namespace Responses {
                export interface $200 {
                }
            }
        }
    }
    namespace Projects$ProjectIdCases$CaseIdImages {
        namespace Get {
            namespace Parameters {
                export type CaseId = string; // uuid
                export type ProjectId = string; // uuid
            }
            export interface PathParameters {
                projectId: Parameters.ProjectId /* uuid */;
                caseId: Parameters.CaseId /* uuid */;
            }
            namespace Responses {
                export type $200 = Components.Schemas.ImageDto[];
            }
        }
        namespace Post {
            namespace Parameters {
                export type CaseId = string; // uuid
                export type ProjectId = string; // uuid
            }
            export interface PathParameters {
                projectId: Parameters.ProjectId /* uuid */;
                caseId: Parameters.CaseId /* uuid */;
            }
            export interface RequestBody {
                image?: string; // binary
            }
            namespace Responses {
                export type $200 = Components.Schemas.ImageDto;
            }
        }
    }
    namespace Projects$ProjectIdCases$CaseIdImages$ImageId {
        namespace Delete {
            namespace Parameters {
                export type CaseId = string; // uuid
                export type ImageId = string; // uuid
                export type ProjectId = string; // uuid
            }
            export interface PathParameters {
                projectId: Parameters.ProjectId /* uuid */;
                caseId: Parameters.CaseId /* uuid */;
                imageId: Parameters.ImageId /* uuid */;
            }
            namespace Responses {
                export interface $200 {
                }
            }
        }
        namespace Put {
            namespace Parameters {
                export type CaseId = string; // uuid
                export type ImageId = string; // uuid
                export type ProjectId = string; // uuid
            }
            export interface PathParameters {
                projectId: Parameters.ProjectId /* uuid */;
                caseId: Parameters.CaseId /* uuid */;
                imageId: Parameters.ImageId /* uuid */;
            }
            export type RequestBody = Components.Schemas.UpdateImageDto;
            namespace Responses {
                export interface $200 {
                }
            }
        }
    }
    namespace Projects$ProjectIdCases$CaseIdOnshorePowerSupply {
        namespace Put {
            namespace Parameters {
                export type CaseId = string; // uuid
                export type ProjectId = string; // uuid
            }
            export interface PathParameters {
                projectId: Parameters.ProjectId /* uuid */;
                caseId: Parameters.CaseId /* uuid */;
            }
            export type RequestBody = Components.Schemas.UpdateOnshorePowerSupplyDto;
            namespace Responses {
                export interface $200 {
                }
            }
        }
    }
    namespace Projects$ProjectIdCases$CaseIdProfilesSaveBatch {
        namespace Post {
            namespace Parameters {
                export type CaseId = string; // uuid
                export type ProjectId = string; // uuid
            }
            export interface PathParameters {
                projectId: Parameters.ProjectId /* uuid */;
                caseId: Parameters.CaseId /* uuid */;
            }
            export type RequestBody = Components.Schemas.SaveTimeSeriesListDto;
            namespace Responses {
                export interface $200 {
                }
            }
        }
    }
    namespace Projects$ProjectIdCases$CaseIdProspCheckForUpdate {
        namespace Get {
            namespace Parameters {
                export type CaseId = string; // uuid
                export type ProjectId = string; // uuid
            }
            export interface PathParameters {
                projectId: Parameters.ProjectId /* uuid */;
                caseId: Parameters.CaseId /* uuid */;
            }
            namespace Responses {
                export type $200 = boolean;
            }
        }
    }
    namespace Projects$ProjectIdCases$CaseIdSubstructure {
        namespace Put {
            namespace Parameters {
                export type CaseId = string; // uuid
                export type ProjectId = string; // uuid
            }
            export interface PathParameters {
                projectId: Parameters.ProjectId /* uuid */;
                caseId: Parameters.CaseId /* uuid */;
            }
            export type RequestBody = Components.Schemas.UpdateSubstructureDto;
            namespace Responses {
                export interface $200 {
                }
            }
        }
    }
    namespace Projects$ProjectIdCases$CaseIdSurf {
        namespace Put {
            namespace Parameters {
                export type CaseId = string; // uuid
                export type ProjectId = string; // uuid
            }
            export interface PathParameters {
                projectId: Parameters.ProjectId /* uuid */;
                caseId: Parameters.CaseId /* uuid */;
            }
            export type RequestBody = Components.Schemas.UpdateSurfDto;
            namespace Responses {
                export interface $200 {
                }
            }
        }
    }
    namespace Projects$ProjectIdCases$CaseIdTopside {
        namespace Put {
            namespace Parameters {
                export type CaseId = string; // uuid
                export type ProjectId = string; // uuid
            }
            export interface PathParameters {
                projectId: Parameters.ProjectId /* uuid */;
                caseId: Parameters.CaseId /* uuid */;
            }
            export type RequestBody = Components.Schemas.UpdateTopsideDto;
            namespace Responses {
                export interface $200 {
                }
            }
        }
    }
    namespace Projects$ProjectIdCases$CaseIdTransport {
        namespace Put {
            namespace Parameters {
                export type CaseId = string; // uuid
                export type ProjectId = string; // uuid
            }
            export interface PathParameters {
                projectId: Parameters.ProjectId /* uuid */;
                caseId: Parameters.CaseId /* uuid */;
            }
            export type RequestBody = Components.Schemas.UpdateTransportDto;
            namespace Responses {
                export interface $200 {
                }
            }
        }
    }
    namespace Projects$ProjectIdCasesCopy {
        namespace Post {
            namespace Parameters {
                export type CopyCaseId = string; // uuid
                export type ProjectId = string; // uuid
            }
            export interface PathParameters {
                projectId: Parameters.ProjectId /* uuid */;
            }
            export interface QueryParameters {
                copyCaseId?: Parameters.CopyCaseId /* uuid */;
            }
            namespace Responses {
                export type $200 = Components.Schemas.ProjectDataDto;
            }
        }
    }
    namespace Projects$ProjectIdChangeLogs {
        namespace Get {
            namespace Parameters {
                export type ProjectId = string; // uuid
            }
            export interface PathParameters {
                projectId: Parameters.ProjectId /* uuid */;
            }
            namespace Responses {
                export type $200 = Components.Schemas.ProjectChangeLogDto[];
            }
        }
    }
    namespace Projects$ProjectIdImages {
        namespace Get {
            namespace Parameters {
                export type ProjectId = string; // uuid
            }
            export interface PathParameters {
                projectId: Parameters.ProjectId /* uuid */;
            }
            namespace Responses {
                export type $200 = Components.Schemas.ImageDto[];
            }
        }
        namespace Post {
            namespace Parameters {
                export type ProjectId = string; // uuid
            }
            export interface PathParameters {
                projectId: Parameters.ProjectId /* uuid */;
            }
            export interface RequestBody {
                image?: string; // binary
            }
            namespace Responses {
                export type $200 = Components.Schemas.ImageDto;
            }
        }
    }
    namespace Projects$ProjectIdImages$ImageId {
        namespace Delete {
            namespace Parameters {
                export type ImageId = string; // uuid
                export type ProjectId = string; // uuid
            }
            export interface PathParameters {
                projectId: Parameters.ProjectId /* uuid */;
                imageId: Parameters.ImageId /* uuid */;
            }
            namespace Responses {
                export interface $200 {
                }
            }
        }
        namespace Put {
            namespace Parameters {
                export type ImageId = string; // uuid
                export type ProjectId = string; // uuid
            }
            export interface PathParameters {
                projectId: Parameters.ProjectId /* uuid */;
                imageId: Parameters.ImageId /* uuid */;
            }
            export type RequestBody = Components.Schemas.UpdateImageDto;
            namespace Responses {
                export interface $200 {
                }
            }
        }
    }
    namespace Projects$ProjectIdMembers {
        namespace Get {
            namespace Parameters {
                export type ProjectId = string; // uuid
            }
            export interface PathParameters {
                projectId: Parameters.ProjectId /* uuid */;
            }
            namespace Responses {
                export type $200 = Components.Schemas.ProjectMemberDto[];
            }
        }
        namespace Post {
            namespace Parameters {
                export type ProjectId = string; // uuid
            }
            export interface PathParameters {
                projectId: Parameters.ProjectId /* uuid */;
            }
            export type RequestBody = Components.Schemas.CreateProjectMemberDto;
            namespace Responses {
                export type $200 = Components.Schemas.ProjectMemberDto;
            }
        }
        namespace Put {
            namespace Parameters {
                export type ProjectId = string; // uuid
            }
            export interface PathParameters {
                projectId: Parameters.ProjectId /* uuid */;
            }
            export type RequestBody = Components.Schemas.UpdateProjectMemberDto;
            namespace Responses {
                export type $200 = Components.Schemas.ProjectMemberDto;
            }
        }
    }
    namespace Projects$ProjectIdMembers$UserId {
        namespace Delete {
            namespace Parameters {
                export type ProjectId = string; // uuid
                export type UserId = string; // uuid
            }
            export interface PathParameters {
                projectId: Parameters.ProjectId /* uuid */;
                userId: Parameters.UserId /* uuid */;
            }
            namespace Responses {
                export interface $200 {
                }
            }
        }
    }
    namespace Projects$ProjectIdMembersContext$ContextId {
        namespace Get {
            namespace Parameters {
                export type ContextId = string; // uuid
                export type ProjectId = string; // uuid
            }
            export interface PathParameters {
                projectId: Parameters.ProjectId /* uuid */;
                contextId: Parameters.ContextId /* uuid */;
            }
            namespace Responses {
                export type $200 = Components.Schemas.ProjectMemberDto[];
            }
        }
    }
    namespace Projects$ProjectIdProspImport {
        namespace Post {
            namespace Parameters {
                export type ProjectId = string; // uuid
            }
            export interface PathParameters {
                projectId: Parameters.ProjectId /* uuid */;
            }
            export type RequestBody = Components.Schemas.SharePointImportDto[];
            namespace Responses {
                export type $200 = Components.Schemas.ProjectDataDto;
            }
        }
    }
    namespace Projects$ProjectIdProspList {
        namespace Post {
            namespace Parameters {
                export type ProjectId = string; // uuid
            }
            export interface PathParameters {
                projectId: Parameters.ProjectId /* uuid */;
            }
            export type RequestBody = Components.Schemas.SharePointSiteUrlDto;
            namespace Responses {
                export type $200 = Components.Schemas.SharePointFileDto[];
            }
        }
    }
    namespace Projects$ProjectIdRevisions {
        namespace Post {
            namespace Parameters {
                export type ProjectId = string; // uuid
            }
            export interface PathParameters {
                projectId: Parameters.ProjectId /* uuid */;
            }
            export type RequestBody = Components.Schemas.CreateRevisionDto;
            namespace Responses {
                export type $200 = Components.Schemas.RevisionDataDto;
            }
        }
    }
    namespace Projects$ProjectIdRevisions$RevisionId {
        namespace Delete {
            namespace Parameters {
                export type ProjectId = string; // uuid
                export type RevisionId = string; // uuid
            }
            export interface PathParameters {
                projectId: Parameters.ProjectId /* uuid */;
                revisionId: Parameters.RevisionId /* uuid */;
            }
            namespace Responses {
                export interface $200 {
                }
            }
        }
        namespace Get {
            namespace Parameters {
                export type ProjectId = string; // uuid
                export type RevisionId = string; // uuid
            }
            export interface PathParameters {
                projectId: Parameters.ProjectId /* uuid */;
                revisionId: Parameters.RevisionId /* uuid */;
            }
            namespace Responses {
                export type $200 = Components.Schemas.RevisionDataDto;
            }
        }
        namespace Put {
            namespace Parameters {
                export type ProjectId = string; // uuid
                export type RevisionId = string; // uuid
            }
            export interface PathParameters {
                projectId: Parameters.ProjectId /* uuid */;
                revisionId: Parameters.RevisionId /* uuid */;
            }
            export type RequestBody = Components.Schemas.UpdateRevisionDto;
            namespace Responses {
                export type $200 = Components.Schemas.RevisionDataDto;
            }
        }
    }
    namespace Projects$ProjectIdWells {
        namespace Put {
            namespace Parameters {
                export type ProjectId = string; // uuid
            }
            export interface PathParameters {
                projectId: Parameters.ProjectId /* uuid */;
            }
            export type RequestBody = Components.Schemas.UpdateWellsDto;
            namespace Responses {
                export type $200 = Components.Schemas.ProjectDataDto;
            }
        }
    }
    namespace Projects$ProjectIdWells$WellIdIsInUse {
        namespace Get {
            namespace Parameters {
                export type ProjectId = string; // uuid
                export type WellId = string; // uuid
            }
            export interface PathParameters {
                projectId: Parameters.ProjectId /* uuid */;
                wellId: Parameters.WellId /* uuid */;
            }
            namespace Responses {
                export type $200 = boolean;
            }
        }
    }
    namespace ProjectsExists {
        namespace Get {
            namespace Parameters {
                export type ContextId = string; // uuid
            }
            export interface QueryParameters {
                contextId?: Parameters.ContextId /* uuid */;
            }
            namespace Responses {
                export type $200 = Components.Schemas.ProjectExistsDto;
            }
        }
    }
    namespace Stea$ProjectId {
        namespace Post {
            namespace Parameters {
                export type ProjectId = string; // uuid
            }
            export interface PathParameters {
                projectId: Parameters.ProjectId /* uuid */;
            }
            namespace Responses {
                export interface $200 {
                }
            }
        }
    }
    namespace Version {
        namespace Get {
            namespace Responses {
                export type $200 = string;
            }
        }
    }
    namespace Videos$VideoName {
        namespace Get {
            namespace Parameters {
                export type VideoName = string;
            }
            export interface PathParameters {
                videoName: Parameters.VideoName;
            }
            namespace Responses {
                export type $200 = Components.Schemas.VideoDto;
            }
        }
    }
}

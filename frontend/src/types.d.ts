declare namespace Components {
    namespace Schemas {
        export type ArtificialLift = 0 | 1 | 2 | 3; // int32
        export interface CampaignDto {
            campaignId: string; // uuid
            campaignType: string;
            rigUpgradingCost: number; // double
            rigMobDemobCost: number; // double
            rigUpgradingProfile: RigProfileDto;
            rigMobDemobProfile: RigProfileDto;
            campaignWells: CampaignWellDto[];
        }
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
            referenceCase: boolean;
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
            capexFactorFEEDStudies: number; // double
            host: string | null;
            dgaDate: string; // date-time
            dgbDate: string; // date-time
            dgcDate: string; // date-time
            apboDate: string; // date-time
            borDate: string; // date-time
            vpboDate: string; // date-time
            dG0Date: string; // date-time
            dG1Date: string; // date-time
            dG2Date: string; // date-time
            dG3Date: string; // date-time
            dG4Date: string; // date-time
            createTime: string; // date-time
            modifyTime: string; // date-time
            surfId: string; // uuid
            substructureId: string; // uuid
            topsideId: string; // uuid
            transportId: string; // uuid
            onshorePowerSupplyId: string; // uuid
            sharepointFileId: string | null;
            sharepointFileName: string | null;
            sharepointFileUrl: string | null;
        }
        export interface CaseWithAssetsDto {
            case: CaseOverviewDto;
            cessationWellsCost: TimeSeriesCostDto;
            cessationWellsCostOverride: TimeSeriesCostOverrideDto;
            cessationOffshoreFacilitiesCost: TimeSeriesCostDto;
            cessationOffshoreFacilitiesCostOverride: TimeSeriesCostOverrideDto;
            cessationOnshoreFacilitiesCostProfile: TimeSeriesCostDto;
            totalFeasibilityAndConceptStudies: TimeSeriesCostDto;
            totalFeasibilityAndConceptStudiesOverride: TimeSeriesCostOverrideDto;
            totalFEEDStudies: TimeSeriesCostDto;
            totalFEEDStudiesOverride: TimeSeriesCostOverrideDto;
            totalOtherStudiesCostProfile: TimeSeriesCostDto;
            historicCostCostProfile: TimeSeriesCostDto;
            wellInterventionCostProfile: TimeSeriesCostDto;
            wellInterventionCostProfileOverride: TimeSeriesCostOverrideDto;
            offshoreFacilitiesOperationsCostProfile: TimeSeriesCostDto;
            offshoreFacilitiesOperationsCostProfileOverride: TimeSeriesCostOverrideDto;
            onshoreRelatedOPEXCostProfile: TimeSeriesCostDto;
            additionalOPEXCostProfile: TimeSeriesCostDto;
            calculatedTotalIncomeCostProfile: TimeSeriesCostDto;
            calculatedTotalCostCostProfile: TimeSeriesCostDto;
            topside: TopsideDto;
            topsideCostProfile: TimeSeriesCostDto;
            topsideCostProfileOverride: TimeSeriesCostOverrideDto;
            topsideCessationCostProfile: TimeSeriesCostDto;
            drainageStrategy: DrainageStrategyDto;
            productionProfileOil: TimeSeriesCostDto;
            additionalProductionProfileOil: TimeSeriesCostDto;
            productionProfileGas: TimeSeriesCostDto;
            additionalProductionProfileGas: TimeSeriesCostDto;
            productionProfileWater: TimeSeriesCostDto;
            productionProfileWaterInjection: TimeSeriesCostDto;
            fuelFlaringAndLosses: TimeSeriesCostDto;
            fuelFlaringAndLossesOverride: TimeSeriesCostOverrideDto;
            netSalesGas: TimeSeriesCostDto;
            netSalesGasOverride: TimeSeriesCostOverrideDto;
            co2Emissions: TimeSeriesCostDto;
            co2EmissionsOverride: TimeSeriesCostOverrideDto;
            productionProfileNgl: TimeSeriesCostDto;
            importedElectricity: TimeSeriesCostDto;
            importedElectricityOverride: TimeSeriesCostOverrideDto;
            co2Intensity: TimeSeriesCostDto;
            deferredOilProduction: TimeSeriesCostDto;
            deferredGasProduction: TimeSeriesCostDto;
            substructure: SubstructureDto;
            substructureCostProfile: TimeSeriesCostDto;
            substructureCostProfileOverride: TimeSeriesCostOverrideDto;
            substructureCessationCostProfile: TimeSeriesCostDto;
            surf: SurfDto;
            surfCostProfile: TimeSeriesCostDto;
            surfCostProfileOverride: TimeSeriesCostOverrideDto;
            surfCessationCostProfile: TimeSeriesCostDto;
            transport: TransportDto;
            transportCostProfile: TimeSeriesCostDto;
            transportCostProfileOverride: TimeSeriesCostOverrideDto;
            transportCessationCostProfile: TimeSeriesCostDto;
            onshorePowerSupply: OnshorePowerSupplyDto;
            onshorePowerSupplyCostProfile: TimeSeriesCostDto;
            onshorePowerSupplyCostProfileOverride: TimeSeriesCostOverrideDto;
            explorationId: string; // uuid
            explorationWells: ExplorationWellDto[] | null;
            explorationCampaigns: CampaignDto[];
            explorationWellCostProfile: TimeSeriesCostDto;
            appraisalWellCostProfile: TimeSeriesCostDto;
            sidetrackCostProfile: TimeSeriesCostDto;
            gAndGAdminCost: TimeSeriesCostDto;
            gAndGAdminCostOverride: TimeSeriesCostOverrideDto;
            seismicAcquisitionAndProcessing: TimeSeriesCostDto;
            countryOfficeCost: TimeSeriesCostDto;
            projectSpecificDrillingCostProfile: TimeSeriesCostDto;
            explorationRigUpgradingCostProfile: TimeSeriesCostDto;
            explorationRigUpgradingCostProfileOverride: TimeSeriesCostOverrideDto;
            explorationRigMobDemob: TimeSeriesCostDto;
            explorationRigMobDemobOverride: TimeSeriesCostOverrideDto;
            wellProjectId: string; // uuid
            developmentWells: DevelopmentWellDto[] | null;
            developmentCampaigns: CampaignDto[];
            oilProducerCostProfile: TimeSeriesCostDto;
            oilProducerCostProfileOverride: TimeSeriesCostOverrideDto;
            gasProducerCostProfile: TimeSeriesCostDto;
            gasProducerCostProfileOverride: TimeSeriesCostOverrideDto;
            waterInjectorCostProfile: TimeSeriesCostDto;
            waterInjectorCostProfileOverride: TimeSeriesCostOverrideDto;
            gasInjectorCostProfile: TimeSeriesCostDto;
            gasInjectorCostProfileOverride: TimeSeriesCostOverrideDto;
            developmentRigUpgradingCostProfile: TimeSeriesCostDto;
            developmentRigUpgradingCostProfileOverride: TimeSeriesCostOverrideDto;
            developmentRigMobDemob: TimeSeriesCostDto;
            developmentRigMobDemobOverride: TimeSeriesCostOverrideDto;
        }
        export interface Co2DrillingFlaringFuelTotalsDto {
            co2Drilling: number; // double
            co2Fuel: number; // double
            co2Flaring: number; // double
        }
        export interface CommonProjectAndRevisionDto {
            modifyTime: string; // date-time
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
            cO2RemovedFromGas: number; // double
            cO2EmissionFromFuelGas: number; // double
            flaredGasPerProducedVolume: number; // double
            cO2EmissionsFromFlaredGas: number; // double
            cO2Vented: number; // double
            dailyEmissionFromDrillingRig: number; // double
            averageDevelopmentDrillingDays: number; // double
            oilPriceUSD: number; // double
            gasPriceNOK: number; // double
            discountRate: number; // double
            exchangeRateUSDToNOK: number; // double
            npvYear: number; // int32
            explorationOperationalWellCosts: ExplorationOperationalWellCostsOverviewDto;
            developmentOperationalWellCosts: DevelopmentOperationalWellCostsOverviewDto;
            cases: CaseOverviewDto[];
            wells: WellOverviewDto[];
            surfs: SurfOverviewDto[];
            substructures: SubstructureOverviewDto[];
            topsides: TopsideOverviewDto[];
            transports: TransportOverviewDto[];
            onshorePowerSupplies: OnshorePowerSupplyOverviewDto[];
            drainageStrategies: DrainageStrategyOverviewDto[];
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
        export interface CreateCaseDto {
            name: string;
            description: string;
            productionStrategyOverview: ProductionStrategyOverview /* int32 */;
            producerCount: number; // int32
            gasInjectorCount: number; // int32
            waterInjectorCount: number; // int32
            dG4Date: string; // date-time
        }
        export interface CreateProjectMemberDto {
            role: ProjectMemberRole /* int32 */;
            userId: string; // uuid
        }
        export interface CreateRevisionDto {
            name: string;
            internalProjectPhase: InternalProjectPhase /* int32 */;
            classification: ProjectClassification /* int32 */;
            arena: boolean;
            mdqc: boolean;
        }
        export interface CreateTimeSeriesScheduleDto {
            startYear: number; // int32
            values: number /* int32 */[];
        }
        export interface CreateWellDto {
            name: string;
            wellCategory: WellCategory /* int32 */;
            wellInterventionCost?: number; // double
            plugingAndAbandonmentCost?: number; // double
            wellCost?: number; // double
            drillingDays?: number; // double
        }
        export type Currency = 1 | 2; // int32
        export interface DeleteWellDto {
            id: string; // uuid
        }
        export interface DevelopmentOperationalWellCostsOverviewDto {
            developmentOperationalWellCostsId: string; // uuid
            projectId: string; // uuid
            rigUpgrading: number; // double
            rigMobDemob: number; // double
            annualWellInterventionCostPerWell: number; // double
            pluggingAndAbandonment: number; // double
        }
        export interface DevelopmentWellDto {
            drillingSchedule: TimeSeriesScheduleDto;
            wellProjectId: string; // uuid
            wellId: string; // uuid
            updatedUtc: string; // date-time
        }
        export interface DrainageStrategyDto {
            id: string; // uuid
            description: string;
            nglYield: number; // double
            producerCount: number; // int32
            gasInjectorCount: number; // int32
            waterInjectorCount: number; // int32
            artificialLift: ArtificialLift /* int32 */;
            gasSolution: GasSolution /* int32 */;
        }
        export interface DrainageStrategyOverviewDto {
            id: string; // uuid
        }
        export interface DriveItemDto {
            name?: string | null;
            id?: string | null;
            sharepointFileUrl?: string | null;
            createdDateTime?: string | null; // date-time
            content?: string | null; // binary
            size?: number | null; // int64
            sharepointIds?: SharepointIds;
            createdBy?: IdentitySet;
            lastModifiedBy?: IdentitySet;
            lastModifiedDateTime?: string | null; // date-time
        }
        export interface ExplorationOperationalWellCostsOverviewDto {
            explorationOperationalWellCostsId: string; // uuid
            projectId: string; // uuid
            explorationRigUpgrading: number; // double
            explorationRigMobDemob: number; // double
            explorationProjectDrillingCosts: number; // double
            appraisalRigMobDemob: number; // double
            appraisalProjectDrillingCosts: number; // double
        }
        export interface ExplorationWellDto {
            drillingSchedule: TimeSeriesScheduleDto;
            explorationId: string; // uuid
            wellId: string; // uuid
            updatedUtc: string; // date-time
        }
        export interface FeatureToggleDto {
            revisionEnabled?: boolean;
            environmentName?: string | null;
        }
        export type GasSolution = 0 | 1; // int32
        export interface Identity {
            [name: string]: any;
            displayName?: string | null;
            id?: string | null;
            "@odata.type"?: string | null;
        }
        export interface IdentitySet {
            [name: string]: any;
            application?: Identity;
            device?: Identity;
            user?: Identity;
            "@odata.type"?: string | null;
        }
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
            dG3Date: string | null; // date-time
            dG4Date: string | null; // date-time
        }
        export interface OnshorePowerSupplyOverviewDto {
            id: string; // uuid
            source: Source /* int32 */;
        }
        export type PhysUnit = 0 | 1; // int32
        export type ProductionFlowline = 0 | 1 | 2 | 3 | 4 | 5 | 6 | 7 | 8 | 9 | 10 | 11 | 12 | 13; // int32
        export type ProductionStrategyOverview = 0 | 1 | 2 | 3 | 4; // int32
        export type ProjectCategory = 0 | 1 | 2 | 3 | 4 | 5 | 6 | 7 | 8 | 9 | 10 | 11 | 12 | 13 | 14 | 15 | 16 | 17 | 18 | 19 | 20 | 21; // int32
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
            userId: string; // uuid
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
            startYear: number; // int32
            values: number /* int32 */[];
        }
        export interface SaveTimeSeriesDto {
            profileType: string;
            startYear: number; // int32
            values: number /* double */[];
        }
        export interface SaveTimeSeriesOverrideDto {
            profileType: string;
            startYear: number; // int32
            values: number /* double */[];
            override: boolean;
        }
        export interface SharePointImportDto {
            id?: string | null;
            surf?: boolean;
            substructure?: boolean;
            topside?: boolean;
            transport?: boolean;
            sharePointFileId?: string | null;
            sharePointFileName?: string | null;
            sharePointFileUrl?: string | null;
            sharePointSiteUrl?: string | null;
        }
        export interface SharepointIds {
            [name: string]: any;
            listId?: string | null;
            listItemId?: string | null;
            listItemUniqueId?: string | null;
            siteId?: string | null;
            siteUrl?: string | null;
            tenantId?: string | null;
            webId?: string | null;
            "@odata.type"?: string | null;
        }
        export type Source = 0 | 1; // int32
        export interface SubstructureDto {
            id: string; // uuid
            dryWeight: number; // double
            maturity: Maturity /* int32 */;
            currency: Currency /* int32 */;
            approvedBy: string;
            costYear: number; // int32
            prospVersion: string | null; // date-time
            source: Source /* int32 */;
            lastChangedDate: string | null; // date-time
            concept: Concept /* int32 */;
            dG3Date: string | null; // date-time
            dG4Date: string | null; // date-time
        }
        export interface SubstructureOverviewDto {
            id: string; // uuid
            source: Source /* int32 */;
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
            currency: Currency /* int32 */;
            lastChangedDate: string; // date-time
            costYear: number; // int32
            source: Source /* int32 */;
            prospVersion: string | null; // date-time
            approvedBy: string;
            dG3Date: string | null; // date-time
            dG4Date: string | null; // date-time
        }
        export interface SurfOverviewDto {
            id: string; // uuid
            maturity: Maturity /* int32 */;
            source: Source /* int32 */;
        }
        export interface TimeSeriesCostDto {
            id: string; // uuid
            startYear: number; // int32
            values: number /* double */[];
            updatedUtc: string; // date-time
        }
        export interface TimeSeriesCostOverrideDto {
            id: string; // uuid
            startYear: number; // int32
            values: number /* double */[];
            override: boolean;
            updatedUtc: string; // date-time
        }
        export interface TimeSeriesScheduleDto {
            id: string; // uuid
            startYear: number; // int32
            values: number /* int32 */[];
        }
        export interface TopsideDto {
            id: string; // uuid
            dryWeight: number; // double
            oilCapacity: number; // double
            gasCapacity: number; // double
            waterInjectionCapacity: number; // double
            artificialLift: ArtificialLift /* int32 */;
            maturity: Maturity /* int32 */;
            currency: Currency /* int32 */;
            fuelConsumption: number; // double
            flaredGas: number; // double
            producerCount: number; // int32
            gasInjectorCount: number; // int32
            waterInjectorCount: number; // int32
            cO2ShareOilProfile: number; // double
            cO2ShareGasProfile: number; // double
            cO2ShareWaterInjectionProfile: number; // double
            cO2OnMaxOilProfile: number; // double
            cO2OnMaxGasProfile: number; // double
            cO2OnMaxWaterInjectionProfile: number; // double
            costYear: number; // int32
            prospVersion: string | null; // date-time
            lastChangedDate: string | null; // date-time
            source: Source /* int32 */;
            approvedBy: string;
            dG3Date: string | null; // date-time
            dG4Date: string | null; // date-time
            facilityOpex: number; // double
            peakElectricityImported: number; // double
        }
        export interface TopsideOverviewDto {
            id: string; // uuid
            fuelConsumption: number; // double
            cO2ShareOilProfile: number; // double
            cO2ShareGasProfile: number; // double
            cO2ShareWaterInjectionProfile: number; // double
            cO2OnMaxOilProfile: number; // double
            cO2OnMaxGasProfile: number; // double
            cO2OnMaxWaterInjectionProfile: number; // double
            source: Source /* int32 */;
        }
        export interface TransportDto {
            id: string; // uuid
            maturity: Maturity /* int32 */;
            gasExportPipelineLength: number; // double
            oilExportPipelineLength: number; // double
            currency: Currency /* int32 */;
            lastChangedDate: string | null; // date-time
            costYear: number; // int32
            source: Source /* int32 */;
            prospVersion: string | null; // date-time
            dG3Date: string | null; // date-time
            dG4Date: string | null; // date-time
        }
        export interface TransportOverviewDto {
            id: string; // uuid
            source: Source /* int32 */;
        }
        export interface UpdateCampaignDto {
            rigUpgradingCost: number; // double
            rigUpgradingCostStartYear: number; // int32
            rigUpgradingCostValues: number /* double */[];
            rigMobDemobCost: number; // double
            rigMobDemobCostStartYear: number; // int32
            rigMobDemobCostValues: number /* double */[];
        }
        export interface UpdateCaseDto {
            name: string;
            description: string;
            referenceCase: boolean;
            archived: boolean;
            artificialLift: ArtificialLift /* int32 */;
            productionStrategyOverview: ProductionStrategyOverview /* int32 */;
            producerCount: number; // int32
            gasInjectorCount: number; // int32
            waterInjectorCount: number; // int32
            facilitiesAvailability: number; // double
            capexFactorFeasibilityStudies: number; // double
            capexFactorFEEDStudies: number; // double
            npv: number; // double
            npvOverride: number | null; // double
            breakEven: number; // double
            breakEvenOverride: number | null; // double
            host: string | null;
            dgaDate: string; // date-time
            dgbDate: string; // date-time
            dgcDate: string; // date-time
            apboDate: string; // date-time
            borDate: string; // date-time
            vpboDate: string; // date-time
            dG0Date: string; // date-time
            dG1Date: string; // date-time
            dG2Date: string; // date-time
            dG3Date: string; // date-time
            dG4Date: string; // date-time
            sharepointFileId: string | null;
            sharepointFileName: string | null;
            sharepointFileUrl: string | null;
        }
        export interface UpdateDevelopmentOperationalWellCostsDto {
            rigUpgrading?: number; // double
            rigMobDemob?: number; // double
            annualWellInterventionCostPerWell?: number; // double
            pluggingAndAbandonment?: number; // double
        }
        export interface UpdateDrainageStrategyDto {
            nglYield?: number; // double
            producerCount?: number; // int32
            gasInjectorCount?: number; // int32
            waterInjectorCount?: number; // int32
            artificialLift?: ArtificialLift /* int32 */;
            gasSolution?: GasSolution /* int32 */;
        }
        export interface UpdateExplorationOperationalWellCostsDto {
            explorationRigUpgrading?: number; // double
            explorationRigMobDemob?: number; // double
            explorationProjectDrillingCosts?: number; // double
            appraisalRigMobDemob?: number; // double
            appraisalProjectDrillingCosts?: number; // double
        }
        export interface UpdateImageDto {
            description: string | null;
        }
        export interface UpdateOnshorePowerSupplyDto {
            costYear?: number; // int32
            dG3Date?: string | null; // date-time
            dG4Date?: string | null; // date-time
            source?: Source /* int32 */;
        }
        export interface UpdateProjectDto {
            name: string;
            referenceCaseId: string; // uuid
            description: string;
            country: string;
            currency: Currency /* int32 */;
            physicalUnit: PhysUnit /* int32 */;
            classification: ProjectClassification /* int32 */;
            projectPhase: ProjectPhase /* int32 */;
            internalProjectPhase: InternalProjectPhase /* int32 */;
            projectCategory: ProjectCategory /* int32 */;
            sharepointSiteUrl: string | null;
            cO2RemovedFromGas: number; // double
            cO2EmissionFromFuelGas: number; // double
            flaredGasPerProducedVolume: number; // double
            cO2EmissionsFromFlaredGas: number; // double
            cO2Vented: number; // double
            dailyEmissionFromDrillingRig: number; // double
            averageDevelopmentDrillingDays: number; // double
            oilPriceUSD: number; // double
            gasPriceNOK: number; // double
            discountRate: number; // double
            exchangeRateUSDToNOK: number; // double
            npvYear: number; // int32
        }
        export interface UpdateProjectMemberDto {
            role: ProjectMemberRole /* int32 */;
            userId: string; // uuid
        }
        export interface UpdateRevisionDto {
            name: string;
            arena: boolean;
            mdqc: boolean;
        }
        export interface UpdateSubstructureDto {
            dryWeight?: number; // double
            currency?: Currency /* int32 */;
            costYear?: number; // int32
            source?: Source /* int32 */;
            concept?: Concept /* int32 */;
            dG3Date?: string | null; // date-time
            dG4Date?: string | null; // date-time
            maturity?: Maturity /* int32 */;
            approvedBy?: string | null;
        }
        export interface UpdateSurfDto {
            cessationCost?: number; // double
            infieldPipelineSystemLength?: number; // double
            umbilicalSystemLength?: number; // double
            artificialLift?: ArtificialLift /* int32 */;
            riserCount?: number; // int32
            templateCount?: number; // int32
            producerCount?: number; // int32
            gasInjectorCount?: number; // int32
            waterInjectorCount?: number; // int32
            productionFlowline?: ProductionFlowline /* int32 */;
            currency?: Currency /* int32 */;
            costYear?: number; // int32
            source?: Source /* int32 */;
            approvedBy?: string | null;
            dG3Date?: string | null; // date-time
            dG4Date?: string | null; // date-time
            maturity?: Maturity /* int32 */;
        }
        export interface UpdateTimeSeriesScheduleDto {
            startYear: number; // int32
            values: number /* int32 */[];
        }
        export interface UpdateTopsideDto {
            dryWeight?: number; // double
            oilCapacity?: number; // double
            gasCapacity?: number; // double
            waterInjectionCapacity?: number; // double
            artificialLift?: ArtificialLift /* int32 */;
            currency?: Currency /* int32 */;
            fuelConsumption?: number; // double
            flaredGas?: number; // double
            producerCount?: number; // int32
            gasInjectorCount?: number; // int32
            waterInjectorCount?: number; // int32
            cO2ShareOilProfile?: number; // double
            cO2ShareGasProfile?: number; // double
            cO2ShareWaterInjectionProfile?: number; // double
            cO2OnMaxOilProfile?: number; // double
            cO2OnMaxGasProfile?: number; // double
            cO2OnMaxWaterInjectionProfile?: number; // double
            costYear?: number; // int32
            dG3Date?: string | null; // date-time
            dG4Date?: string | null; // date-time
            facilityOpex?: number; // double
            peakElectricityImported?: number; // double
            source?: Source /* int32 */;
            maturity?: Maturity /* int32 */;
            approvedBy?: string | null;
        }
        export interface UpdateTransportDto {
            gasExportPipelineLength?: number; // double
            oilExportPipelineLength?: number; // double
            currency?: Currency /* int32 */;
            costYear?: number; // int32
            dG3Date?: string | null; // date-time
            dG4Date?: string | null; // date-time
            source?: Source /* int32 */;
            maturity?: Maturity /* int32 */;
        }
        export interface UpdateWellDto {
            id: string; // uuid
            name?: string | null;
            wellInterventionCost?: number; // double
            plugingAndAbandonmentCost?: number; // double
            wellCategory?: WellCategory /* int32 */;
            wellCost?: number; // double
            drillingDays?: number; // double
        }
        export interface UpdateWellsDto {
            updateWellDtos: UpdateWellDto[];
            createWellDtos: CreateWellDto[];
            deleteWellDtos: DeleteWellDto[];
        }
        export interface UrlDto {
            url?: string | null;
        }
        export interface UserActionsDto {
            canView: boolean;
            canCreateRevision: boolean;
            canEditProjectData: boolean;
            canEditProjectMembers: boolean;
        }
        export type WellCategory = 0 | 1 | 2 | 3 | 4 | 5 | 6 | 7; // int32
        export interface WellOverviewDto {
            id: string; // uuid
            name: string;
            wellCategory: WellCategory /* int32 */;
            wellCost: number; // double
            drillingDays: number; // double
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
    namespace GetSharePointFileNamesAndId {
        namespace Parameters {
            export type ProjectId = string; // uuid
        }
        export interface PathParameters {
            projectId: Parameters.ProjectId /* uuid */;
        }
        export type RequestBody = Components.Schemas.UrlDto;
        namespace Responses {
            export type $200 = Components.Schemas.DriveItemDto[];
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
    namespace Projects$ProjectIdCases$CaseIdCampaigns$CampaignId {
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
    namespace Projects$ProjectIdCases$CaseIdCampaigns$CampaignIdWells$WellIdSave {
        namespace Post {
            namespace Parameters {
                export type CampaignId = string; // uuid
                export type CaseId = string; // uuid
                export type ProjectId = string; // uuid
                export type WellId = string; // uuid
            }
            export interface PathParameters {
                projectId: Parameters.ProjectId /* uuid */;
                caseId: Parameters.CaseId /* uuid */;
                campaignId: Parameters.CampaignId /* uuid */;
                wellId: Parameters.WellId /* uuid */;
            }
            export type RequestBody = Components.Schemas.SaveCampaignWellDto;
            namespace Responses {
                export type $200 = Components.Schemas.CampaignWellDto;
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
    namespace Projects$ProjectIdCases$CaseIdCo2drillingflaringfueltotals {
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
    namespace Projects$ProjectIdCases$CaseIdExplorations$ExplorationIdWells$WellIdDrillingSchedule {
        namespace Post {
            namespace Parameters {
                export type CaseId = string; // uuid
                export type ExplorationId = string; // uuid
                export type ProjectId = string; // uuid
                export type WellId = string; // uuid
            }
            export interface PathParameters {
                projectId: Parameters.ProjectId /* uuid */;
                caseId: Parameters.CaseId /* uuid */;
                explorationId: Parameters.ExplorationId /* uuid */;
                wellId: Parameters.WellId /* uuid */;
            }
            export type RequestBody = Components.Schemas.CreateTimeSeriesScheduleDto;
            namespace Responses {
                export type $200 = Components.Schemas.TimeSeriesScheduleDto;
            }
        }
    }
    namespace Projects$ProjectIdCases$CaseIdExplorations$ExplorationIdWells$WellIdDrillingSchedule$DrillingScheduleId {
        namespace Put {
            namespace Parameters {
                export type CaseId = string; // uuid
                export type DrillingScheduleId = string; // uuid
                export type ExplorationId = string; // uuid
                export type ProjectId = string; // uuid
                export type WellId = string; // uuid
            }
            export interface PathParameters {
                projectId: Parameters.ProjectId /* uuid */;
                caseId: Parameters.CaseId /* uuid */;
                explorationId: Parameters.ExplorationId /* uuid */;
                wellId: Parameters.WellId /* uuid */;
                drillingScheduleId: Parameters.DrillingScheduleId /* uuid */;
            }
            export type RequestBody = Components.Schemas.UpdateTimeSeriesScheduleDto;
            namespace Responses {
                export type $200 = Components.Schemas.TimeSeriesScheduleDto;
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
    namespace Projects$ProjectIdCases$CaseIdOverrideProfilesSave {
        namespace Post {
            namespace Parameters {
                export type CaseId = string; // uuid
                export type ProjectId = string; // uuid
            }
            export interface PathParameters {
                projectId: Parameters.ProjectId /* uuid */;
                caseId: Parameters.CaseId /* uuid */;
            }
            export type RequestBody = Components.Schemas.SaveTimeSeriesOverrideDto;
            namespace Responses {
                export type $200 = Components.Schemas.TimeSeriesCostOverrideDto;
            }
        }
    }
    namespace Projects$ProjectIdCases$CaseIdProfilesSave {
        namespace Post {
            namespace Parameters {
                export type CaseId = string; // uuid
                export type ProjectId = string; // uuid
            }
            export interface PathParameters {
                projectId: Parameters.ProjectId /* uuid */;
                caseId: Parameters.CaseId /* uuid */;
            }
            export type RequestBody = Components.Schemas.SaveTimeSeriesDto;
            namespace Responses {
                export type $200 = Components.Schemas.TimeSeriesCostDto;
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
    namespace Projects$ProjectIdCases$CaseIdWellProjects$WellProjectIdWells$WellIdDrillingSchedule {
        namespace Post {
            namespace Parameters {
                export type CaseId = string; // uuid
                export type ProjectId = string; // uuid
                export type WellId = string; // uuid
                export type WellProjectId = string; // uuid
            }
            export interface PathParameters {
                projectId: Parameters.ProjectId /* uuid */;
                caseId: Parameters.CaseId /* uuid */;
                wellProjectId: Parameters.WellProjectId /* uuid */;
                wellId: Parameters.WellId /* uuid */;
            }
            export type RequestBody = Components.Schemas.CreateTimeSeriesScheduleDto;
            namespace Responses {
                export type $200 = Components.Schemas.TimeSeriesScheduleDto;
            }
        }
    }
    namespace Projects$ProjectIdCases$CaseIdWellProjects$WellProjectIdWells$WellIdDrillingSchedule$DrillingScheduleId {
        namespace Put {
            namespace Parameters {
                export type CaseId = string; // uuid
                export type DrillingScheduleId = string; // uuid
                export type ProjectId = string; // uuid
                export type WellId = string; // uuid
                export type WellProjectId = string; // uuid
            }
            export interface PathParameters {
                projectId: Parameters.ProjectId /* uuid */;
                caseId: Parameters.CaseId /* uuid */;
                wellProjectId: Parameters.WellProjectId /* uuid */;
                wellId: Parameters.WellId /* uuid */;
                drillingScheduleId: Parameters.DrillingScheduleId /* uuid */;
            }
            export type RequestBody = Components.Schemas.UpdateTimeSeriesScheduleDto;
            namespace Responses {
                export type $200 = Components.Schemas.TimeSeriesScheduleDto;
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
    namespace Projects$ProjectIdDevelopmentOperationalWellCosts$DevelopmentOperationalWellCostsId {
        namespace Put {
            namespace Parameters {
                export type DevelopmentOperationalWellCostsId = string; // uuid
                export type ProjectId = string; // uuid
            }
            export interface PathParameters {
                projectId: Parameters.ProjectId /* uuid */;
                developmentOperationalWellCostsId: Parameters.DevelopmentOperationalWellCostsId /* uuid */;
            }
            export type RequestBody = Components.Schemas.UpdateDevelopmentOperationalWellCostsDto;
            namespace Responses {
                export type $200 = Components.Schemas.DevelopmentOperationalWellCostsOverviewDto;
            }
        }
    }
    namespace Projects$ProjectIdExplorationOperationalWellCosts$ExplorationOperationalWellCostsId {
        namespace Put {
            namespace Parameters {
                export type ExplorationOperationalWellCostsId = string; // uuid
                export type ProjectId = string; // uuid
            }
            export interface PathParameters {
                projectId: Parameters.ProjectId /* uuid */;
                explorationOperationalWellCostsId: Parameters.ExplorationOperationalWellCostsId /* uuid */;
            }
            export type RequestBody = Components.Schemas.UpdateExplorationOperationalWellCostsDto;
            namespace Responses {
                export type $200 = Components.Schemas.ExplorationOperationalWellCostsOverviewDto;
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
    namespace Prosp$ProjectIdSharepoint {
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
}

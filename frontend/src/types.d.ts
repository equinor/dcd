declare namespace Components {
    namespace Schemas {
        export type ArtificialLift = 0 | 1 | 2 | 3; // int32
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
            surfLink: string; // uuid
            substructureLink: string; // uuid
            topsideLink: string; // uuid
            transportLink: string; // uuid
            onshorePowerSupplyLink: string; // uuid
            sharepointFileId: string | null;
            sharepointFileName: string | null;
            sharepointFileUrl: string | null;
        }
        export interface CaseWithAssetsDto {
            case: CaseOverviewDto;
            cessationWellsCost?: TimeSeriesCostDto;
            cessationWellsCostOverride?: TimeSeriesCostOverrideDto;
            cessationOffshoreFacilitiesCost?: TimeSeriesCostDto;
            cessationOffshoreFacilitiesCostOverride?: TimeSeriesCostOverrideDto;
            cessationOnshoreFacilitiesCostProfile?: TimeSeriesCostDto;
            totalFeasibilityAndConceptStudies?: TimeSeriesCostDto;
            totalFeasibilityAndConceptStudiesOverride?: TimeSeriesCostOverrideDto;
            totalFEEDStudies?: TimeSeriesCostDto;
            totalFEEDStudiesOverride?: TimeSeriesCostOverrideDto;
            totalOtherStudiesCostProfile?: TimeSeriesCostDto;
            historicCostCostProfile?: TimeSeriesCostDto;
            wellInterventionCostProfile?: TimeSeriesCostDto;
            wellInterventionCostProfileOverride?: TimeSeriesCostOverrideDto;
            offshoreFacilitiesOperationsCostProfile?: TimeSeriesCostDto;
            offshoreFacilitiesOperationsCostProfileOverride?: TimeSeriesCostOverrideDto;
            onshoreRelatedOPEXCostProfile?: TimeSeriesCostDto;
            additionalOPEXCostProfile?: TimeSeriesCostDto;
            calculatedTotalIncomeCostProfile?: TimeSeriesCostDto;
            calculatedTotalCostCostProfile?: TimeSeriesCostDto;
            topside: TopsideDto;
            topsideCostProfile?: TimeSeriesCostDto;
            topsideCostProfileOverride?: TimeSeriesCostOverrideDto;
            topsideCessationCostProfile?: TimeSeriesCostDto;
            drainageStrategy: DrainageStrategyDto;
            productionProfileOil?: TimeSeriesVolumeDto;
            additionalProductionProfileOil?: TimeSeriesVolumeDto;
            productionProfileGas?: TimeSeriesVolumeDto;
            additionalProductionProfileGas?: TimeSeriesVolumeDto;
            productionProfileWater?: TimeSeriesVolumeDto;
            productionProfileWaterInjection?: TimeSeriesVolumeDto;
            fuelFlaringAndLosses?: TimeSeriesVolumeDto;
            fuelFlaringAndLossesOverride?: TimeSeriesVolumeOverrideDto;
            netSalesGas?: TimeSeriesVolumeDto;
            netSalesGasOverride?: TimeSeriesVolumeOverrideDto;
            co2Emissions?: TimeSeriesMassDto;
            co2EmissionsOverride?: TimeSeriesMassOverrideDto;
            productionProfileNgl?: TimeSeriesVolumeDto;
            importedElectricity?: TimeSeriesEnergyDto;
            importedElectricityOverride?: TimeSeriesEnergyOverrideDto;
            co2Intensity?: TimeSeriesMassDto;
            deferredOilProduction?: TimeSeriesVolumeDto;
            deferredGasProduction?: TimeSeriesVolumeDto;
            substructure: SubstructureDto;
            substructureCostProfile?: TimeSeriesCostDto;
            substructureCostProfileOverride?: TimeSeriesCostOverrideDto;
            substructureCessationCostProfile?: TimeSeriesCostDto;
            surf: SurfDto;
            surfCostProfile?: TimeSeriesCostDto;
            surfCostProfileOverride?: TimeSeriesCostOverrideDto;
            surfCessationCostProfile?: TimeSeriesCostDto;
            transport: TransportDto;
            transportCostProfile?: TimeSeriesCostDto;
            transportCostProfileOverride?: TimeSeriesCostOverrideDto;
            transportCessationCostProfile?: TimeSeriesCostDto;
            onshorePowerSupply: OnshorePowerSupplyDto;
            onshorePowerSupplyCostProfile?: TimeSeriesCostDto;
            onshorePowerSupplyCostProfileOverride?: TimeSeriesCostOverrideDto;
            exploration: ExplorationDto;
            explorationWells?: ExplorationWellDto[] | null;
            explorationWellCostProfile?: TimeSeriesCostDto;
            appraisalWellCostProfile?: TimeSeriesCostDto;
            sidetrackCostProfile?: TimeSeriesCostDto;
            gAndGAdminCost?: TimeSeriesCostDto;
            gAndGAdminCostOverride?: TimeSeriesCostOverrideDto;
            seismicAcquisitionAndProcessing?: TimeSeriesCostDto;
            countryOfficeCost?: TimeSeriesCostDto;
            wellProject: WellProjectDto;
            wellProjectWells?: WellProjectWellDto[] | null;
            oilProducerCostProfile?: TimeSeriesCostDto;
            oilProducerCostProfileOverride?: TimeSeriesCostOverrideDto;
            gasProducerCostProfile?: TimeSeriesCostDto;
            gasProducerCostProfileOverride?: TimeSeriesCostOverrideDto;
            waterInjectorCostProfile?: TimeSeriesCostDto;
            waterInjectorCostProfileOverride?: TimeSeriesCostOverrideDto;
            gasInjectorCostProfile?: TimeSeriesCostDto;
            gasInjectorCostProfileOverride?: TimeSeriesCostOverrideDto;
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
        export interface CreateTimeSeriesCostDto {
            startYear: number; // int32
            values: number /* double */[];
            currency: Currency /* int32 */;
        }
        export interface CreateTimeSeriesCostOverrideDto {
            startYear: number; // int32
            values: number /* double */[];
            currency: Currency /* int32 */;
            override: boolean;
        }
        export interface CreateTimeSeriesEnergyDto {
            startYear: number; // int32
            values: number /* double */[];
        }
        export interface CreateTimeSeriesMassOverrideDto {
            startYear: number; // int32
            values: number /* double */[];
            override: boolean;
        }
        export interface CreateTimeSeriesScheduleDto {
            startYear: number; // int32
            values: number /* int32 */[];
        }
        export interface CreateTimeSeriesVolumeDto {
            startYear: number; // int32
            values: number /* double */[];
        }
        export interface CreateTimeSeriesVolumeOverrideDto {
            startYear: number; // int32
            values: number /* double */[];
            override: boolean;
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
        export interface DrainageStrategyDto {
            id: string; // uuid
            projectId: string; // uuid
            name: string;
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
        export interface ExceptionDto {
            details: {
                [name: string]: string;
            } | null;
        }
        export interface ExplorationDto {
            id: string; // uuid
            projectId: string; // uuid
            name: string;
            rigMobDemob: number; // double
            currency: Currency /* int32 */;
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
        export interface OnshorePowerSupplyDto {
            id: string; // uuid
            name: string;
            projectId: string; // uuid
            lastChangedDate?: string | null; // date-time
            costYear: number; // int32
            source: Source /* int32 */;
            prospVersion?: string | null; // date-time
            dG3Date?: string | null; // date-time
            dG4Date?: string | null; // date-time
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
            name: string;
            projectId: string; // uuid
            dryWeight: number; // double
            maturity: Maturity /* int32 */;
            currency: Currency /* int32 */;
            approvedBy: string;
            costYear: number; // int32
            prospVersion?: string | null; // date-time
            source: Source /* int32 */;
            lastChangedDate?: string | null; // date-time
            concept: Concept /* int32 */;
            dG3Date?: string | null; // date-time
            dG4Date?: string | null; // date-time
        }
        export interface SubstructureOverviewDto {
            id: string; // uuid
            source: Source /* int32 */;
        }
        export interface SurfDto {
            id: string; // uuid
            name: string;
            projectId: string; // uuid
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
            prospVersion?: string | null; // date-time
            approvedBy: string;
            dG3Date?: string | null; // date-time
            dG4Date?: string | null; // date-time
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
            sum: number; // double
            currency: Currency /* int32 */;
        }
        export interface TimeSeriesCostOverrideDto {
            id: string; // uuid
            startYear: number; // int32
            values: number /* double */[];
            sum: number; // double
            currency: Currency /* int32 */;
            override: boolean;
        }
        export interface TimeSeriesEnergyDto {
            id: string; // uuid
            startYear: number; // int32
            values: number /* double */[];
            sum: number; // double
        }
        export interface TimeSeriesEnergyOverrideDto {
            id: string; // uuid
            startYear: number; // int32
            values: number /* double */[];
            sum: number; // double
            override: boolean;
        }
        export interface TimeSeriesMassDto {
            id: string; // uuid
            startYear: number; // int32
            values: number /* double */[];
            sum: number; // double
        }
        export interface TimeSeriesMassOverrideDto {
            id: string; // uuid
            startYear: number; // int32
            values: number /* double */[];
            sum: number; // double
            override: boolean;
        }
        export interface TimeSeriesScheduleDto {
            id: string; // uuid
            startYear: number; // int32
            values: number /* int32 */[];
        }
        export interface TimeSeriesVolumeDto {
            id: string; // uuid
            startYear: number; // int32
            values: number /* double */[];
            sum: number; // double
        }
        export interface TimeSeriesVolumeOverrideDto {
            id: string; // uuid
            startYear: number; // int32
            values: number /* double */[];
            sum: number; // double
            override: boolean;
        }
        export interface TopsideDto {
            id: string; // uuid
            name: string;
            projectId: string; // uuid
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
            prospVersion?: string | null; // date-time
            lastChangedDate?: string | null; // date-time
            source: Source /* int32 */;
            approvedBy: string;
            dG3Date?: string | null; // date-time
            dG4Date?: string | null; // date-time
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
            name: string;
            projectId: string; // uuid
            maturity: Maturity /* int32 */;
            gasExportPipelineLength: number; // double
            oilExportPipelineLength: number; // double
            currency: Currency /* int32 */;
            lastChangedDate?: string | null; // date-time
            costYear: number; // int32
            source: Source /* int32 */;
            prospVersion?: string | null; // date-time
            dG3Date?: string | null; // date-time
            dG4Date?: string | null; // date-time
        }
        export interface TransportOverviewDto {
            id: string; // uuid
            source: Source /* int32 */;
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
        export interface UpdateExplorationDto {
            rigMobDemob?: number; // double
            currency?: Currency /* int32 */;
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
        export interface UpdateTimeSeriesCostDto {
            startYear: number; // int32
            values: number /* double */[];
            currency: Currency /* int32 */;
        }
        export interface UpdateTimeSeriesCostOverrideDto {
            startYear: number; // int32
            values: number /* double */[];
            currency: Currency /* int32 */;
            override: boolean;
        }
        export interface UpdateTimeSeriesEnergyOverrideDto {
            startYear: number; // int32
            values: number /* double */[];
            override: boolean;
        }
        export interface UpdateTimeSeriesMassOverrideDto {
            startYear: number; // int32
            values: number /* double */[];
            override: boolean;
        }
        export interface UpdateTimeSeriesScheduleDto {
            startYear: number; // int32
            values: number /* int32 */[];
        }
        export interface UpdateTimeSeriesVolumeDto {
            startYear: number; // int32
            values: number /* double */[];
        }
        export interface UpdateTimeSeriesVolumeOverrideDto {
            startYear: number; // int32
            values: number /* double */[];
            override: boolean;
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
        export interface UpdateWellProjectDto {
            name?: string | null;
            artificialLift?: ArtificialLift /* int32 */;
            currency?: Currency /* int32 */;
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
        export interface WellProjectDto {
            id: string; // uuid
            projectId: string; // uuid
            name: string;
            artificialLift: ArtificialLift /* int32 */;
            currency: Currency /* int32 */;
        }
        export interface WellProjectWellDto {
            drillingSchedule: TimeSeriesScheduleDto;
            wellProjectId: string; // uuid
            wellId: string; // uuid
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
    namespace LogException {
        namespace Post {
            export type RequestBody = Components.Schemas.ExceptionDto;
            namespace Responses {
                export interface $200 {
                }
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
    namespace Projects$ProjectIdCases$CaseIdAdditionalOpexCostProfile {
        namespace Post {
            namespace Parameters {
                export type CaseId = string; // uuid
                export type ProjectId = string; // uuid
            }
            export interface PathParameters {
                projectId: Parameters.ProjectId /* uuid */;
                caseId: Parameters.CaseId /* uuid */;
            }
            export type RequestBody = Components.Schemas.CreateTimeSeriesCostDto;
            namespace Responses {
                export type $200 = Components.Schemas.TimeSeriesCostDto;
            }
        }
    }
    namespace Projects$ProjectIdCases$CaseIdAdditionalOpexCostProfile$CostProfileId {
        namespace Put {
            namespace Parameters {
                export type CaseId = string; // uuid
                export type CostProfileId = string; // uuid
                export type ProjectId = string; // uuid
            }
            export interface PathParameters {
                projectId: Parameters.ProjectId /* uuid */;
                caseId: Parameters.CaseId /* uuid */;
                costProfileId: Parameters.CostProfileId /* uuid */;
            }
            export type RequestBody = Components.Schemas.UpdateTimeSeriesCostDto;
            namespace Responses {
                export type $200 = Components.Schemas.TimeSeriesCostDto;
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
    namespace Projects$ProjectIdCases$CaseIdCessationOffshoreFacilitiesCostOverride {
        namespace Post {
            namespace Parameters {
                export type CaseId = string; // uuid
                export type ProjectId = string; // uuid
            }
            export interface PathParameters {
                projectId: Parameters.ProjectId /* uuid */;
                caseId: Parameters.CaseId /* uuid */;
            }
            export type RequestBody = Components.Schemas.CreateTimeSeriesCostOverrideDto;
            namespace Responses {
                export type $200 = Components.Schemas.TimeSeriesCostOverrideDto;
            }
        }
    }
    namespace Projects$ProjectIdCases$CaseIdCessationOffshoreFacilitiesCostOverride$CostProfileId {
        namespace Put {
            namespace Parameters {
                export type CaseId = string; // uuid
                export type CostProfileId = string; // uuid
                export type ProjectId = string; // uuid
            }
            export interface PathParameters {
                projectId: Parameters.ProjectId /* uuid */;
                caseId: Parameters.CaseId /* uuid */;
                costProfileId: Parameters.CostProfileId /* uuid */;
            }
            export type RequestBody = Components.Schemas.UpdateTimeSeriesCostOverrideDto;
            namespace Responses {
                export type $200 = Components.Schemas.TimeSeriesCostOverrideDto;
            }
        }
    }
    namespace Projects$ProjectIdCases$CaseIdCessationOnshoreFacilitiesCostProfile {
        namespace Post {
            namespace Parameters {
                export type CaseId = string; // uuid
                export type ProjectId = string; // uuid
            }
            export interface PathParameters {
                projectId: Parameters.ProjectId /* uuid */;
                caseId: Parameters.CaseId /* uuid */;
            }
            export type RequestBody = Components.Schemas.CreateTimeSeriesCostDto;
            namespace Responses {
                export type $200 = Components.Schemas.TimeSeriesCostDto;
            }
        }
    }
    namespace Projects$ProjectIdCases$CaseIdCessationOnshoreFacilitiesCostProfile$CostProfileId {
        namespace Put {
            namespace Parameters {
                export type CaseId = string; // uuid
                export type CostProfileId = string; // uuid
                export type ProjectId = string; // uuid
            }
            export interface PathParameters {
                projectId: Parameters.ProjectId /* uuid */;
                caseId: Parameters.CaseId /* uuid */;
                costProfileId: Parameters.CostProfileId /* uuid */;
            }
            export type RequestBody = Components.Schemas.UpdateTimeSeriesCostDto;
            namespace Responses {
                export type $200 = Components.Schemas.TimeSeriesCostDto;
            }
        }
    }
    namespace Projects$ProjectIdCases$CaseIdCessationWellsCostOverride {
        namespace Post {
            namespace Parameters {
                export type CaseId = string; // uuid
                export type ProjectId = string; // uuid
            }
            export interface PathParameters {
                projectId: Parameters.ProjectId /* uuid */;
                caseId: Parameters.CaseId /* uuid */;
            }
            export type RequestBody = Components.Schemas.CreateTimeSeriesCostOverrideDto;
            namespace Responses {
                export type $200 = Components.Schemas.TimeSeriesCostOverrideDto;
            }
        }
    }
    namespace Projects$ProjectIdCases$CaseIdCessationWellsCostOverride$CostProfileId {
        namespace Put {
            namespace Parameters {
                export type CaseId = string; // uuid
                export type CostProfileId = string; // uuid
                export type ProjectId = string; // uuid
            }
            export interface PathParameters {
                projectId: Parameters.ProjectId /* uuid */;
                caseId: Parameters.CaseId /* uuid */;
                costProfileId: Parameters.CostProfileId /* uuid */;
            }
            export type RequestBody = Components.Schemas.UpdateTimeSeriesCostOverrideDto;
            namespace Responses {
                export type $200 = Components.Schemas.TimeSeriesCostOverrideDto;
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
    namespace Projects$ProjectIdCases$CaseIdDrainageStrategies$DrainageStrategyId {
        namespace Put {
            namespace Parameters {
                export type CaseId = string; // uuid
                export type DrainageStrategyId = string; // uuid
                export type ProjectId = string; // uuid
            }
            export interface PathParameters {
                projectId: Parameters.ProjectId /* uuid */;
                caseId: Parameters.CaseId /* uuid */;
                drainageStrategyId: Parameters.DrainageStrategyId /* uuid */;
            }
            export type RequestBody = Components.Schemas.UpdateDrainageStrategyDto;
            namespace Responses {
                export interface $200 {
                }
            }
        }
    }
    namespace Projects$ProjectIdCases$CaseIdDrainageStrategies$DrainageStrategyIdAdditionalProductionProfileGas {
        namespace Post {
            namespace Parameters {
                export type CaseId = string; // uuid
                export type DrainageStrategyId = string; // uuid
                export type ProjectId = string; // uuid
            }
            export interface PathParameters {
                projectId: Parameters.ProjectId /* uuid */;
                caseId: Parameters.CaseId /* uuid */;
                drainageStrategyId: Parameters.DrainageStrategyId /* uuid */;
            }
            export type RequestBody = Components.Schemas.CreateTimeSeriesVolumeDto;
            namespace Responses {
                export type $200 = Components.Schemas.TimeSeriesVolumeDto;
            }
        }
    }
    namespace Projects$ProjectIdCases$CaseIdDrainageStrategies$DrainageStrategyIdAdditionalProductionProfileGas$ProfileId {
        namespace Put {
            namespace Parameters {
                export type CaseId = string; // uuid
                export type DrainageStrategyId = string; // uuid
                export type ProfileId = string; // uuid
                export type ProjectId = string; // uuid
            }
            export interface PathParameters {
                projectId: Parameters.ProjectId /* uuid */;
                caseId: Parameters.CaseId /* uuid */;
                drainageStrategyId: Parameters.DrainageStrategyId /* uuid */;
                profileId: Parameters.ProfileId /* uuid */;
            }
            export type RequestBody = Components.Schemas.UpdateTimeSeriesVolumeDto;
            namespace Responses {
                export type $200 = Components.Schemas.TimeSeriesVolumeDto;
            }
        }
    }
    namespace Projects$ProjectIdCases$CaseIdDrainageStrategies$DrainageStrategyIdAdditionalProductionProfileOil {
        namespace Post {
            namespace Parameters {
                export type CaseId = string; // uuid
                export type DrainageStrategyId = string; // uuid
                export type ProjectId = string; // uuid
            }
            export interface PathParameters {
                projectId: Parameters.ProjectId /* uuid */;
                caseId: Parameters.CaseId /* uuid */;
                drainageStrategyId: Parameters.DrainageStrategyId /* uuid */;
            }
            export type RequestBody = Components.Schemas.CreateTimeSeriesVolumeDto;
            namespace Responses {
                export type $200 = Components.Schemas.TimeSeriesVolumeDto;
            }
        }
    }
    namespace Projects$ProjectIdCases$CaseIdDrainageStrategies$DrainageStrategyIdAdditionalProductionProfileOil$ProfileId {
        namespace Put {
            namespace Parameters {
                export type CaseId = string; // uuid
                export type DrainageStrategyId = string; // uuid
                export type ProfileId = string; // uuid
                export type ProjectId = string; // uuid
            }
            export interface PathParameters {
                projectId: Parameters.ProjectId /* uuid */;
                caseId: Parameters.CaseId /* uuid */;
                drainageStrategyId: Parameters.DrainageStrategyId /* uuid */;
                profileId: Parameters.ProfileId /* uuid */;
            }
            export type RequestBody = Components.Schemas.UpdateTimeSeriesVolumeDto;
            namespace Responses {
                export type $200 = Components.Schemas.TimeSeriesVolumeDto;
            }
        }
    }
    namespace Projects$ProjectIdCases$CaseIdDrainageStrategies$DrainageStrategyIdCo2EmissionsOverride {
        namespace Post {
            namespace Parameters {
                export type CaseId = string; // uuid
                export type DrainageStrategyId = string; // uuid
                export type ProjectId = string; // uuid
            }
            export interface PathParameters {
                projectId: Parameters.ProjectId /* uuid */;
                caseId: Parameters.CaseId /* uuid */;
                drainageStrategyId: Parameters.DrainageStrategyId /* uuid */;
            }
            export type RequestBody = Components.Schemas.CreateTimeSeriesMassOverrideDto;
            namespace Responses {
                export type $200 = Components.Schemas.TimeSeriesMassOverrideDto;
            }
        }
    }
    namespace Projects$ProjectIdCases$CaseIdDrainageStrategies$DrainageStrategyIdCo2EmissionsOverride$ProfileId {
        namespace Put {
            namespace Parameters {
                export type CaseId = string; // uuid
                export type DrainageStrategyId = string; // uuid
                export type ProfileId = string; // uuid
                export type ProjectId = string; // uuid
            }
            export interface PathParameters {
                projectId: Parameters.ProjectId /* uuid */;
                caseId: Parameters.CaseId /* uuid */;
                drainageStrategyId: Parameters.DrainageStrategyId /* uuid */;
                profileId: Parameters.ProfileId /* uuid */;
            }
            export type RequestBody = Components.Schemas.UpdateTimeSeriesMassOverrideDto;
            namespace Responses {
                export type $200 = Components.Schemas.TimeSeriesMassOverrideDto;
            }
        }
    }
    namespace Projects$ProjectIdCases$CaseIdDrainageStrategies$DrainageStrategyIdDeferredGasProduction {
        namespace Post {
            namespace Parameters {
                export type CaseId = string; // uuid
                export type DrainageStrategyId = string; // uuid
                export type ProjectId = string; // uuid
            }
            export interface PathParameters {
                projectId: Parameters.ProjectId /* uuid */;
                caseId: Parameters.CaseId /* uuid */;
                drainageStrategyId: Parameters.DrainageStrategyId /* uuid */;
            }
            export type RequestBody = Components.Schemas.CreateTimeSeriesVolumeDto;
            namespace Responses {
                export type $200 = Components.Schemas.TimeSeriesVolumeDto;
            }
        }
    }
    namespace Projects$ProjectIdCases$CaseIdDrainageStrategies$DrainageStrategyIdDeferredGasProduction$ProfileId {
        namespace Put {
            namespace Parameters {
                export type CaseId = string; // uuid
                export type DrainageStrategyId = string; // uuid
                export type ProfileId = string; // uuid
                export type ProjectId = string; // uuid
            }
            export interface PathParameters {
                projectId: Parameters.ProjectId /* uuid */;
                caseId: Parameters.CaseId /* uuid */;
                drainageStrategyId: Parameters.DrainageStrategyId /* uuid */;
                profileId: Parameters.ProfileId /* uuid */;
            }
            export type RequestBody = Components.Schemas.UpdateTimeSeriesVolumeDto;
            namespace Responses {
                export type $200 = Components.Schemas.TimeSeriesVolumeDto;
            }
        }
    }
    namespace Projects$ProjectIdCases$CaseIdDrainageStrategies$DrainageStrategyIdDeferredOilProduction {
        namespace Post {
            namespace Parameters {
                export type CaseId = string; // uuid
                export type DrainageStrategyId = string; // uuid
                export type ProjectId = string; // uuid
            }
            export interface PathParameters {
                projectId: Parameters.ProjectId /* uuid */;
                caseId: Parameters.CaseId /* uuid */;
                drainageStrategyId: Parameters.DrainageStrategyId /* uuid */;
            }
            export type RequestBody = Components.Schemas.CreateTimeSeriesVolumeDto;
            namespace Responses {
                export type $200 = Components.Schemas.TimeSeriesVolumeDto;
            }
        }
    }
    namespace Projects$ProjectIdCases$CaseIdDrainageStrategies$DrainageStrategyIdDeferredOilProduction$ProfileId {
        namespace Put {
            namespace Parameters {
                export type CaseId = string; // uuid
                export type DrainageStrategyId = string; // uuid
                export type ProfileId = string; // uuid
                export type ProjectId = string; // uuid
            }
            export interface PathParameters {
                projectId: Parameters.ProjectId /* uuid */;
                caseId: Parameters.CaseId /* uuid */;
                drainageStrategyId: Parameters.DrainageStrategyId /* uuid */;
                profileId: Parameters.ProfileId /* uuid */;
            }
            export type RequestBody = Components.Schemas.UpdateTimeSeriesVolumeDto;
            namespace Responses {
                export type $200 = Components.Schemas.TimeSeriesVolumeDto;
            }
        }
    }
    namespace Projects$ProjectIdCases$CaseIdDrainageStrategies$DrainageStrategyIdFuelFlaringAndLossesOverride {
        namespace Post {
            namespace Parameters {
                export type CaseId = string; // uuid
                export type DrainageStrategyId = string; // uuid
                export type ProjectId = string; // uuid
            }
            export interface PathParameters {
                projectId: Parameters.ProjectId /* uuid */;
                caseId: Parameters.CaseId /* uuid */;
                drainageStrategyId: Parameters.DrainageStrategyId /* uuid */;
            }
            export type RequestBody = Components.Schemas.CreateTimeSeriesVolumeOverrideDto;
            namespace Responses {
                export type $200 = Components.Schemas.TimeSeriesVolumeOverrideDto;
            }
        }
    }
    namespace Projects$ProjectIdCases$CaseIdDrainageStrategies$DrainageStrategyIdFuelFlaringAndLossesOverride$ProfileId {
        namespace Put {
            namespace Parameters {
                export type CaseId = string; // uuid
                export type DrainageStrategyId = string; // uuid
                export type ProfileId = string; // uuid
                export type ProjectId = string; // uuid
            }
            export interface PathParameters {
                projectId: Parameters.ProjectId /* uuid */;
                caseId: Parameters.CaseId /* uuid */;
                drainageStrategyId: Parameters.DrainageStrategyId /* uuid */;
                profileId: Parameters.ProfileId /* uuid */;
            }
            export type RequestBody = Components.Schemas.UpdateTimeSeriesVolumeOverrideDto;
            namespace Responses {
                export type $200 = Components.Schemas.TimeSeriesVolumeOverrideDto;
            }
        }
    }
    namespace Projects$ProjectIdCases$CaseIdDrainageStrategies$DrainageStrategyIdImportedElectricityOverride {
        namespace Post {
            namespace Parameters {
                export type CaseId = string; // uuid
                export type DrainageStrategyId = string; // uuid
                export type ProjectId = string; // uuid
            }
            export interface PathParameters {
                projectId: Parameters.ProjectId /* uuid */;
                caseId: Parameters.CaseId /* uuid */;
                drainageStrategyId: Parameters.DrainageStrategyId /* uuid */;
            }
            export type RequestBody = Components.Schemas.CreateTimeSeriesEnergyDto;
            namespace Responses {
                export type $200 = Components.Schemas.TimeSeriesEnergyOverrideDto;
            }
        }
    }
    namespace Projects$ProjectIdCases$CaseIdDrainageStrategies$DrainageStrategyIdImportedElectricityOverride$ProfileId {
        namespace Put {
            namespace Parameters {
                export type CaseId = string; // uuid
                export type DrainageStrategyId = string; // uuid
                export type ProfileId = string; // uuid
                export type ProjectId = string; // uuid
            }
            export interface PathParameters {
                projectId: Parameters.ProjectId /* uuid */;
                caseId: Parameters.CaseId /* uuid */;
                drainageStrategyId: Parameters.DrainageStrategyId /* uuid */;
                profileId: Parameters.ProfileId /* uuid */;
            }
            export type RequestBody = Components.Schemas.UpdateTimeSeriesEnergyOverrideDto;
            namespace Responses {
                export type $200 = Components.Schemas.TimeSeriesEnergyOverrideDto;
            }
        }
    }
    namespace Projects$ProjectIdCases$CaseIdDrainageStrategies$DrainageStrategyIdNetSalesGasOverride {
        namespace Post {
            namespace Parameters {
                export type CaseId = string; // uuid
                export type DrainageStrategyId = string; // uuid
                export type ProjectId = string; // uuid
            }
            export interface PathParameters {
                projectId: Parameters.ProjectId /* uuid */;
                caseId: Parameters.CaseId /* uuid */;
                drainageStrategyId: Parameters.DrainageStrategyId /* uuid */;
            }
            export type RequestBody = Components.Schemas.CreateTimeSeriesVolumeOverrideDto;
            namespace Responses {
                export type $200 = Components.Schemas.TimeSeriesVolumeOverrideDto;
            }
        }
    }
    namespace Projects$ProjectIdCases$CaseIdDrainageStrategies$DrainageStrategyIdNetSalesGasOverride$ProfileId {
        namespace Put {
            namespace Parameters {
                export type CaseId = string; // uuid
                export type DrainageStrategyId = string; // uuid
                export type ProfileId = string; // uuid
                export type ProjectId = string; // uuid
            }
            export interface PathParameters {
                projectId: Parameters.ProjectId /* uuid */;
                caseId: Parameters.CaseId /* uuid */;
                drainageStrategyId: Parameters.DrainageStrategyId /* uuid */;
                profileId: Parameters.ProfileId /* uuid */;
            }
            export type RequestBody = Components.Schemas.UpdateTimeSeriesVolumeOverrideDto;
            namespace Responses {
                export type $200 = Components.Schemas.TimeSeriesVolumeOverrideDto;
            }
        }
    }
    namespace Projects$ProjectIdCases$CaseIdDrainageStrategies$DrainageStrategyIdProductionProfileGas {
        namespace Post {
            namespace Parameters {
                export type CaseId = string; // uuid
                export type DrainageStrategyId = string; // uuid
                export type ProjectId = string; // uuid
            }
            export interface PathParameters {
                projectId: Parameters.ProjectId /* uuid */;
                caseId: Parameters.CaseId /* uuid */;
                drainageStrategyId: Parameters.DrainageStrategyId /* uuid */;
            }
            export type RequestBody = Components.Schemas.CreateTimeSeriesVolumeDto;
            namespace Responses {
                export type $200 = Components.Schemas.TimeSeriesVolumeDto;
            }
        }
    }
    namespace Projects$ProjectIdCases$CaseIdDrainageStrategies$DrainageStrategyIdProductionProfileGas$ProfileId {
        namespace Put {
            namespace Parameters {
                export type CaseId = string; // uuid
                export type DrainageStrategyId = string; // uuid
                export type ProfileId = string; // uuid
                export type ProjectId = string; // uuid
            }
            export interface PathParameters {
                projectId: Parameters.ProjectId /* uuid */;
                caseId: Parameters.CaseId /* uuid */;
                drainageStrategyId: Parameters.DrainageStrategyId /* uuid */;
                profileId: Parameters.ProfileId /* uuid */;
            }
            export type RequestBody = Components.Schemas.UpdateTimeSeriesVolumeDto;
            namespace Responses {
                export type $200 = Components.Schemas.TimeSeriesVolumeDto;
            }
        }
    }
    namespace Projects$ProjectIdCases$CaseIdDrainageStrategies$DrainageStrategyIdProductionProfileOil {
        namespace Post {
            namespace Parameters {
                export type CaseId = string; // uuid
                export type DrainageStrategyId = string; // uuid
                export type ProjectId = string; // uuid
            }
            export interface PathParameters {
                projectId: Parameters.ProjectId /* uuid */;
                caseId: Parameters.CaseId /* uuid */;
                drainageStrategyId: Parameters.DrainageStrategyId /* uuid */;
            }
            export type RequestBody = Components.Schemas.CreateTimeSeriesVolumeDto;
            namespace Responses {
                export type $200 = Components.Schemas.TimeSeriesVolumeDto;
            }
        }
    }
    namespace Projects$ProjectIdCases$CaseIdDrainageStrategies$DrainageStrategyIdProductionProfileOil$ProfileId {
        namespace Put {
            namespace Parameters {
                export type CaseId = string; // uuid
                export type DrainageStrategyId = string; // uuid
                export type ProfileId = string; // uuid
                export type ProjectId = string; // uuid
            }
            export interface PathParameters {
                projectId: Parameters.ProjectId /* uuid */;
                caseId: Parameters.CaseId /* uuid */;
                drainageStrategyId: Parameters.DrainageStrategyId /* uuid */;
                profileId: Parameters.ProfileId /* uuid */;
            }
            export type RequestBody = Components.Schemas.UpdateTimeSeriesVolumeDto;
            namespace Responses {
                export type $200 = Components.Schemas.TimeSeriesVolumeDto;
            }
        }
    }
    namespace Projects$ProjectIdCases$CaseIdDrainageStrategies$DrainageStrategyIdProductionProfileWater {
        namespace Post {
            namespace Parameters {
                export type CaseId = string; // uuid
                export type DrainageStrategyId = string; // uuid
                export type ProjectId = string; // uuid
            }
            export interface PathParameters {
                projectId: Parameters.ProjectId /* uuid */;
                caseId: Parameters.CaseId /* uuid */;
                drainageStrategyId: Parameters.DrainageStrategyId /* uuid */;
            }
            export type RequestBody = Components.Schemas.CreateTimeSeriesVolumeDto;
            namespace Responses {
                export type $200 = Components.Schemas.TimeSeriesVolumeDto;
            }
        }
    }
    namespace Projects$ProjectIdCases$CaseIdDrainageStrategies$DrainageStrategyIdProductionProfileWater$ProfileId {
        namespace Put {
            namespace Parameters {
                export type CaseId = string; // uuid
                export type DrainageStrategyId = string; // uuid
                export type ProfileId = string; // uuid
                export type ProjectId = string; // uuid
            }
            export interface PathParameters {
                projectId: Parameters.ProjectId /* uuid */;
                caseId: Parameters.CaseId /* uuid */;
                drainageStrategyId: Parameters.DrainageStrategyId /* uuid */;
                profileId: Parameters.ProfileId /* uuid */;
            }
            export type RequestBody = Components.Schemas.UpdateTimeSeriesVolumeDto;
            namespace Responses {
                export type $200 = Components.Schemas.TimeSeriesVolumeDto;
            }
        }
    }
    namespace Projects$ProjectIdCases$CaseIdDrainageStrategies$DrainageStrategyIdProductionProfileWaterInjection {
        namespace Post {
            namespace Parameters {
                export type CaseId = string; // uuid
                export type DrainageStrategyId = string; // uuid
                export type ProjectId = string; // uuid
            }
            export interface PathParameters {
                projectId: Parameters.ProjectId /* uuid */;
                caseId: Parameters.CaseId /* uuid */;
                drainageStrategyId: Parameters.DrainageStrategyId /* uuid */;
            }
            export type RequestBody = Components.Schemas.CreateTimeSeriesVolumeDto;
            namespace Responses {
                export type $200 = Components.Schemas.TimeSeriesVolumeDto;
            }
        }
    }
    namespace Projects$ProjectIdCases$CaseIdDrainageStrategies$DrainageStrategyIdProductionProfileWaterInjection$ProfileId {
        namespace Put {
            namespace Parameters {
                export type CaseId = string; // uuid
                export type DrainageStrategyId = string; // uuid
                export type ProfileId = string; // uuid
                export type ProjectId = string; // uuid
            }
            export interface PathParameters {
                projectId: Parameters.ProjectId /* uuid */;
                caseId: Parameters.CaseId /* uuid */;
                drainageStrategyId: Parameters.DrainageStrategyId /* uuid */;
                profileId: Parameters.ProfileId /* uuid */;
            }
            export type RequestBody = Components.Schemas.UpdateTimeSeriesVolumeDto;
            namespace Responses {
                export type $200 = Components.Schemas.TimeSeriesVolumeDto;
            }
        }
    }
    namespace Projects$ProjectIdCases$CaseIdExplorations$ExplorationId {
        namespace Put {
            namespace Parameters {
                export type CaseId = string; // uuid
                export type ExplorationId = string; // uuid
                export type ProjectId = string; // uuid
            }
            export interface PathParameters {
                projectId: Parameters.ProjectId /* uuid */;
                caseId: Parameters.CaseId /* uuid */;
                explorationId: Parameters.ExplorationId /* uuid */;
            }
            export type RequestBody = Components.Schemas.UpdateExplorationDto;
            namespace Responses {
                export interface $200 {
                }
            }
        }
    }
    namespace Projects$ProjectIdCases$CaseIdExplorations$ExplorationIdCountryOfficeCost {
        namespace Post {
            namespace Parameters {
                export type CaseId = string; // uuid
                export type ExplorationId = string; // uuid
                export type ProjectId = string; // uuid
            }
            export interface PathParameters {
                projectId: Parameters.ProjectId /* uuid */;
                caseId: Parameters.CaseId /* uuid */;
                explorationId: Parameters.ExplorationId /* uuid */;
            }
            export type RequestBody = Components.Schemas.CreateTimeSeriesCostDto;
            namespace Responses {
                export type $200 = Components.Schemas.TimeSeriesCostDto;
            }
        }
    }
    namespace Projects$ProjectIdCases$CaseIdExplorations$ExplorationIdCountryOfficeCost$CostProfileId {
        namespace Put {
            namespace Parameters {
                export type CaseId = string; // uuid
                export type CostProfileId = string; // uuid
                export type ExplorationId = string; // uuid
                export type ProjectId = string; // uuid
            }
            export interface PathParameters {
                projectId: Parameters.ProjectId /* uuid */;
                caseId: Parameters.CaseId /* uuid */;
                explorationId: Parameters.ExplorationId /* uuid */;
                costProfileId: Parameters.CostProfileId /* uuid */;
            }
            export type RequestBody = Components.Schemas.UpdateTimeSeriesCostDto;
            namespace Responses {
                export type $200 = Components.Schemas.TimeSeriesCostDto;
            }
        }
    }
    namespace Projects$ProjectIdCases$CaseIdExplorations$ExplorationIdGAndGAndAdminCostOverride {
        namespace Post {
            namespace Parameters {
                export type CaseId = string; // uuid
                export type ExplorationId = string; // uuid
                export type ProjectId = string; // uuid
            }
            export interface PathParameters {
                projectId: Parameters.ProjectId /* uuid */;
                caseId: Parameters.CaseId /* uuid */;
                explorationId: Parameters.ExplorationId /* uuid */;
            }
            export type RequestBody = Components.Schemas.CreateTimeSeriesCostOverrideDto;
            namespace Responses {
                export type $200 = Components.Schemas.TimeSeriesCostOverrideDto;
            }
        }
    }
    namespace Projects$ProjectIdCases$CaseIdExplorations$ExplorationIdGAndGAndAdminCostOverride$CostProfileId {
        namespace Put {
            namespace Parameters {
                export type CaseId = string; // uuid
                export type CostProfileId = string; // uuid
                export type ExplorationId = string; // uuid
                export type ProjectId = string; // uuid
            }
            export interface PathParameters {
                projectId: Parameters.ProjectId /* uuid */;
                caseId: Parameters.CaseId /* uuid */;
                explorationId: Parameters.ExplorationId /* uuid */;
                costProfileId: Parameters.CostProfileId /* uuid */;
            }
            export type RequestBody = Components.Schemas.UpdateTimeSeriesCostOverrideDto;
            namespace Responses {
                export type $200 = Components.Schemas.TimeSeriesCostOverrideDto;
            }
        }
    }
    namespace Projects$ProjectIdCases$CaseIdExplorations$ExplorationIdSeismicAcquisitionAndProcessing {
        namespace Post {
            namespace Parameters {
                export type CaseId = string; // uuid
                export type ExplorationId = string; // uuid
                export type ProjectId = string; // uuid
            }
            export interface PathParameters {
                projectId: Parameters.ProjectId /* uuid */;
                caseId: Parameters.CaseId /* uuid */;
                explorationId: Parameters.ExplorationId /* uuid */;
            }
            export type RequestBody = Components.Schemas.CreateTimeSeriesCostDto;
            namespace Responses {
                export type $200 = Components.Schemas.TimeSeriesCostDto;
            }
        }
    }
    namespace Projects$ProjectIdCases$CaseIdExplorations$ExplorationIdSeismicAcquisitionAndProcessing$CostProfileId {
        namespace Put {
            namespace Parameters {
                export type CaseId = string; // uuid
                export type CostProfileId = string; // uuid
                export type ExplorationId = string; // uuid
                export type ProjectId = string; // uuid
            }
            export interface PathParameters {
                projectId: Parameters.ProjectId /* uuid */;
                caseId: Parameters.CaseId /* uuid */;
                explorationId: Parameters.ExplorationId /* uuid */;
                costProfileId: Parameters.CostProfileId /* uuid */;
            }
            export type RequestBody = Components.Schemas.UpdateTimeSeriesCostDto;
            namespace Responses {
                export type $200 = Components.Schemas.TimeSeriesCostDto;
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
    namespace Projects$ProjectIdCases$CaseIdHistoricCostCostProfile {
        namespace Post {
            namespace Parameters {
                export type CaseId = string; // uuid
                export type ProjectId = string; // uuid
            }
            export interface PathParameters {
                projectId: Parameters.ProjectId /* uuid */;
                caseId: Parameters.CaseId /* uuid */;
            }
            export type RequestBody = Components.Schemas.CreateTimeSeriesCostDto;
            namespace Responses {
                export type $200 = Components.Schemas.TimeSeriesCostDto;
            }
        }
    }
    namespace Projects$ProjectIdCases$CaseIdHistoricCostCostProfile$CostProfileId {
        namespace Put {
            namespace Parameters {
                export type CaseId = string; // uuid
                export type CostProfileId = string; // uuid
                export type ProjectId = string; // uuid
            }
            export interface PathParameters {
                projectId: Parameters.ProjectId /* uuid */;
                caseId: Parameters.CaseId /* uuid */;
                costProfileId: Parameters.CostProfileId /* uuid */;
            }
            export type RequestBody = Components.Schemas.UpdateTimeSeriesCostDto;
            namespace Responses {
                export type $200 = Components.Schemas.TimeSeriesCostDto;
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
    namespace Projects$ProjectIdCases$CaseIdOffshoreFacilitiesOperationsCostProfileOverride {
        namespace Post {
            namespace Parameters {
                export type CaseId = string; // uuid
                export type ProjectId = string; // uuid
            }
            export interface PathParameters {
                projectId: Parameters.ProjectId /* uuid */;
                caseId: Parameters.CaseId /* uuid */;
            }
            export type RequestBody = Components.Schemas.CreateTimeSeriesCostOverrideDto;
            namespace Responses {
                export type $200 = Components.Schemas.TimeSeriesCostOverrideDto;
            }
        }
    }
    namespace Projects$ProjectIdCases$CaseIdOffshoreFacilitiesOperationsCostProfileOverride$CostProfileId {
        namespace Put {
            namespace Parameters {
                export type CaseId = string; // uuid
                export type CostProfileId = string; // uuid
                export type ProjectId = string; // uuid
            }
            export interface PathParameters {
                projectId: Parameters.ProjectId /* uuid */;
                caseId: Parameters.CaseId /* uuid */;
                costProfileId: Parameters.CostProfileId /* uuid */;
            }
            export type RequestBody = Components.Schemas.UpdateTimeSeriesCostOverrideDto;
            namespace Responses {
                export type $200 = Components.Schemas.TimeSeriesCostOverrideDto;
            }
        }
    }
    namespace Projects$ProjectIdCases$CaseIdOnshorePowerSupplies$OnshorePowerSupplyId {
        namespace Put {
            namespace Parameters {
                export type CaseId = string; // uuid
                export type OnshorePowerSupplyId = string; // uuid
                export type ProjectId = string; // uuid
            }
            export interface PathParameters {
                projectId: Parameters.ProjectId /* uuid */;
                caseId: Parameters.CaseId /* uuid */;
                onshorePowerSupplyId: Parameters.OnshorePowerSupplyId /* uuid */;
            }
            export type RequestBody = Components.Schemas.UpdateOnshorePowerSupplyDto;
            namespace Responses {
                export interface $200 {
                }
            }
        }
    }
    namespace Projects$ProjectIdCases$CaseIdOnshorePowerSupplies$OnshorePowerSupplyIdCostProfileOverride {
        namespace Post {
            namespace Parameters {
                export type CaseId = string; // uuid
                export type OnshorePowerSupplyId = string; // uuid
                export type ProjectId = string; // uuid
            }
            export interface PathParameters {
                projectId: Parameters.ProjectId /* uuid */;
                caseId: Parameters.CaseId /* uuid */;
                onshorePowerSupplyId: Parameters.OnshorePowerSupplyId /* uuid */;
            }
            export type RequestBody = Components.Schemas.CreateTimeSeriesCostOverrideDto;
            namespace Responses {
                export type $200 = Components.Schemas.TimeSeriesCostOverrideDto;
            }
        }
    }
    namespace Projects$ProjectIdCases$CaseIdOnshorePowerSupplies$OnshorePowerSupplyIdCostProfileOverride$CostProfileId {
        namespace Put {
            namespace Parameters {
                export type CaseId = string; // uuid
                export type CostProfileId = string; // uuid
                export type OnshorePowerSupplyId = string; // uuid
                export type ProjectId = string; // uuid
            }
            export interface PathParameters {
                projectId: Parameters.ProjectId /* uuid */;
                caseId: Parameters.CaseId /* uuid */;
                onshorePowerSupplyId: Parameters.OnshorePowerSupplyId /* uuid */;
                costProfileId: Parameters.CostProfileId /* uuid */;
            }
            export type RequestBody = Components.Schemas.UpdateTimeSeriesCostOverrideDto;
            namespace Responses {
                export type $200 = Components.Schemas.TimeSeriesCostOverrideDto;
            }
        }
    }
    namespace Projects$ProjectIdCases$CaseIdOnshoreRelatedOpexCostProfile {
        namespace Post {
            namespace Parameters {
                export type CaseId = string; // uuid
                export type ProjectId = string; // uuid
            }
            export interface PathParameters {
                projectId: Parameters.ProjectId /* uuid */;
                caseId: Parameters.CaseId /* uuid */;
            }
            export type RequestBody = Components.Schemas.CreateTimeSeriesCostDto;
            namespace Responses {
                export type $200 = Components.Schemas.TimeSeriesCostDto;
            }
        }
    }
    namespace Projects$ProjectIdCases$CaseIdOnshoreRelatedOpexCostProfile$CostProfileId {
        namespace Put {
            namespace Parameters {
                export type CaseId = string; // uuid
                export type CostProfileId = string; // uuid
                export type ProjectId = string; // uuid
            }
            export interface PathParameters {
                projectId: Parameters.ProjectId /* uuid */;
                caseId: Parameters.CaseId /* uuid */;
                costProfileId: Parameters.CostProfileId /* uuid */;
            }
            export type RequestBody = Components.Schemas.UpdateTimeSeriesCostDto;
            namespace Responses {
                export type $200 = Components.Schemas.TimeSeriesCostDto;
            }
        }
    }
    namespace Projects$ProjectIdCases$CaseIdSubstructures$SubstructureId {
        namespace Put {
            namespace Parameters {
                export type CaseId = string; // uuid
                export type ProjectId = string; // uuid
                export type SubstructureId = string; // uuid
            }
            export interface PathParameters {
                projectId: Parameters.ProjectId /* uuid */;
                caseId: Parameters.CaseId /* uuid */;
                substructureId: Parameters.SubstructureId /* uuid */;
            }
            export type RequestBody = Components.Schemas.UpdateSubstructureDto;
            namespace Responses {
                export interface $200 {
                }
            }
        }
    }
    namespace Projects$ProjectIdCases$CaseIdSubstructures$SubstructureIdCostProfileOverride {
        namespace Post {
            namespace Parameters {
                export type CaseId = string; // uuid
                export type ProjectId = string; // uuid
                export type SubstructureId = string; // uuid
            }
            export interface PathParameters {
                projectId: Parameters.ProjectId /* uuid */;
                caseId: Parameters.CaseId /* uuid */;
                substructureId: Parameters.SubstructureId /* uuid */;
            }
            export type RequestBody = Components.Schemas.CreateTimeSeriesCostOverrideDto;
            namespace Responses {
                export type $200 = Components.Schemas.TimeSeriesCostOverrideDto;
            }
        }
    }
    namespace Projects$ProjectIdCases$CaseIdSubstructures$SubstructureIdCostProfileOverride$CostProfileId {
        namespace Put {
            namespace Parameters {
                export type CaseId = string; // uuid
                export type CostProfileId = string; // uuid
                export type ProjectId = string; // uuid
                export type SubstructureId = string; // uuid
            }
            export interface PathParameters {
                projectId: Parameters.ProjectId /* uuid */;
                caseId: Parameters.CaseId /* uuid */;
                substructureId: Parameters.SubstructureId /* uuid */;
                costProfileId: Parameters.CostProfileId /* uuid */;
            }
            export type RequestBody = Components.Schemas.UpdateTimeSeriesCostOverrideDto;
            namespace Responses {
                export type $200 = Components.Schemas.TimeSeriesCostOverrideDto;
            }
        }
    }
    namespace Projects$ProjectIdCases$CaseIdSurfs$SurfId {
        namespace Put {
            namespace Parameters {
                export type CaseId = string; // uuid
                export type ProjectId = string; // uuid
                export type SurfId = string; // uuid
            }
            export interface PathParameters {
                projectId: Parameters.ProjectId /* uuid */;
                caseId: Parameters.CaseId /* uuid */;
                surfId: Parameters.SurfId /* uuid */;
            }
            export type RequestBody = Components.Schemas.UpdateSurfDto;
            namespace Responses {
                export interface $200 {
                }
            }
        }
    }
    namespace Projects$ProjectIdCases$CaseIdSurfs$SurfIdCostProfileOverride {
        namespace Post {
            namespace Parameters {
                export type CaseId = string; // uuid
                export type ProjectId = string; // uuid
                export type SurfId = string; // uuid
            }
            export interface PathParameters {
                projectId: Parameters.ProjectId /* uuid */;
                caseId: Parameters.CaseId /* uuid */;
                surfId: Parameters.SurfId /* uuid */;
            }
            export type RequestBody = Components.Schemas.CreateTimeSeriesCostOverrideDto;
            namespace Responses {
                export type $200 = Components.Schemas.TimeSeriesCostOverrideDto;
            }
        }
    }
    namespace Projects$ProjectIdCases$CaseIdSurfs$SurfIdCostProfileOverride$CostProfileId {
        namespace Put {
            namespace Parameters {
                export type CaseId = string; // uuid
                export type CostProfileId = string; // uuid
                export type ProjectId = string; // uuid
                export type SurfId = string; // uuid
            }
            export interface PathParameters {
                projectId: Parameters.ProjectId /* uuid */;
                caseId: Parameters.CaseId /* uuid */;
                surfId: Parameters.SurfId /* uuid */;
                costProfileId: Parameters.CostProfileId /* uuid */;
            }
            export type RequestBody = Components.Schemas.UpdateTimeSeriesCostOverrideDto;
            namespace Responses {
                export type $200 = Components.Schemas.TimeSeriesCostOverrideDto;
            }
        }
    }
    namespace Projects$ProjectIdCases$CaseIdTopsides$TopsideId {
        namespace Put {
            namespace Parameters {
                export type CaseId = string; // uuid
                export type ProjectId = string; // uuid
                export type TopsideId = string; // uuid
            }
            export interface PathParameters {
                projectId: Parameters.ProjectId /* uuid */;
                caseId: Parameters.CaseId /* uuid */;
                topsideId: Parameters.TopsideId /* uuid */;
            }
            export type RequestBody = Components.Schemas.UpdateTopsideDto;
            namespace Responses {
                export interface $200 {
                }
            }
        }
    }
    namespace Projects$ProjectIdCases$CaseIdTopsides$TopsideIdCostProfileOverride {
        namespace Post {
            namespace Parameters {
                export type CaseId = string; // uuid
                export type ProjectId = string; // uuid
                export type TopsideId = string; // uuid
            }
            export interface PathParameters {
                projectId: Parameters.ProjectId /* uuid */;
                caseId: Parameters.CaseId /* uuid */;
                topsideId: Parameters.TopsideId /* uuid */;
            }
            export type RequestBody = Components.Schemas.CreateTimeSeriesCostOverrideDto;
            namespace Responses {
                export type $200 = Components.Schemas.TimeSeriesCostOverrideDto;
            }
        }
    }
    namespace Projects$ProjectIdCases$CaseIdTopsides$TopsideIdCostProfileOverride$CostProfileId {
        namespace Put {
            namespace Parameters {
                export type CaseId = string; // uuid
                export type CostProfileId = string; // uuid
                export type ProjectId = string; // uuid
                export type TopsideId = string; // uuid
            }
            export interface PathParameters {
                projectId: Parameters.ProjectId /* uuid */;
                caseId: Parameters.CaseId /* uuid */;
                topsideId: Parameters.TopsideId /* uuid */;
                costProfileId: Parameters.CostProfileId /* uuid */;
            }
            export type RequestBody = Components.Schemas.UpdateTimeSeriesCostOverrideDto;
            namespace Responses {
                export type $200 = Components.Schemas.TimeSeriesCostOverrideDto;
            }
        }
    }
    namespace Projects$ProjectIdCases$CaseIdTotalFeasibilityAndConceptStudiesOverride {
        namespace Post {
            namespace Parameters {
                export type CaseId = string; // uuid
                export type ProjectId = string; // uuid
            }
            export interface PathParameters {
                projectId: Parameters.ProjectId /* uuid */;
                caseId: Parameters.CaseId /* uuid */;
            }
            export type RequestBody = Components.Schemas.CreateTimeSeriesCostOverrideDto;
            namespace Responses {
                export type $200 = Components.Schemas.TimeSeriesCostOverrideDto;
            }
        }
    }
    namespace Projects$ProjectIdCases$CaseIdTotalFeasibilityAndConceptStudiesOverride$CostProfileId {
        namespace Put {
            namespace Parameters {
                export type CaseId = string; // uuid
                export type CostProfileId = string; // uuid
                export type ProjectId = string; // uuid
            }
            export interface PathParameters {
                projectId: Parameters.ProjectId /* uuid */;
                caseId: Parameters.CaseId /* uuid */;
                costProfileId: Parameters.CostProfileId /* uuid */;
            }
            export type RequestBody = Components.Schemas.UpdateTimeSeriesCostOverrideDto;
            namespace Responses {
                export type $200 = Components.Schemas.TimeSeriesCostOverrideDto;
            }
        }
    }
    namespace Projects$ProjectIdCases$CaseIdTotalFeedStudiesOverride {
        namespace Post {
            namespace Parameters {
                export type CaseId = string; // uuid
                export type ProjectId = string; // uuid
            }
            export interface PathParameters {
                projectId: Parameters.ProjectId /* uuid */;
                caseId: Parameters.CaseId /* uuid */;
            }
            export type RequestBody = Components.Schemas.CreateTimeSeriesCostOverrideDto;
            namespace Responses {
                export type $200 = Components.Schemas.TimeSeriesCostOverrideDto;
            }
        }
    }
    namespace Projects$ProjectIdCases$CaseIdTotalFeedStudiesOverride$CostProfileId {
        namespace Put {
            namespace Parameters {
                export type CaseId = string; // uuid
                export type CostProfileId = string; // uuid
                export type ProjectId = string; // uuid
            }
            export interface PathParameters {
                projectId: Parameters.ProjectId /* uuid */;
                caseId: Parameters.CaseId /* uuid */;
                costProfileId: Parameters.CostProfileId /* uuid */;
            }
            export type RequestBody = Components.Schemas.UpdateTimeSeriesCostOverrideDto;
            namespace Responses {
                export type $200 = Components.Schemas.TimeSeriesCostOverrideDto;
            }
        }
    }
    namespace Projects$ProjectIdCases$CaseIdTotalOtherStudiesCostProfile {
        namespace Post {
            namespace Parameters {
                export type CaseId = string; // uuid
                export type ProjectId = string; // uuid
            }
            export interface PathParameters {
                projectId: Parameters.ProjectId /* uuid */;
                caseId: Parameters.CaseId /* uuid */;
            }
            export type RequestBody = Components.Schemas.CreateTimeSeriesCostDto;
            namespace Responses {
                export type $200 = Components.Schemas.TimeSeriesCostDto;
            }
        }
    }
    namespace Projects$ProjectIdCases$CaseIdTotalOtherStudiesCostProfile$CostProfileId {
        namespace Put {
            namespace Parameters {
                export type CaseId = string; // uuid
                export type CostProfileId = string; // uuid
                export type ProjectId = string; // uuid
            }
            export interface PathParameters {
                projectId: Parameters.ProjectId /* uuid */;
                caseId: Parameters.CaseId /* uuid */;
                costProfileId: Parameters.CostProfileId /* uuid */;
            }
            export type RequestBody = Components.Schemas.UpdateTimeSeriesCostOverrideDto;
            namespace Responses {
                export type $200 = Components.Schemas.TimeSeriesCostDto;
            }
        }
    }
    namespace Projects$ProjectIdCases$CaseIdTransports$TransportId {
        namespace Put {
            namespace Parameters {
                export type CaseId = string; // uuid
                export type ProjectId = string; // uuid
                export type TransportId = string; // uuid
            }
            export interface PathParameters {
                projectId: Parameters.ProjectId /* uuid */;
                caseId: Parameters.CaseId /* uuid */;
                transportId: Parameters.TransportId /* uuid */;
            }
            export type RequestBody = Components.Schemas.UpdateTransportDto;
            namespace Responses {
                export interface $200 {
                }
            }
        }
    }
    namespace Projects$ProjectIdCases$CaseIdTransports$TransportIdCostProfileOverride {
        namespace Post {
            namespace Parameters {
                export type CaseId = string; // uuid
                export type ProjectId = string; // uuid
                export type TransportId = string; // uuid
            }
            export interface PathParameters {
                projectId: Parameters.ProjectId /* uuid */;
                caseId: Parameters.CaseId /* uuid */;
                transportId: Parameters.TransportId /* uuid */;
            }
            export type RequestBody = Components.Schemas.CreateTimeSeriesCostOverrideDto;
            namespace Responses {
                export type $200 = Components.Schemas.TimeSeriesCostOverrideDto;
            }
        }
    }
    namespace Projects$ProjectIdCases$CaseIdTransports$TransportIdCostProfileOverride$CostProfileId {
        namespace Put {
            namespace Parameters {
                export type CaseId = string; // uuid
                export type CostProfileId = string; // uuid
                export type ProjectId = string; // uuid
                export type TransportId = string; // uuid
            }
            export interface PathParameters {
                projectId: Parameters.ProjectId /* uuid */;
                caseId: Parameters.CaseId /* uuid */;
                transportId: Parameters.TransportId /* uuid */;
                costProfileId: Parameters.CostProfileId /* uuid */;
            }
            export type RequestBody = Components.Schemas.UpdateTimeSeriesCostOverrideDto;
            namespace Responses {
                export type $200 = Components.Schemas.TimeSeriesCostOverrideDto;
            }
        }
    }
    namespace Projects$ProjectIdCases$CaseIdWellInterventionCostProfileOverride {
        namespace Post {
            namespace Parameters {
                export type CaseId = string; // uuid
                export type ProjectId = string; // uuid
            }
            export interface PathParameters {
                projectId: Parameters.ProjectId /* uuid */;
                caseId: Parameters.CaseId /* uuid */;
            }
            export type RequestBody = Components.Schemas.CreateTimeSeriesCostOverrideDto;
            namespace Responses {
                export type $200 = Components.Schemas.TimeSeriesCostOverrideDto;
            }
        }
    }
    namespace Projects$ProjectIdCases$CaseIdWellInterventionCostProfileOverride$CostProfileId {
        namespace Put {
            namespace Parameters {
                export type CaseId = string; // uuid
                export type CostProfileId = string; // uuid
                export type ProjectId = string; // uuid
            }
            export interface PathParameters {
                projectId: Parameters.ProjectId /* uuid */;
                caseId: Parameters.CaseId /* uuid */;
                costProfileId: Parameters.CostProfileId /* uuid */;
            }
            export type RequestBody = Components.Schemas.UpdateTimeSeriesCostOverrideDto;
            namespace Responses {
                export type $200 = Components.Schemas.TimeSeriesCostOverrideDto;
            }
        }
    }
    namespace Projects$ProjectIdCases$CaseIdWellProjects$WellProjectId {
        namespace Put {
            namespace Parameters {
                export type CaseId = string; // uuid
                export type ProjectId = string; // uuid
                export type WellProjectId = string; // uuid
            }
            export interface PathParameters {
                projectId: Parameters.ProjectId /* uuid */;
                caseId: Parameters.CaseId /* uuid */;
                wellProjectId: Parameters.WellProjectId /* uuid */;
            }
            export type RequestBody = Components.Schemas.UpdateWellProjectDto;
            namespace Responses {
                export interface $200 {
                }
            }
        }
    }
    namespace Projects$ProjectIdCases$CaseIdWellProjects$WellProjectIdGasInjectorCostProfileOverride {
        namespace Post {
            namespace Parameters {
                export type CaseId = string; // uuid
                export type ProjectId = string; // uuid
                export type WellProjectId = string; // uuid
            }
            export interface PathParameters {
                projectId: Parameters.ProjectId /* uuid */;
                caseId: Parameters.CaseId /* uuid */;
                wellProjectId: Parameters.WellProjectId /* uuid */;
            }
            export type RequestBody = Components.Schemas.CreateTimeSeriesCostOverrideDto;
            namespace Responses {
                export type $200 = Components.Schemas.TimeSeriesCostOverrideDto;
            }
        }
    }
    namespace Projects$ProjectIdCases$CaseIdWellProjects$WellProjectIdGasInjectorCostProfileOverride$CostProfileId {
        namespace Put {
            namespace Parameters {
                export type CaseId = string; // uuid
                export type CostProfileId = string; // uuid
                export type ProjectId = string; // uuid
                export type WellProjectId = string; // uuid
            }
            export interface PathParameters {
                projectId: Parameters.ProjectId /* uuid */;
                caseId: Parameters.CaseId /* uuid */;
                wellProjectId: Parameters.WellProjectId /* uuid */;
                costProfileId: Parameters.CostProfileId /* uuid */;
            }
            export type RequestBody = Components.Schemas.UpdateTimeSeriesCostOverrideDto;
            namespace Responses {
                export type $200 = Components.Schemas.TimeSeriesCostOverrideDto;
            }
        }
    }
    namespace Projects$ProjectIdCases$CaseIdWellProjects$WellProjectIdGasProducerCostProfileOverride {
        namespace Post {
            namespace Parameters {
                export type CaseId = string; // uuid
                export type ProjectId = string; // uuid
                export type WellProjectId = string; // uuid
            }
            export interface PathParameters {
                projectId: Parameters.ProjectId /* uuid */;
                caseId: Parameters.CaseId /* uuid */;
                wellProjectId: Parameters.WellProjectId /* uuid */;
            }
            export type RequestBody = Components.Schemas.CreateTimeSeriesCostOverrideDto;
            namespace Responses {
                export type $200 = Components.Schemas.TimeSeriesCostOverrideDto;
            }
        }
    }
    namespace Projects$ProjectIdCases$CaseIdWellProjects$WellProjectIdGasProducerCostProfileOverride$CostProfileId {
        namespace Put {
            namespace Parameters {
                export type CaseId = string; // uuid
                export type CostProfileId = string; // uuid
                export type ProjectId = string; // uuid
                export type WellProjectId = string; // uuid
            }
            export interface PathParameters {
                projectId: Parameters.ProjectId /* uuid */;
                caseId: Parameters.CaseId /* uuid */;
                wellProjectId: Parameters.WellProjectId /* uuid */;
                costProfileId: Parameters.CostProfileId /* uuid */;
            }
            export type RequestBody = Components.Schemas.UpdateTimeSeriesCostOverrideDto;
            namespace Responses {
                export type $200 = Components.Schemas.TimeSeriesCostOverrideDto;
            }
        }
    }
    namespace Projects$ProjectIdCases$CaseIdWellProjects$WellProjectIdOilProducerCostProfileOverride {
        namespace Post {
            namespace Parameters {
                export type CaseId = string; // uuid
                export type ProjectId = string; // uuid
                export type WellProjectId = string; // uuid
            }
            export interface PathParameters {
                projectId: Parameters.ProjectId /* uuid */;
                caseId: Parameters.CaseId /* uuid */;
                wellProjectId: Parameters.WellProjectId /* uuid */;
            }
            export type RequestBody = Components.Schemas.CreateTimeSeriesCostOverrideDto;
            namespace Responses {
                export type $200 = Components.Schemas.TimeSeriesCostOverrideDto;
            }
        }
    }
    namespace Projects$ProjectIdCases$CaseIdWellProjects$WellProjectIdOilProducerCostProfileOverride$CostProfileId {
        namespace Put {
            namespace Parameters {
                export type CaseId = string; // uuid
                export type CostProfileId = string; // uuid
                export type ProjectId = string; // uuid
                export type WellProjectId = string; // uuid
            }
            export interface PathParameters {
                projectId: Parameters.ProjectId /* uuid */;
                caseId: Parameters.CaseId /* uuid */;
                wellProjectId: Parameters.WellProjectId /* uuid */;
                costProfileId: Parameters.CostProfileId /* uuid */;
            }
            export type RequestBody = Components.Schemas.UpdateTimeSeriesCostOverrideDto;
            namespace Responses {
                export type $200 = Components.Schemas.TimeSeriesCostOverrideDto;
            }
        }
    }
    namespace Projects$ProjectIdCases$CaseIdWellProjects$WellProjectIdWaterInjectorCostProfileOverride {
        namespace Post {
            namespace Parameters {
                export type CaseId = string; // uuid
                export type ProjectId = string; // uuid
                export type WellProjectId = string; // uuid
            }
            export interface PathParameters {
                projectId: Parameters.ProjectId /* uuid */;
                caseId: Parameters.CaseId /* uuid */;
                wellProjectId: Parameters.WellProjectId /* uuid */;
            }
            export type RequestBody = Components.Schemas.CreateTimeSeriesCostOverrideDto;
            namespace Responses {
                export type $200 = Components.Schemas.TimeSeriesCostOverrideDto;
            }
        }
    }
    namespace Projects$ProjectIdCases$CaseIdWellProjects$WellProjectIdWaterInjectorCostProfileOverride$CostProfileId {
        namespace Put {
            namespace Parameters {
                export type CaseId = string; // uuid
                export type CostProfileId = string; // uuid
                export type ProjectId = string; // uuid
                export type WellProjectId = string; // uuid
            }
            export interface PathParameters {
                projectId: Parameters.ProjectId /* uuid */;
                caseId: Parameters.CaseId /* uuid */;
                wellProjectId: Parameters.WellProjectId /* uuid */;
                costProfileId: Parameters.CostProfileId /* uuid */;
            }
            export type RequestBody = Components.Schemas.UpdateTimeSeriesCostOverrideDto;
            namespace Responses {
                export type $200 = Components.Schemas.TimeSeriesCostOverrideDto;
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

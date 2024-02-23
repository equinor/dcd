declare namespace Components {
    namespace Schemas {
        export interface AppraisalWellCostProfileDto {
            id: string; // uuid
            startYear: number; // int32
            values?: number /* double */[] | null;
            sum?: number; // double
            epaVersion: string;
            currency: Currency /* int32 */;
        }
        export type ArtificialLift = 0 | 1 | 2 | 3; // int32
        export interface CapexDto {
            id: string; // uuid
            startYear: number; // int32
            values?: number /* double */[] | null;
            sum?: number; // double
            epaVersion: string;
            currency: Currency /* int32 */;
            drilling?: TimeSeriesCostDto;
            offshoreFacilities?: OffshoreFacilitiesCostProfileDto;
            cessationCost?: CessationCostDto;
        }
        export interface CapexYear {
            values?: number /* double */[] | null;
            startYear?: number | null; // int32
        }
        export interface CaseDto {
            id: string; // uuid
            projectId: string; // uuid
            name: string;
            description: string;
            referenceCase: boolean;
            artificialLift: ArtificialLift /* int32 */;
            productionStrategyOverview: ProductionStrategyOverview /* int32 */;
            producerCount: number; // int32
            gasInjectorCount: number; // int32
            waterInjectorCount: number; // int32
            facilitiesAvailability: number; // double
            capexFactorFeasibilityStudies: number; // double
            capexFactorFEEDStudies: number; // double
            npv: number; // double
            breakEven: number; // double
            host?: string | null;
            dgaDate: string; // date-time
            dgbDate: string; // date-time
            dgcDate: string; // date-time
            apxDate: string; // date-time
            apzDate: string; // date-time
            dG0Date: string; // date-time
            dG1Date: string; // date-time
            dG2Date: string; // date-time
            dG3Date: string; // date-time
            dG4Date: string; // date-time
            createTime: string; // date-time
            modifyTime: string; // date-time
            cessationWellsCost: CessationWellsCostDto;
            cessationWellsCostOverride: CessationWellsCostOverrideDto;
            cessationOffshoreFacilitiesCost: CessationOffshoreFacilitiesCostDto;
            cessationOffshoreFacilitiesCostOverride: CessationOffshoreFacilitiesCostOverrideDto;
            totalFeasibilityAndConceptStudies: TotalFeasibilityAndConceptStudiesDto;
            totalFeasibilityAndConceptStudiesOverride: TotalFeasibilityAndConceptStudiesOverrideDto;
            totalFEEDStudies: TotalFEEDStudiesDto;
            totalFEEDStudiesOverride: TotalFEEDStudiesOverrideDto;
            totalOtherStudies: TotalOtherStudiesDto;
            wellInterventionCostProfile: WellInterventionCostProfileDto;
            wellInterventionCostProfileOverride: WellInterventionCostProfileOverrideDto;
            offshoreFacilitiesOperationsCostProfile: OffshoreFacilitiesOperationsCostProfileDto;
            offshoreFacilitiesOperationsCostProfileOverride: OffshoreFacilitiesOperationsCostProfileOverrideDto;
            historicCostCostProfile: HistoricCostCostProfileDto;
            additionalOPEXCostProfile: AdditionalOPEXCostProfileDto;
            drainageStrategyLink: string; // uuid
            wellProjectLink: string; // uuid
            surfLink: string; // uuid
            substructureLink: string; // uuid
            topsideLink: string; // uuid
            transportLink: string; // uuid
            explorationLink: string; // uuid
            capex: number; // double
            capexYear?: CapexYear;
            sharepointFileId?: string | null;
            sharepointFileName?: string | null;
            sharepointFileUrl?: string | null;
        }
        export interface CaseWithAssetsWrapperDto {
            caseDto?: UpdateCaseDto;
            drainageStrategyDto?: UpdateDrainageStrategyDto;
            wellProjectDto?: UpdateWellProjectDto;
            explorationDto?: UpdateExplorationDto;
            surfDto?: UpdateSurfDto;
            substructureDto?: UpdateSubstructureDto;
            topsideDto?: UpdateTopsideDto;
            transportDto?: UpdateTransportDto;
            wellProjectWellDtos?: UpdateWellProjectWellDto[] | null;
            explorationWellDto?: UpdateExplorationWellDto[] | null;
        }
        export interface CessationCostDto {
            id: string; // uuid
            startYear: number; // int32
            values?: number /* double */[] | null;
            sum?: number; // double
            epaVersion: string;
            currency: Currency /* int32 */;
        }
        export interface CessationCostWrapperDto {
            cessationCostDto?: CessationCostDto;
            cessationWellsCostDto?: CessationWellsCostDto;
            cessationOffshoreFacilitiesCostDto?: CessationOffshoreFacilitiesCostDto;
        }
        export interface CessationOffshoreFacilitiesCostDto {
            id: string; // uuid
            startYear: number; // int32
            values?: number /* double */[] | null;
            sum?: number; // double
            epaVersion: string;
            currency: Currency /* int32 */;
        }
        export interface CessationOffshoreFacilitiesCostOverrideDto {
            id: string; // uuid
            startYear: number; // int32
            values?: number /* double */[] | null;
            sum?: number; // double
            epaVersion: string;
            currency: Currency /* int32 */;
            override: boolean;
        }
        export interface CessationWellsCostDto {
            id: string; // uuid
            startYear: number; // int32
            values?: number /* double */[] | null;
            sum?: number; // double
            epaVersion: string;
            currency: Currency /* int32 */;
        }
        export interface CessationWellsCostOverrideDto {
            id: string; // uuid
            startYear: number; // int32
            values?: number /* double */[] | null;
            sum?: number; // double
            epaVersion: string;
            currency: Currency /* int32 */;
            override: boolean;
        }
        export interface Co2DrillingFlaringFuelTotalsDto {
            co2Drilling: number; // double
            co2Fuel: number; // double
            co2Flaring: number; // double
        }
        export interface Co2EmissionsDto {
            id: string; // uuid
            startYear: number; // int32
            values?: number /* double */[] | null;
            sum?: number; // double
        }
        export interface Co2EmissionsOverrideDto {
            id: string; // uuid
            startYear: number; // int32
            values?: number /* double */[] | null;
            sum?: number; // double
            override: boolean;
        }
        export interface Co2IntensityDto {
            id: string; // uuid
            startYear: number; // int32
            values?: number /* double */[] | null;
            sum?: number; // double
        }
        export interface CompareCasesDto {
            caseId: string; // uuid
            totalOilProduction: number; // double
            totalGasProduction: number; // double
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
        export interface CountryOfficeCostDto {
            id: string; // uuid
            startYear: number; // int32
            values?: number /* double */[] | null;
            sum?: number; // double
            epaVersion: string;
            currency: Currency /* int32 */;
        }
        export interface CreateCaseDto {
            name?: string | null;
            description?: string | null;
            productionStrategyOverview?: ProductionStrategyOverview /* int32 */;
            producerCount?: number; // int32
            gasInjectorCount?: number; // int32
            waterInjectorCount?: number; // int32
            dG4Date?: string; // date-time
        }
        export type Currency = 1 | 2; // int32
        export interface DevelopmentOperationalWellCostsDto {
            id: string; // uuid
            projectId: string; // uuid
            rigUpgrading: number; // double
            rigMobDemob: number; // double
            annualWellInterventionCostPerWell: number; // double
            pluggingAndAbandonment: number; // double
            hasChanges: boolean;
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
            productionProfileOil: ProductionProfileOilDto;
            productionProfileGas: ProductionProfileGasDto;
            productionProfileWater: ProductionProfileWaterDto;
            productionProfileWaterInjection: ProductionProfileWaterInjectionDto;
            fuelFlaringAndLosses: FuelFlaringAndLossesDto;
            fuelFlaringAndLossesOverride: FuelFlaringAndLossesOverrideDto;
            netSalesGas: NetSalesGasDto;
            netSalesGasOverride: NetSalesGasOverrideDto;
            co2Emissions: Co2EmissionsDto;
            co2EmissionsOverride: Co2EmissionsOverrideDto;
            productionProfileNGL: ProductionProfileNGLDto;
            importedElectricity: ImportedElectricityDto;
            importedElectricityOverride: ImportedElectricityOverrideDto;
            co2Intensity: Co2IntensityDto;
            hasChanges?: boolean;
        }
        export interface DrillingScheduleDto {
            id: string; // uuid
            startYear: number; // int32
            values?: number /* int32 */[] | null;
        }
        export interface DriveItemDto {
            name?: string | null;
            id?: string | null;
            sharepointFileUrl?: string | null;
            createdDateTime?: string | null; // date-time
            content?: Stream;
            size?: number | null; // int64
            sharepointIds?: SharepointIds;
            createdBy?: IdentitySet;
            lastModifiedBy?: IdentitySet;
            lastModifiedDateTime?: string | null; // date-time
        }
        export interface ExplorationDto {
            id: string; // uuid
            projectId: string; // uuid
            name: string;
            explorationWellCostProfile: ExplorationWellCostProfileDto;
            appraisalWellCostProfile: AppraisalWellCostProfileDto;
            sidetrackCostProfile: SidetrackCostProfileDto;
            gAndGAdminCost: GAndGAdminCostDto;
            seismicAcquisitionAndProcessing: SeismicAcquisitionAndProcessingDto;
            countryOfficeCost: CountryOfficeCostDto;
            rigMobDemob: number; // double
            currency: Currency /* int32 */;
            explorationWells: ExplorationWellDto[];
            hasChanges?: boolean;
        }
        export interface ExplorationOperationalWellCostsDto {
            id: string; // uuid
            projectId: string; // uuid
            rigUpgrading: number; // double
            explorationRigMobDemob: number; // double
            explorationProjectDrillingCosts: number; // double
            appraisalRigMobDemob: number; // double
            appraisalProjectDrillingCosts: number; // double
            hasChanges: boolean;
        }
        export interface ExplorationWellCostProfileDto {
            id: string; // uuid
            startYear: number; // int32
            values?: number /* double */[] | null;
            sum?: number; // double
            epaVersion: string;
            currency: Currency /* int32 */;
        }
        export interface ExplorationWellDto {
            drillingSchedule: DrillingScheduleDto;
            explorationId: string; // uuid
            wellId: string; // uuid
            hasChanges?: boolean;
        }
        export interface FuelFlaringAndLossesDto {
            id: string; // uuid
            startYear: number; // int32
            values?: number /* double */[] | null;
            sum?: number; // double
        }
        export interface FuelFlaringAndLossesOverrideDto {
            id: string; // uuid
            startYear: number; // int32
            values?: number /* double */[] | null;
            sum?: number; // double
            override: boolean;
        }
        export interface GAndGAdminCostDto {
            id: string; // uuid
            startYear: number; // int32
            values?: number /* double */[] | null;
            sum?: number; // double
            epaVersion: string;
            currency: Currency /* int32 */;
        }
        export interface GasInjectorCostProfileDto {
            id: string; // uuid
            startYear: number; // int32
            values?: number /* double */[] | null;
            sum?: number; // double
            epaVersion: string;
            currency: Currency /* int32 */;
        }
        export interface GasInjectorCostProfileOverrideDto {
            id: string; // uuid
            startYear: number; // int32
            values?: number /* double */[] | null;
            sum?: number; // double
            epaVersion: string;
            currency: Currency /* int32 */;
            override: boolean;
        }
        export interface GasProducerCostProfileDto {
            id: string; // uuid
            startYear: number; // int32
            values?: number /* double */[] | null;
            sum?: number; // double
            epaVersion: string;
            currency: Currency /* int32 */;
        }
        export interface GasProducerCostProfileOverrideDto {
            id: string; // uuid
            startYear: number; // int32
            values?: number /* double */[] | null;
            sum?: number; // double
            epaVersion: string;
            currency: Currency /* int32 */;
            override: boolean;
        }
        export type GasSolution = 0 | 1; // int32
        export interface GeneratedProfilesDto {
            studyCostProfileWrapperDto?: StudyCostProfileWrapperDto;
            opexCostProfileWrapperDto?: OpexCostProfileWrapperDto;
            cessationCostWrapperDto?: CessationCostWrapperDto;
            co2EmissionsDto?: Co2EmissionsDto;
            gAndGAdminCostDto?: GAndGAdminCostDto;
            importedElectricityDto?: ImportedElectricityDto;
            fuelFlaringAndLossesDto?: FuelFlaringAndLossesDto;
            netSalesGasDto?: NetSalesGasDto;
        }
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
        export interface ImportedElectricityDto {
            id: string; // uuid
            startYear: number; // int32
            values?: number /* double */[] | null;
            sum?: number; // double
        }
        export interface ImportedElectricityOverrideDto {
            id: string; // uuid
            startYear: number; // int32
            values?: number /* double */[] | null;
            sum?: number; // double
            override: boolean;
        }
        export type Maturity = 0 | 1 | 2 | 3; // int32
        export interface NetSalesGasDto {
            id: string; // uuid
            startYear: number; // int32
            values?: number /* double */[] | null;
            sum?: number; // double
        }
        export interface NetSalesGasOverrideDto {
            id: string; // uuid
            startYear: number; // int32
            values?: number /* double */[] | null;
            sum?: number; // double
            override: boolean;
        }
        export interface OffshoreFacilitiesCostProfileDto {
            id: string; // uuid
            startYear: number; // int32
            values?: number /* double */[] | null;
            sum?: number; // double
            epaVersion: string;
            currency: Currency /* int32 */;
        }
        export interface HistoricCostCostProfileDto {
            id: string; // uuid
            startYear: number; // int32
            values?: number /* double */[] | null;
            sum?: number; // double
            epaVersion: string;
            currency: Currency /* int32 */;
        }
        export interface OffshoreFacilitiesOperationsCostProfileDto {
            id: string; // uuid
            startYear: number; // int32
            values?: number /* double */[] | null;
            sum?: number; // double
            epaVersion: string;
            currency: Currency /* int32 */;
        }
        export interface OffshoreFacilitiesOperationsCostProfileOverrideDto {
            id: string; // uuid
            startYear: number; // int32
            values?: number /* double */[] | null;
            sum?: number; // double
            epaVersion: string;
            currency: Currency /* int32 */;
            override: boolean;
        }
        export interface OilProducerCostProfileDto {
            id: string; // uuid
            startYear: number; // int32
            values?: number /* double */[] | null;
            sum?: number; // double
            epaVersion: string;
            currency: Currency /* int32 */;
        }
        export interface OilProducerCostProfileOverrideDto {
            id: string; // uuid
            startYear: number; // int32
            values?: number /* double */[] | null;
            sum?: number; // double
            epaVersion: string;
            currency: Currency /* int32 */;
            override: boolean;
        }
        export interface OpexCostProfileDto {
            id: string; // uuid
            startYear: number; // int32
            values?: number /* double */[] | null;
            sum?: number; // double
            epaVersion: string;
            currency: Currency /* int32 */;
        }
        export interface OpexCostProfileWrapperDto {
            opexCostProfileDto?: OpexCostProfileDto;
            wellInterventionCostProfileDto?: WellInterventionCostProfileDto;
            offshoreFacilitiesOperationsCostProfileDto?: OffshoreFacilitiesOperationsCostProfileDto;
            historicCostCostProfileDto?: HistoricCostCostProfileDto;
            additionalOPEXCostProfileDto?: AdditionalOPEXCostProfileDto;
        }
        export type PhysUnit = 0 | 1; // int32
        export interface ProductionAndSalesVolumesDto {
            startYear?: number; // int32
            totalAndAnnualOil?: ProductionProfileOilDto;
            totalAndAnnualSalesGas?: NetSalesGasDto;
            co2Emissions?: Co2EmissionsDto;
            importedElectricity?: ImportedElectricityDto;
        }
        export type ProductionFlowline = 0 | 1 | 2 | 3 | 4 | 5 | 6 | 7 | 8 | 9 | 10 | 11 | 12 | 13; // int32
        export interface ProductionProfileGasDto {
            id: string; // uuid
            startYear: number; // int32
            values?: number /* double */[] | null;
            sum?: number; // double
        }
        export interface ProductionProfileNGLDto {
            id: string; // uuid
            startYear: number; // int32
            values?: number /* double */[] | null;
            sum?: number; // double
        }
        export interface ProductionProfileOilDto {
            id: string; // uuid
            startYear: number; // int32
            values?: number /* double */[] | null;
            sum?: number; // double
        }
        export interface ProductionProfileWaterDto {
            id: string; // uuid
            startYear: number; // int32
            values?: number /* double */[] | null;
            sum?: number; // double
        }
        export interface ProductionProfileWaterInjectionDto {
            id: string; // uuid
            startYear: number; // int32
            values?: number /* double */[] | null;
            sum?: number; // double
        }
        export type ProductionStrategyOverview = 0 | 1 | 2 | 3 | 4; // int32
        export type ProjectCategory = 0 | 1 | 2 | 3 | 4 | 5 | 6 | 7 | 8 | 9 | 10 | 11 | 12 | 13 | 14 | 15 | 16 | 17 | 18 | 19 | 20 | 21; // int32
        export interface ProjectDto {
            id: string; // uuid
            name: string;
            commonLibraryId: string; // uuid
            fusionProjectId: string; // uuid
            referenceCaseId: string; // uuid
            commonLibraryName: string;
            description: string;
            country: string;
            currency: Currency /* int32 */;
            physUnit: PhysUnit /* int32 */;
            createDate: string; // date-time
            projectPhase: ProjectPhase /* int32 */;
            projectCategory: ProjectCategory /* int32 */;
            explorationOperationalWellCosts: ExplorationOperationalWellCostsDto;
            developmentOperationalWellCosts: DevelopmentOperationalWellCostsDto;
            cases: CaseDto[];
            wells: WellDto[];
            explorations: ExplorationDto[];
            surfs: SurfDto[];
            substructures: SubstructureDto[];
            topsides: TopsideDto[];
            transports: TransportDto[];
            drainageStrategies: DrainageStrategyDto[];
            wellProjects: WellProjectDto[];
            sharepointSiteUrl?: string | null;
            cO2RemovedFromGas: number; // double
            cO2EmissionFromFuelGas: number; // double
            flaredGasPerProducedVolume: number; // double
            cO2EmissionsFromFlaredGas: number; // double
            cO2Vented: number; // double
            dailyEmissionFromDrillingRig: number; // double
            averageDevelopmentDrillingDays: number; // double
            hasChanges: boolean;
        }
        export type ProjectPhase = 0 | 1 | 2 | 3 | 4 | 5 | 6 | 7 | 8 | 9; // int32
        export interface ProjectWithGeneratedProfilesDto {
            projectDto: ProjectDto;
            generatedProfilesDto: GeneratedProfilesDto;
        }
        export interface STEACaseDto {
            name?: string | null;
            startYear?: number; // int32
            exploration?: TimeSeriesCostDto;
            capex?: CapexDto;
            productionAndSalesVolumes?: ProductionAndSalesVolumesDto;
            offshoreFacilitiesCostProfileDto?: OffshoreFacilitiesCostProfileDto;
            studyCostProfile?: StudyCostProfileDto;
            opexCostProfile?: OpexCostProfileDto;
        }
        export interface STEAProjectDto {
            name?: string | null;
            startYear?: number; // int32
            steaCases?: STEACaseDto[] | null;
        }
        export interface SeismicAcquisitionAndProcessingDto {
            id: string; // uuid
            startYear: number; // int32
            values?: number /* double */[] | null;
            sum?: number; // double
            epaVersion: string;
            currency: Currency /* int32 */;
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
        export interface SidetrackCostProfileDto {
            id: string; // uuid
            startYear: number; // int32
            values?: number /* double */[] | null;
            sum?: number; // double
            epaVersion: string;
            currency: Currency /* int32 */;
        }
        export type Source = 0 | 1; // int32
        export interface Stream {
            canRead?: boolean;
            canWrite?: boolean;
            canSeek?: boolean;
            canTimeout?: boolean;
            length?: number; // int64
            position?: number; // int64
            readTimeout?: number; // int32
            writeTimeout?: number; // int32
        }
        export interface StudyCostProfileDto {
            id: string; // uuid
            startYear: number; // int32
            values?: number /* double */[] | null;
            sum?: number; // double
            epaVersion: string;
            currency: Currency /* int32 */;
        }
        export interface StudyCostProfileWrapperDto {
            studyCostProfileDto?: StudyCostProfileDto;
            totalFeasibilityAndConceptStudiesDto?: TotalFeasibilityAndConceptStudiesDto;
            totalFEEDStudiesDto?: TotalFEEDStudiesDto;
            totalOtherStudiesDto?: totalOtherStudiesDto;
        }
        export interface SubstructureCessationCostProfileDto {
            id: string; // uuid
            startYear: number; // int32
            values?: number /* double */[] | null;
            sum?: number; // double
            epaVersion: string;
            currency: Currency /* int32 */;
        }
        export interface SubstructureCostProfileDto {
            id: string; // uuid
            startYear: number; // int32
            values?: number /* double */[] | null;
            sum?: number; // double
            epaVersion: string;
            currency: Currency /* int32 */;
        }
        export interface SubstructureCostProfileOverrideDto {
            id: string; // uuid
            startYear: number; // int32
            values?: number /* double */[] | null;
            sum?: number; // double
            epaVersion: string;
            currency: Currency /* int32 */;
            override: boolean;
        }
        export interface SubstructureDto {
            id: string; // uuid
            name: string;
            projectId: string; // uuid
            costProfile: SubstructureCostProfileDto;
            costProfileOverride: SubstructureCostProfileOverrideDto;
            cessationCostProfile: SubstructureCessationCostProfileDto;
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
            hasChanges?: boolean;
        }
        export interface SurfCessationCostProfileDto {
            id: string; // uuid
            startYear: number; // int32
            values?: number /* double */[] | null;
            sum?: number; // double
            epaVersion: string;
            currency: Currency /* int32 */;
        }
        export interface SurfCostProfileDto {
            id: string; // uuid
            startYear: number; // int32
            values?: number /* double */[] | null;
            sum?: number; // double
            epaVersion: string;
            currency: Currency /* int32 */;
        }
        export interface SurfCostProfileOverrideDto {
            id: string; // uuid
            startYear: number; // int32
            values?: number /* double */[] | null;
            sum?: number; // double
            epaVersion: string;
            currency: Currency /* int32 */;
            override: boolean;
        }
        export interface SurfDto {
            id: string; // uuid
            name: string;
            projectId: string; // uuid
            costProfile: SurfCostProfileDto;
            costProfileOverride: SurfCostProfileOverrideDto;
            cessationCostProfile: SurfCessationCostProfileDto;
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
            hasChanges?: boolean;
        }
        export interface TechnicalInputDto {
            developmentOperationalWellCostsDto?: DevelopmentOperationalWellCostsDto;
            explorationOperationalWellCostsDto?: ExplorationOperationalWellCostsDto;
            wellDtos?: WellDto[] | null;
            projectDto?: ProjectDto;
            explorationDto?: ExplorationDto;
            wellProjectDto?: WellProjectDto;
            caseId?: string | null; // uuid
        }
        export interface TimeSeriesCostDto {
            id: string; // uuid
            startYear: number; // int32
            values?: number /* double */[] | null;
            sum?: number; // double
            epaVersion: string;
            currency: Currency /* int32 */;
        }
        export interface TopsideCessationCostProfileDto {
            id: string; // uuid
            startYear: number; // int32
            values?: number /* double */[] | null;
            sum?: number; // double
            epaVersion: string;
            currency: Currency /* int32 */;
        }
        export interface TopsideCostProfileDto {
            id: string; // uuid
            startYear: number; // int32
            values?: number /* double */[] | null;
            sum?: number; // double
            epaVersion: string;
            currency: Currency /* int32 */;
        }
        export interface TopsideCostProfileOverrideDto {
            id: string; // uuid
            startYear: number; // int32
            values?: number /* double */[] | null;
            sum?: number; // double
            epaVersion: string;
            currency: Currency /* int32 */;
            override: boolean;
        }
        export interface TopsideDto {
            id: string; // uuid
            name: string;
            projectId: string; // uuid
            costProfile: TopsideCostProfileDto;
            costProfileOverride: TopsideCostProfileOverrideDto;
            cessationCostProfile: TopsideCessationCostProfileDto;
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
            hasChanges?: boolean;
        }
        export interface TotalFEEDStudiesDto {
            id: string; // uuid
            startYear: number; // int32
            values?: number /* double */[] | null;
            sum?: number; // double
            epaVersion: string;
            currency: Currency /* int32 */;
        }
        export interface TotalFEEDStudiesOverrideDto {
            id: string; // uuid
            startYear: number; // int32
            values?: number /* double */[] | null;
            sum?: number; // double
            epaVersion: string;
            currency: Currency /* int32 */;
            override: boolean;
        }
        export interface TotalFeasibilityAndConceptStudiesDto {
            id: string; // uuid
            startYear: number; // int32
            values?: number /* double */[] | null;
            sum?: number; // double
            epaVersion: string;
            currency: Currency /* int32 */;
        }
        export interface TotalFeasibilityAndConceptStudiesOverrideDto {
            id: string; // uuid
            startYear: number; // int32
            values?: number /* double */[] | null;
            sum?: number; // double
            epaVersion: string;
            currency: Currency /* int32 */;
            override: boolean;
        }
        export interface TransportCessationCostProfileDto {
            id: string; // uuid
            startYear: number; // int32
            values?: number /* double */[] | null;
            sum?: number; // double
            epaVersion: string;
            currency: Currency /* int32 */;
        }
        export interface TransportCostProfileDto {
            id: string; // uuid
            startYear: number; // int32
            values?: number /* double */[] | null;
            sum?: number; // double
            epaVersion: string;
            currency: Currency /* int32 */;
        }
        export interface TransportCostProfileOverrideDto {
            id: string; // uuid
            startYear: number; // int32
            values?: number /* double */[] | null;
            sum?: number; // double
            epaVersion: string;
            currency: Currency /* int32 */;
            override: boolean;
        }
        export interface TransportDto {
            id: string; // uuid
            name: string;
            projectId: string; // uuid
            costProfile: TransportCostProfileDto;
            costProfileOverride: TransportCostProfileOverrideDto;
            cessationCostProfile: TransportCessationCostProfileDto;
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
            hasChanges?: boolean;
        }
        export interface UpdateCaseDto {
            name?: string | null;
            description?: string | null;
            referenceCase?: boolean | null;
            artificialLift?: ArtificialLift /* int32 */;
            productionStrategyOverview?: ProductionStrategyOverview /* int32 */;
            producerCount?: number | null; // int32
            gasInjectorCount?: number | null; // int32
            waterInjectorCount?: number | null; // int32
            facilitiesAvailability?: number | null; // double
            capexFactorFeasibilityStudies?: number | null; // double
            capexFactorFEEDStudies?: number | null; // double
            npv?: number | null; // double
            breakEven?: number | null; // double
            host?: string | null;
            dgaDate?: string; // date-time
            dgbDate?: string; // date-time
            dgcDate?: string; // date-time
            apxDate?: string; // date-time
            apzDate?: string; // date-time
            dG0Date?: string; // date-time
            dG1Date?: string; // date-time
            dG2Date?: string; // date-time
            dG3Date?: string; // date-time
            dG4Date?: string; // date-time
            cessationWellsCostOverride?: UpdateCessationWellsCostOverrideDto;
            cessationOffshoreFacilitiesCostOverride?: UpdateCessationOffshoreFacilitiesCostOverrideDto;
            totalFeasibilityAndConceptStudiesOverride?: UpdateTotalFeasibilityAndConceptStudiesOverrideDto;
            totalFEEDStudiesOverride?: UpdateTotalFEEDStudiesOverrideDto;
            totalOtherStudies?:TotalOtherStudiesDto; 
            wellInterventionCostProfileOverride?: UpdateWellInterventionCostProfileOverrideDto;
            offshoreFacilitiesOperationsCostProfileOverride?: UpdateOffshoreFacilitiesOperationsCostProfileOverrideDto;
            capex?: number; // double
            capexYear?: CapexYear;
            sharepointFileId?: string | null;
            sharepointFileName?: string | null;
            sharepointFileUrl?: string | null;
        }
        export interface UpdateCessationOffshoreFacilitiesCostOverrideDto {
            startYear?: number; // int32
            values?: number /* double */[] | null;
            currency?: Currency /* int32 */;
            override?: boolean;
        }
        export interface UpdateCessationWellsCostOverrideDto {
            startYear?: number; // int32
            values?: number /* double */[] | null;
            currency?: Currency /* int32 */;
            override?: boolean;
        }
        export interface UpdateCo2EmissionsOverrideDto {
            startYear?: number; // int32
            values?: number /* double */[] | null;
            override?: boolean;
        }
        export interface UpdateDrainageStrategyDto {
            name?: string | null;
            description?: string | null;
            nglYield?: number; // double
            producerCount?: number; // int32
            gasInjectorCount?: number; // int32
            waterInjectorCount?: number; // int32
            artificialLift?: ArtificialLift /* int32 */;
            gasSolution?: GasSolution /* int32 */;
            fuelFlaringAndLossesOverride?: UpdateFuelFlaringAndLossesOverrideDto;
            netSalesGasOverride?: UpdateNetSalesGasOverrideDto;
            co2EmissionsOverride?: UpdateCo2EmissionsOverrideDto;
        }
        export interface UpdateExplorationDto {
            name?: string | null;
            rigMobDemob?: number; // double
            currency?: Currency /* int32 */;
            explorationWells?: ExplorationWellDto[] | null;
        }
        export interface UpdateExplorationWellDto {
            drillingSchedule?: DrillingScheduleDto;
            explorationId: string; // uuid
            wellId: string; // uuid
        }
        export interface UpdateFuelFlaringAndLossesOverrideDto {
            startYear?: number; // int32
            values?: number /* double */[] | null;
            override?: boolean;
        }
        export interface UpdateGasInjectorCostProfileOverrideDto {
            startYear?: number; // int32
            values?: number /* double */[] | null;
            currency?: Currency /* int32 */;
            override?: boolean;
        }
        export interface UpdateGasProducerCostProfileOverrideDto {
            startYear?: number; // int32
            values?: number /* double */[] | null;
            currency?: Currency /* int32 */;
            override?: boolean;
        }
        export interface UpdateNetSalesGasOverrideDto {
            startYear?: number; // int32
            values?: number /* double */[] | null;
            override?: boolean;
        }
        export interface UpdateOffshoreFacilitiesOperationsCostProfileOverrideDto {
            startYear?: number; // int32
            values?: number /* double */[] | null;
            currency?: Currency /* int32 */;
            override?: boolean;
        }
        export interface UpdateOilProducerCostProfileOverrideDto {
            startYear?: number; // int32
            values?: number /* double */[] | null;
            currency?: Currency /* int32 */;
            override?: boolean;
        }
        export interface UpdateSubstructureCostProfileOverrideDto {
            startYear?: number; // int32
            values?: number /* double */[] | null;
            currency?: Currency /* int32 */;
            override?: boolean;
        }
        export interface UpdateSubstructureDto {
            name?: string | null;
            costProfileOverride?: UpdateSubstructureCostProfileOverrideDto;
            dryWeight?: number; // double
            maturity?: Maturity /* int32 */;
            currency?: Currency /* int32 */;
            approvedBy?: string | null;
            costYear?: number; // int32
            prospVersion?: string | null; // date-time
            concept?: Concept /* int32 */;
            dG3Date?: string | null; // date-time
            dG4Date?: string | null; // date-time
        }
        export interface UpdateSurfCostProfileOverrideDto {
            startYear?: number; // int32
            values?: number /* double */[] | null;
            currency?: Currency /* int32 */;
            override?: boolean;
        }
        export interface UpdateSurfDto {
            name?: string | null;
            costProfileOverride?: UpdateSurfCostProfileOverrideDto;
            cessationCost?: number; // double
            maturity?: Maturity /* int32 */;
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
            prospVersion?: string | null; // date-time
            approvedBy?: string | null;
            dG3Date?: string | null; // date-time
            dG4Date?: string | null; // date-time
            hasChanges?: boolean;
        }
        export interface UpdateTopsideCostProfileOverrideDto {
            startYear?: number; // int32
            values?: number /* double */[] | null;
            currency?: Currency /* int32 */;
            override?: boolean;
        }
        export interface UpdateTopsideDto {
            name?: string | null;
            costProfileOverride?: UpdateTopsideCostProfileOverrideDto;
            dryWeight?: number; // double
            oilCapacity?: number; // double
            gasCapacity?: number; // double
            waterInjectionCapacity?: number; // double
            artificialLift?: ArtificialLift /* int32 */;
            maturity?: Maturity /* int32 */;
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
            prospVersion?: string | null; // date-time
            approvedBy?: string | null;
            dG3Date?: string | null; // date-time
            dG4Date?: string | null; // date-time
            facilityOpex?: number; // double
            peakElectricityImported?: number; // double
        }
        export interface UpdateTotalFEEDStudiesOverrideDto {
            startYear?: number; // int32
            values?: number /* double */[] | null;
            currency?: Currency /* int32 */;
            override?: boolean;
        }
        export interface UpdateTotalFeasibilityAndConceptStudiesOverrideDto {
            startYear?: number; // int32
            values?: number /* double */[] | null;
            currency?: Currency /* int32 */;
            override?: boolean;
        }
        export interface TotalOtherStudiesDto {
            id: string; // uuid
            startYear: number; // int32
            values?: number /* double */[] | null;
            sum?: number; // double
            epaVersion: string;
            currency: Currency /* int32 */;
        }
        export interface UpdateTransportCostProfileOverrideDto {
            i?: string; // uuid
            startYear?: number; // int32
            values?: number /* double */[] | null;
            currency?: Currency /* int32 */;
            override?: boolean;
        }
        export interface UpdateTransportDto {
            name?: string | null;
            costProfileOverride?: UpdateTransportCostProfileOverrideDto;
            maturity?: Maturity /* int32 */;
            gasExportPipelineLength?: number; // double
            oilExportPipelineLength?: number; // double
            currency?: Currency /* int32 */;
            costYear?: number; // int32
            prospVersion?: string | null; // date-time
            dG3Date?: string | null; // date-time
            dG4Date?: string | null; // date-time
        }
        export interface UpdateWaterInjectorCostProfileOverrideDto {
            startYear?: number; // int32
            values?: number /* double */[] | null;
            currency?: Currency /* int32 */;
            override?: boolean;
        }
        export interface UpdateWellInterventionCostProfileOverrideDto {
            startYear?: number; // int32
            values?: number /* double */[] | null;
            currency?: Currency /* int32 */;
            override?: boolean;
        }
        export interface UpdateWellProjectDto {
            name?: string | null;
            oilProducerCostProfileOverride?: UpdateOilProducerCostProfileOverrideDto;
            gasProducerCostProfileOverride?: UpdateGasProducerCostProfileOverrideDto;
            waterInjectorCostProfileOverride?: UpdateWaterInjectorCostProfileOverrideDto;
            gasInjectorCostProfileOverride?: UpdateGasInjectorCostProfileOverrideDto;
            artificialLift?: ArtificialLift /* int32 */;
            currency?: Currency /* int32 */;
            wellProjectWells?: WellProjectWellDto[] | null;
        }
        export interface UpdateWellProjectWellDto {
            drillingSchedule?: DrillingScheduleDto;
            wellProjectId: string; // uuid
            wellId: string; // uuid
        }
        export interface UrlDto {
            url?: string | null;
        }
        export interface WaterInjectorCostProfileDto {
            id: string; // uuid
            startYear: number; // int32
            values?: number /* double */[] | null;
            sum?: number; // double
            epaVersion: string;
            currency: Currency /* int32 */;
        }
        export interface WaterInjectorCostProfileOverrideDto {
            id: string; // uuid
            startYear: number; // int32
            values?: number /* double */[] | null;
            sum?: number; // double
            epaVersion: string;
            currency: Currency /* int32 */;
            override: boolean;
        }
        export type WellCategory = 0 | 1 | 2 | 3 | 4 | 5 | 6 | 7; // int32
        export interface WellDto {
            id: string; // uuid
            projectId: string; // uuid
            name: string;
            wellInterventionCost: number; // double
            plugingAndAbandonmentCost: number; // double
            wellCategory: WellCategory /* int32 */;
            wellCost: number; // double
            drillingDays: number; // double
            hasChanges?: boolean;
            hasCostChanges?: boolean;
        }
        export interface WellInterventionCostProfileDto {
            id: string; // uuid
            startYear: number; // int32
            values?: number /* double */[] | null;
            sum?: number; // double
            epaVersion: string;
            currency: Currency /* int32 */;
        }
        export interface WellInterventionCostProfileOverrideDto {
            id: string; // uuid
            startYear: number; // int32
            values?: number /* double */[] | null;
            sum?: number; // double
            epaVersion: string;
            currency: Currency /* int32 */;
            override: boolean;
        }
        export interface AdditionalOPEXCostProfileDto {
            id: string; // uuid
            startYear: number; // int32
            values?: number /* double */[] | null;
            sum?: number; // double
            epaVersion: string;
            currency: Currency /* int32 */;
        }
        export interface WellProjectDto {
            id: string; // uuid
            projectId: string; // uuid
            name: string;
            oilProducerCostProfile: OilProducerCostProfileDto;
            oilProducerCostProfileOverride: OilProducerCostProfileOverrideDto;
            gasProducerCostProfile: GasProducerCostProfileDto;
            gasProducerCostProfileOverride: GasProducerCostProfileOverrideDto;
            waterInjectorCostProfile: WaterInjectorCostProfileDto;
            waterInjectorCostProfileOverride: WaterInjectorCostProfileOverrideDto;
            gasInjectorCostProfile: GasInjectorCostProfileDto;
            gasInjectorCostProfileOverride: GasInjectorCostProfileOverrideDto;
            artificialLift: ArtificialLift /* int32 */;
            currency: Currency /* int32 */;
            wellProjectWells: WellProjectWellDto[];
            hasChanges?: boolean;
        }
        export interface WellProjectWellDto {
            drillingSchedule: DrillingScheduleDto;
            wellProjectId: string; // uuid
            wellId: string; // uuid
            hasChanges?: boolean;
        }
    }
}
declare namespace Paths {
    namespace CreateProject {
        export type RequestBody = Components.Schemas.ProjectDto;
        namespace Responses {
            export type $200 = Components.Schemas.ProjectDto;
        }
    }
    namespace CreateProjectFromContextId {
        namespace Parameters {
            export type ContextId = string; // uuid
        }
        export interface QueryParameters {
            contextId?: Parameters.ContextId /* uuid */;
        }
        namespace Responses {
            export type $200 = Components.Schemas.ProjectDto;
        }
    }
    namespace DeleteWell {
        namespace Parameters {
            export type ProjectId = string;
            export type WellId = string; // uuid
        }
        export interface PathParameters {
            wellId: Parameters.WellId /* uuid */;
            projectId: Parameters.ProjectId;
        }
        namespace Responses {
            export type $200 = Components.Schemas.ProjectDto;
        }
    }
    namespace Duplicate {
        namespace Parameters {
            export type CopyCaseId = string; // uuid
            export type ProjectId = string;
        }
        export interface PathParameters {
            projectId: Parameters.ProjectId;
        }
        export interface QueryParameters {
            copyCaseId?: Parameters.CopyCaseId /* uuid */;
        }
        namespace Responses {
            export type $200 = Components.Schemas.ProjectDto;
        }
    }
    namespace ExcelToSTEA {
        namespace Parameters {
            export type ProjectId = string; // uuid
        }
        export interface PathParameters {
            ProjectId: Parameters.ProjectId /* uuid */;
        }
        namespace Responses {
            export interface $200 {
            }
        }
    }
    namespace GetInputToSTEA {
        namespace Parameters {
            export type ProjectId = string; // uuid
        }
        export interface PathParameters {
            ProjectId: Parameters.ProjectId /* uuid */;
        }
        namespace Responses {
            export type $200 = Components.Schemas.STEAProjectDto;
        }
    }
    namespace GetProject {
        namespace Parameters {
            export type ProjectId = string; // uuid
        }
        export interface PathParameters {
            projectId: Parameters.ProjectId /* uuid */;
        }
        namespace Responses {
            export type $200 = Components.Schemas.ProjectDto;
        }
    }
    namespace GetSharePointFileNamesAndId {
        export type RequestBody = Components.Schemas.UrlDto;
        namespace Responses {
            export type $200 = Components.Schemas.DriveItemDto[];
        }
    }
    namespace ImportFilesFromSharepointAsync {
        namespace Parameters {
            export type ProjectId = string; // uuid
        }
        export interface PathParameters {
            projectId: Parameters.ProjectId /* uuid */;
        }
        export interface QueryParameters {
            projectId?: Parameters.ProjectId /* uuid */;
        }
        export type RequestBody = Components.Schemas.SharePointImportDto[];
        namespace Responses {
            export type $200 = Components.Schemas.ProjectDto;
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
                export type $200 = Components.Schemas.ProjectDto;
            }
        }
    }
    namespace Projects$ProjectIdCases$CaseId {
        namespace Delete {
            namespace Parameters {
                export type CaseId = string; // uuid
                export type ProjectId = string;
            }
            export interface PathParameters {
                caseId: Parameters.CaseId /* uuid */;
                projectId: Parameters.ProjectId;
            }
            namespace Responses {
                export type $200 = Components.Schemas.ProjectDto;
            }
        }
        namespace Put {
            namespace Parameters {
                export type CaseId = string; // uuid
                export type ProjectId = string;
            }
            export interface PathParameters {
                caseId: Parameters.CaseId /* uuid */;
                projectId: Parameters.ProjectId;
            }
            export type RequestBody = Components.Schemas.CaseDto;
            namespace Responses {
                export type $200 = Components.Schemas.ProjectDto;
            }
        }
    }
    namespace Projects$ProjectIdCases$CaseIdCessation {
        namespace Get {
            namespace Parameters {
                export type CaseId = string; // uuid
                export type ProjectId = string;
            }
            export interface PathParameters {
                caseId: Parameters.CaseId /* uuid */;
                projectId: Parameters.ProjectId;
            }
            namespace Responses {
                export type $200 = Components.Schemas.CessationCostWrapperDto;
            }
        }
    }
    namespace Projects$ProjectIdCases$CaseIdCo2drillingflaringfueltotals {
        namespace Get {
            namespace Parameters {
                export type CaseId = string; // uuid
                export type ProjectId = string;
            }
            export interface PathParameters {
                caseId: Parameters.CaseId /* uuid */;
                projectId: Parameters.ProjectId;
            }
            namespace Responses {
                export type $200 = Components.Schemas.Co2DrillingFlaringFuelTotalsDto;
            }
        }
    }
    namespace Projects$ProjectIdCases$CaseIdCo2intensity {
        namespace Get {
            namespace Parameters {
                export type CaseId = string; // uuid
                export type ProjectId = string;
            }
            export interface PathParameters {
                caseId: Parameters.CaseId /* uuid */;
                projectId: Parameters.ProjectId;
            }
            namespace Responses {
                export type $200 = Components.Schemas.Co2IntensityDto;
            }
        }
    }
    namespace Projects$ProjectIdCases$CaseIdCo2intensitytotal {
        namespace Get {
            namespace Parameters {
                export type CaseId = string; // uuid
                export type ProjectId = string;
            }
            export interface PathParameters {
                caseId: Parameters.CaseId /* uuid */;
                projectId: Parameters.ProjectId;
            }
            namespace Responses {
                export type $200 = number; // double
            }
        }
    }
    namespace Projects$ProjectIdCases$CaseIdOpex {
        namespace Get {
            namespace Parameters {
                export type CaseId = string; // uuid
                export type ProjectId = string;
            }
            export interface PathParameters {
                caseId: Parameters.CaseId /* uuid */;
                projectId: Parameters.ProjectId;
            }
            namespace Responses {
                export type $200 = Components.Schemas.OpexCostProfileWrapperDto;
            }
        }
    }
    namespace Projects$ProjectIdCases$CaseIdStudy {
        namespace Get {
            namespace Parameters {
                export type CaseId = string; // uuid
                export type ProjectId = string;
            }
            export interface PathParameters {
                caseId: Parameters.CaseId /* uuid */;
                projectId: Parameters.ProjectId;
            }
            namespace Responses {
                export type $200 = Components.Schemas.StudyCostProfileWrapperDto;
            }
        }
    }
    namespace Projects$ProjectIdTechnicalInput {
        namespace Put {
            namespace Parameters {
                export type ProjectId = string; // uuid
            }
            export interface PathParameters {
                projectId: Parameters.ProjectId /* uuid */;
            }
            export type RequestBody = Components.Schemas.TechnicalInputDto;
            namespace Responses {
                export type $200 = Components.Schemas.TechnicalInputDto;
            }
        }
    }
    namespace UpdateCaseWithAssets {
        namespace Parameters {
            export type CaseId = string; // uuid
            export type ProjectId = string; // uuid
        }
        export interface PathParameters {
            projectId: Parameters.ProjectId /* uuid */;
            caseId: Parameters.CaseId /* uuid */;
        }
        export type RequestBody = Components.Schemas.CaseWithAssetsWrapperDto;
        namespace Responses {
            export type $200 = Components.Schemas.ProjectWithGeneratedProfilesDto;
        }
    }
    namespace UpdateProject {
        export type RequestBody = Components.Schemas.ProjectDto;
        namespace Responses {
            export type $200 = Components.Schemas.ProjectDto;
        }
    }
}

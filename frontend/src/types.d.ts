declare namespace Components {
    namespace Schemas {
        export interface AppraisalWellCostProfileDto {
            id?: string; // uuid
            startYear?: number; // int32
            values?: number /* double */[] | null;
            sum?: number; // double
            epaVersion?: string | null;
            currency?: Currency /* int32 */;
            override?: boolean;
        }
        export type ArtificialLift = 0 | 1 | 2 | 3; // int32
        export interface CapexDto {
            id?: string; // uuid
            startYear?: number; // int32
            values?: number /* double */[] | null;
            sum?: number; // double
            epaVersion?: string | null;
            currency?: Currency /* int32 */;
            drilling?: TimeSeriesCostDto;
            offshoreFacilities?: OffshoreFacilitiesCostProfileDto;
            cessationCost?: CessationCostDto;
        }
        export interface CapexYear {
            values?: number /* double */[] | null;
            startYear?: number | null; // int32
        }
        export interface CaseDto {
            id?: string; // uuid
            projectId?: string; // uuid
            name?: string | null;
            description?: string | null;
            referenceCase?: boolean;
            artificialLift?: ArtificialLift /* int32 */;
            productionStrategyOverview?: ProductionStrategyOverview /* int32 */;
            producerCount?: number; // int32
            gasInjectorCount?: number; // int32
            waterInjectorCount?: number; // int32
            facilitiesAvailability?: number; // double
            capexFactorFeasibilityStudies?: number; // double
            capexFactorFEEDStudies?: number; // double
            npv?: number; // double
            breakEven?: number; // double
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
            createTime?: string; // date-time
            modifyTime?: string; // date-time
            cessationWellsCost?: CessationWellsCostDto;
            cessationWellsCostOverride?: CessationWellsCostOverrideDto;
            cessationOffshoreFacilitiesCost?: CessationOffshoreFacilitiesCostDto;
            cessationOffshoreFacilitiesCostOverride?: CessationOffshoreFacilitiesCostOverrideDto;
            totalFeasibilityAndConceptStudies?: TotalFeasibilityAndConceptStudiesDto;
            totalFeasibilityAndConceptStudiesOverride?: TotalFeasibilityAndConceptStudiesOverrideDto;
            totalFEEDStudies?: TotalFEEDStudiesDto;
            totalFEEDStudiesOverride?: TotalFEEDStudiesOverrideDto;
            wellInterventionCostProfile?: WellInterventionCostProfileDto;
            wellInterventionCostProfileOverride?: WellInterventionCostProfileOverrideDto;
            offshoreFacilitiesOperationsCostProfile?: OffshoreFacilitiesOperationsCostProfileDto;
            offshoreFacilitiesOperationsCostProfileOverride?: OffshoreFacilitiesOperationsCostProfileOverrideDto;
            drainageStrategyLink?: string; // uuid
            wellProjectLink?: string; // uuid
            surfLink?: string; // uuid
            substructureLink?: string; // uuid
            topsideLink?: string; // uuid
            transportLink?: string; // uuid
            explorationLink?: string; // uuid
            capex?: number; // double
            capexYear?: CapexYear;
            cessationCost?: CessationCostDto;
            sharepointFileId?: string | null;
            sharepointFileName?: string | null;
            sharepointFileUrl?: string | null;
            hasChanges?: boolean;
        }
        export interface CaseWithAssetsWrapperDto {
            caseDto?: CaseDto;
            drainageStrategyDto?: DrainageStrategyDto;
            wellProjectDto?: WellProjectDto;
            explorationDto?: ExplorationDto;
            surfDto?: SurfDto;
            substructureDto?: SubstructureDto;
            topsideDto?: TopsideDto;
            transportDto?: TransportDto;
            wellProjectWellDtos?: WellProjectWellDto[] | null;
            explorationWellDto?: ExplorationWellDto[] | null;
        }
        export interface CessationCostDto {
            id?: string; // uuid
            startYear?: number; // int32
            values?: number /* double */[] | null;
            sum?: number; // double
            epaVersion?: string | null;
            currency?: Currency /* int32 */;
        }
        export interface CessationCostWrapperDto {
            cessationCostDto?: CessationCostDto;
            cessationWellsCostDto?: CessationWellsCostDto;
            cessationOffshoreFacilitiesCostDto?: CessationOffshoreFacilitiesCostDto;
        }
        export interface CessationOffshoreFacilitiesCostDto {
            id?: string; // uuid
            startYear?: number; // int32
            values?: number /* double */[] | null;
            sum?: number; // double
            epaVersion?: string | null;
            currency?: Currency /* int32 */;
        }
        export interface CessationOffshoreFacilitiesCostOverrideDto {
            id?: string; // uuid
            startYear?: number; // int32
            values?: number /* double */[] | null;
            sum?: number; // double
            epaVersion?: string | null;
            currency?: Currency /* int32 */;
            override?: boolean;
        }
        export interface CessationWellsCostDto {
            id?: string; // uuid
            startYear?: number; // int32
            values?: number /* double */[] | null;
            sum?: number; // double
            epaVersion?: string | null;
            currency?: Currency /* int32 */;
        }
        export interface CessationWellsCostOverrideDto {
            id?: string; // uuid
            startYear?: number; // int32
            values?: number /* double */[] | null;
            sum?: number; // double
            epaVersion?: string | null;
            currency?: Currency /* int32 */;
            override?: boolean;
        }
        export interface Co2DrillingFlaringFuelTotalsDto {
            co2Drilling?: number; // double
            co2Fuel?: number; // double
            co2Flaring?: number; // double
        }
        export interface Co2EmissionsDto {
            id?: string; // uuid
            startYear?: number; // int32
            values?: number /* double */[] | null;
            sum?: number; // double
        }
        export interface Co2EmissionsOverrideDto {
            id?: string; // uuid
            startYear?: number; // int32
            values?: number /* double */[] | null;
            sum?: number; // double
            override?: boolean;
        }
        export interface Co2IntensityDto {
            id?: string; // uuid
            startYear?: number; // int32
            values?: number /* double */[] | null;
            sum?: number; // double
        }
        export interface CommonLibraryProjectDto {
            id?: string; // uuid
            name?: string | null;
            description?: string | null;
            projectState?: string | null;
            country?: string | null;
            projectPhase?: ProjectPhase /* int32 */;
            projectCategory?: ProjectCategory /* int32 */;
        }
        export interface CompareCasesDto {
            totalOilProduction?: number; // double
            totalGasProduction?: number; // double
            totalExportedVolumes?: number; // double
            totalStudyCostsPlusOpex?: number; // double
            totalCessationCosts?: number; // double
            offshorePlusOnshoreFacilityCosts?: number; // double
            developmentWellCosts?: number; // double
            explorationWellCosts?: number; // double
            totalCo2Emissions?: number; // double
            co2Intensity?: number; // double
        }
        export type Concept = 0 | 1 | 2 | 3 | 4 | 5 | 6 | 7 | 8 | 9 | 10 | 11 | 12; // int32
        export interface CountryOfficeCostDto {
            id?: string; // uuid
            startYear?: number; // int32
            values?: number /* double */[] | null;
            sum?: number; // double
            epaVersion?: string | null;
            currency?: Currency /* int32 */;
        }
        export type Currency = 1 | 2; // int32
        export interface DevelopmentOperationalWellCostsDto {
            id?: string; // uuid
            projectId?: string; // uuid
            rigUpgrading?: number; // double
            rigMobDemob?: number; // double
            annualWellInterventionCostPerWell?: number; // double
            pluggingAndAbandonment?: number; // double
            hasChanges?: boolean;
        }
        export interface DrainageStrategyDto {
            id?: string; // uuid
            projectId?: string; // uuid
            name?: string | null;
            description?: string | null;
            nglYield?: number; // double
            producerCount?: number; // int32
            gasInjectorCount?: number; // int32
            waterInjectorCount?: number; // int32
            artificialLift?: ArtificialLift /* int32 */;
            gasSolution?: GasSolution /* int32 */;
            productionProfileOil?: ProductionProfileOilDto;
            productionProfileGas?: ProductionProfileGasDto;
            productionProfileWater?: ProductionProfileWaterDto;
            productionProfileWaterInjection?: ProductionProfileWaterInjectionDto;
            fuelFlaringAndLosses?: FuelFlaringAndLossesDto;
            fuelFlaringAndLossesOverride?: FuelFlaringAndLossesOverrideDto;
            netSalesGas?: NetSalesGasDto;
            netSalesGasOverride?: NetSalesGasOverrideDto;
            co2Emissions?: Co2EmissionsDto;
            co2EmissionsOverride?: Co2EmissionsOverrideDto;
            productionProfileNGL?: ProductionProfileNGLDto;
            importedElectricity?: ImportedElectricityDto;
            importedElectricityOverride?: ImportedElectricityOverrideDto;
            co2Intensity?: Co2IntensityDto;
            hasChanges?: boolean;
        }
        export interface DrillingScheduleDto {
            id?: string; // uuid
            startYear?: number; // int32
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
            id?: string; // uuid
            projectId?: string; // uuid
            name?: string | null;
            explorationWellCostProfile?: ExplorationWellCostProfileDto;
            appraisalWellCostProfile?: AppraisalWellCostProfileDto;
            sidetrackCostProfile?: SidetrackCostProfileDto;
            gAndGAdminCost?: GAndGAdminCostDto;
            seismicAcquisitionAndProcessing?: SeismicAcquisitionAndProcessingDto;
            countryOfficeCost?: CountryOfficeCostDto;
            rigMobDemob?: number; // double
            currency?: Currency /* int32 */;
            explorationWells?: ExplorationWellDto[] | null;
            hasChanges?: boolean;
        }
        export interface ExplorationOperationalWellCostsDto {
            id?: string; // uuid
            projectId?: string; // uuid
            rigUpgrading?: number; // double
            explorationRigMobDemob?: number; // double
            explorationProjectDrillingCosts?: number; // double
            appraisalRigMobDemob?: number; // double
            appraisalProjectDrillingCosts?: number; // double
            hasChanges?: boolean;
        }
        export interface ExplorationWellCostProfileDto {
            id?: string; // uuid
            startYear?: number; // int32
            values?: number /* double */[] | null;
            sum?: number; // double
            epaVersion?: string | null;
            currency?: Currency /* int32 */;
            override?: boolean;
        }
        export interface ExplorationWellDto {
            drillingSchedule?: DrillingScheduleDto;
            explorationId?: string; // uuid
            wellId?: string; // uuid
            hasChanges?: boolean;
        }
        export interface FuelFlaringAndLossesDto {
            id?: string; // uuid
            startYear?: number; // int32
            values?: number /* double */[] | null;
            sum?: number; // double
        }
        export interface FuelFlaringAndLossesOverrideDto {
            id?: string; // uuid
            startYear?: number; // int32
            values?: number /* double */[] | null;
            sum?: number; // double
            override?: boolean;
        }
        export interface GAndGAdminCostDto {
            id?: string; // uuid
            startYear?: number; // int32
            values?: number /* double */[] | null;
            sum?: number; // double
            epaVersion?: string | null;
            currency?: Currency /* int32 */;
        }
        export interface GasInjectorCostProfileDto {
            id?: string; // uuid
            startYear?: number; // int32
            values?: number /* double */[] | null;
            sum?: number; // double
            epaVersion?: string | null;
            currency?: Currency /* int32 */;
        }
        export interface GasInjectorCostProfileOverrideDto {
            id?: string; // uuid
            startYear?: number; // int32
            values?: number /* double */[] | null;
            sum?: number; // double
            epaVersion?: string | null;
            currency?: Currency /* int32 */;
            override?: boolean;
        }
        export interface GasProducerCostProfileDto {
            id?: string; // uuid
            startYear?: number; // int32
            values?: number /* double */[] | null;
            sum?: number; // double
            epaVersion?: string | null;
            currency?: Currency /* int32 */;
        }
        export interface GasProducerCostProfileOverrideDto {
            id?: string; // uuid
            startYear?: number; // int32
            values?: number /* double */[] | null;
            sum?: number; // double
            epaVersion?: string | null;
            currency?: Currency /* int32 */;
            override?: boolean;
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
        export interface ImportedElectricityDto {
            id?: string; // uuid
            startYear?: number; // int32
            values?: number /* double */[] | null;
            sum?: number; // double
        }
        export interface ImportedElectricityOverrideDto {
            id?: string; // uuid
            startYear?: number; // int32
            values?: number /* double */[] | null;
            sum?: number; // double
            override?: boolean;
        }
        export type Maturity = 0 | 1 | 2 | 3; // int32
        export interface NetSalesGasDto {
            id?: string; // uuid
            startYear?: number; // int32
            values?: number /* double */[] | null;
            sum?: number; // double
        }
        export interface NetSalesGasOverrideDto {
            id?: string; // uuid
            startYear?: number; // int32
            values?: number /* double */[] | null;
            sum?: number; // double
            override?: boolean;
        }
        export interface OffshoreFacilitiesCostProfileDto {
            id?: string; // uuid
            startYear?: number; // int32
            values?: number /* double */[] | null;
            sum?: number; // double
            epaVersion?: string | null;
            currency?: Currency /* int32 */;
        }
        export interface OffshoreFacilitiesOperationsCostProfileDto {
            id?: string; // uuid
            startYear?: number; // int32
            values?: number /* double */[] | null;
            sum?: number; // double
            epaVersion?: string | null;
            currency?: Currency /* int32 */;
        }
        export interface OffshoreFacilitiesOperationsCostProfileOverrideDto {
            id?: string; // uuid
            startYear?: number; // int32
            values?: number /* double */[] | null;
            sum?: number; // double
            epaVersion?: string | null;
            currency?: Currency /* int32 */;
            override?: boolean;
        }
        export interface OilProducerCostProfileDto {
            id?: string; // uuid
            startYear?: number; // int32
            values?: number /* double */[] | null;
            sum?: number; // double
            epaVersion?: string | null;
            currency?: Currency /* int32 */;
        }
        export interface OilProducerCostProfileOverrideDto {
            id?: string; // uuid
            startYear?: number; // int32
            values?: number /* double */[] | null;
            sum?: number; // double
            epaVersion?: string | null;
            currency?: Currency /* int32 */;
            override?: boolean;
        }
        export interface OpexCostProfileDto {
            id?: string; // uuid
            startYear?: number; // int32
            values?: number /* double */[] | null;
            sum?: number; // double
            epaVersion?: string | null;
            currency?: Currency /* int32 */;
        }
        export interface OpexCostProfileWrapperDto {
            opexCostProfileDto?: OpexCostProfileDto;
            wellInterventionCostProfileDto?: WellInterventionCostProfileDto;
            offshoreFacilitiesOperationsCostProfileDto?: OffshoreFacilitiesOperationsCostProfileDto;
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
            id?: string; // uuid
            startYear?: number; // int32
            values?: number /* double */[] | null;
            sum?: number; // double
        }
        export interface ProductionProfileNGLDto {
            id?: string; // uuid
            startYear?: number; // int32
            values?: number /* double */[] | null;
            sum?: number; // double
        }
        export interface ProductionProfileOilDto {
            id?: string; // uuid
            startYear?: number; // int32
            values?: number /* double */[] | null;
            sum?: number; // double
        }
        export interface ProductionProfileWaterDto {
            id?: string; // uuid
            startYear?: number; // int32
            values?: number /* double */[] | null;
            sum?: number; // double
        }
        export interface ProductionProfileWaterInjectionDto {
            id?: string; // uuid
            startYear?: number; // int32
            values?: number /* double */[] | null;
            sum?: number; // double
        }
        export type ProductionStrategyOverview = 0 | 1 | 2 | 3 | 4; // int32
        export type ProjectCategory = 0 | 1 | 2 | 3 | 4 | 5 | 6 | 7 | 8 | 9 | 10 | 11 | 12 | 13 | 14 | 15 | 16 | 17 | 18 | 19 | 20 | 21; // int32
        export interface ProjectDto {
            projectId?: string; // uuid
            name?: string | null;
            commonLibraryId?: string; // uuid
            fusionProjectId?: string; // uuid
            referenceCaseId?: string; // uuid
            commonLibraryName?: string | null;
            description?: string | null;
            country?: string | null;
            currency?: Currency /* int32 */;
            physUnit?: PhysUnit /* int32 */;
            createDate?: string; // date-time
            projectPhase?: ProjectPhase /* int32 */;
            projectCategory?: ProjectCategory /* int32 */;
            explorationOperationalWellCosts?: ExplorationOperationalWellCostsDto;
            developmentOperationalWellCosts?: DevelopmentOperationalWellCostsDto;
            cases?: CaseDto[] | null;
            wells?: WellDto[] | null;
            explorations?: ExplorationDto[] | null;
            surfs?: SurfDto[] | null;
            substructures?: SubstructureDto[] | null;
            topsides?: TopsideDto[] | null;
            transports?: TransportDto[] | null;
            drainageStrategies?: DrainageStrategyDto[] | null;
            wellProjects?: WellProjectDto[] | null;
            sharepointSiteUrl?: string | null;
            cO2RemovedFromGas?: number; // double
            cO2EmissionFromFuelGas?: number; // double
            flaredGasPerProducedVolume?: number; // double
            cO2EmissionsFromFlaredGas?: number; // double
            cO2Vented?: number; // double
            dailyEmissionFromDrillingRig?: number; // double
            averageDevelopmentDrillingDays?: number; // double
            hasChanges?: boolean;
        }
        export type ProjectPhase = 0 | 1 | 2 | 3 | 4 | 5 | 6 | 7 | 8 | 9; // int32
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
            id?: string; // uuid
            startYear?: number; // int32
            values?: number /* double */[] | null;
            sum?: number; // double
            epaVersion?: string | null;
            currency?: Currency /* int32 */;
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
            id?: string; // uuid
            startYear?: number; // int32
            values?: number /* double */[] | null;
            sum?: number; // double
            epaVersion?: string | null;
            currency?: Currency /* int32 */;
            override?: boolean;
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
            id?: string; // uuid
            startYear?: number; // int32
            values?: number /* double */[] | null;
            sum?: number; // double
            epaVersion?: string | null;
            currency?: Currency /* int32 */;
        }
        export interface StudyCostProfileWrapperDto {
            studyCostProfileDto?: StudyCostProfileDto;
            totalFeasibilityAndConceptStudiesDto?: TotalFeasibilityAndConceptStudiesDto;
            totalFEEDStudiesDto?: TotalFEEDStudiesDto;
        }
        export interface SubstructureCessationCostProfileDto {
            id?: string; // uuid
            startYear?: number; // int32
            values?: number /* double */[] | null;
            sum?: number; // double
            epaVersion?: string | null;
            currency?: Currency /* int32 */;
        }
        export interface SubstructureCostProfileDto {
            id?: string; // uuid
            startYear?: number; // int32
            values?: number /* double */[] | null;
            sum?: number; // double
            epaVersion?: string | null;
            currency?: Currency /* int32 */;
        }
        export interface SubstructureCostProfileOverrideDto {
            id?: string; // uuid
            startYear?: number; // int32
            values?: number /* double */[] | null;
            sum?: number; // double
            epaVersion?: string | null;
            currency?: Currency /* int32 */;
            override?: boolean;
        }
        export interface SubstructureDto {
            id?: string; // uuid
            name?: string | null;
            projectId?: string; // uuid
            costProfile?: SubstructureCostProfileDto;
            costProfileOverride?: SubstructureCostProfileOverrideDto;
            cessationCostProfile?: SubstructureCessationCostProfileDto;
            dryWeight?: number; // double
            maturity?: Maturity /* int32 */;
            currency?: Currency /* int32 */;
            approvedBy?: string | null;
            costYear?: number; // int32
            prospVersion?: string | null; // date-time
            source?: Source /* int32 */;
            lastChangedDate?: string | null; // date-time
            concept?: Concept /* int32 */;
            dG3Date?: string | null; // date-time
            dG4Date?: string | null; // date-time
            hasChanges?: boolean;
        }
        export interface SurfCessationCostProfileDto {
            id?: string; // uuid
            startYear?: number; // int32
            values?: number /* double */[] | null;
            sum?: number; // double
            epaVersion?: string | null;
            currency?: Currency /* int32 */;
        }
        export interface SurfCostProfileDto {
            id?: string; // uuid
            startYear?: number; // int32
            values?: number /* double */[] | null;
            sum?: number; // double
            epaVersion?: string | null;
            currency?: Currency /* int32 */;
        }
        export interface SurfCostProfileOverrideDto {
            id?: string; // uuid
            startYear?: number; // int32
            values?: number /* double */[] | null;
            sum?: number; // double
            epaVersion?: string | null;
            currency?: Currency /* int32 */;
            override?: boolean;
        }
        export interface SurfDto {
            id?: string; // uuid
            name?: string | null;
            projectId?: string; // uuid
            costProfile?: SurfCostProfileDto;
            costProfileOverride?: SurfCostProfileOverrideDto;
            cessationCostProfile?: SurfCessationCostProfileDto;
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
            lastChangedDate?: string | null; // date-time
            costYear?: number; // int32
            source?: Source /* int32 */;
            prospVersion?: string | null; // date-time
            approvedBy?: string | null;
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
            id?: string; // uuid
            startYear?: number; // int32
            values?: number /* double */[] | null;
            sum?: number; // double
            epaVersion?: string | null;
            currency?: Currency /* int32 */;
        }
        export interface TopsideCessationCostProfileDto {
            id?: string; // uuid
            startYear?: number; // int32
            values?: number /* double */[] | null;
            sum?: number; // double
            epaVersion?: string | null;
            currency?: Currency /* int32 */;
        }
        export interface TopsideCostProfileDto {
            id?: string; // uuid
            startYear?: number; // int32
            values?: number /* double */[] | null;
            sum?: number; // double
            epaVersion?: string | null;
            currency?: Currency /* int32 */;
        }
        export interface TopsideCostProfileOverrideDto {
            id?: string; // uuid
            startYear?: number; // int32
            values?: number /* double */[] | null;
            sum?: number; // double
            epaVersion?: string | null;
            currency?: Currency /* int32 */;
            override?: boolean;
        }
        export interface TopsideDto {
            id?: string; // uuid
            name?: string | null;
            projectId?: string; // uuid
            costProfile?: TopsideCostProfileDto;
            costProfileOverride?: TopsideCostProfileOverrideDto;
            cessationCostProfile?: TopsideCessationCostProfileDto;
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
            lastChangedDate?: string | null; // date-time
            source?: Source /* int32 */;
            approvedBy?: string | null;
            dG3Date?: string | null; // date-time
            dG4Date?: string | null; // date-time
            facilityOpex?: number; // double
            peakElectricityImported?: number; // double
            hasChanges?: boolean;
        }
        export interface TotalFEEDStudiesDto {
            id?: string; // uuid
            startYear?: number; // int32
            values?: number /* double */[] | null;
            sum?: number; // double
            epaVersion?: string | null;
            currency?: Currency /* int32 */;
        }
        export interface TotalFEEDStudiesOverrideDto {
            id?: string; // uuid
            startYear?: number; // int32
            values?: number /* double */[] | null;
            sum?: number; // double
            epaVersion?: string | null;
            currency?: Currency /* int32 */;
            override?: boolean;
        }
        export interface TotalFeasibilityAndConceptStudiesDto {
            id?: string; // uuid
            startYear?: number; // int32
            values?: number /* double */[] | null;
            sum?: number; // double
            epaVersion?: string | null;
            currency?: Currency /* int32 */;
        }
        export interface TotalFeasibilityAndConceptStudiesOverrideDto {
            id?: string; // uuid
            startYear?: number; // int32
            values?: number /* double */[] | null;
            sum?: number; // double
            epaVersion?: string | null;
            currency?: Currency /* int32 */;
            override?: boolean;
        }
        export interface TransportCessationCostProfileDto {
            id?: string; // uuid
            startYear?: number; // int32
            values?: number /* double */[] | null;
            sum?: number; // double
            epaVersion?: string | null;
            currency?: Currency /* int32 */;
        }
        export interface TransportCostProfileDto {
            id?: string; // uuid
            startYear?: number; // int32
            values?: number /* double */[] | null;
            sum?: number; // double
            epaVersion?: string | null;
            currency?: Currency /* int32 */;
        }
        export interface TransportCostProfileOverrideDto {
            id?: string; // uuid
            startYear?: number; // int32
            values?: number /* double */[] | null;
            sum?: number; // double
            epaVersion?: string | null;
            currency?: Currency /* int32 */;
            override?: boolean;
        }
        export interface TransportDto {
            id?: string; // uuid
            name?: string | null;
            projectId?: string; // uuid
            costProfile?: TransportCostProfileDto;
            costProfileOverride?: TransportCostProfileOverrideDto;
            cessationCostProfile?: TransportCessationCostProfileDto;
            maturity?: Maturity /* int32 */;
            gasExportPipelineLength?: number; // double
            oilExportPipelineLength?: number; // double
            currency?: Currency /* int32 */;
            lastChangedDate?: string | null; // date-time
            costYear?: number; // int32
            source?: Source /* int32 */;
            prospVersion?: string | null; // date-time
            dG3Date?: string | null; // date-time
            dG4Date?: string | null; // date-time
            hasChanges?: boolean;
        }
        export interface UrlDto {
            url?: string | null;
        }
        export interface WaterInjectorCostProfileDto {
            id?: string; // uuid
            startYear?: number; // int32
            values?: number /* double */[] | null;
            sum?: number; // double
            epaVersion?: string | null;
            currency?: Currency /* int32 */;
        }
        export interface WaterInjectorCostProfileOverrideDto {
            id?: string; // uuid
            startYear?: number; // int32
            values?: number /* double */[] | null;
            sum?: number; // double
            epaVersion?: string | null;
            currency?: Currency /* int32 */;
            override?: boolean;
        }
        export type WellCategory = 0 | 1 | 2 | 3 | 4 | 5 | 6 | 7; // int32
        export interface WellDto {
            id?: string; // uuid
            projectId?: string; // uuid
            name?: string | null;
            wellInterventionCost?: number; // double
            plugingAndAbandonmentCost?: number; // double
            wellCategory?: WellCategory /* int32 */;
            wellCost?: number; // double
            drillingDays?: number; // double
            hasChanges?: boolean;
            hasCostChanges?: boolean;
        }
        export interface WellInterventionCostProfileDto {
            id?: string; // uuid
            startYear?: number; // int32
            values?: number /* double */[] | null;
            sum?: number; // double
            epaVersion?: string | null;
            currency?: Currency /* int32 */;
        }
        export interface WellInterventionCostProfileOverrideDto {
            id?: string; // uuid
            startYear?: number; // int32
            values?: number /* double */[] | null;
            sum?: number; // double
            epaVersion?: string | null;
            currency?: Currency /* int32 */;
            override?: boolean;
        }
        export interface WellProjectDto {
            id?: string; // uuid
            projectId?: string; // uuid
            name?: string | null;
            oilProducerCostProfile?: OilProducerCostProfileDto;
            oilProducerCostProfileOverride?: OilProducerCostProfileOverrideDto;
            gasProducerCostProfile?: GasProducerCostProfileDto;
            gasProducerCostProfileOverride?: GasProducerCostProfileOverrideDto;
            waterInjectorCostProfile?: WaterInjectorCostProfileDto;
            waterInjectorCostProfileOverride?: WaterInjectorCostProfileOverrideDto;
            gasInjectorCostProfile?: GasInjectorCostProfileDto;
            gasInjectorCostProfileOverride?: GasInjectorCostProfileOverrideDto;
            artificialLift?: ArtificialLift /* int32 */;
            currency?: Currency /* int32 */;
            wellProjectWells?: WellProjectWellDto[] | null;
            hasChanges?: boolean;
        }
        export interface WellProjectWellDto {
            drillingSchedule?: DrillingScheduleDto;
            wellProjectId?: string; // uuid
            wellId?: string; // uuid
            hasChanges?: boolean;
        }
    }
}
declare namespace Paths {
    namespace CalculateCompareCasesTotals {
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
    namespace CopyDrainageStrategy {
        namespace Parameters {
            export type CaseId = string; // uuid
            export type DrainageStrategyId = string; // uuid
        }
        export interface PathParameters {
            drainageStrategyId: Parameters.DrainageStrategyId /* uuid */;
        }
        export interface QueryParameters {
            caseId?: Parameters.CaseId /* uuid */;
        }
        namespace Responses {
            export type $200 = Components.Schemas.DrainageStrategyDto;
        }
    }
    namespace CopyExploration {
        namespace Parameters {
            export type CaseId = string; // uuid
            export type ExplorationId = string; // uuid
        }
        export interface PathParameters {
            explorationId: Parameters.ExplorationId /* uuid */;
        }
        export interface QueryParameters {
            caseId?: Parameters.CaseId /* uuid */;
        }
        namespace Responses {
            export type $200 = Components.Schemas.ExplorationDto;
        }
    }
    namespace CopySubstructure {
        namespace Parameters {
            export type CaseId = string; // uuid
            export type SubstructureId = string; // uuid
        }
        export interface PathParameters {
            substructureId: Parameters.SubstructureId /* uuid */;
        }
        export interface QueryParameters {
            caseId?: Parameters.CaseId /* uuid */;
        }
        namespace Responses {
            export type $200 = Components.Schemas.SubstructureDto;
        }
    }
    namespace CopySurf {
        namespace Parameters {
            export type CaseId = string; // uuid
            export type SurfId = string; // uuid
        }
        export interface PathParameters {
            surfId: Parameters.SurfId /* uuid */;
        }
        export interface QueryParameters {
            caseId?: Parameters.CaseId /* uuid */;
        }
        namespace Responses {
            export type $200 = Components.Schemas.SurfDto;
        }
    }
    namespace CopyTopside {
        namespace Parameters {
            export type CaseId = string; // uuid
            export type TopsideId = string; // uuid
        }
        export interface PathParameters {
            topsideId: Parameters.TopsideId /* uuid */;
        }
        export interface QueryParameters {
            caseId?: Parameters.CaseId /* uuid */;
        }
        namespace Responses {
            export type $200 = Components.Schemas.TopsideDto;
        }
    }
    namespace CopyTransport {
        namespace Parameters {
            export type CaseId = string; // uuid
            export type TransportId = string; // uuid
        }
        export interface PathParameters {
            transportId: Parameters.TransportId /* uuid */;
        }
        export interface QueryParameters {
            caseId?: Parameters.CaseId /* uuid */;
        }
        namespace Responses {
            export type $200 = Components.Schemas.TransportDto;
        }
    }
    namespace CopyWellProject {
        namespace Parameters {
            export type CaseId = string; // uuid
            export type WellProjectId = string; // uuid
        }
        export interface PathParameters {
            wellProjectId: Parameters.WellProjectId /* uuid */;
        }
        export interface QueryParameters {
            caseId?: Parameters.CaseId /* uuid */;
        }
        namespace Responses {
            export type $200 = Components.Schemas.WellProjectDto;
        }
    }
    namespace CreateCase {
        export type RequestBody = Components.Schemas.CaseDto;
        namespace Responses {
            export type $200 = Components.Schemas.ProjectDto;
        }
    }
    namespace CreateDevelopmentOperationalWellCosts {
        export type RequestBody = Components.Schemas.DevelopmentOperationalWellCostsDto;
        namespace Responses {
            export type $200 = Components.Schemas.DevelopmentOperationalWellCostsDto;
        }
    }
    namespace CreateDrainageStrategy {
        namespace Parameters {
            export type SourceCaseId = string; // uuid
        }
        export interface QueryParameters {
            sourceCaseId?: Parameters.SourceCaseId /* uuid */;
        }
        export type RequestBody = Components.Schemas.DrainageStrategyDto;
        namespace Responses {
            export type $200 = Components.Schemas.ProjectDto;
        }
    }
    namespace CreateExploration {
        namespace Parameters {
            export type SourceCaseId = string; // uuid
        }
        export interface QueryParameters {
            sourceCaseId?: Parameters.SourceCaseId /* uuid */;
        }
        export type RequestBody = Components.Schemas.ExplorationDto;
        namespace Responses {
            export type $200 = Components.Schemas.ProjectDto;
        }
    }
    namespace CreateExplorationOperationalWellCosts {
        export type RequestBody = Components.Schemas.ExplorationOperationalWellCostsDto;
        namespace Responses {
            export type $200 = Components.Schemas.ExplorationOperationalWellCostsDto;
        }
    }
    namespace CreateExplorationWell {
        export type RequestBody = Components.Schemas.ExplorationWellDto;
        namespace Responses {
            export type $200 = Components.Schemas.ProjectDto;
        }
    }
    namespace CreateMultipleExplorationWells {
        export type RequestBody = Components.Schemas.ExplorationWellDto[];
        namespace Responses {
            export type $200 = Components.Schemas.ExplorationWellDto[];
        }
    }
    namespace CreateMultipleWellProjectWells {
        export type RequestBody = Components.Schemas.WellProjectWellDto[];
        namespace Responses {
            export type $200 = Components.Schemas.WellProjectWellDto[];
        }
    }
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
    namespace CreateSubstructure {
        namespace Parameters {
            export type SourceCaseId = string; // uuid
        }
        export interface QueryParameters {
            sourceCaseId?: Parameters.SourceCaseId /* uuid */;
        }
        export type RequestBody = Components.Schemas.SubstructureDto;
        namespace Responses {
            export type $200 = Components.Schemas.ProjectDto;
        }
    }
    namespace CreateSurf {
        namespace Parameters {
            export type SourceCaseId = string; // uuid
        }
        export interface QueryParameters {
            sourceCaseId?: Parameters.SourceCaseId /* uuid */;
        }
        export type RequestBody = Components.Schemas.SurfDto;
        namespace Responses {
            export type $200 = Components.Schemas.ProjectDto;
        }
    }
    namespace CreateTopside {
        namespace Parameters {
            export type SourceCaseId = string; // uuid
        }
        export interface QueryParameters {
            sourceCaseId?: Parameters.SourceCaseId /* uuid */;
        }
        export type RequestBody = Components.Schemas.TopsideDto;
        namespace Responses {
            export type $200 = Components.Schemas.ProjectDto;
        }
    }
    namespace CreateTransport {
        namespace Parameters {
            export type SourceCaseId = string; // uuid
        }
        export interface QueryParameters {
            sourceCaseId?: Parameters.SourceCaseId /* uuid */;
        }
        export type RequestBody = Components.Schemas.TransportDto;
        namespace Responses {
            export type $200 = Components.Schemas.ProjectDto;
        }
    }
    namespace CreateWell {
        export type RequestBody = Components.Schemas.WellDto;
        namespace Responses {
            export type $200 = Components.Schemas.ProjectDto;
        }
    }
    namespace CreateWellProject {
        namespace Parameters {
            export type SourceCaseId = string; // uuid
        }
        export interface QueryParameters {
            sourceCaseId?: Parameters.SourceCaseId /* uuid */;
        }
        export type RequestBody = Components.Schemas.WellProjectDto;
        namespace Responses {
            export type $200 = Components.Schemas.ProjectDto;
        }
    }
    namespace CreateWellProjectWell {
        export type RequestBody = Components.Schemas.WellProjectWellDto;
        namespace Responses {
            export type $200 = Components.Schemas.ProjectDto;
        }
    }
    namespace DeleteCase {
        namespace Parameters {
            export type CaseId = string; // uuid
        }
        export interface PathParameters {
            caseId: Parameters.CaseId /* uuid */;
        }
        namespace Responses {
            export type $200 = Components.Schemas.ProjectDto;
        }
    }
    namespace DeleteDrainageStrategy {
        namespace Parameters {
            export type DrainageStrategyId = string; // uuid
        }
        export interface PathParameters {
            drainageStrategyId: Parameters.DrainageStrategyId /* uuid */;
        }
        namespace Responses {
            export type $200 = Components.Schemas.ProjectDto;
        }
    }
    namespace DeleteExploration {
        namespace Parameters {
            export type ExplorationId = string; // uuid
        }
        export interface PathParameters {
            explorationId: Parameters.ExplorationId /* uuid */;
        }
        namespace Responses {
            export type $200 = Components.Schemas.ProjectDto;
        }
    }
    namespace DeleteSubstructure {
        namespace Parameters {
            export type SubstructureId = string; // uuid
        }
        export interface PathParameters {
            substructureId: Parameters.SubstructureId /* uuid */;
        }
        namespace Responses {
            export type $200 = Components.Schemas.ProjectDto;
        }
    }
    namespace DeleteSurf {
        namespace Parameters {
            export type SurfId = string; // uuid
        }
        export interface PathParameters {
            surfId: Parameters.SurfId /* uuid */;
        }
        namespace Responses {
            export type $200 = Components.Schemas.ProjectDto;
        }
    }
    namespace DeleteTopside {
        namespace Parameters {
            export type TopsideId = string; // uuid
        }
        export interface PathParameters {
            topsideId: Parameters.TopsideId /* uuid */;
        }
        namespace Responses {
            export type $200 = Components.Schemas.ProjectDto;
        }
    }
    namespace DeleteTransport {
        namespace Parameters {
            export type TransportId = string; // uuid
        }
        export interface PathParameters {
            transportId: Parameters.TransportId /* uuid */;
        }
        namespace Responses {
            export type $200 = Components.Schemas.ProjectDto;
        }
    }
    namespace DeleteWellProject {
        namespace Parameters {
            export type WellProjectId = string; // uuid
        }
        export interface PathParameters {
            wellProjectId: Parameters.WellProjectId /* uuid */;
        }
        namespace Responses {
            export type $200 = Components.Schemas.ProjectDto;
        }
    }
    namespace Duplicate {
        namespace Parameters {
            export type CopyCaseId = string; // uuid
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
    namespace GenerateCessation {
        namespace Parameters {
            export type CaseId = string; // uuid
        }
        export interface PathParameters {
            caseId: Parameters.CaseId /* uuid */;
        }
        namespace Responses {
            export type $200 = Components.Schemas.CessationCostWrapperDto;
        }
    }
    namespace GenerateCo2DrillingFlaringFuelTotals {
        namespace Parameters {
            export type CaseId = string; // uuid
        }
        export interface PathParameters {
            caseId: Parameters.CaseId /* uuid */;
        }
        namespace Responses {
            export type $200 = Components.Schemas.Co2DrillingFlaringFuelTotalsDto;
        }
    }
    namespace GenerateCo2Emissions {
        namespace Parameters {
            export type CaseId = string; // uuid
        }
        export interface PathParameters {
            caseId: Parameters.CaseId /* uuid */;
        }
        namespace Responses {
            export type $200 = Components.Schemas.Co2EmissionsDto;
        }
    }
    namespace GenerateCo2Intensity {
        namespace Parameters {
            export type CaseId = string; // uuid
        }
        export interface PathParameters {
            caseId: Parameters.CaseId /* uuid */;
        }
        namespace Responses {
            export type $200 = Components.Schemas.Co2IntensityDto;
        }
    }
    namespace GenerateCo2IntensityTotal {
        namespace Parameters {
            export type CaseId = string; // uuid
        }
        export interface PathParameters {
            caseId: Parameters.CaseId /* uuid */;
        }
        namespace Responses {
            export type $200 = number; // double
        }
    }
    namespace GenerateFuelFlaringLosses {
        namespace Parameters {
            export type CaseId = string; // uuid
        }
        export interface PathParameters {
            caseId: Parameters.CaseId /* uuid */;
        }
        namespace Responses {
            export type $200 = Components.Schemas.FuelFlaringAndLossesDto;
        }
    }
    namespace GenerateGAndGAdminCost {
        namespace Parameters {
            export type CaseId = string; // uuid
        }
        export interface PathParameters {
            caseId: Parameters.CaseId /* uuid */;
        }
        namespace Responses {
            export type $200 = Components.Schemas.GAndGAdminCostDto;
        }
    }
    namespace GenerateImportedElectricity {
        namespace Parameters {
            export type CaseId = string; // uuid
        }
        export interface PathParameters {
            caseId: Parameters.CaseId /* uuid */;
        }
        namespace Responses {
            export type $200 = Components.Schemas.ImportedElectricityDto;
        }
    }
    namespace GenerateNetSaleGas {
        namespace Parameters {
            export type CaseId = string; // uuid
        }
        export interface PathParameters {
            caseId: Parameters.CaseId /* uuid */;
        }
        namespace Responses {
            export type $200 = Components.Schemas.NetSalesGasDto;
        }
    }
    namespace GenerateOpex {
        namespace Parameters {
            export type CaseId = string; // uuid
        }
        export interface PathParameters {
            caseId: Parameters.CaseId /* uuid */;
        }
        namespace Responses {
            export type $200 = Components.Schemas.OpexCostProfileWrapperDto;
        }
    }
    namespace GenerateStudy {
        namespace Parameters {
            export type CaseId = string; // uuid
        }
        export interface PathParameters {
            caseId: Parameters.CaseId /* uuid */;
        }
        namespace Responses {
            export type $200 = Components.Schemas.StudyCostProfileWrapperDto;
        }
    }
    namespace GetExplorationWells {
        namespace Parameters {
            export type ProjectId = string; // uuid
        }
        export interface QueryParameters {
            projectId?: Parameters.ProjectId /* uuid */;
        }
        namespace Responses {
            export type $200 = Components.Schemas.ExplorationWellDto[];
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
    namespace GetProjects {
        namespace Responses {
            export type $200 = Components.Schemas.ProjectDto[];
        }
    }
    namespace GetProjectsFromCommonLibrary {
        namespace Responses {
            export type $200 = Components.Schemas.CommonLibraryProjectDto[];
        }
    }
    namespace GetSharePointFileNamesAndId {
        export type RequestBody = Components.Schemas.UrlDto;
        namespace Responses {
            export type $200 = Components.Schemas.DriveItemDto[];
        }
    }
    namespace GetWell {
        namespace Parameters {
            export type WellId = string; // uuid
        }
        export interface PathParameters {
            wellId: Parameters.WellId /* uuid */;
        }
        namespace Responses {
            export type $200 = Components.Schemas.WellDto;
        }
    }
    namespace GetWellProjectWells {
        namespace Parameters {
            export type ProjectId = string; // uuid
        }
        export interface QueryParameters {
            projectId?: Parameters.ProjectId /* uuid */;
        }
        namespace Responses {
            export type $200 = Components.Schemas.WellProjectWellDto[];
        }
    }
    namespace GetWells {
        namespace Parameters {
            export type ProjectId = string; // uuid
        }
        export interface QueryParameters {
            projectId?: Parameters.ProjectId /* uuid */;
        }
        namespace Responses {
            export type $200 = Components.Schemas.WellDto[];
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
    namespace NewCreateCase {
        export type RequestBody = Components.Schemas.CaseDto;
        namespace Responses {
            export type $200 = Components.Schemas.ProjectDto;
        }
    }
    namespace NewUpdateCase {
        export type RequestBody = Components.Schemas.CaseDto;
        namespace Responses {
            export type $200 = Components.Schemas.CaseDto;
        }
    }
    namespace NewUpdateDrainageStrategy {
        export type RequestBody = Components.Schemas.DrainageStrategyDto;
        namespace Responses {
            export type $200 = Components.Schemas.DrainageStrategyDto;
        }
    }
    namespace NewUpdateExploration {
        export type RequestBody = Components.Schemas.ExplorationDto;
        namespace Responses {
            export type $200 = Components.Schemas.ExplorationDto;
        }
    }
    namespace NewUpdateSubstructure {
        export type RequestBody = Components.Schemas.SubstructureDto;
        namespace Responses {
            export type $200 = Components.Schemas.SubstructureDto;
        }
    }
    namespace NewUpdateSurf {
        export type RequestBody = Components.Schemas.SurfDto;
        namespace Responses {
            export type $200 = Components.Schemas.SurfDto;
        }
    }
    namespace NewUpdateTopside {
        export type RequestBody = Components.Schemas.TopsideDto;
        namespace Responses {
            export type $200 = Components.Schemas.TopsideDto;
        }
    }
    namespace NewUpdateTransport {
        export type RequestBody = Components.Schemas.TransportDto;
        namespace Responses {
            export type $200 = Components.Schemas.TransportDto;
        }
    }
    namespace NewUpdateWellProject {
        export type RequestBody = Components.Schemas.WellProjectDto;
        namespace Responses {
            export type $200 = Components.Schemas.WellProjectDto;
        }
    }
    namespace SetReferenceCase {
        export type RequestBody = Components.Schemas.ProjectDto;
        namespace Responses {
            export type $200 = Components.Schemas.ProjectDto;
        }
    }
    namespace UpdateCase {
        export type RequestBody = Components.Schemas.CaseDto;
        namespace Responses {
            export type $200 = Components.Schemas.ProjectDto;
        }
    }
    namespace UpdateCaseWithAssets {
        export type RequestBody = Components.Schemas.CaseWithAssetsWrapperDto;
        namespace Responses {
            export type $200 = Components.Schemas.ProjectDto;
        }
    }
    namespace UpdateDevelopmentOperationalWellCosts {
        export type RequestBody = Components.Schemas.DevelopmentOperationalWellCostsDto;
        namespace Responses {
            export type $200 = Components.Schemas.DevelopmentOperationalWellCostsDto;
        }
    }
    namespace UpdateDrainageStrategy {
        export type RequestBody = Components.Schemas.DrainageStrategyDto;
        namespace Responses {
            export type $200 = Components.Schemas.ProjectDto;
        }
    }
    namespace UpdateExploration {
        export type RequestBody = Components.Schemas.ExplorationDto;
        namespace Responses {
            export type $200 = Components.Schemas.ProjectDto;
        }
    }
    namespace UpdateExplorationOperationalWellCosts {
        export type RequestBody = Components.Schemas.ExplorationOperationalWellCostsDto;
        namespace Responses {
            export type $200 = Components.Schemas.ExplorationOperationalWellCostsDto;
        }
    }
    namespace UpdateExplorationWell {
        export type RequestBody = Components.Schemas.ExplorationWellDto;
        namespace Responses {
            export type $200 = Components.Schemas.ProjectDto;
        }
    }
    namespace UpdateMultipleExplorationWells {
        namespace Parameters {
            export type CaseId = string; // uuid
        }
        export interface QueryParameters {
            caseId?: Parameters.CaseId /* uuid */;
        }
        export type RequestBody = Components.Schemas.ExplorationWellDto[];
        namespace Responses {
            export type $200 = Components.Schemas.ExplorationWellDto[];
        }
    }
    namespace UpdateMultipleWellProjectWells {
        namespace Parameters {
            export type CaseId = string; // uuid
        }
        export interface QueryParameters {
            caseId?: Parameters.CaseId /* uuid */;
        }
        export type RequestBody = Components.Schemas.WellProjectWellDto[];
        namespace Responses {
            export type $200 = Components.Schemas.WellProjectWellDto[];
        }
    }
    namespace UpdateMultipleWells {
        export type RequestBody = Components.Schemas.WellDto[];
        namespace Responses {
            export type $200 = Components.Schemas.WellDto[];
        }
    }
    namespace UpdateProject {
        export type RequestBody = Components.Schemas.ProjectDto;
        namespace Responses {
            export type $200 = Components.Schemas.ProjectDto;
        }
    }
    namespace UpdateSubstructure {
        export type RequestBody = Components.Schemas.SubstructureDto;
        namespace Responses {
            export type $200 = Components.Schemas.ProjectDto;
        }
    }
    namespace UpdateSurf {
        export type RequestBody = Components.Schemas.SurfDto;
        namespace Responses {
            export type $200 = Components.Schemas.ProjectDto;
        }
    }
    namespace UpdateTechnicalInput {
        export type RequestBody = Components.Schemas.TechnicalInputDto;
        namespace Responses {
            export type $200 = Components.Schemas.TechnicalInputDto;
        }
    }
    namespace UpdateTopside {
        export type RequestBody = Components.Schemas.TopsideDto;
        namespace Responses {
            export type $200 = Components.Schemas.ProjectDto;
        }
    }
    namespace UpdateTransport {
        export type RequestBody = Components.Schemas.TransportDto;
        namespace Responses {
            export type $200 = Components.Schemas.ProjectDto;
        }
    }
    namespace UpdateWell {
        export type RequestBody = Components.Schemas.WellDto;
        namespace Responses {
            export type $200 = Components.Schemas.ProjectDto;
        }
    }
    namespace UpdateWellProject {
        export type RequestBody = Components.Schemas.WellProjectDto;
        namespace Responses {
            export type $200 = Components.Schemas.ProjectDto;
        }
    }
    namespace UpdateWellProjectWell {
        export type RequestBody = Components.Schemas.WellProjectWellDto;
        namespace Responses {
            export type $200 = Components.Schemas.ProjectDto;
        }
    }
}

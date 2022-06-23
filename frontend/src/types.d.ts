declare namespace Components {
    namespace Schemas {
        export type ArtificialLift = 0 | 1 | 2 | 3; // int32
        export interface CapexDto {
            id?: string; // uuid
            startYear?: number; // int32
            values?: number /* double */[] | null;
            epaVersion?: string | null;
            currency?: Currency /* int32 */;
            sum?: number; // double
            drilling?: WellProjectCostProfileDto;
            offshoreFacilities?: OffshoreFacilitiesCostProfileDto;
            cessationOffshoreFacilities?: CessationOffshoreFacilities;
        }
        export interface CapexYear {
            values?: number /* double */[] | null;
            startYear?: number | null; // int32
        }
        export interface Case {
            id?: string; // uuid
            projectId?: string; // uuid
            name?: string | null;
            description?: string | null;
            createTime?: string; // date-time
            modifyTime?: string; // date-time
            referenceCase?: boolean;
            dG0Date?: string; // date-time
            dG1Date?: string; // date-time
            dG2Date?: string; // date-time
            dG3Date?: string; // date-time
            dG4Date?: string; // date-time
            project?: Project;
            artificialLift?: ArtificialLift /* int32 */;
            productionStrategyOverview?: ProductionStrategyOverview /* int32 */;
            producerCount?: number; // int32
            gasInjectorCount?: number; // int32
            waterInjectorCount?: number; // int32
            facilitiesAvailability?: number; // double
            drainageStrategyLink?: string; // uuid
            wellProjectLink?: string; // uuid
            surfLink?: string; // uuid
            substructureLink?: string; // uuid
            topsideLink?: string; // uuid
            transportLink?: string; // uuid
            explorationLink?: string; // uuid
            wells?: Well[] | null;
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
            dG0Date?: string; // date-time
            facilitiesAvailability?: number; // double
            dG1Date?: string; // date-time
            dG2Date?: string; // date-time
            dG3Date?: string; // date-time
            dG4Date?: string; // date-time
            createTime?: string; // date-time
            modifyTime?: string; // date-time
            drainageStrategyLink?: string; // uuid
            wellProjectLink?: string; // uuid
            surfLink?: string; // uuid
            substructureLink?: string; // uuid
            topsideLink?: string; // uuid
            transportLink?: string; // uuid
            explorationLink?: string; // uuid
            capex?: number; // double
            wells?: Well[] | null;
            capexYear?: CapexYear;
        }
        export interface CessationOffshoreFacilities {
            id?: string; // uuid
            startYear?: number; // int32
            values?: number /* double */[] | null;
            epaVersion?: string | null;
            currency?: Currency /* int32 */;
            sum?: number; // double
            surfCessationCostProfileDto?: SurfCessationCostProfileDto;
            topsideCessationCostProfileDto?: TopsideCessationCostProfileDto;
            substructureCessationCostProfileDto?: SubstructureCessationCostProfileDto;
            transportCessationCostProfileDto?: TransportCessationCostProfileDto;
        }
        export interface Co2Emissions {
            id?: string; // uuid
            startYear?: number; // int32
            internalData?: string | null;
            values?: number /* double */[] | null;
            drainageStrategy?: DrainageStrategy;
        }
        export interface Co2EmissionsDto {
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
        export type Concept = 0 | 1 | 2 | 3 | 4 | 5 | 6 | 7 | 8 | 9 | 10 | 11 | 12; // int32
        export type Currency = 1 | 2; // int32
        export interface DrainageStrategy {
            id?: string; // uuid
            project?: Project;
            projectId?: string; // uuid
            name?: string | null;
            description?: string | null;
            nglYield?: number; // double
            producerCount?: number; // int32
            gasInjectorCount?: number; // int32
            waterInjectorCount?: number; // int32
            artificialLift?: ArtificialLift /* int32 */;
            productionProfileOil?: ProductionProfileOil;
            productionProfileGas?: ProductionProfileGas;
            productionProfileWater?: ProductionProfileWater;
            productionProfileWaterInjection?: ProductionProfileWaterInjection;
            fuelFlaringAndLosses?: FuelFlaringAndLosses;
            netSalesGas?: NetSalesGas;
            co2Emissions?: Co2Emissions;
            productionProfileNGL?: ProductionProfileNGL;
            facilitiesAvailability?: number; // double
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
            productionProfileOil?: ProductionProfileOilDto;
            productionProfileGas?: ProductionProfileGasDto;
            productionProfileWater?: ProductionProfileWaterDto;
            productionProfileWaterInjection?: ProductionProfileWaterInjectionDto;
            fuelFlaringAndLosses?: FuelFlaringAndLossesDto;
            netSalesGas?: NetSalesGasDto;
            co2Emissions?: Co2EmissionsDto;
            productionProfileNGL?: ProductionProfileNGLDto;
            facilitiesAvailability?: number; // double
        }
        export interface DrillingSchedule {
            id?: string; // uuid
            startYear?: number; // int32
            internalData?: string | null;
            values?: number /* int32 */[] | null;
            wellProject?: WellProject;
        }
        export interface DrillingScheduleDto {
            id?: string; // uuid
            startYear?: number; // int32
            values?: number /* int32 */[] | null;
        }
        export interface Exploration {
            id?: string; // uuid
            project?: Project;
            projectId?: string; // uuid
            name?: string | null;
            costProfile?: ExplorationCostProfile;
            drillingSchedule?: ExplorationDrillingSchedule;
            gAndGAdminCost?: GAndGAdminCost;
            rigMobDemob?: number; // double
            currency?: Currency /* int32 */;
            explorationWellType?: ExplorationWellType;
        }
        export interface ExplorationCostProfile {
            id?: string; // uuid
            startYear?: number; // int32
            internalData?: string | null;
            values?: number /* double */[] | null;
            epaVersion?: string | null;
            currency?: Currency /* int32 */;
            exploration?: Exploration;
        }
        export interface ExplorationCostProfileDto {
            id?: string; // uuid
            startYear?: number; // int32
            values?: number /* double */[] | null;
            epaVersion?: string | null;
            currency?: Currency /* int32 */;
            sum?: number; // double
        }
        export interface ExplorationDrillingSchedule {
            id?: string; // uuid
            startYear?: number; // int32
            internalData?: string | null;
            values?: number /* int32 */[] | null;
            exploration?: Exploration;
        }
        export interface ExplorationDrillingScheduleDto {
            id?: string; // uuid
            startYear?: number; // int32
            values?: number /* int32 */[] | null;
        }
        export interface ExplorationDto {
            id?: string; // uuid
            projectId?: string; // uuid
            name?: string | null;
            costProfile?: ExplorationCostProfileDto;
            drillingSchedule?: ExplorationDrillingScheduleDto;
            gAndGAdminCost?: GAndGAdminCostDto;
            rigMobDemob?: number; // double
            currency?: Currency /* int32 */;
            explorationWellType?: ExplorationWellType;
        }
        export interface ExplorationWellType {
            id?: string; // uuid
            name?: string | null;
            category?: ExplorationWellTypeCategory /* int32 */;
            wellCost?: number; // double
            drillingDays?: number; // double
            description?: string | null;
        }
        export type ExplorationWellTypeCategory = 0 | 1 | 2 | 3 | 4 | 5 | 6; // int32
        export interface FuelFlaringAndLosses {
            id?: string; // uuid
            startYear?: number; // int32
            internalData?: string | null;
            values?: number /* double */[] | null;
            drainageStrategy?: DrainageStrategy;
        }
        export interface FuelFlaringAndLossesDto {
            id?: string; // uuid
            startYear?: number; // int32
            values?: number /* double */[] | null;
            sum?: number; // double
        }
        export interface GAndGAdminCost {
            id?: string; // uuid
            startYear?: number; // int32
            internalData?: string | null;
            values?: number /* double */[] | null;
            epaVersion?: string | null;
            currency?: Currency /* int32 */;
            exploration?: Exploration;
        }
        export interface GAndGAdminCostDto {
            id?: string; // uuid
            startYear?: number; // int32
            values?: number /* double */[] | null;
            epaVersion?: string | null;
            currency?: Currency /* int32 */;
            sum?: number; // double
        }
        export type Maturity = 0 | 1 | 2 | 3; // int32
        export interface NetSalesGas {
            id?: string; // uuid
            startYear?: number; // int32
            internalData?: string | null;
            values?: number /* double */[] | null;
            drainageStrategy?: DrainageStrategy;
        }
        export interface NetSalesGasDto {
            id?: string; // uuid
            startYear?: number; // int32
            values?: number /* double */[] | null;
            sum?: number; // double
        }
        export interface OffshoreFacilitiesCostProfileDto {
            id?: string; // uuid
            startYear?: number; // int32
            values?: number /* double */[] | null;
            epaVersion?: string | null;
            currency?: Currency /* int32 */;
            sum?: number; // double
        }
        export type PhysUnit = 0 | 1; // int32
        export interface ProductionAndSalesVolumesDto {
            startYear?: number; // int32
            totalAndAnnualOil?: ProductionProfileOilDto;
            totalAndAnnualSalesGas?: NetSalesGasDto;
            co2Emissions?: Co2EmissionsDto;
        }
        export type ProductionFlowline = 0 | 1 | 2 | 3 | 4 | 5 | 6 | 7 | 8 | 9 | 10 | 11 | 12 | 13; // int32
        export interface ProductionProfileGas {
            id?: string; // uuid
            startYear?: number; // int32
            internalData?: string | null;
            values?: number /* double */[] | null;
            drainageStrategy?: DrainageStrategy;
        }
        export interface ProductionProfileGasDto {
            id?: string; // uuid
            startYear?: number; // int32
            values?: number /* double */[] | null;
            sum?: number; // double
        }
        export interface ProductionProfileNGL {
            id?: string; // uuid
            startYear?: number; // int32
            internalData?: string | null;
            values?: number /* double */[] | null;
            drainageStrategy?: DrainageStrategy;
        }
        export interface ProductionProfileNGLDto {
            id?: string; // uuid
            startYear?: number; // int32
            values?: number /* double */[] | null;
            sum?: number; // double
        }
        export interface ProductionProfileOil {
            id?: string; // uuid
            startYear?: number; // int32
            internalData?: string | null;
            values?: number /* double */[] | null;
            drainageStrategy?: DrainageStrategy;
        }
        export interface ProductionProfileOilDto {
            id?: string; // uuid
            startYear?: number; // int32
            values?: number /* double */[] | null;
            sum?: number; // double
        }
        export interface ProductionProfileWater {
            id?: string; // uuid
            startYear?: number; // int32
            internalData?: string | null;
            values?: number /* double */[] | null;
            drainageStrategy?: DrainageStrategy;
        }
        export interface ProductionProfileWaterDto {
            id?: string; // uuid
            startYear?: number; // int32
            values?: number /* double */[] | null;
            sum?: number; // double
        }
        export interface ProductionProfileWaterInjection {
            id?: string; // uuid
            startYear?: number; // int32
            internalData?: string | null;
            values?: number /* double */[] | null;
            drainageStrategy?: DrainageStrategy;
        }
        export interface ProductionProfileWaterInjectionDto {
            id?: string; // uuid
            startYear?: number; // int32
            values?: number /* double */[] | null;
            sum?: number; // double
        }
        export type ProductionStrategyOverview = 0 | 1 | 2 | 3 | 4; // int32
        export interface Project {
            id?: string; // uuid
            name?: string | null;
            commonLibraryId?: string; // uuid
            fusionProjectId?: string; // uuid
            commonLibraryName?: string | null;
            description?: string | null;
            country?: string | null;
            currency?: Currency /* int32 */;
            physicalUnit?: PhysUnit /* int32 */;
            createDate?: string; // date-time
            cases?: Case[] | null;
            wells?: Well[] | null;
            surfs?: Surf[] | null;
            substructures?: Substructure[] | null;
            topsides?: Topside[] | null;
            transports?: Transport[] | null;
            projectPhase?: ProjectPhase /* int32 */;
            projectCategory?: ProjectCategory /* int32 */;
            drainageStrategies?: DrainageStrategy[] | null;
            wellProjects?: WellProject[] | null;
            explorations?: Exploration[] | null;
        }
        export type ProjectCategory = 0 | 1 | 2 | 3 | 4 | 5 | 6 | 7 | 8 | 9 | 10 | 11 | 12 | 13 | 14 | 15 | 16 | 17 | 18 | 19 | 20 | 21; // int32
        export interface ProjectDto {
            projectId?: string; // uuid
            name?: string | null;
            commonLibraryId?: string; // uuid
            fusionProjectId?: string; // uuid
            commonLibraryName?: string | null;
            description?: string | null;
            country?: string | null;
            currency?: Currency /* int32 */;
            physUnit?: PhysUnit /* int32 */;
            createDate?: string; // date-time
            projectPhase?: ProjectPhase /* int32 */;
            projectCategory?: ProjectCategory /* int32 */;
            cases?: CaseDto[] | null;
            wells?: WellDto[] | null;
            explorations?: ExplorationDto[] | null;
            surfs?: SurfDto[] | null;
            substructures?: SubstructureDto[] | null;
            topsides?: TopsideDto[] | null;
            transports?: TransportDto[] | null;
            drainageStrategies?: DrainageStrategyDto[] | null;
            wellProjects?: WellProjectDto[] | null;
        }
        export type ProjectPhase = 0 | 1 | 2 | 3 | 4 | 5 | 6 | 7 | 8 | 9; // int32
        export interface STEACaseDto {
            name?: string | null;
            startYear?: number; // int32
            exploration?: ExplorationCostProfileDto;
            capex?: CapexDto;
            productionAndSalesVolumes?: ProductionAndSalesVolumesDto;
            offshoreFacilitiesCostProfileDto?: OffshoreFacilitiesCostProfileDto;
        }
        export interface STEAProjectDto {
            name?: string | null;
            startYear?: number; // int32
            steaCases?: STEACaseDto[] | null;
        }
        export type Source = 0 | 1; // int32
        export interface Substructure {
            id?: string; // uuid
            name?: string | null;
            project?: Project;
            projectId?: string; // uuid
            costProfile?: SubstructureCostProfile;
            cessationCostProfile?: SubstructureCessationCostProfile;
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
        }
        export interface SubstructureCessationCostProfile {
            id?: string; // uuid
            startYear?: number; // int32
            internalData?: string | null;
            values?: number /* double */[] | null;
            epaVersion?: string | null;
            currency?: Currency /* int32 */;
            substructure?: Substructure;
        }
        export interface SubstructureCessationCostProfileDto {
            id?: string; // uuid
            startYear?: number; // int32
            values?: number /* double */[] | null;
            epaVersion?: string | null;
            currency?: Currency /* int32 */;
            sum?: number; // double
        }
        export interface SubstructureCostProfile {
            id?: string; // uuid
            startYear?: number; // int32
            internalData?: string | null;
            values?: number /* double */[] | null;
            epaVersion?: string | null;
            currency?: Currency /* int32 */;
            substructure?: Substructure;
        }
        export interface SubstructureCostProfileDto {
            id?: string; // uuid
            startYear?: number; // int32
            values?: number /* double */[] | null;
            epaVersion?: string | null;
            currency?: Currency /* int32 */;
            sum?: number; // double
        }
        export interface SubstructureDto {
            id?: string; // uuid
            name?: string | null;
            projectId?: string; // uuid
            costProfile?: SubstructureCostProfileDto;
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
        }
        export interface Surf {
            id?: string; // uuid
            name?: string | null;
            project?: Project;
            projectId?: string; // uuid
            costProfile?: SurfCostProfile;
            cessationCostProfile?: SurfCessationCostProfile;
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
        }
        export interface SurfCessationCostProfile {
            id?: string; // uuid
            startYear?: number; // int32
            internalData?: string | null;
            values?: number /* double */[] | null;
            epaVersion?: string | null;
            currency?: Currency /* int32 */;
            surf?: Surf;
        }
        export interface SurfCessationCostProfileDto {
            id?: string; // uuid
            startYear?: number; // int32
            values?: number /* double */[] | null;
            epaVersion?: string | null;
            currency?: Currency /* int32 */;
            sum?: number; // double
        }
        export interface SurfCostProfile {
            id?: string; // uuid
            startYear?: number; // int32
            internalData?: string | null;
            values?: number /* double */[] | null;
            epaVersion?: string | null;
            currency?: Currency /* int32 */;
            surf?: Surf;
        }
        export interface SurfCostProfileDto {
            id?: string; // uuid
            startYear?: number; // int32
            values?: number /* double */[] | null;
            epaVersion?: string | null;
            currency?: Currency /* int32 */;
            sum?: number; // double
        }
        export interface SurfDto {
            id?: string; // uuid
            name?: string | null;
            projectId?: string; // uuid
            costProfile?: SurfCostProfileDto;
            cessationCostProfile?: SurfCessationCostProfileDto;
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
        }
        export interface Topside {
            id?: string; // uuid
            name?: string | null;
            project?: Project;
            projectId?: string; // uuid
            costProfile?: TopsideCostProfile;
            cessationCostProfile?: TopsideCessationCostProfile;
            dryWeight?: number; // double
            oilCapacity?: number; // double
            gasCapacity?: number; // double
            facilitiesAvailability?: number; // double
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
        }
        export interface TopsideCessationCostProfile {
            id?: string; // uuid
            startYear?: number; // int32
            internalData?: string | null;
            values?: number /* double */[] | null;
            epaVersion?: string | null;
            currency?: Currency /* int32 */;
            topside?: Topside;
        }
        export interface TopsideCessationCostProfileDto {
            id?: string; // uuid
            startYear?: number; // int32
            values?: number /* double */[] | null;
            epaVersion?: string | null;
            currency?: Currency /* int32 */;
            sum?: number; // double
        }
        export interface TopsideCostProfile {
            id?: string; // uuid
            startYear?: number; // int32
            internalData?: string | null;
            values?: number /* double */[] | null;
            epaVersion?: string | null;
            currency?: Currency /* int32 */;
            topside?: Topside;
        }
        export interface TopsideCostProfileDto {
            id?: string; // uuid
            startYear?: number; // int32
            values?: number /* double */[] | null;
            epaVersion?: string | null;
            currency?: Currency /* int32 */;
            sum?: number; // double
        }
        export interface TopsideDto {
            id?: string; // uuid
            name?: string | null;
            projectId?: string; // uuid
            costProfile?: TopsideCostProfileDto;
            cessationCostProfile?: TopsideCessationCostProfileDto;
            dryWeight?: number; // double
            oilCapacity?: number; // double
            gasCapacity?: number; // double
            facilitiesAvailability?: number; // double
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
        }
        export interface Transport {
            id?: string; // uuid
            name?: string | null;
            project?: Project;
            projectId?: string; // uuid
            costProfile?: TransportCostProfile;
            cessationCostProfile?: TransportCessationCostProfile;
            gasExportPipelineLength?: number; // double
            oilExportPipelineLength?: number; // double
            maturity?: Maturity /* int32 */;
            currency?: Currency /* int32 */;
            lastChangedDate?: string | null; // date-time
            costYear?: number; // int32
            source?: Source /* int32 */;
            prospVersion?: string | null; // date-time
            dG3Date?: string | null; // date-time
            dG4Date?: string | null; // date-time
        }
        export interface TransportCessationCostProfile {
            id?: string; // uuid
            startYear?: number; // int32
            internalData?: string | null;
            values?: number /* double */[] | null;
            epaVersion?: string | null;
            currency?: Currency /* int32 */;
            transport?: Transport;
        }
        export interface TransportCessationCostProfileDto {
            id?: string; // uuid
            startYear?: number; // int32
            values?: number /* double */[] | null;
            epaVersion?: string | null;
            currency?: Currency /* int32 */;
            sum?: number; // double
        }
        export interface TransportCostProfile {
            id?: string; // uuid
            startYear?: number; // int32
            internalData?: string | null;
            values?: number /* double */[] | null;
            epaVersion?: string | null;
            currency?: Currency /* int32 */;
            transport?: Transport;
        }
        export interface TransportCostProfileDto {
            id?: string; // uuid
            startYear?: number; // int32
            values?: number /* double */[] | null;
            epaVersion?: string | null;
            currency?: Currency /* int32 */;
            sum?: number; // double
        }
        export interface TransportDto {
            id?: string; // uuid
            name?: string | null;
            projectId?: string; // uuid
            costProfile?: TransportCostProfileDto;
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
        }
        export interface Well {
            id?: string; // uuid
            projectId?: string; // uuid
            name?: string | null;
            project?: Project;
            wellType?: WellType;
            explorationWellType?: ExplorationWellType;
            wellInterventionCost?: number; // double
            plugingAndAbandonmentCost?: number; // double
        }
        export interface WellDto {
            id?: string; // uuid
            projectId?: string; // uuid
            name?: string | null;
            project?: Project;
            wellType?: WellType;
            explorationWellType?: ExplorationWellType;
            wellInterventionCost?: number; // double
            plugingAndAbandonmentCost?: number; // double
        }
        export interface WellProject {
            id?: string; // uuid
            project?: Project;
            projectId?: string; // uuid
            name?: string | null;
            costProfile?: WellProjectCostProfile;
            drillingSchedule?: DrillingSchedule;
            artificialLift?: ArtificialLift /* int32 */;
            rigMobDemob?: number; // double
            annualWellInterventionCost?: number; // double
            pluggingAndAbandonment?: number; // double
            currency?: Currency /* int32 */;
            wellType?: WellType;
        }
        export interface WellProjectCostProfile {
            id?: string; // uuid
            startYear?: number; // int32
            internalData?: string | null;
            values?: number /* double */[] | null;
            epaVersion?: string | null;
            currency?: Currency /* int32 */;
            wellProject?: WellProject;
        }
        export interface WellProjectCostProfileDto {
            id?: string; // uuid
            startYear?: number; // int32
            values?: number /* double */[] | null;
            epaVersion?: string | null;
            currency?: Currency /* int32 */;
            sum?: number; // double
        }
        export interface WellProjectDto {
            id?: string; // uuid
            projectId?: string; // uuid
            name?: string | null;
            costProfile?: WellProjectCostProfileDto;
            drillingSchedule?: DrillingScheduleDto;
            artificialLift?: ArtificialLift /* int32 */;
            rigMobDemob?: number; // double
            annualWellInterventionCost?: number; // double
            pluggingAndAbandonment?: number; // double
            currency?: Currency /* int32 */;
            wellType?: WellType;
        }
        export interface WellType {
            id?: string; // uuid
            name?: string | null;
            category?: WellTypeCategory /* int32 */;
            wellCost?: number; // double
            drillingDays?: number; // double
            description?: string | null;
        }
        export type WellTypeCategory = 0 | 1 | 2 | 3 | 4 | 5 | 6; // int32
    }
}
declare namespace Paths {
    namespace CreateCase {
        export type RequestBody = Components.Schemas.CaseDto;
        namespace Responses {
            export type $200 = Components.Schemas.ProjectDto;
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
    namespace CreateProject {
        export type RequestBody = Components.Schemas.ProjectDto;
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
    namespace UpdateCase {
        export type RequestBody = Components.Schemas.CaseDto;
        namespace Responses {
            export type $200 = Components.Schemas.ProjectDto;
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
    namespace Upload {
        namespace Parameters {
            export type ProjectId = string; // uuid
            export type SourceCaseId = string; // uuid
        }
        export interface QueryParameters {
            projectId?: Parameters.ProjectId /* uuid */;
            sourceCaseId?: Parameters.SourceCaseId /* uuid */;
        }
        namespace Responses {
            export type $200 = Components.Schemas.ProjectDto;
        }
    }
}

declare namespace Components {
    namespace Schemas {
        export type ArtificialLift = 0 | 1 | 2 | 3; // int32
        export interface Case {
            id?: string; // uuid
            projectId?: string; // uuid
            name?: string | null;
            description?: string | null;
            createTime?: string; // date-time
            modifyTime?: string; // date-time
            referenceCase?: boolean;
            dG4Date?: string; // date-time
            project?: Project;
            drainageStrategyLink?: string; // uuid
            wellProjectLink?: string; // uuid
            surfLink?: string; // uuid
            substructureLink?: string; // uuid
            topsideLink?: string; // uuid
            transportLink?: string; // uuid
            explorationLink?: string; // uuid
        }
        export interface CaseDto {
            projectId?: string; // uuid
            name?: string | null;
            description?: string | null;
            referenceCase?: boolean;
            dG4Date?: string; // date-time
            createTime?: string; // date-time
            modifyTime?: string; // date-time
        }
        export interface Co2Emissions {
            id?: string; // uuid
            yearValues?: DoubleYearValue[] | null;
            drainageStrategy?: DrainageStrategy;
        }
        export interface Co2EmissionsDto {
            id?: string; // uuid
            yearValues?: DoubleYearValue[] | null;
        }
        export type Currency = 0 | 1; // int32
        export interface DoubleYearValue {
            id?: number; // int32
            year?: number; // int32
            value?: number; // double
        }
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
        }
        export interface DrainageStrategyDto {
            projectId?: string; // uuid
            sourceCaseId?: string; // uuid
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
        }
        export interface DrillingSchedule {
            id?: string; // uuid
            yearValues?: Int32YearValue[] | null;
            wellProject?: WellProject;
        }
        export interface DrillingScheduleDto {
            id?: string; // uuid
            yearValues?: Int32YearValue[] | null;
        }
        export interface Exploration {
            id?: string; // uuid
            project?: Project;
            projectId?: string; // uuid
            name?: string | null;
            wellType?: WellType /* int32 */;
            costProfile?: ExplorationCostProfile;
            drillingSchedule?: ExplorationDrillingSchedule;
            gAndGAdminCost?: GAndGAdminCost;
            rigMobDemob?: number; // double
        }
        export interface ExplorationCostProfile {
            id?: string; // uuid
            yearValues?: DoubleYearValue[] | null;
            epaVersion?: string | null;
            currency?: Currency /* int32 */;
            exploration?: Exploration;
        }
        export interface ExplorationCostProfileDto {
            id?: string; // uuid
            yearValues?: DoubleYearValue[] | null;
            epaVersion?: string | null;
            currency?: Currency /* int32 */;
        }
        export interface ExplorationDrillingSchedule {
            id?: string; // uuid
            yearValues?: Int32YearValue[] | null;
            exploration?: Exploration;
        }
        export interface ExplorationDrillingScheduleDto {
            id?: string; // uuid
            yearValues?: Int32YearValue[] | null;
        }
        export interface ExplorationDto {
            projectId?: string; // uuid
            sourceCaseId?: string; // uuid
            name?: string | null;
            wellType?: WellType /* int32 */;
            costProfile?: ExplorationCostProfileDto;
            drillingSchedule?: ExplorationDrillingScheduleDto;
            gAndGAdminCost?: GAndGAdminCostDto;
            rigMobDemob?: number; // double
        }
        export interface FuelFlaringAndLosses {
            id?: string; // uuid
            yearValues?: DoubleYearValue[] | null;
            drainageStrategy?: DrainageStrategy;
        }
        export interface FuelFlaringAndLossesDto {
            id?: string; // uuid
            yearValues?: DoubleYearValue[] | null;
        }
        export interface GAndGAdminCost {
            id?: string; // uuid
            yearValues?: DoubleYearValue[] | null;
            epaVersion?: string | null;
            currency?: Currency /* int32 */;
            exploration?: Exploration;
        }
        export interface GAndGAdminCostDto {
            id?: string; // uuid
            yearValues?: DoubleYearValue[] | null;
            epaVersion?: string | null;
            currency?: Currency /* int32 */;
        }
        export interface Int32YearValue {
            id?: number; // int32
            year?: number; // int32
            value?: number; // int32
        }
        export type Maturity = 0 | 1 | 2 | 3; // int32
        export interface NetSalesGas {
            id?: string; // uuid
            yearValues?: DoubleYearValue[] | null;
            drainageStrategy?: DrainageStrategy;
        }
        export interface NetSalesGasDto {
            id?: string; // uuid
            yearValues?: DoubleYearValue[] | null;
        }
        export type ProductionFlowline = 999; // int32
        export interface ProductionProfileGas {
            id?: string; // uuid
            yearValues?: DoubleYearValue[] | null;
            drainageStrategy?: DrainageStrategy;
        }
        export interface ProductionProfileGasDto {
            id?: string; // uuid
            yearValues?: DoubleYearValue[] | null;
        }
        export interface ProductionProfileOil {
            id?: string; // uuid
            yearValues?: DoubleYearValue[] | null;
            drainageStrategy?: DrainageStrategy;
        }
        export interface ProductionProfileOilDto {
            id?: string; // uuid
            yearValues?: DoubleYearValue[] | null;
        }
        export interface ProductionProfileWater {
            id?: string; // uuid
            yearValues?: DoubleYearValue[] | null;
            drainageStrategy?: DrainageStrategy;
        }
        export interface ProductionProfileWaterDto {
            id?: string; // uuid
            yearValues?: DoubleYearValue[] | null;
        }
        export interface ProductionProfileWaterInjection {
            id?: string; // uuid
            yearValues?: DoubleYearValue[] | null;
            drainageStrategy?: DrainageStrategy;
        }
        export interface ProductionProfileWaterInjectionDto {
            id?: string; // uuid
            yearValues?: DoubleYearValue[] | null;
        }
        export interface Project {
            id?: string; // uuid
            name?: string | null;
            description?: string | null;
            country?: string | null;
            createDate?: string; // date-time
            cases?: Case[] | null;
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
        export type ProjectCategory = 0 | 1 | 2 | 3 | 4 | 5 | 6 | 7 | 8 | 9 | 10 | 11 | 12 | 13; // int32
        export interface ProjectDto {
            projectId?: string; // uuid
            name?: string | null;
            description?: string | null;
            country?: string | null;
            projectPhase?: ProjectPhase /* int32 */;
            projectCategory?: ProjectCategory /* int32 */;
        }
        export type ProjectPhase = 0 | 1 | 2 | 3; // int32
        export interface Substructure {
            id?: string; // uuid
            name?: string | null;
            project?: Project;
            projectId?: string; // uuid
            costProfile?: SubstructureCostProfile;
            dryWeight?: number; // double
            maturity?: Maturity /* int32 */;
        }
        export interface SubstructureCostProfile {
            id?: string; // uuid
            yearValues?: DoubleYearValue[] | null;
            epaVersion?: string | null;
            currency?: Currency /* int32 */;
            substructure?: Substructure;
        }
        export interface SubstructureCostProfileDto {
            id?: string; // uuid
            yearValues?: DoubleYearValue[] | null;
            epaVersion?: string | null;
            currency?: Currency /* int32 */;
        }
        export interface SubstructureDto {
            name?: string | null;
            projectId?: string; // uuid
            sourceCaseId?: string; // uuid
            costProfile?: SubstructureCostProfileDto;
            dryWeight?: number; // double
            maturity?: Maturity /* int32 */;
        }
        export interface Surf {
            id?: string; // uuid
            name?: string | null;
            project?: Project;
            projectId?: string; // uuid
            costProfile?: SurfCostProfile;
            maturity?: Maturity /* int32 */;
            infieldPipelineSystemLength?: number; // double
            umbilicalSystemLength?: number; // double
            artificialLift?: ArtificialLift /* int32 */;
            riserCount?: number; // int32
            templateCount?: number; // int32
            productionFlowline?: ProductionFlowline /* int32 */;
        }
        export interface SurfCostProfile {
            id?: string; // uuid
            yearValues?: DoubleYearValue[] | null;
            epaVersion?: string | null;
            currency?: Currency /* int32 */;
            surf?: Surf;
        }
        export interface SurfCostProfileDto {
            id?: string; // uuid
            yearValues?: DoubleYearValue[] | null;
            epaVersion?: string | null;
            currency?: Currency /* int32 */;
        }
        export interface SurfDto {
            id?: string; // uuid
            name?: string | null;
            projectId?: string; // uuid
            sourceCaseId?: string; // uuid
            costProfile?: SurfCostProfileDto;
            maturity?: Maturity /* int32 */;
            infieldPipelineSystemLength?: number; // double
            umbilicalSystemLength?: number; // double
            artificialLift?: ArtificialLift /* int32 */;
            riserCount?: number; // int32
            templateCount?: number; // int32
            productionFlowline?: ProductionFlowline /* int32 */;
        }
        export interface Topside {
            id?: string; // uuid
            name?: string | null;
            project?: Project;
            projectId?: string; // uuid
            costProfile?: TopsideCostProfile;
            dryWeight?: number; // double
            oilCapacity?: number; // double
            gasCapacity?: number; // double
            facilitiesAvailability?: number; // double
            artificialLift?: ArtificialLift /* int32 */;
            maturity?: Maturity /* int32 */;
        }
        export interface TopsideCostProfile {
            id?: string; // uuid
            yearValues?: DoubleYearValue[] | null;
            epaVersion?: string | null;
            currency?: Currency /* int32 */;
            topside?: Topside;
        }
        export interface TopsideCostProfileDto {
            id?: string; // uuid
            yearValues?: DoubleYearValue[] | null;
            epaVersion?: string | null;
            currency?: Currency /* int32 */;
        }
        export interface TopsideDto {
            name?: string | null;
            projectId?: string; // uuid
            sourceCaseId?: string; // uuid
            costProfile?: TopsideCostProfileDto;
            dryWeight?: number; // double
            oilCapacity?: number; // double
            gasCapacity?: number; // double
            facilitiesAvailability?: number; // double
            artificialLift?: ArtificialLift /* int32 */;
            maturity?: Maturity /* int32 */;
        }
        export interface Transport {
            id?: string; // uuid
            name?: string | null;
            project?: Project;
            projectId?: string; // uuid
            costProfile?: TransportCostProfile;
            gasExportPipelineLength?: number; // double
            oilExportPipelineLength?: number; // double
            maturity?: Maturity /* int32 */;
        }
        export interface TransportCostProfile {
            id?: string; // uuid
            yearValues?: DoubleYearValue[] | null;
            epaVersion?: string | null;
            currency?: Currency /* int32 */;
            transport?: Transport;
        }
        export interface TransportCostProfileDto {
            id?: string; // uuid
            yearValues?: DoubleYearValue[] | null;
            epaVersion?: string | null;
            currency?: Currency /* int32 */;
        }
        export interface TransportDto {
            id?: string; // uuid
            name?: string | null;
            projectId?: string; // uuid
            sourceCaseId?: string; // uuid
            costProfile?: TransportCostProfileDto;
            maturity?: Maturity /* int32 */;
            gasExportPipelineLength?: number; // double
            oilExportPipelineLength?: number; // double
        }
        export interface WellProject {
            id?: string; // uuid
            project?: Project;
            projectId?: string; // uuid
            name?: string | null;
            costProfile?: WellProjectCostProfile;
            drillingSchedule?: DrillingSchedule;
            producerCount?: number; // int32
            gasInjectorCount?: number; // int32
            waterInjectorCount?: number; // int32
            artificialLift?: ArtificialLift /* int32 */;
            rigMobDemob?: number; // double
            annualWellInterventionCost?: number; // double
            pluggingAndAbandonment?: number; // double
        }
        export interface WellProjectCostProfile {
            id?: string; // uuid
            yearValues?: DoubleYearValue[] | null;
            epaVersion?: string | null;
            currency?: Currency /* int32 */;
            wellProject?: WellProject;
        }
        export interface WellProjectCostProfileDto {
            id?: string; // uuid
            yearValues?: DoubleYearValue[] | null;
            epaVersion?: string | null;
            currency?: Currency /* int32 */;
        }
        export interface WellProjectDto {
            projectId?: string; // uuid
            sourceCaseId?: string; // uuid
            name?: string | null;
            costProfile?: WellProjectCostProfileDto;
            drillingSchedule?: DrillingScheduleDto;
            producerCount?: number; // int32
            gasInjectorCount?: number; // int32
            waterInjectorCount?: number; // int32
            artificialLift?: ArtificialLift /* int32 */;
            rigMobDemob?: number; // double
            annualWellInterventionCost?: number; // double
            pluggingAndAbandonment?: number; // double
        }
        export type WellType = 0 | 1; // int32
    }
}
declare namespace Paths {
    namespace CreateCase {
        export type RequestBody = Components.Schemas.CaseDto;
        namespace Responses {
            export type $200 = Components.Schemas.Project;
        }
    }
    namespace CreateDrainageStrategy {
        export type RequestBody = Components.Schemas.DrainageStrategyDto;
        namespace Responses {
            export type $200 = Components.Schemas.Project;
        }
    }
    namespace CreateExploration {
        export type RequestBody = Components.Schemas.ExplorationDto;
        namespace Responses {
            export type $200 = Components.Schemas.Project;
        }
    }
    namespace CreateProject {
        export type RequestBody = Components.Schemas.ProjectDto;
        namespace Responses {
            export type $200 = Components.Schemas.Project;
        }
    }
    namespace CreateSubstructure {
        export type RequestBody = Components.Schemas.SubstructureDto;
        namespace Responses {
            export type $200 = Components.Schemas.Project;
        }
    }
    namespace CreateSurf {
        export type RequestBody = Components.Schemas.SurfDto;
        namespace Responses {
            export type $200 = Components.Schemas.Project;
        }
    }
    namespace CreateTopside {
        export type RequestBody = Components.Schemas.TopsideDto;
        namespace Responses {
            export type $200 = Components.Schemas.Project;
        }
    }
    namespace CreateTransport {
        export type RequestBody = Components.Schemas.TransportDto;
        namespace Responses {
            export type $200 = Components.Schemas.Project;
        }
    }
    namespace CreateWellProject {
        export type RequestBody = Components.Schemas.WellProjectDto;
        namespace Responses {
            export type $200 = Components.Schemas.Project;
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
            export type $200 = Components.Schemas.Project;
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
            export type $200 = Components.Schemas.Project;
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
            export type $200 = Components.Schemas.Project;
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
            export type $200 = Components.Schemas.Project;
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
            export type $200 = Components.Schemas.Project;
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
            export type $200 = Components.Schemas.Project;
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
            export type $200 = Components.Schemas.Project;
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
            export type $200 = Components.Schemas.Project;
        }
    }
    namespace GetProjects {
        namespace Responses {
            export type $200 = Components.Schemas.Project[];
        }
    }
    namespace UpdateDrainageStrategy {
        namespace Parameters {
            export type DrainageStrategyId = string; // uuid
        }
        export interface PathParameters {
            drainageStrategyId: Parameters.DrainageStrategyId /* uuid */;
        }
        export type RequestBody = Components.Schemas.DrainageStrategyDto;
        namespace Responses {
            export type $200 = Components.Schemas.Project;
        }
    }
    namespace UpdateExploration {
        namespace Parameters {
            export type ExplorationId = string; // uuid
        }
        export interface PathParameters {
            explorationId: Parameters.ExplorationId /* uuid */;
        }
        export type RequestBody = Components.Schemas.ExplorationDto;
        namespace Responses {
            export type $200 = Components.Schemas.Project;
        }
    }
    namespace UpdateSubstructure {
        namespace Parameters {
            export type SubstructureId = string; // uuid
        }
        export interface PathParameters {
            substructureId: Parameters.SubstructureId /* uuid */;
        }
        export type RequestBody = Components.Schemas.SubstructureDto;
        namespace Responses {
            export type $200 = Components.Schemas.Project;
        }
    }
    namespace UpdateSurf {
        namespace Parameters {
            export type SurfId = string; // uuid
        }
        export interface PathParameters {
            surfId: Parameters.SurfId /* uuid */;
        }
        export type RequestBody = Components.Schemas.SurfDto;
        namespace Responses {
            export type $200 = Components.Schemas.Project;
        }
    }
    namespace UpdateTopside {
        namespace Parameters {
            export type TopsideId = string; // uuid
        }
        export interface PathParameters {
            topsideId: Parameters.TopsideId /* uuid */;
        }
        export type RequestBody = Components.Schemas.TopsideDto;
        namespace Responses {
            export type $200 = Components.Schemas.Project;
        }
    }
    namespace UpdateTransport {
        namespace Parameters {
            export type TransportId = string; // uuid
        }
        export interface PathParameters {
            transportId: Parameters.TransportId /* uuid */;
        }
        export type RequestBody = Components.Schemas.TransportDto;
        namespace Responses {
            export type $200 = Components.Schemas.Project;
        }
    }
    namespace UpdateWellProject {
        namespace Parameters {
            export type WellProjectId = string; // uuid
        }
        export interface PathParameters {
            wellProjectId: Parameters.WellProjectId /* uuid */;
        }
        export type RequestBody = Components.Schemas.WellProjectDto;
        namespace Responses {
            export type $200 = Components.Schemas.Project;
        }
    }
}

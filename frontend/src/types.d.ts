declare namespace Components {
    namespace Schemas {
        export interface APIUpdateCaseDto {
            name: string;
            description?: string | null;
            referenceCase?: boolean | null;
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
            capex?: number; // double
            capexYear?: CapexYear;
            sharepointFileId?: string | null;
            sharepointFileName?: string | null;
            sharepointFileUrl?: string | null;
        }
        export interface APIUpdateCaseWithProfilesDto {
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
            cessationOnshoreFacilitiesCostProfile?: UpdateCessationOnshoreFacilitiesCostProfileDto;
            totalFeasibilityAndConceptStudiesOverride?: UpdateTotalFeasibilityAndConceptStudiesOverrideDto;
            totalFEEDStudiesOverride?: UpdateTotalFEEDStudiesOverrideDto;
            totalOtherStudiesCostProfile?: UpdateTotalOtherStudiesCostProfileDto;
            historicCostCostProfile?: UpdateHistoricCostCostProfileDto;
            wellInterventionCostProfileOverride?: UpdateWellInterventionCostProfileOverrideDto;
            offshoreFacilitiesOperationsCostProfileOverride?: UpdateOffshoreFacilitiesOperationsCostProfileOverrideDto;
            onshoreRelatedOPEXCostProfile?: UpdateOnshoreRelatedOPEXCostProfileDto;
            additionalOPEXCostProfile?: UpdateAdditionalOPEXCostProfileDto;
            capex?: number; // double
            capexYear?: CapexYear;
            sharepointFileId?: string | null;
            sharepointFileName?: string | null;
            sharepointFileUrl?: string | null;
        }
        export interface APIUpdateSubstructureDto {
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
        export interface APIUpdateSurfDto {
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
        export interface APIUpdateTopsideDto {
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
        export interface APIUpdateTransportDto {
            gasExportPipelineLength?: number; // double
            oilExportPipelineLength?: number; // double
            currency?: Currency /* int32 */;
            costYear?: number; // int32
            dG3Date?: string | null; // date-time
            dG4Date?: string | null; // date-time
            source?: Source /* int32 */;
            maturity?: Maturity /* int32 */;
        }
        export interface AdditionalOPEXCostProfileDto {
            id: string; // uuid
            startYear: number; // int32
            values?: number /* double */[] | null;
            sum?: number; // double
            epaVersion: string;
            currency: Currency /* int32 */;
        }
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
        export interface CaseWithAssetsDto {
            case: CaseDto;
            cessationWellsCost?: CessationWellsCostDto;
            cessationWellsCostOverride?: CessationWellsCostOverrideDto;
            cessationOffshoreFacilitiesCost?: CessationOffshoreFacilitiesCostDto;
            cessationOffshoreFacilitiesCostOverride?: CessationOffshoreFacilitiesCostOverrideDto;
            cessationOnshoreFacilitiesCostProfile?: CessationOnshoreFacilitiesCostProfileDto;
            totalFeasibilityAndConceptStudies?: TotalFeasibilityAndConceptStudiesDto;
            totalFeasibilityAndConceptStudiesOverride?: TotalFeasibilityAndConceptStudiesOverrideDto;
            totalFEEDStudies?: TotalFEEDStudiesDto;
            totalFEEDStudiesOverride?: TotalFEEDStudiesOverrideDto;
            totalOtherStudiesCostProfile?: TotalOtherStudiesCostProfileDto;
            historicCostCostProfile?: HistoricCostCostProfileDto;
            wellInterventionCostProfile?: WellInterventionCostProfileDto;
            wellInterventionCostProfileOverride?: WellInterventionCostProfileOverrideDto;
            offshoreFacilitiesOperationsCostProfile?: OffshoreFacilitiesOperationsCostProfileDto;
            offshoreFacilitiesOperationsCostProfileOverride?: OffshoreFacilitiesOperationsCostProfileOverrideDto;
            onshoreRelatedOPEXCostProfile?: OnshoreRelatedOPEXCostProfileDto;
            additionalOPEXCostProfile?: AdditionalOPEXCostProfileDto;
            topside: TopsideDto;
            topsideCostProfile?: TopsideCostProfileDto;
            topsideCostProfileOverride?: TopsideCostProfileOverrideDto;
            topsideCessationCostProfile?: TopsideCessationCostProfileDto;
            drainageStrategy: DrainageStrategyDto;
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
            deferredOilProduction?: DeferredOilProductionDto;
            deferredGasProduction?: DeferredGasProductionDto;
            substructure: SubstructureDto;
            substructureCostProfile?: SubstructureCostProfileDto;
            substructureCostProfileOverride?: SubstructureCostProfileOverrideDto;
            substructureCessationCostProfile?: SubstructureCessationCostProfileDto;
            surf: SurfDto;
            surfCostProfile?: SurfCostProfileDto;
            surfCostProfileOverride?: SurfCostProfileOverrideDto;
            surfCessationCostProfile?: SurfCessationCostProfileDto;
            transport: TransportDto;
            transportCostProfile?: TransportCostProfileDto;
            transportCostProfileOverride?: TransportCostProfileOverrideDto;
            transportCessationCostProfile?: TransportCessationCostProfileDto;
            exploration: ExplorationDto;
            explorationWells?: ExplorationWellDto[] | null;
            explorationWellCostProfile?: ExplorationWellCostProfileDto;
            appraisalWellCostProfile?: AppraisalWellCostProfileDto;
            sidetrackCostProfile?: SidetrackCostProfileDto;
            gAndGAdminCost?: GAndGAdminCostDto;
            gAndGAdminCostOverride?: GAndGAdminCostOverrideDto;
            seismicAcquisitionAndProcessing?: SeismicAcquisitionAndProcessingDto;
            countryOfficeCost?: CountryOfficeCostDto;
            wellProject: WellProjectDto;
            wellProjectWells?: WellProjectWellDto[] | null;
            oilProducerCostProfile?: OilProducerCostProfileDto;
            oilProducerCostProfileOverride?: OilProducerCostProfileOverrideDto;
            gasProducerCostProfile?: GasProducerCostProfileDto;
            gasProducerCostProfileOverride?: GasProducerCostProfileOverrideDto;
            waterInjectorCostProfile?: WaterInjectorCostProfileDto;
            waterInjectorCostProfileOverride?: WaterInjectorCostProfileOverrideDto;
            gasInjectorCostProfile?: GasInjectorCostProfileDto;
            gasInjectorCostProfileOverride?: GasInjectorCostProfileOverrideDto;
        }
        export interface CaseWithProfilesDto {
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
            cessationOnshoreFacilitiesCostProfile: CessationOnshoreFacilitiesCostProfileDto;
            totalFeasibilityAndConceptStudies: TotalFeasibilityAndConceptStudiesDto;
            totalFeasibilityAndConceptStudiesOverride: TotalFeasibilityAndConceptStudiesOverrideDto;
            totalFEEDStudies: TotalFEEDStudiesDto;
            totalFEEDStudiesOverride: TotalFEEDStudiesOverrideDto;
            totalOtherStudiesCostProfile: TotalOtherStudiesCostProfileDto;
            historicCostCostProfile: HistoricCostCostProfileDto;
            wellInterventionCostProfile: WellInterventionCostProfileDto;
            wellInterventionCostProfileOverride: WellInterventionCostProfileOverrideDto;
            offshoreFacilitiesOperationsCostProfile: OffshoreFacilitiesOperationsCostProfileDto;
            offshoreFacilitiesOperationsCostProfileOverride: OffshoreFacilitiesOperationsCostProfileOverrideDto;
            onshoreRelatedOPEXCostProfile: OnshoreRelatedOPEXCostProfileDto;
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
        export interface CessationCostDto {
            id: string; // uuid
            startYear: number; // int32
            values?: number /* double */[] | null;
            sum?: number; // double
            epaVersion: string;
            currency: Currency /* int32 */;
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
        export interface CessationOnshoreFacilitiesCostProfileDto {
            id: string; // uuid
            startYear: number; // int32
            values?: number /* double */[] | null;
            sum?: number; // double
            epaVersion: string;
            currency: Currency /* int32 */;
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
        export interface CreateAdditionalOPEXCostProfileDto {
            startYear?: number; // int32
            values?: number /* double */[] | null;
            currency?: Currency /* int32 */;
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
        export interface CreateCessationOffshoreFacilitiesCostOverrideDto {
            startYear?: number; // int32
            values?: number /* double */[] | null;
            currency?: Currency /* int32 */;
            override?: boolean;
        }
        export interface CreateCessationOnshoreFacilitiesCostProfileDto {
            startYear?: number; // int32
            values?: number /* double */[] | null;
            currency?: Currency /* int32 */;
        }
        export interface CreateCessationWellsCostOverrideDto {
            startYear?: number; // int32
            values?: number /* double */[] | null;
            currency?: Currency /* int32 */;
            override?: boolean;
        }
        export interface CreateCo2EmissionsOverrideDto {
            startYear?: number; // int32
            values?: number /* double */[] | null;
            override?: boolean;
        }
        export interface CreateCountryOfficeCostDto {
            startYear?: number; // int32
            values?: number /* double */[] | null;
            currency?: Currency /* int32 */;
        }
        export interface CreateDeferredGasProductionDto {
            startYear?: number; // int32
            values?: number /* double */[] | null;
        }
        export interface CreateDeferredOilProductionDto {
            startYear?: number; // int32
            values?: number /* double */[] | null;
        }
        export interface CreateDrillingScheduleDto {
            startYear?: number; // int32
            values?: number /* int32 */[] | null;
        }
        export interface CreateFuelFlaringAndLossesOverrideDto {
            startYear?: number; // int32
            values?: number /* double */[] | null;
            override?: boolean;
        }
        export interface CreateGAndGAdminCostOverrideDto {
            startYear?: number; // int32
            values?: number /* double */[] | null;
            currency?: Currency /* int32 */;
            override?: boolean;
        }
        export interface CreateGasInjectorCostProfileOverrideDto {
            startYear?: number; // int32
            values?: number /* double */[] | null;
            currency?: Currency /* int32 */;
            override?: boolean;
        }
        export interface CreateGasProducerCostProfileOverrideDto {
            startYear?: number; // int32
            values?: number /* double */[] | null;
            currency?: Currency /* int32 */;
            override?: boolean;
        }
        export interface CreateHistoricCostCostProfileDto {
            startYear?: number; // int32
            values?: number /* double */[] | null;
            currency?: Currency /* int32 */;
        }
        export interface CreateImportedElectricityOverrideDto {
            startYear?: number; // int32
            values?: number /* double */[] | null;
            override?: boolean;
        }
        export interface CreateNetSalesGasOverrideDto {
            startYear?: number; // int32
            values?: number /* double */[] | null;
            override?: boolean;
        }
        export interface CreateOffshoreFacilitiesOperationsCostProfileOverrideDto {
            startYear?: number; // int32
            values?: number /* double */[] | null;
            currency?: Currency /* int32 */;
            override?: boolean;
        }
        export interface CreateOilProducerCostProfileOverrideDto {
            startYear?: number; // int32
            values?: number /* double */[] | null;
            currency?: Currency /* int32 */;
            override?: boolean;
        }
        export interface CreateOnshoreRelatedOPEXCostProfileDto {
            startYear?: number; // int32
            values?: number /* double */[] | null;
            currency?: Currency /* int32 */;
        }
        export interface CreateProductionProfileGasDto {
            startYear?: number; // int32
            values?: number /* double */[] | null;
        }
        export interface CreateProductionProfileOilDto {
            startYear?: number; // int32
            values?: number /* double */[] | null;
        }
        export interface CreateProductionProfileWaterDto {
            startYear?: number; // int32
            values?: number /* double */[] | null;
        }
        export interface CreateProductionProfileWaterInjectionDto {
            startYear?: number; // int32
            values?: number /* double */[] | null;
        }
        export interface CreateSeismicAcquisitionAndProcessingDto {
            startYear?: number; // int32
            values?: number /* double */[] | null;
            currency?: Currency /* int32 */;
        }
        export interface CreateSubstructureCostProfileOverrideDto {
            startYear?: number; // int32
            values?: number /* double */[] | null;
            currency?: Currency /* int32 */;
            override?: boolean;
        }
        export interface CreateSurfCostProfileOverrideDto {
            startYear?: number; // int32
            values?: number /* double */[] | null;
            currency?: Currency /* int32 */;
            override?: boolean;
        }
        export interface CreateTopsideCostProfileOverrideDto {
            startYear?: number; // int32
            values?: number /* double */[] | null;
            currency?: Currency /* int32 */;
            override?: boolean;
        }
        export interface CreateTotalFEEDStudiesOverrideDto {
            startYear?: number; // int32
            values?: number /* double */[] | null;
            currency?: Currency /* int32 */;
            override?: boolean;
        }
        export interface CreateTotalFeasibilityAndConceptStudiesOverrideDto {
            startYear?: number; // int32
            values?: number /* double */[] | null;
            currency?: Currency /* int32 */;
            override?: boolean;
        }
        export interface CreateTotalOtherStudiesCostProfileDto {
            startYear?: number; // int32
            values?: number /* double */[] | null;
            currency?: Currency /* int32 */;
        }
        export interface CreateTransportCostProfileOverrideDto {
            startYear?: number; // int32
            values?: number /* double */[] | null;
            currency?: Currency /* int32 */;
            override?: boolean;
        }
        export interface CreateWaterInjectorCostProfileOverrideDto {
            startYear?: number; // int32
            values?: number /* double */[] | null;
            currency?: Currency /* int32 */;
            override?: boolean;
        }
        export interface CreateWellDto {
            name: string;
            wellCategory: WellCategory /* int32 */;
            wellInterventionCost?: number; // double
            plugingAndAbandonmentCost?: number; // double
            wellCost?: number; // double
            drillingDays?: number; // double
        }
        export interface CreateWellInterventionCostProfileOverrideDto {
            startYear?: number; // int32
            values?: number /* double */[] | null;
            currency?: Currency /* int32 */;
            override?: boolean;
        }
        export type Currency = 1 | 2; // int32
        export interface DeferredGasProductionDto {
            id: string; // uuid
            startYear: number; // int32
            values?: number /* double */[] | null;
            sum?: number; // double
        }
        export interface DeferredOilProductionDto {
            id: string; // uuid
            startYear: number; // int32
            values?: number /* double */[] | null;
            sum?: number; // double
        }
        export interface DeleteWellDto {
            id: string; // uuid
        }
        export interface DevelopmentOperationalWellCostsDto {
            id: string; // uuid
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
        export interface DrainageStrategyWithProfilesDto {
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
            deferredOilProduction: DeferredOilProductionDto;
            deferredGasProduction: DeferredGasProductionDto;
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
            rigMobDemob: number; // double
            currency: Currency /* int32 */;
        }
        export interface ExplorationOperationalWellCostsDto {
            id: string; // uuid
            projectId: string; // uuid
            explorationRigUpgrading: number; // double
            explorationRigMobDemob: number; // double
            explorationProjectDrillingCosts: number; // double
            appraisalRigMobDemob: number; // double
            appraisalProjectDrillingCosts: number; // double
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
        }
        export interface ExplorationWithProfilesDto {
            id: string; // uuid
            projectId: string; // uuid
            name: string;
            explorationWellCostProfile: ExplorationWellCostProfileDto;
            appraisalWellCostProfile: AppraisalWellCostProfileDto;
            sidetrackCostProfile: SidetrackCostProfileDto;
            gAndGAdminCost: GAndGAdminCostDto;
            gAndGAdminCostOverride: GAndGAdminCostOverrideDto;
            seismicAcquisitionAndProcessing: SeismicAcquisitionAndProcessingDto;
            countryOfficeCost: CountryOfficeCostDto;
            rigMobDemob: number; // double
            currency: Currency /* int32 */;
            explorationWells: ExplorationWellDto[];
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
        export interface GAndGAdminCostOverrideDto {
            id: string; // uuid
            startYear: number; // int32
            values?: number /* double */[] | null;
            sum?: number; // double
            epaVersion: string;
            currency: Currency /* int32 */;
            override: boolean;
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
        export interface HistoricCostCostProfileDto {
            id: string; // uuid
            startYear: number; // int32
            values?: number /* double */[] | null;
            sum?: number; // double
            epaVersion: string;
            currency: Currency /* int32 */;
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
        export interface ImageDto {
            id: string; // uuid
            url: string; // uri
            createTime: string; // date-time
            description?: string | null;
            caseId: string; // uuid
            projectName: string;
            projectId: string; // uuid
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
        export interface OnshoreRelatedOPEXCostProfileDto {
            id: string; // uuid
            startYear: number; // int32
            values?: number /* double */[] | null;
            sum?: number; // double
            epaVersion: string;
            currency: Currency /* int32 */;
        }
        export interface OpexCostProfileDto {
            id: string; // uuid
            startYear: number; // int32
            values?: number /* double */[] | null;
            sum?: number; // double
            epaVersion: string;
            currency: Currency /* int32 */;
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
        export type ProjectClassification = 0 | 1 | 2 | 3; // int32
        export interface ProjectDto {
            classification: ProjectClassification /* int32 */;
            id: string; // uuid
            name: string;
            commonLibraryId: string; // uuid
            fusionProjectId: string; // uuid
            referenceCaseId: string; // uuid
            commonLibraryName: string;
            description: string;
            country: string;
            currency: Currency /* int32 */;
            physicalUnit: PhysUnit /* int32 */;
            createDate: string; // date-time
            projectPhase: ProjectPhase /* int32 */;
            projectCategory: ProjectCategory /* int32 */;
            explorationOperationalWellCosts: ExplorationOperationalWellCostsDto;
            developmentOperationalWellCosts: DevelopmentOperationalWellCostsDto;
            cases: CaseWithProfilesDto[];
            wells: WellDto[];
            explorations: ExplorationWithProfilesDto[];
            surfs: SurfWithProfilesDto[];
            substructures: SubstructureWithProfilesDto[];
            topsides: TopsideWithProfilesDto[];
            transports: TransportWithProfilesDto[];
            drainageStrategies: DrainageStrategyWithProfilesDto[];
            wellProjects: WellProjectWithProfilesDto[];
            sharepointSiteUrl?: string | null;
            cO2RemovedFromGas: number; // double
            cO2EmissionFromFuelGas: number; // double
            flaredGasPerProducedVolume: number; // double
            cO2EmissionsFromFlaredGas: number; // double
            cO2Vented: number; // double
            dailyEmissionFromDrillingRig: number; // double
            averageDevelopmentDrillingDays: number; // double
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
        export interface SubstructureWithProfilesDto {
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
        export interface SurfWithProfilesDto {
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
        }
        export interface TechnicalInputDto {
            developmentOperationalWellCostsDto?: DevelopmentOperationalWellCostsDto;
            explorationOperationalWellCostsDto?: ExplorationOperationalWellCostsDto;
            wellDtos?: WellDto[] | null;
            projectDto?: ProjectDto;
            explorationDto?: ExplorationWithProfilesDto;
            wellProjectDto?: WellProjectWithProfilesDto;
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
        export interface TopsideWithProfilesDto {
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
        export interface TotalOtherStudiesCostProfileDto {
            id: string; // uuid
            startYear: number; // int32
            values?: number /* double */[] | null;
            sum?: number; // double
            epaVersion: string;
            currency: Currency /* int32 */;
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
        export interface TransportWithProfilesDto {
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
        }
        export interface UpdateAdditionalOPEXCostProfileDto {
            startYear?: number; // int32
            values?: number /* double */[] | null;
            currency?: Currency /* int32 */;
        }
        export interface UpdateAppraisalWellCostProfileDto {
            startYear?: number; // int32
            values?: number /* double */[] | null;
            currency?: Currency /* int32 */;
        }
        export interface UpdateCessationOffshoreFacilitiesCostOverrideDto {
            startYear?: number; // int32
            values?: number /* double */[] | null;
            currency?: Currency /* int32 */;
            override?: boolean;
        }
        export interface UpdateCessationOnshoreFacilitiesCostProfileDto {
            startYear?: number; // int32
            values?: number /* double */[] | null;
            currency?: Currency /* int32 */;
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
        export interface UpdateCountryOfficeCostDto {
            startYear?: number; // int32
            values?: number /* double */[] | null;
            currency?: Currency /* int32 */;
        }
        export interface UpdateDeferredGasProductionDto {
            startYear?: number; // int32
            values?: number /* double */[] | null;
        }
        export interface UpdateDeferredOilProductionDto {
            startYear?: number; // int32
            values?: number /* double */[] | null;
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
        export interface UpdateDrillingScheduleDto {
            startYear?: number; // int32
            values?: number /* int32 */[] | null;
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
        export interface UpdateExplorationWellCostProfileDto {
            startYear?: number; // int32
            values?: number /* double */[] | null;
            currency?: Currency /* int32 */;
        }
        export interface UpdateExplorationWithProfilesDto {
            name?: string | null;
            rigMobDemob?: number; // double
            currency?: Currency /* int32 */;
            gAndGAdminCostOverride?: UpdateGAndGAdminCostOverrideDto;
            seismicAcquisitionAndProcessing?: UpdateSeismicAcquisitionAndProcessingDto;
            countryOfficeCost?: UpdateCountryOfficeCostDto;
            explorationWellCostProfile?: UpdateExplorationWellCostProfileDto;
            appraisalWellCostProfile?: UpdateAppraisalWellCostProfileDto;
            sidetrackCostProfile?: UpdateSidetrackCostProfileDto;
        }
        export interface UpdateFuelFlaringAndLossesOverrideDto {
            startYear?: number; // int32
            values?: number /* double */[] | null;
            override?: boolean;
        }
        export interface UpdateGAndGAdminCostOverrideDto {
            startYear?: number; // int32
            values?: number /* double */[] | null;
            currency?: Currency /* int32 */;
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
        export interface UpdateHistoricCostCostProfileDto {
            startYear?: number; // int32
            values?: number /* double */[] | null;
            currency?: Currency /* int32 */;
        }
        export interface UpdateImportedElectricityOverrideDto {
            startYear?: number; // int32
            values?: number /* double */[] | null;
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
        export interface UpdateOnshoreRelatedOPEXCostProfileDto {
            startYear?: number; // int32
            values?: number /* double */[] | null;
            currency?: Currency /* int32 */;
        }
        export interface UpdateProductionProfileGasDto {
            startYear?: number; // int32
            values?: number /* double */[] | null;
        }
        export interface UpdateProductionProfileOilDto {
            startYear?: number; // int32
            values?: number /* double */[] | null;
        }
        export interface UpdateProductionProfileWaterDto {
            startYear?: number; // int32
            values?: number /* double */[] | null;
        }
        export interface UpdateProductionProfileWaterInjectionDto {
            startYear?: number; // int32
            values?: number /* double */[] | null;
        }
        export interface UpdateProjectDto {
            name?: string | null;
            referenceCaseId?: string; // uuid
            commonLibraryName?: string | null;
            description?: string | null;
            country?: string | null;
            currency?: Currency /* int32 */;
            physicalUnit?: PhysUnit /* int32 */;
            classification?: ProjectClassification /* int32 */;
            projectPhase?: ProjectPhase /* int32 */;
            projectCategory?: ProjectCategory /* int32 */;
            explorationOperationalWellCosts?: UpdateExplorationOperationalWellCostsDto;
            developmentOperationalWellCosts?: UpdateDevelopmentOperationalWellCostsDto;
            sharepointSiteUrl?: string | null;
            cO2RemovedFromGas?: number; // double
            cO2EmissionFromFuelGas?: number; // double
            flaredGasPerProducedVolume?: number; // double
            cO2EmissionsFromFlaredGas?: number; // double
            cO2Vented?: number; // double
            dailyEmissionFromDrillingRig?: number; // double
            averageDevelopmentDrillingDays?: number; // double
        }
        export interface UpdateSeismicAcquisitionAndProcessingDto {
            startYear?: number; // int32
            values?: number /* double */[] | null;
            currency?: Currency /* int32 */;
        }
        export interface UpdateSidetrackCostProfileDto {
            startYear?: number; // int32
            values?: number /* double */[] | null;
            currency?: Currency /* int32 */;
        }
        export interface UpdateSubstructureCostProfileOverrideDto {
            startYear?: number; // int32
            values?: number /* double */[] | null;
            currency?: Currency /* int32 */;
            override?: boolean;
        }
        export interface UpdateSurfCostProfileOverrideDto {
            startYear?: number; // int32
            values?: number /* double */[] | null;
            currency?: Currency /* int32 */;
            override?: boolean;
        }
        export interface UpdateTechnicalInputDto {
            developmentOperationalWellCostsDto?: UpdateDevelopmentOperationalWellCostsDto;
            explorationOperationalWellCostsDto?: UpdateExplorationOperationalWellCostsDto;
            updateWellDtos?: UpdateWellDto[] | null;
            createWellDtos?: CreateWellDto[] | null;
            deleteWellDtos?: DeleteWellDto[] | null;
            projectDto?: UpdateProjectDto;
            explorationDto?: UpdateExplorationWithProfilesDto;
            wellProjectDto?: UpdateWellProjectWithProfilesDto;
        }
        export interface UpdateTopsideCostProfileOverrideDto {
            startYear?: number; // int32
            values?: number /* double */[] | null;
            currency?: Currency /* int32 */;
            override?: boolean;
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
        export interface UpdateTotalOtherStudiesCostProfileDto {
            startYear?: number; // int32
            values?: number /* double */[] | null;
            currency?: Currency /* int32 */;
        }
        export interface UpdateTransportCostProfileOverrideDto {
            startYear?: number; // int32
            values?: number /* double */[] | null;
            currency?: Currency /* int32 */;
            override?: boolean;
        }
        export interface UpdateWaterInjectorCostProfileOverrideDto {
            startYear?: number; // int32
            values?: number /* double */[] | null;
            currency?: Currency /* int32 */;
            override?: boolean;
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
        export interface UpdateWellInterventionCostProfileOverrideDto {
            startYear?: number; // int32
            values?: number /* double */[] | null;
            currency?: Currency /* int32 */;
            override?: boolean;
        }
        export interface UpdateWellProjectDto {
            name?: string | null;
            artificialLift?: ArtificialLift /* int32 */;
            currency?: Currency /* int32 */;
        }
        export interface UpdateWellProjectWithProfilesDto {
            name?: string | null;
            oilProducerCostProfileOverride?: UpdateOilProducerCostProfileOverrideDto;
            gasProducerCostProfileOverride?: UpdateGasProducerCostProfileOverrideDto;
            waterInjectorCostProfileOverride?: UpdateWaterInjectorCostProfileOverrideDto;
            gasInjectorCostProfileOverride?: UpdateGasInjectorCostProfileOverrideDto;
            artificialLift?: ArtificialLift /* int32 */;
            currency?: Currency /* int32 */;
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
        export interface WellProjectDto {
            id: string; // uuid
            projectId: string; // uuid
            name: string;
            artificialLift: ArtificialLift /* int32 */;
            currency: Currency /* int32 */;
        }
        export interface WellProjectWellDto {
            drillingSchedule: DrillingScheduleDto;
            wellProjectId: string; // uuid
            wellId: string; // uuid
        }
        export interface WellProjectWithProfilesDto {
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
        }
    }
}
declare namespace Paths {
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
    namespace Projects {
        namespace Post {
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
                export type $200 = Components.Schemas.ProjectDto;
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
                export type $200 = Components.Schemas.ProjectDto;
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
            export type RequestBody = Components.Schemas.APIUpdateCaseDto;
            namespace Responses {
                export type $200 = Components.Schemas.CaseDto;
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
            export type RequestBody = Components.Schemas.CreateAdditionalOPEXCostProfileDto;
            namespace Responses {
                export type $200 = Components.Schemas.AdditionalOPEXCostProfileDto;
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
            export type RequestBody = Components.Schemas.UpdateAdditionalOPEXCostProfileDto;
            namespace Responses {
                export type $200 = Components.Schemas.AdditionalOPEXCostProfileDto;
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
            export type RequestBody = Components.Schemas.CreateCessationOffshoreFacilitiesCostOverrideDto;
            namespace Responses {
                export type $200 = Components.Schemas.CessationOffshoreFacilitiesCostOverrideDto;
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
            export type RequestBody = Components.Schemas.UpdateCessationOffshoreFacilitiesCostOverrideDto;
            namespace Responses {
                export type $200 = Components.Schemas.CessationOffshoreFacilitiesCostOverrideDto;
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
            export type RequestBody = Components.Schemas.CreateCessationOnshoreFacilitiesCostProfileDto;
            namespace Responses {
                export type $200 = Components.Schemas.CessationOnshoreFacilitiesCostProfileDto;
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
            export type RequestBody = Components.Schemas.UpdateCessationOnshoreFacilitiesCostProfileDto;
            namespace Responses {
                export type $200 = Components.Schemas.CessationOnshoreFacilitiesCostProfileDto;
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
            export type RequestBody = Components.Schemas.CreateCessationWellsCostOverrideDto;
            namespace Responses {
                export type $200 = Components.Schemas.CessationWellsCostOverrideDto;
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
            export type RequestBody = Components.Schemas.UpdateCessationWellsCostOverrideDto;
            namespace Responses {
                export type $200 = Components.Schemas.CessationWellsCostOverrideDto;
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
                export type $200 = Components.Schemas.DrainageStrategyDto;
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
            export type RequestBody = Components.Schemas.CreateCo2EmissionsOverrideDto;
            namespace Responses {
                export type $200 = Components.Schemas.Co2EmissionsOverrideDto;
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
            export type RequestBody = Components.Schemas.UpdateCo2EmissionsOverrideDto;
            namespace Responses {
                export type $200 = Components.Schemas.Co2EmissionsOverrideDto;
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
            export type RequestBody = Components.Schemas.CreateDeferredGasProductionDto;
            namespace Responses {
                export type $200 = Components.Schemas.DeferredGasProductionDto;
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
            export type RequestBody = Components.Schemas.UpdateDeferredGasProductionDto;
            namespace Responses {
                export type $200 = Components.Schemas.DeferredGasProductionDto;
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
            export type RequestBody = Components.Schemas.CreateDeferredOilProductionDto;
            namespace Responses {
                export type $200 = Components.Schemas.DeferredOilProductionDto;
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
            export type RequestBody = Components.Schemas.UpdateDeferredOilProductionDto;
            namespace Responses {
                export type $200 = Components.Schemas.DeferredOilProductionDto;
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
            export type RequestBody = Components.Schemas.CreateFuelFlaringAndLossesOverrideDto;
            namespace Responses {
                export type $200 = Components.Schemas.FuelFlaringAndLossesOverrideDto;
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
            export type RequestBody = Components.Schemas.UpdateFuelFlaringAndLossesOverrideDto;
            namespace Responses {
                export type $200 = Components.Schemas.FuelFlaringAndLossesOverrideDto;
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
            export type RequestBody = Components.Schemas.CreateImportedElectricityOverrideDto;
            namespace Responses {
                export type $200 = Components.Schemas.ImportedElectricityOverrideDto;
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
            export type RequestBody = Components.Schemas.UpdateImportedElectricityOverrideDto;
            namespace Responses {
                export type $200 = Components.Schemas.ImportedElectricityOverrideDto;
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
            export type RequestBody = Components.Schemas.CreateNetSalesGasOverrideDto;
            namespace Responses {
                export type $200 = Components.Schemas.NetSalesGasOverrideDto;
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
            export type RequestBody = Components.Schemas.UpdateNetSalesGasOverrideDto;
            namespace Responses {
                export type $200 = Components.Schemas.NetSalesGasOverrideDto;
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
            export type RequestBody = Components.Schemas.CreateProductionProfileGasDto;
            namespace Responses {
                export type $200 = Components.Schemas.ProductionProfileGasDto;
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
            export type RequestBody = Components.Schemas.UpdateProductionProfileGasDto;
            namespace Responses {
                export type $200 = Components.Schemas.ProductionProfileGasDto;
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
            export type RequestBody = Components.Schemas.CreateProductionProfileOilDto;
            namespace Responses {
                export type $200 = Components.Schemas.ProductionProfileOilDto;
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
            export type RequestBody = Components.Schemas.UpdateProductionProfileOilDto;
            namespace Responses {
                export type $200 = Components.Schemas.ProductionProfileOilDto;
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
            export type RequestBody = Components.Schemas.CreateProductionProfileWaterDto;
            namespace Responses {
                export type $200 = Components.Schemas.ProductionProfileWaterDto;
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
            export type RequestBody = Components.Schemas.UpdateProductionProfileWaterDto;
            namespace Responses {
                export type $200 = Components.Schemas.ProductionProfileWaterDto;
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
            export type RequestBody = Components.Schemas.CreateProductionProfileWaterInjectionDto;
            namespace Responses {
                export type $200 = Components.Schemas.ProductionProfileWaterInjectionDto;
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
            export type RequestBody = Components.Schemas.UpdateProductionProfileWaterInjectionDto;
            namespace Responses {
                export type $200 = Components.Schemas.ProductionProfileWaterInjectionDto;
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
                export type $200 = Components.Schemas.ExplorationDto;
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
            export type RequestBody = Components.Schemas.CreateCountryOfficeCostDto;
            namespace Responses {
                export type $200 = Components.Schemas.CountryOfficeCostDto;
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
            export type RequestBody = Components.Schemas.UpdateCountryOfficeCostDto;
            namespace Responses {
                export type $200 = Components.Schemas.CountryOfficeCostDto;
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
            export type RequestBody = Components.Schemas.CreateGAndGAdminCostOverrideDto;
            namespace Responses {
                export type $200 = Components.Schemas.GAndGAdminCostOverrideDto;
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
            export type RequestBody = Components.Schemas.UpdateGAndGAdminCostOverrideDto;
            namespace Responses {
                export type $200 = Components.Schemas.GAndGAdminCostOverrideDto;
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
            export type RequestBody = Components.Schemas.CreateSeismicAcquisitionAndProcessingDto;
            namespace Responses {
                export type $200 = Components.Schemas.SeismicAcquisitionAndProcessingDto;
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
            export type RequestBody = Components.Schemas.UpdateSeismicAcquisitionAndProcessingDto;
            namespace Responses {
                export type $200 = Components.Schemas.SeismicAcquisitionAndProcessingDto;
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
            export type RequestBody = Components.Schemas.CreateDrillingScheduleDto;
            namespace Responses {
                export type $200 = Components.Schemas.DrillingScheduleDto;
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
            export type RequestBody = Components.Schemas.UpdateDrillingScheduleDto;
            namespace Responses {
                export type $200 = Components.Schemas.DrillingScheduleDto;
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
            export type RequestBody = Components.Schemas.CreateHistoricCostCostProfileDto;
            namespace Responses {
                export type $200 = Components.Schemas.HistoricCostCostProfileDto;
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
            export type RequestBody = Components.Schemas.UpdateHistoricCostCostProfileDto;
            namespace Responses {
                export type $200 = Components.Schemas.HistoricCostCostProfileDto;
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
                projectName?: string;
                ContentType?: string;
                ContentDisposition?: string;
                Headers?: {
                    [name: string]: string[];
                };
                Length?: number; // int64
                Name?: string;
                FileName?: string;
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
            export type RequestBody = Components.Schemas.CreateOffshoreFacilitiesOperationsCostProfileOverrideDto;
            namespace Responses {
                export type $200 = Components.Schemas.OffshoreFacilitiesOperationsCostProfileOverrideDto;
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
            export type RequestBody = Components.Schemas.UpdateOffshoreFacilitiesOperationsCostProfileOverrideDto;
            namespace Responses {
                export type $200 = Components.Schemas.OffshoreFacilitiesOperationsCostProfileOverrideDto;
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
            export type RequestBody = Components.Schemas.CreateOnshoreRelatedOPEXCostProfileDto;
            namespace Responses {
                export type $200 = Components.Schemas.OnshoreRelatedOPEXCostProfileDto;
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
            export type RequestBody = Components.Schemas.UpdateOnshoreRelatedOPEXCostProfileDto;
            namespace Responses {
                export type $200 = Components.Schemas.OnshoreRelatedOPEXCostProfileDto;
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
            export type RequestBody = Components.Schemas.APIUpdateSubstructureDto;
            namespace Responses {
                export type $200 = Components.Schemas.SubstructureDto;
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
            export type RequestBody = Components.Schemas.CreateSubstructureCostProfileOverrideDto;
            namespace Responses {
                export type $200 = Components.Schemas.SubstructureCostProfileOverrideDto;
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
            export type RequestBody = Components.Schemas.UpdateSubstructureCostProfileOverrideDto;
            namespace Responses {
                export type $200 = Components.Schemas.SubstructureCostProfileOverrideDto;
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
            export type RequestBody = Components.Schemas.APIUpdateSurfDto;
            namespace Responses {
                export type $200 = Components.Schemas.SurfDto;
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
            export type RequestBody = Components.Schemas.CreateSurfCostProfileOverrideDto;
            namespace Responses {
                export type $200 = Components.Schemas.SurfCostProfileOverrideDto;
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
            export type RequestBody = Components.Schemas.UpdateSurfCostProfileOverrideDto;
            namespace Responses {
                export type $200 = Components.Schemas.SurfCostProfileOverrideDto;
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
            export type RequestBody = Components.Schemas.APIUpdateTopsideDto;
            namespace Responses {
                export type $200 = Components.Schemas.TopsideDto;
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
            export type RequestBody = Components.Schemas.CreateTopsideCostProfileOverrideDto;
            namespace Responses {
                export type $200 = Components.Schemas.TopsideCostProfileOverrideDto;
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
            export type RequestBody = Components.Schemas.UpdateTopsideCostProfileOverrideDto;
            namespace Responses {
                export type $200 = Components.Schemas.TopsideCostProfileOverrideDto;
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
            export type RequestBody = Components.Schemas.CreateTotalFeasibilityAndConceptStudiesOverrideDto;
            namespace Responses {
                export type $200 = Components.Schemas.TotalFeasibilityAndConceptStudiesOverrideDto;
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
            export type RequestBody = Components.Schemas.UpdateTotalFeasibilityAndConceptStudiesOverrideDto;
            namespace Responses {
                export type $200 = Components.Schemas.TotalFeasibilityAndConceptStudiesOverrideDto;
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
            export type RequestBody = Components.Schemas.CreateTotalFEEDStudiesOverrideDto;
            namespace Responses {
                export type $200 = Components.Schemas.TotalFEEDStudiesOverrideDto;
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
            export type RequestBody = Components.Schemas.UpdateTotalFEEDStudiesOverrideDto;
            namespace Responses {
                export type $200 = Components.Schemas.TotalFEEDStudiesOverrideDto;
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
            export type RequestBody = Components.Schemas.CreateTotalOtherStudiesCostProfileDto;
            namespace Responses {
                export type $200 = Components.Schemas.TotalOtherStudiesCostProfileDto;
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
            export type RequestBody = Components.Schemas.UpdateTotalOtherStudiesCostProfileDto;
            namespace Responses {
                export type $200 = Components.Schemas.TotalOtherStudiesCostProfileDto;
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
            export type RequestBody = Components.Schemas.APIUpdateTransportDto;
            namespace Responses {
                export type $200 = Components.Schemas.TransportDto;
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
            export type RequestBody = Components.Schemas.CreateTransportCostProfileOverrideDto;
            namespace Responses {
                export type $200 = Components.Schemas.TransportCostProfileOverrideDto;
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
            export type RequestBody = Components.Schemas.UpdateTransportCostProfileOverrideDto;
            namespace Responses {
                export type $200 = Components.Schemas.TransportCostProfileOverrideDto;
            }
        }
    }
    namespace Projects$ProjectIdCases$CaseIdUpdateCaseAndProfiles {
        namespace Put {
            namespace Parameters {
                export type CaseId = string; // uuid
                export type ProjectId = string;
            }
            export interface PathParameters {
                caseId: Parameters.CaseId /* uuid */;
                projectId: Parameters.ProjectId;
            }
            export type RequestBody = Components.Schemas.APIUpdateCaseWithProfilesDto;
            namespace Responses {
                export type $200 = Components.Schemas.ProjectDto;
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
            export type RequestBody = Components.Schemas.CreateWellInterventionCostProfileOverrideDto;
            namespace Responses {
                export type $200 = Components.Schemas.WellInterventionCostProfileOverrideDto;
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
            export type RequestBody = Components.Schemas.UpdateWellInterventionCostProfileOverrideDto;
            namespace Responses {
                export type $200 = Components.Schemas.WellInterventionCostProfileOverrideDto;
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
                export type $200 = Components.Schemas.WellProjectDto;
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
            export type RequestBody = Components.Schemas.CreateGasInjectorCostProfileOverrideDto;
            namespace Responses {
                export type $200 = Components.Schemas.GasInjectorCostProfileOverrideDto;
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
            export type RequestBody = Components.Schemas.UpdateGasInjectorCostProfileOverrideDto;
            namespace Responses {
                export type $200 = Components.Schemas.GasInjectorCostProfileOverrideDto;
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
            export type RequestBody = Components.Schemas.CreateGasProducerCostProfileOverrideDto;
            namespace Responses {
                export type $200 = Components.Schemas.GasProducerCostProfileOverrideDto;
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
            export type RequestBody = Components.Schemas.UpdateGasProducerCostProfileOverrideDto;
            namespace Responses {
                export type $200 = Components.Schemas.GasProducerCostProfileOverrideDto;
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
            export type RequestBody = Components.Schemas.CreateOilProducerCostProfileOverrideDto;
            namespace Responses {
                export type $200 = Components.Schemas.OilProducerCostProfileOverrideDto;
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
            export type RequestBody = Components.Schemas.UpdateOilProducerCostProfileOverrideDto;
            namespace Responses {
                export type $200 = Components.Schemas.OilProducerCostProfileOverrideDto;
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
            export type RequestBody = Components.Schemas.CreateWaterInjectorCostProfileOverrideDto;
            namespace Responses {
                export type $200 = Components.Schemas.WaterInjectorCostProfileOverrideDto;
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
            export type RequestBody = Components.Schemas.UpdateWaterInjectorCostProfileOverrideDto;
            namespace Responses {
                export type $200 = Components.Schemas.WaterInjectorCostProfileOverrideDto;
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
            export type RequestBody = Components.Schemas.CreateDrillingScheduleDto;
            namespace Responses {
                export type $200 = Components.Schemas.DrillingScheduleDto;
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
            export type RequestBody = Components.Schemas.UpdateDrillingScheduleDto;
            namespace Responses {
                export type $200 = Components.Schemas.DrillingScheduleDto;
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
                projectName?: string;
                ContentType?: string;
                ContentDisposition?: string;
                Headers?: {
                    [name: string]: string[];
                };
                Length?: number; // int64
                Name?: string;
                FileName?: string;
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
    }
    namespace Projects$ProjectIdTechnicalInput {
        namespace Put {
            namespace Parameters {
                export type ProjectId = string; // uuid
            }
            export interface PathParameters {
                projectId: Parameters.ProjectId /* uuid */;
            }
            export type RequestBody = Components.Schemas.UpdateTechnicalInputDto;
            namespace Responses {
                export type $200 = Components.Schemas.TechnicalInputDto;
            }
        }
    }
    namespace Projects$ProjectIdWells$WellId {
        namespace Put {
            namespace Parameters {
                export type ProjectId = string; // uuid
                export type WellId = string; // uuid
            }
            export interface PathParameters {
                projectId: Parameters.ProjectId /* uuid */;
                wellId: Parameters.WellId /* uuid */;
            }
            export type RequestBody = Components.Schemas.UpdateWellDto;
            namespace Responses {
                export type $200 = Components.Schemas.WellDto;
            }
        }
    }
}

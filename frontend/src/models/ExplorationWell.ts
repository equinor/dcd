export class ExplorationWell implements Components.Schemas.ExplorationWellDto {
    drillingSchedule?: Components.Schemas.DrillingScheduleDto
    explorationId?: string
    wellId?: string

    constructor(data?: Components.Schemas.ExplorationWellDto) {
        this.drillingSchedule = data?.drillingSchedule
        this.explorationId = data?.explorationId
        this.wellId = data?.wellId
    }

    static fromJSON(data: Components.Schemas.ExplorationWellDto): ExplorationWell {
        return new ExplorationWell(data)
    }
}

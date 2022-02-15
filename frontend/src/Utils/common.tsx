export function GetDrainageStrategy(project: Components.Schemas.ProjectDto, drainageStrategyId?: string) {
  return project.drainageStrategies?.find(o => o.id === drainageStrategyId);
};
export function ProjectPhaseNumberToText(phaseNumber: Components.Schemas.ProjectPhase) {
    return "DG" + (phaseNumber+1).toString()
}

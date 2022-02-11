export function GetDrainageStrategy(project: Components.Schemas.ProjectDto, drainageStrategyId?: string) {
  return project.drainageStrategies?.find(o => o.id === drainageStrategyId);
};

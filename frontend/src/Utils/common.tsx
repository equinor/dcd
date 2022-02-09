import Project = Components.Schemas.Project;

export function GetDrainageStrategy(project: Project, drainageStrategyId: string | undefined) {
  return project.drainageStrategies?.find(o => o.id === drainageStrategyId);
};
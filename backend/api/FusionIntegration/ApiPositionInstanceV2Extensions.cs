namespace Api.Services.FusionIntegration
{
    using System;
    using System.Linq;

    using Fusion.ApiClients.Org;

    public static class ApiPositionInstanceV2Extensions
    {
        public static ApiPositionInstanceV2? LatestPositionInstance(this ApiPositionV2? position, DateTime todayDate)
        {
            // A position might have multiple "instances". I.e. different person might have held the same position
            // at different times.
            // Here we pick the "latest" instance, assuming that one will be the most current person holding the
            // position. However, we also need to check that the "AppliesTo" of the latest instance has not passed
            // (the case where the position exists but is currently not occupied by a person).
            return position?.Instances?
                .GroupBy(instance => instance.AppliesFrom)
                .Select(instanceGroup => instanceGroup.OrderByDescending(instance => instance.AppliesFrom).FirstOrDefault())
                .Where(instance => instance?.AppliesFrom <= todayDate)
                .FirstOrDefault(instance => instance?.AppliesTo >= todayDate);
        }
    }
}

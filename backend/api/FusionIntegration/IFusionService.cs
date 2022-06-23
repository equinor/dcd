namespace Api.Services.FusionIntegration
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Api.Services.Models;

    public interface IFusionService
    {
        /// <summary>
        /// Obtain a <see cref="ProjectMaster"/> based on the given context ID/projectMasterId.
        /// </summary>
        /// <param name="contextId">The projectMaster ID to query for.</param>
        /// <returns>A <see cref="ProjectMaster"/> for the given id.</returns>
        /// <exception cref="OperationFailed">If no projectMaster was found for the given ID.</exception>
        public Task<ProjectMaster> ProjectMasterAsync(Guid contextId);

        /// <summary>
        /// Use the organizational chart to resolve multiple person IDs (Azure Unique ID) from a given
        /// list of Fusion position IDs.
        /// </summary>
        /// <param name="positionIds">The position IDs to query for.</param>
        /// <returns>
        /// A Dictionary of Position IDs and the person who currently has the given position. If no person
        /// was found, a null ID will be present for the given position ID in the dictionary.
        /// </returns>
        public Task<IDictionary<Guid, Guid?>> ResolvePersonIdsFromPositionIdsAsync(IEnumerable<Guid> positionIds);

        /// <summary>
        /// Use the organizational chart to resolve a persons ID (Azure Unique ID) from a given Fusion position ID.
        /// Overload of the more generic <see cref="ResolvePersonIdsFromPositionIdsAsync"/>.
        /// </summary>
        /// <param name="positionId">The position to query for.</param>
        /// <returns>
        /// A Dictionary of Position IDs and the person who currently has the given position.
        /// <c>null</c> will be returned if the person could not be resolved.
        /// </returns>
        public Task<Guid?> ResolvePersonIdFromPositionIdAsync(Guid positionId);

        /// <summary>
        /// Obtain the email of a given Person, identified by the <c>azureUniqueId</c>.
        /// </summary>
        /// <param name="azureUniqueId">The unique ID of the person to lookup the email for.</param>
        /// <returns>The email of the given person. <c>null</c> if not found.</returns>
        public Task<string?> ResolveUserEmailFromPersonId(Guid azureUniqueId);
    }
}

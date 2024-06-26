using api.Models;
using api.Services;
using api.Services.GenerateCostProfiles;

namespace api.Services.Observers;

public class CaseChangedEventHandler : IDomainEventHandler
{
    private readonly IRecalculationService _recalculationService;

    public CaseChangedEventHandler(IRecalculationService recalculationService)
    {
        _recalculationService = recalculationService;
    }

    public void Handle(DomainEvent domainEvent)
    {
        if (domainEvent.EventType == "CaseChanged")
        {
        }
    }
}

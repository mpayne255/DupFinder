using DupFinder.Domain;

namespace DupFinder.Application.Services.Interfaces
{
    public interface IDuplicateService
    {
        DuplicateResult GetDuplicates();
    }
}

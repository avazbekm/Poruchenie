using Poruchenie.Domain.Etities;
using Poruchenie.Service.Exceptions;

namespace Poruchenie.Service.Interfaces;

public interface IBlockDateService
{
    Task<Response<BlockedDate>> CreateAsync();
    Task<Response<BlockedDate>> GetAsync();
    Task<Response<BlockedDate>> UpdateAsync();
}

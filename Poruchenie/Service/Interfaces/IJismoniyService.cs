using Poruchenie.Domain.Etities;
using Poruchenie.Service.Exceptions;

namespace Poruchenie.Service.Interfaces;

public interface IJismoniyService
{
    Task<Response<Jismoniy>> CreateAsync(Jismoniy jismoniy);
    Task<Response<Jismoniy>> GetByCountNumberAsync(string count);
}

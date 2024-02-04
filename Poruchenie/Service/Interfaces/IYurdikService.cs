using Poruchenie.Domain.Etities;
using Poruchenie.Service.Exceptions;

namespace Poruchenie.Service.Interfaces;

public interface IYurdikService
{
    Task<Response<Yurdik>> CreateAsync(Yurdik yurdik);
    Task<Response<Yurdik>> GetByCountNumberAsync(string count);
}
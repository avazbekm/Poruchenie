using Poruchenie.Domain.Commons;
using System.Linq.Expressions;

namespace Poruchenie.Data;

public interface IRepository<T> where T : Auditable
{
    ValueTask CreateAsync(T entity);
    void Update(T entity);
    void Delete(T entity);
    void Destroy(T entity);
    ValueTask<T> SelectAsync(Expression<Func<T, bool>> expression, string[] includes = null);
    ValueTask SaveAsync();
}

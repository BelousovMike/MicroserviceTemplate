namespace Microservice.Template.Infrastructure.Data;

/// <summary>
/// Пример репозитория.
/// </summary>
/// <param name="dbContext">Контекст БД.</param>
/// <typeparam name="T">Тип сущности.</typeparam>
public class EfRepository<T>(AppDbContext dbContext) :
  RepositoryBase<T>(dbContext), IReadRepository<T>, IRepository<T>
    where T : class, IAggregateRoot
    {
}

using Marten;
using Wolverine.API.Models;
using Wolverine.Http;

namespace Wolverine.API.Features;

public static class GetAllTodosEndpoint
{
    [WolverineGet("/todos")]
    public static async Task<IReadOnlyList<Todo>> GetAllTodos(IQuerySession session)
    {
        return await session.Query<Todo>().ToListAsync();
    }
}
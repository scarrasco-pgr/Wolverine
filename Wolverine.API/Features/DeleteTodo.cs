using Events;
using Marten;
using Wolverine.Http;

namespace Wolverine.API.Features
{
    public static class DeleteTodoEndpoint
    {
        [WolverineDelete("/todos/{Id}")]
        public static (IResult, TodoDeleted) UpdateTodo(Guid Id, IDocumentSession session)
        {

            session.Events.ArchiveStream(Id);
            return (
                Results.NoContent(),
                new TodoDeleted(Id)
            );
        }
    }
}

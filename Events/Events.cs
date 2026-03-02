namespace Events
{
    public record TodoCreated(Guid Id, string Description, bool Completed);
    public record TodoDeleted(Guid Id);
    public record TodoUpdated(Guid Id, string Description, bool Completed);
}

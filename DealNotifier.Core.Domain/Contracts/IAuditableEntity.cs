namespace DealNotifier.Core.Domain.Contracts
{
    public interface IAuditableEntity
    {
        int Id { get; set; }
        string CreatedBy { get; set; }
        DateTime Created { get; set; }
        string? LastModifiedBy { get; set; }
        DateTime? LastModified { get; set; }
    }
}
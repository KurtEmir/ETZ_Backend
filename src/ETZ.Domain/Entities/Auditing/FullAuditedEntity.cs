namespace ETZ.Domain.Entities.Auditing;

public abstract class FullAuditedEntity<TKey> 
{
    public required TKey Id {get; set;}
    public bool IsDeleted { get; set; } = false;
    public Guid CreatorUserId { get; set; }
    public DateTimeOffset CreationTime { get; set; } 
    public Guid? DeleterUserId { get; set; }
    public DateTimeOffset? DeletionTime { get; set; }
    public Guid? LastModifierUserId { get; set; }
    public DateTimeOffset? LastModificationTime { get; set; }
}
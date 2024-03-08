namespace WatsonsCase.Domain.Core.Base.Concrete;

public abstract class BaseEntity
{
    public long Id { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreatedDate { get; set; }

    public string CreatedUserId { get; set; }

    public DateTime? ModifiedDate { get; set; }

    public string? ModifiedUserId { get; set; }

    public DateTime? DeletedDate { get; set; }

    public string? DeletedUserId { get; set; }
}
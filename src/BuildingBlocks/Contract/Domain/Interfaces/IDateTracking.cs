namespace Contract.Domain.Interfaces;

public interface IDateTracking
{
   DateTimeOffset CreatedTime { get; set; }
   DateTimeOffset? LastModifiedTime { get; set; }
}

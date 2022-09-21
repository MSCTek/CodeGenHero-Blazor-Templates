namespace $safeprojectname$.Audit;

public interface IAuditEntity
{
    /// Setup - Set these to match any Audit Fields your entities may have, and implement on your generated Entity classes.

    DateTimeOffset CreatedDate { get; set; }

    string CreatedBy { get; set; }

    DateTimeOffset ModifiedDate { get; set; }

    string ModifiedBy { get; set; }
}

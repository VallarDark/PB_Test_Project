namespace Domain.Aggregates.UserAggregate
{
    /// <summary>
    /// User roles. Should be placed in order of growing access level.
    /// </summary>
    public enum UserRoleType
    {
        User,
        Admin
    }
}

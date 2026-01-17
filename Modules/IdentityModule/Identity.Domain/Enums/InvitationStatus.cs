namespace Identity.Domain.Enums
{
    /// <summary>
    /// Status of a tenant invitation
    /// </summary>
    public enum InvitationStatus
    {
        /// <summary>
        /// Invitation sent, awaiting acceptance
        /// </summary>
        Pending = 0,

        /// <summary>
        /// Invitation accepted by user
        /// </summary>
        Accepted = 1,

        /// <summary>
        /// Invitation expired (past expiry date)
        /// </summary>
        Expired = 2,

        /// <summary>
        /// Invitation revoked by admin
        /// </summary>
        Revoked = 3
    }
}

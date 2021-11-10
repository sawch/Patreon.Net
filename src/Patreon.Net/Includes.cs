using System;

namespace Patreon.Net
{
    /// <summary>
    /// 
    /// </summary>
    [Flags]
    public enum Includes
    {
        /// <summary>
        /// Do not include any additional information.
        /// </summary>
        None = 0,
        [Include("tiers", typeof(Models.Tier))] Tiers = 1 << 0,
        [Include("creator", typeof(Models.User))] Creator = 1 << 1,
        [Include("benefits", typeof(Models.Benefit))] Benefits = 1 << 2,
        [Include("goals", typeof(Models.Goal))] Goals = 1 << 3,
        [Include("address", typeof(Models.Address))] Address = 1 << 4,
        [Include("campaign", typeof(Models.Campaign))] Campaign = 1 << 5,
        [Include("currently_entitled_tiers", typeof(Models.Tier))] CurrentlyEntitledTiers = 1 << 6,
        [Include("user", typeof(Models.User))] User = 1 << 7,
        [Include("memberships", typeof(Models.Member))] Memberships = 1 << 8,
        /// <summary>
        /// Include all supported information on the resource being fetched.
        /// </summary>
        All = int.MaxValue
    }

    /// <summary>
    /// Used to specify the field name and resource type of an include for use in API URLs.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    internal sealed class IncludeAttribute : Attribute
    {
        /// <summary>
        /// The name of the include field in the url, such as "tiers" (include=tiers).
        /// </summary>
        public readonly string includeName;
        /// <summary>
        /// The type of resource such as <see cref="Models.Member"/> or <see cref="Models.Campaign"/>.
        /// </summary>
        public readonly Type resourceType;

        public IncludeAttribute(string includeName, Type resourceType)
        {
            this.includeName = includeName; this.resourceType = resourceType;
        }
    }
}

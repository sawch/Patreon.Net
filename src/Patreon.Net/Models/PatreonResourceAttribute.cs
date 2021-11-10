using System;
using System.Collections.Generic;
using System.Text;

namespace Patreon.Net.Models
{
    /// <summary>
    /// Used to specify the name of a Patreon resource for use in endpoint fields.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    internal sealed class PatreonResourceAttribute : Attribute
    {
        /// <summary>
        /// The name of this resource for use in endpoint fields, such as "user" or "campaign".
        /// </summary>
        public readonly string fieldName;

        /// <summary>
        /// Creates a new instance of a <see cref="PatreonResourceAttribute"/>.
        /// </summary>
        /// <param name="fieldName">The name of this resource for use in endpoint fields, such as "user" or "campaign".</param>
        public PatreonResourceAttribute(string fieldName)
        {
            this.fieldName = fieldName;
        }
    }
}

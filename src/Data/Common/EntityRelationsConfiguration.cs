using Microsoft.EntityFrameworkCore;
using System;

namespace Data.Common
{
    internal static class EntityRelationsConfiguration
    {
#pragma warning disable IDE0060 // Remove unused parameter
        public static void Configure(this ModelBuilder modelBuilder)
#pragma warning restore IDE0060 // Remove unused parameter
        {

        }

        public static void ConfigureUserIdentityRelations(this ModelBuilder builder, Type contextType)
            => builder.ApplyConfigurationsFromAssembly(contextType.Assembly);
       
    }
}

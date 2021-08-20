using System;
using System.Collections.Generic;
using Entities.Constants;
using Entities.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Entities
{
    public class ApplicationDbContext : IdentityDbContext<User, IdentityRole<Guid>, Guid, IdentityUserClaim<Guid>,
        IdentityUserRole<Guid>, IdentityUserLogin<Guid>, IdentityRoleClaim<Guid>, IdentityUserToken<Guid>>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            UserRoleSeed(builder);

            base.OnModelCreating(builder);
        }

        private void UserRoleSeed(ModelBuilder builder)
        {
            builder.Entity<IdentityRole<Guid>>()
                .HasData(new List<IdentityRole<Guid>>
                {
                    new()
                    {
                        Id = Guid.NewGuid(),
                        Name = UserRoleConstants.Administrator,
                        NormalizedName = UserRoleConstants.Administrator.ToUpper()
                    },
                    new()
                    {
                        Id = Guid.NewGuid(),
                        Name = UserRoleConstants.Correspondent,
                        NormalizedName = UserRoleConstants.Correspondent.ToUpper()
                    },
                    new()
                    {
                        Id = Guid.NewGuid(),
                        Name = UserRoleConstants.MediaAnalyst,
                        NormalizedName = UserRoleConstants.MediaAnalyst.ToUpper()
                    },
                    new()
                    {
                        Id = Guid.NewGuid(),
                        Name = UserRoleConstants.Editor,
                        NormalizedName = UserRoleConstants.Editor.ToUpper()
                    },
                    new()
                    {
                        Id = Guid.NewGuid(),
                        Name = UserRoleConstants.Corrector,
                        NormalizedName = UserRoleConstants.Corrector.ToUpper()
                    }
                });
        }
    }
}
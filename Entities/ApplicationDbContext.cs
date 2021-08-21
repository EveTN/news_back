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
                        Id = Guid.Parse("8d691ee5-2ee9-4534-84c2-8391e9c503f8"),
                        ConcurrencyStamp = "9ee28384-8710-4902-aabd-38aa18400e7d",
                        Name = UserRoleConstants.Administrator,
                        NormalizedName = UserRoleConstants.Administrator.ToUpper()
                    },
                    new()
                    {
                        Id = Guid.Parse("aceb7ed9-4f66-4218-a63b-6593b3fe0b6b"),
                        ConcurrencyStamp = "fb6c54e9-7e77-47e6-b20e-e84648f510a4",
                        Name = UserRoleConstants.Correspondent,
                        NormalizedName = UserRoleConstants.Correspondent.ToUpper()
                    },
                    new()
                    {
                        Id = Guid.Parse("cf9b7fae-577e-4893-920f-9f008be81479"),
                        ConcurrencyStamp = "585a3412-a3f3-4a30-a815-7a318cf887f9",
                        Name = UserRoleConstants.MediaAnalyst,
                        NormalizedName = UserRoleConstants.MediaAnalyst.ToUpper()
                    },
                    new()
                    {
                        Id = Guid.Parse("50184fc9-0ede-49fd-80d8-7f77031293e9"),
                        ConcurrencyStamp = "2a768eae-9e9c-4559-87ca-94d59455f327",
                        Name = UserRoleConstants.Editor,
                        NormalizedName = UserRoleConstants.Editor.ToUpper()
                    },
                    new()
                    {
                        Id = Guid.Parse("d7461d24-0b4e-43ec-bcdd-f3ef35ce2422"),
                        ConcurrencyStamp = "7ae935c8-2349-45b4-93bd-e7ce7a3fe7f3",
                        Name = UserRoleConstants.Corrector,
                        NormalizedName = UserRoleConstants.Corrector.ToUpper()
                    }
                });
        }
    }
}
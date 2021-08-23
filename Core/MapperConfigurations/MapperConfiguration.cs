using System;
using Entities.Entities.Identity;
using Mapster;
using Models.Dtos.Administrator;
using Models.Dtos.Identity;

namespace Core.MapperConfigurations
{
    public static partial class MapperConfiguration
    {
        public static void Configure(TypeAdapterConfig config)
        {
            UserConfigure(config);
        }

        private static void UserConfigure(TypeAdapterConfig config)
        {
            config.ForType<RegisterDto, User>()
                // todo: пока нет подтверждение почты, будет true
                .Map(dest => dest.EmailConfirmed, src => true)
                .Map(dest => dest.IsChangedPassword, src => true)
                .Map(dest => dest.CreatedDt, src => DateTime.UtcNow);

            config.ForType<CreateUserDto, User>()
                // todo: пока нет подтверждение почты, будет true
                .Map(dest => dest.UserName, src => src.Email)
                .Map(dest => dest.EmailConfirmed, src => true)
                .Map(dest => dest.IsChangedPassword, src => true)
                .Map(dest => dest.CreatedDt, src => DateTime.UtcNow);

            config.ForType<UpdateUserDto, User>()
                .Map(dest => dest.LastName, src => src.LastName)
                .Map(dest => dest.FirstName, src => src.FirstName)
                .Map(dest => dest.MiddleName, src => src.MiddleName)
                .Map(dest => dest.Email, src => src.Email)
                .Map(dest => dest.PhoneNumber, src => src.PhoneNumber)
                .Map(dest => dest.UserName, src => src.Email)
                .Ignore(dest => dest.CreatedDt)
                .Ignore(dest => dest.IsChangedPassword);
        }
    }
}
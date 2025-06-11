using PhyGen.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Domain.Users.Errors
{
    public static class UserErrors
    {
        public static Error NotFound(Guid? userId) => Error.NotFound(
       "Users.NotFound",
       $"The user with the Id = '{userId}' was not found");

        public static readonly Error NotFoundByEmail = Error.NotFound(
            "Users.NotFoundByEmail",
            "The user with the specified email was not found");
        public static readonly Error NotFoundByUsername = Error.NotFound(
            "Users.NotFoundByUsername",
            "The user with the specified username was not found");
        public static readonly Error NotFoundByRole = Error.NotFound(
            "Users.NotFoundByRole",
            "The user with the specified role was not found");
        public static readonly Error EmailNotUnique = Error.Conflict(
            "Users.EmailNotUnique",
            "The provided email is not unique");

        public static readonly Error WrongPassword = Error.Conflict(
            "Users.WrongPassword",
            "The passsword for this account is wrong");
        public static readonly Error InActive = Error.Conflict(
            "Users.InActive",
            "The user is inactive");

        public static readonly Error InvalidRefreshToken = Error.Conflict(
            "Users.RefreshTokenInvalid",
            "The refresh token is invalid");

        public static readonly Error InvalidCurrentPassword = Error.Conflict(
            "Users.InvalidCurrentPassword",
            "The current password provided is incorrect.");
    }
}

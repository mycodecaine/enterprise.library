using Cdcn.Enterprise.Library.Domain.Primitives;

namespace Cdcn.Enterprise.Library.Infrastructure.Authentication
{
    public class AuthenticationErrors
    {

        public static Error InvalidUserNameOrPassword => new Error(
            "Authentication.InvalidUserNameOrPassword",
            "The specified username or password are incorrect.");

        public static Error InvalidResetPassword => new Error(
            "Authentication.InvalidResetPassword",
            "The specified username or password are incorrect.");

        public static Error InvalidAssignRole => new Error(
            "Authentication.InvalidAssignRole",
            "The specified username or rolename are incorrect.");

        public static Error PasswordNotIdentical => new Error(
            "Authentication.PasswordNotIdentical",
            "Password is incorrect");

        public static Error InvalidAdminToken => new Error(
            "Authentication.InvalidAdminToken",
            "The specified token is incorrect.");

        public static Error AssignRoleNotSuccessfull => new Error(
                "Authentication.AssignRoleNotSuccessfull",
                "Assigning Role is not successfull.");

        public static Error ResetPasswordError => new Error(
               "Authentication.ResetPasswordError",
               "Reset Password is not successfull.");

        public static Error SubIdNotExist => new Error(
            "Authentication.SubIdNotExist",
            "Token SubId is not valid");

    }

    
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace backendApp.Utilities
{
    public static class ResponseMessages
    {
        public const string AllFieldsRequired = "All fields (Username, password, email) are required";
        public const string InvalidEmailFormat = "Invalid email format";
        public const string EmailAlreadyInUse = "Email is already in use";
        public const string Unauthorized = "User not authenticated";
        public const string UserNotFound = "User not found";
        public const string UserCreatedSuccess = "User created successfully";
        public const string UserUpdatedSuccess = "User updated successfully";
        public const string UserDeletedSuccess = "User deleted successfully";
        public const string InternalServerError = "An error occurred while processing the request";
    }
}
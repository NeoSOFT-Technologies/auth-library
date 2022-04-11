using System;
using System.Collections.Generic;

namespace AuthLibrary.Models.Response
{
    public class UserPermissionsResponse
    {
        public bool IsAuthorized { get; set; }
        public string Message { get; set; }
        public List<UserPermission> Permissions { get; set; }
    }

    public class UserPermission
    {
        public List<string> Scopes { get; set; }
        public Guid RsId { get; set; }
        public string RsName { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirsoftMatchMaker.Core.Models.RoleRequests
{
    public class RoleRequestRouteModel
    {
        public string UserId { get; set; } = null!;
        public string RoleName { get; set; } = null!;
        public string RoleRequestId { get; set; } = null!;
    }
}

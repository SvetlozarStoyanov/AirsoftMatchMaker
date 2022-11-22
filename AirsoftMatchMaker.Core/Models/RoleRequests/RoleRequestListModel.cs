namespace AirsoftMatchMaker.Core.Models.RoleRequests
{
    public class RoleRequestListModel
    {
        public int Id { get; set; }
        public string RoleId { get; set; } = null!;
        public string RoleName { get; set; } = null!;
        public string UserId { get; set; } = null!;
        public string UserName { get; set; } = null!;
    }
}

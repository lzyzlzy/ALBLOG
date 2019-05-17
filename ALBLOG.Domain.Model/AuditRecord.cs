namespace ALBLOG.Domain.Model
{
    public class AuditRecord : DomainModel
    {
        public string SessionId { get; set; }
        public string AccessTime { get; set; }
        public string RemoteAddress { get; set; }
        public string Username { get; set; }
        public string AccessUrl { get; set; }
        public string ControllerName { get; set; }
        public string ActionName { get; set; }
        public string Data { get; set; }
        public PlatformType Platform { get; set; }
    }
}
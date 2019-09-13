using ALBLOG.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ALBLOG.Domain.Service.Interface
{
    public interface ILogService
    {
        Task Log(string sessionId, string controllerName, string actionName, string IPAddress, string content, bool isAdmin = false);

        Task Error(string sessionId, string controllerName, string actionName, string IPAddress, string content, bool isAdmin = false);

        Task Exception(string sessionId, string controllerName, string actionName, string IPAddress, string content, bool isAdmin = false);

        Task Assert(string sessionId, string controllerName, string actionName, string IPAddress, string content, bool isAdmin = false);

        Task Warning(string sessionId, string controllerName, string actionName, string IPAddress, string content, bool isAdmin = false);

        Task<Log> GetOne(Expression<Func<Log, bool>> expression);

        Task<IEnumerable<Log>> GetAll(Expression<Func<Log, bool>> expression);
    }
}

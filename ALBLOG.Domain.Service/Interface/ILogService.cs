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

        Task<Log> GetOneAsync(Expression<Func<Log, bool>> expression);

        Task<IEnumerable<Log>> GetAllAsync(Expression<Func<Log, bool>> expression);

        Task<LogPage> GetPageAsync(int pageSize, int pageIndex);

        Task<LogPage> GetPageAsync(Expression<Func<Log, bool>> filter, int pageSize, int pageIndex);

        Task<int> GetPageCountAsync(int pageSize);

        Task<int> GetPageCountAsync(Expression<Func<Log, bool>> filter, int pageSize);

        Task<int> GetPageViewNum(DateTime date);

        Task<int> GetPageViewNum(Expression<Func<Log, bool>> filter);

        Task<int> GetIpCount(DateTime date);
    }
}

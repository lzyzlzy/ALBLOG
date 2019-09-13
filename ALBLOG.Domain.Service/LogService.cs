using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using ALBLOG.Domain.Model;
using ALBLOG.Domain.Repository;
using ALBLOG.Domain.Service.Interface;

namespace ALBLOG.Domain.Service
{
    public class LogService : ILogService
    {

        private readonly LogRepository _repository;

        public LogService()
        {
            this._repository = new LogRepository();
        }

        public async Task Assert(string sessionId, string controllerName, string actionName, string IPAddress, string content, bool isAdmin = false)
        {
            await Write(LogType.Assert, sessionId, controllerName, actionName, IPAddress, content, isAdmin);
        }

        public async Task Error(string sessionId, string controllerName, string actionName, string IPAddress, string content, bool isAdmin = false)
        {
            await Write(LogType.Error, sessionId, controllerName, actionName, IPAddress, content, isAdmin);
        }

        public async Task Exception(string sessionId, string controllerName, string actionName, string IPAddress, string content, bool isAdmin = false)
        {
            await Write(LogType.Exception, sessionId, controllerName, actionName, IPAddress, content, isAdmin);
        }

        public async Task Log(string sessionId, string controllerName, string actionName, string IPAddress, string content, bool isAdmin = false)
        {
            await Write(LogType.Log, sessionId, controllerName, actionName, IPAddress, content, isAdmin);
        }

        public async Task Warning(string sessionId, string controllerName, string actionName, string IPAddress, string content, bool isAdmin = false)
        {
            await Write(LogType.Warning, sessionId, controllerName, actionName, IPAddress, content, isAdmin);
        }

        public async Task<IEnumerable<Log>> GetAll(Expression<Func<Log, bool>> expression)
        {
            return await _repository.GetAllAsync(expression);
        }

        public async Task<Log> GetOne(Expression<Func<Log, bool>> expression)
        {
            return await _repository.GetOneAsync(expression);
        }

        private async Task Write(LogType type, string sessionId, string controllerName, string actionName, string IPAddress, string content, bool isAdmin = false)
        {
            var log = new Log
            {
                Type = type,
                SessionId = sessionId,
                ControllerName = controllerName,
                ActionName = actionName,
                IPAddress = IPAddress,
                Content = content,
                IsAdmin = isAdmin
            };
            await _repository.AddAsync(log);
        }
    }
}

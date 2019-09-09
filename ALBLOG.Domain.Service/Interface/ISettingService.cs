using ALBLOG.Domain.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ALBLOG.Domain.Service.Interface
{
    public interface ISettingService
    {
        Task<Setting> GetSettingAsync();

        Task<string> GetAboutAsync();

        Task<string> GetProfileAsync();

        Task<(string ShowPath,string FullPath)> GetProfileImgPathAsync();

        Task<string> GetCVAsync();

        Task ChangeSettingAsync(Setting setting);

        Task ChangeAboutAsync(string content);

        Task ChangeCVAsync(string content);

        Task ChangeProfileAsync(string content);

        Task ChangeProfileImgPathAsync(string content);
    }
}

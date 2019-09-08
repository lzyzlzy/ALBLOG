using ALBLOG.Domain.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ALBLOG.Domain.Service.Interface
{
    interface ISettingService
    {
        Task<Setting> GetSettingAsync();

        Task<string> GetAboutAsync();

        Task<string> GetProfileAsync();

        Task<string> GetProfileImgPathAsync();

        Task<string> GetCVAsync();

        Task<bool> ChangeSettingAsync();

        Task<bool> ChangeAboutAsync();

        Task<bool> ChangeCVAsync();

        Task<bool> ChangeProfileAsync();

        Task<bool> ChangeProfileImgPathAsync();
    }
}

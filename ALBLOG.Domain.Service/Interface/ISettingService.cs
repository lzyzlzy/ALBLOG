using ALBLOG.Domain.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ALBLOG.Domain.Service.Interface
{
    interface ISettingService
    {
        Task<Setting> GetSetting();

        Task<string> GetAbout();

        Task<string> GetProfile();

        Task<string> GetProfileImgPath();

        Task<string> GetCV();

        Task<bool> ChangeSetting();

        Task<bool> ChangeAbout();

        Task<bool> ChangeCV();

        Task<bool> ChangeProfile();

        Task<bool> ChangeProfileImgPath();
    }
}

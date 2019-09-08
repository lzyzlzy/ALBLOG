using ALBLOG.Domain.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace ALBLOG.Domain.Service.Interface
{
    interface ISettingService
    {
        Setting GetSetting();

        string GetAbout();

        string GetProfile();

        string GetProfileImgPath();

        string GetCV();

        bool ChangeSetting();

        bool ChangeAbout();

        bool ChangeCV();

        bool ChangeProfile();

        bool ChangeProfileImgPath();
    }
}

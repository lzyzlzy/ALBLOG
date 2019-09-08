
using ALBLOG.Domain.Model;
using ALBLOG.Domain.Repository;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ALBLOG.Domain.Service
{
    public class SettingService
    {
        protected SettingRepository repository;

        public SettingService()
        {
            this.repository = new SettingRepository();
            if (repository.GetAll().Count() == 0)
            {
                repository.AddAsync(new Setting() { CV = "", Profile = "", ProfilePhotoPath = "", About = "" });
            }
        }

        public void ChangeCV(string context) => ChangeSettings(SettingType.CV, context);

        public void ChangeAbout(string context) => ChangeSettings(SettingType.About, context);

        public void ChangeProfile(string context) => ChangeSettings(SettingType.Profile, context);

        public void ChangeSettings(SettingType type, string context)
        {
            var introduction = repository.GetAll().Single();
            switch (type)
            {
                case SettingType.Profile:
                    introduction.Profile = context;
                    break;
                case SettingType.CV:
                    introduction.CV = context;
                    break;
                case SettingType.About:
                    introduction.About = context;
                    break;
                default:
                    break;
            }
            repository.UpdateAsync(introduction);
        }

        public string GetCV() => GetSettings().CV;

        public string GetAbout() => GetSettings().About;

        public string GetProfile() => GetSettings().Profile;

        public void ChangeProfilePhoto(string photoPath)
        {
            var entity = repository.GetAll().SingleOrDefault();
            entity.ProfilePhotoPath = photoPath;
            repository.UpdateAsync(entity);
        }

        public (string ShowPath, string FullPath) GetProfilePhotoPath()
        {
            var fullPath = repository.GetAll().SingleOrDefault().ProfilePhotoPath;
            return (fullPath.GetShowPath(), fullPath);
        }

        private Setting GetSettings() => repository.GetAll().Single();
    }
}
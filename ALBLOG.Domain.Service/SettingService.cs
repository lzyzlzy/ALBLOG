
using ALBLOG.Domain.Model;
using ALBLOG.Domain.Repository;
using ALBLOG.Domain.Service.Interface;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ALBLOG.Domain.Service
{
    public class SettingService : ISettingService
    {
        protected SettingRepository _repository;

        public SettingService()
        {
            this._repository = new SettingRepository();
            if (_repository.GetCount() == 0)
            {
                _repository.Add(new Setting() { CV = "", Profile = "", ProfilePhotoPath = "", About = "" });
            }
        }

        public async Task<Setting> GetSettingAsync()
        {
            return (await _repository.GetAllAsync()).FirstOrDefault();
        }

        public async Task<string> GetAboutAsync()
        {
            return (await GetSettingAsync()).About;
        }

        public async Task<string> GetProfileAsync()
        {
            return (await GetSettingAsync()).Profile;
        }

        public async Task<(string ShowPath, string FullPath)> GetProfileImgPathAsync()
        {
            var path = (await GetSettingAsync()).ProfilePhotoPath;
            return (path.GetShowPath(), path);
        }

        public async Task<string> GetCVAsync()
        {
            return (await GetSettingAsync()).CV;
        }

        public async Task ChangeSettingAsync(Setting setting)
        {
            await _repository.GetOneAndUpdateAsync(i => true, j =>
             {
                 j = setting;
                 return j;
             });
        }

        public async Task ChangeAboutAsync(string content)
        {
            await _repository.GetOneAndUpdateAsync(i => true, j =>
            {
                j.About = content;
                return j;
            });
        }

        public async Task ChangeCVAsync(string content)
        {
            await _repository.GetOneAndUpdateAsync(i => true, j =>
            {
                j.CV = content;
                return j;
            });
        }

        public async Task ChangeProfileAsync(string content)
        {
            await _repository.GetOneAndUpdateAsync(i => true, j =>
            {
                j.Profile = content;
                return j;
            });
        }

        public async Task ChangeProfileImgPathAsync(string content)
        {
            await _repository.GetOneAndUpdateAsync(i => true, j =>
            {
                j.ProfilePhotoPath = content;
                return j;
            });
        }
    }
}
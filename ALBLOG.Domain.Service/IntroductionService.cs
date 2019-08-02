
using ALBLOG.Domain.Model;
using ALBLOG.Domain.Repository;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ALBLOG.Domain.Service
{
    public class IntroductionService
    {
        protected IntroductionRepository repository;

        public IntroductionService()
        {
            this.repository = new IntroductionRepository();
            if (repository.GetAll().Count() == 0)
            {
                repository.AddAsync(new Introduction());
            }
        }

        public void ChangeCV(string context) => ChangeIntroduction(IntroductionType.CV, context);

        public void ChangeAbout(string context) => ChangeIntroduction(IntroductionType.About, context);

        public void ChangeProfile(string context) => ChangeIntroduction(IntroductionType.Profile, context);

        public void ChangeIntroduction(IntroductionType type, string context)
        {
            var introduction = repository.GetAll().Single();
            switch (type)
            {
                case IntroductionType.Profile:
                    introduction.Profile = context;
                    break;
                case IntroductionType.CV:
                    introduction.CV = context;
                    break;
                case IntroductionType.About:
                    introduction.About = context;
                    break;
                default:
                    break;
            }
            repository.UpdateAsync(introduction);
        }

        public string GetCV() => GetIntroduction().CV;

        public string GetAbout() => GetIntroduction().About;

        public string GetProfile() => GetIntroduction().Profile;

        public void ChangeProfilePhoto(string photoPath)
        {
            var entity = repository.GetAll().SingleOrDefault();
            entity.ProfilePhotoPath = photoPath;
            repository.UpdateAsync(entity);
        }

        public (string ShowPath, string FullPath) GetProfilePhotoPath()
        {
            var fullPath = repository.GetAll().SingleOrDefault().ProfilePhotoPath ?? "";
            return (fullPath.GetShowPath(), fullPath);
        }

        private Introduction GetIntroduction() => repository.GetAll().Single();
    }
}
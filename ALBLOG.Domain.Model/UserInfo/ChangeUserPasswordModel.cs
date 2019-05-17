using System;
using System.Collections.Generic;
using System.Text;

namespace ALBLOG.Domain.Model
{
    public class ChangeUserPasswordModel
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string NewPassword { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace ALBLOG.Domain.Dto
{
   public class UpLoadImgDto
    {
        public int Errno { get; set; }

        public List<string> Data { get; set; }
    }
}

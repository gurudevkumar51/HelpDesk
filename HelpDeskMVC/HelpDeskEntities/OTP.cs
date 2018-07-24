using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelpDeskEntities
{
   public class OTP
    {
        public OTP()
        {

        }
        public OTP(DateTime CreateDateTime)
        {
            this.CreateDateTime = CreateDateTime;
            this.ExpireDateTime = CreateDateTime.AddMinutes(5);
        }
        public string OTPvalue { get; set; }
        public DateTime CreateDateTime { get; set; }
        public DateTime ExpireDateTime { get; set; }
    }
}

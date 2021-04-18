using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Jobsity.Models
{
    public class BotResponse
    {
        public bool Success { get; set; }
        public string Content { get; set; }

        public BotResponse(bool s, string c)
        {
            Success = s;
            Content = c;
        }
    }
}
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace AspConsole.Models
{
   public class Settings
    {
        private readonly IConfiguration configuration;

        public Settings(IConfiguration configuration)
        {
            this.configuration = configuration;
        }
        public int WorkersCount => configuration.GetValue<int>("WorkersCount");
        public int RunInterval => configuration.GetValue<int>("RunInterval");
        public int InstanceName => configuration.GetValue<int>("name");
        public int ResultPath => configuration.GetValue<int>("ResultPath");
    }
}

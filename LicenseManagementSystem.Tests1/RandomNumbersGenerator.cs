using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LicenseManagementSystem.Tests
{
    public class RandomNumbersGenerator
    {
        public string GetNumber()
        {
            return Guid.NewGuid().ToString().Substring(0, 8);
        }
    }
}

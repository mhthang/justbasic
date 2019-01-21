using JustBasic.Data;
using JustBasic.Model.Models;
using JustBasic.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test
{
    class Program
    {
        
        static void Main(string[] args)
        {
            var context = new JustBasicDbContext();
            var list = context.Tags.ToArray();
        }
    }
}

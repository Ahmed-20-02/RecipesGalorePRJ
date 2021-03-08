using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RecipesGalorePRJ.Models
{
    public class Connection
    {
        private string ConnectionString = "";

        public string ReturnConnectionString()
        {
            return ConnectionString;
        }
    }
}

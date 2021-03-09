using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RecipesGalorePRJ.Models
{
    public class RecipeModel
    {
        public int RecipeID { get; set; }
        public string Name { get; set; }
        public string CuisineType { get; set; }
        public string CookingTime { get; set; }
        public string Ingredients { get; set; }
        public string Method { get; set; }
        public string File { get; set; }
        public UserModel UserID { get; set; }
    }
}

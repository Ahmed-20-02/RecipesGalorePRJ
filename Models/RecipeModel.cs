using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RecipesGalorePRJ.Models
{
    public class RecipeModel
    {
        public int RecipeId { get; set; }
        public string RecipeName { get; set; }
        public string RecipeCuisineType { get; set; }
        public string RecipeCookingTime { get; set; }
        public string RecipeIngredients { get; set; }
        public string RecipeMethod { get; set; }
        public string File { get; set; }
        public UserModel Creator { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RecipesGalorePRJ.Models
{
    public class Recipe
    {
        
        public int RecipeId { get; set; }

        [Display(Name = "Recipe Name")]
        public string RecipeName { get; set; }

        [Display(Name = "Recipe Ingredients")]
        public string RecipeCuisineType { get; set; }

        [Display(Name = "Recipe Cook Time")]
        public int RecipeCookingTime { get; set; }

        [Display(Name = "Recipe Ingredients")]
        public string RecipeIngredients { get; set; }

        [Display(Name = "Recipe Method")]
        public string RecipeMethod { get; set; }

        public string File { get; set; }

        public User Creator { get; set; }
    }
}

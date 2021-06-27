using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace MealPlan.Models
{
    [Table("meals")]
    public class Meal
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public int Order { get; set; }
        [NotNull]
        public string Name { get; set; }
        public string Author { get; set; }
        public string Description { get; set; }
        public bool Favortie { get; set; }
        public string RecipeImage { get; set; }
        public string Recipe { get; set; }
    }
}

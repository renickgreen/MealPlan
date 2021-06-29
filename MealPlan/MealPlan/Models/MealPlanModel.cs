using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace MealPlan.Models
{
    [Table("plans")]
    public class MealPlanModel : Meal
    {
        [NotNull]
        public string Meals { get; set; }

        internal void ExtractMealsAsync()
        {
            throw new NotImplementedException();
        }
    }
}

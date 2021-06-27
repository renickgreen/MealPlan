using System;
using System.Collections.Generic;
using System.Text;

namespace MealPlan.Models
{
    public class MealPlan : Meal
    {
        public List<Meal> Meals { get; set; }
    }
}

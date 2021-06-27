using MvvmHelpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace MealPlan.ViewModels
{
    public class ViewModelBase : BaseViewModel
    {
        public int Id { get; set; }
        private string _name;
        public string Name { get => _name; set => SetProperty(ref _name, value); }
        private string _author;
        public string Author { get => _author; set => SetProperty(ref _author, value); }
        private string _description;
        public string Description { get => _description; set => SetProperty(ref _description, value); }
        private bool _favortie;
        public bool Favortie { get => _favortie; set => SetProperty(ref _favortie, value); }
    }
}

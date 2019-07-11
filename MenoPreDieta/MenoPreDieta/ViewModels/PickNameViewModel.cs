﻿using System.Threading.Tasks;
using MenoPreDieta.Models;

namespace MenoPreDieta.ViewModels
{
    public abstract class PickNameViewModel : ViewModel
    {
        private NameModel first;
        private NameModel second;
        private int namesCount;

        public NameModel First
        {
            get => first;
            set
            {
                if (Equals(value, first)) return;
                first = value;
                OnPropertyChanged();
            }
        }

        public NameModel Second
        {
            get => second;
            set
            {
                if (Equals(value, second)) return;
                second = value;
                OnPropertyChanged();
            }
        }

        public int NamesCount
        {
            get => namesCount;
            set
            {
                if (value == namesCount) return;
                namesCount = value;
                OnPropertyChanged();
            }
        }

        public abstract Task LoadAsync();
    }
}
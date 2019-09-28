using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Forms.ViewModels
{
    public class ListEntryPageViewModel : BaseViewModel
    {
        private ObservableCollection<string> _source;
        public ObservableCollection<string> Source
        {
            get => _source;
            set => SetProperty(ref _source, value);
        }

        public ListEntryPageViewModel() : base()
        {
            Source = new ObservableCollection<string>();
            for (int i = 0; i < 10; i++)
            {
                Source.Add($"item {i}");
            }
        }
    }
}

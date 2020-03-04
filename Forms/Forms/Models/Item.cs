using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Forms.Models
{
    public class Item : INotifyPropertyChanged
    {
        public string Id { get; set; }
        public string Text { get; set; }
        public string Description { get; set; }

        private bool _selected = false;
        public bool Selected
        {
            get => _selected;
            set
            {
                _selected = value;
                OnPropertyChanged(nameof(Selected));
            }
        }

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            var changed = PropertyChanged;
            if (changed == null)
                return;

            changed.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
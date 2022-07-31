using System;
using System.ComponentModel;

namespace VisualFindReferences.Core.Graph.Model
{
    public class ModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        public virtual void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public Guid Guid { get; private set; }

        public ModelBase()
        {
            Guid = Guid.NewGuid();
        }
    }
}
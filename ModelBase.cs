using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace PP.Data.Models
{
    public abstract class ModelBase<T> : INotifyPropertyChanged, IDataErrorInfo
    {
        public bool IsDeleted { get; set; }
        
        private long _id;

        /// <summary>
        /// RowId
        /// </summary>
        [PrimaryKey]
        public long Id
        {
            get { return _id; }
            set
            {
                if(value == _id)
                    return;
                _id = value;
            }
        }
        
        #region IDataErrorInfo
        

        [Ignore]
        public bool IsValid => GetIsValid();

        protected virtual bool GetIsValid()
        {
            return true;
        }

        protected virtual string GetErrorInfo(string prop)
        {
            return null;
        }

        string IDataErrorInfo.this[string columnName] => GetErrorInfo(columnName);

        string IDataErrorInfo.Error => null;

        #endregion

#region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        public bool NotifyPropertyChanged { get; set; } = true;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            if (!NotifyPropertyChanged) return;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
#endregion

    }

    public class NonClonable : Attribute
    {
        
    }
}

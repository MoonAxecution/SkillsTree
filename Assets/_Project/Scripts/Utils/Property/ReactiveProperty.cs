using System;

namespace Game
{

    public interface IReactiveProperty<TType>
    {
        TType Value { get; }
        event Action<TType> Changed;
    }
    
    public class ReactiveProperty<TType> : IReactiveProperty<TType>
    {
        private TType _value;
        
        public event Action<TType> Changed;
        
        public TType Value
        {
            get { return _value; }
            
            set
            {
                SetValue(value);
            }
            
        }
        
        public ReactiveProperty() : this(default) {}

        public ReactiveProperty(TType originValue)
        {
            SetValue(originValue);
        }

        public TType GetValue() => _value;

        private void SetValue(TType value)
        {
            this._value = value;
            Changed.Fire(this._value);
        }
    }
}
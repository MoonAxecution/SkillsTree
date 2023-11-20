using System;
using TMPro;
using UnityEngine;

namespace Game.UI
{
    public class ValueView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _skillPointsLabel;

        private Action<int> _updateValueDelegate;
        private string _prefix;
        private IReactiveProperty<int> _observableValue;

        public void Init(string prefix, IReactiveProperty<int> observableValue)
        {
            _updateValueDelegate = UpdateValue;
            
            _prefix = prefix;
            _observableValue = observableValue;
            _observableValue.Changed += _updateValueDelegate;

            UpdateValue(_observableValue.Value);
        }
        
        private void UpdateValue(int value)
        {
            _skillPointsLabel.text = $"{_prefix}{value.ToString()}";
        }

        private void OnDestroy()
        {
            _observableValue.Changed -= _updateValueDelegate;
        }
    }
}
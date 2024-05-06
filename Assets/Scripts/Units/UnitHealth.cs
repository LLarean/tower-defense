using Globals;
using Towers;

namespace Units
{
    public class UnitHealth
    {
        private int _currentValue;
        private ElementalType _resistType;
        
        public int CurrentValue => _currentValue;

        public UnitHealth(int maximumHealth, ElementalType resistType)
        {
            _currentValue = maximumHealth;
            _resistType = resistType;
        }
        
        public void TakeDamage(int damage, ElementalType attackType)
        {
            var calculatedDamage = GetCalculatedDamage(damage, attackType);
            
            if (calculatedDamage < 0)
            {
                calculatedDamage = 0;
            }
            
            var calculatedHealth = _currentValue - calculatedDamage;
            SetValue(calculatedHealth);
        }

        private int GetCalculatedDamage(int damage, ElementalType attackType)
        {
            if (_resistType == ElementalType.None)
            {
                return damage;
            }
            
            if (_resistType == attackType)
            {
                damage -= GlobalParams.DamageReduction;
            }
            
            return damage;
        }

        private void SetValue(int value)
        {
            _currentValue = value;
            
            if (_currentValue < 0)
            {
                _currentValue = 0;
            }
        }
    }
}
using Builds;

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
            damage = GetCalculatedDamage(damage, attackType);
            _currentValue -= damage;

            if (_currentValue < 0)
            {
                _currentValue = 0;
            }
        }

        private int GetCalculatedDamage(int damage, ElementalType attackType)
        {
            if (damage < 0)
            {
                damage = 0;
            }
            
            if (_resistType != ElementalType.None && _resistType == attackType)
            {
                damage -= GlobalParams.DamageReduction;
            }
            
            return damage;
        }
    }
}
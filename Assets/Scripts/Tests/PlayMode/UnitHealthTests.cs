using Globals;
using NUnit.Framework;
using Towers;
using Units;

namespace Tests
{
    public class UnitHealthTests
    {
        [Test]
        public void TakeDamage_TakeDamageWithoutResistance_HealthReducedDamageValue()
        {
            var healthValue = 100;
            var damageValue = 100;
            var newHealthValue = healthValue - damageValue;
            
            var unitHealth = new UnitHealth(healthValue, ElementalType.None);
            unitHealth.TakeDamage(damageValue, ElementalType.None);
            
            Assert.IsTrue(unitHealth.CurrentValue == newHealthValue);
        }
        
        [Test]
        public void TakeDamage_TakeDamageWithResistance_HealthReducedDamageValueAfterResistance()
        {
            var healthValue = 100;
            var damageValue = 100;
            var reducedDamage = damageValue - GlobalParams.DamageReduction;
            var unitHealth = new UnitHealth(healthValue, ElementalType.Fire);
            
            unitHealth.TakeDamage(damageValue, ElementalType.Fire);
            var newHealthValue = healthValue - reducedDamage;
            
            Assert.IsTrue(unitHealth.CurrentValue == newHealthValue);
        }

        [Test]
        public void TakeDamage_TakeDamageGreaterThanCurrentHealth_HealthIsZero()
        {
            var healthValue = 10;
            var damageValue = 100;
            var unitHealth = new UnitHealth(healthValue, ElementalType.None);
            
            unitHealth.TakeDamage(damageValue, ElementalType.None);
            
            Assert.IsTrue(unitHealth.CurrentValue == 0);
        }

        [Test]
        public void TakeDamage_TakeNegativeDamageValue_HealthNotChange()
        {
            var healthValue = 100;
            var damageValue = -100;
            var unitHealth = new UnitHealth(healthValue, ElementalType.None);
           
            unitHealth.TakeDamage(damageValue, ElementalType.None);
            
            Assert.IsTrue(unitHealth.CurrentValue == healthValue);
        }
        
        [Test]
        public void TakeDamage_TakeLessDamageThanDamageReduction_HealthNotChange()
        {
            var healthValue = 10;
            var damageValue = 1;
            var unitHealth = new UnitHealth(healthValue, ElementalType.Fire);
           
            unitHealth.TakeDamage(damageValue, ElementalType.Fire);
            
            Assert.IsTrue(unitHealth.CurrentValue == healthValue);
        }
    }
}
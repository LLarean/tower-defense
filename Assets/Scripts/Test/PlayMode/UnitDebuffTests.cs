using Builds;
using NUnit.Framework;
using Units;

namespace Test
{
    public class UnitDebuffTests
    {
        [Test]
        public void TakeFireEffect()
        {
            var unitEffects = new UnitEffects();
            var effect = ElementalType.Fire;
            unitEffects.TakeEffect(effect);
        
            Assert.IsTrue(unitEffects.ActiveEffects.Count == 1);
            Assert.IsTrue(unitEffects.DebuffModels.Count == 1);
            Assert.IsTrue(unitEffects.DebuffModels[0].DebuffType == DebuffType.Burning);
        }
    
        [Test]
        public void TakePoisonEffect()
        {
            var unitEffects = new UnitEffects();
            var effect = ElementalType.Poison;
            unitEffects.TakeEffect(effect);
        
            Assert.IsTrue(unitEffects.ActiveEffects.Count == 1);
            Assert.IsTrue(unitEffects.DebuffModels.Count == 1);
            Assert.IsTrue(unitEffects.DebuffModels[0].DebuffType == DebuffType.Intoxication);
        }
    
        [Test]
        public void TakeWaterEffect()
        {
            var unitEffects = new UnitEffects();
        
            var effect = ElementalType.Water;
            unitEffects.TakeEffect(effect);
        
            Assert.IsTrue(unitEffects.ActiveEffects.Count == 1);
            Assert.IsTrue(unitEffects.DebuffModels.Count == 1);
            Assert.IsTrue(unitEffects.DebuffModels[0].DebuffType == DebuffType.Wet);
        }
    
        [Test]
        public void TakeIceEffect()
        {
            var unitEffects = new UnitEffects();
        
            var effect = ElementalType.Ice;
            unitEffects.TakeEffect(effect);
        
            Assert.IsTrue(unitEffects.ActiveEffects.Count == 1);
            Assert.IsTrue(unitEffects.DebuffModels.Count == 1);
            Assert.IsTrue(unitEffects.DebuffModels[0].DebuffType == DebuffType.Slow);
        }
    
        [Test]
        public void TakeFirePoisonEffects()
        {
            var unitEffects = new UnitEffects();
        
            var effect = ElementalType.Fire;
            unitEffects.TakeEffect(effect);
        
            effect = ElementalType.Poison;
            unitEffects.TakeEffect(effect);
        
            Assert.IsTrue(unitEffects.ActiveEffects.Count == 2);
            Assert.IsTrue(unitEffects.DebuffModels.Count == 2);
            Assert.IsTrue(unitEffects.DebuffModels[0].DebuffType == DebuffType.Burning);
            Assert.IsTrue(unitEffects.DebuffModels[1].DebuffType == DebuffType.Intoxication);
        }
        
        [Test]
        public void TakeFireWaterEffects()
        {
            var unitEffects = new UnitEffects();
        
            var effect = ElementalType.Fire;
            unitEffects.TakeEffect(effect);
        
            effect = ElementalType.Water;
            unitEffects.TakeEffect(effect);
        
            Assert.IsTrue(unitEffects.ActiveEffects.Count == 0);
            Assert.IsTrue(unitEffects.DebuffModels.Count == 0);
        }
        
        [Test]
        public void TakeFireIceEffects()
        {
            var unitEffects = new UnitEffects();
        
            var effect = ElementalType.Fire;
            unitEffects.TakeEffect(effect);
        
            effect = ElementalType.Ice;
            unitEffects.TakeEffect(effect);
        
            Assert.IsTrue(unitEffects.ActiveEffects.Count == 0);
            Assert.IsTrue(unitEffects.DebuffModels.Count == 0);
        }
        
        [Test]
        public void TakeFireFireFireEffects()
        {
            var unitEffects = new UnitEffects();
        
            var effect = ElementalType.Fire;
            unitEffects.TakeEffect(effect);
            unitEffects.TakeEffect(effect);
            unitEffects.TakeEffect(effect);
        
            Assert.IsTrue(unitEffects.ActiveEffects.Count == 1);
            Assert.IsTrue(unitEffects.DebuffModels.Count == 1);
            Assert.IsTrue(unitEffects.DebuffModels[0].DebuffType == DebuffType.Burning);
        }
        
        [Test]
        public void TakePoisonPoisonPoisonEffects()
        {
            var unitEffects = new UnitEffects();
        
            var effect = ElementalType.Poison;
            unitEffects.TakeEffect(effect);
            unitEffects.TakeEffect(effect);
            unitEffects.TakeEffect(effect);
        
            Assert.IsTrue(unitEffects.ActiveEffects.Count == 1);
            Assert.IsTrue(unitEffects.DebuffModels.Count == 1);
            Assert.IsTrue(unitEffects.DebuffModels[0].DebuffType == DebuffType.Intoxication);
        }
        
        [Test]
        public void TakeWaterWaterWaterEffects()
        {
            var unitEffects = new UnitEffects();
        
            var effect = ElementalType.Water;
            unitEffects.TakeEffect(effect);
            unitEffects.TakeEffect(effect);
            unitEffects.TakeEffect(effect);
        
            Assert.IsTrue(unitEffects.ActiveEffects.Count == 1);
            Assert.IsTrue(unitEffects.DebuffModels.Count == 1);
            Assert.IsTrue(unitEffects.DebuffModels[0].DebuffType == DebuffType.Wet);
        }
        
        [Test]
        public void TakeIceIceIceEffects()
        {
            var unitEffects = new UnitEffects();
        
            var effect = ElementalType.Ice;
            unitEffects.TakeEffect(effect);
            unitEffects.TakeEffect(effect);
            unitEffects.TakeEffect(effect);
        
            Assert.IsTrue(unitEffects.ActiveEffects.Count == 1);
            Assert.IsTrue(unitEffects.DebuffModels.Count == 1);
            Assert.IsTrue(unitEffects.DebuffModels[0].DebuffType == DebuffType.Slow);
        }
        
        [Test]
        public void TakeFireIceFireEffects()
        {
            var unitEffects = new UnitEffects();
        
            var effect = ElementalType.Fire;
            unitEffects.TakeEffect(effect);
            effect = ElementalType.Ice;
            unitEffects.TakeEffect(effect);
            effect = ElementalType.Fire;
            unitEffects.TakeEffect(effect);
        
            Assert.IsTrue(unitEffects.ActiveEffects.Count == 1);
            Assert.IsTrue(unitEffects.DebuffModels.Count == 1);
            Assert.IsTrue(unitEffects.DebuffModels[0].DebuffType == DebuffType.Burning);
        }
        
        [Test]
        public void TakeFirePoisonFirePoisonEffects()
        {
            var unitEffects = new UnitEffects();
        
            var effect = ElementalType.Fire;
            unitEffects.TakeEffect(effect);
            effect = ElementalType.Poison;
            unitEffects.TakeEffect(effect);
            effect = ElementalType.Fire;
            unitEffects.TakeEffect(effect);
            effect = ElementalType.Poison;
            unitEffects.TakeEffect(effect);
        
            Assert.IsTrue(unitEffects.ActiveEffects.Count == 2);
            Assert.IsTrue(unitEffects.DebuffModels.Count == 2);
            Assert.IsTrue(unitEffects.DebuffModels[0].DebuffType == DebuffType.Burning);
            Assert.IsTrue(unitEffects.DebuffModels[1].DebuffType == DebuffType.Intoxication);
        }
        
        [Test]
        public void TakeIceFireIceEffects()
        {
            var unitEffects = new UnitEffects();
        
            var effect = ElementalType.Ice;
            unitEffects.TakeEffect(effect);
            effect = ElementalType.Fire;
            unitEffects.TakeEffect(effect);
            effect = ElementalType.Ice;
            unitEffects.TakeEffect(effect);
        
            Assert.IsTrue(unitEffects.ActiveEffects.Count == 1);
            Assert.IsTrue(unitEffects.DebuffModels.Count == 1);
            Assert.IsTrue(unitEffects.DebuffModels[0].DebuffType == DebuffType.Slow);
        }
        
        [Test]
        public void TakePoisonWaterEffects()
        {
            var unitEffects = new UnitEffects();
        
            var effect = ElementalType.Poison;
            unitEffects.TakeEffect(effect);
            effect = ElementalType.Water;
            unitEffects.TakeEffect(effect);
        
            Assert.IsTrue(unitEffects.ActiveEffects.Count == 0);
            Assert.IsTrue(unitEffects.DebuffModels.Count == 0);
        }
        
        [Test]
        public void TakeWaterPoisonEffects()
        {
            var unitEffects = new UnitEffects();
        
            var effect = ElementalType.Water;
            unitEffects.TakeEffect(effect);
            effect = ElementalType.Poison;
            unitEffects.TakeEffect(effect);
        
            Assert.IsTrue(unitEffects.ActiveEffects.Count == 0);
            Assert.IsTrue(unitEffects.DebuffModels.Count == 0);
        }
    }
}

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
        
        // [Test]
        // public void TakeCopyEffect()
        // {
        //     var effect = ElementalType.Fire;
        //     var unitEffects = new UnitEffects();
        //     unitEffects.TakeEffect(effect);
        //     unitEffects.TakeEffect(effect);
        //
        //     Assert.IsTrue(unitEffects.ActiveEffects.Count == 1);
        // }
        //
        //
        // [Test]
        // public void TakeWaterIceEffects()
        // {
        //     var unitEffects = new UnitEffects();
        //
        //     var effect = ElementalType.Water;
        //     unitEffects.TakeEffect(effect);
        //
        //     effect = ElementalType.Ice;
        //     unitEffects.TakeEffect(effect);
        //
        //     Assert.IsTrue(unitEffects.ActiveEffects.Count == 2);
        //     Assert.IsTrue(unitEffects.DebuffModels[0].DebuffType == DebuffType.Frozen);
        // }
        //
        // [Test]
        // public void TakeWaterIceFireEffects()
        // {
        //     var unitEffects = new UnitEffects();
        //
        //     var effect = ElementalType.Water;
        //     unitEffects.TakeEffect(effect);
        //
        //     effect = ElementalType.Ice;
        //     unitEffects.TakeEffect(effect);
        //
        //     effect = ElementalType.Fire;
        //     unitEffects.TakeEffect(effect);
        //
        //     Assert.IsTrue(unitEffects.DebuffModels.Count == 0);
        //     Assert.IsTrue(unitEffects.ActiveEffects.Count == 0);
        // }
        //
        // [Test]
        // public void TakeFireIceEffects()
        // {
        //     var unitEffects = new UnitEffects();
        //
        //     var effect = ElementalType.Fire;
        //     unitEffects.TakeEffect(effect);
        //
        //     effect = ElementalType.Ice;
        //     unitEffects.TakeEffect(effect);
        //
        //     Assert.IsTrue(unitEffects.DebuffModels.Count == 0);
        //     Assert.IsTrue(unitEffects.ActiveEffects.Count == 0);
        // }
        //
        // [Test]
        // public void TakeFireWaterEffects()
        // {
        //     var unitEffects = new UnitEffects();
        //
        //     var effect = ElementalType.Fire;
        //     unitEffects.TakeEffect(effect);
        //
        //     effect = ElementalType.Water;
        //     unitEffects.TakeEffect(effect);
        //
        //     Assert.IsTrue(unitEffects.DebuffModels.Count == 0);
        //     Assert.IsTrue(unitEffects.ActiveEffects.Count == 0);
        // }
    }
}

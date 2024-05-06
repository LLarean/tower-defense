using System.Collections.Generic;
using Globals;
using Towers;
using NUnit.Framework;
using Units;

namespace Tests
{
    public class ElementalEffectsTests
    {
        #region Adding a single effect. Without resistance to the elements
        [Test]
        public void TakeEffect_AddingFireEffect_PresenceBurningDebuff()
        {
            var unitEffects = new ElementalEffects(ElementalType.None);
            var effect = ElementalType.Fire;
            unitEffects.TakeEffect(effect);

            Assert.IsTrue(unitEffects.CurrentEffects.Count == 1);
            Assert.IsTrue(unitEffects.ActiveDebuffs.Count == 1);
            Assert.IsTrue(unitEffects.ActiveDebuffs[0].DebuffType == DebuffType.Burning);
        }

        [Test]
        public void TakeEffect_AddingPoisonEffect_PresenceIntoxicationDebuff()
        {
            var unitEffects = new ElementalEffects(ElementalType.None);
            var effect = ElementalType.Poison;
            unitEffects.TakeEffect(effect);

            Assert.IsTrue(unitEffects.CurrentEffects.Count == 1);
            Assert.IsTrue(unitEffects.ActiveDebuffs.Count == 1);
            Assert.IsTrue(unitEffects.ActiveDebuffs[0].DebuffType == DebuffType.Intoxication);
        }

        [Test]
        public void TakeEffect_AddingWaterEffect_PresenceWetDebuff()
        {
            var unitEffects = new ElementalEffects(ElementalType.None);

            var effect = ElementalType.Water;
            unitEffects.TakeEffect(effect);

            Assert.IsTrue(unitEffects.CurrentEffects.Count == 1);
            Assert.IsTrue(unitEffects.ActiveDebuffs.Count == 1);
            Assert.IsTrue(unitEffects.ActiveDebuffs[0].DebuffType == DebuffType.Wet);
        }

        [Test]
        public void TakeEffect_AddingIceEffect_PresenceSlowDebuff()
        {
            var unitEffects = new ElementalEffects(ElementalType.None);

            var effect = ElementalType.Ice;
            unitEffects.TakeEffect(effect);

            Assert.IsTrue(unitEffects.CurrentEffects.Count == 1);
            Assert.IsTrue(unitEffects.ActiveDebuffs.Count == 1);
            Assert.IsTrue(unitEffects.ActiveDebuffs[0].DebuffType == DebuffType.Slow);
        }
        #endregion
        
        #region Adding a single effect. With resistance to the elements
        [Test]
        public void TakeEffect_AddingFireEffectWithFireResistance_NoDebuffs()
        {
            var unitEffects = new ElementalEffects(ElementalType.Fire);
            var effect = ElementalType.Fire;
            unitEffects.TakeEffect(effect);

            Assert.IsTrue(unitEffects.CurrentEffects.Count == 0);
            Assert.IsTrue(unitEffects.ActiveDebuffs.Count == 0);
        }

        [Test]
        public void TakeEffect_AddingPoisonEffectWithPoisonResistance_NoDebuffs()
        {
            var unitEffects = new ElementalEffects(ElementalType.Poison);
            var effect = ElementalType.Poison;
            unitEffects.TakeEffect(effect);

            Assert.IsTrue(unitEffects.CurrentEffects.Count == 0);
            Assert.IsTrue(unitEffects.ActiveDebuffs.Count == 0);
        }

        [Test]
        public void TakeEffect_AddingWaterEffectWithWaterResistance_NoDebuffs()
        {
            var unitEffects = new ElementalEffects(ElementalType.Water);

            var effect = ElementalType.Water;
            unitEffects.TakeEffect(effect);

            Assert.IsTrue(unitEffects.CurrentEffects.Count == 0);
            Assert.IsTrue(unitEffects.ActiveDebuffs.Count == 0);
        }

        [Test]
        public void TakeEffect_AddingIceEffectWithIceResistance_NoDebuffs()
        {
            var unitEffects = new ElementalEffects(ElementalType.Ice);

            var effect = ElementalType.Ice;
            unitEffects.TakeEffect(effect);

            Assert.IsTrue(unitEffects.CurrentEffects.Count == 0);
            Assert.IsTrue(unitEffects.ActiveDebuffs.Count == 0);
        }
        #endregion

        #region Adding a fire effect and then others
        [Test]
        public void TakeEffect_AddingMultipleFireEffects_PresenceSingleBurningDebuff()
        {
            var unitEffects = new ElementalEffects(ElementalType.None);

            var effect = ElementalType.Fire;
            unitEffects.TakeEffect(effect);
            unitEffects.TakeEffect(effect);
            unitEffects.TakeEffect(effect);

            Assert.IsTrue(unitEffects.CurrentEffects.Count == 1);
            Assert.IsTrue(unitEffects.ActiveDebuffs.Count == 1);
            Assert.IsTrue(unitEffects.ActiveDebuffs[0].DebuffType == DebuffType.Burning);
        }

        [Test]
        public void TakeEffect_AddingFireAndPoisonEffect_PresenceBurningAndIntoxicationDebuffs()
        {
            var unitEffects = new ElementalEffects(ElementalType.None);

            var effect = ElementalType.Fire;
            unitEffects.TakeEffect(effect);

            effect = ElementalType.Poison;
            unitEffects.TakeEffect(effect);

            Assert.IsTrue(unitEffects.CurrentEffects.Count == 2);
            Assert.IsTrue(unitEffects.ActiveDebuffs.Count == 2);
            Assert.IsTrue(unitEffects.ActiveDebuffs[0].DebuffType == DebuffType.Burning);
            Assert.IsTrue(unitEffects.ActiveDebuffs[1].DebuffType == DebuffType.Intoxication);
        }

        [Test]
        public void TakeEffect_AddingFireAndWaterEffect_NoDebuffs()
        {
            var unitEffects = new ElementalEffects(ElementalType.None);

            var effect = ElementalType.Fire;
            unitEffects.TakeEffect(effect);

            effect = ElementalType.Water;
            unitEffects.TakeEffect(effect);

            Assert.IsTrue(unitEffects.CurrentEffects.Count == 0);
            Assert.IsTrue(unitEffects.ActiveDebuffs.Count == 0);
        }

        [Test]
        public void TakeEffect_AddingFireAndIceEffect_NoDebuffs()
        {
            var unitEffects = new ElementalEffects(ElementalType.None);

            var effect = ElementalType.Fire;
            unitEffects.TakeEffect(effect);

            effect = ElementalType.Ice;
            unitEffects.TakeEffect(effect);

            Assert.IsTrue(unitEffects.CurrentEffects.Count == 0);
            Assert.IsTrue(unitEffects.ActiveDebuffs.Count == 0);
        }
        
        [Test]
        public void TakeEffect_AddingFirePoisonFireEffects_PresenceBurningAndIntoxicationDebuffs()
        {
            var unitEffects = new ElementalEffects(ElementalType.None);

            var effect = ElementalType.Fire;
            unitEffects.TakeEffect(effect);
            effect = ElementalType.Poison;
            unitEffects.TakeEffect(effect);
            effect = ElementalType.Fire;
            unitEffects.TakeEffect(effect);

            Assert.IsTrue(unitEffects.CurrentEffects.Count == 2);
            Assert.IsTrue(unitEffects.ActiveDebuffs.Count == 2);
            Assert.IsTrue(unitEffects.ActiveDebuffs[0].DebuffType == DebuffType.Burning);
            Assert.IsTrue(unitEffects.ActiveDebuffs[1].DebuffType == DebuffType.Intoxication);
        }
        
        [Test]
        public void TakeEffect_AddingFireWaterFireEffects_PresenceSingleBurningDebuff()
        {
            var unitEffects = new ElementalEffects(ElementalType.None);

            var effect = ElementalType.Fire;
            unitEffects.TakeEffect(effect);
            effect = ElementalType.Water;
            unitEffects.TakeEffect(effect);
            effect = ElementalType.Fire;
            unitEffects.TakeEffect(effect);

            Assert.IsTrue(unitEffects.CurrentEffects.Count == 1);
            Assert.IsTrue(unitEffects.ActiveDebuffs.Count == 1);
            Assert.IsTrue(unitEffects.ActiveDebuffs[0].DebuffType == DebuffType.Burning);
        }
        
        [Test]
        public void TakeEffect_AddingFireIceFireEffects_PresenceBurningDebuff()
        {
            var unitEffects = new ElementalEffects(ElementalType.None);

            var effect = ElementalType.Fire;
            unitEffects.TakeEffect(effect);
            effect = ElementalType.Ice;
            unitEffects.TakeEffect(effect);
            effect = ElementalType.Fire;
            unitEffects.TakeEffect(effect);

            Assert.IsTrue(unitEffects.CurrentEffects.Count == 1);
            Assert.IsTrue(unitEffects.ActiveDebuffs.Count == 1);
            Assert.IsTrue(unitEffects.ActiveDebuffs[0].DebuffType == DebuffType.Burning);
        }
        #endregion

        #region Adding a poison effect and then others
        [Test]
        public void TakeEffect_AddingMultiplePoisonEffects_PresenceSingleIntoxicationDebuff()
        {
            var unitEffects = new ElementalEffects(ElementalType.None);

            var effect = ElementalType.Poison;
            unitEffects.TakeEffect(effect);
            unitEffects.TakeEffect(effect);
            unitEffects.TakeEffect(effect);

            Assert.IsTrue(unitEffects.CurrentEffects.Count == 1);
            Assert.IsTrue(unitEffects.ActiveDebuffs.Count == 1);
            Assert.IsTrue(unitEffects.ActiveDebuffs[0].DebuffType == DebuffType.Intoxication);
        }

        [Test]
        public void TakeEffect_AddingPoisonAndFireEffect_PresenceIntoxicationAndBurningDebuffs()
        {
            var unitEffects = new ElementalEffects(ElementalType.None);

            var effect = ElementalType.Poison;
            unitEffects.TakeEffect(effect);

            effect = ElementalType.Fire;
            unitEffects.TakeEffect(effect);

            Assert.IsTrue(unitEffects.CurrentEffects.Count == 2);
            Assert.IsTrue(unitEffects.ActiveDebuffs.Count == 2);
            Assert.IsTrue(unitEffects.ActiveDebuffs[0].DebuffType == DebuffType.Intoxication);
            Assert.IsTrue(unitEffects.ActiveDebuffs[1].DebuffType == DebuffType.Burning);
        }

        [Test]
        public void TakeEffect_AddingPoisonAndWaterEffect_NoDebuffs()
        {
            var unitEffects = new ElementalEffects(ElementalType.None);

            var effect = ElementalType.Poison;
            unitEffects.TakeEffect(effect);
            effect = ElementalType.Water;
            unitEffects.TakeEffect(effect);

            Assert.IsTrue(unitEffects.CurrentEffects.Count == 0);
            Assert.IsTrue(unitEffects.ActiveDebuffs.Count == 0);
        }
        
        [Test]
        public void TakeEffect_AddingPoisonAndIceEffect_PresenceIntoxicationAndSlowDebuffs()
        {
            var unitEffects = new ElementalEffects(ElementalType.None);

            var effect = ElementalType.Poison;
            unitEffects.TakeEffect(effect);

            effect = ElementalType.Ice;
            unitEffects.TakeEffect(effect);

            Assert.IsTrue(unitEffects.CurrentEffects.Count == 2);
            Assert.IsTrue(unitEffects.ActiveDebuffs.Count == 2);
            Assert.IsTrue(unitEffects.ActiveDebuffs[0].DebuffType == DebuffType.Intoxication);
            Assert.IsTrue(unitEffects.ActiveDebuffs[1].DebuffType == DebuffType.Slow);
        }
        #endregion

        #region Adding a water effect and then others
        [Test]
        public void TakeEffect_AddingMultipleWaterEffects_PresenceSingleWetDebuff()
        {
            var unitEffects = new ElementalEffects(ElementalType.None);

            var effect = ElementalType.Water;
            unitEffects.TakeEffect(effect);
            unitEffects.TakeEffect(effect);
            unitEffects.TakeEffect(effect);

            Assert.IsTrue(unitEffects.CurrentEffects.Count == 1);
            Assert.IsTrue(unitEffects.ActiveDebuffs.Count == 1);
            Assert.IsTrue(unitEffects.ActiveDebuffs[0].DebuffType == DebuffType.Wet);
        }
        
        [Test]
        public void TakeEffect_AddingWaterAndFireEffect_NoDebuffs()
        {
            var unitEffects = new ElementalEffects(ElementalType.None);

            var effect = ElementalType.Water;
            unitEffects.TakeEffect(effect);
            effect = ElementalType.Fire;
            unitEffects.TakeEffect(effect);

            Assert.IsTrue(unitEffects.CurrentEffects.Count == 0);
            Assert.IsTrue(unitEffects.ActiveDebuffs.Count == 0);
        }
        
        [Test]
        public void TakeEffect_AddingWaterAndPoisonEffect_NoDebuffs()
        {
            var unitEffects = new ElementalEffects(ElementalType.None);

            var effect = ElementalType.Water;
            unitEffects.TakeEffect(effect);
            effect = ElementalType.Poison;
            unitEffects.TakeEffect(effect);

            Assert.IsTrue(unitEffects.CurrentEffects.Count == 0);
            Assert.IsTrue(unitEffects.ActiveDebuffs.Count == 0);
        }
        
        [Test]
        public void TakeEffect_AddingWaterAndIceEffect_PresenceSingleFrozenDebuff()
        {
            var unitEffects = new ElementalEffects(ElementalType.None);

            var effect = ElementalType.Water;
            unitEffects.TakeEffect(effect);
            effect = ElementalType.Ice;
            unitEffects.TakeEffect(effect);

            Assert.IsTrue(unitEffects.CurrentEffects.Count == 2);
            Assert.IsTrue(unitEffects.ActiveDebuffs.Count == 1);
            Assert.IsTrue(unitEffects.ActiveDebuffs[0].DebuffType == DebuffType.Frozen);
        }
        #endregion

        #region Adding a water effect and then others
        [Test]
        public void TakeEffect_AddingMultipleIceEffects_PresenceSingleSlowDebuff()
        {
            var unitEffects = new ElementalEffects(ElementalType.None);

            var effect = ElementalType.Ice;
            unitEffects.TakeEffect(effect);
            unitEffects.TakeEffect(effect);
            unitEffects.TakeEffect(effect);

            Assert.IsTrue(unitEffects.CurrentEffects.Count == 1);
            Assert.IsTrue(unitEffects.ActiveDebuffs.Count == 1);
            Assert.IsTrue(unitEffects.ActiveDebuffs[0].DebuffType == DebuffType.Slow);
        }
        
        [Test]
        public void TakeEffect_AddingIceAndFireEffects_NoDebuffs()
        {
            var unitEffects = new ElementalEffects(ElementalType.None);

            var effect = ElementalType.Ice;
            unitEffects.TakeEffect(effect);
            effect = ElementalType.Fire;
            unitEffects.TakeEffect(effect);

            Assert.IsTrue(unitEffects.CurrentEffects.Count == 0);
            Assert.IsTrue(unitEffects.ActiveDebuffs.Count == 0);
        }
        
        [Test]
        public void TakeEffect_AddingIceAndPoisonEffects_PresenceSlowAndIntoxicationDebuffs()
        {
            var unitEffects = new ElementalEffects(ElementalType.None);

            var effect = ElementalType.Ice;
            unitEffects.TakeEffect(effect);
            effect = ElementalType.Poison;
            unitEffects.TakeEffect(effect);

            Assert.IsTrue(unitEffects.CurrentEffects.Count == 2);
            Assert.IsTrue(unitEffects.ActiveDebuffs.Count == 2);
            Assert.IsTrue(unitEffects.ActiveDebuffs[0].DebuffType == DebuffType.Slow);
            Assert.IsTrue(unitEffects.ActiveDebuffs[1].DebuffType == DebuffType.Intoxication);
        }
        
        [Test]
        public void TakeEffect_AddingIceAndWaterEffects_PresenceSingleFrozenDebuff()
        {
            var unitEffects = new ElementalEffects(ElementalType.None);

            var effect = ElementalType.Ice;
            unitEffects.TakeEffect(effect);
            effect = ElementalType.Water;
            unitEffects.TakeEffect(effect);

            Assert.IsTrue(unitEffects.CurrentEffects.Count == 2);
            Assert.IsTrue(unitEffects.ActiveDebuffs.Count == 1);
            Assert.IsTrue(unitEffects.ActiveDebuffs[0].DebuffType == DebuffType.Frozen);
        }
        #endregion

        #region Updating the duration of the effects
        [Test]
        public void UpdateDuration_UpdateDurationWithoutEffects_NoErrors()
        {
            var unitEffects = new ElementalEffects(ElementalType.None);
            unitEffects.UpdateDuration(GlobalParams.TickTime);
        }

        [Test]
        public void UpdateDuration_AddOneTickTimeToDebuffs_DebuffTimeIsCorrect()
        {
            var unitEffects = new ElementalEffects(ElementalType.None);

            var effect = ElementalType.Fire;
            unitEffects.TakeEffect(effect);

            effect = ElementalType.Poison;
            unitEffects.TakeEffect(effect);

            Assert.IsTrue(unitEffects.ActiveDebuffs[0].Duration == GlobalParams.DebuffDuration);
            Assert.IsTrue(unitEffects.ActiveDebuffs[1].Duration == GlobalParams.DebuffDuration);
            unitEffects.UpdateDuration(GlobalParams.TickTime);

            var newDuration = GlobalParams.DebuffDuration - GlobalParams.TickTime;
            Assert.IsTrue(unitEffects.ActiveDebuffs[0].Duration == newDuration);
            Assert.IsTrue(unitEffects.ActiveDebuffs[1].Duration == newDuration);
        }

        [Test]
        public void UpdateDuration_AddTimeLongerThanDurationEffects_NoDebuffs()
        {
            var unitEffects = new ElementalEffects(ElementalType.None);

            var effect = ElementalType.Fire;
            unitEffects.TakeEffect(effect);

            var tickTime = GlobalParams.DebuffDuration + 1;
            unitEffects.UpdateDuration(tickTime);

            List<DebuffModel> debuffModels;
            var isSuccess = unitEffects.TryGetUpdatedDebuffModels(out debuffModels);

            Assert.IsTrue(isSuccess == true);
            Assert.IsTrue(debuffModels.Count == 0);
            Assert.IsTrue(unitEffects.CurrentEffects.Count == 0);
            Assert.IsTrue(unitEffects.ActiveDebuffs.Count == 0);
        }

        [Test]
        public void UpdateDuration_DebuffEndTime_NoDebuffs()
        {
            var unitEffects = new ElementalEffects(ElementalType.None);

            var effect = ElementalType.Fire;
            unitEffects.TakeEffect(effect);

            effect = ElementalType.Poison;
            unitEffects.TakeEffect(effect);

            unitEffects.UpdateDuration(GlobalParams.DebuffDuration);

            List<DebuffModel> debuffModels;
            var isSuccess = unitEffects.TryGetUpdatedDebuffModels(out debuffModels);

            Assert.IsTrue(isSuccess == true);
            Assert.IsTrue(debuffModels.Count == 0);
            Assert.IsTrue(unitEffects.CurrentEffects.Count == 0);
            Assert.IsTrue(unitEffects.ActiveDebuffs.Count == 0);
        }
        #endregion
    }
}
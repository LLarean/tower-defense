using Game.Core.Elements;
using Game.Core.Monsters;
using Game.Core.Reactions;
using Game.Core.Zones;
using UnityEngine;

namespace Game.Stage.Config
{
    [CreateAssetMenu(menuName = "Game/Chemistry")]
    public sealed class ChemistryAsset : ScriptableObject
    {
        [Header("Electrocute: Water + Lightning")]
        [SerializeField] private float _electrocuteRadius = 5f;
        [SerializeField] private float _electrocuteMultiplier = 1.5f;
        [Header("Freeze: Water + Ice")]
        [SerializeField] private float _freezeSeconds = 2f;
        [Header("Shatter: Frozen + Earth")]
        [SerializeField] private float _shatterRadius = 3f;
        [SerializeField] private float _shatterMultiplier = 4f;
        [SerializeField] private float _shatterSplash = 0.5f;
        [Header("Steam: Fire + Water")]
        [SerializeField] private float _steamRadius = 3.5f;
        [SerializeField] private float _steamSeconds = 4f;
        [SerializeField] private float _steamDamage = 8f;
        [SerializeField] private float _steamSlow = 0.3f;
        [Header("Thermal shock: Fire + Ice")]
        [SerializeField] private float _thermalMultiplier = 3f;
        [Header("Gas explosion: Fire + Poison")]
        [SerializeField] private float _gasRadius = 4f;
        [SerializeField] private float _gasMultiplier = 2f;
        [SerializeField] private float _gasSeconds = 3f;
        [Header("Overload: Fire + Lightning")]
        [SerializeField] private float _overloadRadius = 3f;
        [SerializeField] private float _overloadMultiplier = 1.5f;
        [SerializeField] private float _overloadPush = 3f;
        [Header("Toxic puddle: Poison + Water")]
        [SerializeField] private float _puddleRadius = 3f;
        [SerializeField] private float _puddleSeconds = 5f;
        [SerializeField] private float _puddlePoison = 3f;
        [Header("Mud: Earth + Water")]
        [SerializeField] private float _mudRadius = 3.5f;
        [SerializeField] private float _mudSeconds = 6f;
        [SerializeField] private float _mudSlow = 0.5f;
        [Header("Magma: Earth + Fire")]
        [SerializeField] private float _magmaRadius = 3f;
        [SerializeField] private float _magmaSeconds = 5f;
        [SerializeField] private float _magmaBurn = 6f;
        [Header("Superconduct: Ice + Lightning")]
        [SerializeField] private float _superconductRadius = 4f;
        [SerializeField] private float _superconductShred = 0.4f;
        [SerializeField] private float _superconductSeconds = 5f;
        [Header("Corrosion: Poison + Lightning")]
        [SerializeField] private float _corrosionShred = 0.7f;
        [SerializeField] private float _corrosionSeconds = 6f;
        [Header("Blaze: Oil + Fire")]
        [SerializeField] private float _blazeRadius = 5f;
        [SerializeField] private float _blazeMultiplier = 3f;
        [SerializeField] private float _blazeSeconds = 6f;
        [Header("Slick: Oil + Water")]
        [SerializeField] private float _slickRadius = 4f;
        [SerializeField] private float _slickSeconds = 6f;
        [SerializeField] private float _slickSlow = 0.25f;
        [Header("Tar: Oil + Earth")]
        [SerializeField] private float _tarRadius = 3.5f;
        [SerializeField] private float _tarSeconds = 6f;
        [SerializeField] private float _tarSlow = 0.7f;

        public Chemistry Chemistry() =>
            new Chemistry()
                .With(Element.Water, Element.Lightning, new Electrocute(_electrocuteRadius, _electrocuteMultiplier))
                .With(Element.Water, Element.Ice, new Freeze(_freezeSeconds))
                .With(Condition.Frozen, Element.Earth, new Shatter(_shatterRadius, _shatterMultiplier, _shatterSplash))
                .With(Element.Fire, Element.Water, new Pool(ReactionKind.Steam, _steamRadius, _steamSeconds, new Mixture(new Scald(Element.Fire, _steamDamage), new Affliction(Condition.Mired, _steamSlow))))
                .With(Element.Fire, Element.Ice, new ThermalShock(_thermalMultiplier))
                .With(Element.Fire, Element.Poison, new Ignition(ReactionKind.GasExplosion, _gasRadius, _gasMultiplier, _gasSeconds))
                .With(Element.Fire, Element.Lightning, new Overload(_overloadRadius, _overloadMultiplier, _overloadPush))
                .With(Element.Poison, Element.Water, new Pool(ReactionKind.ToxicPuddle, _puddleRadius, _puddleSeconds, new Affliction(Condition.Poisoned, _puddlePoison)))
                .With(Element.Earth, Element.Water, new Pool(ReactionKind.Mud, _mudRadius, _mudSeconds, new Affliction(Condition.Mired, _mudSlow)))
                .With(Element.Earth, Element.Fire, new Pool(ReactionKind.Magma, _magmaRadius, _magmaSeconds, new Affliction(Condition.Burning, _magmaBurn)))
                .With(Element.Ice, Element.Lightning, new Corrode(ReactionKind.Superconduct, _superconductRadius, _superconductShred, _superconductSeconds))
                .With(Element.Poison, Element.Lightning, new Corrode(ReactionKind.Corrosion, 0f, _corrosionShred, _corrosionSeconds))
                .With(Element.Oil, Element.Fire, new Ignition(ReactionKind.Blaze, _blazeRadius, _blazeMultiplier, _blazeSeconds))
                .With(Element.Oil, Element.Water, new Pool(ReactionKind.Slick, _slickRadius, _slickSeconds, new Affliction(Condition.Oiled, _slickSlow)))
                .With(Element.Oil, Element.Earth, new Pool(ReactionKind.Tar, _tarRadius, _tarSeconds, new Affliction(Condition.Mired, _tarSlow)));
    }
}

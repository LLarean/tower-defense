using System.Collections.Generic;
using System.Numerics;
using Game.Core.Arenas;
using Game.Core.Economy;

namespace Game.Core.Towers
{
    public sealed class Garrison
    {
        private readonly Plots _plots;
        private readonly Treasury _treasury;
        private readonly IGarrisonFeedback _feedback;
        private readonly List<Tower> _towers = new List<Tower>();
        private int _next;

        public Garrison(Plots plots, Treasury treasury)
            : this(plots, treasury, new SilentGarrison())
        {
        }

        public Garrison(Plots plots, Treasury treasury, IGarrisonFeedback feedback)
        {
            _plots = plots;
            _treasury = treasury;
            _feedback = feedback;
        }

        public IReadOnlyList<Tower> Towers => _towers;

        public bool TryBuild(int player, TowerSpec spec, Vector3 point, out Tower tower)
        {
            tower = new Tower(_next, spec, _plots.Snap(point), player);
            if (!_plots.Free(point) || !_treasury.Wallet(player).TrySpend(spec.Price))
            {
                return false;
            }
            _next++;
            _plots.TryClaim(point, out _);
            _towers.Add(tower);
            _feedback.Built(tower);
            return true;
        }

        public bool TryFind(int id, out Tower tower)
        {
            foreach (var standing in _towers)
            {
                if (standing.Id == id)
                {
                    tower = standing;
                    return true;
                }
            }
            tower = null;
            return false;
        }

        public bool TryUpgrade(int player, Tower tower)
        {
            if (!tower.Upgradable || !_treasury.Wallet(player).TrySpend(tower.UpgradeCost))
            {
                return false;
            }
            tower.Upgrade();
            _feedback.Upgraded(tower);
            return true;
        }

        public bool TrySell(int player, Tower tower)
        {
            if (tower.Owner != player || !_towers.Remove(tower))
            {
                return false;
            }
            _plots.Release(tower.Position);
            _treasury.Wallet(player).Earn(tower.Refund);
            _feedback.Sold(tower);
            return true;
        }

        public void Tick(float seconds, Arena arena, Volley volley)
        {
            foreach (var tower in _towers)
            {
                tower.Tick(seconds, arena, volley);
            }
        }
    }
}

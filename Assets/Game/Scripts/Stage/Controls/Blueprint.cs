using Game.Core.Towers;
using Game.Stage.Orders;
using Game.Stage.Towers;
using UnityEngine;

namespace Game.Stage.Controls
{
    public sealed class Blueprint
    {
        private readonly Plots _plots;
        private readonly Armory _armory;
        private readonly IOrders _orders;
        private readonly Ghost _ghost;
        private readonly float _cell;
        private TowerSpec _spec;

        public Blueprint(Plots plots, Armory armory, IOrders orders, Ghost ghost, float cell)
        {
            _plots = plots;
            _armory = armory;
            _orders = orders;
            _ghost = ghost;
            _cell = cell;
        }

        public bool Active { get; private set; }

        public void Choose(TowerSpec spec)
        {
            _spec = spec;
            Active = true;
            _ghost.Show(_armory.Asset(spec).Model, spec.Grade(0).Range, _cell);
        }

        public void Cancel()
        {
            Active = false;
            _ghost.Hide();
        }

        public void Aim(Vector3 point, bool place)
        {
            var core = point.ToCore();
            var allowed = _plots.Free(core);
            var spot = _plots.Snap(core).ToUnity();
            spot.y = point.y;
            _ghost.Place(spot, allowed);
            if (place && allowed)
            {
                _orders.Build(_spec, spot.ToCore());
            }
        }
    }
}

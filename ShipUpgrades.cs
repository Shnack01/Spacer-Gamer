using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SpacerGamer.Collisions;

namespace SpacerGamer
{
    public abstract class ShipUpgrade
    {
        private ShipBase _ship;
        public ShipUpgrade(ShipBase ship)
        {
            _ship = ship;
        }

        public ShipBase IncreaseShip;

    }
}
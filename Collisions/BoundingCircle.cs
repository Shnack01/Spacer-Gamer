using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using SpacerGamer.Collisions;
namespace SpacerGamer.Collisions
{
    /// <summary>
    /// a struct representing circular bounds
    /// </summary>
    public class BoundingCircle
    {
        /// <summary>
        /// the center of the boundingCircle
        /// </summary>
        public Vector2 Center;
        /// <summary>
        /// radius of the bounding circle
        /// </summary>
        public float Radius;
        /// <summary>
        /// Constructs a new Bounding Circle
        /// </summary>
        /// <param name="center">the center</param>
        /// <param name="radius">the radius</param>
        public BoundingCircle(Vector2 center, float radius)
        {
            Center = center;
            Radius = radius;
        }
        /// <summary>
        /// tests for a collisions between this and another bounding circle
        /// </summary>
        /// <param name="other">the bounding circle</param>
        /// <returns>true for collision, false other</returns>
        public bool CollidesWith(BoundingCircle other)
        {
            return CollisionHelper.Collides(this, other);
        }

        public bool CollidesWith(BoundingRectangle other)
        {
            return CollisionHelper.Collides(this, other);
        }
    }
}
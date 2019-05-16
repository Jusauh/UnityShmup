using Shmup.Identifiers;

namespace Shmup.DataStructures
{
    public struct BulletDataContainer
    {
        public int Delay { get; private set; }
        public float? Speed { get; private set; }
        public float Acceleration { get; private set; }
        public float MaxSpeed { get; private set; }
        public float? Angle { get; private set; }
        public float Rotation { get; private set; }
        public BulletSpriteIdentifier BulletSprite { get; private set; }

        public BulletDataContainer(int delay, float? speed, float acceleration, float maxSpeed, float? angle, float rotation, BulletSpriteIdentifier sprite)
        {
            Delay = delay;
            Speed = speed;
            Acceleration = acceleration;
            MaxSpeed = maxSpeed;
            Angle = angle;
            Rotation = rotation;
            BulletSprite = sprite;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LailaWalking
{
    class Sprite
    {
        public Texture2D Texture;

        //protected List<Rectangle> frames = new List<Rectangle>();
        protected List<List<Rectangle>> frames = new List<List<Rectangle>>();
        private int frameWidth = 0;
        private int frameHeight = 0;
        private int currentAnimation = 0;
        private int currentFrame;
        private float frameTime = 0.1f;
        private float timeForCurrentFrame = 0.0f;
        private bool animating = false;

        private Color tintColor = Color.White;
        private float rotation = 0.0f;

        public int CollisionRadius = 0;
        public int BoundingXPadding = 0;
        public int BoundingYPadding = 0;

        protected Vector2 location = Vector2.Zero;
        protected Vector2 velocity = Vector2.Zero;

        public Sprite(Vector2 location, Texture2D texture, Rectangle initialFrame, Vector2 velocity)
        {
            this.location = location;
            Texture = texture;
            this.velocity = velocity;

            frames.Add(new List<Rectangle>());
            frames[0].Add(initialFrame);
            currentAnimation = 0;
            frameWidth = initialFrame.Width;
            frameHeight = initialFrame.Height;
        }

        public Vector2 Location
        {
            get { return location; }
            set { location = value; }
        }

        public Vector2 Velocity
        {
            get { return velocity; }
            set { velocity = value; }
        }

        public Color TintColor
        {
            get { return tintColor; }
            set { tintColor = value; }
        }

        public float Rotation
        {
            get { return rotation; }
            set { rotation = value % MathHelper.TwoPi; }
        }

        public int Frame
        {
            get { return currentFrame; }
            set { currentFrame = (int)MathHelper.Clamp(value, 0, frames.Count - 1); }
        }

        public float FrameTime
        {
            get { return frameTime; }
            set { frameTime = MathHelper.Max(0, value); }
        }

        public Rectangle Source
        {
            get { return frames[currentAnimation][currentFrame]; }
        }

        public Rectangle Destination
        {
            get { return new Rectangle((int)location.X, (int)location.Y, frameWidth, frameHeight); }
        }

        public Vector2 Center
        {
            get { return location + new Vector2(frameWidth / 2, frameHeight / 2); }
        }

        public Rectangle BoundingBoxRect
        {
            get
            {
                return new Rectangle((int)location.X + BoundingXPadding,
                (int)location.Y + BoundingYPadding, frameWidth - (BoundingXPadding * 2),
                frameHeight - (BoundingYPadding * 2));
            }
        }

        public bool IsBoxColliding(Rectangle otherBox)
        {
            return BoundingBoxRect.Intersects(otherBox);
        }

        public bool IsCircleColliding(Vector2 otherCenter, float otherRadius)
        {
            if (Vector2.Distance(Center, otherCenter) < (CollisionRadius + otherRadius))
                { return true; }
            else
                { return false; }
        }

        public void AddFrame(int animation, Rectangle frameRectangle)
        {
            frames[animation].Add(frameRectangle);
        }

        public void AddAnimation()
        {
            frames.Add(new List<Rectangle>());
        }

        public void ChangeAnimation(int animation)
        {
            currentAnimation = animation;
        }

        public void ChangeFrame(int frame)
        {
            currentFrame = frame;
        }

        //override just in case
        public void ChangeFrame(int animation, int frame)
        {
            currentAnimation = animation;
            currentFrame = frame;
        }

        public void IsAnimating(bool animating)
        {
            this.animating = animating;
        }

        public virtual void Update(GameTime gameTime)
        {
            float elapsed = (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            timeForCurrentFrame += elapsed;

            if (animating)
            {
                if (timeForCurrentFrame >= 200)
                {
                    currentFrame = (currentFrame + 1) % (frames[currentAnimation].Count);
                    timeForCurrentFrame = 0.0f;
                }

                location += (velocity * (float)gameTime.ElapsedGameTime.TotalSeconds);
            }
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, Center, Source, tintColor, rotation,
                new Vector2(frameWidth / 2, frameHeight / 2), 1.0f, SpriteEffects.None, 0.0f);
        }
    }
}

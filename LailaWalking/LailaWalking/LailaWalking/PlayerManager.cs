using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace LailaWalking
{
    class PlayerManager
    {
        //might change this to be more useful
        int animations = 4;

        public Sprite playerSprite;
        private float playerSpeed = 160.0f;
        private Rectangle playerAreaLimit;

        public long playerScore = 0;
        public int livesRemaining = 3;
        public bool destroyed = false;

        //private Vector2 gunOffset = new Vector2(25, 10);
        //private float shotTimer = 0.0f;
        //private float minShotTimer = 0.2f;
        private int playerRadius = 15;
        //public ShotManager PlayerShotManager;

        public PlayerManager(Texture2D texture, Rectangle initialFrame, int frameCount,
            Rectangle screenBounds)
        {
            playerSprite = new Sprite(new Vector2(500, 500), texture, initialFrame, Vector2.Zero);

            /*PlayerShotManager = new ShotManager(texture, new Rectangle(0, 300, 5, 5), 4, 2, 250f, 
                screenBounds);*/

            playerAreaLimit = new Rectangle(0, 0, screenBounds.Width, 
                screenBounds.Height);

            for (int x = 1; x < frameCount; x++)
            {
                playerSprite.AddFrame(0, new Rectangle(initialFrame.X + (initialFrame.Width * x),
                    initialFrame.Y, initialFrame.Width, initialFrame.Height));
            }

            for (int y = 1; y < animations; y++)
            {
                playerSprite.AddAnimation();

                for (int x = 0; x < frameCount; x++)
                {
                    playerSprite.AddFrame(y, new Rectangle(initialFrame.X + (initialFrame.Width * x),
                    initialFrame.Y + (initialFrame.Height * y), 
                    initialFrame.Width, initialFrame.Height));
                }
            }

            playerSprite.CollisionRadius = playerRadius;
        }

        /*private void FireShot()
        {
            if (shotTimer >= minShotTimer)
            {
                PlayerShotManager.FireShot(playerSprite.Location + gunOffset, 
                    new Vector2(0, -1), true);

                shotTimer = 0.0f;
            }
        }*/

        private void HandleKeyboardInput(KeyboardState keyState, GamePadState gamePadState)
        {
            switch (keyState.IsKeyDown(Keys.Up) || keyState.IsKeyDown(Keys.Down)
                || keyState.IsKeyDown(Keys.Left) || keyState.IsKeyDown(Keys.Right))
            {

                case true:
                    if (keyState.IsKeyDown(Keys.Up))
                    {
                        playerSprite.IsAnimating(true);
                        playerSprite.ChangeAnimation(2);
                        if (playerSprite.Frame == 0)
                        {
                            playerSprite.ChangeFrame(1);
                        }
                        playerSprite.Velocity += new Vector2(0, -1);
                    }

                    if (keyState.IsKeyDown(Keys.Down))
                    {
                        playerSprite.IsAnimating(true);
                        playerSprite.ChangeAnimation(0);
                        if (playerSprite.Frame == 0)
                        {
                            playerSprite.ChangeFrame(1);
                        }
                        playerSprite.Velocity += new Vector2(0, 1);
                    }

                    if (keyState.IsKeyDown(Keys.Left))
                    {
                        playerSprite.IsAnimating(true);
                        playerSprite.ChangeAnimation(1);
                        if (playerSprite.Frame == 0)
                        {
                            playerSprite.ChangeFrame(1);
                        }
                        playerSprite.Velocity += new Vector2(-1, 0);
                    }


                    if (keyState.IsKeyDown(Keys.Right))
                    {
                        playerSprite.IsAnimating(true);
                        playerSprite.ChangeAnimation(3);
                        if (playerSprite.Frame == 0)
                        {
                            playerSprite.ChangeFrame(1);
                        }
                        playerSprite.Velocity += new Vector2(1, 0);
                    }

                    break;


                case false:
                    if((gamePadState.ThumbSticks.Left.X == 0)&&
                        (gamePadState.ThumbSticks.Left.Y == 0))
                    {
                        playerSprite.IsAnimating(false);
                        playerSprite.ChangeFrame(0);
                    }
                    break;
            }
                
            



            /*if (keyState.IsKeyDown(Keys.Space))
            {
                FireShot();
            }*/
        }

        private void HandleGamepadInput(GamePadState gamePadState, KeyboardState keyState)
        {
            Vector2 direction = new Vector2(gamePadState.ThumbSticks.Left.X,
                -gamePadState.ThumbSticks.Left.Y);

            if (direction.Y < 0)
            {
                playerSprite.IsAnimating(true);
                playerSprite.ChangeAnimation(2);
                if (playerSprite.Frame == 0)
                {
                    playerSprite.ChangeFrame(1);
                }
            }

            if (direction.Y > 0)
            {
                playerSprite.IsAnimating(true);
                playerSprite.ChangeAnimation(0);
                if (playerSprite.Frame == 0)
                {
                    playerSprite.ChangeFrame(1);
                }
            }

            if (direction.X < 0)
            {
                playerSprite.IsAnimating(true);
                playerSprite.ChangeAnimation(1);
                if (playerSprite.Frame == 0)
                {
                    playerSprite.ChangeFrame(1);
                }
            }

            if (direction.X > 0)
            {
                playerSprite.IsAnimating(true);
                playerSprite.ChangeAnimation(3);
                if (playerSprite.Frame == 0)
                {
                    playerSprite.ChangeFrame(1);
                }
            }

            if ((direction.X == 0) && (direction.Y == 0))
            {
                if (!keyState.IsKeyDown(Keys.Up) && !keyState.IsKeyDown(Keys.Down)
                && !keyState.IsKeyDown(Keys.Left) && !keyState.IsKeyDown(Keys.Right))
                {
                    playerSprite.IsAnimating(false);
                    playerSprite.ChangeFrame(0);
                }
            }

            playerSprite.Velocity += direction;

            /*if (gamePadState.Buttons.A == ButtonState.Pressed)
            { FireShot(); }*/
        }

        private void ImposeMovementLimits()
        {
            Vector2 location = playerSprite.Location;

            if(location.X < playerAreaLimit.X)
            {location.X = playerAreaLimit.X;}

            if(location.X > (playerAreaLimit.Right - playerSprite.Source.Width))
            {location.X = (playerAreaLimit.Right - playerSprite.Source.Width);}

            if(location.Y < playerAreaLimit.Y)
            {location.Y = playerAreaLimit.Y;}

            if(location.Y > (playerAreaLimit.Bottom - playerSprite.Source.Height))
            {location.Y = (playerAreaLimit.Bottom - playerSprite.Source.Height);}

            playerSprite.Location = location;
        }

        public void Update (GameTime gameTime)
        {
            //PlayerShotManager.Update(gameTime);

            if(!destroyed)
            {
                playerSprite.Velocity = Vector2.Zero;

                //shotTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;

                HandleKeyboardInput(Keyboard.GetState(), GamePad.GetState(PlayerIndex.One));
                HandleGamepadInput(GamePad.GetState(PlayerIndex.One), Keyboard.GetState());

                playerSprite.Velocity.Normalize();
                playerSprite.Velocity *= playerSpeed;

                playerSprite.Update(gameTime);
                ImposeMovementLimits();
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            //PlayerShotManager.Draw(spriteBatch);

            if(!destroyed)
            {
                playerSprite.Draw(spriteBatch);
            }
        }

    }
}

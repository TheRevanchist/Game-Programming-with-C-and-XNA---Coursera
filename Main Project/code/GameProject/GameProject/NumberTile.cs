using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GameProject
{
    /// <remarks>
    /// A number tile
    /// </remarks>
    class NumberTile
    {
        #region Fields

        // original length of each side of the tile
        int originalSideLength;

        // whether or not this tile is the correct number
        bool isCorrectNumber;

        // drawing support
        Texture2D texture;
        Rectangle drawRectangle;
        Rectangle sourceRectangle;
        Texture2D blinkingTexture;
        Texture2D currentTexture;

        // blinking support
        const int TOTAL_BLINK_MILLISECONDS = 2000;
        int elapsedBlinkMilliseconds = 0;
        const int FRAME_BLINK_MILLISECONDS = 1000;
        int elapsedFrameMilliseconds = 0;

        // shrinking support
        const int TOTAL_SHRINK_MILLISECONDS = 4000;
        int elapsedShrinkMilliseconds = 0;

        // fields if the tile is visible, if the tile is blinking and if the tile is shrinking
        bool isVisible = true;
        bool isBlinking = false;
        bool isShrinking = false;

        // clickStarted and buttonReleased fields
        bool buttonReleased = false;
        bool clickStarted = false;

        // audio support
        SoundBank soundBank;

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="contentManager">the content manager</param>
        /// <param name="center">the center of the tile</param>
        /// <param name="sideLength">the side length for the tile</param>
        /// <param name="number">the number for the tile</param>
        /// <param name="correctNumber">the correct number</param>
        /// <param name="soundBank">the sound bank for playing cues</param>
        public NumberTile(ContentManager contentManager, Vector2 center, int sideLength,
            int number, int correctNumber, SoundBank soundBank)
        {
            // set original side length field
            this.originalSideLength = sideLength;

            // set sound bank field
            this.soundBank = soundBank;

            // load content for the tile and create draw rectangle
            LoadContent(contentManager, number);
            drawRectangle = new Rectangle((int)center.X - sideLength / 2,
                 (int)center.Y - sideLength / 2, sideLength, sideLength);

            // set isCorrectNumber flag
            isCorrectNumber = number == correctNumber;
        }

        #endregion

        #region Public methods

         // <summary>
         // Updates the tile based on game time and mouse state
         // </summary>
         // <param name="gameTime">the current GameTime</param>
         // <param name="mouse">the current mouse state</param>
         // <return>true if the correct number was guessed, false otherwise</return>
        public bool Update(GameTime gameTime, MouseState mouse)
        {
            // handle blinking
            if (isBlinking)
            {
                 elapsedBlinkMilliseconds = elapsedBlinkMilliseconds + gameTime.ElapsedGameTime.Milliseconds;
                 if (elapsedBlinkMilliseconds < TOTAL_BLINK_MILLISECONDS)
                 {
                     elapsedFrameMilliseconds += gameTime.ElapsedGameTime.Milliseconds;

                     if (elapsedFrameMilliseconds < FRAME_BLINK_MILLISECONDS)
                     {
                         elapsedFrameMilliseconds -= FRAME_BLINK_MILLISECONDS;
                         if (sourceRectangle.X == 0)
                             sourceRectangle.X = currentTexture.Width / 2;
                         else
                             sourceRectangle.X = 0;
                     }

                     //Resetting the Frame time  
                     elapsedFrameMilliseconds = 0;
                     isBlinking = true;
                 }
                 else
                 {
                     // return true
                     isVisible = false;
                     return true;
                 }
            }

            // handle shrinking
            else if (isShrinking)
            {
                elapsedShrinkMilliseconds += gameTime.ElapsedGameTime.Milliseconds;
                int tileSideLength = (int)(originalSideLength *
                    ((float)(TOTAL_SHRINK_MILLISECONDS - elapsedShrinkMilliseconds) / TOTAL_SHRINK_MILLISECONDS));

                // int tileSideLength = originalSideLength *
                //    (TOTAL_SHRINK_MILLISECONDS - elapsedShrinkMilliseconds) / TOTAL_SHRINK_MILLISECONDS;

                if (tileSideLength > 0)
                {
                    drawRectangle.Width = tileSideLength;
                    drawRectangle.Height = tileSideLength;
                }
                else
                {
                    isVisible = false;
                }
            }

            else
            {
                if (drawRectangle.Contains(mouse.X, mouse.Y))
                {
                    sourceRectangle.X = texture.Width / 2;

                    if (mouse.LeftButton == ButtonState.Pressed &&
                        buttonReleased)
                    {
                        clickStarted = true;
                        buttonReleased = false;
                    }
                    else if (mouse.LeftButton == ButtonState.Released)
                    {
                        buttonReleased = true;

                        // if click finished on button, change game state
                        if (clickStarted)
                        {
                            // check for blinking or shrinking
                            if (isCorrectNumber)
                            {
                                isBlinking = true;

                                // changing the current texture
                                currentTexture = blinkingTexture;
                                sourceRectangle.X = 0;

                                // play correct tile sound
                                soundBank.PlayCue("correctGuess");
                            }
                            else
                            {
                                isShrinking = true;

                                // play incorrect tile sound
                                soundBank.PlayCue("incorrectGuess");
                            }
                        }
                    }
                }
                else
                {
                    sourceRectangle.X = 0;

                    clickStarted = false;
                    buttonReleased = false;
                }
            }

            // if we get here, return false
            return false;
        }

        /// <summary>
        /// Draws the number tile
        /// </summary>
        /// <param name="spriteBatch">the SpriteBatch to use for the drawing</param>
        public void Draw(SpriteBatch spriteBatch)
        {
            // draw the tile
            if (isVisible)
            {
                spriteBatch.Draw(currentTexture, drawRectangle, sourceRectangle, Color.White);
            }
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Loads the content for the tile
        /// </summary>
        /// <param name="contentManager">the content manager</param>
        /// <param name="number">the tile number</param>
        private void LoadContent(ContentManager contentManager, int number)
        {
            // convert the number to a string
            string numberString = ConvertIntToString(number);

            // load content for the tile and set source rectangle
            texture = contentManager.Load<Texture2D>(numberString);
            sourceRectangle = new Rectangle(drawRectangle.X, drawRectangle.Y, texture.Width / 2, texture.Height);

            // load content for the blinking texture
            blinkingTexture = contentManager.Load<Texture2D>("blinking" + numberString);

            // setting the current texture to non-blinking texture
            currentTexture = texture;

        }

        /// <summary>
        /// Converts an integer to a string for the corresponding number
        /// </summary>
        /// <param name="number">the integer to convert</param>
        /// <returns>the string for the corresponding number</returns>
        private String ConvertIntToString(int number)
        {
            switch (number)
            {
                case 1:
                    return "one";
                case 2:
                    return "two";
                case 3:
                    return "three";
                case 4:
                    return "four";
                case 5:
                    return "five";
                case 6:
                    return "six";
                case 7:
                    return "seven";
                case 8:
                    return "eight";
                case 9:
                    return "nine";
                default:
                    throw new Exception("Unsupported number for number tile");
            }

        }

        #endregion
    }
}

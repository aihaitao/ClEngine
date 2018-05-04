/*  
 Copyright © 2009 Project Mercury Team Members (http://mpe.codeplex.com/People/ProjectPeople.aspx)

 This program is licensed under the Microsoft Permissive License (Ms-PL).  You should 
 have received a copy of the license along with the source code.  If not, an online copy
 of the license can be found at http://mpe.codeplex.com/license.
*/

namespace ProjectMercury.EffectEditor
{
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using Renderers;
    using System;
    using System.Diagnostics;
    using System.IO;

    internal enum ImageOptions
    {
        Stretch,
        Center,
        Tile
    }

    public class ParticleEffectPreviewControl : GraphicsDeviceControl
    {
        protected override void Initialize()
        {
            SpriteBatch = new SpriteBatch(GraphicsDevice);
        }

        private Vector2 Origin { get; set; }

        private ImageOptions ImageOptions { get; set; }

        public ParticleEffect ParticleEffect { get; set; }

        public Renderer Renderer { get; set; }

        private Vector3 BackgroundColour;

        private SpriteBatch SpriteBatch;
        new private Texture2D BackgroundImage;

        public void SetBackgroundColor(byte r, byte g, byte b)
        {
            BackgroundColour.X = r / 255f;
            BackgroundColour.Y = g / 255f;
            BackgroundColour.Z = b / 255f;
        }

        public void LoadBackgroundImage(string filePath)
        {
            try
            {
                using (FileStream inputStream = File.OpenRead(filePath))
                {
                    BackgroundImage = Texture2D.FromStream(GraphicsDevice, inputStream);
                }

                Origin = new Vector2(BackgroundImage.Width / 2, BackgroundImage.Height / 2);
            }
            catch (Exception e)
            {
                Trace.TraceError(e.Message);
            }
        }

        public void ClearBackgroundImage()
        {
            if (BackgroundImage != null)
            {
                BackgroundImage.Dispose();
                BackgroundImage = null;
            }
        }

        protected override void Draw()
        {
            GraphicsDevice.Clear(new Color(BackgroundColour));

            if (BackgroundImage != null)
            {
                SpriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Opaque);

                switch (ImageOptions)
                {
                    case ImageOptions.Stretch:
                        {
                            SpriteBatch.Draw(BackgroundImage, new Rectangle(0, 0, ClientSize.Width, ClientSize.Height), Color.White);
                         
                            break;
                        }
                    case ImageOptions.Center:
                        {
                            SpriteBatch.Draw(BackgroundImage, new Rectangle(ClientSize.Width / 2, ClientSize.Height / 2, BackgroundImage.Width, BackgroundImage.Height), null, Color.White, 0, Origin, SpriteEffects.None, 1);
                         
                            break;
                        }
                    case ImageOptions.Tile:
                        {
                            for (int x = 0; x < Width; x += BackgroundImage.Width)
                            {
                                for (int y = 0; y < Height; y += BackgroundImage.Height)
                                {
                                    SpriteBatch.Draw(BackgroundImage, new Rectangle(x, y, BackgroundImage.Width, BackgroundImage.Height), Color.White);
                                }
                            }
                         
                            break;
                        }
                }

                SpriteBatch.End();
            }

            if (Renderer != null)
                if (ParticleEffect != null)
                    Renderer.RenderEffect(ParticleEffect);
        }

        internal void ImageOptionsChanged(ImageOptions imageOptions)
        {
            ImageOptions = imageOptions;
        }
    }
}
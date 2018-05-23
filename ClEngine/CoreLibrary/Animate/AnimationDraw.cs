using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Animations;
using MonoGame.Extended.Animations.SpriteSheets;
using MonoGame.Extended.Sprites;
using MonoGame.Extended.TextureAtlases;
using MonoGame.Framework.WpfInterop;
using MonoGame.Framework.WpfInterop.Input;

namespace ClEngine.CoreLibrary.Animate
{
    public class AnimationDraw : WpfGame
    {
        private IGraphicsDeviceService _graphicsDeviceService;
        private WpfKeyboard _keyboard;
        private WpfMouse _mouse;
        private Texture2D _characterTexture;
        private Dictionary<string, Rectangle> _characterMap;
        private TextureAtlas _textureAtlas;
        private SpriteSheetAnimationFactory _spriteSheetAnimationFactory;
        private AnimatedSprite _animatedSprite;
        private SpriteBatch _spriteBatch;

        protected override void Initialize()
        {
            _graphicsDeviceService = new WpfGraphicsDeviceService(this);
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            _keyboard = new WpfKeyboard(this);
            
            _mouse = new WpfMouse(this);
            
            base.Initialize();
        }

        protected override void LoadContent()
        {
        }

        protected override void Update(GameTime gameTime)
        {
            var deltaSeconds = (float)gameTime.ElapsedGameTime.TotalSeconds;

            _animatedSprite?.Update(deltaSeconds);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();
            if (_animatedSprite != null)
                _spriteBatch.Draw(_animatedSprite);
            _spriteBatch.End();
        }

        protected override void UnloadContent()
        {
            _spriteBatch?.Dispose();
            _characterTexture?.Dispose();

            base.UnloadContent();
        }

        public void LoadCharacterTexture(string characterTexture)
        {
            _characterTexture = Content.Load<Texture2D>(characterTexture);
        }

        public void LoadCharacterMap(string characterMap)
        {
            _characterMap = Content.Load<Dictionary<string, Rectangle>>(characterMap);
        }

        public void LoadCharacterAtals(string characterAtlas)
        {
            _textureAtlas = new TextureAtlas(characterAtlas, _characterTexture, _characterMap);
        }

        public void LoadCharacterSpriteAnimation(string characterSpriteAnimation)
        {
            _animatedSprite = new AnimatedSprite(_spriteSheetAnimationFactory, characterSpriteAnimation);
        }
    }
}
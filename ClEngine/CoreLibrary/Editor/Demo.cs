using System;
using ClEngine.CoreLibrary.Map;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.ViewportAdapters;

namespace ClEngine.CoreLibrary.Editor
{
	public abstract class Demo : IComparable<Demo>
	{
		private readonly MapDraw _game;
		private bool _isInitialized;

		protected Demo(MapDraw game)
		{
			_game = game;
		}

		public abstract string Name { get; }

		protected ContentManager Content { get; private set; }

		protected GraphicsDevice GraphicsDevice => _game.GraphicsDevice;
		protected GameComponentCollection Components => _game.Components;
		protected Camera2D Camera { get; private set; }
		protected ViewportAdapter ViewportAdapter { get; private set; }

		protected virtual void Initialize()
		{
			ViewportAdapter = _game.ViewportAdapter;
			Camera = new Camera2D(ViewportAdapter);
		}

		protected virtual void LoadContent() { }
		protected virtual void UnloadContent() { }
		protected virtual void Update(GameTime gameTime) { }
		protected virtual void Draw(GameTime gameTime) { }

		protected void Exit()
		{
		}

		public void Load()
		{
			if (!_isInitialized)
			{
				Initialize();
				_isInitialized = true;
			}

			if (Content == null)
			{
				Content = new ContentManager(_game.Services, "Content");
				LoadContent();
			}
		}

		public void Unload()
		{
			if (Content != null)
			{
				Components.Clear();
				UnloadContent();
				Content.Unload();
				Content.Dispose();
				Content = null;
			}
		}

		public void OnUpdate(GameTime gameTime)
		{
			Update(gameTime);
		}

		public void OnDraw(GameTime gameTime)
		{
			Draw(gameTime);
		}

		public int CompareTo(Demo other)
		{
			return string.Compare(GetType().Name, other.GetType().Name, StringComparison.Ordinal);
		}
	}
}
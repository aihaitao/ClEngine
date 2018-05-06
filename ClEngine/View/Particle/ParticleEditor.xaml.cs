using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using ClEngine.CoreLibrary.Particle;
using ClEngine.Model;
using ClEngine.ViewModel;
using CLEngine.PluginInterfaces;
using GalaSoft.MvvmLight.Messaging;
using MonoGame.Extended.Particles;

namespace ClEngine.View.Particle
{
	/// <summary>
	/// ParticleEditor.xaml 的交互逻辑
	/// </summary>
	public partial class ParticleEditor : IInterfaceProvider
	{
		private ParticleViewModel _particleViewModel;

		public ParticleEditor()
		{
			InitializeComponent();

			_particleViewModel = new ParticleViewModel();
			DataContext = _particleViewModel;

			TriggerTimer = new Stopwatch();
		}

		private void EffectTree_OnSelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
		{
			ParticlePropertyGrid.SelectedObject = e.NewValue;
		}

		private void EffectTree_OnKeyUp(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Delete)
			{
				if (EffectTree.SelectedItem is EmitterTreeNode node)
				{
					var emitter = node.Emitter;
					
					OnEmitterRemoved(new EmitterEventArgs(emitter));
				}

				if (EffectTree.SelectedItem is ModifierTreeNode treeNode)
				{
					var modifier = treeNode.Modifier;

					OnModifierRemoved(new ModifierEventArgs(modifier));
				}

				EffectTree.Items.Remove(EffectTree.SelectedItem);
			}
		}

		public void SetSerializationPlugins(IEnumerable<IEffectSerializationPlugin> plugins)
		{
			foreach (var effectSerializationPlugin in plugins)
				AddSerializationPlugin(effectSerializationPlugin);
		}

		private void AddSerializationPlugin(IEffectSerializationPlugin plugin)
		{
			if (plugin.DeserializeSupported)
			{
				// TODO: Import MenuItem
			}

			if (plugin.SerializeSuported)
			{
				// TODO: Import MenuItem
			}
		}

		void IInterfaceProvider.SetParticleEffect(ParticleEffect effect)
		{
			SetParticleEffect(effect);
		}

		public void SetUpdateTime(float totalSeconds)
		{
			throw new NotImplementedException();
		}

		public IEnumerable<TextureReference> TextureReferences { get; set; }

		private void SetParticleEffect(ParticleEffect effect)
		{
			EffectTree.Items.Clear();

			var node = new ParticleEffectTreeNode(effect);

			EffectTree.Items.Add(node);

			node.IsSelected = true;
			node.IsExpanded = true;
		}

		public event CloneModifierEventHandler ModifierCloned;

		public event EmitterReinitialisedEventHandler EmitterReinitialised;
		public event NewTextureReferenceEventHandler TextureReferenceAdded;
		public event TextureReferenceChangedEventHandler TextureReferenceChanged;
		public event EventHandler NewParticleEffect;

		private Point LocalMousePosition;
		private bool MouseButtonPressed;
		private Stopwatch TriggerTimer { get; set; }
		public bool TriggerRequired(out float x, out float y)
		{
			x = (float)LocalMousePosition.X;
			y = (float)LocalMousePosition.Y;

			if (MouseButtonPressed)
			{
				if (TriggerTimer.Elapsed.TotalSeconds > (1f / 60f))
				{
					TriggerTimer.Reset();
					TriggerTimer.Start();

					return true;
				}
			}

			return false;
		}

		public void Draw(ParticleEffect effect, Renderer renderer)
		{
			ParticleDraw.ParticleEffect = effect;
			ParticleDraw.Renderer = renderer;
		}

		public void SetEmitterPlugins(IEnumerable<IEmitterPlugin> plugins)
		{
			foreach (var emitterPlugin in plugins)
				AddEmitterPlugin(emitterPlugin);
		}

		private void AddEmitterPlugin(IEmitterPlugin plugin)
		{
			// TODO: copy plugin to the interface
		}

		public void SetModifierPlugins(IEnumerable<IModifierPlugin> plugins)
		{
			foreach (var modifierPlugin in plugins)
				AddModifierPlugin(modifierPlugin);
		}

		private void AddModifierPlugin(IModifierPlugin plugin)
		{
			// TODO: add modifier plugin to the interface
		}

		public event ModifierEventHandler ModifierRemoved;

		internal virtual void OnModifierRemoved(ModifierEventArgs e)
		{
			using (new HourglassCursor())
			{
				var handler = Interlocked.CompareExchange(ref ModifierRemoved, null, null);

				handler?.Invoke(this, e);
			}

			AssertOperationOK(e.Result);
		}


		public event EventHandler Ready;
		public event SerializeEventHandler Serialize;
		public event SerializeEventHandler Deserialize;
		public event NewEmitterEventHandler EmitterAdded;
		public event CloneEmitterEventHandler EmitterCloned;

		public event NewModifierEventHandler ModifierAdded;
		public event EmitterEventHandler EmitterRemoved;

		internal virtual void OnEmitterRemoved(EmitterEventArgs e)
		{
			using (new HourglassCursor())
			{
				var handler = Interlocked.CompareExchange(ref EmitterRemoved, null, null);

				handler?.Invoke(this, e);
			}

			AssertOperationOK(e.Result);
		}

		private void AssertOperationOK(CoreOperationResult operationResult)
		{
			Guard.ArgumentNull("operationResult", operationResult);

			if (operationResult == CoreOperationResult.OK)
				return;

			var logModel = new LogModel
			{
				Message = "发生错误时，错误消息现在将写入Trace.log",
				LogLevel = LogLevel.Error
			};
			Messenger.Default.Send(logModel, "Log");
		}

		private void ParticleDraw_OnMouseDown(object sender, MouseButtonEventArgs e)
		{
			MouseButtonPressed = true;
			LocalMousePosition = e.GetPosition(this);
			TriggerTimer.Start();
		}

		private void ParticleDraw_OnMouseUp(object sender, MouseButtonEventArgs e)
		{
			MouseButtonPressed = false;
			TriggerTimer.Stop();
		}

		private void ParticleDraw_OnMouseMove(object sender, MouseEventArgs e)
		{
			if (MouseButtonPressed)
			{
				LocalMousePosition = e.GetPosition(this);
			}
		}
		
		protected virtual void OnReady(EventArgs e)
		{
			var handler = Interlocked.CompareExchange(ref Ready, null, null);

			handler?.Invoke(this, e);
		}

		private void DisplayLibraryEffects()
		{
			var effectsDir = new DirectoryInfo(System.Windows.Forms.Application.StartupPath + "\\EffectLibrary");

			foreach (var fileInfo in effectsDir.GetFiles())
			{
				bool pluginFound = false;

				
			}
		}

		private void ParticleEditor_OnLoaded(object sender, RoutedEventArgs e)
		{
			OnReady(EventArgs.Empty);

			DisplayLibraryEffects();
		}
	}
}

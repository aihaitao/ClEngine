using System;
using System.Collections.Generic;
using CLEngine.PluginInterfaces;
using MonoGame.Extended.Particles;

namespace ClEngine.CoreLibrary.Particle
{
	public delegate void SerializeEventHandler(object sender, SerializeEventArgs e);
	public delegate void NewEmitterEventHandler(object sender, NewEmitterEventArgs e);
	public delegate void CloneEmitterEventHandler(object sender, CloneEmitterEventArgs e);
	public delegate void EmitterEventHandler(object sender, EmitterEventArgs e);
	public delegate void NewModifierEventHandler(object sender, NewModifierEventArgs e);
	public delegate void CloneModifierEventHandler(object sender, CloneModifierEventArgs e);
	public delegate void ModifierEventHandler(object sender, ModifierEventArgs e);
	public delegate void EmitterReinitialisedEventHandler(object sender, EmitterReinitialisedEventArgs e);
	public delegate void NewTextureReferenceEventHandler(object sender, NewTextureReferenceEventArgs e);
	public delegate void TextureReferenceChangedEventHandler(object sender, TextureReferenceChangedEventArgs e);

	public interface IInterfaceProvider
	{
		event EventHandler Ready;
		event SerializeEventHandler Serialize;
		event SerializeEventHandler Deserialize;
		event NewEmitterEventHandler EmitterAdded;
		event CloneEmitterEventHandler EmitterCloned;
		event EmitterEventHandler EmitterRemoved;
		event NewModifierEventHandler ModifierAdded;
		event CloneModifierEventHandler ModifierCloned;
		event ModifierEventHandler ModifierRemoved;
		event EmitterReinitialisedEventHandler EmitterReinitialised;
		event NewTextureReferenceEventHandler TextureReferenceAdded;
		event TextureReferenceChangedEventHandler TextureReferenceChanged;
		event EventHandler NewParticleEffect;

		bool TriggerRequired(out float x, out float y);
		void Draw(ParticleEffect effect, Renderer renderer);

		void SetEmitterPlugins(IEnumerable<IEmitterPlugin> plugins);
		void SetModifierPlugins(IEnumerable<IModifierPlugin> plugins);
		void SetSerializationPlugins(IEnumerable<IEffectSerializationPlugin> plugins);
		void SetParticleEffect(ParticleEffect effect);
		void SetUpdateTime(float totalSeconds);

		IEnumerable<TextureReference> TextureReferences { get; set; }
	}
}
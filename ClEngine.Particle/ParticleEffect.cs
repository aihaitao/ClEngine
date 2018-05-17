using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using System.IO;
using System.Linq;
using ClEngine.Particle.Core;
using ClEngine.Particle.Serialization;
using ClEngine.Particle.TypeEditor;
using Microsoft.Xna.Framework;
using MonoGame.Extended.Serialization;
using Newtonsoft.Json;

namespace ClEngine.Particle
{
	public class ParticleEffect : Transform2D
	{
		public ParticleEffect(string name = null, bool autoTrigger = true, float autoTriggerDelay = 0f)
		{
			Name = name;
			Emitters = new List<ParticleEmitter>();
		}

		[ParticleDisplayName("Name"), ParticleDescription("ParticleName"), Category("ParticleEffect")]
		public string Name { get; set; }
		
		[ParticleDescription("TransmitterProperties"), ParticleDisplayName("EmitterGroup"),Editor(typeof(EmitterTypeEditor), typeof(UITypeEditor)),Category("ParticleEffect")]
		public List<ParticleEmitter> Emitters { get; set; }
		[ParticleDisplayName("ActivatedParticle"), Category("ParticleEffect")]
		public int ActiveParticles => Emitters.Sum(t => t.ActiveParticles);

		public void FastForward(Vector2 position, float seconds, float triggerPeriod)
		{
			var time = 0f;
			while (time < seconds)
			{
				Update(triggerPeriod);
				Trigger(position);
				time += triggerPeriod;
			}
		}

		public static ParticleEffect FromFile(ITextureRegionService textureRegionService, string path)
		{
			using (var stream = TitleContainer.OpenStream(path))
			{
				return FromStream(textureRegionService, stream);
			}
		}

		public static ParticleEffect FromStream(ITextureRegionService textureRegionService, Stream stream)
		{
			var serializer = new ParticleJsonSerializer(textureRegionService);

			using (var streamReader = new StreamReader(stream))
			using (var jsonReader = new JsonTextReader(streamReader))
			{
				return serializer.Deserialize<ParticleEffect>(jsonReader);
			}
		}

		public void Update(float elapsedSeconds)
		{
			for (var i = 0; i < Emitters.Count; i++)
				Emitters[i].Update(elapsedSeconds, Position);
		}

		public void Trigger()
		{
			Trigger(Position);
		}

		public void Trigger(Vector2 position, float layerDepth = 0)
		{
			for (var i = 0; i < Emitters.Count; i++)
				Emitters[i].Trigger(position, layerDepth);
		}

		public void Trigger(LineSegment line)
		{
			for (var i = 0; i < Emitters.Count; i++)
				Emitters[i].Trigger(line);
		}

		public override string ToString()
		{
			return Name;
		}
	}
}
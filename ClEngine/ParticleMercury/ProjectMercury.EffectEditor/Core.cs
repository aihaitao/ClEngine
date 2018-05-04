/*  
 Copyright © 2010 Project Mercury Team Members (http://mpe.codeplex.com/People/ProjectPeople.aspx)

 This program is licensed under the Microsoft Permissive License (Ms-PL).  You should 
 have received a copy of the license along with the source code.  If not, an online copy
 of the license can be found at http://mpe.codeplex.com/license.
*/

namespace ProjectMercury.EffectEditor
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.Composition;
    using System.ComponentModel.Composition.Hosting;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Windows.Forms;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using IO;
    using PluginInterfaces;
    using Emitters;
    using Modifiers;
    using Renderers;

    internal class Core : ApplicationContext
    {
        private CompositionContainer CompositionContainer { get; set; }

        private IEnumerable<IEmitterPlugin> _emitterPlugins;

        /// <summary>
        /// Gets or sets the copy plugins.
        /// </summary>
        /// <value>The copy plugins.</value>
        [ImportMany(typeof(IEmitterPlugin), AllowRecomposition = true)]
        public IEnumerable<IEmitterPlugin> EmitterPlugins
        {
            get
            {
                return _emitterPlugins;
            }
            private set
            {
                _emitterPlugins = value;

                if (EmitterPlugins != null)
                {
                    if (!EmitterPlugins.Any())
                        throw new Exception("找不到任何 Emitter 插件!");

                    Trace.WriteLine("找到 " + EmitterPlugins.Count() + " emitter 插件...", "CORE");

                    using (new TraceIndenter())
                    {
                        EmitterPlugins.ToList().ForEach(p => Trace.WriteLine("插件: " + p.Name));
                    }
                }
            }
        }

        private IEnumerable<IModifierPlugin> _modifierPlugins;

        /// <summary>
        /// Gets or sets the modifier plugins.
        /// </summary>
        /// <value>The modifier plugins.</value>
        [ImportMany(typeof(IModifierPlugin), AllowRecomposition = true)]
        public IEnumerable<IModifierPlugin> ModifierPlugins
        {
            get
            {
                return _modifierPlugins;
            }
            set
            {
                _modifierPlugins = value;

                if (ModifierPlugins != null)
                {
                    if (ModifierPlugins.Count() == 0)
                        throw new Exception("找不到任何 modifier 插件!");

                    Trace.WriteLine("找到 " + ModifierPlugins.Count() + " modifier 插件...", "CORE");

                    using (new TraceIndenter())
                    {
                        ModifierPlugins.ToList().ForEach(p => Trace.WriteLine("插件: " + p.Name));
                    }
                }
            }
        }

        private IEnumerable<IEffectSerializationPlugin> _serializationPlugins;

        /// <summary>
        /// Gets or sets the serialization plugins.
        /// </summary>
        /// <value>The serialization plugins.</value>
        [ImportMany(typeof(IEffectSerializationPlugin), AllowRecomposition = true)]
        public IEnumerable<IEffectSerializationPlugin> SerializationPlugins
        {
            get
            {
                return _serializationPlugins;
            }
            set
            {
                _serializationPlugins = value;

                if (SerializationPlugins != null)
                {
                    if (SerializationPlugins.Count() == 0)
                        throw new Exception("找不到任何 serialization 插件!");

                    Trace.WriteLine("找到 " + SerializationPlugins.Count() + " serialization 插件...", "CORE");

                    using (new TraceIndenter())
                    {
                        SerializationPlugins.ToList().ForEach(p => Trace.WriteLine("插件: " + p.Name));
                    }
                }
            }
        }

        private IInterfaceProvider _interface;

        /// <summary>
        /// Gets or sets the user interface.
        /// </summary>
        /// <value>The user interface.</value>
        [Import(typeof(IInterfaceProvider))]
        private IInterfaceProvider Interface
        {
            get { return _interface; }
            set
            {
                _interface = value;

                if (Interface != null)
                    Trace.WriteLine("InterfaceProvider已加载: " + value.GetType().AssemblyQualifiedName, "CORE");
            }
        }

        /// <summary>
        /// Gets or sets the timer object which measures time between app idle events.
        /// </summary>
        private Stopwatch TickTimer { get; set; }

        /// <summary>
        /// Gets or sets the asynchronous update manager.
        /// </summary>
        private AsyncUpdateManager UpdateManager { get; set; }

        /// <summary>
        /// Gets or sets the particle effect renderer.
        /// </summary>
        /// <value>The particle effect renderer.</value>
        private Renderer ParticleEffectRenderer { get; set; }

        /// <summary>
        /// Gets or sets the default texture to use when rendering Particles.
        /// </summary>
        private Texture2D DefaultParticleTexture { get; set; }

        /// <summary>
        /// Gets or sets the ParticleEffect which is being designed.
        /// </summary>
        private ParticleEffect ParticleEffect { get; set; }

        /// <summary>
        /// Gets or sets the list of TextureReferences.
        /// </summary>
        private List<TextureReference> TextureReferences { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Core"/> class.
        /// </summary>
        public Core() : base()
        {
            Trace.WriteLine("程序核心初始化...", "CORE");

            Compose();

            if (Interface is Form)
                MainForm = Interface as Form;

            Interface.Ready += Interface_Ready;
            Interface.Serialize += Interface_Serialize;
            Interface.Deserialize += Interface_Deserialize;
            Interface.EmitterAdded +=Interface_EmitterAdded;
            Interface.EmitterCloned += Interface_EmitterCloned;
            Interface.EmitterRemoved += Interface_EmitterRemoved;
            Interface.ModifierAdded += Interface_ModifierAdded;
            Interface.ModifierCloned += Interface_ModifierCloned;
            Interface.ModifierRemoved += Interface_ModifierRemoved;
            Interface.EmitterReinitialised += Interface_EmitterReinitialised;
            Interface.TextureReferenceAdded += Interface_TextureReferenceAdded;
            Interface.TextureReferenceChanged += Interface_TextureReferenceChanged;
            Interface.NewParticleEffect += Interface_NewParticleEffect;
        }

        /// <summary>
        /// Releases the unmanaged resources used by the <see cref="T:System.Windows.Forms.ApplicationContext"/> and optionally releases the managed resources.
        /// </summary>
        /// <param name="disposing">true to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                Trace.WriteLine("释放程序核心...", "CORE");

                if (ParticleEffectRenderer != null)
                    ParticleEffectRenderer.Dispose();

                if (Interface != null)
                    Interface.Dispose();

                if (CompositionContainer != null)
                    CompositionContainer.Dispose();
            }

            base.Dispose(disposing);
        }

        /// <summary>
        /// Composes this instance.
        /// </summary>
        private void Compose()
        {
            Trace.WriteLine("构建 IOC 容器...", "CORE");

            try
            {
                using (var assemblyCatalog = new AssemblyCatalog(Assembly.GetExecutingAssembly()))
                {
                    DirectoryInfo pluginsDirectory = new DirectoryInfo(Path.Combine(Application.StartupPath, "Plugins"));

                    // Ensure that the plugins directory is not blocked, otherwise the plugins will not load...
                    pluginsDirectory.Unblock(true);

                    using (var pluginsCatalog = new DirectoryCatalog(pluginsDirectory.FullName))
                    {
                        Trace.WriteLine("发现插件程序集...", "CORE");

                        using (new TraceIndenter())
                        {
                            pluginsCatalog.LoadedFiles.ToList().ForEach(f => Trace.WriteLine("Assembly: " + f));
                        }

                        Trace.WriteLine("找到 exported 部分...", "CORE");

                        using (new TraceIndenter())
                        {
                            foreach(var part in pluginsCatalog.Parts)
                            {
                                Trace.WriteLine("Part:" + part);

                                using (new TraceIndenter())
                                {
                                    part.ExportDefinitions.ToList().ForEach(e => Trace.WriteLine("Implementing: " + e.ContractName));
                                }
                            }
                        }

                        using (var catalog = new AggregateCatalog(assemblyCatalog, pluginsCatalog))
                        {
                            CompositionContainer = new CompositionContainer(catalog);

                            var batch = new CompositionBatch();

                            batch.AddPart(this);

                            CompositionContainer.Compose(batch);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Trace.TraceError(e.Message);

                throw;
            }
        }

        /// <summary>
        /// Executed when the interface provider is ready.
        /// </summary>
        private void Interface_Ready(object sender, EventArgs e)
        {
            Trace.WriteLine("用户界面已就绪...", "CORE");

            ParticleEffectRenderer     = InstantiateRenderer();
            DefaultParticleTexture     = LoadDefaultParticleTexture();
            UpdateManager              = new AsyncUpdateManager();
            ParticleEffect             = InstantiateDefaultParticleEffect();
            TextureReferences          = LoadDefaultTextureReferences();

            Interface.SetEmitterPlugins(EmitterPlugins.OrderBy(p => p.DisplayName));
            Interface.SetModifierPlugins(ModifierPlugins.OrderBy(p => p.DisplayName));
            Interface.SetSerializationPlugins(SerializationPlugins.OrderBy(p => p.DisplayName));

            Interface.TextureReferences = TextureReferences;
            Interface.SetParticleEffect(ParticleEffect);

            UpdateManager.Start();

            Application.Idle += Tick;
        }

        /// <summary>
        /// Instantiates the renderer.
        /// </summary>
        private Renderer InstantiateRenderer()
        {
            Trace.WriteLine("实例化粒子渲染器...", "CORE");

            Renderer renderer = new SpriteBatchRenderer
            {
                GraphicsDeviceService = GraphicsDeviceService.Instance
            };

            renderer.LoadContent(null);

            return renderer;
        }

        /// <summary>
        /// Loads the default particle texture.
        /// </summary>
        private Texture2D LoadDefaultParticleTexture()
        {
            Trace.WriteLine("加载默认的粒子纹理...", "CORE");

            using (FileStream inputStream = File.OpenRead("Textures\\FlowerBurst.png"))
            {
                return Texture2D.FromStream(GraphicsDeviceService.Instance.GraphicsDevice, inputStream);
            }
        }

        /// <summary>
        /// Instantiates the default particle effect.
        /// </summary>
        private ParticleEffect InstantiateDefaultParticleEffect()
        {
            Trace.WriteLine("实例化默认粒子效果...", "CORE");

            ParticleEffect effect = new ParticleEffect
            {
                new Emitter
                {
                    Budget                   = 5000,
                    Enabled                  = true,
                    MinimumTriggerPeriod     = 0f,
                    Name                     = "基础发射器",
                    ParticleTexture          = DefaultParticleTexture,
                    ParticleTextureAssetName = "FlowerBurst",
                    ReleaseColour            = Color.White.ToVector3(),
                    ReleaseOpacity           = 1f,
                    ReleaseQuantity          = 10,
                    ReleaseScale             = new VariableFloat { Value = 32f, Variation = 16f },
                    ReleaseSpeed             = new VariableFloat { Value = 25f, Variation = 25f },
                    Term                     = 1f,
                    Modifiers                = new ModifierCollection(),
                },
            };

            effect.Initialise();

            return effect;
        }

        /// <summary>
        /// Loads the default texture references.
        /// </summary>
        private List<TextureReference> LoadDefaultTextureReferences()
        {
            Trace.WriteLine("加载默认的纹理...", "CORE");

            List<TextureReference> references = new List<TextureReference>();

            DirectoryInfo texturesDirectory = new DirectoryInfo("Textures");

            if (texturesDirectory.Exists)
            {
                foreach (FileInfo file in texturesDirectory.GetFiles())
                {
                    if (file.Extension == ".bmp" || file.Extension == ".jpg" || file.Extension == ".png")
                    {
                        try
                        {
                            TextureReference reference = new TextureReference(file.FullName);

                            references.Add(reference);

                            using (new TraceIndenter())
                            {
                                Trace.WriteLine("文件: " + file.FullName);
                            }
                        }
                        catch (Exception e)
                        {
                            Trace.TraceError(e.Message);

                            continue;
                        }
                    }
                }
            }

            return references;
        }

        /// <summary>
        /// Handles the Application Idle event.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void Tick(object sender, EventArgs e)
        {
            float elapsedSeconds = TickTimer != null ? (float)TickTimer.Elapsed.TotalSeconds : 1f / 60f;

            if (TickTimer == null)
                TickTimer = new Stopwatch();

            float x, y;

            if (Interface.TriggerRequired(out x, out y))
                ParticleEffect.Trigger(new Vector2 { X = x, Y = y });

            Stopwatch updateTimer = Stopwatch.StartNew();

            UpdateManager.BeginUpdate(elapsedSeconds, ParticleEffect);
            UpdateManager.EndUpdate();

            Interface.SetUpdateTime((float)updateTimer.Elapsed.TotalSeconds);

            Interface.Draw(ParticleEffect, ParticleEffectRenderer);

            TickTimer.Reset();
            TickTimer.Start();
        }

        /// <summary>
        /// Handles the Serialize event of the Interface.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="ProjectMercury.EffectEditor.SerializeEventArgs"/> instance containing the event data.</param>
        private void Interface_Serialize(object sender, SerializeEventArgs e)
        {
            Trace.WriteLine("序列化粒子效果 " + e.FilePath, "CORE");
            
            using (new TraceIndenter())
            {
                Trace.WriteLine("使用插件: " + e.Plugin.Name);
            }

            try
            {
                e.Plugin.Serialize(ParticleEffect, e.FilePath);

                e.Result = CoreOperationResult.OK;
            }
            catch (Exception error)
            {
                e.Result = new CoreOperationResult(error);
            }
        }

        /// <summary>
        /// Handles the Deserialize event of the Interface.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="ProjectMercury.EffectEditor.SerializeEventArgs"/> instance containing the event data.</param>
        private void Interface_Deserialize(object sender, SerializeEventArgs e)
        {
            Trace.WriteLine("反序列化粒子效果 " + e.FilePath, "CORE");
            
            using (new TraceIndenter())
            {
                Trace.WriteLine("使用插件: " + e.Plugin.Name);
            }

            try
            {
                ParticleEffect = e.Plugin.Deserialize(e.FilePath);

                ParticleEffect.Initialise();

                foreach (Emitter emitter in ParticleEffect)
                {
                    if (String.IsNullOrEmpty(emitter.ParticleTextureAssetName))
                    {
                        emitter.ParticleTexture = DefaultParticleTexture;
                    }
                    else
                    {
                        bool textureFound = false;

                        foreach (TextureReference reference in TextureReferences)
                        {
                            if (reference.GetAssetName() == emitter.ParticleTextureAssetName)
                            {
                                emitter.ParticleTexture = reference.Texture;

                                textureFound = true;

                                break;
                            }
                        }

                        if (!textureFound)
                        {
                            MessageBox.Show(@"无法找到纹理资源 '" + emitter.ParticleTextureAssetName + @"'. " +
								@"使用默认的粒子纹理...", @"资源未找到", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

                            emitter.ParticleTexture = DefaultParticleTexture;
                        }
                    }
                }

                Interface.SetParticleEffect(ParticleEffect);

                e.Result = CoreOperationResult.OK;
            }
            catch (Exception error)
            {
                e.Result = new CoreOperationResult(error);
            }
        }

        /// <summary>
        /// Handles the EmitterAdded event of the Interface.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="ProjectMercury.EffectEditor.NewEmitterEventArgs"/> instance containing the event data.</param>
        public void Interface_EmitterAdded(object sender, NewEmitterEventArgs e)
        {
            Trace.WriteLine("新增发射器...", "CORE");

            using (new TraceIndenter())
            {
                Trace.WriteLine("使用插件: " + e.Plugin.Name);
            }

            try
            {
                Emitter emitter = e.Plugin.CreateDefaultInstance();

                emitter.Initialise(e.Budget, e.Term);

                emitter.ParticleTexture = DefaultParticleTexture;

                ParticleEffect.Add(emitter);

                e.AddedEmitter = emitter;

                e.Result = CoreOperationResult.OK;
            }
            catch (Exception error)
            {
                e.Result = new CoreOperationResult(error);
            }
        }

        /// <summary>
        /// Handles the EmitterCloned event of the Interface.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="ProjectMercury.EffectEditor.CloneEmitterEventArgs"/> instance containing the event data.</param>
        public void Interface_EmitterCloned(object sender, CloneEmitterEventArgs e)
        {
            Trace.WriteLine("复制发射器...", "CORE");

            try
            {
                Emitter clone = e.Prototype.DeepCopy();

                clone.Initialise();

                clone.ParticleTexture = e.Prototype.ParticleTexture;

                ParticleEffect.Add(clone);

                e.AddedEmitter = clone;

                e.Result = CoreOperationResult.OK;
            }
            catch (Exception error)
            {
                e.Result = new CoreOperationResult(error);
            }
        }

        /// <summary>
        /// Handles the ModifierAdded event of the Interface.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="ProjectMercury.EffectEditor.NewModifierEventArgs"/> instance containing the event data.</param>
        public void Interface_ModifierAdded(object sender, NewModifierEventArgs e)
        {
            Trace.WriteLine("新增修改器...", "CORE");

            using (new TraceIndenter())
            {
                Trace.WriteLine("使用插件: " + e.Plugin.Name);
            }

            try
            {
                foreach (Emitter emitter in ParticleEffect)
                {
                    if (ReferenceEquals(emitter, e.ParentEmitter))
                    {
                        Modifier modifier = e.Plugin.CreateDefaultInstance();

                        emitter.Modifiers.Add(modifier);

                        e.AddedModifier = modifier;

                        e.Result = CoreOperationResult.OK;

                        return;
                    }
                }

                e.Result = new CoreOperationResult(new Exception("找不到指定的发射器."));
            }
            catch (Exception error)
            {
                e.Result = new CoreOperationResult(error);
            }
        }

        /// <summary>
        /// Handles the ModifierCloned event of the Interface.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="ProjectMercury.EffectEditor.CloneModifierEventArgs"/> instance containing the event data.</param>
        public void Interface_ModifierCloned(object sender, CloneModifierEventArgs e)
        {
            Trace.WriteLine("复制发射器...", "CORE");

            try
            {
                Modifier clone = e.Prototype.DeepCopy();

                foreach (Emitter emitter in ParticleEffect)
                {
                    foreach (Modifier modifier in emitter.Modifiers)
                    {
                        if (ReferenceEquals(modifier, e.Prototype))
                        {
                            emitter.Modifiers.Add(clone);

                            e.AddedModifier = clone;

                            e.Result = CoreOperationResult.OK;

                            return;
                        }
                    }
                }

                e.Result = new CoreOperationResult(new Exception("在效果层次结构中找不到修饰符原型."));
            }
            catch (Exception error)
            {
                e.Result = new CoreOperationResult(error);
            }
        }

        /// <summary>
        /// Handles the EmitterRemoved event of the Interface.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="ProjectMercury.EffectEditor.EmitterEventArgs"/> instance containing the event data.</param>
        public void Interface_EmitterRemoved(object sender, EmitterEventArgs e)
        {
            Trace.WriteLine("移除发射器...", "CORE");

            try
            {
                foreach (Emitter emitter in ParticleEffect)
                {
                    if (ReferenceEquals(emitter, e.Emitter))
                    {
                        ParticleEffect.Remove(e.Emitter);

                        e.Result = CoreOperationResult.OK;

                        return;
                    }
                }

                e.Result = new CoreOperationResult(new Exception("找不到指定的发射器."));
            }
            catch (Exception error)
            {
                e.Result = new CoreOperationResult(error);
            }
        }

        /// <summary>
        /// Handles the ModifierRemoved event of the Interface.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="ProjectMercury.EffectEditor.ModifierEventArgs"/> instance containing the event data.</param>
        public void Interface_ModifierRemoved(object sender, ModifierEventArgs e)
        {
            Trace.WriteLine("移除发射器...", "CORE");

            try
            {
                foreach (Emitter emitter in ParticleEffect)
                {
                    foreach (Modifier modifier in emitter.Modifiers)
                    {
                        if (ReferenceEquals(modifier, e.Modifier))
                        {
                            emitter.Modifiers.Remove(e.Modifier);

                            e.Result = CoreOperationResult.OK;

                            return;
                        }
                    }
                }

                e.Result = new CoreOperationResult(new Exception("找不到指定的发射器."));
            }
            catch (Exception error)
            {
                e.Result = new CoreOperationResult(error);
            }
        }

        /// <summary>
        /// Handles the EmitterReinitialised event of the Interface.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="ProjectMercury.EffectEditor.EmitterReinitialisedEventArgs"/> instance containing the event data.</param>
        public void Interface_EmitterReinitialised(object sender, EmitterReinitialisedEventArgs e)
        {
            Trace.WriteLine("重新初始化发射器...", "CORE");

            try
            {
                e.Emitter.Initialise(e.Budget, e.Term);

                e.Result = CoreOperationResult.OK;
            }
            catch (Exception error)
            {
                e.Result = new CoreOperationResult(error);
            }
        }

        /// <summary>
        /// Handles the TextureReferenceAdded event of the Interface.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="ProjectMercury.EffectEditor.NewTextureReferenceEventArgs"/> instance containing the event data.</param>
        private void Interface_TextureReferenceAdded(object sender, NewTextureReferenceEventArgs e)
        {
            Trace.WriteLine("添加新的纹理...", "CORE");

            using (new TraceIndenter())
            {
                Trace.WriteLine("文件路径: " + e.FilePath);
            }

            try
            {
                TextureReference reference = new TextureReference(e.FilePath);

                e.AddedTextureReference = reference;

                TextureReferences.Add(reference);

                e.Result = CoreOperationResult.OK;
            }
            catch (Exception error)
            {
                e.Result = new CoreOperationResult(error);
            }
        }

        /// <summary>
        /// Handles the TextureReferenceChanged event of the Interface.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="ProjectMercury.EffectEditor.TextureReferenceChangedEventArgs"/> instance containing the event data.</param>
        private void Interface_TextureReferenceChanged(object sender, TextureReferenceChangedEventArgs e)
        {
            try
            {
                e.Emitter.ParticleTexture = e.TextureReference.Texture;

                e.Emitter.ParticleTextureAssetName = e.TextureReference.GetAssetName();

                e.Result = CoreOperationResult.OK;
            }
            catch (Exception error)
            {
                e.Result = new CoreOperationResult(error);
            }
        }

        /// <summary>
        /// Handles the NewParticleEffect event of the Interface.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void Interface_NewParticleEffect(object sender, EventArgs e)
        {
            ParticleEffect = InstantiateDefaultParticleEffect();

            Interface.SetParticleEffect(ParticleEffect);
        }
    }
}
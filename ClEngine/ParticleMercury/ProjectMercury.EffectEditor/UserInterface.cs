/*  
 Copyright © 2009 Project Mercury Team Members (http://mpe.codeplex.com/People/ProjectPeople.aspx)

 This program is licensed under the Microsoft Permissive License (Ms-PL).  You should 
 have received a copy of the license along with the source code.  If not, an online copy
 of the license can be found at http://mpe.codeplex.com/license.
*/

using ProjectMercury.EffectEditor.Properties;

namespace ProjectMercury.EffectEditor
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.Composition;
    using System.Diagnostics;
    using System.Drawing;
    using System.IO;
    using System.Linq;
    using System.Threading;
    using System.Windows.Forms;
    using PluginInterfaces;
    using TreeNodes;
    using Emitters;
    using Modifiers;
    using Renderers;

	/// <inheritdoc>
	///     <cref></cref>
	/// </inheritdoc>
	[Export(typeof(IInterfaceProvider))]
	// ReSharper disable once ClassWithVirtualMembersNeverInherited.Global
	internal partial class UserInterface : Form, IInterfaceProvider
    {
	    // ReSharper disable once UnusedMember.Local
	    private readonly object _padLock = new object();

        public event EventHandler Ready;

        protected virtual void OnReady(EventArgs e)
        {
            Trace.WriteLine("用户界面准备就绪...", "UI");

            var handler = Interlocked.CompareExchange(ref Ready, null, null);

	        handler?.Invoke(this, e);
        }

        public event SerializeEventHandler Serialize;

        protected virtual void OnSerialize(SerializeEventArgs e)
        {
            Trace.WriteLine("用户请求序列化粒子效果...", "UI");

            using (new TraceIndenter())
            {
                Trace.WriteLine("Filepath: " + e.FilePath);
            }

            using (new HourglassCursor())
            {
                var handler = Interlocked.CompareExchange(ref Serialize, null, null);

	            handler?.Invoke(this, e);
            }

            AssertOperationOk(e.Result);
        }

        public event SerializeEventHandler Deserialize;

        protected virtual void OnDeserialize(SerializeEventArgs e)
        {
            Trace.WriteLine("用户请求反序列化粒子效果...", "UI");

            using (new TraceIndenter())
            {
                Trace.WriteLine("Filepath:" + e.FilePath);
            }

            using (new HourglassCursor())
            {
                var handler = Interlocked.CompareExchange(ref Deserialize, null, null);

	            handler?.Invoke(this, e);
            }

            AssertOperationOk(e.Result);
        }

        public event NewEmitterEventHandler EmitterAdded;

        protected virtual void OnEmitterAdded(NewEmitterEventArgs e)
        {
            Trace.WriteLine("用户请求添加发射器...", "UI");

            using (new HourglassCursor())
            {
                var handler = Interlocked.CompareExchange(ref EmitterAdded, null, null);

	            handler?.Invoke(this, e);
            }

            AssertOperationOk(e.Result);
        }

        public event CloneEmitterEventHandler EmitterCloned;

        protected virtual void OnEmitterCloned(CloneEmitterEventArgs e)
        {
            Trace.WriteLine("用户请求复制发射器...", "UI");

            using (new TraceIndenter())
            {
                Trace.WriteLine("Prototype: " + e.Prototype.Name);
            }

            using (new HourglassCursor())
            {
                var handler = Interlocked.CompareExchange(ref EmitterCloned, null, null);

	            handler?.Invoke(this, e);
            }

            AssertOperationOk(e.Result);
        }

        public event EmitterEventHandler EmitterRemoved;

        protected virtual void OnEmitterRemoved(EmitterEventArgs e)
        {
            Trace.WriteLine("用户请求移除发射器...", "UI");

            using (new TraceIndenter())
            {
                Trace.WriteLine("Emitter: " + e.Emitter.Name);
            }

            using (new HourglassCursor())
            {
                var handler = Interlocked.CompareExchange(ref EmitterRemoved, null, null);

	            handler?.Invoke(this, e);
            }

            AssertOperationOk(e.Result);
        }

        public event NewModifierEventHandler ModifierAdded;

        protected virtual void OnModifierAdded(NewModifierEventArgs e)
        {
            Trace.WriteLine("用户请求添加修改器...", "UI");

            using (new TraceIndenter())
            {
                Trace.WriteLine("Emitter: " + e.ParentEmitter.Name);
            }

            using (new HourglassCursor())
            {
                var handler = Interlocked.CompareExchange(ref ModifierAdded, null, null);

	            handler?.Invoke(this, e);
            }

            AssertOperationOk(e.Result);
        }

        public event CloneModifierEventHandler ModifierCloned;

        protected virtual void OnModifierCloned(CloneModifierEventArgs e)
        {
            Trace.WriteLine("用户请求复制修改器...", "UI");

            using (new HourglassCursor())
            {
                var handler = Interlocked.CompareExchange(ref ModifierCloned, null, null);

	            handler?.Invoke(this, e);
            }

            AssertOperationOk(e.Result);
        }

        public event ModifierEventHandler ModifierRemoved;

        protected virtual void OnModifierRemoved(ModifierEventArgs e)
        {
            Trace.WriteLine("用户请求删除修改器...");

            using (new HourglassCursor())
            {
                var handler = Interlocked.CompareExchange(ref ModifierRemoved, null, null);

	            handler?.Invoke(this, e);
            }

            AssertOperationOk(e.Result);
        }

        public event EmitterReinitialisedEventHandler EmitterReinitialised;

        protected virtual void OnEmitterReinitialised(EmitterReinitialisedEventArgs e)
        {
            Trace.WriteLine("用户请求重新初始化发射器...", "UI");

            using (new TraceIndenter())
            {
                Trace.WriteLine("预计: " + e.Budget);
                Trace.WriteLine("预期: " + e.Term);
            }

            using (new HourglassCursor())
            {
                var handler = Interlocked.CompareExchange(ref EmitterReinitialised, null, null);

	            handler?.Invoke(this, e);
            }

            AssertOperationOk(e.Result);
        }

        public event NewTextureReferenceEventHandler TextureReferenceAdded;

	    public event TextureReferenceChangedEventHandler TextureReferenceChanged;

        protected virtual void OnTextureReferenceChanged(TextureReferenceChangedEventArgs e)
        {
            Trace.WriteLine("用户请求将纹理参考分配给发射器...", "UI");

            using (new TraceIndenter())
            {
                Trace.WriteLine("发射器: " + e.Emitter.Name);
                Trace.WriteLine("纹理: " + e.TextureReference.FilePath);
            }

            using (new HourglassCursor())
            {
                var handler = Interlocked.CompareExchange(ref TextureReferenceChanged, null, null);

	            handler?.Invoke(this, e);
            }

            AssertOperationOk(e.Result);
        }

        public event EventHandler NewParticleEffect;

        protected virtual void OnNewParticleEffect(EventArgs e)
        {
            Trace.WriteLine("用户请求新建粒子效果...", "UI");

            using (new HourglassCursor())
            {
                var handler = Interlocked.CompareExchange(ref NewParticleEffect, null, null);

	            handler?.Invoke(this, e);
            }
        }

        public IEnumerable<TextureReference> TextureReferences { get; set; }

        public void SetEmitterPlugins(IEnumerable<IEmitterPlugin> plugins)
        {
            foreach (IEmitterPlugin plugin in plugins)
                AddEmitterPlugin(plugin);
        }

        public void SetModifierPlugins(IEnumerable<IModifierPlugin> plugins)
        {
            foreach (IModifierPlugin plugin in plugins)
                AddModifierPlugin(plugin);
        }

        public void SetSerializationPlugins(IEnumerable<IEffectSerializationPlugin> plugins)
        {
            foreach (IEffectSerializationPlugin plugin in plugins)
                AddSerializationPlugin(plugin);
        }

	    /// <summary>
	    /// Initializes a new instance of the <see>
	    ///         <cref>EffectEditorWindow</cref>
	    ///     </see>
	    ///     class.
	    /// </summary>
	    public UserInterface()
        {
            InitializeComponent();

            TriggerTimer = new Stopwatch();
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Form.Load"/> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"/> that contains the event data.</param>
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            OnReady(EventArgs.Empty);

            DisplayLibraryEffects();
        }

        private bool _mouseButtonPressed;

        private Point _localMousePosition;

	    private Color _previewBackgroundColor = Color.Black;

        /// <summary>
        /// Displays the library effects.
        /// </summary>
        private void DisplayLibraryEffects()
        {
            Trace.WriteLine("搜索库粒子效果...", "UI");

            DirectoryInfo effectsDir = new DirectoryInfo(Application.StartupPath + "\\EffectLibrary");

            foreach (FileInfo file in effectsDir.GetFiles())
            {
                using (new TraceIndenter())
                {
                    Trace.WriteLine("文件: " + file.FullName);
                }

                bool pluginFound = false;

                foreach (IEffectSerializationPlugin plugin in (from ToolStripItem menuItem in uxImportMenuItem.DropDownItems
                                                               where menuItem.Tag is IEffectSerializationPlugin
                                                               select menuItem.Tag as IEffectSerializationPlugin))
                {
                    if (plugin.FileFilter.Contains(file.Extension))
                    {
                        uxLibraryImageList.Images.Add(file.Name, plugin.DisplayIcon);

                        ListViewItem item = new ListViewItem
                        {
                            Text = file.Name,
                            Tag = plugin,
                            ImageIndex = uxLibraryImageList.Images.IndexOfKey(file.Name),
                            StateImageIndex = uxLibraryImageList.Images.IndexOfKey(file.Name)
                        };

                        uxLibraryListView.Items.Add(item);

                        pluginFound = true;

                        break;
                    }
                }

                if (!pluginFound)
                    Trace.TraceWarning("找不到效果库文件的序列化插件: " + file.FullName);
            }
        }

        /// <summary>
        /// Handles the Click event of the uxMainMenuToggleEffectTree control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void uxMainMenuToggleEffectTree_Click(object sender, EventArgs e)
        {
            ThreadPool.QueueUserWorkItem(delegate
            {
                Invoke((MethodInvoker)delegate
                {
                    uxOuterSplitContainer.Panel1Collapsed = !uxMainMenuToggleEffectTree.Checked;
                });
            });
        }

        /// <summary>
        /// Handles the Click event of the uxMainMenuTogglePropertyBrowser control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void uxMainMenuTogglePropertyBrowser_Click(object sender, EventArgs e)
        {
            ThreadPool.QueueUserWorkItem(delegate
            {
                Invoke((MethodInvoker)delegate
                {
                    uxInnerSplitContainer.Panel2Collapsed = !uxMainMenuTogglePropertyBrowser.Checked;
                });
            });
        }

        /// <summary>
        /// Handles the AfterSelect event of the uxEffectTree control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Forms.TreeViewEventArgs"/> instance containing the event data.</param>
        private void uxEffectTree_AfterSelect(object sender, TreeViewEventArgs e)
        {
            uxPropertyBrowser.SelectedObject = e.Node.Tag;
        }

	    /// <summary>
        /// Handles the Click event of the uxMainMenuOptions control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void uxMainMenuOptions_Click(object sender, EventArgs e)
        {
            Trace.WriteLine("用户请求选项窗口...", "UI");

            using (OptionsWindow options = new OptionsWindow
                {
                    BackgroundColor = _previewBackgroundColor,
                    BackgroundColourPickedCallback = delegate(Color color)
                    {
	                    Trace.WriteLine("用户更改背景颜色...", "UI");

	                    _previewBackgroundColor = color;

	                    uxEffectPreview.SetBackgroundColor(color.R, color.G, color.B);
                    },
                    BackgroundImagePickedCallback = delegate(string filePath)
                    {
	                    Trace.WriteLine("用户更改背景图片...", "UI");

	                    uxEffectPreview.LoadBackgroundImage(filePath);
                    },
                    BackgroundImageClearedCallback = delegate
                    {
	                    Trace.WriteLine("用户清除背景图像...", "UI");

	                    uxEffectPreview.ClearBackgroundImage();
                    },
                    BackgroundImageOptionsCallback = delegate(ImageOptions imageOptions)
                    {
	                    Trace.WriteLine("用户更改背景图片布局...", "UI");

	                    uxEffectPreview.ImageOptionsChanged(imageOptions);
                    },
                })
            {
                options.ShowDialog();
            }
        }

        /// <summary>
        /// Handles the KeyUp event of the uxEffectTree control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Forms.KeyEventArgs"/> instance containing the event data.</param>
        private void uxEffectTree_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                if (uxEffectTree.SelectedNode is EmitterTreeNode node)
                {
                    Emitter emitter = node.Emitter;

                    OnEmitterRemoved(new EmitterEventArgs(emitter));
                }

                if (uxEffectTree.SelectedNode is ModifierTreeNode treeNode)
                {
                    Modifier modifier = treeNode.Modifier;

                    OnModifierRemoved(new ModifierEventArgs(modifier));
                }

                uxEffectTree.SelectedNode.Remove();
            }
        }

        /// <summary>
        /// Handles the Click event of the uxMainMenuExit control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void uxMainMenuExit_Click(object sender, EventArgs e)
        {
            Trace.WriteLine("用户请求退出程序...", "UI");

            Application.Exit();
        }

        /// <summary>
        /// Adds the copy plugin to the interface.
        /// </summary>
        /// <param name="plugin">The plugin.</param>
        private void AddEmitterPlugin(IEmitterPlugin plugin)
        {
            Trace.WriteLine("添加菜单项 '" + plugin.Name + "' 插件...", "UI");

            ToolStripMenuItem item = new ToolStripMenuItem
            {
                Text = plugin.DisplayName,
                ToolTipText = plugin.Description,
                Image = plugin.DisplayIcon.ToBitmap(),
                Tag = plugin
            };

            uxAddEmitterMenuItem.DropDownItems.Add(item);
        }

        /// <summary>
        /// Adds the modifier plugin to the interface.
        /// </summary>
        /// <param name="plugin">The plugin.</param>
        private void AddModifierPlugin(IModifierPlugin plugin)
        {
            Trace.WriteLine("添加菜单项 '" + plugin.Name + "' 插件...", "UI");

            ToolStripMenuItem item = new ToolStripMenuItem
            {
                Text = plugin.DisplayName,
                ToolTipText = plugin.Description,
                Image = plugin.DisplayIcon.ToBitmap(),
                Tag = plugin
            };

            bool foundCategory = false;

            foreach (ToolStripMenuItem categoryItem in uxAddModifierMenuItem.DropDownItems)
            {
                if (categoryItem.Text.Equals(plugin.Category))
                {
                    categoryItem.DropDownItems.Add(item);

                    foundCategory = true;
                }
            }

            if (!foundCategory)
            {
                ToolStripMenuItem newCategoryItem = new ToolStripMenuItem(plugin.Category);

                newCategoryItem.DropDownItemClicked += uxAddModifierMenuItem_DropDownItemClicked;

                uxAddModifierMenuItem.DropDownItems.Add(newCategoryItem);

                newCategoryItem.DropDownItems.Add(item);
            }

            //this.uxAddModifierMenuItem.DropDownItems.Add(item);
        }

        /// <summary>
        /// Adds the serialization plugin to the interface.
        /// </summary>
        /// <param name="plugin">The plugin.</param>
        private void AddSerializationPlugin(IEffectSerializationPlugin plugin)
        {
            Trace.WriteLine("添加菜单项 '" + plugin.Name + "' 插件...", "UI");

            //TODO: Perhaps it would be better to load the plugins and don't add them to the uxImportMenuItem
            //Instead we have a simple import button, and it has the filetypes with the supported plugins.
            //Example: .em and .pe (or something similar) are the only visible types in the import dialog.
            //Doing it this way will prevent loading of dynamic menu items and fix the bug where the menu items is
            //still displayed even tho the load dialog has been displayed.
            if (plugin.DeserializeSupported)
            {
                ToolStripMenuItem item = new ToolStripMenuItem
                {
                    Image = plugin.DisplayIcon.ToBitmap(),
                    Text = plugin.DisplayName,
                    ToolTipText = plugin.Description,
                    Tag = plugin
                };

                uxImportMenuItem.DropDownItems.Add(item);
            }

            if (plugin.SerializeSuported)
            {
                ToolStripMenuItem item = new ToolStripMenuItem
                {
                    Image = plugin.DisplayIcon.ToBitmap(),
                    Text = plugin.DisplayName,
                    ToolTipText = plugin.Description,
                    Tag = plugin
                };

                uxExportMenuItem.DropDownItems.Add(item);
            }
        }

        /// <summary>
        /// Sets the particle effect.
        /// </summary>
        /// <param name="effect">The effect.</param>
        public void SetParticleEffect(ParticleEffect effect)
        {
            uxEffectTree.Nodes.Clear();

            ParticleEffectTreeNode node = new ParticleEffectTreeNode(effect);

            uxEffectTree.Nodes.Add(node);

            uxEffectTree.SelectedNode = node;

            node.Expand();
        }

        /// <summary>
        /// Sets the amount of time it took to update the particle effect.
        /// </summary>
        /// <param name="totalSeconds"></param>
        public void SetUpdateTime(float totalSeconds)
        {
            uxUpdateTimeLabel.Text = string.Format(Resources.UserInterface_SetUpdateTime_更新___0_F5__秒, totalSeconds);

            float framePercent = (totalSeconds / 0.016f) * 100f;

            framePercent = framePercent >= 100f ? 100f : framePercent;

            uxFrameRateProgress.Value = (int)framePercent;
        }

        /// <summary>
        /// Handles the DropDownItemClicked event of the uxImportMenuItem control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Forms.ToolStripItemClickedEventArgs"/> instance containing the event data.</param>
        private void uxImportMenuItem_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
	        if (e.ClickedItem.Tag is IEffectSerializationPlugin plugin)
	        {
		        uxImportEffectDialog.Filter = plugin.FileFilter;

		        if (uxImportEffectDialog.ShowDialog() == DialogResult.OK)
		        {
			        try
			        {
				        var filePath = uxImportEffectDialog.FileName;

				        var args = new SerializeEventArgs(plugin, filePath);

				        OnDeserialize(args);
			        }
			        catch (Exception ex)
			        {
				        MessageBox.Show(ex.Message);
			        }
		        }
	        }
        }

        /// <summary>
        /// Handles the DropDownItemClicked event of the uxExportMenuItem control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Forms.ToolStripItemClickedEventArgs"/> instance containing the event data.</param>
        private void uxExportMenuItem_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
	        if (e.ClickedItem.Tag is IEffectSerializationPlugin plugin)
	        {
		        uxExportEffectDialog.Filter = plugin.FileFilter;

		        if (uxExportEffectDialog.ShowDialog() == DialogResult.OK)
		        {
			        try
			        {
				        var filePath = uxExportEffectDialog.FileName;

				        var args = new SerializeEventArgs(plugin, filePath);

				        OnSerialize(args);
			        }
			        catch (Exception ex)
			        {
				        MessageBox.Show(ex.Message);
			        }
		        }
	        }
        }

        /// <summary>
        /// Draws the specified effect.
        /// </summary>
        /// <param name="effect">The effect.</param>
        /// <param name="renderer">The renderer.</param>
        public void Draw(ParticleEffect effect, Renderer renderer)
        {
            uxActiveParticlesLabel.Text = $@"{effect.ActiveParticlesCount} 活动粒子";

            uxEffectPreview.ParticleEffect = effect;
            uxEffectPreview.Renderer = renderer;

            uxEffectPreview.Invalidate();
        }

        /// <summary>
        /// Handles the MouseDown event of the uxEffectPreview control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Forms.MouseEventArgs"/> instance containing the event data.</param>
        private void uxEffectPreview_MouseDown(object sender, MouseEventArgs e)
        {
            _mouseButtonPressed = true;

            _localMousePosition = e.Location;

            uxStatusLabel.Text = _localMousePosition.ToString();

            TriggerTimer.Start();
        }

        /// <summary>
        /// Handles the MouseUp event of the uxEffectPreview control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Forms.MouseEventArgs"/> instance containing the event data.</param>
        private void uxEffectPreview_MouseUp(object sender, MouseEventArgs e)
        {
            _mouseButtonPressed = false;

            uxStatusLabel.Text = Resources.UserInterface_uxEffectPreview_MouseUp_准备就绪;

            TriggerTimer.Stop();
        }

        /// <summary>
        /// Handles the MouseMove event of the uxEffectPreview control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Forms.MouseEventArgs"/> instance containing the event data.</param>
        private void uxEffectPreview_MouseMove(object sender, MouseEventArgs e)
        {
            if (_mouseButtonPressed)
            {
                _localMousePosition = e.Location;

                uxStatusLabel.Text = _localMousePosition.ToString();
            }
        }

        private Stopwatch TriggerTimer { get; }

        /// <summary>
        /// Gets a value indicating wether or not a trigger is required.
        /// </summary>
        /// <param name="x">The x location of the trigger.</param>
        /// <param name="y">The y location of the trigger.</param>
        public bool TriggerRequired(out float x, out float y)
        {
            x = _localMousePosition.X;
            y = _localMousePosition.Y;

            if (_mouseButtonPressed)
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

        /// <summary>
        /// Handles the Opening event of the uxEffectTreeContextMenu control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.ComponentModel.CancelEventArgs"/> instance containing the event data.</param>
        private void uxEffectTreeContextMenu_Opening(object sender, CancelEventArgs e)
        {
            TreeNode selectedNode = uxEffectTree.SelectedNode;

            uxAddEmitterMenuItem.Visible = selectedNode is ParticleEffectTreeNode;
            uxAddModifierMenuItem.Visible = selectedNode is EmitterTreeNode;
            uxReinitialiseEmitterMenuItem.Visible = selectedNode is EmitterTreeNode;
            uxSelectTextureMenuItem.Visible = selectedNode is EmitterTreeNode;
            uxToggleEmitterEnabledMenuItem.Visible = selectedNode is EmitterTreeNode;

            uxEffectTreeContextMenuSeperator.Visible = (selectedNode is ParticleEffectTreeNode) == false;
            uxDeleteMenuItem.Visible = (selectedNode is ParticleEffectTreeNode) == false;
            uxCloneMenuItem.Visible = (selectedNode is ParticleEffectTreeNode) == false;

            if (selectedNode is EmitterTreeNode node)
            {
                Emitter emitter = node.Emitter;

                uxToggleEmitterEnabledMenuItem.Text = emitter.Enabled ? "禁用" : "启用";
            }
        }

        /// <summary>
        /// Handles the DropDownItemClicked event of the uxAddEmitterMenuItem control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Forms.ToolStripItemClickedEventArgs"/> instance containing the event data.</param>
        private void uxAddEmitterMenuItem_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            try
            {
                using (NewEmitterDialog dialog = new NewEmitterDialog())
                {
                    if (dialog.ShowDialog() == DialogResult.OK)
                    {
                        IEmitterPlugin plugin = e.ClickedItem.Tag as IEmitterPlugin;

                        var args = new NewEmitterEventArgs(plugin, dialog.EmitterBudget, dialog.EmitterTerm);

                        OnEmitterAdded(args);

                        if (args.AddedEmitter != null)
                        {
                            Emitter emitter = args.AddedEmitter;

                            EmitterTreeNode node = new EmitterTreeNode(emitter);

                            uxEffectTree.Nodes[0].Nodes.Add(node);

                            node.EnsureVisible();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// Handles the DropDownItemClicked event of the uxAddModifierMenuItem control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Forms.ToolStripItemClickedEventArgs"/> instance containing the event data.</param>
        private void uxAddModifierMenuItem_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            try
            {
                EmitterTreeNode parentNode = uxEffectTree.SelectedNode as EmitterTreeNode;

                IModifierPlugin plugin = e.ClickedItem.Tag as IModifierPlugin;

	            if (parentNode != null)
	            {
		            Emitter parent = parentNode.Emitter;

		            var args = new NewModifierEventArgs(parent, plugin);

		            OnModifierAdded(args);

		            if (args.AddedModifier != null)
		            {
			            Modifier modifier = args.AddedModifier;

			            ModifierTreeNode node = new ModifierTreeNode(modifier);

			            parentNode.Nodes.Add(node);

			            node.EnsureVisible();
		            }
	            }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// Handles the Click event of the uxCloneMenuItem control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void uxCloneMenuItem_Click(object sender, EventArgs e)
        {
            var selectedNode = uxEffectTree.SelectedNode;

            if (selectedNode is EmitterTreeNode node)
            {
                CloneEmitterEventArgs args = new CloneEmitterEventArgs(node.Emitter);

                OnEmitterCloned(args);

                if (args.AddedEmitter != null)
                {
                    EmitterTreeNode newNode = new EmitterTreeNode(args.AddedEmitter);

                    node.Parent.Nodes.Add(newNode);

                    newNode.Expand();

                    newNode.EnsureVisible();
                }
            }
            else if (selectedNode is ModifierTreeNode treeNode)
            {
                CloneModifierEventArgs args = new CloneModifierEventArgs(treeNode.Modifier);

                OnModifierCloned(args);

                if (args.AddedModifier != null)
                {
                    ModifierTreeNode newNode = new ModifierTreeNode(args.AddedModifier);

                    treeNode.Parent.Nodes.Add(newNode);

                    newNode.EnsureVisible();
                }
            }
        }

        /// <summary>
        /// Handles the Click event of the uxDeleteMenuItem control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void uxDeleteMenuItem_Click(object sender, EventArgs e)
        {
            var selectedNode = uxEffectTree.SelectedNode;

            if (selectedNode is EmitterTreeNode node)
            {
                try
                {
                    Emitter emitter = node.Emitter;

                    OnEmitterRemoved(new EmitterEventArgs(emitter));

                    node.Remove();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }

            else if (selectedNode is ModifierTreeNode treeNode)
            {
                try
                {
                    Modifier modifier = treeNode.Modifier;

                    OnModifierRemoved(new ModifierEventArgs(modifier));

                    treeNode.Remove();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        /// <summary>
        /// Handles the Click event of the uxReinitialiseEmitter control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void uxReinitialiseEmitter_Click(object sender, EventArgs e)
        {
	        if (uxEffectTree.SelectedNode is EmitterTreeNode node)
		        using (NewEmitterDialog dialog = new NewEmitterDialog(node.Emitter.Budget, node.Emitter.Term))
		        {
			        if (dialog.ShowDialog() == DialogResult.OK)
			        {
				        var args = new EmitterReinitialisedEventArgs(node.Emitter, dialog.EmitterBudget, dialog.EmitterTerm);

				        OnEmitterReinitialised(args);
			        }
		        }
        }

        /// <summary>
        /// Handles the Click event of the uxMainMenuViewTextures control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void uxMainMenuViewTextures_Click(object sender, EventArgs e)
        {
            using (TextureReferenceBrowser browser = new TextureReferenceBrowser(TextureReferences, TextureReferenceAdded))
            {
                browser.ShowDialog();
            }
        }

        /// <summary>
        /// Handles the Click event of the uxSelectTexture control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void uxSelectTexture_Click(object sender, EventArgs e)
        {
            using (TextureReferenceBrowser browser = new TextureReferenceBrowser(TextureReferences, TextureReferenceAdded))
            {
                if (browser.ShowDialog() == DialogResult.OK)
                {
	                if (uxEffectTree.SelectedNode is EmitterTreeNode node)
	                {
		                var args = new TextureReferenceChangedEventArgs(node.Emitter, browser.SelectedReference);

		                OnTextureReferenceChanged(args);
	                }

	                uxPropertyBrowser.Refresh();
                }
            }
        }

        /// <summary>
        /// Handles the ItemActivate event of the uxLibraryListView control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void uxLibraryListView_ItemActivate(object sender, EventArgs e)
        {
            if (MessageBox.Show(Resources.UserInterface_uxLibraryListView_ItemActivate_,
	                Resources.UserInterface_uxLibraryListView_ItemActivate_确认,
                                MessageBoxButtons.YesNoCancel,
                                MessageBoxIcon.Question) == DialogResult.Yes)
            {
                ListViewItem item = uxLibraryListView.SelectedItems[0];

                IEffectSerializationPlugin plugin = item.Tag as IEffectSerializationPlugin;

                string filePath = Application.StartupPath + "\\EffectLibrary\\" + item.Text;

                OnDeserialize(new SerializeEventArgs(plugin, filePath));
            }
        }

        /// <summary>
        /// Handles the Click event of the uxMainMenuNew control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void uxMainMenuNew_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(Resources.UserInterface_uxLibraryListView_ItemActivate_,
                                Resources.UserInterface_uxLibraryListView_ItemActivate_确认,
                                MessageBoxButtons.YesNoCancel,
                                MessageBoxIcon.Question) == DialogResult.Yes)
            {
                OnNewParticleEffect(EventArgs.Empty);
            }
        }

        /// <summary>
        /// Handles the click event of the uxMainMenuLog control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void uxMainMenuLog_Click(object sender, EventArgs e)
        {
            Trace.WriteLine("用户请求日志文件...", "UI");

            ThreadPool.QueueUserWorkItem(s => Process.Start("Trace.log"));
        }

        /// <summary>
        /// Handles the CheckedChanged event of the uxToggleEmitterEnabledMenuItem control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void uxToggleEmitterEnabledMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            Emitter emitter = (uxEffectTree.SelectedNode as EmitterTreeNode)?.Emitter;

            if (emitter != null && emitter.Enabled == false)
            {
                uxEffectTree.SelectedNode.ForeColor = SystemColors.WindowText;
                
                uxEffectTree.SelectedNode.Text = uxEffectTree.SelectedNode.Text.Replace(" (禁用)", "");
                
                uxEffectTree.SelectedNode.Expand();

                emitter.Enabled = true;
            }
            else
            {
                uxEffectTree.SelectedNode.Collapse();

                uxEffectTree.SelectedNode.ForeColor = Color.Gray;
                
                uxEffectTree.SelectedNode.Text = $@"{uxEffectTree.SelectedNode.Text} (禁用)";

	            if (emitter != null) emitter.Enabled = false;
            }
        }

	    private void AssertOperationOk(CoreOperationResult operationResult)
        {
            Guard.ArgumentNull("operationResult", operationResult);

            if (operationResult == CoreOperationResult.OK)
                return;

            MessageBox.Show(Resources.UserInterface_AssertOperationOK_,
							Resources.UserInterface_AssertOperationOK_程序错误,
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error);

            LogError(operationResult.Exception);
        }

        private void LogError(Exception error)
        {
            Guard.ArgumentNull("错误", error);

            Trace.TraceError(error.Message);
        }
    }
}

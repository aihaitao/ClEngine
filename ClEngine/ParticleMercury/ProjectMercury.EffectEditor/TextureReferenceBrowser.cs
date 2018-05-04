/*  
 Copyright © 2009 Project Mercury Team Members (http://mpe.codeplex.com/People/ProjectPeople.aspx)

 This program is licensed under the Microsoft Permissive License (Ms-PL).  You should 
 have received a copy of the license along with the source code.  If not, an online copy
 of the license can be found at http://mpe.codeplex.com/license.
*/

namespace ProjectMercury.EffectEditor
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Windows.Forms;

	internal partial class TextureReferenceBrowser : Form
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TextureReferenceBrowser"/> class.
        /// </summary>
        public TextureReferenceBrowser()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TextureReferenceBrowser"/> class.
        /// </summary>
        /// <param name="references">The references.</param>
        public TextureReferenceBrowser(IEnumerable<TextureReference> references, NewTextureReferenceEventHandler newHandler)
            : this()
        {
            TextureReferences = references;

            NewTextureReferenceHandler = newHandler;

            RefreshTextureDisplay();
        }

        /// <summary>
        /// Refreshes the texture display.
        /// </summary>
        private void RefreshTextureDisplay()
        {
            uxTextureImageList.Images.Clear();
            uxTextureReferences.Items.Clear();

            foreach (TextureReference reference in TextureReferences)
            {
                Image image = Image.FromFile(reference.FilePath, true);

                uxTextureImageList.Images.Add(reference.GetAssetName(), image.GetThumbnailImage(128, 128, null, IntPtr.Zero));

                ListViewItem item = new ListViewItem
                {
                    Text = reference.GetAssetName(),
                    ImageIndex = uxTextureImageList.Images.IndexOfKey(reference.GetAssetName()),
                    ToolTipText = reference.FilePath,
                    Tag = reference
                };

                uxTextureReferences.Items.Add(item);
            }
        }
        public IEnumerable<TextureReference> TextureReferences { get; private set; }

        public NewTextureReferenceEventHandler NewTextureReferenceHandler { get; private set; }

        /// <summary>
        /// Gets the selected reference.
        /// </summary>
        /// <value>The selected reference.</value>
        public TextureReference SelectedReference
        {
            get
            {
                if (uxTextureReferences.SelectedItems.Count > 0)
                    if (uxTextureReferences.SelectedItems[0] != null)
                        return uxTextureReferences.SelectedItems[0].Tag as TextureReference;

                return null;
            }
        }

        /// <summary>
        /// Handles the ItemActivate event of the uxTextureReferences control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void uxTextureReferences_ItemActivate(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;

            Close();
        }

        /// <summary>
        /// Handles the Click event of the uxImport control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void uxImport_Click(object sender, EventArgs e)
        {
            if (uxImportDialog.ShowDialog() == DialogResult.OK)
            {
                foreach (string filePath in uxImportDialog.FileNames)
                {
                    var args = new NewTextureReferenceEventArgs(filePath);

                    NewTextureReferenceHandler(this, args);

                    if (args.AddedTextureReference != null)
                    {
                        var texRef = args.AddedTextureReference;

                        Image image = Image.FromFile(texRef.FilePath, true);

                        uxTextureImageList.Images.Add(texRef.GetAssetName(), image.GetThumbnailImage(128, 128, null, IntPtr.Zero));

                        ListViewItem item = new ListViewItem
                        {
                            Text = texRef.GetAssetName(),
                            ImageIndex = uxTextureImageList.Images.IndexOfKey(texRef.GetAssetName()),
                            ToolTipText = texRef.FilePath,
                            Tag = texRef
                        };

                        uxTextureReferences.Items.Add(item);
                    }
                }
            }
        }
    }
}

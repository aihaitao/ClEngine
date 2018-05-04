﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------



namespace Microsoft.Xna.Framework.MonoGame_Framework_WindowsUniversal_XamlTypeInfo
{
    /// <summary>
    /// Main class for providing metadata for the app or library
    /// </summary>
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 10.0.16.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    public sealed class XamlMetaDataProvider : global::Windows.UI.Xaml.Markup.IXamlMetadataProvider
    {
        private global::Microsoft.Xna.Framework.MonoGame_Framework_WindowsUniversal_XamlTypeInfo.XamlTypeInfoProvider _provider = null;

        private global::Microsoft.Xna.Framework.MonoGame_Framework_WindowsUniversal_XamlTypeInfo.XamlTypeInfoProvider Provider
        {
            get
            {
                if (_provider == null)
                {
                    _provider = new global::Microsoft.Xna.Framework.MonoGame_Framework_WindowsUniversal_XamlTypeInfo.XamlTypeInfoProvider();
                }
                return _provider;
            }
        }

        /// <summary>
        /// GetXamlType(Type)
        /// </summary>
        [global::System.CLSCompliant(false)] public global::Windows.UI.Xaml.Markup.IXamlType GetXamlType(global::System.Type type)
        {
            return Provider.GetXamlTypeByType(type);
        }

        /// <summary>
        /// GetXamlType(String)
        /// </summary>
        [global::System.CLSCompliant(false)] public global::Windows.UI.Xaml.Markup.IXamlType GetXamlType(string fullName)
        {
            return Provider.GetXamlTypeByName(fullName);
        }

        /// <summary>
        /// GetXmlnsDefinitions()
        /// </summary>
        [global::System.CLSCompliant(false)] public global::Windows.UI.Xaml.Markup.XmlnsDefinition[] GetXmlnsDefinitions()
        {
            return new global::Windows.UI.Xaml.Markup.XmlnsDefinition[0];
        }
    }

    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 10.0.16.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    internal partial class XamlTypeInfoProvider
    {
        public global::Windows.UI.Xaml.Markup.IXamlType GetXamlTypeByType(global::System.Type type)
        {
            global::Windows.UI.Xaml.Markup.IXamlType xamlType;
            if (_xamlTypeCacheByType.TryGetValue(type, out xamlType))
            {
                return xamlType;
            }
            int typeIndex = LookupTypeIndexByType(type);
            if(typeIndex != -1)
            {
                xamlType = CreateXamlType(typeIndex);
            }
            if (xamlType != null)
            {
                _xamlTypeCacheByName.Add(xamlType.FullName, xamlType);
                _xamlTypeCacheByType.Add(xamlType.UnderlyingType, xamlType);
            }
            return xamlType;
        }

        public global::Windows.UI.Xaml.Markup.IXamlType GetXamlTypeByName(string typeName)
        {
            if (string.IsNullOrEmpty(typeName))
            {
                return null;
            }
            global::Windows.UI.Xaml.Markup.IXamlType xamlType;
            if (_xamlTypeCacheByName.TryGetValue(typeName, out xamlType))
            {
                return xamlType;
            }
            int typeIndex = LookupTypeIndexByName(typeName);
            if(typeIndex != -1)
            {
                xamlType = CreateXamlType(typeIndex);
            }
            if (xamlType != null)
            {
                _xamlTypeCacheByName.Add(xamlType.FullName, xamlType);
                _xamlTypeCacheByType.Add(xamlType.UnderlyingType, xamlType);
            }
            return xamlType;
        }

        public global::Windows.UI.Xaml.Markup.IXamlMember GetMemberByLongName(string longMemberName)
        {
            if (string.IsNullOrEmpty(longMemberName))
            {
                return null;
            }
            global::Windows.UI.Xaml.Markup.IXamlMember xamlMember;
            if (_xamlMembers.TryGetValue(longMemberName, out xamlMember))
            {
                return xamlMember;
            }
            xamlMember = CreateXamlMember(longMemberName);
            if (xamlMember != null)
            {
                _xamlMembers.Add(longMemberName, xamlMember);
            }
            return xamlMember;
        }

        global::System.Collections.Generic.Dictionary<string, global::Windows.UI.Xaml.Markup.IXamlType>
                _xamlTypeCacheByName = new global::System.Collections.Generic.Dictionary<string, global::Windows.UI.Xaml.Markup.IXamlType>();

        global::System.Collections.Generic.Dictionary<global::System.Type, global::Windows.UI.Xaml.Markup.IXamlType>
                _xamlTypeCacheByType = new global::System.Collections.Generic.Dictionary<global::System.Type, global::Windows.UI.Xaml.Markup.IXamlType>();

        global::System.Collections.Generic.Dictionary<string, global::Windows.UI.Xaml.Markup.IXamlMember>
                _xamlMembers = new global::System.Collections.Generic.Dictionary<string, global::Windows.UI.Xaml.Markup.IXamlMember>();

        string[] _typeNameTable = null;
        global::System.Type[] _typeTable = null;

        private void InitTypeTables()
        {
            _typeNameTable = new string[6];
            _typeNameTable[0] = "Microsoft.Xna.Framework.Input.InputDialog";
            _typeNameTable[1] = "Windows.UI.Xaml.Controls.Control";
            _typeNameTable[2] = "Windows.UI.Xaml.Media.Brush";
            _typeNameTable[3] = "Windows.UI.Xaml.Style";
            _typeNameTable[4] = "Boolean";
            _typeNameTable[5] = "Windows.UI.Xaml.Controls.Orientation";

            _typeTable = new global::System.Type[6];
            _typeTable[0] = typeof(global::Microsoft.Xna.Framework.Input.InputDialog);
            _typeTable[1] = typeof(global::Windows.UI.Xaml.Controls.Control);
            _typeTable[2] = typeof(global::Windows.UI.Xaml.Media.Brush);
            _typeTable[3] = typeof(global::Windows.UI.Xaml.Style);
            _typeTable[4] = typeof(global::System.Boolean);
            _typeTable[5] = typeof(global::Windows.UI.Xaml.Controls.Orientation);
        }

        private int LookupTypeIndexByName(string typeName)
        {
            if (_typeNameTable == null)
            {
                InitTypeTables();
            }
            for (int i=0; i<_typeNameTable.Length; i++)
            {
                if(0 == string.CompareOrdinal(_typeNameTable[i], typeName))
                {
                    return i;
                }
            }
            return -1;
        }

        private int LookupTypeIndexByType(global::System.Type type)
        {
            if (_typeTable == null)
            {
                InitTypeTables();
            }
            for(int i=0; i<_typeTable.Length; i++)
            {
                if(type == _typeTable[i])
                {
                    return i;
                }
            }
            return -1;
        }

        private object Activate_0_InputDialog() { return new global::Microsoft.Xna.Framework.Input.InputDialog(); }

        private global::Windows.UI.Xaml.Markup.IXamlType CreateXamlType(int typeIndex)
        {
            global::Microsoft.Xna.Framework.MonoGame_Framework_WindowsUniversal_XamlTypeInfo.XamlSystemBaseType xamlType = null;
            global::Microsoft.Xna.Framework.MonoGame_Framework_WindowsUniversal_XamlTypeInfo.XamlUserType userType;
            string typeName = _typeNameTable[typeIndex];
            global::System.Type type = _typeTable[typeIndex];

            switch (typeIndex)
            {

            case 0:   //  Microsoft.Xna.Framework.Input.InputDialog
                userType = new global::Microsoft.Xna.Framework.MonoGame_Framework_WindowsUniversal_XamlTypeInfo.XamlUserType(this, typeName, type, GetXamlTypeByName("Windows.UI.Xaml.Controls.Control"));
                userType.Activator = Activate_0_InputDialog;
                userType.AddMemberName("BackgroundScreenBrush");
                userType.AddMemberName("BackgroundStripeBrush");
                userType.AddMemberName("TitleStyle");
                userType.AddMemberName("TextStyle");
                userType.AddMemberName("InputTextStyle");
                userType.AddMemberName("InputPasswordStyle");
                userType.AddMemberName("ButtonStyle");
                userType.AddMemberName("IsLightDismissEnabled");
                userType.AddMemberName("AwaitsCloseTransition");
                userType.AddMemberName("ButtonsPanelOrientation");
                userType.SetIsLocalType();
                xamlType = userType;
                break;

            case 1:   //  Windows.UI.Xaml.Controls.Control
                xamlType = new global::Microsoft.Xna.Framework.MonoGame_Framework_WindowsUniversal_XamlTypeInfo.XamlSystemBaseType(typeName, type);
                break;

            case 2:   //  Windows.UI.Xaml.Media.Brush
                xamlType = new global::Microsoft.Xna.Framework.MonoGame_Framework_WindowsUniversal_XamlTypeInfo.XamlSystemBaseType(typeName, type);
                break;

            case 3:   //  Windows.UI.Xaml.Style
                xamlType = new global::Microsoft.Xna.Framework.MonoGame_Framework_WindowsUniversal_XamlTypeInfo.XamlSystemBaseType(typeName, type);
                break;

            case 4:   //  Boolean
                xamlType = new global::Microsoft.Xna.Framework.MonoGame_Framework_WindowsUniversal_XamlTypeInfo.XamlSystemBaseType(typeName, type);
                break;

            case 5:   //  Windows.UI.Xaml.Controls.Orientation
                xamlType = new global::Microsoft.Xna.Framework.MonoGame_Framework_WindowsUniversal_XamlTypeInfo.XamlSystemBaseType(typeName, type);
                break;
            }
            return xamlType;
        }


        private object get_0_InputDialog_BackgroundScreenBrush(object instance)
        {
            var that = (global::Microsoft.Xna.Framework.Input.InputDialog)instance;
            return that.BackgroundScreenBrush;
        }
        private void set_0_InputDialog_BackgroundScreenBrush(object instance, object Value)
        {
            var that = (global::Microsoft.Xna.Framework.Input.InputDialog)instance;
            that.BackgroundScreenBrush = (global::Windows.UI.Xaml.Media.Brush)Value;
        }
        private object get_1_InputDialog_BackgroundStripeBrush(object instance)
        {
            var that = (global::Microsoft.Xna.Framework.Input.InputDialog)instance;
            return that.BackgroundStripeBrush;
        }
        private void set_1_InputDialog_BackgroundStripeBrush(object instance, object Value)
        {
            var that = (global::Microsoft.Xna.Framework.Input.InputDialog)instance;
            that.BackgroundStripeBrush = (global::Windows.UI.Xaml.Media.Brush)Value;
        }
        private object get_2_InputDialog_TitleStyle(object instance)
        {
            var that = (global::Microsoft.Xna.Framework.Input.InputDialog)instance;
            return that.TitleStyle;
        }
        private void set_2_InputDialog_TitleStyle(object instance, object Value)
        {
            var that = (global::Microsoft.Xna.Framework.Input.InputDialog)instance;
            that.TitleStyle = (global::Windows.UI.Xaml.Style)Value;
        }
        private object get_3_InputDialog_TextStyle(object instance)
        {
            var that = (global::Microsoft.Xna.Framework.Input.InputDialog)instance;
            return that.TextStyle;
        }
        private void set_3_InputDialog_TextStyle(object instance, object Value)
        {
            var that = (global::Microsoft.Xna.Framework.Input.InputDialog)instance;
            that.TextStyle = (global::Windows.UI.Xaml.Style)Value;
        }
        private object get_4_InputDialog_InputTextStyle(object instance)
        {
            var that = (global::Microsoft.Xna.Framework.Input.InputDialog)instance;
            return that.InputTextStyle;
        }
        private void set_4_InputDialog_InputTextStyle(object instance, object Value)
        {
            var that = (global::Microsoft.Xna.Framework.Input.InputDialog)instance;
            that.InputTextStyle = (global::Windows.UI.Xaml.Style)Value;
        }
        private object get_5_InputDialog_InputPasswordStyle(object instance)
        {
            var that = (global::Microsoft.Xna.Framework.Input.InputDialog)instance;
            return that.InputPasswordStyle;
        }
        private void set_5_InputDialog_InputPasswordStyle(object instance, object Value)
        {
            var that = (global::Microsoft.Xna.Framework.Input.InputDialog)instance;
            that.InputPasswordStyle = (global::Windows.UI.Xaml.Style)Value;
        }
        private object get_6_InputDialog_ButtonStyle(object instance)
        {
            var that = (global::Microsoft.Xna.Framework.Input.InputDialog)instance;
            return that.ButtonStyle;
        }
        private void set_6_InputDialog_ButtonStyle(object instance, object Value)
        {
            var that = (global::Microsoft.Xna.Framework.Input.InputDialog)instance;
            that.ButtonStyle = (global::Windows.UI.Xaml.Style)Value;
        }
        private object get_7_InputDialog_IsLightDismissEnabled(object instance)
        {
            var that = (global::Microsoft.Xna.Framework.Input.InputDialog)instance;
            return that.IsLightDismissEnabled;
        }
        private void set_7_InputDialog_IsLightDismissEnabled(object instance, object Value)
        {
            var that = (global::Microsoft.Xna.Framework.Input.InputDialog)instance;
            that.IsLightDismissEnabled = (global::System.Boolean)Value;
        }
        private object get_8_InputDialog_AwaitsCloseTransition(object instance)
        {
            var that = (global::Microsoft.Xna.Framework.Input.InputDialog)instance;
            return that.AwaitsCloseTransition;
        }
        private void set_8_InputDialog_AwaitsCloseTransition(object instance, object Value)
        {
            var that = (global::Microsoft.Xna.Framework.Input.InputDialog)instance;
            that.AwaitsCloseTransition = (global::System.Boolean)Value;
        }
        private object get_9_InputDialog_ButtonsPanelOrientation(object instance)
        {
            var that = (global::Microsoft.Xna.Framework.Input.InputDialog)instance;
            return that.ButtonsPanelOrientation;
        }
        private void set_9_InputDialog_ButtonsPanelOrientation(object instance, object Value)
        {
            var that = (global::Microsoft.Xna.Framework.Input.InputDialog)instance;
            that.ButtonsPanelOrientation = (global::Windows.UI.Xaml.Controls.Orientation)Value;
        }

        private global::Windows.UI.Xaml.Markup.IXamlMember CreateXamlMember(string longMemberName)
        {
            global::Microsoft.Xna.Framework.MonoGame_Framework_WindowsUniversal_XamlTypeInfo.XamlMember xamlMember = null;
            global::Microsoft.Xna.Framework.MonoGame_Framework_WindowsUniversal_XamlTypeInfo.XamlUserType userType;

            switch (longMemberName)
            {
            case "Microsoft.Xna.Framework.Input.InputDialog.BackgroundScreenBrush":
                userType = (global::Microsoft.Xna.Framework.MonoGame_Framework_WindowsUniversal_XamlTypeInfo.XamlUserType)GetXamlTypeByName("Microsoft.Xna.Framework.Input.InputDialog");
                xamlMember = new global::Microsoft.Xna.Framework.MonoGame_Framework_WindowsUniversal_XamlTypeInfo.XamlMember(this, "BackgroundScreenBrush", "Windows.UI.Xaml.Media.Brush");
                xamlMember.SetIsDependencyProperty();
                xamlMember.Getter = get_0_InputDialog_BackgroundScreenBrush;
                xamlMember.Setter = set_0_InputDialog_BackgroundScreenBrush;
                break;
            case "Microsoft.Xna.Framework.Input.InputDialog.BackgroundStripeBrush":
                userType = (global::Microsoft.Xna.Framework.MonoGame_Framework_WindowsUniversal_XamlTypeInfo.XamlUserType)GetXamlTypeByName("Microsoft.Xna.Framework.Input.InputDialog");
                xamlMember = new global::Microsoft.Xna.Framework.MonoGame_Framework_WindowsUniversal_XamlTypeInfo.XamlMember(this, "BackgroundStripeBrush", "Windows.UI.Xaml.Media.Brush");
                xamlMember.SetIsDependencyProperty();
                xamlMember.Getter = get_1_InputDialog_BackgroundStripeBrush;
                xamlMember.Setter = set_1_InputDialog_BackgroundStripeBrush;
                break;
            case "Microsoft.Xna.Framework.Input.InputDialog.TitleStyle":
                userType = (global::Microsoft.Xna.Framework.MonoGame_Framework_WindowsUniversal_XamlTypeInfo.XamlUserType)GetXamlTypeByName("Microsoft.Xna.Framework.Input.InputDialog");
                xamlMember = new global::Microsoft.Xna.Framework.MonoGame_Framework_WindowsUniversal_XamlTypeInfo.XamlMember(this, "TitleStyle", "Windows.UI.Xaml.Style");
                xamlMember.SetIsDependencyProperty();
                xamlMember.Getter = get_2_InputDialog_TitleStyle;
                xamlMember.Setter = set_2_InputDialog_TitleStyle;
                break;
            case "Microsoft.Xna.Framework.Input.InputDialog.TextStyle":
                userType = (global::Microsoft.Xna.Framework.MonoGame_Framework_WindowsUniversal_XamlTypeInfo.XamlUserType)GetXamlTypeByName("Microsoft.Xna.Framework.Input.InputDialog");
                xamlMember = new global::Microsoft.Xna.Framework.MonoGame_Framework_WindowsUniversal_XamlTypeInfo.XamlMember(this, "TextStyle", "Windows.UI.Xaml.Style");
                xamlMember.SetIsDependencyProperty();
                xamlMember.Getter = get_3_InputDialog_TextStyle;
                xamlMember.Setter = set_3_InputDialog_TextStyle;
                break;
            case "Microsoft.Xna.Framework.Input.InputDialog.InputTextStyle":
                userType = (global::Microsoft.Xna.Framework.MonoGame_Framework_WindowsUniversal_XamlTypeInfo.XamlUserType)GetXamlTypeByName("Microsoft.Xna.Framework.Input.InputDialog");
                xamlMember = new global::Microsoft.Xna.Framework.MonoGame_Framework_WindowsUniversal_XamlTypeInfo.XamlMember(this, "InputTextStyle", "Windows.UI.Xaml.Style");
                xamlMember.SetIsDependencyProperty();
                xamlMember.Getter = get_4_InputDialog_InputTextStyle;
                xamlMember.Setter = set_4_InputDialog_InputTextStyle;
                break;
            case "Microsoft.Xna.Framework.Input.InputDialog.InputPasswordStyle":
                userType = (global::Microsoft.Xna.Framework.MonoGame_Framework_WindowsUniversal_XamlTypeInfo.XamlUserType)GetXamlTypeByName("Microsoft.Xna.Framework.Input.InputDialog");
                xamlMember = new global::Microsoft.Xna.Framework.MonoGame_Framework_WindowsUniversal_XamlTypeInfo.XamlMember(this, "InputPasswordStyle", "Windows.UI.Xaml.Style");
                xamlMember.SetIsDependencyProperty();
                xamlMember.Getter = get_5_InputDialog_InputPasswordStyle;
                xamlMember.Setter = set_5_InputDialog_InputPasswordStyle;
                break;
            case "Microsoft.Xna.Framework.Input.InputDialog.ButtonStyle":
                userType = (global::Microsoft.Xna.Framework.MonoGame_Framework_WindowsUniversal_XamlTypeInfo.XamlUserType)GetXamlTypeByName("Microsoft.Xna.Framework.Input.InputDialog");
                xamlMember = new global::Microsoft.Xna.Framework.MonoGame_Framework_WindowsUniversal_XamlTypeInfo.XamlMember(this, "ButtonStyle", "Windows.UI.Xaml.Style");
                xamlMember.SetIsDependencyProperty();
                xamlMember.Getter = get_6_InputDialog_ButtonStyle;
                xamlMember.Setter = set_6_InputDialog_ButtonStyle;
                break;
            case "Microsoft.Xna.Framework.Input.InputDialog.IsLightDismissEnabled":
                userType = (global::Microsoft.Xna.Framework.MonoGame_Framework_WindowsUniversal_XamlTypeInfo.XamlUserType)GetXamlTypeByName("Microsoft.Xna.Framework.Input.InputDialog");
                xamlMember = new global::Microsoft.Xna.Framework.MonoGame_Framework_WindowsUniversal_XamlTypeInfo.XamlMember(this, "IsLightDismissEnabled", "Boolean");
                xamlMember.SetIsDependencyProperty();
                xamlMember.Getter = get_7_InputDialog_IsLightDismissEnabled;
                xamlMember.Setter = set_7_InputDialog_IsLightDismissEnabled;
                break;
            case "Microsoft.Xna.Framework.Input.InputDialog.AwaitsCloseTransition":
                userType = (global::Microsoft.Xna.Framework.MonoGame_Framework_WindowsUniversal_XamlTypeInfo.XamlUserType)GetXamlTypeByName("Microsoft.Xna.Framework.Input.InputDialog");
                xamlMember = new global::Microsoft.Xna.Framework.MonoGame_Framework_WindowsUniversal_XamlTypeInfo.XamlMember(this, "AwaitsCloseTransition", "Boolean");
                xamlMember.SetIsDependencyProperty();
                xamlMember.Getter = get_8_InputDialog_AwaitsCloseTransition;
                xamlMember.Setter = set_8_InputDialog_AwaitsCloseTransition;
                break;
            case "Microsoft.Xna.Framework.Input.InputDialog.ButtonsPanelOrientation":
                userType = (global::Microsoft.Xna.Framework.MonoGame_Framework_WindowsUniversal_XamlTypeInfo.XamlUserType)GetXamlTypeByName("Microsoft.Xna.Framework.Input.InputDialog");
                xamlMember = new global::Microsoft.Xna.Framework.MonoGame_Framework_WindowsUniversal_XamlTypeInfo.XamlMember(this, "ButtonsPanelOrientation", "Windows.UI.Xaml.Controls.Orientation");
                xamlMember.SetIsDependencyProperty();
                xamlMember.Getter = get_9_InputDialog_ButtonsPanelOrientation;
                xamlMember.Setter = set_9_InputDialog_ButtonsPanelOrientation;
                break;
            }
            return xamlMember;
        }
    }

    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 10.0.16.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    internal class XamlSystemBaseType : global::Windows.UI.Xaml.Markup.IXamlType
    {
        string _fullName;
        global::System.Type _underlyingType;

        public XamlSystemBaseType(string fullName, global::System.Type underlyingType)
        {
            _fullName = fullName;
            _underlyingType = underlyingType;
        }

        public string FullName { get { return _fullName; } }

        public global::System.Type UnderlyingType
        {
            get
            {
                return _underlyingType;
            }
        }

        virtual public global::Windows.UI.Xaml.Markup.IXamlType BaseType { get { throw new global::System.NotImplementedException(); } }
        virtual public global::Windows.UI.Xaml.Markup.IXamlMember ContentProperty { get { throw new global::System.NotImplementedException(); } }
        virtual public global::Windows.UI.Xaml.Markup.IXamlMember GetMember(string name) { throw new global::System.NotImplementedException(); }
        virtual public bool IsArray { get { throw new global::System.NotImplementedException(); } }
        virtual public bool IsCollection { get { throw new global::System.NotImplementedException(); } }
        virtual public bool IsConstructible { get { throw new global::System.NotImplementedException(); } }
        virtual public bool IsDictionary { get { throw new global::System.NotImplementedException(); } }
        virtual public bool IsMarkupExtension { get { throw new global::System.NotImplementedException(); } }
        virtual public bool IsBindable { get { throw new global::System.NotImplementedException(); } }
        virtual public bool IsReturnTypeStub { get { throw new global::System.NotImplementedException(); } }
        virtual public bool IsLocalType { get { throw new global::System.NotImplementedException(); } }
        virtual public global::Windows.UI.Xaml.Markup.IXamlType ItemType { get { throw new global::System.NotImplementedException(); } }
        virtual public global::Windows.UI.Xaml.Markup.IXamlType KeyType { get { throw new global::System.NotImplementedException(); } }
        virtual public object ActivateInstance() { throw new global::System.NotImplementedException(); }
        virtual public void AddToMap(object instance, object key, object item)  { throw new global::System.NotImplementedException(); }
        virtual public void AddToVector(object instance, object item)  { throw new global::System.NotImplementedException(); }
        virtual public void RunInitializer()   { throw new global::System.NotImplementedException(); }
        virtual public object CreateFromString(string input)   { throw new global::System.NotImplementedException(); }
    }
    
    internal delegate object Activator();
    internal delegate void AddToCollection(object instance, object item);
    internal delegate void AddToDictionary(object instance, object key, object item);
    internal delegate object CreateFromStringMethod(string args);

    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 10.0.16.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    internal class XamlUserType : global::Microsoft.Xna.Framework.MonoGame_Framework_WindowsUniversal_XamlTypeInfo.XamlSystemBaseType
    {
        global::Microsoft.Xna.Framework.MonoGame_Framework_WindowsUniversal_XamlTypeInfo.XamlTypeInfoProvider _provider;
        global::Windows.UI.Xaml.Markup.IXamlType _baseType;
        bool _isArray;
        bool _isMarkupExtension;
        bool _isBindable;
        bool _isReturnTypeStub;
        bool _isLocalType;

        string _contentPropertyName;
        string _itemTypeName;
        string _keyTypeName;
        global::System.Collections.Generic.Dictionary<string, string> _memberNames;
        global::System.Collections.Generic.Dictionary<string, object> _enumValues;

        public XamlUserType(global::Microsoft.Xna.Framework.MonoGame_Framework_WindowsUniversal_XamlTypeInfo.XamlTypeInfoProvider provider, string fullName, global::System.Type fullType, global::Windows.UI.Xaml.Markup.IXamlType baseType)
            :base(fullName, fullType)
        {
            _provider = provider;
            _baseType = baseType;
        }

        // --- Interface methods ----

        override public global::Windows.UI.Xaml.Markup.IXamlType BaseType { get { return _baseType; } }
        override public bool IsArray { get { return _isArray; } }
        override public bool IsCollection { get { return (CollectionAdd != null); } }
        override public bool IsConstructible { get { return (Activator != null); } }
        override public bool IsDictionary { get { return (DictionaryAdd != null); } }
        override public bool IsMarkupExtension { get { return _isMarkupExtension; } }
        override public bool IsBindable { get { return _isBindable; } }
        override public bool IsReturnTypeStub { get { return _isReturnTypeStub; } }
        override public bool IsLocalType { get { return _isLocalType; } }

        override public global::Windows.UI.Xaml.Markup.IXamlMember ContentProperty
        {
            get { return _provider.GetMemberByLongName(_contentPropertyName); }
        }

        override public global::Windows.UI.Xaml.Markup.IXamlType ItemType
        {
            get { return _provider.GetXamlTypeByName(_itemTypeName); }
        }

        override public global::Windows.UI.Xaml.Markup.IXamlType KeyType
        {
            get { return _provider.GetXamlTypeByName(_keyTypeName); }
        }

        override public global::Windows.UI.Xaml.Markup.IXamlMember GetMember(string name)
        {
            if (_memberNames == null)
            {
                return null;
            }
            string longName;
            if (_memberNames.TryGetValue(name, out longName))
            {
                return _provider.GetMemberByLongName(longName);
            }
            return null;
        }

        override public object ActivateInstance()
        {
            return Activator(); 
        }

        override public void AddToMap(object instance, object key, object item) 
        {
            DictionaryAdd(instance, key, item);
        }

        override public void AddToVector(object instance, object item)
        {
            CollectionAdd(instance, item);
        }

        override public void RunInitializer() 
        {
            global::System.Runtime.CompilerServices.RuntimeHelpers.RunClassConstructor(UnderlyingType.TypeHandle);
        }

        override public object CreateFromString(string input)
        {
            if (CreateFromStringMethod != null)
            {
                return this.CreateFromStringMethod(input);
            }
            else if (_enumValues != null)
            {
                int value = 0;

                string[] valueParts = input.Split(',');

                foreach (string valuePart in valueParts) 
                {
                    object partValue;
                    int enumFieldValue = 0;
                    try
                    {
                        if (_enumValues.TryGetValue(valuePart.Trim(), out partValue))
                        {
                            enumFieldValue = global::System.Convert.ToInt32(partValue);
                        }
                        else
                        {
                            try
                            {
                                enumFieldValue = global::System.Convert.ToInt32(valuePart.Trim());
                            }
                            catch( global::System.FormatException )
                            {
                                foreach( string key in _enumValues.Keys )
                                {
                                    if( string.Compare(valuePart.Trim(), key, global::System.StringComparison.OrdinalIgnoreCase) == 0 )
                                    {
                                        if( _enumValues.TryGetValue(key.Trim(), out partValue) )
                                        {
                                            enumFieldValue = global::System.Convert.ToInt32(partValue);
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                        value |= enumFieldValue; 
                    }
                    catch( global::System.FormatException )
                    {
                        throw new global::System.ArgumentException(input, FullName);
                    }
                }

                return value; 
            }
            throw new global::System.ArgumentException(input, FullName);
        }

        // --- End of Interface methods

        public Activator Activator { get; set; }
        public AddToCollection CollectionAdd { get; set; }
        public AddToDictionary DictionaryAdd { get; set; }
        public CreateFromStringMethod CreateFromStringMethod {get; set; }

        public void SetContentPropertyName(string contentPropertyName)
        {
            _contentPropertyName = contentPropertyName;
        }

        public void SetIsArray()
        {
            _isArray = true; 
        }

        public void SetIsMarkupExtension()
        {
            _isMarkupExtension = true;
        }

        public void SetIsBindable()
        {
            _isBindable = true;
        }

        public void SetIsReturnTypeStub()
        {
            _isReturnTypeStub = true;
        }

        public void SetIsLocalType()
        {
            _isLocalType = true;
        }

        public void SetItemTypeName(string itemTypeName)
        {
            _itemTypeName = itemTypeName;
        }

        public void SetKeyTypeName(string keyTypeName)
        {
            _keyTypeName = keyTypeName;
        }

        public void AddMemberName(string shortName)
        {
            if(_memberNames == null)
            {
                _memberNames =  new global::System.Collections.Generic.Dictionary<string,string>();
            }
            _memberNames.Add(shortName, FullName + "." + shortName);
        }

        public void AddEnumValue(string name, object value)
        {
            if (_enumValues == null)
            {
                _enumValues = new global::System.Collections.Generic.Dictionary<string, object>();
            }
            _enumValues.Add(name, value);
        }
    }

    internal delegate object Getter(object instance);
    internal delegate void Setter(object instance, object value);

    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 10.0.16.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    internal class XamlMember : global::Windows.UI.Xaml.Markup.IXamlMember
    {
        global::Microsoft.Xna.Framework.MonoGame_Framework_WindowsUniversal_XamlTypeInfo.XamlTypeInfoProvider _provider;
        string _name;
        bool _isAttachable;
        bool _isDependencyProperty;
        bool _isReadOnly;

        string _typeName;
        string _targetTypeName;

        public XamlMember(global::Microsoft.Xna.Framework.MonoGame_Framework_WindowsUniversal_XamlTypeInfo.XamlTypeInfoProvider provider, string name, string typeName)
        {
            _name = name;
            _typeName = typeName;
            _provider = provider;
        }

        public string Name { get { return _name; } }

        public global::Windows.UI.Xaml.Markup.IXamlType Type
        {
            get { return _provider.GetXamlTypeByName(_typeName); }
        }

        public void SetTargetTypeName(string targetTypeName)
        {
            _targetTypeName = targetTypeName;
        }
        public global::Windows.UI.Xaml.Markup.IXamlType TargetType
        {
            get { return _provider.GetXamlTypeByName(_targetTypeName); }
        }

        public void SetIsAttachable() { _isAttachable = true; }
        public bool IsAttachable { get { return _isAttachable; } }

        public void SetIsDependencyProperty() { _isDependencyProperty = true; }
        public bool IsDependencyProperty { get { return _isDependencyProperty; } }

        public void SetIsReadOnly() { _isReadOnly = true; }
        public bool IsReadOnly { get { return _isReadOnly; } }

        public Getter Getter { get; set; }
        public object GetValue(object instance)
        {
            if (Getter != null)
                return Getter(instance);
            else
                throw new global::System.InvalidOperationException("GetValue");
        }

        public Setter Setter { get; set; }
        public void SetValue(object instance, object value)
        {
            if (Setter != null)
                Setter(instance, value);
            else
                throw new global::System.InvalidOperationException("SetValue");
        }
    }
}

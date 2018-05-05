
// 注意: 生成的代码可能至少需要 .NET Framework 4.5 或 .NET Core/Standard 2.0。
/// <remarks/>
[System.SerializableAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
[System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
public partial class SourceFileCollection
{

	private string profileField;

	private string platformField;

	private object configField;

	private string[] sourceFilesField;

	/// <remarks/>
	public string Profile
	{
		get
		{
			return this.profileField;
		}
		set
		{
			this.profileField = value;
		}
	}

	/// <remarks/>
	public string Platform
	{
		get
		{
			return this.platformField;
		}
		set
		{
			this.platformField = value;
		}
	}

	/// <remarks/>
	public object Config
	{
		get
		{
			return this.configField;
		}
		set
		{
			this.configField = value;
		}
	}

	/// <remarks/>
	[System.Xml.Serialization.XmlArrayItemAttribute("File", IsNullable = false)]
	public string[] SourceFiles
	{
		get
		{
			return this.sourceFilesField;
		}
		set
		{
			this.sourceFilesField = value;
		}
	}
}


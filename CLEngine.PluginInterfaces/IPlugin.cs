using System;
using System.Drawing;

namespace CLEngine.PluginInterfaces
{
	public interface IPlugin
	{
		string Name { get; }

		string DisplayName { get; }

		Icon DisplayIcon { get; }

		string Description { get; }

		string Author { get; }

		string Library { get; }

		Version Version { get; }

		Version MinimumRequiredVersion { get; }
	}
}
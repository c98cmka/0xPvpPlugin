using System;
using System.Numerics;
using System.Runtime.CompilerServices;
using Dalamud.Interface.Utility;
using FFXIVClientStructs.FFXIV.Client.System.Resource.Handle;
using FFXIVClientStructs.FFXIV.Component.GUI;
using FFXIVClientStructs.STD;
using ImGuiNET;

namespace OPP.fr;

internal static class AreaMap
{
	public unsafe static AtkUnitBase* AreaMapAddon => (AtkUnitBase*)Plugin.gui.GetAddonByName("AreaMap", 1);

	public unsafe static bool HasMap => AreaMapAddon != (AtkUnitBase*)IntPtr.Zero;

	public unsafe static bool MapVisible
	{
		get
		{
			if (HasMap)
			{
				return (*AreaMapAddon).IsVisible;
			}
			return false;
		}
	}

	public unsafe static ref float MapScale => ref *(float*)((byte*)AreaMapAddon + 956);
}

using System;
using System.Runtime.InteropServices;
using Dalamud.Game;

namespace OPP.fr;

internal class AddressResolver : BaseAddressResolver
{
	public IntPtr CamPtr { get; private set; }

	public IntPtr GetMatrix { get; private set; }

	public IntPtr MapIdDungeon { get; private set; }

	public IntPtr MapIdWorld { get; private set; }

	protected override void Setup64Bit(ISigScanner scanner)
	{
		CamPtr = Marshal.ReadIntPtr(scanner.GetStaticAddressFromSig("48 8D 0D ?? ?? ?? ?? 45 33 C9 45 33 C0 33 D2 C6 40 09 01", 0));
		GetMatrix = scanner.ScanText("E8 ?? ?? ?? ?? 48 8D 4C 24 ?? 48 89 4c 24 ?? 4C 8D 4D ?? 4C 8D 44 24 ??");
		MapIdDungeon = scanner.GetStaticAddressFromSig("44 8B 3D ?? ?? ?? ?? 45 85 FF", 0);
		MapIdWorld = scanner.GetStaticAddressFromSig("44 0F 44 3D ?? ?? ?? ??", 0);
	}
}

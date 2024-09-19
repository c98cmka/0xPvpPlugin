using Dalamud.Interface.Utility;
using Dalamud.Utility;
using System;
using System.Numerics;
using System.Reflection;
using System.Runtime.InteropServices;

namespace OPP.fr;

unsafe static class Camera
{
    static FieldInfo matrixSingleton;

    public delegate IntPtr GetMatrixSingletonDelegate();
    public static readonly GetMatrixSingletonDelegate getMatrixSingleton;

    static Camera() {
        IntPtr addr = Plugin.SigScanner.ScanText("E8 ?? ?? ?? ?? 48 8D 4C 24 ?? 48 89 4c 24 ?? 4C 8D 4D ?? 4C 8D 44 24 ??");
        getMatrixSingleton ??= Marshal.GetDelegateForFunctionPointer<GetMatrixSingletonDelegate>(addr);
    }
    public static bool WorldToScreen(Vector3 worldPos, out Vector2 screenPos) => WorldToScreen(worldPos, out screenPos, out _);

    internal static bool WorldToScreen(Vector3 worldPos, out Vector2 screenPos, out Vector3 pCoordsRaw)
    {
        if (getMatrixSingleton == null)
            throw new InvalidOperationException("getMatrixSingleton did not initiate correctly");

        var matrixSingleton = getMatrixSingleton();
        if (matrixSingleton == IntPtr.Zero)
            throw new InvalidOperationException("Cannot get matrixSingleton");

        var viewProjectionMatrix = default(SharpDX.Matrix);
        float width, height;
        var windowPos = ImGuiHelpers.MainViewport.Pos;

        unsafe
        {
            var rawMatrix = (float*)(matrixSingleton + 0x1b4);
            if (rawMatrix == null) throw new InvalidOperationException("Cannot get rawMatrix");

            for (var i = 0; i < 16; i++, rawMatrix++)
                viewProjectionMatrix[i] = *rawMatrix;

            width = *rawMatrix;
            height = *(rawMatrix + 1);
        }

        var worldPosDx = new SharpDX.Vector3(worldPos.X, worldPos.Y, worldPos.Z);
        SharpDX.Vector3.Transform(ref worldPosDx, ref viewProjectionMatrix, out SharpDX.Vector3 pCoords);

        pCoordsRaw = new Vector3(pCoords.X, pCoords.Y, pCoords.Z);

        screenPos = new Vector2(pCoords.X / MathF.Abs(pCoords.Z), pCoords.Y / MathF.Abs(pCoords.Z));

        screenPos.X = (0.5f * width * (screenPos.X + 1f)) + windowPos.X;
        screenPos.Y = (0.5f * height * (1f - screenPos.Y)) + windowPos.Y;

        return pCoords.Z > 0
            && screenPos.X > windowPos.X && screenPos.X < windowPos.X + width
            && screenPos.Y > windowPos.Y && screenPos.Y < windowPos.Y + height;
    }
}

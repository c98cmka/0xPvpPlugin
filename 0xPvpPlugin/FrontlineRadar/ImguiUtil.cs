using System;
using System.Collections.Generic;
using System.Numerics;
using Dalamud.Interface;
using Dalamud.Interface.Internal;
using Dalamud.Interface.Utility;
using Dalamud.Utility;
using ImGuiNET;

namespace OPP.fr;

internal static class ImguiUtil
{

    public static void DrawText(this ImDrawListPtr drawList, Vector2 pos, string text, uint col, bool stroke, bool centerAlignX = true, uint strokecol = 4278190080u)
	{
		if (centerAlignX)
		{
			pos -= new Vector2(ImGui.CalcTextSize(text).X, 0f) / 2f;
		}
		if (stroke)
		{
			drawList.AddText(pos + new Vector2(-1f, -1f), strokecol, text);
			drawList.AddText(pos + new Vector2(-1f, 1f), strokecol, text);
			drawList.AddText(pos + new Vector2(1f, -1f), strokecol, text);
			drawList.AddText(pos + new Vector2(1f, 1f), strokecol, text);
		}
		drawList.AddText(pos, col, text);
	}

    public static void DrawTextNoSwift(this ImDrawListPtr drawList, Vector2 pos, string text, uint col, bool stroke, bool centerAlignX = true, uint strokecol = 4278190080u)
    {
        if (centerAlignX)
        {
            pos -= new Vector2(ImGui.CalcTextSize(text).X, 0f) / 2f;
        }
        if (stroke)
        {
            drawList.AddText(pos + new Vector2(-1f, -1f), strokecol, text);
            drawList.AddText(pos + new Vector2(-1f, 1f), strokecol, text);
            drawList.AddText(pos + new Vector2(1f, -1f), strokecol, text);
            drawList.AddText(pos + new Vector2(1f, 1f), strokecol, text);
        }
        drawList.AddText(pos, col, text);
    }

    public static void DrawMapTextDot(this ImDrawListPtr drawList, Vector2 pos, string str, uint fgcolor, uint bgcolor)
	{
		if (!string.IsNullOrWhiteSpace(str))
		{
			drawList.DrawText(pos, str, fgcolor, true, centerAlignX: true, bgcolor);
		}
		drawList.AddCircleFilled(pos, 5f, fgcolor);
        if (1f != 0f)
        {
            drawList.AddCircle(pos, 5f, bgcolor, 0, 1f);
        }
    }	





    public static Vector2 Rotate(this Vector2 vin, float rotation)
    {
        return vin.Rotate(new Vector2((float)Math.Sin(rotation), (float)Math.Cos(rotation)));
    }
    public static Vector2 Rotate(this Vector2 vin, Vector2 rotation)
    {
        rotation = rotation.Normalize();
        return new Vector2(rotation.Y * vin.X + rotation.X * vin.Y, rotation.Y * vin.Y - rotation.X * vin.X);
    }
    public static Vector2 Normalize(this Vector2 v)
    {
        float num = v.Length();
        if (num != 0)
        {
            float num2 = 1f / num;
            v.X *= num2;
            v.Y *= num2;
            return v;
        }
        return v;
    }
}

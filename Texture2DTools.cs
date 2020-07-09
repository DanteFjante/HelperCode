using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Analytics;

namespace DefaultNamespace
{
    public static class Texture2DTools
    {
        public static Texture2D DrawOnTexture(Color color, float strength, int radius, ref Texture2D tex, params Vector2Int[] positions)
        {
            Color[] pixels = tex.GetPixels();
            foreach (var pos in positions)
            {
                for (int x = -radius; x < radius; x++)
                {
                    if (pos.x + x < 0 || pos.x + x > tex.width)
                        continue;
                    
                    for (int y = -radius; y < radius; y++)
                    {
                        int arraypos = pos.x + x + (pos.y + y) * tex.width;
                        if(arraypos < pixels.Length && arraypos > 0)
                            pixels[arraypos] = Color.Lerp(pixels[arraypos], color, strength);
                    }
                }
            }
            tex.SetPixels(pixels);
            tex.Apply();
            return tex;
        }

        public static void FillTexture(Color color, float strength, ref Texture2D tex)
        {

            Color[] pixels = tex.GetPixels();
            for (int i = 0; i < pixels.Length; i++)
            {
                pixels[i] = Color.Lerp(pixels[i], color, strength);
            }
            tex.SetPixels(pixels);
            tex.Apply();
        }

        public static void ReplaceColor(Color colorToReplace, Color replacement, RectInt area, ref Texture2D tex)
        {
            Color[] colors = tex.GetPixels(area.x, area.y, area.width, area.height);
            
            for (var i = 0; i < colors.Length; i++)
            {
                if (colors[i] == colorToReplace)
                    colors[i] = replacement;
            }
            
            tex.SetPixels(area.x, area.y, area.width, area.height,colors);
            tex.Apply();
        }
    }
}
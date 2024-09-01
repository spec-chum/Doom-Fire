using Raylib_cs;
using System.Numerics;

namespace Fire;

internal static class Program
{
    const int CanvasWidth = 320;
    const int CanvasHeight = 180;

    const int Scale = 4;
    const int WindowWidth = CanvasWidth * Scale;
    const int WindowHeight = CanvasHeight * Scale;

    static readonly Random random = new();

    static void Main()
    {
        ReadOnlySpan<Color> colors = [
            new Color(0, 0, 0, 255),
            new Color(7, 7, 7, 255),
            new Color(31, 7, 7, 255),
            new Color(47, 15, 7, 255),
            new Color(71, 15, 7, 255),
            new Color(87, 23, 7, 255),
            new Color(103, 31, 7, 255),
            new Color(119, 31, 7, 255),
            new Color(143, 39, 7, 255),
            new Color(159, 47, 7, 255),
            new Color(175, 63, 7, 255),
            new Color(191, 71, 7, 255),
            new Color(199, 71, 7, 255),
            new Color(223, 79, 7, 255),
            new Color(223, 87, 7, 255),
            new Color(223, 87, 7, 255),
            new Color(215, 95, 7, 255),
            new Color(215, 95, 7, 255),
            new Color(215, 103, 15, 255),
            new Color(207, 111, 15, 255),
            new Color(207, 119, 15, 255),
            new Color(207, 127, 15, 255),
            new Color(207, 135, 23, 255),
            new Color(199, 135, 23, 255),
            new Color(199, 143, 23, 255),
            new Color(199, 151, 31, 255),
            new Color(191, 159, 31, 255),
            new Color(191, 159, 31, 255),
            new Color(191, 167, 39, 255),
            new Color(191, 167, 39, 255),
            new Color(191, 175, 47, 255),
            new Color(183, 175, 47, 255),
            new Color(183, 183, 47, 255),
            new Color(183, 183, 55, 255),
            new Color(207, 207, 111, 255),
            new Color(223, 223, 159, 255),
            new Color(239, 239, 199, 255)
        ];

        Raylib.InitWindow(WindowWidth, WindowHeight, "Fire");
        Raylib.SetTargetFPS(30);

        Image canvas = Raylib.GenImageColor(CanvasWidth, CanvasHeight, Color.Black);
        Texture2D texture = Raylib.LoadTextureFromImage(canvas);

        Span<int> buffer = stackalloc int[CanvasWidth * CanvasHeight];
        buffer[^CanvasWidth..].Fill(colors.Length - 1);

        Span<Color> framebuffer;
        unsafe
        {
            framebuffer = new((Color *)canvas.Data, buffer.Length);
        }

        while (!Raylib.WindowShouldClose())
        {
            DoFire(buffer);
            UpdateFramebuffer(framebuffer, buffer, colors);

            Raylib.BeginDrawing();

            Raylib.UpdateTexture<Color>(texture, framebuffer);
            Raylib.DrawTextureEx(texture, Vector2.Zero, 0, Scale, Color.White);

            Raylib.EndDrawing();
        }

        Raylib.UnloadImage(canvas);
        Raylib.UnloadTexture(texture);
    }

    static void UpdateFramebuffer(Span<Color> framebuffer, ReadOnlySpan<int> buffer, ReadOnlySpan<Color> colors)
    {
        for (int i = 0; i < framebuffer.Length; i++)
        {
            framebuffer[i] = colors[buffer[i]];
        }
    }

    static void DoFire(Span<int> buffer)
    {
        for (int x = 0; x < CanvasWidth; x++)
        {
            for (int y = 1; y < CanvasHeight; y++)
            {
                int src = (y * CanvasWidth) + x;
                int pixel = buffer[src];
                if (pixel == 0)
                {
                    buffer[src - CanvasWidth] = 0;
                }
                else
                {
                    int rand = random.Next(4);
                    int dst = 1 + src - rand;
                    buffer[dst - CanvasWidth] = pixel - (rand & 1);
                }
            }
        }
    }
}

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

    static readonly Color[] colors = [
        new (  0,   0,   0, 255),  new ( 31,   7,   7, 255),  new ( 71,  15,   7, 255),
        new (  7,   7,   7, 255),  new ( 47,  15,   7, 255),  new ( 87,  23,   7, 255),
        new (103,  31,   7, 255),  new (119,  31,   7, 255),  new (143,  39,   7, 255),
        new (159,  47,   7, 255),  new (175,  63,   7, 255),  new (191,  71,   7, 255),
        new (199,  71,   7, 255),  new (223,  79,   7, 255),  new (223,  87,   7, 255),
        new (223,  87,   7, 255),  new (215,  95,   7, 255),  new (215,  95,   7, 255),
        new (215, 103,  15, 255),  new (207, 111,  15, 255),  new (207, 119,  15, 255),
        new (207, 127,  15, 255),  new (207, 135,  23, 255),  new (199, 135,  23, 255),
        new (199, 143,  23, 255),  new (199, 151,  31, 255),  new (191, 159,  31, 255),
        new (191, 159,  31, 255),  new (191, 167,  39, 255),  new (191, 167,  39, 255),
        new (191, 175,  47, 255),  new (183, 175,  47, 255),  new (183, 183,  47, 255),
        new (183, 183,  55, 255),  new (207, 207, 111, 255),  new (223, 223, 159, 255),
        new (239, 239, 199, 255)
    ];

    static void Main()
    {
        Raylib.InitWindow(WindowWidth, WindowHeight, "Fire");
        Raylib.SetTargetFPS(30);

        Image canvas = Raylib.GenImageColor(CanvasWidth, CanvasHeight, Color.Black);
        Texture2D texture = Raylib.LoadTextureFromImage(canvas);

        Span<int> buffer = stackalloc int[CanvasWidth * CanvasHeight];
        buffer[^CanvasWidth..].Fill(colors.Length - 1);

        Span<Color> framebuffer;
        unsafe
        {
            framebuffer = new((Color*)canvas.Data, buffer.Length);
        }

        while (!Raylib.WindowShouldClose())
        {
            DoFire(buffer);
            UpdateFramebuffer(framebuffer, buffer);

            Raylib.BeginDrawing();

            Raylib.UpdateTexture<Color>(texture, framebuffer);
            Raylib.DrawTextureEx(texture, Vector2.Zero, 0, Scale, Color.White);

            Raylib.EndDrawing();
        }

        Raylib.UnloadImage(canvas);
        Raylib.UnloadTexture(texture);
    }

    static void UpdateFramebuffer(Span<Color> framebuffer, ReadOnlySpan<int> buffer)
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
            for (int y = CanvasWidth; y < buffer.Length; y += CanvasWidth)
            {
                int src = y + x;
                int pixel = buffer[src];
                if (pixel == 0)
                {
                    buffer[src - CanvasWidth] = pixel;
                }
                else
                {
                    int rand = Random.Shared.Next(4);
                    int dst = 1 + src - rand;
                    buffer[dst - CanvasWidth] = pixel - (rand & 1);
                }
            }
        }
    }
}

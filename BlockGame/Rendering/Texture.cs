using BlockGame.OpenGL;
using StbImageSharp;

namespace BlockGame.Rendering
{
    public readonly struct Texture
    {
        readonly ColorComponents _colorComponents;
        readonly int _height;
        readonly int _width;
        readonly int _graphicsPointer;

        public readonly int Height => _height;
        public readonly int Width => _width;
        public readonly int GlPointer => _graphicsPointer;
        public readonly ColorComponents ColorComponents => _colorComponents;


        public Texture(byte[] imageBuffer, GraphicsDevice device)
        {
            ImageResult imageData = ImageResult.FromMemory(imageBuffer);

            _height = imageData.Height;
            _width = imageData.Width;

            _colorComponents = imageData.SourceComp;

            _graphicsPointer = device.CreateTexture2D(Width, Height, ColorComponents, imageData.Data);
        }
    }
}

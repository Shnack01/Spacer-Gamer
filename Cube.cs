using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SpacerGamer;
/// <summary>
/// A class for rendering a cube
/// </summary>
public class Cube
{
    /// <summary>
    /// The vertices of the cube
    /// </summary>
    VertexBuffer vertices;

    /// <summary>
    /// The vertex indices of the cube
    /// </summary>
    IndexBuffer indices;

    /// <summary>
    /// The effect to use rendering the cube
    /// </summary>    BasicEffect effect;
    BasicEffect effect;
    /// <summary>
    /// The game this cube belongs to 
    /// </summary>
    Game game;
        /// <summary>
    /// Initialize the vertex buffer
    /// </summary>
    public void InitializeVertices()
    {
        var vertexData = new VertexPositionColor[] { 
            new VertexPositionColor() { Position = new Vector3(-3,  3, -3), Color = Color.Blue },
            new VertexPositionColor() { Position = new Vector3( 3,  3, -3), Color = Color.Green },
            new VertexPositionColor() { Position = new Vector3(-3, -3, -3), Color = Color.Red },
            new VertexPositionColor() { Position = new Vector3( 3, -3, -3), Color = Color.Cyan },
            new VertexPositionColor() { Position = new Vector3(-3,  3,  3), Color = Color.Blue },
            new VertexPositionColor() { Position = new Vector3( 3,  3,  3), Color = Color.Red },
            new VertexPositionColor() { Position = new Vector3(-3, -3,  3), Color = Color.Green },
            new VertexPositionColor() { Position = new Vector3( 3, -3,  3), Color = Color.Cyan }
        };
        vertices = new VertexBuffer(
            game.GraphicsDevice,            // The graphics device to load the buffer on 
            typeof(VertexPositionColor),    // The type of the vertex data 
            8,                              // The count of the vertices 
            BufferUsage.None                // How the buffer will be used
        );
        vertices.SetData<VertexPositionColor>(vertexData);
    }

        /// <summary>
    /// Initializes the index buffer
    /// </summary>
    public void InitializeIndices()
    {
        var indexData = new short[]
        {
            0, 1, 2, // Side 0
            2, 1, 3,
            4, 0, 6, // Side 1
            6, 0, 2,
            7, 5, 6, // Side 2
            6, 5, 4,
            3, 1, 7, // Side 3 
            7, 1, 5,
            4, 5, 0, // Side 4 
            0, 5, 1,
            3, 7, 2, // Side 5 
            2, 7, 6
        };
        indices = new IndexBuffer(
            game.GraphicsDevice,            // The graphics device to use
            IndexElementSize.SixteenBits,   // The size of the index 
            36,                             // The count of the indices
            BufferUsage.None                // How the buffer will be used
        );
        indices.SetData<short>(indexData);
    }
        /// <summary>
    /// Initializes the BasicEffect to render our cube
    /// </summary>
    void InitializeEffect()
    {
        effect = new BasicEffect(game.GraphicsDevice);
        effect.World = Matrix.Identity;
        effect.View = Matrix.Identity;
        var vp = game.GraphicsDevice.Viewport;
        effect.Projection = Matrix.CreateOrthographicOffCenter(0, vp.Width, vp.Height, 0, -1000, 1000);
        effect.VertexColorEnabled = true;
    }

    public void Draw(Vector2 position, float radius, float angle)
    {
        effect.World = Matrix.CreateScale(radius / 3f)
                    * Matrix.CreateRotationX(angle)
                    * Matrix.CreateRotationY(angle * 0.7f)
                    * Matrix.CreateTranslation(position.X, position.Y, 0);

        var gd = game.GraphicsDevice;
        gd.DepthStencilState = DepthStencilState.Default;
        gd.BlendState = BlendState.Opaque;
        gd.RasterizerState = RasterizerState.CullNone; 

        effect.CurrentTechnique.Passes[0].Apply();
        gd.SetVertexBuffer(vertices);
        gd.Indices = indices;
        gd.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, 12);
    }
        /// <summary>
    /// Constructs a cube instance
    /// </summary>
    /// <param name="game">The game that is creating the cube</param>
    public Cube(Game game)
    {
        this.game = game;
        InitializeVertices();
        InitializeIndices();
        InitializeEffect();
    }
        /// <summary>
    /// Updates the Cube
    /// </summary>
    /// <param name="gameTime"></param>
    public void Update(GameTime gameTime)
    {
        float angle = (float)gameTime.TotalGameTime.TotalSeconds;
        // Look at the cube from farther away while spinning around it
        effect.View = Matrix.CreateRotationY(angle) * Matrix.CreateLookAt(
            new Vector3(0, 5, -10),
            Vector3.Zero,
            Vector3.Up
        ); 
    }
}
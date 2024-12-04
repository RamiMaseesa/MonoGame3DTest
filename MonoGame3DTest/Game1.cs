using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame3DTest.Code;

namespace MonoGame3DTest;

public class Game1 : Game
{
     private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private BasicEffect basicEffect;
        private Camera camera;
        
        private Model model;
        private Texture2D texture;
        // triangle
        VertexPositionColor[] triangleVertices;
        VertexBuffer vertexBuffer;
        public Game1()
        {
            camera = new Camera(this);
            
            _graphics = new GraphicsDeviceManager(this);
            _graphics.PreferredBackBufferWidth = 1920;
            _graphics.PreferredBackBufferHeight = 1080;

            Utility.windowWidth = _graphics.PreferredBackBufferWidth;
            Utility.windowHeight = _graphics.PreferredBackBufferHeight;
            Utility.centerOfScreen = new Vector2(Utility.windowWidth, Utility.windowHeight);

            _graphics.IsFullScreen = false;
            Content.RootDirectory = "Content";
            IsMouseVisible = false;
            
            
        }

        protected override void Initialize()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            camera.Initialize();
            
            basicEffect = new BasicEffect(GraphicsDevice);
            basicEffect.Alpha = 1.0f;

            
            // triangle
            triangleVertices = new VertexPositionColor[3];
            triangleVertices[0] = new VertexPositionColor(new Vector3(
                0, 20, 0), Color.Red);
            triangleVertices[1] = new VertexPositionColor(new Vector3(-
                20, -20, 0), Color.Green);
            triangleVertices[2] = new VertexPositionColor(new Vector3(
                20, -20, 0), Color.Blue);
            
            vertexBuffer = new VertexBuffer(GraphicsDevice, typeof(VertexPositionColor), 3, BufferUsage.WriteOnly);
            vertexBuffer.SetData<VertexPositionColor>(triangleVertices);
            
            texture = Content.Load<Texture2D>("GoodDoom");
            GraphicsDevice.BlendState = BlendState.AlphaBlend;
            
            base.Initialize();
        }
    

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            camera.Update(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            //Turn off culling so we see both sides of our render
            RasterizerState rasterizerState = new RasterizerState();
            rasterizerState.CullMode = CullMode.None;
            GraphicsDevice.RasterizerState = rasterizerState;
            
            //DrawMesh(model);
            
            // Define vertices for a textured quad
            VertexPositionTexture[] quadVertices = new VertexPositionTexture[]
            {
                new VertexPositionTexture(new Vector3(-1, 1, 0), new Vector2(0, 0)),  // Top-left
                new VertexPositionTexture(new Vector3(1, 1, 0), new Vector2(1, 0)),   // Top-right
                new VertexPositionTexture(new Vector3(-1, -1, 0), new Vector2(0, 1)), // Bottom-left
                new VertexPositionTexture(new Vector3(1, -1, 0), new Vector2(1, 1))   // Bottom-right
            };

            short[] quadIndices = new short[] { 0, 1, 2, 1, 3, 2 }; // Define two triangles

            // Create a vertex and index buffer
            VertexBuffer vertexBuffer = new VertexBuffer(GraphicsDevice, typeof(VertexPositionTexture), quadVertices.Length, BufferUsage.WriteOnly);
            vertexBuffer.SetData(quadVertices);
            IndexBuffer indexBuffer = new IndexBuffer(GraphicsDevice, IndexElementSize.SixteenBits, quadIndices.Length, BufferUsage.WriteOnly);
            indexBuffer.SetData(quadIndices);

            GraphicsDevice.SetVertexBuffer(vertexBuffer);
            GraphicsDevice.Indices = indexBuffer;

            // Set up BasicEffect
            basicEffect.VertexColorEnabled = false;
            basicEffect.TextureEnabled = true;
            basicEffect.Texture = texture;
            basicEffect.View = camera.viewMatrix;
            basicEffect.Projection = camera.projectionMatrix;
            basicEffect.World = Matrix.CreateTranslation(0, 0, -15); // Position quad in 3D space

            foreach (EffectPass pass in basicEffect.CurrentTechnique.Passes)
            {
                pass.Apply();
                GraphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, quadIndices.Length / 3);
            }
            base.Draw(gameTime);
        }

        private void DrawMesh(Model model)
        {
            foreach(ModelMesh mesh in model.Meshes)
            {
                foreach(BasicEffect effect in mesh.Effects)
                {
                    //effect.EnableDefaultLighting();
                    effect.AmbientLightColor = new Vector3(0f, 0f, 0f);
                    effect.View = camera.viewMatrix;
                    effect.World = camera.worldMatrix;
                    effect.Projection = camera.projectionMatrix;
                }
                mesh.Draw();
            }
        }
}
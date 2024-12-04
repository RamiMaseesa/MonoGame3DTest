using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace MonoGame3DTest.Code;

public class Camera : GameObject
{
    //Camera
    Vector3 camTarget;
    Vector3 camPosition;
    public Matrix projectionMatrix;
    public Matrix viewMatrix;
    public Matrix worldMatrix;
    
    private MouseState previousMouseState;
    
    public Camera(Game game) : base(game)
    {
        
    }

    public override void Initialize()
    {
        // MVP MATRIX
        
        camTarget = new Vector3(0f, 0f, 0f);
        camPosition = new Vector3(0f, 0f, -10f);

        projectionMatrix = Matrix.CreatePerspectiveFieldOfView(
            MathHelper.ToRadians(45f), // FOV
            game.GraphicsDevice.DisplayMode.AspectRatio,
            1, // start seeing
            1000f); // stop seeing
        viewMatrix = Matrix.CreateLookAt(camPosition, camTarget, Vector3.Up); // orientatie
        worldMatrix = Matrix.CreateWorld(camTarget, Vector3.Forward, Vector3.Up); // pos in world?
        
        previousMouseState = Mouse.GetState();
    }

    public override void Update(GameTime gameTime)
    {
        
        MouseState currentMouseState = Mouse.GetState();
        
        
        float rotationSpeed = 0.005f; // Adjust sensitivity
        float movementSpeed = 0.1f;  // Adjust movement speed
        
        // Calculate mouse delta
        float deltaX = currentMouseState.X - previousMouseState.X;
        float deltaY = currentMouseState.Y - previousMouseState.Y;
    
        // Rotate horizontally around the Y-axis
        if (deltaX != 0)
        {
            Matrix rotationY = Matrix.CreateRotationY(-deltaX * rotationSpeed);
            Vector3 direction = camTarget - camPosition;
            direction = Vector3.Transform(direction, rotationY);
            camTarget = camPosition + direction;
        }
    
        // Rotate vertically around the perpendicular axis (cross product)
        if (deltaY != 0)
        {
            Vector3 direction = camTarget - camPosition;
            Vector3 right = Vector3.Cross(Vector3.Up, direction); // Perpendicular axis
            right.Normalize();
    
            Matrix rotationX = Matrix.CreateFromAxisAngle(right, deltaY * rotationSpeed);
            Vector3 newDirection = Vector3.Transform(direction, rotationX);
    
            // Clamp the pitch to avoid flipping
            float pitchLimit = 0.95f; // Cosine of max pitch angle
            if (Vector3.Dot(Vector3.Up, Vector3.Normalize(newDirection)) < pitchLimit &&
                Vector3.Dot(Vector3.Up, Vector3.Normalize(newDirection)) > -pitchLimit)
            {
                camTarget = camPosition + newDirection;
            }
        }
    
        // Reset mouse position to the center of the screen
        if (currentMouseState.X <= 30 || currentMouseState.X >= Utility.windowWidth - 30 ||
            currentMouseState.Y <= 30 || currentMouseState.Y >= Utility.windowHeight - 30)
        {
            Mouse.SetPosition((int)Utility.windowWidth / 2, (int)Utility.windowHeight / 2);
            previousMouseState = Mouse.GetState(); // Update to avoid large jumps
        }
        else
        {
            previousMouseState = currentMouseState;
        }
    
        // Handle camera movement (WASD + QE keys)
        Vector3 movement = Vector3.Zero;
        KeyboardState keyboardState = Keyboard.GetState();
    
        Vector3 forwardMove = Vector3.Normalize(camTarget - camPosition);
        Vector3 rightMove = Vector3.Cross(Vector3.Up, forwardMove);
        rightMove.Normalize();
        Vector3 up = Vector3.Up;
        
        if (keyboardState.IsKeyDown(Keys.W)) movement += forwardMove * movementSpeed;
        if (keyboardState.IsKeyDown(Keys.S)) movement -= forwardMove * movementSpeed;
        if (keyboardState.IsKeyDown(Keys.A)) movement += rightMove * movementSpeed;
        if (keyboardState.IsKeyDown(Keys.D)) movement -= rightMove * movementSpeed;
        if (keyboardState.IsKeyDown(Keys.Q)) movement -= up * movementSpeed;
        if (keyboardState.IsKeyDown(Keys.E)) movement += up * movementSpeed;
    
        if (movement != Vector3.Zero)
        {
            movement.Normalize();
            camPosition += movement;
            camTarget += movement;
        }
    
        // Update view matrix
        viewMatrix = Matrix.CreateLookAt(camPosition, camTarget, Vector3.Up);
    }
}
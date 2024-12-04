using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGame3DTest.Code;

public class GameObject
{
    protected Game game;
    public GameObject(Game game)
    {
        this.game = game;
    }
    
    public virtual void Initialize()
    {

    }
    public virtual void LoadContent(ContentManager content)
    {

    }
    public virtual void Update(GameTime gameTime)
    {

    }
    public virtual void Draw(SpriteBatch spriteBatch)
    {

    }
}
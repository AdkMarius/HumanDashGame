using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using HumanDash.Manager;

namespace HumanDash;

public class HumanDashGame : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;

    private Texture2D _skyTexture; // Texture pour les nuages
    private SkyManager _skyManager; // Gestionnaire des nuages

    public HumanDashGame()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        // Initialisation de base
        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        // Charger la texture des nuages
        _skyTexture = Content.Load<Texture2D>("decor"); // Assurez-vous que "cloud.png" est dans le pipeline de contenu

        // Initialiser le SkyManager avec la texture des nuages
        _skyManager = new SkyManager(_skyTexture);
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
            Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        // Mettre à jour les nuages
        _skyManager.Update(gameTime);

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        // Dessiner les nuages
        _spriteBatch.Begin();
        _skyManager.Draw(gameTime, _spriteBatch);
        _spriteBatch.End();

        base.Draw(gameTime);
    }
}

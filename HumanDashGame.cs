using HumanDash.Entities;
using HumanDash.Enum;
using HumanDash.Manager;
using HumanDash.System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Vector2 = System.Numerics.Vector2;

namespace HumanDash;

public class HumanDashGame : Game
{
    private const int WINDOW_WIDTH = 1200;
    private const int WINDOW_HEIGHT = 600;
    
    private const int RUNNER_DEFAULT_POSX = 20;
    private const int RUNNER_DEFAULT_POSY = 300;
    
    private const string RUNNER_TEXTURE_NAME = "runner";
    private const string SLIDING_RUNNER_TEXTURE_NAME = "sliding-runner";
    private const string PNEU_TEXTURE_NAME = "pneu";
    private const string OISEAUX_TEXTURE_NAME = "oisaux";
    private const string PILE_DE_CARTONS_TEXTURE_NAME = "pile_de_cartons";
    private const string DECOR_TEXTURE_NAME = "decor";
    private const string BRIQUES_TEXTURE_NAME = "briques";
    private const string BUTTON_PRESS_SOUND_NAME = "button-press";
    private const string HIT_OBSTACLE_SOUND_NAME = "hit";
    private const string SCORE_REACHED_SOUND_NAME = "score";

    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    
    private Texture2D _runnerTexture;
    private Texture2D _slidingRunnerTexture;
    private Texture2D _pneuTexture;
    private Texture2D _oisauxTexture;
    private Texture2D _pile_de_cartonsTexture;
    private Texture2D _decorTexture;
    private Texture2D _briquesTexture;
    private SoundEffect _buttonPressSound;
    private SoundEffect _scoreReachedSound;
    private SoundEffect _hitObstacleSound;
    
    private Avatar _avatar;
    private EntityManager _entityManager;
    private InputController _inputController;
    
    public GameState GameState { get; set; }

    public HumanDashGame()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
        _entityManager = new EntityManager();
        GameState = GameState.Initial;
    }

    protected override void Initialize()
    {
        // TODO: Add your initialization logic here
        
        _graphics.PreferredBackBufferWidth = WINDOW_WIDTH;
        _graphics.PreferredBackBufferHeight = WINDOW_HEIGHT;
        _graphics.ApplyChanges();
        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        // TODO: use this.Content to load your game content here
        _runnerTexture = Content.Load<Texture2D>(RUNNER_TEXTURE_NAME);
        _slidingRunnerTexture = Content.Load<Texture2D>(SLIDING_RUNNER_TEXTURE_NAME);
        _buttonPressSound = Content.Load<SoundEffect>(BUTTON_PRESS_SOUND_NAME);

        _avatar = new Avatar(_slidingRunnerTexture, _runnerTexture, new Vector2(RUNNER_DEFAULT_POSX, RUNNER_DEFAULT_POSY), _buttonPressSound);
        _inputController = new InputController(_avatar);
        
        _entityManager.AddEntity(_avatar);
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
            Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        // TODO: Add your update logic here
        _inputController.ProcessControls(gameTime);
        _entityManager.UpdateEntities(gameTime);
        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        // TODO: Add your drawing code here
        _spriteBatch.Begin();
        _entityManager.DrawEntities(gameTime, _spriteBatch);
        _spriteBatch.End();
        
        base.Draw(gameTime);
    }
}
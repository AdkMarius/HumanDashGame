using System;
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
    private const int WINDOW_WIDTH = 545;
    private const int WINDOW_HEIGHT = 300;
    
    private const int RUNNER_DEFAULT_POSX = 2;
    private const int RUNNER_DEFAULT_POSY = 188;
    
    private const string RUNNER_TEXTURE_NAME = "runner";
    private const string SLIDING_RUNNER_TEXTURE_NAME = "sliding-runner";
    private const string PNEU_TEXTURE_NAME = "pneu";
    private const string OISEAUX_TEXTURE_NAME = "oisaux";
    private const string PILE_DE_CARTONS_TEXTURE_NAME = "pile_de_cartons";
    private const string DECOR_TEXTURE_NAME = "decor";
    private const string BRIQUES_TEXTURE_NAME = "briques";
    private const string SOL_TEXTURE_NAME = "sol";
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
    private Texture2D _solTexture;
    private SoundEffect _buttonPressSound;
    private SoundEffect _scoreReachedSound;
    private SoundEffect _hitObstacleSound;
    
    private Avatar _avatar;
    private EntityManager _entityManager;
    private InputController _inputController;
    private GroundManager _groundManager;
    
    public GameState GameState { get; set; }
    
    private KeyboardState _previousKeyboardState;

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
    
    private void avatar_JumpComplete(object sender, EventArgs e)
    {
        if (GameState == GameState.Transition)
        {
            GameState = GameState.Playing;
            _avatar.Initialize();
        }
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        // TODO: use this.Content to load your game content here
        _runnerTexture = Content.Load<Texture2D>(RUNNER_TEXTURE_NAME);
        _slidingRunnerTexture = Content.Load<Texture2D>(SLIDING_RUNNER_TEXTURE_NAME);
        _solTexture = Content.Load<Texture2D>(SOL_TEXTURE_NAME);
        _buttonPressSound = Content.Load<SoundEffect>(BUTTON_PRESS_SOUND_NAME);

        _avatar = new Avatar(_slidingRunnerTexture, _runnerTexture, new Vector2(RUNNER_DEFAULT_POSX, RUNNER_DEFAULT_POSY), _buttonPressSound);
        _avatar.DrawOrder = 10;
        _avatar.JumpComplete += avatar_JumpComplete;
        
        _groundManager = new GroundManager(_solTexture, _avatar);
        _inputController = new InputController(_avatar);
        
        _entityManager.AddEntity(_avatar);
        _entityManager.AddEntity(_groundManager);
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
            Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        KeyboardState _keyboardState = new KeyboardState();
        if (GameState == GameState.Initial)
        {
            KeyboardState currentKeyboardState = Keyboard.GetState();
            bool isJumpingKeyPressed = currentKeyboardState.IsKeyDown(Keys.Up) || currentKeyboardState.IsKeyDown(Keys.Space);
            bool wasJumpingKeyPressed = _previousKeyboardState.IsKeyDown(Keys.Up) || _previousKeyboardState.IsKeyDown(Keys.Space);
        
            if (!wasJumpingKeyPressed && isJumpingKeyPressed)
            {
                if (_avatar.State != AvatarState.Jumping && _avatar.State != AvatarState.Falling)
                {
                    StartGame();
                }
            }
        }
        else if (GameState == GameState.Playing)
        {
            _inputController.ProcessControls(gameTime);
        } 
        
        _previousKeyboardState = _keyboardState;
        _entityManager.UpdateEntities(gameTime);
        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.White);

        _spriteBatch.Begin();
        _entityManager.DrawEntities(gameTime, _spriteBatch);
        _spriteBatch.End();
        
        base.Draw(gameTime);
    }

    private bool StartGame()
    {
        if (GameState != GameState.Initial)
        {
            return false;
        }

        GameState = GameState.Transition;
        _avatar.StartJump();
        return true;
    }
}
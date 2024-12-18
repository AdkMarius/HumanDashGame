using System;
using System.Timers;
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
    public const int WINDOW_WIDTH = 545;
    public const int WINDOW_HEIGHT = 300;
    
    private const int RUNNER_DEFAULT_POSX = 2;
    private const int RUNNER_DEFAULT_POSY = 188;
    
    private const string RUNNER_TEXTURE_NAME = "runner";
    private const string SLIDING_RUNNER_TEXTURE_NAME = "sliding-runner";
    private const string BIRDS_TEXTURE_NAME = "oisaux";
    private const string DECOR_TEXTURE_NAME = "decor";
    private const string GAME_OVER_TEXTURE_NAME = "game-over";
    private const string SOL_TEXTURE_NAME = "sol";
    private const string BUTTON_PRESS_SOUND_NAME = "button-press";
    private const string HIT_OBSTACLE_SOUND_NAME = "hit";
    private const string SCORE_REACHED_SOUND_NAME = "score";

    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    
    private Texture2D _runnerTexture;
    private Texture2D _slidingRunnerTexture;
    private Texture2D _birdsTexture;
    private Texture2D _decorTexture;
    private Texture2D _solTexture;
    private Texture2D _spriteSheet;
    private SoundEffect _buttonPressSound;
    private SoundEffect _scoreReachedSound;
    private SoundEffect _hitObstacleSound;
    
    private Avatar _avatar;
    private readonly EntityManager _entityManager;
    private InputController _inputController;
    private GroundManager _groundManager;
    private ObstacleManager _obstacleManager;
    private GameOverScreen _gameOverScreen;
    private ScoreBoard _scoreBoard;
    
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

    private void avatar_Died(object sender, EventArgs e)
    {
        GameState = GameState.GameOver;
        _obstacleManager.IsEnabled = false;
        _gameOverScreen.IsEnabled = true;
        _scoreBoard.setHighScore();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        // TODO: use this.Content to load your game content here
        _runnerTexture = Content.Load<Texture2D>(RUNNER_TEXTURE_NAME);
        _slidingRunnerTexture = Content.Load<Texture2D>(SLIDING_RUNNER_TEXTURE_NAME);
        _solTexture = Content.Load<Texture2D>(SOL_TEXTURE_NAME);
        _decorTexture = Content.Load<Texture2D>(DECOR_TEXTURE_NAME);
        _spriteSheet = Content.Load<Texture2D>(GAME_OVER_TEXTURE_NAME);
        _buttonPressSound = Content.Load<SoundEffect>(BUTTON_PRESS_SOUND_NAME);
        _hitObstacleSound = Content.Load<SoundEffect>(HIT_OBSTACLE_SOUND_NAME);

        _avatar = new Avatar(_slidingRunnerTexture, _runnerTexture, new Vector2(RUNNER_DEFAULT_POSX, RUNNER_DEFAULT_POSY), _buttonPressSound, _hitObstacleSound);
        _avatar.DrawOrder = 10;
        _avatar.JumpComplete += avatar_JumpComplete;
        _avatar.Died += avatar_Died;
        
        _scoreBoard = new ScoreBoard(_spriteSheet, new Vector2(WINDOW_WIDTH - 150, WINDOW_HEIGHT - 270), _avatar);

        _gameOverScreen = new GameOverScreen(_spriteSheet) { IsEnabled = false };
        _gameOverScreen.Position =
            new Vector2(WINDOW_WIDTH / 2 - GameOverScreen.GAME_OVER_SPRITE_WIDTH / 2, WINDOW_HEIGHT / 2 - 30);
        _gameOverScreen.Replay += game_replay;
        
        _groundManager = new GroundManager(_solTexture, _avatar);
        
        _obstacleManager = new ObstacleManager(_avatar, _decorTexture, _scoreBoard);
        _obstacleManager.IsEnabled = true;
        
        _inputController = new InputController(_avatar);
        
        _entityManager.AddEntity(_avatar);
        _entityManager.AddEntity(_groundManager);
        _entityManager.AddEntity(_obstacleManager);
        _entityManager.AddEntity(_gameOverScreen);
        _entityManager.AddEntity(_scoreBoard);
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

    private void game_replay(object sender, EventArgs e)
    {
        if (GameState != GameState.GameOver)
            return;

        GameState = GameState.Playing;
        _avatar.Initialize();
        _avatar.Position = new Vector2(RUNNER_DEFAULT_POSX, RUNNER_DEFAULT_POSY);
        _obstacleManager.RemoveAllObstacles();
        _obstacleManager.Initialize();
        _gameOverScreen.IsEnabled = false;
        _scoreBoard.Score = 0;
    }
}
using System;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using HumanDash.Graphics;
using HumanDash.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HumanDash.Entities;

[XmlRoot("jeu:game", Namespace = "http://www.univ-grenoble-alpes.fr/l3miage/humanDash")]
[Serializable]
public class ScoreBoard : IGameEntity
{
    private const int TEXTURE_COORDS_NUMBER_X = 655;
    private const int TEXTURE_COORDS_NUMBER_Y = 0;
    private const int TEXTURE_COORDS_NUMBER_WIDTH = 10;
    private const int TEXTURE_COORDS_NUMBER_HEIGHT = 13;

    private const int TEXTURE_COORDS_HI_X = 755;
    private const int TEXTURE_COORDS_HI_Y = 0;
    private const int TEXTURE_COORDS_HI_WIDTH = 20;
    private const int TEXTURE_COORDS_HI_HEIGHT = 13;

    private const int HI_TEXT_MARGIN = 28;

    private const int NUMBER_DIGITS_TO_DRAW = 5;
    private const float SCORE_INCREMENT_MULTIPLIER = 0.05f;

    private XmlDocument doc;
    private XmlNamespaceManager nsmgr;
    
    private static string binPath = AppDomain.CurrentDomain.BaseDirectory;
    private static string projectDirectory = Directory.GetParent(binPath)?.Parent?.Parent?.Parent?.FullName;
    private string FilePath = Path.Combine(projectDirectory, "humanDashGame.xml");

    [XmlIgnore] public double Score { get; set; }
    
    [XmlIgnore] public int DisplayScore => (int)Math.Floor(Score);
    
    [XmlElement("highScore")] public int HighestScore { get; private set; }
    
    [XmlIgnore] public bool HasHighScore => HighestScore > 0;
    
    [XmlIgnore] public Vector2 Position { get; private set; }
    
    [XmlIgnore] public int DrawOrder { get; set; } = 100;

    private Avatar _avatar;
    
    private Texture2D _texture;
    private Sprite _scoreSprite;
    private Sprite _hIScoreSprite;

    public ScoreBoard(Texture2D texture, Vector2 position, Avatar avatar)
    {
        Position = position;
        _avatar = avatar;
        _texture = texture;
        
        // chargement du fichier
        doc = new XmlDocument();
        doc.Load(FilePath);
            
        // definir l'espace de nom
        nsmgr = new XmlNamespaceManager(doc.NameTable);
        nsmgr.AddNamespace("jeu", "http://www.univ-grenoble-alpes.fr/l3miage/humanDash");
    }
    
    public void Update(GameTime gameTime)
    {
        Score += _avatar.Speed * SCORE_INCREMENT_MULTIPLIER * gameTime.ElapsedGameTime.TotalSeconds;
    }

    public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        if (HasHighScore)
        {
            DrawScore(spriteBatch, HighestScore, Position.X);
            _hIScoreSprite = new Sprite(_texture, TEXTURE_COORDS_HI_X, TEXTURE_COORDS_HI_Y, TEXTURE_COORDS_HI_WIDTH, 
                TEXTURE_COORDS_HI_HEIGHT);
            _hIScoreSprite.Draw(spriteBatch, new Vector2(Position.X - HI_TEXT_MARGIN, Position.Y));
        }
        
        DrawScore(spriteBatch, DisplayScore, Position.X + 80);
    }

    private void DrawScore(SpriteBatch spriteBatch, int score, float startPosX)
    {
        int[] scoreDigits = SplitDigits(score);
        float posX = startPosX;
        
        foreach (int scoreDigit in scoreDigits)
        {
            Vector2 screenPos = new Vector2(posX, Position.Y);
            
            _scoreSprite = new Sprite(_texture, TEXTURE_COORDS_NUMBER_X + scoreDigit * TEXTURE_COORDS_NUMBER_WIDTH, 
                TEXTURE_COORDS_NUMBER_Y, TEXTURE_COORDS_NUMBER_WIDTH, TEXTURE_COORDS_NUMBER_HEIGHT);
            _scoreSprite.Draw(spriteBatch, screenPos);
            posX += TEXTURE_COORDS_NUMBER_WIDTH;
        }
    }

    private int[] SplitDigits(int input)
    {
        string inputString = input.ToString().PadLeft(NUMBER_DIGITS_TO_DRAW, '0');
        
        int[] result = new int[inputString.Length];
        for (int i = 0; i < inputString.Length; i++)
        {
            result[i] = (int)char.GetNumericValue(inputString[i]);
        }
        
        return result;
    }

    private Rectangle GetDigitTextureBounds(int digit)
    {
        if (digit < 0 || digit > 9)
        {
            throw new ArgumentOutOfRangeException(nameof(digit), "The digit must be between 0 or 9");
        }

        int posX = TEXTURE_COORDS_NUMBER_X + digit * TEXTURE_COORDS_NUMBER_WIDTH;
        int posY = TEXTURE_COORDS_NUMBER_Y;

        return new Rectangle(posX, posY, TEXTURE_COORDS_NUMBER_WIDTH, TEXTURE_COORDS_NUMBER_HEIGHT);
    }

        public void GetHighScoreFromXmlFile()
        {
            XmlNode highScoreNode = doc.SelectSingleNode("//jeu:highScore", nsmgr);
            if (highScoreNode != null)
            {
                string highScoreString = highScoreNode.InnerText;
                HighestScore = int.Parse(highScoreString);
            }
    }

    public void SetHighScore()
    {
        if (HighestScore == 0)
            HighestScore = DisplayScore;
        else if (HighestScore < DisplayScore)
            HighestScore = DisplayScore;
        
        XmlNode highScoreNode = doc.SelectSingleNode("//jeu:highScore", nsmgr);
        if (highScoreNode != null)
        {
            highScoreNode.InnerText = Convert.ToString(HighestScore); 
            doc.Save(FilePath);
        } 
    }
}
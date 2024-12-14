using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace HumanDash.Manager
{
    public class SkyManager
    {
        private const int SKY_Y_POSITION = 50; // Position verticale des nuages
        private const int SKY_SPEED = 100; // Vitesse de déplacement des nuages
        private const int SCREEN_WIDTH = 800; // Largeur de la fenêtre 

        private Texture2D _skyTexture; // Texture contenant tous les éléments
        private List<Rectangle> _cloudSourceRectangles; // Zones de découpe des nuages
        private List<Vector2> _cloudPositions; // Positions actuelles des nuages
        private int _cloudWidth; // Largeur d'un nuage
        private Random _random;

        public SkyManager(Texture2D skyTexture)
        {
            _skyTexture = skyTexture;
            _cloudSourceRectangles = new List<Rectangle>();
            _cloudPositions = new List<Vector2>();
            _random = new Random();

            InitializeClouds();
        }

        private void InitializeClouds()
        {
            // Définir les rectangles des nuages dans la texture
            _cloudSourceRectangles.Add(new Rectangle(0, 0, 50, 25));  // Premier nuage
            _cloudSourceRectangles.Add(new Rectangle(55, 0, 50, 25)); // Deuxième nuage
            _cloudWidth = _cloudSourceRectangles[0].Width;

            // Initialiser les positions des nuages pour qu'ils remplissent l'écran
            for (int i = 0; i < SCREEN_WIDTH / _cloudWidth + 2; i++)
            {
                AddCloud(i * _cloudWidth);
            }
        }

        private void AddCloud(float xPosition)
        {
            // Ajouter un nuage à une position donnée
            int randomCloudIndex = _random.Next(_cloudSourceRectangles.Count);
            _cloudPositions.Add(new Vector2(xPosition, SKY_Y_POSITION));
        }

        public void Update(GameTime gameTime)
        {
            // Déplacer chaque nuage vers la gauche
            for (int i = 0; i < _cloudPositions.Count; i++)
            {
                _cloudPositions[i] = new Vector2(
                    _cloudPositions[i].X - SKY_SPEED * (float)gameTime.ElapsedGameTime.TotalSeconds,
                    _cloudPositions[i].Y
                );
            }

            // Vérifier si un nuage est sorti de l'écran, et le repositionner à droite
            for (int i = 0; i < _cloudPositions.Count; i++)
            {
                if (_cloudPositions[i].X < -_cloudWidth) // Si le nuage est sorti à gauche
                {
                    _cloudPositions[i] = new Vector2(GetRightmostCloudPosition() + _cloudWidth, SKY_Y_POSITION);
                }
            }
        }

        private float GetRightmostCloudPosition()
        {
            // Trouver le nuage le plus à droite
            float maxX = 0;
            foreach (var position in _cloudPositions)
            {
                if (position.X > maxX)
                    maxX = position.X;
            }
            return maxX;
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            // Dessiner chaque nuage à sa position respective
            for (int i = 0; i < _cloudPositions.Count; i++)
            {
                var sourceRectangle = _cloudSourceRectangles[i % _cloudSourceRectangles.Count];
                spriteBatch.Draw(_skyTexture, _cloudPositions[i], sourceRectangle, Color.White);
            }
        }
    }
}

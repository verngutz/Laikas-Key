using System.Collections.Generic;

using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;

namespace MiUtil
{
    /// <summary>
    /// MiAnimatingComponent defines an animation sequence as a queue of textures. 
    /// The textures are drawn one by one using a queue, with each texture staying on the queue for a certain amount of time.
    /// On top of that, the position, scaling, and rotation of the textures can be changed over time 
    /// by manipulating the sequence's movement curves.
    /// </summary>
    public class MiAnimatingComponent : MiDrawableComponent
    {
        private enum SpriteStates { DEFAULT }
        protected Dictionary<Enum, Queue<KeyValuePair<Texture2D, int>>> spriteQueue;
        private int spriteQueueTimer;
        public bool SpriteQueueLoop { get; set; }
        public bool SpriteQueueEnabled { get; set; }
        public Enum SpriteState { get; set; }

        public Curve XPositionOverTime { get; set; }
        public Curve YPositionOverTime { get; set; }
        public Curve WidthOverTime { get; set; }
        public Curve HeightOverTime { get; set; }
        public Curve RotationOverTime { get; set; }
        public Curve OriginXOverTime { get; set; }
        public Curve OriginYOverTime { get; set; }
        public Curve AlphaOverTime { get; set; }

        public bool MoveEnabled { get; set; }
        public bool ScaleEnabled { get; set; }
        public bool RotateEnabled { get; set; }
        public bool OriginChangeEnabled { get; set; }
        public bool AlphaChangeEnabled { get; set; }

        private ulong moveTimer;
        public ulong MoveTimer { get { return moveTimer; } }

        private ulong scaleTimer;
        public ulong ScaleTimer { get { return scaleTimer; } }

        private ulong rotateTimer;
        public ulong RotateTimer { get { return rotateTimer; } }

        private ulong originChangeTimer;
        public ulong OriginChangeTimer { get { return originChangeTimer; } }

        private ulong alphaChangeTimer;
        public ulong AlphaChangeTimer { get { return alphaChangeTimer; } }

        private Rectangle boundingRectangle;
        public Point Position
        {
            get { return boundingRectangle.Location; }
            set { boundingRectangle.Location = value; }
        }

        public int Width { get { return boundingRectangle.Width; } }
        public int Height { get { return boundingRectangle.Height; } }

        private Vector2 origin;
        private float rotation;

        private Color color;
        public Color Color
        {
            get { return color; }
            set { color = value; }
        }

        public MiAnimatingComponent(MiGame game, int init_x, int init_y, int width, int height) : this(game, init_x, init_y, width, height, 0, 0, 0) { }

        public MiAnimatingComponent(MiGame game, int init_x, int init_y, int width, int height, float init_rotate, float origin_x, float origin_y)
            : this(game, init_x, init_y, width, height, init_rotate, origin_x, origin_y, 255) { }

        public MiAnimatingComponent(MiGame game, int init_x, int init_y, int width, int height, float init_rotate, float origin_x, float origin_y, byte alpha)
            : base(game)
        {
            spriteQueue = new Dictionary<Enum, Queue<KeyValuePair<Texture2D, int>>>();
            spriteQueueTimer = 0;
            SpriteState = SpriteStates.DEFAULT;

            moveTimer = 0;

            boundingRectangle = new Rectangle(init_x, init_y, width, height);
            rotation = init_rotate;
            origin = new Vector2(origin_x, origin_y);

            XPositionOverTime = new Curve();
            XPositionOverTime.Keys.Add(new CurveKey(0, init_x));

            YPositionOverTime = new Curve();
            YPositionOverTime.Keys.Add(new CurveKey(0, init_y));

            WidthOverTime = new Curve();
            WidthOverTime.Keys.Add(new CurveKey(0, width));

            HeightOverTime = new Curve();
            HeightOverTime.Keys.Add(new CurveKey(0, height));

            RotationOverTime = new Curve();
            RotationOverTime.Keys.Add(new CurveKey(0, init_rotate));

            OriginXOverTime = new Curve();
            OriginXOverTime.Keys.Add(new CurveKey(0, origin_x));

            OriginYOverTime = new Curve();
            OriginYOverTime.Keys.Add(new CurveKey(0, origin_y));

            AlphaOverTime = new Curve();
            AlphaOverTime.Keys.Add(new CurveKey(0, alpha));
            color = Color.White;
            color.A = alpha;
        }

        /// <summary>
        /// Add a texture to this animation sequence.
        /// </summary>
        /// <param name="texture">The texture that will be drawn</param>
        /// <param name="spriteState">The state of the sprite when the texture will be shown</param>
        /// <param name="time">The number of frames the texture will be drawn</param>
        public virtual void AddTexture(Texture2D texture, Enum spriteState, int time)
        {
            if (!spriteQueue.ContainsKey(spriteState))
            {
                spriteQueue[spriteState] = new Queue<KeyValuePair<Texture2D, int>>();
            }
            spriteQueue[spriteState].Enqueue(new KeyValuePair<Texture2D, int>(texture, time));
        }

        public void AddTexture(Texture2D texture, int time)
        {
            AddTexture(texture, SpriteStates.DEFAULT, time);
        }

        public override void Update(GameTime gameTime)
        {
            if (MoveEnabled)
            {
                moveTimer++;
                boundingRectangle.X = (int)XPositionOverTime.Evaluate(MoveTimer);
                boundingRectangle.Y = (int)YPositionOverTime.Evaluate(MoveTimer);
            }

            if (ScaleEnabled)
            {
                scaleTimer++;
                boundingRectangle.Width = (int)WidthOverTime.Evaluate(MoveTimer);
                boundingRectangle.Height = (int)HeightOverTime.Evaluate(MoveTimer);
            }

            if (RotateEnabled)
            {
                rotateTimer++;
                rotation = RotationOverTime.Evaluate(RotateTimer);
            }

            if(OriginChangeEnabled)
            {
                originChangeTimer++;
                origin.X = OriginXOverTime.Evaluate(OriginChangeTimer);
                origin.Y = OriginYOverTime.Evaluate(OriginChangeTimer);
            }

            if (AlphaChangeEnabled)
            {
                alphaChangeTimer++;
                color.A = (byte)AlphaOverTime.Evaluate(AlphaChangeTimer);
            }

            if (SpriteQueueEnabled)
            {
                if (spriteQueueTimer++ > spriteQueue[SpriteState].Peek().Value)
                {
                    spriteQueueTimer = 0;
                    if (SpriteQueueLoop)
                        spriteQueue[SpriteState].Enqueue(spriteQueue[SpriteState].Dequeue());
                    else if (spriteQueue.Count > 1)
                        spriteQueue[SpriteState].Dequeue();
                }
            }
        }

        public override void Draw(GameTime gameTime)
        {
            Game.SpriteBatch.Draw(spriteQueue[SpriteState].Peek().Key, boundingRectangle, null, Color, rotation, origin, SpriteEffects.None, 0);
        }
    }
}

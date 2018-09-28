﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Maquina.Objects
{
    public enum BallTypes { Standard, Passthrough };

    public class Ball : GameObjectBase
    {
        public Ball(string name)
            : base(name)
        {
            BallSpeed = Speed.Normal;
            BallType = BallTypes.Standard;
            base.Breakable = false;
        }
        public Speed BallSpeed;
        public BallTypes BallType;
    }

    public enum BrickTypes { Standard, OneHit, Passthrough, Unbreakable };
    public class Brick : GameObjectBase
    {
        public Brick(string name, BrickTypes ballType) : base(name) { }
    }

    public enum PlayerTypes { Human, AI, Dummy };
    public class Player : GameObjectBase
    {
        public Player(string name, PlayerTypes plrType) : base(name) { }
    }
}

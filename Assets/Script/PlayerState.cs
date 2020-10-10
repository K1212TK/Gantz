using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerState
{

    //  下記プレイヤー状態クラス

    //  立ち状態
    public class StandState : State
    {
        public override string getStateName()
        {
            return "Idle";
        }
        public override int getAimStateType()
        {
            return 0;
        }
        public override int getAttackStateType()
        {
            return 0;
        }
    }
    //移動状態
    public class MoveState : State
    {
        public float inputVertical;
        public float inputHorizontal;
        public override string getStateName()
        {
            return "Move";
        }
        public override int getAimStateType()
        {
            return 0;
        }
        public override int getAttackStateType()
        {
            return 0;
        }
    }
    //ジャンプ状態
    public class JumpState : State
    {
        public override string getStateName()
        {
            return "Jump";
        }
        public override int getAimStateType()
        {
            return 0;
        }
        public override int getAttackStateType()
        {
            return 0;
        }
    }
    //銃のAIM状態
    public class AimGunState : State
    {
        public override string getStateName()
        {
            return "AimGun";
        }
        public override int getAimStateType()
        {
            return 1;
        }
        public override int getAttackStateType()
        {
            return 0;
        }
    }
    //射撃状態
    public class ShootingState : State
    {
        public override string getStateName()
        {
            return "Shooting";
        }
        public override int getAimStateType()
        {
            return 1;
        }
        public override int getAttackStateType()
        {
            return 1;
        }
    }

    //Aim移動状態
    public class AimMoveState : State
    {
        public override string getStateName()
        {
            return "AimMove";
        }
        public override int getAimStateType()
        {
            return 1;
        }
        public override int getAttackStateType()
        {
            return 0;
        }
    }

    //刀斬状態
    public class SlashSwordState : State
    {
        public override string getStateName()
        {
            return "SlashSword";
        }
        public override int getAimStateType()
        {
            return 0;
        }
        public override int getAttackStateType()
        {
            return 1;
        }
    }
    //ローリング状態
    public class RollingState : State
    {
        public override string getStateName()
        {
            return "Roll";
        }
        public override int getAimStateType()
        {
            return 0;
        }
        public override int getAttackStateType()
        {
            return 0;
        }
    }
    //ローリング状態
    public class SlideState : State
    {
        public override string getStateName()
        {
            return "Slide";
        }
        public override int getAimStateType()
        {
            return 0;
        }
        public override int getAttackStateType()
        {
            return 0;
        }
    }
}

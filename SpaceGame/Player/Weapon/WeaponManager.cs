using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Yna.Engine;
using Yna.Engine.Graphics;
using SpaceGame.Data.Description;

namespace SpaceGame.Player.Weapon
{
    public class WeaponManager : YnGroup
    {
        private SpacePlayer _player;
        private YnTimer _shootTimer;
        private Stack<int> _recycled;
        private bool _canShoot;
        private WeaponDescription _primaryWeaponDescription;
        private WeaponDescription _secondaryWeaponDescription;

        public WeaponManager(SpacePlayer player)
        {
            _player = player;
            _recycled = new Stack<int>();
            _canShoot = true;
            _shootTimer = new YnTimer(100, 0);
        }

        public override void Initialize()
        {
            base.Initialize();

            _primaryWeaponDescription = Registry.WeaponDescriptions[(int)_player.PrimaryWeaponType];
            _secondaryWeaponDescription = Registry.WeaponDescriptions[(int)_player.SecondaryWeaponType];

            _shootTimer.Completed += () => _canShoot = true;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            _shootTimer.Update(gameTime);
        }

        public override void Kill()
        {
            base.Kill();
        }

        public override void Revive()
        {
            base.Revive();
        }

        void weapon_Killed(object sender, EventArgs e)
        {
               int index = Members.IndexOf(sender as BaseWeapon);
        }

        public void Shoot(int type)
        {
            if (_canShoot)
            {
                if (type == 1)
                {
                    PrimaryWeapon weapon;

                    int[] minMax = GetStartEndValues();
      
                    for (int i = minMax[0]; i <= minMax[1]; i++)
                    {
                        if (_recycled.Count > 0)
                        {
                            weapon = Members.ElementAt(_recycled.Pop()) as PrimaryWeapon;
                            weapon.Reset(_player.Rectangle, i);
                        }
                        else
                        {
                            weapon = new PrimaryWeapon(_primaryWeaponDescription, _player.Rectangle, i);
                            weapon.SetOrigin(SpriteOrigin.Center);
                            weapon.Viewport = _player.Viewport;
                            weapon.Killed += weapon_Killed;
                            Add(weapon);
                        }

                        weapon.Rotation = MathHelper.ToRadians(_primaryWeaponDescription.Rotations[i]);

                        _shootTimer.Interval = weapon.Interval;
                    }
                }
                else
                    AddSecondaryWeapon();

                _canShoot = false;

                _shootTimer.Start();
            }
        }

        private void AddSecondaryWeapon()
        {
            SecondaryWeapon missile = new SecondaryWeapon(_player.Rectangle);
            missile.Killed += weapon_Killed;
            _shootTimer.Interval = missile.Interval;
            Add(missile);
        }

        private int[] GetStartEndValues()
        {
            int[] values = new int[2];

            switch (_player.BonusLevel)
            {
                case BonusLevel.None: values[0] = values[1] = 3; break;
                case BonusLevel.Level1: 
                    values[0] = 2;
                    values[1] = 4;
                    break;
                case BonusLevel.Level2:
                    values[0] = 1;
                    values[1] = 5;
                    break;
                case BonusLevel.Level3:
                case BonusLevel.Level4:
                    values[0] = 0;
                    values[1] = 6;
                    break;
            }

            return values;
        }
    }
}

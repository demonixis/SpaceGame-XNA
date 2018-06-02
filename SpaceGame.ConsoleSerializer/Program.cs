using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Intermediate;
using SpaceGame.Data;
using SpaceGame.Data.Collection;
using SpaceGame.Data.Description;

namespace SpaceGame.ConsoleSerializer
{
    class Program
    {
        static void Main(string[] args)
        {
            SerializeAll();
        }

        private static void SerializeAll()
        {
            #region Création des collections

            string[] spaceShiptTypeNames = Enum.GetNames(typeof(SpaceShipType));
            string[] weaponTypeNames = Enum.GetNames(typeof(WeaponType));
            string[] alienTypeNames = Enum.GetNames(typeof(EnnemyType));
            string[] levelTypeNames = Enum.GetNames(typeof(LevelType));

            SpaceCollection<SpaceShipDescription> spaceShipCollection = new SpaceCollection<SpaceShipDescription>(spaceShiptTypeNames.Length);
            SpaceCollection<WeaponDescription> weaponCollection = new SpaceCollection<WeaponDescription>(weaponTypeNames.Length);
            SpaceCollection<EnnemyDescription> alienCollection = new SpaceCollection<EnnemyDescription>(alienTypeNames.Length);
            SpaceCollection<LevelDescription> levelCollection = new SpaceCollection<LevelDescription>(levelTypeNames.Length);
            SpaceCollection<MessageDescription> messageCollection = new SpaceCollection<MessageDescription>();

            #endregion

            #region Paramètres XML

            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;

            #endregion

            #region Sérialisation des joueurs

            for (int i = 0; i < spaceShiptTypeNames.Length; i++)
                spaceShipCollection.Add(i, InitSpaceShipDescription((SpaceShipType)i, i));

            SerializeCollection<SpaceShipDescription>("players", spaceShipCollection, settings);

            #endregion

            #region Sérialisation des armes

            for (int i = 0; i < weaponTypeNames.Length; i++)
                weaponCollection.Add(i, InitWeaponDescription((WeaponType)i, "green", i));

            SerializeCollection<WeaponDescription>("weapons", weaponCollection, settings);

            #endregion

            #region Sérialisation des ennemies

            for (int i = 0; i < alienTypeNames.Length; i++)
                alienCollection.Add(i, InitEnnemyDescription((EnnemyType)i, i));

            SerializeCollection<EnnemyDescription>("enemies", alienCollection, settings);

            #endregion

            #region Sérialisation des niveaux

            for (int i = 0; i < levelTypeNames.Length; i++)
                levelCollection.Add(i, InitLevelDescription((LevelType)i, i));

            SerializeCollection<LevelDescription>("levels", levelCollection, settings);

            #endregion

            #region Sérialisation des messages

            messageCollection.Add(0, new MessageDescription() { Label = "mission_1_title", Language = "EN", Message = "Mission 1" });
            messageCollection.Add(0, new MessageDescription() { Label = "mission_1_title", Language = "FR", Message = "Mission 1" });
            messageCollection.Add(0, new MessageDescription() { Label = "mission_1_text", Language = "EN", Message = "Destroy all the enemmies" });
            messageCollection.Add(0, new MessageDescription() { Label = "mission_1_text", Language = "FR", Message = "Détruisez tous les ennemies !" });

            SerializeCollection<MessageDescription>("messages", messageCollection, settings);

            #endregion
        }

        private static void SerializeCollection<T>(string filename, SpaceCollection<T> collection, XmlWriterSettings settings)
        {
            using (XmlWriter writter = XmlWriter.Create(String.Format("{0}.xml", filename), settings))
            {
                IntermediateSerializer.Serialize(writter, collection, null);
            }
        }

        private static SpaceShipDescription InitSpaceShipDescription(SpaceShipType type, int id)
        {
            SpaceShipDescription spaceShipDescription = new SpaceShipDescription();
            spaceShipDescription.Id = id;
            spaceShipDescription.SpaceShitType = type;
            spaceShipDescription.Health = 100.0f;
            spaceShipDescription.Shield = 100.0f;

            switch (type)
            {
                case SpaceShipType.MercsA:

                    spaceShipDescription.AssetName = "Ship/MercsA_64";
                    spaceShipDescription.Model = "Raptor d1";
                    spaceShipDescription.Category = "Mercenary";
                    spaceShipDescription.Weight = 2405;
                    spaceShipDescription.ShipSpeed = new ShipSpeed(0.90f, 0.90f, 0.15f, 0.45f); ;
                    spaceShipDescription.Description = "It is a lightweight fighter, his only goal is destruction. \n";
                    spaceShipDescription.Description += "Mercenaries respect one thing: money. If you pay them well \n";
                    spaceShipDescription.Description += "so they will do a fantastic job, but otherwise .. they will turn against you!";
                    spaceShipDescription.PrimaryWeapons = "Three photon lasers";
                    spaceShipDescription.SecondaryWeapons = "Aggressor special missile";
                    spaceShipDescription.PrimaryWeaponId = 0;
                    spaceShipDescription.SecondaryWeaponId = 3;
                    break;

                case SpaceShipType.MercsB:
                    spaceShipDescription.AssetName = "Ship/MercsB_64";
                    spaceShipDescription.Model = "Mantra N4";
                    spaceShipDescription.Category = "Mercenary";
                    spaceShipDescription.Weight = 2500;
                    spaceShipDescription.ShipSpeed = new ShipSpeed(0.90f, 0.90f, 0.15f, 0.45f); ;
                    spaceShipDescription.Description = "A ship with an old alien technology.. Nobody knows exactly how the \n";
                    spaceShipDescription.Description += "ship works, but it is a formidable opponent in close combat";
                    spaceShipDescription.PrimaryWeapons = "Two tacticals lasers";
                    spaceShipDescription.SecondaryWeapons = "Unknow missile technologie";
                    spaceShipDescription.PrimaryWeaponId = 0;
                    spaceShipDescription.SecondaryWeaponId = 3;
                    break;

                case SpaceShipType.ShipA:
                    spaceShipDescription.AssetName = "Ship/ShipA_64";
                    spaceShipDescription.Model = "ZThunder 4";
                    spaceShipDescription.Category = "Destroyer";
                    spaceShipDescription.Weight = 6750;
                    spaceShipDescription.ShipSpeed = new ShipSpeed(0.90f, 0.90f, 0.15f, 0.45f); ;
                    spaceShipDescription.Description = "No description yet, but don't worry.. it's coming !";
                    spaceShipDescription.PrimaryWeapons = "Two tacticals lasers";
                    spaceShipDescription.SecondaryWeapons = "One missile launcher";
                    spaceShipDescription.PrimaryWeaponId = 1;
                    spaceShipDescription.SecondaryWeaponId = 3;
                    break;

                case SpaceShipType.ShipB:
                    spaceShipDescription.AssetName = "Ship/ShipB_64";
                    spaceShipDescription.Model = "Pulsar X6";
                    spaceShipDescription.Category = "Interceptor";
                    spaceShipDescription.Weight = 5560;
                    spaceShipDescription.ShipSpeed = new ShipSpeed(0.90f, 0.90f, 0.15f, 0.45f); ;
                    spaceShipDescription.Description = "No description yet too, I'm so sorry, but don't worry.. it's coming !";
                    spaceShipDescription.PrimaryWeapons = "Two tacticals lasers";
                    spaceShipDescription.SecondaryWeapons = "One missile launcher";
                    spaceShipDescription.Health = 100;
                    spaceShipDescription.PrimaryWeaponId = 2;
                    spaceShipDescription.SecondaryWeaponId = 3;
                    break;
            }

            return spaceShipDescription;
        }

        private static WeaponDescription InitWeaponDescription(WeaponType type, string color, int id)
        {
            WeaponDescription desc = new WeaponDescription();
            desc.Id = id;
            desc.SoundAsset = "";
            switch (type)
            {
                case WeaponType.Burst:
                    desc.AssetName = "Weapons/Burst/burst_blue";
                    desc.WeaponType = type;
                    desc.Interval = 45;
                    desc.Damage = 15;
                    desc.Speed = 6;
                    desc.Acceleration = new Vector2(2.0f, 2.0f);
                    break;

                case WeaponType.Laser:
                    desc.AssetName = "Weapons/Lasers/laser_green";
                    desc.WeaponType = type;
                    desc.Interval = 250;
                    desc.Damage = 55;
                    desc.Speed = 6;
                    desc.Acceleration = new Vector2(0, 2.0f);
                    break;

                case WeaponType.Missile:
                    desc.AssetName = "Weapons/Missile/missile";
                    desc.WeaponType = type;
                    desc.Interval = 1500;
                    desc.Damage = 105;
                    desc.Speed = 6;
                    desc.Acceleration = new Vector2(0, 2.0f);
                    break;

                case WeaponType.Sphere:
                    desc.AssetName = "Weapons/Sphere/sphere_blue";
                    desc.WeaponType = type;
                    desc.Interval = 75;
                    desc.Damage = 15;
                    desc.Speed = 8;
                    desc.Acceleration = new Vector2(2.0f, 2.0f);
                    break;
            }

            return desc;
        }

        private static EnnemyDescription InitEnnemyDescription(EnnemyType type, int id)
        {
            EnnemyDescription desc = new EnnemyDescription();
            desc.Id = id;

            switch (type)
            {
                case EnnemyType.AncienA:
                    desc.AnimationIndex = new int[] { 1, 2, 3, 4 };
                    desc.AnimationSize = new int[] { 32, 48 };
                    desc.AssetName = "Aliens/AnciensA_48";
                    desc.Framerate = 150;
                    desc.Speed = 2.0f;
                    desc.Health = 150;
                    break;

                case EnnemyType.AncienB:
                    desc.AnimationIndex = new int[] { 1, 2, 3, 4 };
                    desc.AnimationSize = new int[] { 75, 64 };
                    desc.AssetName = "Aliens/AnciensB_48";
                    desc.Framerate = 200;
                    desc.Speed = 2.0f;
                    desc.Health = 120;
                    break;

                case EnnemyType.OrganicsA:
                    desc.AnimationIndex = new int[] { 1, 2, 3, 4 };
                    desc.AnimationSize = new int[] { 48, 48 };
                    desc.AssetName = "Aliens/OrganicA_48";
                    desc.Framerate = 150;
                    desc.Speed = 1.0f;
                    desc.Health = 100;
                    break;

                case EnnemyType.OrganicsB:
                    desc.AnimationIndex = new int[] { 1, 2, 3, 4 };
                    desc.AnimationSize = new int[] { 48, 48 };
                    desc.AssetName = "Aliens/OrganicB_48";
                    desc.Framerate = 150;
                    desc.Speed = 1.0f;
                    desc.Health = 100;
                    break;

                case EnnemyType.RobotA:
                    desc.AnimationIndex = new int[] { 1, 2, 3, 4 };
                    desc.AnimationSize = new int[] { 48, 48 };
                    desc.AssetName = "Aliens/RobotA_48";
                    desc.Framerate = 75;
                    desc.Speed = 2.0f;
                    desc.Health = 130;
                    break;

                case EnnemyType.RobotB:
                    desc.AnimationIndex = new int[] { 1, 2, 3, 4 };
                    desc.AnimationSize = new int[] { 63, 48 };
                    desc.AssetName = "Aliens/RobotB_48";
                    desc.Framerate = 75;
                    desc.Speed = 2.0f;
                    desc.Health = 100;
                    break;
            }

            return desc;
        }

        private static LevelDescription InitLevelDescription(LevelType type, int id)
        {
            LevelDescription desc = new LevelDescription();

            switch (type)
            {
                case LevelType.Interior:
                    desc.AuthorizedEnnemy = new EnnemyType[] { EnnemyType.OrganicsA, EnnemyType.RobotB };
                    desc.Id = id;
                    desc.LevelTimeMax = 60 * 5;
                    desc.Name = "First Escape";
                    break;

                case LevelType.Space:
                    desc.AuthorizedEnnemy = new EnnemyType[] { EnnemyType.AncienA, EnnemyType.RobotA };
                    desc.Id = id;
                    desc.LevelTimeMax = 60 * 5;
                    desc.Name = "First Escape";
                    break;
            }

            return desc;
        }
    }
}

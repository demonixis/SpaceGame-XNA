#define MONOGAME
using SpaceGame.Data;

namespace SpaceGame.Manager
{
    public class StorageDevice
    {

    }

    public class StorageContainer
    {

    }

    public class StorageManager
    {
        private StorageDevice _storageDevice;

        private const string SaveGameFilename = "SpaceGame.sav";
        private const string OptionGameFilename = "Config.sav";

        public StorageManager()
        {
#if !MONOGAME
            IAsyncResult result =  StorageDevice.BeginShowSelector(PlayerIndex.One, null, null);
            _storageDevice = StorageDevice.EndShowSelector(result);
#endif
        }

        public bool LoadGameSave()
        {
#if !MONOGAME
            StorageContainer container = GetContainer("Saves");

            if (!container.FileExists(SaveGameFilename))
                return false;
            
            Stream stream = container.OpenFile(SaveGameFilename, FileMode.Open);

            XmlSerializer serializer = new XmlSerializer(typeof(List<GameScore>));

            List<GameScore> scores = (List<GameScore>)serializer.Deserialize(stream);

            Registry.ScoreManager.GameScores = scores;
            
            stream.Close();

            container.Dispose();
#endif
            return true;
        }

        public void LoadGameConfiguration()
        {

        }

        public void RecordGameSave()
        {
#if !MONOGAME
            StorageContainer container = GetContainer("Saves");

            if (container.FileExists(SaveGameFilename))
                container.DeleteFile(SaveGameFilename);

            Stream stream = container.CreateFile(SaveGameFilename);
            
            XmlSerializer serializer = new XmlSerializer(typeof(List<GameScore>));
            serializer.Serialize(stream, Registry.ScoreManager.GameScores);
            stream.Close();

            container.Dispose();
#endif
        }

        public void RecordGameConfiguration(GameConfiguration gameConfig)
        {
#if !MONOGAME
            StorageContainer container = GetContainer("Config");

            if (container.FileExists(SaveGameFilename))
                container.DeleteFile(SaveGameFilename);

            Stream stream = container.CreateFile(SaveGameFilename);

            XmlSerializer serializer = new XmlSerializer(typeof(GameConfiguration));
            serializer.Serialize(stream, gameConfig);
            stream.Close();

            container.Dispose();
#endif
        }

        private StorageContainer GetContainer(string name)
        {
#if !MONOGAME
            IAsyncResult result = _storageDevice.BeginOpenContainer(name, null, null);
            result.AsyncWaitHandle.WaitOne();
            
            StorageContainer container = _storageDevice.EndOpenContainer(result);
            result.AsyncWaitHandle.Close();

            return container;
#else
            return null;
#endif
        }
    }
}

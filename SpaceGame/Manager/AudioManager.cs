using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using Yna.Engine;
#if SPEECH
using SpaceGame;
#endif
#if MONOGAME && WINDOWS
using WMPLib;
#endif

namespace SpaceGame.Manager
{
    public class AudioManager
    {
#if SPEECH
        private VocalSynthetizer vocalSynthetizer;
#endif

#if MONOGAME && WINDOWS
        private WindowsMediaPlayer mediaPlayer;
#endif

        private bool _soundEnabled;
        private float _soundVolume;

        private bool _musicEnabled;

        #region Propriétés

        /// <summary>
        /// Définie la rapidité avec laquelle la voix s'exprime
        /// </summary>
        public int VocalRate
        {
#if SPEECH
            get { return vocalSynthetizer.Rate; }
            set { vocalSynthetizer.Rate = value; }
#else
            get; set;
#endif
        }

        public VocalSynthetizer VocalSynthetize
        {
#if SPEECH
            get { return vocalSynthetizer; }
#else
            get; set;
#endif
        }

        /// <summary>
        /// Activation ou désactivation du son
        /// </summary>
        public bool SoundEnabled
        {
            get { return _soundEnabled; }
            set { _soundEnabled = value; }
        }

        /// <summary>
        /// réglage du volume sonore compris entre 0.0f et 1.0f
        /// </summary>
        public float SoundVolume
        {
            get { return _soundVolume; }
            set { _soundVolume = value; }
        }

        /// <summary>
        /// Activation ou désactivation de la musique
        /// </summary>
        public bool MusicEnabled
        {
            get { return _musicEnabled; }
            set { _musicEnabled = value; }
        }

        /// <summary>
        /// Réglage du volume de la musique compris entre 0.0f et 1.0f
        /// </summary>
        public float MusicVolume
        {
            get { return MediaPlayer.Volume; }
            set { MediaPlayer.Volume = value; }
        }

        #endregion

        public AudioManager()
        {
#if SPEECH
            vocalSynthetizer = new VocalSynthetizer();
#endif
            MusicEnabled = true;
            SoundEnabled = true;
            SoundVolume = 0.4f;
            MusicVolume = 0.6f;

#if MONOGAME && WINDOWS
            mediaPlayer = new WindowsMediaPlayer();
#endif
        }

        public void Initialize()
        {
#if SPEECH
            vocalSynthetizer.Initialize();
#endif
        }

        public void LoadContent()
        {
            YnG.Content.Load<Song>("Audio/kickit.mp3");
            YnG.Content.Load<SoundEffect>("Audio/laser1");
            YnG.Content.Load<SoundEffect>("Audio/laser9");
            YnG.Content.Load<SoundEffect>("Audio/gama-laser");
        }

        public bool SpeakAsync(string text)
        {
#if SPEECH
            vocalSynthetizer.SpeakAsync(text);
            return true;
#else
            return false;
#endif
        }

        public void PlaySound(string assetName, float volume, float pitch = 1.0f, float pan = 0.0f)
        {
            if (_soundEnabled)
            {
                if (volume > _soundVolume)
                    volume = _soundVolume;

                SoundEffect sound = YnG.Content.Load<SoundEffect>(assetName);
                sound.Play(volume, pitch, pan);
            }
        }

        public void PlayMusic(string assetName, bool repeat = true)
        {
            if (MusicEnabled)
            {
                if (MediaPlayer.State == MediaState.Playing)
                    MediaPlayer.Stop();
#if !MONOGAME
                Song music = YnG.Content.Load<Song>(assetName);
                MediaPlayer.IsRepeating = repeat;
                MediaPlayer.Play(music);
#elif MONOGAME && WINDOWS && OPENGL

                mediaPlayer.URL = "Content\\" + assetName.Replace("/", "\\") + ".wma";
                mediaPlayer.controls.play();
#endif
            }
        }

        public void StopMusic()
        {
            if (MediaPlayer.State == MediaState.Playing)
                MediaPlayer.Stop();
        }

        public void PauseMusic()
        {
            if (MediaPlayer.State == MediaState.Playing)
                MediaPlayer.Pause();
        }

        public void ResumeMusic()
        {
            if (MediaPlayer.State == MediaState.Paused)
                MediaPlayer.Resume();
        }

        public void Dispose()
        {
#if SPEECH
            vocalSynthetizer.Dispose();
#endif
            if (MediaPlayer.State == MediaState.Playing)
                MediaPlayer.Stop();
        }
    }
}

using System;
using System.Collections.Generic;
#if SPEECH
using System.Speech;
using System.Speech.Synthesis;
using System.Globalization;

namespace SpaceGame
{
    public class VocalSynthetizer
    {
        private SpeechSynthesizer speechSynth;
        private List<string> voices;

        public int Rate
        {
            get { return speechSynth.Rate; }
            set { speechSynth.Rate = value; }
        }

        public VocalSynthetizer()
        {
            speechSynth = new SpeechSynthesizer();
            speechSynth.SpeakCompleted += new EventHandler<SpeakCompletedEventArgs>(speechSynth_SpeakCompleted);
            voices = new List<string>();
        }

        public void Initialize()
        {
            var installedVoices = speechSynth.GetInstalledVoices();

            if (installedVoices.Count > 0)
            {
                foreach (InstalledVoice v in installedVoices)
                    voices.Add(v.VoiceInfo.Name);
            }
            else
                throw new Exception("[VocalSynthetizer] No voices founded on this computer");
        }

        

        public void SpeakAsync(string message, SayAs sayAs = SayAs.Text)
        {
            if (voices.Count > 0)
            {
                StopSpeak();

                PromptBuilder builder = new PromptBuilder(CultureInfo.CreateSpecificCulture("en-US"));
                builder.AppendTextWithHint(message, sayAs);
                speechSynth.SpeakAsync(builder);
            }
            else
                Initialize();
        }

        public void SpeakAsync(PromptBuilder builder)
        {
            if (voices.Count > 0)
            {
                StopSpeak();
                speechSynth.SpeakAsync(builder);
            }
            else
                Initialize();
        }

        public void Speak(string message, SayAs sayAs = SayAs.Text)
        {
            if (voices.Count > 0)
            {
                StopSpeak();

                PromptBuilder builder = new PromptBuilder(CultureInfo.CreateSpecificCulture("en-US"));
                builder.AppendTextWithHint(message, sayAs);
                speechSynth.Speak(builder);
            }
            else
                Initialize();
        }

        public bool StopSpeak()
        {
            if (speechSynth.State == SynthesizerState.Speaking)
            {
                Prompt prompt = speechSynth.GetCurrentlySpokenPrompt();

                if (prompt != null)
                {
                    speechSynth.SpeakAsyncCancel(prompt);
                    return true;
                }
            }

            return false;
        }

        void speechSynth_SpeakCompleted(object sender, SpeakCompletedEventArgs e)
        {
            if (e.Error != null)
                Console.Error.WriteLine(e.Error.Message);
        }

        public void Dispose()
        {
            if (speechSynth.State == SynthesizerState.Speaking)
                speechSynth.SpeakAsyncCancelAll();
            
            speechSynth.Dispose();
        }
    }
}
#else
namespace SpaceGame
{
    public class VocalSynthetizer
    {
	}
}
#endif

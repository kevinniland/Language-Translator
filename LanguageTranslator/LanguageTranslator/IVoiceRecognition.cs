using System;
using System.Collections.Generic;
using System.Text;

namespace LanguageTranslator
{
    public interface IVoiceRecognition
    {
        void startSpeechToText();
        void stopSpeechToText();
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace LanguageTranslator
{
    // This interface and it's methods will be implemented on Android to access it's platform specific APIs
    public interface IVoiceRecognition
    {
        void startSpeechToText();
        void stopSpeechToText();
    }
}

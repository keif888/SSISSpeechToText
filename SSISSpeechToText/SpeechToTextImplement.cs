using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Bing.Speech;
using Microsoft.SqlServer.Dts.Pipeline.Wrapper;
using CognitiveServicesAuthorization;
using NAudio;
using NAudio.Wave;

namespace Martin.SQLServer.Dts
{
    public class SpeechToTextImplement
    {
        public SpeechToTextImplement(IDTSComponentMetaData100 componentMetaData, string locale, Uri serviceUrl, string subscriptionKey)
        {
            this._componentMetaData = componentMetaData;
            this._preferences = new Preferences(locale, serviceUrl, new CognitiveServicesAuthorizationProvider(subscriptionKey));
        }

        /// <summary>
        /// A completed task
        /// </summary>
        private static readonly Task CompletedTask = Task.FromResult(true);

        /// <summary>
        /// Cancellation token used to stop sending the audio.
        /// </summary>
        private readonly CancellationTokenSource cts = new CancellationTokenSource();

        /// <summary>
        /// Stores the results from the recognition process.
        /// </summary>
        private List<string> recognitionResults = new List<string>();

        public List<string> Results
        {
            get { return recognitionResults; }
        }

        private IDTSComponentMetaData100 _componentMetaData;
        public IDTSComponentMetaData100 ComponentMetaData
        {
            get { return _componentMetaData; }
            set { _componentMetaData = value; }
        }

        private Preferences _preferences;






        public async Task ExecuteRecogniseAsync(string audioFile)
        {
            FileInfo inputFileInfo = new FileInfo(audioFile);
            WaveStream audioStream = null;
            Mp3FileReader mp3Stream = null;

            switch (inputFileInfo.Extension.ToLower())
            {
                case ".mp3":
                    mp3Stream = new Mp3FileReader(audioFile);
                    audioStream = WaveFormatConversionStream.CreatePcmStream(mp3Stream);
                    // Have to send to a physical file, as the NAudio stream doesn't interact with the
                    // Speech client nicely.
                    // ToDo: refactor to temporary file name.
                    WaveFileWriter.CreateWaveFile("tempfile.wav", audioStream);
                    break;
                default:
                    break;
            }

            //using (var inputReader = new AudioFileReader(audioFile))
            //{
            //    var mono = inputReader.ToMono(0.5f, 0.5f);
                
            //}
            // Create a a speech client
            using (var speechClient = new SpeechClient(this._preferences))
            {
                speechClient.SubscribeToRecognitionResult(this.OnRecognitionResultAsync);

                var deviceMetadata = new DeviceMetadata(DeviceType.Near, DeviceFamily.Desktop, NetworkType.Ethernet, OsName.Windows, "1607", "HyperV", "HyperV");
                var applicationMetadata = new ApplicationMetadata("SSISSpeachToText", "1.0.0.0");
                var requestMetadata = new RequestMetadata(Guid.NewGuid(), deviceMetadata, applicationMetadata, "SQL Server Integration Services");
                // ToDo: refactor to temporary file name.
                using (var audio = new FileStream("tempfile.wav", FileMode.Open, FileAccess.Read))
                    await speechClient.RecognizeAsync(new SpeechInput(audio, requestMetadata), this.cts.Token).ConfigureAwait(false);
            }

            if (audioStream != null)
            {
                audioStream.Dispose();
            }
            if (mp3Stream != null)
            {
                mp3Stream.Dispose();
            }
        }

        /// <summary>
        /// Invoked when the speech client receives a phrase recognition result(s) from the server.
        /// </summary>
        /// <param name="args">The recognition result.</param>
        /// <returns>
        /// A task
        /// </returns>
        public Task OnRecognitionResultAsync(RecognitionResult args)
        {
            var response = args;
            bool pbFireAgain = true;

            _componentMetaData.FireInformation(0, "OnRecognitionResultAsync", string.Format("***** Phrase Recognition Status = [{0}] ***", response.RecognitionStatus), string.Empty, 0, ref pbFireAgain);
            if (response.Phrases != null)
            {
                foreach (var result in response.Phrases)
                {
                    recognitionResults.Add(string.Format("{0} (Confidence:{1})", result.DisplayText, result.Confidence));
                }
            }
            return CompletedTask;
        }


    }
}

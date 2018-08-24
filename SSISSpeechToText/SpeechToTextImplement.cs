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



///  ToDo:
/// <summary>
/// Change to using client instead of server DLL's.
/// Add x86/x64 DLL switching as per:
/// https://support.microsoft.com/en-au/help/837908/how-to-load-an-assembly-at-runtime-that-is-located-in-a-folder-that-is
/// Potentially pull this entire class out to 2 separate DLL's, which are to be installed where the component can't find them, and use the above to load them.
/// 
/// Investigate migration to the preview speech services, as they provide handling for dual channel, and start of speech timings.
/// 
/// </summary>


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





        /// <summary>
        /// Takes a passed in file that is assumed to be mono and PCM encoded,
        /// and sends it to the speech client.
        /// </summary>
        /// <param name="audioFile">The full path and file name of a PCM encoded MONO channel file.</param>
        /// <returns></returns>
        public async Task ExecuteRecogniseAsync(string audioFile)
        {
            FileInfo inputFileInfo = new FileInfo(audioFile);
            WaveStream audioStream = null;
            Mp3FileReader mp3Stream = null;
            string tempAudioFile = audioFile;

            switch (inputFileInfo.Extension.ToLower())
            {
                case ".mp3":
                    mp3Stream = new Mp3FileReader(audioFile);
                    audioStream = WaveFormatConversionStream.CreatePcmStream(mp3Stream);
                    // Have to send to a physical file, as the NAudio stream doesn't interact with the
                    // Speech client nicely.
                    tempAudioFile = System.IO.Path.GetTempFileName();
                    WaveFileWriter.CreateWaveFile(tempAudioFile, audioStream);
                    break;
                default:
                    // Try and send the file to the speechClient, in the hope it is a PCM encoded file.
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
                using (var audio = new FileStream(tempAudioFile, FileMode.Open, FileAccess.Read))
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

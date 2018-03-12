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
            // Create a a speech client
            using (var speechClient = new SpeechClient(this._preferences))
            {
                speechClient.SubscribeToRecognitionResult(this.OnRecognitionResultAsync);

                // create an audio content and pass it a stream.
                using (var audio = new FileStream(audioFile, FileMode.Open, FileAccess.Read))
                {
                    var deviceMetadata = new DeviceMetadata(DeviceType.Near, DeviceFamily.Desktop, NetworkType.Ethernet, OsName.Windows, "1607", "HyperV", "HyperV");
                    var applicationMetadata = new ApplicationMetadata("SSISSpeachToText", "1.0.0.0");
                    var requestMetadata = new RequestMetadata(Guid.NewGuid(), deviceMetadata, applicationMetadata, "SQL Server Integration Services");

                    await speechClient.RecognizeAsync(new SpeechInput(audio, requestMetadata), this.cts.Token).ConfigureAwait(false);
                }
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

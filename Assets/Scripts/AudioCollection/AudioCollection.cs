using System.Collections;
using System.IO;
using TMPro;
using UnityEngine;

namespace AudioCollection
{
    public class AudioCollection : MonoBehaviour
    {
        [Tooltip("This script is used for recording the tapping audio in during the test in the android devices")]
        public TextMesh dataPath;

        private void Start()
        {
            StartRecording();
        }

        private AudioClip _clip;
        private byte[] _bytes;
        private bool _recording;


        private string microPhoneName;

        private void StartRecording()
        {
            microPhoneName = Microphone.devices[0];
            dataPath.text = microPhoneName;
            _clip = Microphone.Start(microPhoneName, false, 10, AudioSettings.outputSampleRate);
            _recording = true;
            dataPath.text = "Start Recording";
            StartCoroutine(Recording(10f));
        }

        IEnumerator Recording(float time)
        {
            yield return new WaitForSeconds(time);
            StopRecording();
        }

        private void Update()
        {
            // if (_recording && Microphone.GetPosition(microPhoneName) >= _clip.samples)
            // {
            //     dataPath.text = "Stop Recording";
            //     StopRecording();
            // }
        }

        private void StopRecording()
        {
            StartCoroutine(SaveAudio());
        }

        IEnumerator SaveAudio()
        {
            dataPath.text = "Save Recording";
            var position = Microphone.GetPosition(null);
            Microphone.End(null);
            var samples = new float[position * _clip.channels];
            _clip.GetData(samples, 0);
            _bytes = EncodeAsWAV(samples, _clip.frequency, _clip.channels);
            SaveRecording();
            _recording = false;
            yield break;
        }

        private byte[] EncodeAsWAV(float[] samples, int frequency, int channels)
        {
            using (var memoryStream = new MemoryStream(44 + samples.Length * 2))
            {
                using (var writer = new BinaryWriter(memoryStream))
                {
                    writer.Write("RIFF".ToCharArray());
                    writer.Write(36 + samples.Length * 2);
                    writer.Write("WAVE".ToCharArray());
                    writer.Write("fmt ".ToCharArray());
                    writer.Write(16);
                    writer.Write((ushort)1);
                    writer.Write((ushort)channels);
                    writer.Write(frequency);
                    writer.Write(frequency * channels * 2);
                    writer.Write((ushort)(channels * 2));
                    writer.Write((ushort)16);
                    writer.Write("data".ToCharArray());
                    writer.Write(samples.Length * 2);

                    foreach (var sample in samples)
                    {
                        writer.Write((short)(sample * short.MaxValue));
                    }
                }

                return memoryStream.ToArray();
            }
        }

        private void SaveRecording()
        {
            Debug.Log(Application.persistentDataPath + "/audioCollection.wav");
            dataPath.text = Application.persistentDataPath;
            File.WriteAllBytes(Application.persistentDataPath + "/audioCollection.wav", _bytes);
        }
    }
}
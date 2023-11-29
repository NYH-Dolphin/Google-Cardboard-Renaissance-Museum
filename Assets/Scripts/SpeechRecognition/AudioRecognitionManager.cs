using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using HuggingFace.API;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SpeechRecognition
{
    public class AudioRecognitionManager : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI textSpeech;
        [SerializeField] private Image imgRecordStatus; 

        private AudioClip _clip;
        private byte[] _bytes;
        private bool _recording;

        private delegate void OnTriggerAudioEvent();

        private Dictionary<string, OnTriggerAudioEvent> _audioEvents = new();

        private void Awake()
        {
            _audioEvents.Add("Move", OnMove);
            _audioEvents.Add("move", OnMove);
        }

        // Keep Recording
        private void Update()
        {
            if (!_recording)
            {
                StartRecording();
            }

            if (_recording && Microphone.GetPosition(null) >= _clip.samples)
            {
                StopRecording();
            }
        }

        private void StartRecording()
        {
            imgRecordStatus.color = Color.green;
            _clip = Microphone.Start(null, false, 4, 44100);
            _recording = true;
        }

        private void StopRecording()
        {
            StartCoroutine(SaveAudio());
        }

        IEnumerator SaveAudio()
        {
            imgRecordStatus.color = Color.red;
            textSpeech.text = string.Empty;
            var position = Microphone.GetPosition(null);
            Microphone.End(null);
            var samples = new float[position * _clip.channels];
            _clip.GetData(samples, 0);
            _bytes = EncodeAsWAV(samples, _clip.frequency, _clip.channels);
            SendRecording();
            yield return new WaitForSeconds(0.3f);
            _recording = false;
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

        private void SendRecording()
        {
            HuggingFaceAPI.AutomaticSpeechRecognition(_bytes, KeyWordDetect, error => { textSpeech.text = "Fail to connect"; });
            Debug.Log(Application.dataPath);
            // File.WriteAllBytes(Application.dataPath + "/test.wav", _bytes);
        }
        
        private void KeyWordDetect(string text)
        {
            Debug.Log(text);
            foreach (var key in _audioEvents.Keys)
            {
                if (text.Contains(key))
                {
                    textSpeech.text = key;
                    _audioEvents[key].Invoke();
                    break;
                }
            }
        }

        private void OnMove()
        {
            Debug.Log("Move");
        }
    }
}
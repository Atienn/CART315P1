using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioAnalyzer : MonoBehaviour {

    static short[] freqSeparators = { 60, 250, 500, 2000, 4000, 6000, 20000, short.MaxValue };

    [Header("General")]
    [SerializeField] AudioSource source;
    [SerializeField] short samplesAmt;
    [SerializeField] float visualScaling;

    float[] spectrumData;

    [Header("Waveform")]
    [SerializeField] LineRenderer waveform;
    Vector3[] wfVerticies;
    //float[] spectrumMax;

    [Header("Reduced Spectrum / Bands")]
    [SerializeField] byte separationMethod = 0;
    [SerializeField] Transform[] bands;
    float[] reduxSpectrData;
    [SerializeField] short[] reduxSpectrRanges;

    void Start() {

        //Replace samples amount with closest power of 2 (required for spectrum data).
        samplesAmt = (short)Mathf.ClosestPowerOfTwo(samplesAmt);
        //Initialize spectrum data array.
        spectrumData = new float[samplesAmt];
        //Top half never gets used >:(
        samplesAmt /= 2;


        //Redux spectrum data setup.
        {
            //Gets the number of consecutive powers of 2 (starting from 1) can fit in the amount of samples.
            byte reduxSpectrLength = (byte)(Mathf.Log(samplesAmt, 2));
            reduxSpectrRanges = new short[reduxSpectrLength];
            reduxSpectrData = new float[reduxSpectrLength];


            //Aims to follow the frequency range separators.
            if (separationMethod == 0) {
                int hertzPerSample = source.clip.frequency / samplesAmt;
                int totalHertz = 0;
                short sampleCount;

                for (int i = 0; i < reduxSpectrRanges.Length; i++) {
                    //Reset how many samples should go into the next band.
                    sampleCount = 0;

                    //Keep adding to the number of samples for the partition until
                    //it reaches/exceeds a frequency range separator.
                    while (totalHertz < freqSeparators[i]) {
                        totalHertz += hertzPerSample;
                        sampleCount++;
                    }

                    //Record how many samples should go into this partition.
                    reduxSpectrRanges[i] = sampleCount;
                }
            }

            //Logarithmically partitions the samples array. (Each partition is double the size of the previous)
            else {
                short currentRange = samplesAmt;
                for(int i = reduxSpectrRanges.Length - 1; i >= 0; i--) {
                    reduxSpectrRanges[i] = currentRange;
                    currentRange /= 2;
                }
                //for(int i = 0; i < reduxSpectrRanges.Length; i++) {
                //    reduxSpectrRanges[i] = (short)Mathf.Pow(2, i + 1);
                //}
                //By starting with the first partition 2 samples instead of 1, there are 2 extra samples 
                //at the end that have not been placed in any partition. Add them to the last one.
                //reduxSpectrRanges[reduxSpectrRanges.Length - 1] += 2;
            }
        }


        {
            //spectrumMax = new float[samplesAmt];

            //Initialize the waveform verticies.
            waveform.positionCount = samplesAmt;
            wfVerticies = new Vector3[samplesAmt];
            for (short i = 0; i < samplesAmt; i++) {
                wfVerticies[i] = new Vector3((25f / samplesAmt) * i - 12f, 0, -1f);

                /*
                pos[i] = new Vector3(
                    Mathf.Sin(Mathf.PI * i / samples),
                    Mathf.Cos(Mathf.PI * i / samples),
                    -1f
                );
                 */
            }
            waveform.SetPositions(wfVerticies);
        }
    }


    void Update() {
        WaveFormDirect();
        //BandListRedux();
    }

    void WaveFormDirect() {
        //Get data from audio clip.
        source.GetSpectrumData(spectrumData, 0, FFTWindow.Blackman);

        //Make verticies match position 
        for (short i = 0; i < samplesAmt; i++) {
            //if(spectrumData[i] > spectrumMax[i]) {
            //    spectrumMax[i] = spectrumData[i];
            //}

            wfVerticies[i].y = (Mathf.Sqrt(spectrumData[i]) * visualScaling + wfVerticies[i].y) / 2;

            //Average of 2 last frames.
            //wfVerticies[i].y = Mathf.Sqrt((spectrumData[i] + wfVerticies[i].y) / 2) * 50;

            //wfVerticies[i].y = ((spectrumData[i] + wfVerticies[i].y) / (spectrumMax[i] + 0.1f)) * 25;

            //Circular arrangement.
            //pos[i].x = (data[i]*5 + 1) * Mathf.Sin(Mathf.PI * i / samples);
            //pos[i].y = (data[i]*5 + 1) * Mathf.Cos(Mathf.PI * i / samples);
        }
        waveform.SetPositions(wfVerticies);
    }

    void BandListRedux() {
        source.GetSpectrumData(spectrumData, 0, FFTWindow.Blackman);
        
        float spectrSum;
        short spectrIndex = 0;

        for (short i = 0; i < reduxSpectrData.Length; i++) {
            spectrSum = 0;

            //Tally all the data for the partition's range.
            for(; spectrIndex < reduxSpectrRanges[i]; spectrIndex++) {
                spectrSum += spectrumData[spectrIndex];
            }
            //Set the band's data as the average of its partition.
            bands[i].localScale = new Vector3(1, spectrSum * visualScaling / reduxSpectrRanges[i], 1);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Went through several iterations, super messy.
public class WaveformDisplay : MonoBehaviour {


    [Header("General")]
    [SerializeField] AudioSource source;
    [SerializeField] float visualScaling;
    [SerializeField] short samplesAmt;

    float[] spectrumData;

    [Header("Waveform")]
    [SerializeField] LineRenderer waveform;
    [SerializeField] float waveWidth = 450f;
    Vector3[] wfVerticies;

    void Start() {

        //Replace samples amount with closest power of 2 (required for spectrum data).
        samplesAmt = (short)Mathf.ClosestPowerOfTwo(samplesAmt);
        //Initialize spectrum data array.
        spectrumData = new float[samplesAmt];
        //Top half never gets used >:(
        samplesAmt /= 2;


        {
            //Initialize the waveform verticies.
            waveform.positionCount = 4 * samplesAmt;
            wfVerticies = new Vector3[4 * samplesAmt];

            //Avoids re-doing same operations on each iteration.
            int samplesAmt2 = (samplesAmt * 2);
            int samplesAmt3 = (samplesAmt * 3);
            int j;

            for (int i = 0; i < samplesAmt; i++) {
                j = i;
                //Top-right.
                wfVerticies[j] = new Vector3((waveWidth / samplesAmt) * -i, -50f, 25f);

                j = i + samplesAmt;
                //Bottom-right
                wfVerticies[j] = wfVerticies[i];
                wfVerticies[j].z *= -1;

                //Bottom-left
                j = i + samplesAmt2;
                wfVerticies[j] = wfVerticies[i];
                wfVerticies[j].z *= -1;
                wfVerticies[j].x *= -1;

                j = i + samplesAmt3;
                //Top-left.
                wfVerticies[j] = wfVerticies[i];
                wfVerticies[j].x *= -1;
            }
        }

    }

    void Update() {
        WaveFormDirect();
    }

    void WaveFormDirect() {
        //Get data from audio clip.
        source.GetSpectrumData(spectrumData, 0, FFTWindow.Blackman);

        short i = 0, j;

        //Make verticies match position 
        for (j = 0; i < samplesAmt;) {
            wfVerticies[i++].z = spectrumData[j++] * visualScaling;
        }

        i++;
        for (j = 0; i < samplesAmt * 2;) {
            wfVerticies[i++].z = spectrumData[j++] * -visualScaling;
        }

        i++;
        for (j = 0; i < samplesAmt * 3;) {
            wfVerticies[i++].z = spectrumData[j++] * -visualScaling;
        }

        i++;
        for (j = 0; i < samplesAmt * 4;) {
            wfVerticies[i++].z = spectrumData[j++] * visualScaling;
        }

        waveform.SetPositions(wfVerticies);
    }
}

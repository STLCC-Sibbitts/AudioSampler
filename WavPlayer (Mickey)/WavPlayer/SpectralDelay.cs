/*using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace WavPlayer
{
    class SpectralDelay : Filter
    {
        public int EchoDelay { get; set; }
        public float EchoFactor { get; set; }

        private Queue<float> samples;

        public SpectralDelay(int length = 20000, float factor = .5f)
        {
            this.EchoDelay = length;
            this.EchoFactor = factor;

            for (int i = 0; i < length; i++)
                samples.Enqueue(0f);

        }

        public float addFilter(Complex[] sample)
        {
            Complex[] fftSamp = new Complex[sample.Length];
            Fourier.FFT(fftSamp, Fourier.Direction.Forward);
             

                return Math.Min(1, Math.Max(-1, sample + EchoFactor * samples.Dequeue()));
        }
    }
}
*/
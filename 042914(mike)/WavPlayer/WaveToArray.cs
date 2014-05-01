using NAudio.Wave;
using NAudio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
namespace WavPlayer
{
   class WaveToArray
    {
        public Array getFilteredStream(System.IO.Stream stream)
        {
       
            WaveFileReader wfr = new WaveFileReader(stream);
            WaveStream ws = WaveFormatConversionStream.CreatePcmStream(wfr);
            WaveStream baStream = new BlockAlignReductionStream(ws);
       
            WaveFormat format = new WaveFormat(baStream.WaveFormat.SampleRate, (int)baStream.WaveFormat.BitsPerSample, (int)baStream.WaveFormat.Channels);
          
            byte[] buffer1 = new byte[baStream.WaveFormat.BitsPerSample / 8];

      
            int SampleRate = baStream.WaveFormat.SampleRate;
           // wavFormat = baStream.WaveFormat;

      int bytesRead = 0;
      //long previousPosition = 0;
      List<Complex> lst = new List<Complex>();
      double currentValue;
      double avg = 0;
      while (baStream.Position < baStream.Length)
      {
        bytesRead = baStream.Read(buffer1, 0, baStream.WaveFormat.BitsPerSample / 8);

        if (wfr.WaveFormat.Channels > 1 /* && rdbReadTrack2.Checked == true*/) // Use 2nd channel
          bytesRead = baStream.Read(buffer1, 0, baStream.WaveFormat.BitsPerSample / 8);

        if (baStream.WaveFormat.BitsPerSample / 8 > 1)
        {
          currentValue = 100.0 * BitConverter.ToInt16(buffer1, 0) / short.MaxValue;
        }
        else // Special case for 8 bit signal
        {
          currentValue = 100.0 * 255 * (buffer1[0] - 128) / short.MaxValue;
        }
         
        lst.Add(currentValue);
        avg += currentValue;

        if (wfr.WaveFormat.Channels > 1/* && rdbReadTrack1.Checked == true */) // Skip 2nd channel
          bytesRead = baStream.Read(buffer1, 0, baStream.WaveFormat.BitsPerSample / 8);
      }

      avg /= baStream.Length;

      // ... Now lst contain the data I need!
            
      return lst.ToArray();
    }
   }
  }



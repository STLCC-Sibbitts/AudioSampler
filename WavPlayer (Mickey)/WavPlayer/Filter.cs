using System;


namespace WavPlayer
{
   public interface Filter
    {
        float addFilter(float sample);  
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComplexCompressor.Model
{
    public class MainModel
    {
        public MainModel() 
        {
            WindowSize = 10;
            BufferSize = 10;
            WindowSizeSliderValue = 10;
            BufferSizeSliderValue = 32;
        }
        public int WindowSize { get; set; }
        public int BufferSize { get; set; }

        public int WindowSizeSliderValue { get; set; }
        public int BufferSizeSliderValue { get; set; }
    }
}

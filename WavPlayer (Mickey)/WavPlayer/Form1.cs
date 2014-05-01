using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using NAudio.Wave;
using WavPlayer.Filters;

namespace WavPlayer
{
    public partial class Form1 : Form
    {
        //turn filter on and off
        bool addFilter = false;
        //default pad map
        private static Keys pad1 = Keys.Q;
        private static Keys pad2 = Keys.W;
        private static Keys pad3 = Keys.E;
        private static Keys pad4 = Keys.A;
        private static Keys pad5 = Keys.S;
        private static Keys pad6 = Keys.D;
        private static Keys pad7 = Keys.Z;
        private static Keys pad8 = Keys.X;
        private static Keys pad9 = Keys.C;

        //need to convert between keyevent and keypress ... between KeyChar and KeyCode. 
        char p1 = Convert.ToChar(pad1);
        char p2 = Convert.ToChar(pad2);
        char p3 = Convert.ToChar(pad3);
        char p4 = Convert.ToChar(pad4);
        char p5 = Convert.ToChar(pad5);
        char p6 = Convert.ToChar(pad6);
        char p7 = Convert.ToChar(pad7);
        char p8 = Convert.ToChar(pad8);
        char p9 = Convert.ToChar(pad9);


        public Form1()
        {
            InitializeComponent();
        }
        //use NAudio to read in wave files
        private WaveFileReader wave = null;
        private WaveFileReader wave2 = null;

        //keep track of how many pad samples are loaded
        private int numberOfSamplesSelected = 0;

        //to be used to open samples
        private OpenFileDialog multiSampleOpen = new OpenFileDialog();
    
        //use NAudio to play output sound
        private DirectSoundOut output = null;
        private DirectSoundOut output2 = null;

        //array to keep the filenames of selected samples
        public string[] openFileObjects;

        //DirectSoundOut and Stream objects for use when using filters
        private DirectSoundOut filterOutput = null;
        private BlockAlignReductionStream stream = null;
       
        private void pauseButton_Click(object sender, EventArgs e)
        {

            if(output != null)
            {
                if (output.PlaybackState == PlaybackState.Playing)
                    output.Pause();
                else if (output.PlaybackState == PlaybackState.Paused) 
                    output.Play();
                
            }

        }

        private void DisposeWave(DirectSoundOut outputx, WaveFileReader wavex)
        {
            if(output!=null)
            {
                if (output.PlaybackState == PlaybackState.Playing)
                    output.Stop();

                output.Dispose();
                output = null;
             }
            if (wave != null)
            {
                wave.Dispose();
                wave = null;
            }


           
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            //stop playback and use Dispose() to dump
            DisposeWave(output,wave);
            DisposeWave(output2, wave2);
            
            
            //make sure mic is not recording
            button7_Click(sender, e);
        }


        //play the filename # sampleNum out of openFileObject array
        private void playSample(int sampleNum)
        {
            
            wave2 = new WaveFileReader(openFileObjects[sampleNum]);  //(fileName(sampleNum))
            output2 = new DirectSoundOut();
            output2.Init(new WaveChannel32(wave2));

         



            if (wave2 != null)
            {
                //play sample without effects
                if(addFilter == false)
                    output2.Play();
                //if echo button was pressed addFilter is true so we use FilterStream to add echo filter
                
                if(addFilter != false)
                {
                    // WaveCHannel32 guarenties 32bit floating point for filtering
                    WaveChannel32 wave3 = new WaveChannel32(new WaveFileReader(openFileObjects[sampleNum]));

                    FilterStream delayedFilter = new FilterStream(wave3);
                    stream = new BlockAlignReductionStream(delayedFilter);

                    //apply a filter - make sure there is an effect assigned to each channel
                    for (int i = 0; i < wave2.WaveFormat.Channels; i++)
                        delayedFilter.filters.Add(new Echo());


                    filterOutput = new DirectSoundOut(200);
                    filterOutput.Init(stream);

                    filterOutput.Play();

                }
            }
            DisposeWave(output2, wave2);
            DisposeWave(filterOutput, wave2);
        }

     //echo button is on or off
        private void button4_Click(object sender, EventArgs e)
        {
            addFilter = !addFilter;
          
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void splitContainer1_Panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void splitContainer1_SplitterMoved(object sender, SplitterEventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {
            //create list of all our possible input options
            List<WaveInCapabilities> soundInputs = new List<WaveInCapabilities>();

            //populate list with available input options
            for (int i=0; i<WaveIn.DeviceCount; i++)
            {
                soundInputs.Add(WaveIn.GetCapabilities(i));
             }

            //make sure the old input options are cleared out
            listView1.Clear();
            
            //populate input options with available from list
            foreach (var element in soundInputs)
            {
                //populate list referencing name
                ListViewItem soundInputView = new ListViewItem(element.ProductName);
            
                //attach the amount of channels to each name in list
                soundInputView.SubItems.Add(new ListViewItem.ListViewSubItem(soundInputView, element.Channels.ToString()));
                
                //toolsToolStripMenuItem.DropDownItems.Add(soundInputView);
                listView1.Items.Add(soundInputView);
            }
        }

        private void selectInputToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private WaveIn micSourceStream = null;
        private DirectSoundOut micWavOut = null;

        private void button6_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count == 0) return;
            int chosenInputNumber = listView1.SelectedItems[0].Index;

            micSourceStream = new WaveIn();
            micSourceStream.DeviceNumber = chosenInputNumber;
            micSourceStream.WaveFormat = new WaveFormat(44100,WaveIn.GetCapabilities(chosenInputNumber).Channels);

            //use wavein to go between wave in and sound out
            WaveInProvider waveIn = new WaveInProvider(micSourceStream);
            
            micWavOut = new DirectSoundOut();
            micWavOut.Init(waveIn);

            //push recording into buffer
            micSourceStream.StartRecording();
            micWavOut.Play();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            if(micWavOut != null)
            {
                micWavOut.Stop();
                micWavOut.Dispose();
                micWavOut = null;
            }
            if(micSourceStream != null)
            {
                micSourceStream.StopRecording();
                micSourceStream.Dispose();
                micSourceStream = null;
            }
            //dispose for mic-record-to-wav
            if(micWaveWriter!=null)
            {
                micWaveWriter.Dispose();
                micWaveWriter = null;
            }


        }
        WaveFileWriter micWaveWriter = null;


        //Record Wave Button
        private void button8_Click(object sender, EventArgs e)
        {
            //make  sure input source is selected
            if (listView1.SelectedItems.Count == 0) return;

            //set a file to save wav file to
            SaveFileDialog micSaveWavDialog = new SaveFileDialog();
            micSaveWavDialog.Filter = "Wave File (*.wav|*.wav;";
            if(micSaveWavDialog.ShowDialog() != DialogResult.OK) return;



            int chosenInputNumber = listView1.SelectedItems[0].Index;


            micSourceStream = new WaveIn();
            micSourceStream.DeviceNumber = chosenInputNumber;
            
            //set up new waveformat using the chosen devices amount of channels
            micSourceStream.WaveFormat = new WaveFormat(44100, WaveIn.GetCapabilities(chosenInputNumber).Channels);

            
            micSourceStream.DataAvailable += new EventHandler<WaveInEventArgs>(micSourceStream_DataAvailable);
            micWaveWriter = new WaveFileWriter(micSaveWavDialog.FileName, micSourceStream.WaveFormat);

            micSourceStream.StartRecording();
        }
        
        private void micSourceStream_DataAvailable(object sender, WaveInEventArgs e)
        {
            if (micWaveWriter == null) return;
            micWaveWriter.WriteData(e.Buffer, 0, e.BytesRecorded);
            micWaveWriter.Flush();
        }


        //rand num 0-7
        public int getRandNumber()
        {
            int randNum;
                Random random = new Random();
            randNum = random.Next(7);
            return randNum;

        }

        //spit out rand color 
        public Color getRandColor()
        {
            Color[] colors = new Color[8];
            colors[0] = Color.Red;
            colors[1] = Color.BlueViolet;
            colors[2] = Color.White;
            colors[3] = Color.Yellow;
            colors[4] = Color.Green;
            colors[5] = Color.Blue;
            colors[6] = Color.DarkGreen;
            colors[7] = Color.Indigo;

            Color color = colors[getRandNumber()];
            return color;        
        }

        


        private void sampleButton1_Click(object sender, EventArgs e)
        {
            if (numberOfSamplesSelected>=1)
            {
                playSample(0);
                this.sampleButton1.BackColor = getRandColor();

            }

        }

        private void sampleButton2_Click(object sender, EventArgs e)
        {
            if (numberOfSamplesSelected >= 2)
            {
                playSample(1);
                //turnColor(sender);
                this.sampleButton2.BackColor = getRandColor();
            }

        }

        private void sampleButton3_Click(object sender, EventArgs e)
        {
            if (numberOfSamplesSelected >= 3)
            {
                playSample(2);
                this.sampleButton3.BackColor = getRandColor();
            }
        }

        private void sampleButton4_Click(object sender, EventArgs e)
        {
            if (numberOfSamplesSelected >= 4)
            {
                playSample(3);
                this.sampleButton4.BackColor = getRandColor();
            }
        }

        private void sampleButton5_Click(object sender, EventArgs e)
        {
            if (numberOfSamplesSelected >= 5)
            {
                playSample(4);
                this.sampleButton5.BackColor = getRandColor();
            }
        }

        private void sampleButton6_Click(object sender, EventArgs e)
        {
            if (numberOfSamplesSelected >= 6)
            {
                playSample(5);
                this.sampleButton6.BackColor = getRandColor();
            }
        }

        private void sampleButton7_Click(object sender, EventArgs e)
        {
            if (numberOfSamplesSelected>=7)
            {
                playSample(6);
                this.sampleButton7.BackColor = getRandColor();
            }
        }

        private void sampleButton8_Click(object sender, EventArgs e)
        {
            if (numberOfSamplesSelected>=8)
            {
                playSample(7);
                this.sampleButton8.BackColor = getRandColor();
            }
        }

        private void sampleButton9_Click(object sender, EventArgs e)
        {
            if (numberOfSamplesSelected==9)
            {
                playSample(8);
                this.sampleButton9.BackColor = getRandColor();
            }
        }

        private void Form1_KeyPress(object sender, KeyPressEventArgs e)
        {


            if (e.KeyChar == p1) playSample(0);
            if (e.KeyChar == p2) playSample(1);
            if (e.KeyChar == p3) playSample(2);
            if (e.KeyChar == p4) playSample(3);
            if (e.KeyChar == p5) playSample(4);
            if (e.KeyChar == p6) playSample(5);
            if (e.KeyChar == p7) playSample(6);
            if (e.KeyChar == p8) playSample(7);
            if (e.KeyChar == p9) playSample(8);
                  


        }

        private void Form1_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            
            

                   }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void toolStripTextBox1_KeyDown(object sender, KeyEventArgs e)
        {
            pad1 = e.KeyCode;
        }

        private void toolStripTextBox2_KeyDown(object sender, KeyEventArgs e)
        {
            pad2 = e.KeyCode;
        }

        private void toolStripTextBox3_KeyDown(object sender, KeyEventArgs e)
        {
            pad3 = e.KeyCode;
        }

        private void toolStripTextBox4_KeyDown(object sender, KeyEventArgs e)
        {
            pad4 = e.KeyCode;
        }

        private void toolStripTextBox5_KeyDown(object sender, KeyEventArgs e)
        {
            pad5 = e.KeyCode;
        }

        private void toolStripTextBox6_KeyDown(object sender, KeyEventArgs e)
        {
            pad6 = e.KeyCode;
        }

        private void toolStripTextBox7_KeyDown(object sender, KeyEventArgs e)
        {
            pad7 = e.KeyCode;
        }

        private void toolStripTextBox8_KeyDown(object sender, KeyEventArgs e)
        {
            pad8 = e.KeyCode;
        }

        private void toolStripTextBox9_KeyDown(object sender, KeyEventArgs e)
        {
            pad9 = e.KeyCode;
        }

        private void openSamplesToolStripMenuItem_Click(object sender, EventArgs e)
        {


            //set filter for wave files
            this.multiSampleOpen.Filter = "Wave File (*.wav|*.wav;";
            //allow multi selection by user
            this.multiSampleOpen.Multiselect = true;
            this.multiSampleOpen.Title = "Choose 9 Samples:";


            DialogResult dr = this.multiSampleOpen.ShowDialog();
            if (dr != System.Windows.Forms.DialogResult.OK) return;
            numberOfSamplesSelected = 0;
            foreach (String files in this.multiSampleOpen.FileNames)
            {
                numberOfSamplesSelected++;
            }

            openFileObjects = this.multiSampleOpen.FileNames;
        }

        

    }
}


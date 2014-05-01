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
        Pad pd1 = new Pad(Keys.Q);
        Pad pd2 = new Pad(Keys.W);
        Pad pd3 = new Pad(Keys.E);
        Pad pd4 = new Pad(Keys.A);
        Pad pd5 = new Pad(Keys.S);
        Pad pd6 = new Pad(Keys.D);
        Pad pd7 = new Pad(Keys.Z);
        Pad pd8 = new Pad(Keys.X);
        Pad pd9 = new Pad(Keys.C);

        



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


        Player player = new Player();


        public Form1()
        {
            InitializeComponent();
         
        }
        
        
        //to be used to open samples
        private OpenFileDialog multiSampleOpen = new OpenFileDialog();
        private OpenFileDialog snglSampleOpen = new OpenFileDialog();
  
        
        private void pauseButton_Click(object sender, EventArgs e)
        {

            player.pause();
        }

       

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            //stop playback and use Dispose() to dump
           
            //DisposeWavee(output,wave);
           /// DisposeWave(output2, wave2);
            
            
            //make sure mic is not recording
            button7_Click(sender, e);
        }



     //echo button is on or off
        private void button4_Click(object sender, EventArgs e)
        {
                      
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

        }

        private void selectInputToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count == 0) return;
            int chosenInputNumber = listView1.SelectedItems[0].Index;

            player.micOn(chosenInputNumber);        }

        private void button7_Click(object sender, EventArgs e)
        {
            player.stopRecording();
        }



        //Record Wave Button
        private void button8_Click(object sender, EventArgs e)
        {
            //make  sure input source is selected
            if (listView1.SelectedItems.Count == 0) return;

            //set a file to save wav file to
            SaveFileDialog micSaveWavDialog = new SaveFileDialog();
            micSaveWavDialog.Filter = "Wave File (*.wav|*.wav;";
            if (micSaveWavDialog.ShowDialog() != DialogResult.OK) return;

            
            player.recordWav(listView1, micSaveWavDialog);
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

            if (pd1.isLoaded)
            {
                player.playSample(pd1);
                this.sampleButton1.BackColor = getRandColor();

            }

        }

        private void sampleButton2_Click(object sender, EventArgs e)
        {
            if (player.loadedSamps[1])
            {
                player.playSample(pd2);
                //turnColor(sender);
                this.sampleButton2.BackColor = getRandColor();
            }

        }

        private void sampleButton3_Click(object sender, EventArgs e)
        {
            if (player.loadedSamps[2])
            {
                player.playSample(pd3);
                this.sampleButton3.BackColor = getRandColor();
            }
        }

        private void sampleButton4_Click(object sender, EventArgs e)
        {
            if (player.loadedSamps[3])
            {
                player.playSample(pd4);
                this.sampleButton4.BackColor = getRandColor();
            }
        }

        private void sampleButton5_Click(object sender, EventArgs e)
        {
            if (player.loadedSamps[4])
            {
                player.playSample(pd5);
                this.sampleButton5.BackColor = getRandColor();
            }
        }

        private void sampleButton6_Click(object sender, EventArgs e)
        {
            if (player.loadedSamps[5])
            {
                player.playSample(pd6);
                this.sampleButton6.BackColor = getRandColor();
            }
        }

        private void sampleButton7_Click(object sender, EventArgs e)
        {
            if (player.loadedSamps[6])
            {
                player.playSample(pd7);
                this.sampleButton7.BackColor = getRandColor();
            }
        }

        private void sampleButton8_Click(object sender, EventArgs e)
        {
            if (player.loadedSamps[7])
            {
                player.playSample(pd8);
                this.sampleButton8.BackColor = getRandColor();
            }
        }

        private void sampleButton9_Click(object sender, EventArgs e)
        {
            if (player.loadedSamps[8])
            {
                player.playSample(pd9);
                this.sampleButton9.BackColor = getRandColor();
            }
        }

        private void Form1_KeyPress(object sender, KeyPressEventArgs e)
        {
            Pad[] padz = new Pad[9] { pd1,pd2,pd3,pd4,pd5,pd6,pd7,pd8,pd9};

            foreach(var element in padz)
            {
                if(e.KeyChar == element.keyShortCut) 
                    player.playSample(element);
            }

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
           
            player.openFileObjects = this.multiSampleOpen.FileNames;
            
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void micSourceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            player.loadMicSources(listView1);
            
        }
       



        private void loadSampleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //TODO:   GET TO  WORK FOR ALL PADS
            int buttonContextMenu = 0;
            
            Pad[] padz = new Pad[9] { pd1, pd2, pd3, pd4, pd5, pd6, pd7, pd8, pd9 };


            player.loadSamp(padz[buttonContextMenu]);

           
        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
            
        }

        private void splitContainer1_Panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        

    }
}


using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Gatling
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            Work();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            HideGB(gbMedia);
        }

        private void HideGB(GroupBox box)
        {
            Size HideSize = new Size(0, 0);
            Size ShowSize = new Size(498, 385);
            gbMedia.Size = HideSize;
            gbProfile.Size = HideSize;
            gbView.Size = HideSize;

            box.Size = ShowSize;
           // gbProfile.Size = ShowSize;
        }

        private void Button8_Click(object sender, EventArgs e)
        {
            HideGB(gbProfile);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            HideGB(gbView);
        }

        //Baz kardan Folder bataye gereftan Media
        private void btnPostFolder_Click(object sender, EventArgs e)
        {
            using (var fbd = new FolderBrowserDialog())
            {
                //Set kardane Akharin mahale baz shode:
                fbd.SelectedPath = AppSettings.Default.LastAddress;
                DialogResult result = fbd.ShowDialog();

                if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                {
                    btnPostFolder.Enabled = false;
                    tbMediaSource.Enabled = false;

                    tbMediaSource.Text = fbd.SelectedPath;
                    rbLog.Text = rbLog.Text + "Source: " + fbd.SelectedPath + "\n";

                    //Save Akharin mahale baz shode:
                    AppSettings.Default.LastAddress = fbd.SelectedPath;
                    AppSettings.Default.Save();

                    //var Ops = MediaManager.Render(fbd.SelectedPath);
                    //WriteLogForMedia(Ops);
                }
            }
        }

        private void WriteLogForMedia(List<MediaBox> ops)
        {
            int Video = 0, Picture = 0, VG = 0, PG = 0, SVideo = 0, SPicture = 0, SVG = 0, SPG = 0;

            foreach (var item in ops)
            {
                if(item.kind == ItemKind.Picture)
                {
                    Picture++;
                }
                else if (item.kind == ItemKind.Video)
                {
                    Video++;
                }
                else if (item.kind == ItemKind.SPicture)
                {
                    SPicture++;
                }
                else if (item.kind == ItemKind.SVideo)
                {
                    SVideo++;
                }
                else if (item.kind == ItemKind.SVideoGallery)
                {
                    SVG++;
                }
                else if (item.kind == ItemKind.SPictureGallery)
                {
                    SPG++;
                }
                else if (item.kind == ItemKind.VideoGallery)
                {
                    VG++;
                }
                else if (item.kind == ItemKind.PictureGallery)
                {
                    PG++;
                }
            }

            //Simple Page Log:
            if(Video>0)
            rbLog.Invoke((MethodInvoker)delegate { rbLog.Text = rbLog.Text + "Video:" + Video + "\n"; });
            if(Picture>0)
            rbLog.Invoke((MethodInvoker)delegate { rbLog.Text = rbLog.Text + "Picture:" + Picture + "\n"; });
            if(VG>0)
            rbLog.Invoke((MethodInvoker)delegate { rbLog.Text = rbLog.Text + "Video Gallery:" + VG + "\n"; });
            if(PG>0)
            rbLog.Invoke((MethodInvoker)delegate { rbLog.Text = rbLog.Text + "Picture Gallery:" + PG + "\n"; });

            //Spicial Post:
            if(SVideo>0)
            rbLog.Invoke((MethodInvoker)delegate { rbLog.Text = rbLog.Text + "Special Video:" + SVideo + "\n"; });
            if(SPicture>0)
            rbLog.Invoke((MethodInvoker)delegate { rbLog.Text = rbLog.Text + "Special Picture:" + SPicture + "\n"; });
            if(SVG>0)
            rbLog.Invoke((MethodInvoker)delegate { rbLog.Text = rbLog.Text + "Special Video Gallery:" + SVG + "\n"; });
            if(SPG>0)
            rbLog.Invoke((MethodInvoker)delegate { rbLog.Text = rbLog.Text + "Special Picture Gallery:" + SPG + "\n"; });

            if(Video==0 && Picture ==0&& VG == 0 && PG == 0 && SVG == 0 && SPG == 0 && SPicture == 0 && SVideo == 0)
            {
                rbLog.Invoke((MethodInvoker)delegate { rbLog.Text = rbLog.Text + "There are no media here.\n"; });
            }

            btnPostFolder.Enabled = true;
            tbMediaSource.Enabled = true;
        }

        private void tbMediaSource_TextChanged(object sender, EventArgs e)
        {
            if (Directory.Exists(tbMediaSource.Text))
            {
                var Ops = MediaManager.Render(tbMediaSource.Text);
                WriteLogForMedia(Ops);
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            string path = @"C:\Users\Ya-Allah\Downloads\Telegram Desktop\2.mp4";
            VideoManager video = new VideoManager();
            video.TakeScreen(path);
        }

        private void Work()
        {
           
        }

        private void SundayWorking()
        {
            if(AppSettings.Default.sun != "N")
            {

            }
        }

        private void MyTime_Tick(object sender, EventArgs e)
        {
            Work();
            IALow();
        }

        private void IALow()
        {
            switch (DateTime.Now.DayOfWeek.ToString())
            {
                case "Sunday":
                    {
                        SundayWorking();
                    }
                    break;
            }
        }

        private void cbSun_CheckedChanged(object sender, EventArgs e)
        {
            tbHFSun.Enabled = cbSun.Checked;
            tbHESun.Enabled = cbSun.Checked;
            cbPostSun.Enabled = cbSun.Checked;
            cbStorySun.Enabled = cbSun.Checked;
            cbFollowingSun.Enabled = cbSun.Checked;
            cbHomeSun.Enabled = cbSun.Checked;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            if(AppSettings.Default.sun == "N")
            {
                tbHFSun.Enabled = false;
                tbHESun.Enabled = false;
                cbPostSun.Enabled = false;
                cbStorySun.Enabled = false;
                cbFollowingSun.Enabled = false;
                cbHomeSun.Enabled = false;
            }
            else
            {
                List<string> Order = AppSettings.Default.sun.Split(',').ToList();

                tbHFSun.Text = Order[0];
                tbHESun.Text = Order[1];
                if (Order[2] == "1")
                {
                    cbPostSun.Checked = true;
                }
                if (Order[3] == "1")
                {
                    cbStorySun.Checked = true;
                }
                if (Order[4] == "1")
                {
                    cbFollowingSun.Checked = true;
                }
                if (Order[5] == "1")
                {
                    cbHomeSun.Checked = true;
                }
            }
        }

        private void btnProfileApply_Click(object sender, EventArgs e)
        {
            if (cbSun.Checked)
            {
                if(tbHESun.Text.Length>0 && tbHFSun.Text.Length > 0)
                {
                    string Order = tbHFSun.Text + "," + tbHESun.Text;
                    if (cbPostSun.Checked)
                    {
                        Order += "," + 1;
                    }
                    else
                    {
                        Order += "," + 0;
                    }
                    if (cbStorySun.Checked)
                    {
                        Order += "," + 1;
                    }
                    else
                    {
                        Order += "," + 0;
                    }
                    if (cbFollowingSun.Checked)
                    {
                        Order += "," + 1;
                    }
                    else
                    {
                        Order += "," + 0;
                    }
                    if (cbHomeSun.Checked)
                    {
                        Order += "," + 1;
                    }
                    else
                    {
                        Order += "," + 0;
                    }

                    AppSettings.Default.sun = Order;
                    AppSettings.Default.Save();

                }
                else
                {
                    AppSettings.Default.sun = "N";
                    AppSettings.Default.Save();
                }
            }
            else
            {
                AppSettings.Default.sun = "N";
                AppSettings.Default.Save();
            }


        }
    }
}

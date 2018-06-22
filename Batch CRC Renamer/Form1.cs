using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using DamienG.Security.Cryptography;
// Created by Muhammad Thomas Fadhila Yahya
// mthomasfadhilayahya@gmail.com


namespace Batch_CRC_Renamer
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            
        }

        //inialisasi var
        int jumlah = 0;
        Crc32 crc32 = new Crc32();
        String hash = String.Empty;

        private void cekCRC(ListView LV1, int jumlah, ToolStripStatusLabel sl1)
        {
            //fungsi untuk memeriksa CRC file

            hash = String.Empty;

            for (int j = 0; j < jumlah; j++)
            {
                using (FileStream fs = File.Open(sl1 + LV1.Items[j].SubItems[0].Text, FileMode.Open))
                    foreach (byte b in crc32.ComputeHash(fs)) hash += b.ToString("x2").ToUpper();
                LV1.Items[j].SubItems[1].Text = hash;
                LV1.Refresh();
                hash = String.Empty;
            }
        }

        private String cekeks(ListView LV1, int j, int titik)
        {
            //fungsi untuk menghasilkan string ekstensi

            if (titik > 0)
            {
                int panjang = LV1.Items[j].SubItems[0].Text.Length;
                return LV1.Items[j].SubItems[0].Text.Remove(0, titik);
            }
            else
            {
                return "";
            }
        }

        private String namatpeks(ListView LV1, int titik, int j, ToolStripStatusLabel sl1)
        {
            //fungsi untuk menghasilkan string nama file tanpa ekstensi

            if (titik > 0)
            {
                return sl1 + LV1.Items[j].SubItems[0].Text.Remove(titik);
            }
            else
            {
                return sl1 + LV1.Items[j].SubItems[0].Text;
            }
        }

        private void cekRename(ListView LV1, int jumlah, ToolStripStatusLabel sl1)
        {
            //fungsi untuk memeriksa CRC dan melakukan Rename

            if (LV1.Items[0].SubItems[1].Text != "")
            {
                for (int j = 0; j < jumlah; j++)
                {
                    int titik = LV1.Items[j].SubItems[0].Text.LastIndexOf('.');
                    string ext = cekeks(LV1, j, titik);
                    string nama = sl1 + LV1.Items[j].SubItems[0].Text;
                    string namaeks = namatpeks(LV1, titik, j, sl1);
                    File.Move(nama, namaeks + " [" + LV1.Items[j].SubItems[1].Text + "]" + ext);
                }
            }
            else
            {
                cekCRC(LV1, jumlah, sl1);
                cekRename(LV1, jumlah, sl1);
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //keluar
            Application.ExitThread();
        }

        public void AddRow(ListView lvw, string[] items)
        {
            //item utama
            ListViewItem new_item = lvw.Items.Add(items[0]);

            //sub-items
            for (int i = 1; i < items.Length; i++)
                new_item.SubItems.Add(items[i]);
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                //clear listview
                LV1.Items.Clear();

                //mengambil path
                string fullPath = openFileDialog1.FileName;
                string fileName = openFileDialog1.SafeFileName;
                string path = fullPath.Remove(fullPath.Length - fileName.Length);

                sl1.Text = path;
                int i = 1;
                foreach(string s in openFileDialog1.SafeFileNames){
                    AddRow(LV1,new string[] {s,""});
                    i++;
                }

                //menampilkan jumlah file
                sl2.Text = "   [ " + (i - 1).ToString() + " files ]";
                jumlah = i - 1;
            }
        }

        private void checkToolStripMenuItem_Click(object sender, EventArgs e)
        {
            cekCRC(LV1, jumlah, sl1);
        }

        private void checkRenameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            cekRename(LV1, jumlah, sl1);
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //profil
            MessageBox.Show("Created by\n\nMuhammad Thomas Fadhila Yahya\nJuly 1st, 2017", "About", MessageBoxButtons.OK,
        MessageBoxIcon.Information);
        }

    }
}

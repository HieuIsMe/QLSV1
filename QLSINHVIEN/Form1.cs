using QLSINHVIEN.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QLSINHVIEN
{
    public partial class Form1 : Form
    {
        public Form1()
        {

            InitializeComponent();
            listView1.View = View.Details;
            listView1.FullRowSelect = true;

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            LoadData(); 
        }
        void LoadData()
        {
            using (QLSVEntities2 qlEntity = new QLSVEntities2())
            {
                var kq = from xyz in qlEntity.QLSVs select xyz;
                listView1.Items.Clear();
                listView1.Groups.Clear();

                ListViewGroup lv_van = new ListViewGroup("Khoa Văn");
                listView1.Groups.Add(lv_van);
                ListViewGroup lv_vatly = new ListViewGroup("Khoa Vật lý");
                listView1.Groups.Add(lv_vatly);
                ListViewGroup lv_cntt = new ListViewGroup("Khoa CNTT");
                listView1.Groups.Add(lv_cntt);

                foreach (var data in kq)
                {
                    DateTime bd = (DateTime)data.NgaySinh;
                    ListViewItem lv_item = new ListViewItem(data.MaSV);
                    lv_item.SubItems.Add(data.HoTen);
                    lv_item.SubItems.Add(data.Khoa);
                    lv_item.SubItems.Add(data.GioiTinh.ToString());
                    lv_item.SubItems.Add(bd.ToString("dd'/'MM'/'yyyy"));
                    lv_item.SubItems.Add(data.Diem1.ToString());
                    lv_item.SubItems.Add(data.Diem2.ToString());
                    lv_item.SubItems.Add(data.Diem3.ToString());
                    lv_item.SubItems.Add(data.Diem4.ToString());

                    float dtb = -1;
                    if (String.Compare(data.Khoa, "Van", true) == 0)
                        dtb = (float)(data.Diem1 + data.Diem2) / 2;
                    if (String.Compare(data.Khoa, "Vatly", true) == 0)
                        dtb = (float)(data.Diem1 + data.Diem2 + data.Diem3 + data.Diem4) / 4;
                    if (String.Compare(data.Khoa, "CNTT", true) == 0)
                        dtb = (float)(data.Diem1 + data.Diem2 + data.Diem3) / 3;

                    lv_item.SubItems.Add(dtb.ToString());                   
                    listView1.Items.Add(lv_item);
                    if (String.Compare(data.Khoa, "Van", true) == 0) lv_item.Group = lv_van;
                    if (String.Compare(data.Khoa, "VatLy", true) == 0) lv_item.Group = lv_vatly;
                    if (String.Compare(data.Khoa, "CNTT", true) == 0) lv_item.Group = lv_cntt;

                }
            }
        }
        private void lv_DanhSach_SelectedIndexChanged(object sender, EventArgs e)
        {
            foreach (ListViewItem list in listView1.SelectedItems)
            {
                txtName.Text = list.SubItems[1].Text;
                if (list.SubItems[3].Text == "True") ckbGioitinh.Checked = true;
                else ckbGioitinh.Checked = false;
                dtimeNgaySinh.Text = list.SubItems[4].Text;

                if (list.SubItems[2].Text == "Van")
                {
                    textVHCD.Text = list.SubItems[5].Text;
                    textVHHD.Text = list.SubItems[6].Text;
                    tcDiem.SelectedTab = tabVan;
                }

                if (list.SubItems[2].Text == "VatLy")
                {
                    textCOHOC.Text = list.SubItems[5].Text;
                    textquanghoc.Text = list.SubItems[6].Text;
                    textdien.Text = list.SubItems[7].Text;
                    textvlHatnhan.Text = list.SubItems[8].Text;
                    tcDiem.SelectedTab = tabVatLy;
                }

                if (list.SubItems[2].Text == "CNTT")
                {
                    textpascal.Text = list.SubItems[5].Text;
                    textcshap.Text = list.SubItems[6].Text;
                    textsql.Text = list.SubItems[7].Text;
                    tcDiem.SelectedTab = tabCNTT;
                }

                textDTB.Text = list.SubItems[9].Text;
            }
        }

        void LamMoi()
        {
            txtName.Text = "";
            ckbGioitinh.Text = "";
            dtimeNgaySinh.Checked = false;
            textDTB.Text = "";
            textVHCD.Text = "";
            textVHHD.Text = "";
            textCOHOC.Text = "";
            textquanghoc.Text = "";
            textdien.Text = "";
            textvlHatnhan.Text = "";
            textpascal.Text = "";
            textcshap.Text = "";
            textsql.Text = "";
        }
        String key = "";
        QLSVEntities2 qlE = new QLSVEntities2();
        private String tab = "";
        private void toolStripLabel4_Click(object sender, EventArgs e)
        {
                InsertSV();
                LoadData();
        }
        private void toolStripLabel5_Click(object sender, EventArgs e)
        {

                delete();
                LoadData();
        }
        public void InsertSV()
        {

            QLSV sv = new QLSV();
            sv.MaSV = MaSV.getMa();
            sv.HoTen = txtName.Text;
            if (ckbGioitinh.Checked == true) sv.GioiTinh = true;
            else sv.GioiTinh = false;
            DateTime bd = Convert.ToDateTime(dtimeNgaySinh.Text);
            sv.NgaySinh = bd;
            sv.Khoa = tab;
            switch (tab)
            {
                case "Van":
                    {
                        sv.Diem1 = float.Parse(textVHCD.Text);
                        sv.Diem2 = float.Parse(textVHHD.Text);
                        break;
                    }
                case "VatLy":
                    {
                        sv.Diem1 = float.Parse(textCOHOC.Text);
                        sv.Diem2 = float.Parse(textquanghoc.Text);
                        sv.Diem3 = float.Parse(textdien.Text);
                        sv.Diem4 = float.Parse(textvlHatnhan.Text);
                        break;
                    }
                default:
                    {
                        sv.Diem1 = float.Parse(textpascal.Text);
                        sv.Diem2 = float.Parse(textcshap.Text);
                        sv.Diem3 = float.Parse(textsql.Text);
                        break;
                    }
            }
            qlE.QLSVs.Add(sv);
            qlE.SaveChanges();
        }

        public void delete()
        {
            foreach (ListViewItem list in listView1.SelectedItems)
            {
                key = list.SubItems[0].Text;
                break;
            }

            DialogResult dr = MessageBox.Show("Are you sure?", "Confirm", MessageBoxButtons.YesNo);
            if (dr == System.Windows.Forms.DialogResult.Yes)
            {
                try
                {
                    QLSV sv = qlE.QLSVs.Single(c => c.MaSV.Equals(key));
                    qlE.QLSVs.Remove(sv);
                    qlE.SaveChanges();
                    MessageBox.Show("Delete Successful");

                }
                catch
                {
                    MessageBox.Show("Delete failed");
                }
            }
        }

        private void toolStripLabel3_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem list in listView1.SelectedItems)
            {
                key = list.SubItems[0].Text;
                break;
            }
                if (key == "") return;
                QLSV sv = qlE.QLSVs.Where(p => p.MaSV == key).SingleOrDefault();
                sv.HoTen = txtName.Text;
                DateTime bd = Convert.ToDateTime(dtimeNgaySinh.Text);
                sv.NgaySinh = bd;
                switch (sv.Khoa)
                {
                    case "Van":
                        {
                            sv.Diem1 = float.Parse(textVHCD.Text);
                            sv.Diem2 = float.Parse(textVHHD.Text);
                            break;
                        }
                    case "VatLy":
                        {
                            sv.Diem1 = float.Parse(textCOHOC.Text);
                            sv.Diem2 = float.Parse(textquanghoc.Text);
                            sv.Diem3 = float.Parse(textdien.Text);
                            sv.Diem4 = float.Parse(textvlHatnhan.Text);
                            break;
                        }
                    default:
                        {
                            sv.Diem1 = float.Parse(textpascal.Text);
                            sv.Diem2 = float.Parse(textcshap.Text);
                            sv.Diem3 = float.Parse(textsql.Text);
                            break;
                        }
                }
                qlE.SaveChanges();
                LoadData();
            
        }

        private void sVVănToolStripMenuItem_Click(object sender, EventArgs e)
        {
            tcDiem.SelectedTab = tabVan;
            LamMoi();
            tab = "Van";
        }


        private void sVVậtLýToolStripMenuItem_Click(object sender, EventArgs e)
        {
            tcDiem.SelectedTab = tabVatLy;
            LamMoi();
            tab = "VatLy";
        }

        private void sVCNTTToolStripMenuItem_Click(object sender, EventArgs e)
        {
            tcDiem.SelectedTab = tabCNTT;
            LamMoi();
            tab = "CNTT";
        }

        private void toolStripSplitButton1_ButtonClick(object sender, EventArgs e)
        {

        }
    } 
}
﻿using ExcelDataReader;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.UI;                                                                                                                                                                                                                                                       

namespace Prolab3_0
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        //Ana Değişkenler
        DataTableCollection dtc;
        List<kisi> kisiler = new List<kisi>();
        List<kisi> ata_meslegi_yapanlar = new List<kisi>();
        List<kisi> ata_meslegi_yapanlar_atalar = new List<kisi>();
        List<kisi> cocugu_olmayanlar = new List<kisi>();
        List<kisi> ismi_ayni_olanlar = new List<kisi>();
        List<int> ismi_ayni_olanlar_yas = new List<int>();
        List<kisi> uveykardesler = new List<kisi>();
        string akrabalik;
        int say = 1;

        private void Form1_Load(object sender, EventArgs e)
        {

        }


        private void button1_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openfile = new OpenFileDialog() { Title = "EXCEL DOSYALARI" })
            {
                if (openfile.ShowDialog() == DialogResult.OK)
                {
                    textBox1.Text = openfile.FileName;

                    using (var stream = File.Open(openfile.FileName, FileMode.Open, FileAccess.Read))
                    {
                        using (IExcelDataReader excelreader = ExcelReaderFactory.CreateReader(stream))
                        {
                            DataSet reseult = excelreader.AsDataSet(new ExcelDataSetConfiguration()
                            {
                                ConfigureDataTable = (x) => new ExcelDataTableConfiguration() { UseHeaderRow = true }
                            }
                            );

                            dtc = reseult.Tables;
                            comboBox1.Items.Clear();
                            foreach (DataTable table in dtc) comboBox1.Items.Add(table.TableName);
                        }
                    }
                }
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataTable dt = dtc[comboBox1.SelectedIndex];
            dataGridView1.DataSource = dt;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if(kisiler.Count==0)
            {
                listBox1.Items.Add("Liste boş");
            }

            else
            {
                int i = 0;
                string isa = "-)";
                foreach (kisi k in kisiler) { listBox1.Items.Add((i+1)+isa+k.ad+" "+k.soyad);i++; }

            }
            
        }

        private void button4_Click(object sender, EventArgs e)
        {
            say = 1;
            kisiler.Clear();
            ata_meslegi_yapanlar.Clear();
            cocugu_olmayanlar.Clear();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            kisi k1 = new kisi();

            k1.ebeveyn_bagla(kisiler);
            k1.es_bagla(kisiler);
            k1.cocuk_bagla(kisiler);
            k1.ata_meslek(kisiler, ata_meslegi_yapanlar,ata_meslegi_yapanlar_atalar);
            k1.cocugum_yok(kisiler, cocugu_olmayanlar);
            k1.ayni_isimliler(kisiler, ismi_ayni_olanlar,ismi_ayni_olanlar_yas);
            k1.cocugum_yok_sirala(cocugu_olmayanlar);
            k1.uvey_kardes_bul(kisiler, uveykardesler);
            
        }


        private void button2_Click(object sender, EventArgs e)
        {

            int satir_sayisi = dataGridView1.Rows.Count;

            say = 1;
            //Datagridview'den bilgiler oluşturulan nesnelere atılıyor
            if (satir_sayisi > 0)
            {
                while (say <= satir_sayisi)
                {
                    kisiler.Add(new kisi(Convert.ToInt32(dataGridView1.Rows[say - 1].Cells["id"].Value), dataGridView1.Rows[say - 1].Cells["İsim"].Value.ToString(), dataGridView1.Rows[say - 1].Cells["Soyisim"].Value.ToString(), dataGridView1.Rows[say - 1].Cells["Doğum Tarihi"].Value.ToString(), dataGridView1.Rows[say - 1].Cells["Eşi"].Value.ToString(), dataGridView1.Rows[say - 1].Cells["Anne Adı"].Value.ToString(), dataGridView1.Rows[say - 1].Cells["Baba Adı"].Value.ToString(), dataGridView1.Rows[say - 1].Cells["Kan Grubu"].Value.ToString(), dataGridView1.Rows[say - 1].Cells["Meslek"].Value.ToString(), dataGridView1.Rows[say - 1].Cells["Medeni Hali"].Value.ToString(), dataGridView1.Rows[say - 1].Cells["Kızlık Soyismi"].Value.ToString(), dataGridView1.Rows[say - 1].Cells["Cinsiyet"].Value.ToString()));
                    say++;
                }
            }


            //Listede birden fazla bulunan aynı kişiler çıkartılıyor
            for (int i = 0; i < kisiler.Count; i++)
            {
                for (int k = i + 1; k < kisiler.Count; k++)
                {
                    if (kisiler[i].TC == kisiler[k].TC)
                    {
                        kisiler.RemoveAt(k);
                    }
                }
            }

        }

        private void button6_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            listBox2.Items.Clear();
            listBox3.Items.Clear();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            string isa = "-)";
            for(int i=0;i<kisiler.Count;i++)
            {
                if(kisiler[i].father!=null)
                {
                    listBox2.Items.Add((i+1)+isa+kisiler[i].father.ad+" "+ kisiler[i].father.soyad);
                }

                else
                {
                    listBox2.Items.Add((i+1)+" ");
                }

            }


            for (int i = 0; i < kisiler.Count; i++)
            {
                if (kisiler[i].mother != null)
                {
                    listBox3.Items.Add(kisiler[i].mother.ad + " " + kisiler[i].mother.soyad);
                }

                else
                {
                    listBox3.Items.Add(" ");
                }

            }

        }

        private void button8_Click(object sender, EventArgs e)
        {
            string isa = "-)";
            for(int i=0;i<kisiler.Count;i++)
            {
                if(kisiler[i].es!=null)
                {
                    listBox2.Items.Add((i+1)+isa+kisiler[i].es.ad+" "+ kisiler[i].es.soyad);
                }

                else
                {
                    listBox2.Items.Add((i+1)+isa);
                }
            }
        }


        private void button9_Click(object sender, EventArgs e)
        {

            Graphics grafik = this.CreateGraphics();
            grafik.DrawRectangle(Pens.Blue, 0, 600, 100, 20);
            grafik.DrawLine(Pens.Red, 100, 600, 150, 700);
        }

        private void button10_Click(object sender, EventArgs e)//Kan gruplarını yazdırma
        {
            foreach(kisi k in kisiler)
            {
                string isim = k.kan_grubu.Substring(0, k.kan_grubu.IndexOf("("));
                string isaret = k.kan_grubu.Substring(k.kan_grubu.IndexOf("("));

                if (checkBox1.Checked && checkBox2.Checked)
                {
                    if(checkBox4.Checked)
                    {
                        if(isim.Equals("AB") && isaret.Equals("(+)"))
                        {
                            listBox2.Items.Add(k.kan_grubu);
                            listBox1.Items.Add(k.ad+" "+k.soyad);
                        }
                    }

                    if(checkBox3.Checked)
                    {
                        if (isim.Equals("AB") && isaret.Equals("(-)"))
                        {
                            listBox2.Items.Add(k.kan_grubu);
                            listBox1.Items.Add(k.ad + " " + k.soyad);
                        }
                    }
                }

                else if (checkBox1.Checked && checkBox2.Enabled)
                {
                    if (checkBox4.Checked)
                    {
                        if (isim.Equals("A") && isaret.Equals("(+)"))
                        {
                            listBox2.Items.Add(k.kan_grubu);
                            listBox1.Items.Add(k.ad + " " + k.soyad);
                        }
                    }

                    if(checkBox3.Checked)
                    {
                        if (isim.Equals("A") && isaret.Equals("(-)"))
                        {
                            listBox2.Items.Add(k.kan_grubu);
                            listBox1.Items.Add(k.ad + " " + k.soyad);
                        }
                    }
                }

                else if (checkBox1.Enabled && checkBox2.Checked)
                {
                    if (checkBox4.Checked)
                    {
                        if (isim.Equals("B") && isaret.Equals("(+)"))
                        {
                            listBox2.Items.Add(k.kan_grubu);
                            listBox1.Items.Add(k.ad + " " + k.soyad);
                        }
                    }

                    if(checkBox3.Checked)
                    {
                        if (isim.Equals("B") && isaret.Equals("(-)"))
                        {
                            listBox2.Items.Add(k.kan_grubu);
                            listBox1.Items.Add(k.ad + " " + k.soyad);
                        }
                    }
                }

                else if (checkBox1.Enabled && checkBox2.Enabled)
                {
                    if (checkBox4.Checked)
                    {
                        if (isim.Equals("0") && isaret.Equals("(+)"))
                        {
                            listBox2.Items.Add(k.kan_grubu);
                            listBox1.Items.Add(k.ad + " " + k.soyad);
                        }
                    }

                    if(checkBox3.Checked)
                    {
                        if (isim.Equals("0") && isaret.Equals("(-)"))
                        {
                            listBox2.Items.Add(k.kan_grubu);
                            listBox1.Items.Add(k.ad + " " + k.soyad);
                        }
                    }
                }

            }
        }

        private void button11_Click(object sender, EventArgs e)
        {
            foreach(kisi k in kisiler)
            {
                string isa = "-)";
                if(k.chil.Count>0)
                {
                    for(int i=0;i<k.chil.Count;i++)
                    {
                        listBox1.Items.Add((i+1)+isa+k.ad + " " + k.soyad);
                        listBox2.Items.Add((i + 1) + isa + k.chil[i].ad+ " " + k.chil[i].soyad);
                    }
                    listBox1.Items.Add(" ");
                    listBox2.Items.Add(" ");
                }
            }
        }

        private void button12_Click(object sender, EventArgs e)
        {
            string isa = "-)";
            int i = 0;
            foreach(kisi k in ata_meslegi_yapanlar) {   listBox3.Items.Add((i+1)+isa+k.ad + " " + k.soyad);i++;  }
            foreach(kisi k in ata_meslegi_yapanlar_atalar) { listBox2.Items.Add((i + 1) + isa + k.ad + " " + k.soyad);i++; }
        }

        private void button13_Click(object sender, EventArgs e)
        {
            int i = 0;
            string isa = "-)";
            foreach(kisi k in cocugu_olmayanlar) { listBox2.Items.Add((i+1)+isa+k.ad + " " + k.soyad);i++; }
        }

        private void button14_Click(object sender, EventArgs e)
        {
            string isa = "==>";
            string isa2 = "-)";
            kisi k = new kisi();
            for(int i=0;i<ismi_ayni_olanlar.Count;i++)
            {
                listBox2.Items.Add((i+1)+isa2+ismi_ayni_olanlar[i].ad + " " + ismi_ayni_olanlar[i].soyad + " " + isa + ismi_ayni_olanlar_yas[i]);
            }
        }

        private void button16_Click(object sender, EventArgs e)
        {
            string[] isim_soyad = textBox2.Text.Split(' ');

            for(int i=0;i<kisiler.Count;i++)
            {
                if (kisiler[i].ad == isim_soyad[0] && kisiler[i].soyad== isim_soyad[1])
                {
                    label5.Text=Convert.ToString(kisiler[i].kac_nesil());
                    break;
                }
            }

        }

        private void button17_Click(object sender, EventArgs e)
        {
            string[] isim_soyad = textBox2.Text.Split(' ');
            string[] isim_soyad2 = textBox3.Text.Split(' ');
            kisi k1 = new kisi();

            for(int i=0;i<kisiler.Count;i++)
            {
                if(kisiler[i].ad==isim_soyad[0] && kisiler[i].soyad==isim_soyad[1])
                {
                    for (int j = 0; j < kisiler.Count; j++)
                    {
                        if(kisiler[j].ad == isim_soyad2[0] && kisiler[j].soyad == isim_soyad2[1])
                        {
                            k1.sen_kimsin_lan(kisiler[i], kisiler[j], akrabalik);
                        }
                    }
                }
            }

            label5.Text = akrabalik;

        }

        private void button15_Click(object sender, EventArgs e)
        {
            foreach(kisi k in uveykardesler) { listBox1.Items.Add(k.ad + " " + k.soyad); }
        }
    }


    class kisi
    {
        public int TC;
        public string ad;
        public string soyad;
        public string tarih;
        public string kan_grubu;
        public string meslek;
        public string medeni_hal;
        public string kizlik_soyadi;
        public string cinsiyet;
        public string ana_adi;
        public string baba_adi;
        public string esi;

        public kisi()
        {

        }

        public kisi(int tc)
        {
            this.TC = tc;
        }

        public kisi(int tc, string ad, string soyad, string tarih, string esi, string ana_adi, string baba_adi, string kan_grubu, string meslek, string medeni_hali, string kizlik_soyadi, string cinsiyet)
        {
            this.TC = tc;
            this.ad = ad;
            this.soyad = soyad;
            this.tarih = tarih;
            this.esi = esi;
            this.kan_grubu = kan_grubu;
            this.meslek = meslek;
            this.medeni_hal = medeni_hali;
            this.kizlik_soyadi = kizlik_soyadi;
            this.cinsiyet = cinsiyet;
            this.ana_adi = ana_adi;
            this.baba_adi = baba_adi;
        }

        public kisi father=null;
        public kisi mother=null;
        public kisi es=null;
        public List<kisi> chil= new List<kisi>();


        public void ebeveyn_bagla(List<kisi> liste)//Ebeveynleri bağlayan fonksiyon
        {
            int j;
            for (int i = 2; i < liste.Count; i++)
            {
                j = 0;
                while (j < liste.Count)
                {
                    if (liste[i].mother==null && liste[i].ana_adi == liste[j].ad)
                    {
                        liste[i].mother = liste[j];
                    }

                    if (liste[i].father == null && liste[i].baba_adi == liste[j].ad)
                    {
                        liste[i].father = liste[j];
                    }

                    j++;
                }

            }

        }

        public void es_bagla(List<kisi> liste1)
        {
            int j;
            for (int i = 0; i < liste1.Count; i++)
            {
                j = 0;
                while (j < liste1.Count)
                {
                    if(liste1[i].es==null && liste1[i].esi!=null && liste1[i].esi.Contains(liste1[j].ad) && liste1[i].esi.Contains(liste1[j].soyad))
                    {
                        liste1[i].es = liste1[j]; 
                    }

                    j++;
                }

            }
        }

        public void cocuk_bagla(List<kisi> liste2)
        {
            int j;
            string tarih1, tarih2;
            for (int i = 0; i < liste2.Count; i++)
            {
                j = 0;
                while (j < liste2.Count)
                {
                    if (liste2[i].ad == liste2[j].ana_adi || liste2[i].ad == liste2[j].baba_adi)
                    {
                        int sira = 0;
                        tarih1 = null;
                        tarih2 = null;
                        for (int k=0;k<=liste2[i].tarih.Length-1;k++)
                        {
                            if(liste2[i].tarih[k]=='.')
                            {
                                sira++;
                            }

                            if(sira==2)
                            {
                                tarih1 = liste2[i].tarih.Substring(k+1, 4);
                                sira = 0;
                                break;
                            }

                        }

                        
                        for (int k = 0; k <= liste2[j].tarih.Length - 1; k++)
                        {
                            if (liste2[j].tarih[k] == '.')
                            {
                                sira++;
                            }

                            if (sira == 2)
                            {
                                tarih2 = liste2[j].tarih.Substring(k+1, 4);
                                sira = 0;
                                break;
                            }

                        }


                        if(Convert.ToInt32(tarih1.Trim()) < Convert.ToInt32(tarih2))
                        {
                            liste2[i].chil.Add(liste2[j]);
                        }

                    }

                    j++;
                }

            }

        }

        public void ata_meslek(List<kisi> liste3,List<kisi> liste4,List<kisi> liste5)
        {
            int j;
            int k;

            for (int i = 0; i < liste3.Count; i++)
            {
                j = 0;
                while (j < liste3.Count)
                {
                    if(liste3[i].chil != null && liste3[i].meslek==liste3[j].meslek)
                    {
                        for(int y=0;y<liste3[i].chil.Count;y++)
                        {
                            if(liste3[i].chil[y] == liste3[j])
                            {
                                liste4.Add(liste3[j]);
                                liste5.Add(liste3[i]);
                            }
                        }
                    }

                    j++;
                }

            }
        }


        public void cocugum_yok(List<kisi> liste1,List<kisi> liste2)
        {

            for (int i = 0; i < liste1.Count; i++)
            {
                if(liste1[i].chil.Count==0 && liste1[i].medeni_hal=="Bekar")
                {
                    liste2.Add(liste1[i]);
                }

            }

        }

        public void cocugum_yok_sirala(List<kisi> liste2)
        {
            string tarih1;
            string tarih2;
            kisi temp = new kisi();
            for(int i=0;i<liste2.Count;i++)
            {
                for(int j=0;j<liste2.Count;j++)
                {
                    int sira = 0;
                    tarih1 = null;
                    tarih2 = null;
                    for (int k = 0; k <= liste2[i].tarih.Length - 1; k++)
                    {
                        if (liste2[i].tarih[k] == '.')
                        {
                            sira++;
                        }

                        if (sira == 2)
                        {
                            tarih1 = liste2[i].tarih.Substring(k + 1, 4);
                            sira = 0;
                            break;
                        }

                    }


                    for (int k = 0; k <= liste2[j].tarih.Length - 1; k++)
                    {
                        if (liste2[j].tarih[k] == '.')
                        {
                            sira++;
                        }

                        if (sira == 2)
                        {
                            tarih2 = liste2[j].tarih.Substring(k + 1, 4);
                            sira = 0;
                            break;
                        }

                    }

                    if(Convert.ToInt32(tarih1)<Convert.ToInt32(tarih2))
                    {
                        temp = liste2[i];
                        liste2[i] = liste2[j];
                        liste2[j] = temp;
                    }

                }
            }
        }


        public void ayni_isimliler(List<kisi> liste1, List<kisi> liste2,List<int> liste3)
        {
            int j;
            string tarih1;
            for (int i = 0; i < liste1.Count; i++)
            {
                j = 0;
                while (j < liste1.Count)
                {
                    if(liste1[i].ad==liste1[j].ad && liste1[i]!=liste1[j])
                    {
                        int sira = 0;
                        tarih1 = null;
                        for (int k = 0; k <= liste1[i].tarih.Length - 1; k++)
                        {
                            if (liste1[i].tarih[k] == '.')
                            {
                                sira++;
                            }

                            if (sira == 2)
                            {
                                tarih1 = liste1[i].tarih.Substring(k + 1, 4);
                                sira = 0;
                                break;
                            }

                        }

                        int a = 2022 - Convert.ToInt32(tarih1);

                        liste2.Add(liste1[i]);
                        liste3.Add(a);
                    }
                    j++;
                }

            }
        }


        public int kac_nesil()
        {
            int nesil = 0;

            if(chil.Count!=0)
            {
                nesil++;

                for(int i=0;i<chil.Count;i++)
                {
                    if(chil[i].chil.Count!=0)
                    {
                        nesil++;
                        chil[i].kac_nesil();
                    }
                }

            }

            return nesil;
        }


        public void sen_kimsin_lan(kisi k1,kisi k2,string akrabalik)//İlk büyük olan kişi
        {
            if(k2.father==k1)
            {
                akrabalik += "Babasıdır";
            }

            if(k2.mother==k1)
            {
                akrabalik += "Annesidir";
            }

            if(k1.father==k2.father && k1.mother==k2.mother)
            {
                akrabalik += "Kardeşidir";
            }

            for(int i=0;i<k2.chil.Count;i++)
            {
                
            }

        }



        public void uvey_kardes_bul(List<kisi> liste1,List<kisi> liste2)
        {
            
             for (int i = 0; i < liste1.Count; i++)
             {
                 for (int j = 0; j < liste1.Count; j++)
                 {

                    if(liste1[i].father==liste1[j].father && liste1[i].mother!=liste1[j].mother)
                    {
                        liste2.Add(liste1[i]);
                    }

                    if(liste1[i].father != liste1[j].father && liste1[i].mother == liste1[j].mother)
                    {
                        liste2.Add(liste1[i]);
                    }


                 }
             }
        }



    }

}

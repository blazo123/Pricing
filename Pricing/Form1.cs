using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Oracle.ManagedDataAccess.Client;
using System.Globalization;

namespace Pricing
{
    public partial class Form1 : Form
    {
        string nzlec;
        string odbiorca;
        string dataZlecenie;
        public int Width { get; set; }

        public Form1()
        {
            InitializeComponent();
            Read_Main();
        }
        public void Read_Main()
        {
            try
            {
                using (OracleConnection conn = new OracleConnection(Connection.oradb))
                using (OracleCommand cmd = new OracleCommand("SELECT VBELN as ZLECENIE,KUNNR as ODBIORCA,DATA_ZLEC as DATA,UZEIT_ZLEC as GODZINA,MAIL_UTWORZYL as UTWORZYŁ,MAIL_PH as PH, STATUS as ST,WARTOSC_N AS WARTOSC FROM DWS1.AUTOMAT_NGL_POZ", conn))
                {
                    if (conn.State == ConnectionState.Closed)
                    {
                        conn.Open();
                        using (OracleDataReader reader = cmd.ExecuteReader())
                        {
                            DataTable dataTable = new DataTable();
                            dataTable.Load(reader);
                            dataGridView1.DataSource = dataTable;
                            dataGridView1.RowHeadersVisible = false;
                            dataGridView2.Hide();
                            label3.Show();
                            dataGridView2.DataSource = "";
                            negativetbox.Clear();
                        }
                        conn.Close();
                    }
                   
                }
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        public void dataGridView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            wart_zleclbl.Text = "Wartosc zlecenia : " + dataGridView1.CurrentRow.Cells[7].Value.ToString();
            nzlec = dataGridView1.CurrentRow.Cells[0].Value.ToString();
            odbiorca = dataGridView1.CurrentRow.Cells[1].Value.ToString();
            dataZlecenie = dataGridView1.CurrentRow.Cells[2].Value.ToString();
            Give_Details_toDG2(nzlec, odbiorca);
        }

        private void dataGridView2_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            int numer = Convert.ToInt32(dataGridView2.CurrentRow.Index);

            string odbiorca = dataGridView2.Rows[numer].Cells[0].Value.ToString();
            string material = dataGridView2.Rows[numer].Cells[1].Value.ToString();

            try
            {
                using (OracleConnection conn5 = new OracleConnection(Connection.oradb))
                {
                    using (OracleCommand cmd12 = new OracleCommand("SELECT * FROM DWS1.AUTOMAT_MATERIAL_TABLE WHERE MATERIAL = '" + material.ToString() + "'", conn5))
                    {
                        if (conn5.State == ConnectionState.Closed)
                        {
                            conn5.Open();
                        }
                        using (OracleDataReader reader1 = cmd12.ExecuteReader())
                        {
                            reader1.Read();

                            Ilsprz90lbl.Text = "Ilosc Sprzedana 90/dni : " + reader1["Ilosc90dni"].ToString();
                            ilsprz30lbl.Text = "Ilosc Sprzedana 30/dni : " + reader1["Ilosc30dni"].ToString();
                            zapas90lbl.Text = "Zapas w dniach 90dni : " + reader1["Zapas_90_dni"].ToString();
                            zapas30lbl.Text = "Zapas w dniach 30dni : " + reader1["Zapas_30_dni"].ToString();
                            katmatlbl.Text = "Kat. Towaru : " + reader1["MAABC"].ToString();
                            stanATPlbl.Text = "Stan ATP : " + reader1["STANATP"].ToString();
                            StanNIelbl.Text = "Stan Nieog : " + reader1["STAN"].ToString();
                            MaterialNamelbl.Text = reader1["NAZWA_MATERIALU"].ToString();
                        }

                        using (OracleCommand cmd15 = new OracleCommand("SELECT material,data1,cena1,data2,cena2,data3,cena3 FROM DWS1.AUTOMAT_CENY_ODTW_TABLE WHERE MATERIAL = '" + material.ToString() + "'", conn5))
                        {
                            if (conn5.State == ConnectionState.Closed)
                            {
                                conn5.Open();
                            }
                            using (OracleDataReader reader3 = cmd15.ExecuteReader())
                            {
                                reader3.Read();
                                if (reader3.HasRows)
                                {
                                    cenaodtw1lbl.Text = reader3["CENA1"].ToString();
                                    cenaodtw2lbl.Text = reader3["CENA2"].ToString();
                                    cenaodtw3lbl.Text = reader3["CENA3"].ToString();

                                    dataodtw1lbl.Text = reader3["DATA1"].ToString();
                                    dataodtw2lbl.Text = reader3["DATA2"].ToString();
                                    dataodtw3lbl.Text = reader3["DATA3"].ToString();
                                }
                                else
                                {
                                    cenaodtw1lbl.Text = "---";
                                    cenaodtw2lbl.Text = "---";
                                    cenaodtw3lbl.Text = "---";

                                    dataodtw1lbl.Text = "---";
                                    dataodtw2lbl.Text = "---";
                                    dataodtw3lbl.Text = "---";
                                }

                            }

                        }

                        using (OracleCommand cmd13 = new OracleCommand("SELECT TYP_KLIENTA,wielkosc,ROUND(AVG(Srednia90),2)sr FROM DWS1.AUTOMAT_TYPKLIENTA_CENA_TABLE WHERE MATERIAL = '" + material.ToString() + "' GROUP BY TYP_KLIENTA,wielkosc ", conn5))
                        {
                            if (conn5.State == ConnectionState.Closed)
                            {
                                conn5.Open();
                            }
                            using (OracleDataReader reader3 = cmd13.ExecuteReader())
                            {
                                DataTable dataTable2 = new DataTable();
                                dataTable2.Load(reader3);
                                dataGridView3.DataSource = dataTable2;
                                dataGridView3.RowHeadersVisible = false;
                                dataGridView3.AutoResizeColumns();
                            }

                            using (OracleCommand cmd14 = new OracleCommand("SELECT odbiorca,material,cena1,cena2,cena3,srednia90,OSCENA FROM AUTOMAT_TYPKLIENTA_CENA_TABLE WHERE MATERIAL = '" + material.ToString() + "'" +
                                                "and odbiorca ='" + odbiorca.ToString() + "'", conn5))
                            {
                                if (conn5.State == ConnectionState.Closed)
                                {
                                    conn5.Open();
                                }
                                using (OracleDataReader reader2 = cmd14.ExecuteReader())
                                {
                                    reader2.Read();

                                    if (reader2.HasRows)
                                    {
                                        fstlastpricelbl.Text = reader2["CENA1"].ToString();
                                        seclastpricelbl.Text = reader2["CENA2"].ToString();
                                        trdlastprice.Text = reader2["CENA3"].ToString();
                                        lastdsprzMKlabl.Text = reader2["OSCENA"].ToString();
                                    }
                                    else
                                    {
                                        fstlastpricelbl.Text = "---";
                                        seclastpricelbl.Text = "---";
                                        trdlastprice.Text = "---";
                                        lastdsprzMKlabl.Text = "---";
                                    }
                                }
                            }

                           

                            conn5.Close();

                        }
                    }
                }
                    }
            catch (Exception)
            {

                MessageBox.Show("Nie dziala tabela dotyczaca materialu lub cen.");

            }

        }

        private void Give_Details_toDG2(string num,string odb)
        {
            DateTime dokiedyzlecenie = dateTimePicker1.Value.Date;

            string numer = num;
            string odbiorca = odb;

            try
            {
                using (OracleConnection conn1 = new OracleConnection(Connection.oradb))

                using (OracleCommand cmd1 = new OracleCommand("SELECT '" + odbiorca.ToString() + "' as ODBIORCA,MATERIAL as Material,PARTIA as Partia,JM as JM, CENA_MIN,CENA_PROP as Cena_Prop, ILOSC,CENA_NOWA, OKRES_DO,RABAT,VPRS_ZAKLAD_0001 as VPRS, MARZA_MIN,MARZA_PROP, MARZA_NOWA FROM DWS1.POZYCJE WHERE vbeln ='" + nzlec.ToString() + "'", conn1))
                {
                    conn1.Open();
                    using (OracleDataReader reader1 = cmd1.ExecuteReader())
                    {
                        DataTable dataTable1 = new DataTable();
                        dataTable1.Load(reader1);
                        label3.Hide();
                        dataGridView2.Show();
                        dataGridView2.DataSource = dataTable1;
                        ColumnProperty();
                        dataGridView2.RowHeadersVisible = false;

                        for (int a = 0; a < dataGridView2.Rows.Count; a++)
                        {
                            dataGridView2.Rows[a].Cells[8].Value = dokiedyzlecenie.Date.ToString("yyyyMMdd");
                        }



                        for (int c = 0; c < dataGridView2.Rows.Count; c++)
                        {
                            if (dataGridView2.Rows[c].Cells[7].Value.ToString() == "0")
                            {
                                dataGridView2.Rows[c].Cells[7].Value = dataGridView2.Rows[c].Cells[5].Value;

                            }
                        }

                    }

                    using (OracleCommand cmd11 = new OracleCommand("SELECT * FROM DWS1.AUTOMAT_KLIENT_TABLE WHERE ODBIORCA = '" + odbiorca.ToString() + "'", conn1))
                    {
                        using (OracleDataReader reader = cmd11.ExecuteReader())
                        {
                            reader.Read();

                            currSeglbl.Text = "Segment Obecny O/Z : " + reader["Segment-O/Z-2019"].ToString();

                            if (string.IsNullOrEmpty(reader["KONTROLOWANY"].ToString()) == true)
                            {
                                namofClientlbl.Text = reader["Nazwa_Firmy"].ToString();
                                namofClientlbl.ForeColor = Color.Black;
                            }
                            else
                            {
                                namofClientlbl.Text = reader["Nazwa_Firmy"].ToString();
                                namofClientlbl.ForeColor = Color.Red;
                            }

                            lastySeglbl.Text = "Segment Rok Poprzedni O/Z : " + reader["Segment-O/Z-2018"].ToString();
                            marzaII2018.Text = "Marza II '18 : " + reader["MARZA_II"].ToString();
                            marza2018_bezmag.Text = "Marza II_'18_Bez_Mag : " + reader["MARZA_II_BEZ_MAG"].ToString();
                            maxdatasprzlbl.Text = "Ost. Data Sprz/Klient : " + reader["Max. Data Faktury"].ToString();
                            marzacurryearlbl.Text = "Marża Rok Obecny : " + reader["Marza_Rok_Obecny"].ToString();
                            marzatotal.Text = "Marża Klient : " + reader["Marża_TOTAL"].ToString();
                            branzalbl.Text = "Branża : " + reader["Branza"].ToString();
                            nkUklbl.Text = "Czy NK/UK : " + reader["NK_UK"].ToString();
                        }
                    }

                    conn1.Close();
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Nie dziala tabela dotyczaca pozycji na zleceniu lub danych dotyczacych odbiorcy." );
            }
        }

       

        private void opiniabtn_Click(object sender, EventArgs e)
        {
            string a = this.dataGridView1.CurrentRow.Cells[0].Value.ToString();
            
            Smail m = new Smail();
            m.SendEmail("herman.a@marcopol.pl","Zlecenie nr: " + a + " budzi moje wątpliwości. Proszę o opinię", "Automat Cen Test");
            
        }

        private void negative_opinion_btn_Click(object sender, EventArgs e)
        {
            string powod = "Powód odrzucenia:  " + negativetbox.Text;

            Get_Status_Insert_SAP_and_Mail("O");

            Smail neg = new Smail();

            string adresaciMaila = adresaci();
            neg.SendEmail(adresaciMaila.ToString(),
                  "<font=Times New Roman >" +
                  "Zlecenie nr " + nzlec + " zostało odrzucone." +
                  "<p><br>" + powod.ToString() +
                  "<p><br>" +
                  "</font>",
                
                 "Zlecenie nr " + nzlec);


            MessageBox.Show("Treść: " + powod.ToString());

            ClearData();
            Read_Main();
        }

        private void positive_opinion_btn_Click(object sender, EventArgs e)
        {
            string uwagi = "Uwagi: " + negativetbox.Text;

            Get_Status_Insert_SAP_and_Mail("P");
            Smail poz = new Smail();


            string adresaciMaila = adresaci();

            poz.SendEmail(adresaciMaila.ToString(),
                       "<font=Times New Roman >" +
                       "Zlecenie nr " + nzlec + "  dla klienta " + namofClientlbl.Text + " zostało zaakceptowane i przekazane do SAP" +
                       "<p><br>" + uwagi.ToString() +
                       "<p><br>" +
                       "</font>"
                       , "Zlecenie nr " + nzlec + "  " + namofClientlbl.Text);

            ClearData();
            Read_Main();

        }


        private void Get_Status_Insert_SAP_and_Mail(string stat)
        {
            string dataZlecenie = dataGridView1.CurrentRow.Cells[2].Value.ToString();

            string dokiedyTest = DateTest();

            string StringQuery;
            string StringQuery2;


            try
            {
                using (OracleConnection conne1 = new OracleConnection(Connection.oradb))
                {
                    using (OracleCommand cmdstat3 = new OracleCommand())
                    {
                        cmdstat3.Connection = conne1;
                        conne1.Open();

                        for (int i = 0; i < dataGridView2.Rows.Count; i++)
                        {

                            string odb = dataGridView2.Rows[i].Cells[0].Value.ToString();
                            string mat = dataGridView2.Rows[i].Cells[1].Value.ToString();
                            string partia = dataGridView2.Rows[i].Cells[2].Value.ToString();
                            string JM = dataGridView2.Rows[i].Cells[3].Value.ToString();
                            var cena = dataGridView2.Rows[i].Cells[6].Value;
                            string CENA_MIN = dataGridView2.Rows[i].Cells[4].Value.ToString();
                            string CENA_ZPR1 = dataGridView2.Rows[i].Cells[5].Value.ToString();
                            string CENA_NOWA = dataGridView2.Rows[i].Cells[7].Value.ToString();



                            StringQuery = @"UPDATE DWS1.MONITOR_POS
                                            SET MANDT = '500',STATUS = '"
                                            + stat.ToString() + "',CENA_NOWA ='"
                                            + CENA_NOWA.ToString() + "',OKRES_OD ='"
                                            + dataZlecenie.ToString() + "',OKRES_DO = '"
                                            + dokiedyTest.ToString() + "' WHERE VBELN ='"
                                            + nzlec.ToString() + "' and MATNR ='"
                                            + mat.ToString() + "' and CHARG= '"
                                            + partia.ToString() + "'";                                           

                            cmdstat3.CommandText = StringQuery;
                            cmdstat3.ExecuteNonQuery();
                        }

                        StringQuery2 = @"UPDATE DWS1.MONITOR_NGL
                                         SET STATUS = '" + stat.ToString()
                                         + "'WHERE VBELN = '" + nzlec.ToString() + "'";

                        cmdstat3.CommandText = StringQuery2;
                        cmdstat3.ExecuteNonQuery();
      
                        MessageBox.Show("Zlecenie nr " + nzlec + " zostało zaakceptowane i przekazane do SAP"); 
                    }
                    conne1.Close(); 
                    
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

      

        public void dataGridView2_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
           

            string merge = dataGridView2.CurrentRow.Cells[10].Value.ToString();

            if (dataGridView2.CurrentCell.Value.Equals(0))
            {
                dataGridView2.CurrentRow.Cells[7].Value.Equals(0).ToString();
                
            }
            else
            {
                string v1 = dataGridView2.CurrentRow.Cells[7].Value.ToString();
                string v2 = dataGridView2.CurrentRow.Cells[10].Value.ToString();

                double vd1 = Convert.ToDouble(v1);
                double vd2 = Convert.ToDouble(v2);
                double vd3 = Math.Round(((vd1-vd2)/vd1)*100,2);

                string v3 = vd3.ToString() + " %";

                dataGridView2.CurrentRow.Cells[13].Value = v3.ToString();
            };


        }

        public string adresaci()
        {

            string adresat1 = dataGridView1.CurrentRow.Cells[4].Value.ToString();
            string adresat2 = dataGridView1.CurrentRow.Cells[5].Value.ToString();
            Smail.AddPH();


            if (Smail.ListPH.Contains(adresat1))
            {
                adresat1 = "";
            }
            else
            {
                adresat1 = dataGridView1.CurrentRow.Cells[4].Value.ToString();
            }
            if (Smail.ListPH.Contains(adresat2))
            {
                adresat2 = "";
            }
            else
            {
                adresat2 = dataGridView1.CurrentRow.Cells[5].Value.ToString();
            }

            string adresaciMaila = adresat1.ToString() + ";" + adresat2.ToString();
            return adresaciMaila;
        }

        public string DateTest()
        {
            CultureInfo provider = CultureInfo.InvariantCulture;
            string dokiedy = null;

            DateTime datadoKiedy = dateTimePicker1.Value.Date;

            DateTime dataZleceniaParse = DateTime.ParseExact(dataZlecenie, "yyyyMMdd", provider);

            if (dataZleceniaParse > datadoKiedy)
            {
                MessageBox.Show("Zlecenie nie może być zmienione dla daty wcześniejszej niż data zlecenia. Data zmiany będzie datą zlecenia");

                dokiedy = dataZlecenie.ToString();
            }
            else
            {
                dokiedy = datadoKiedy.Date.ToString("yyyyMMdd");

            }

            return dokiedy;
        }

        public void ColumnProperty()
        {
            DataGridViewColumn column = dataGridView2.Columns[0];
            column.Width = 80;

            DataGridViewColumn column2 = dataGridView2.Columns[1];
            column2.Width = 120;

            DataGridViewColumn column3 = dataGridView2.Columns[2];
            column3.Width = 55;

            DataGridViewColumn column4 = dataGridView2.Columns[3];
            column4.Width = 25;

            DataGridViewColumn column5 = dataGridView2.Columns[4];
            column5.Width = 70;

            DataGridViewColumn column6 = dataGridView2.Columns[5];
            column6.Width = 75;

            DataGridViewColumn column7 = dataGridView2.Columns[6];
            column7.Width = 55;

            DataGridViewColumn column8 = dataGridView2.Columns[7];
            column8.Width = 80;


            DataGridViewColumn column9 = dataGridView2.Columns[8];
            column9.Width = 75;

            DataGridViewColumn column10 = dataGridView2.Columns[9];
            column10.Width = 45;

            DataGridViewColumn column11 = dataGridView2.Columns[10];
            column11.Width = 45;

            DataGridViewColumn column12 = dataGridView2.Columns[11];
            column12.Width = 80;

            DataGridViewColumn column13 = dataGridView2.Columns[12];
            column13.Width = 90;

            DataGridViewColumn column14 = dataGridView2.Columns[13];
            column14.Width = 90;

        }

        private void RefreshData_Click(object sender, EventArgs e)
        {
            Read_Main();
        }

        private void ClearData()
        {
            Ilsprz90lbl.Text = "Ilosc Sprzedana 90/dni : ";
            ilsprz30lbl.Text = "Ilosc Sprzedana 30/dni : ";
            zapas90lbl.Text = "Zapas w dniach 90dni : ";
            zapas30lbl.Text = "Zapas w dniach 30dni : ";
            katmatlbl.Text = "Kat. Towaru : ";
            braki90lbl.Text = "Braki 90 dni : ";
            stanATPlbl.Text = "Stan ATP : ";
            StanNIelbl.Text = "Stan N : ";
            MaterialNamelbl.Text = "---";

            fstlastpricelbl.Text = "---";
            seclastpricelbl.Text = "---";
            trdlastprice.Text = "---";
            lastdsprzMKlabl.Text = "---";

            cenaodtw1lbl.Text = "---";
            cenaodtw2lbl.Text = "---";
            cenaodtw3lbl.Text = "---";

            dataodtw1lbl.Text = "---";
            dataodtw2lbl.Text = "---";
            dataodtw3lbl.Text = "---";

            lastySeglbl.Text = "Segment Rok Poprzedni O/Z : ";
            marzaII2018.Text = "Marza II '18 : ";
            marza2018_bezmag.Text = "Marza II_'18_Bez_Mag : ";
            maxdatasprzlbl.Text = "Ost. Data Sprz/Klient : ";
            marzacurryearlbl.Text = "Marża Rok Obecny : ";
            marzatotal.Text = "Marża Klient : ";
            branzalbl.Text = "Branża : ";
            nkUklbl.Text = "Czy NK/UK : ";
            namofClientlbl.Text = "---";
        }
    }
}

    

    

        

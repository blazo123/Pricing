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

namespace Pricing
{
    public partial class Form1 : Form
    {
        string nzlec;
        string oradb = "DATA SOURCE = DBOLAP; USER ID = DWS1; Password=DWS1";
        Connections con = new Connections();



        public Form1()
        {
            InitializeComponent();
            Read_Main();


        }


        public void Read_Main()
        {
            try
            {

                using (OracleConnection conn = new OracleConnection(oradb))
                using (OracleCommand cmd = new OracleCommand("SELECT DISTINCT vbeln as Zlecenie,kunnr as Odbiorca,erdat as Data,ernam as Utworzyl,oferta,ROUND((ILOSC_WZ * VPRS_0001_0),2) as Wartosc FROM OLAP_DANE.TSAP_REZERWACJE WHERE erdat >= SYSDATE -5 AND ROUND((ILOSC_WZ * VPRS_0001_0),2) > 0", conn))
                {
                    conn.Open();
                    using (OracleDataReader reader = cmd.ExecuteReader())
                    {
                        DataTable dataTable = new DataTable();
                        dataTable.Load(reader);
                        dataGridView1.DataSource = dataTable;
                        dataGridView1.RowHeadersVisible = false;

                     

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




            wart_zleclbl.Text = "Wartosc zlecenia : " + dataGridView1.CurrentRow.Cells[5].Value.ToString();
            nzlec = dataGridView1.CurrentRow.Cells[0].Value.ToString();
            Give_Details_toDG2();

        }

 

        private void dataGridView2_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            int numer = Convert.ToInt32(dataGridView2.CurrentRow.Index);

           string odbiorca  =  dataGridView2.Rows[numer].Cells[0].Value.ToString();
            string material = dataGridView2.Rows[numer].Cells[1].Value.ToString();


            
            try
            {


                using (OracleConnection conn4 = new OracleConnection(oradb))
                using (OracleCommand cmd11 = new OracleCommand("SELECT * FROM DWS1.AUTOMAT_KLIENT WHERE ODBIORCA = '" + odbiorca.ToString() + "'", conn4))
                {
                    conn4.Open();
                    using (OracleDataReader reader = cmd11.ExecuteReader())
                    {
                       


                        reader.Read();

                        currSeglbl.Text = "Segment Obecny O/Z : " + reader["Segment-O/Z-2018"].ToString();


                        lastySeglbl.Text = "Segment Rok Poprzedni O/Z : " + reader["Segment-O/Z-2017"].ToString();
                        maxdatasprzlbl.Text = "Ost. Data Sprz/Klient : " + reader["Max. Data Faktury"].ToString();
                        marzacurryearlbl.Text = "Marża Rok Obecny : " + reader["Marza_Rok_Obecny"].ToString();
                        marzatotal.Text = "Marża Klient : " + reader["Marża_TOTAL"].ToString();
                        branzalbl.Text = "Branża : " + reader["Branza"].ToString();

                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            try
            {

                using (OracleConnection conn5 = new OracleConnection(oradb))
                using (OracleCommand cmd12 = new OracleCommand("SELECT * FROM DWS1.AUTOMAT_MATERIAL WHERE MATERIAL = '" + material.ToString() + "'", conn5))
                {
                    conn5.Open();
                    using (OracleDataReader reader1 = cmd12.ExecuteReader())
                    {
                       reader1.Read();

                        Ilsprz90lbl.Text = "Ilosc Sprzedana 90/dni : " + reader1["Ilosc90dni"].ToString();
                        ilsprz30lbl.Text = "Ilosc Sprzedana 30/dni : " + reader1["Ilosc30dni"].ToString();
                        zapas90lbl.Text = "Zapas w dniach 90dni : " + reader1["Zapas_90_dni"].ToString();
                        zapas30lbl.Text = "Zapas w dniach 30dni : " + reader1["Zapas_30_dni"].ToString();
                        katmatlbl.Text = "Kat. Towaru : " + reader1["MAABC"].ToString();
                        braki90lbl.Text = "Braki 90 dni : " + reader1["BRAKI90"].ToString();
                        stanATPlbl.Text = "Stan ATP : " + reader1["STANATP"].ToString();
                        StanNIelbl.Text = "Stan N : " + reader1["STAN"].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            try
            {

                using (OracleConnection conn6 = new OracleConnection(oradb))
                using (OracleCommand cmd13 = new OracleCommand("SELECT TYP_KLIENTA,wielkosc,ROUND(AVG(Srednia90),2)sr FROM DWS1.automat_typklienta_cena WHERE MATERIAL = '" + material.ToString() + "' GROUP BY TYP_KLIENTA,wielkosc ", conn6))
                {
                    conn6.Open();
                    using (OracleDataReader reader3 = cmd13.ExecuteReader())
                    {
                        DataTable dataTable2 = new DataTable();
                        dataTable2.Load(reader3);
                        dataGridView3.DataSource = dataTable2;
                        dataGridView3.RowHeadersVisible = false;
                        dataGridView3.AutoResizeColumns();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }


            try
            {

                using (OracleConnection conn7 = new OracleConnection(oradb))
                using (OracleCommand cmd14 = new OracleCommand("SELECT odbiorca,material,cena1,cena2,cena3,srednia90,OSCENA FROM automat_typklienta_cena WHERE MATERIAL = '" + material.ToString() + "'" +
                    "and odbiorca ='" + odbiorca.ToString() + "'", conn7))
                {
                    conn7.Open();
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
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }



            try
            {

                using (OracleConnection conn8 = new OracleConnection(oradb))
                using (OracleCommand cmd15 = new OracleCommand("SELECT material,data1,cena1,wal1,data2,cena2,wal2,data3,cena3,wal3 FROM DWS1.automat_ceny_odtw WHERE MATERIAL = '" + material.ToString() + "'", conn8))
                {
                    conn8.Open();
                    using (OracleDataReader reader3 = cmd15.ExecuteReader())
                    {
                        reader3.Read();
                        if (reader3.HasRows)
                        {
                            cenaodtw1lbl.Text = reader3["CENA1"].ToString();
                            cenaodtw2lbl.Text = reader3["CENA2"].ToString();
                            cenaodtw3lbl.Text = reader3["CENA3"].ToString();

                            WALodtw1lbl.Text = reader3["WAL1"].ToString();
                            WALodtw2lbl.Text = reader3["WAL2"].ToString();
                            WALodtw3lbl.Text = reader3["WAL3"].ToString();

                            dataodtw1lbl.Text = reader3["DATA1"].ToString();
                            dataodtw2lbl.Text = reader3["DATA2"].ToString();
                            dataodtw3lbl.Text = reader3["DATA3"].ToString();
                        }
                        else
                        {
                            cenaodtw1lbl.Text = "---";
                            cenaodtw2lbl.Text = "---";
                            cenaodtw3lbl.Text = "---";

                            WALodtw1lbl.Text = "---";
                            WALodtw2lbl.Text = "---";
                            WALodtw3lbl.Text = "---";

                            dataodtw1lbl.Text = "---";
                            dataodtw2lbl.Text = "---";
                            dataodtw3lbl.Text = "---";
                        }
                       
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }




        }





        private void Give_Details_toDG2()
        {
            try
            {

                using (OracleConnection conn1 = new OracleConnection(oradb))
                using (OracleCommand cmd1 = new OracleCommand("SELECT DISTINCT kunnr as Odbiorca, MATNR as Material,CHARG as Partia,VPRS_0001_0 as VPRS,ernam as Utworzyl,oferta,ROUND((ILOSC_WZ * VPRS_0001_0),2) as Wartosc FROM OLAP_DANE.TSAP_REZERWACJE WHERE vbeln='" + nzlec.ToString() + "'", conn1))
                {
                    conn1.Open();
                    using (OracleDataReader reader1 = cmd1.ExecuteReader())
                    {
                        DataTable dataTable1 = new DataTable();
                        dataTable1.Load(reader1);
                        dataGridView2.DataSource = dataTable1;
                        dataGridView2.RowHeadersVisible = false;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void opiniabtn_Click(object sender, EventArgs e)
        {
            string a = this.dataGridView1.CurrentRow.Cells[0].Value.ToString();
            
            Smail m = new Smail();
            m.SendEmail("herman.a@marcopol.pl","Zlecenie nr: " + a + " budzi moje wątpliwości. Proszę o opinię", "Automat Cen Test");
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
         
        }
    }
}
        

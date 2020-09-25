using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Outlook = Microsoft.Office.Interop.Outlook;


namespace Pricing
{
    public class Smail
    {

        public static List<string> ListPH = new List<string>();

        public static void AddPH()
        {
            ListPH.Add("brzezinski.j@marcopol.pl");
            ListPH.Add("ruszkiewicz.r@marcopol.pl");
        }

        public void SendEmail(string dokogo, string tresc, string temat)
        {
            try { 
                Outlook._Application _app = new Outlook.Application();
                Outlook.MailItem mail = (Outlook.MailItem)_app.CreateItem(Outlook.OlItemType.olMailItem);
                mail.To = dokogo;
                mail.Subject = temat;
                mail.HTMLBody = tresc;

                string body = "<font=Times New Roman >" +
                    "<p><p>Pozdrawiamy," +
                    "<br>Dział Analiz i Polityki Cenowej" +
                    @"<p><img src = C:\Users\b_kolek.AD\Desktop\Projekt_Automa_Ceny\Pricing\Pricing\Pricing\marcopol.png" + " </a>" +
                    "<p>Marcopol Sp. z o.o. Producent Śrub" +
                    "<br>tel.: (+48 58) 55 40 418 fax.: (+48 58) 55 40 439" +
                    "<br>80-209 Chwaszczyno, ul. Oliwska 100" +
                    "<br>NIP 589 000 85 28, KRS 0000084735" +
                    "<br>Sąd Rejonowy w Gdańsku, VIII Wydział Gospodarczy" +
                    "<br>Kapitał zakładowy 2 425 404 zł" +
                    "</font>";

                mail.HTMLBody = tresc + body;
                mail.Importance = Outlook.OlImportance.olImportanceNormal;
                ((Outlook.MailItem)mail).Send();
                //System.Windows.Forms.MessageBox.Show("Twoja wiadomosc zostala wyslana poprawnie.","Message",System.Windows.Forms.MessageBoxButtons.OK,System.Windows.Forms.MessageBoxIcon.Information);
            }
            catch(Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message,"Message",System.Windows.Forms.MessageBoxButtons.OK);
            }
        }
    }
}

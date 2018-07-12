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
       


        public void SendEmail(string dokogo, string tresc, string temat)
        {
            try { 
            Outlook._Application _app = new Outlook.Application();
            Outlook.MailItem mail = (Outlook.MailItem)_app.CreateItem(Outlook.OlItemType.olMailItem);
            mail.To = dokogo;
            mail.Subject = temat;
            mail.Body = tresc;
            mail.Importance = Outlook.OlImportance.olImportanceNormal;
            ((Outlook.MailItem)mail).Send();
                System.Windows.Forms.MessageBox.Show("Twoja wiadomosc zostala wyslana poprawnie.","Message",System.Windows.Forms.MessageBoxButtons.OK,System.Windows.Forms.MessageBoxIcon.Information);

            }
            catch(Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message,"Message",System.Windows.Forms.MessageBoxButtons.OK);
            }



        }
        

            }
}

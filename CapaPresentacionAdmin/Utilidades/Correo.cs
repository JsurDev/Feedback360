using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Web;

namespace CapaPresentacionAdmin.Utilidades
{
    public class Correo
    {
        public static bool EnviarCorreo(string destino, string asunto, string cuerpo)
        {
            bool resultado = false;

            try
            {
                MailMessage mail = new MailMessage();
                mail.To.Add(destino);
                mail.From = new MailAddress("juansuriano01@gmail.com", "Juan Suriano - Gerente RH");
                mail.Subject = asunto;
                mail.Body = cuerpo;
                mail.IsBodyHtml = true;

                SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);
                smtp.Credentials = new NetworkCredential("juansuriano01@gmail.com",                                           "npll qfww zjpx lrmd");
                smtp.EnableSsl = true;

                smtp.Send(mail);
                resultado = true;
            }
            catch (Exception)
            {
                resultado = false;
            }
            return resultado;
        }
    }
}
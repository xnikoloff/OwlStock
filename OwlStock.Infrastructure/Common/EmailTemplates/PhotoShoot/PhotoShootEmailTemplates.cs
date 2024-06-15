using OwlStock.Domain.Enumerations;

namespace OwlStock.Infrastructure.Common.EmailTemplates.PhotoShoot
{
    public static class PhotoShootEmailTemplates
    {
        public static string CreatePhotoShootTemplate()
        {
            string template =
                "<!doctypehtml><title>Email Template</title><style>@media only screen and (max-width:600px){.container{width:100%!important}.responsive-img{width:100%!important;height:auto!important}}</style><body style=margin:0;padding:0><table border=0 cellpadding=0 cellspacing=0 width=100%><tr><td style=padding:20px align=center bgcolor=#f0f0f0><table border=0 cellpadding=0 cellspacing=0 width=600 class=container style=background-color:#fff><tr><td style=padding:20px;text-align:center><img src=https://i.ibb.co/Zg5pTVZ/email-check.png style=display: block; background: rgba(55, 128, 73, .2); color:#378049;width:20%;margin:0 auto;padding:10px;text-align:center;border-radius:10px><h1 style=color:#333;font-family:Arial,sans-serif>Благодарим Ви за резервацията!</h1><h2 style=color:#666;font-family:Arial,sans-serif;font-size:14px;line-height:1.5>Ще се свържем с Вас за повече информация</h2><p style=color:#666;font-family:Arial,sans-serif;font-size:14px;line-height:1.5>Можете да следите резервациите Ви във Вашия профил</p><a href=http://www.flash-studio.co/photoshoot/myphotoshoots style=display:inline-block;padding:10px 20px;color:#fff;background-color:#007bff;text-decoration:none;font-family:Arial,sans-serif;font-size:14px;border-radius:5px>Можете да следите кадрите от фотосесията тук</a><div class=footer-wrap style=margin-top:60px;width:100%;background:#585858;color:#fff;padding:1px 0><footer style=text-align:center><h4>DREAMPIX © 2024</h4></footer></div><div class=footer-links-wrap style=width:100%;text-align:center;margin-top:10px><a href=http://www.flash-studio.co style=width:400px;color:#424242;margin-left:20px;font-size:.9rem>DREAMPIX.COM</a> <a href=http://www.flash-studio.co style=width:400px;color:#424242;margin-left:20px;font-size:.9rem>@dreampix</a></div></table></table>";

            return template;
        }

        public static string CreatePhotoShootTemplateDreampix(string personFullName, DateTime date, PhotoShootType photoShootType)
        {
            return $"<div class=\"container\"> <div class=\"content\"> <h2 class=\"username\">Здравей,</h2> <br> <h1 class=\"text\">Направена е нова рервация.</i></h1> <hr> <div class=\"details\"> <table> <tbody> <tr> <td>Date:</td> <td><i>{date.ToShortDateString()}</i></td> </tr> <tr> <td>Type:</td> <td><i>{photoShootType}</i></td> </tr> </tbody> </table> </div> <hr> <div class=\"greetings\"</div> <hr> <div class=\"footer\"> <h1>OwlStock</h1> <br> <h4>2023</h4> </div> </div> </div> <style> *{{margin: 0;padding: 5px 8px;}}hr{{margin: 10px 10px;padding: 0;}}.container{{font-family:'Gill Sans', 'Gill Sans MT', Calibri, 'Trebuchet MS', sans-serif;width: 100%;text-align: center;}}.container .content{{width: 100%;margin: 0 auto;padding: 20px 0;background-color: #EBEAE1;}}.container .content .details table{{margin: 0 auto;}}.container .content .greetings h2{{margin-top: 20px;}}</style> ";
        }

        public static string UpdatePhotoShootTemplate(string personFullName, string url)
        {
            return $"<div class=\"container\"> <div class=\"content\"> <h2 class=\"username\">{personFullName},</h2> <br> <h1 class=\"text\">The photos for your photo shoot were just uploaded! <br> Check them out here: <br>{url}</h1> <hr> <div class=\"footer\"> <h1>OwlStock</h1> <br> <h4>2023</h4> </div> </div> </div> <style> *{{margin: 0;padding: 5px 8px;}}hr{{margin: 10px 10px;padding: 0;}}.container{{font-family:'Gill Sans', 'Gill Sans MT', Calibri, 'Trebuchet MS', sans-serif;width: 100%;text-align: center;}}.container .content{{width: 100%;margin: 0 auto;padding: 20px 0;background-color: #EBEAE1;}}.container .content .details table{{margin: 0 auto;}}.container .content .greetings h2{{margin-top: 20px;}}</style> ";
        }
    }
}

using OwlStock.Domain.Enumerations;

namespace OwlStock.Infrastructure.Common.EmailTemplates.PhotoShoot
{
    public static class PhotoShootEmailTemplates
    {
        public static string CreatePhotoShootTemplate(string personFullName, DateTime date, PhotoShootType photoShootType)
        {
            return $"<div class=\"container\"> <div class=\"content\"> <h2 class=\"username\">{personFullName},</h2> <br> <h1 class=\"text\">Your reservation was <i>successful!</i></h1> <hr> <div class=\"details\"> <table> <tbody> <tr> <td>Date:</td> <td><i>{date.ToShortDateString()}</i></td> </tr> <tr> <td>Type:</td> <td><i>{photoShootType}</i></td> </tr> </tbody> </table> </div> <hr> <div class=\"greetings\"> <h4>We'll contact you as soon as possible for <br> further details.</h4> <h2>We're looking forward <br> to meet you!</h2> </div> <hr> <div class=\"footer\"> <h1>OwlStock</h1> <br> <h4>2023</h4> </div> </div> </div> <style> *{{margin: 0;padding: 5px 8px;}}hr{{margin: 10px 10px;padding: 0;}}.container{{font-family:'Gill Sans', 'Gill Sans MT', Calibri, 'Trebuchet MS', sans-serif;width: 100%;text-align: center;}}.container .content{{width: 100%;margin: 0 auto;padding: 20px 0;background-color: #EBEAE1;}}.container .content .details table{{margin: 0 auto;}}.container .content .greetings h2{{margin-top: 20px;}}</style> ";
        }

        public static string UpdatePhotoShootTemplate(string personFullName, string url)
        {
            return $"<div class=\"container\"> <div class=\"content\"> <h2 class=\"username\">{personFullName},</h2> <br> <h1 class=\"text\">The photos for your photo shoot were just uploaded! <br> Check them out here: <br>{url}</h1> <hr> <div class=\"footer\"> <h1>OwlStock</h1> <br> <h4>2023</h4> </div> </div> </div> <style> *{{margin: 0;padding: 5px 8px;}}hr{{margin: 10px 10px;padding: 0;}}.container{{font-family:'Gill Sans', 'Gill Sans MT', Calibri, 'Trebuchet MS', sans-serif;width: 100%;text-align: center;}}.container .content{{width: 100%;margin: 0 auto;padding: 20px 0;background-color: #EBEAE1;}}.container .content .details table{{margin: 0 auto;}}.container .content .greetings h2{{margin-top: 20px;}}</style> ";
        }
    }
}

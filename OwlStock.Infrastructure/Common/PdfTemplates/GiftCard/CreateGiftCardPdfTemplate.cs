namespace OwlStock.Infrastructure.Common.PdfTemplates.GiftCard
{
    public class CreateGiftCardPdfTemplate
    {
         public static string CreateGiftCard(GiftCardTemplateBaseDTO dto)
        {
            return
            @$"
                <div class=""certificate-wrap"" style=""background:#ececec;font-family:'Montserrat',sans-serif;display:flex;justify-content:center;align-items:center;min-height:100vh;padding:40px;box-sizing:border-box;margin:0;"">
    
                    <div class=""certificate"" style=""width:1000px;max-width:100%;box-shadow:0 15px 40px rgba(0,0,0,.12);padding:90px 100px;position:relative;background:#dae0e3;box-sizing:border-box;"">

                        <div class=""header"" style=""text-align:center;margin-bottom:60px;position:relative;z-index:2;"">
                            <span class=""small-title"" style=""text-transform:uppercase;letter-spacing:6px;color:#627582;font-size:13px;"">
                                Подаръчен ваучер
                            </span>

                            <h1 style=""font-family:'Cormorant Garamond',serif;font-size:62px;font-weight:600;color:#222;margin:15px 0;"">
                                <img src=""~/img/icons/icon-1.png"" alt=""logo"" />
                            </h1>
                        </div>

                        <div class=""content"" style=""position:relative;z-index:2;"">

                            <p class=""presented"" style=""text-align:center;color:#777;letter-spacing:1px;margin-top:100px;margin-bottom:20px;font-size:24px;"">
                                Този ваучер е създаден специално за
                            </p>

                            <div class=""recipient"" style=""text-align:center;font-family:'Cormorant Garamond',serif;font-size:56px;font-weight:bold;color:#111;border-bottom:2px solid #627582;display:block;width:100%;padding-bottom:12px;margin-bottom:60px;box-sizing:border-box;"">
                                {dto.Receiver}
                            </div>

                            <div class=""details"" style=""display:flex;justify-content:space-between;gap:40px;margin-bottom:70px;"">

                                <div class=""item"" style=""flex:1;text-align:center;border:1px solid #627582;border-radius:8px;padding:30px;box-sizing:border-box;"">
                                    <span class=""label"" style=""display:block;color:#999;text-transform:uppercase;letter-spacing:2px;font-size:13px;margin-bottom:12px;"">
                                        Фотосесия
                                    </span>
                                    <span class=""value"" style=""font-size:22px;color:#222;font-weight:500;"">
                                        {dto.Photoshoot}
                                    </span>
                                </div>

                                <div class=""item"" style=""flex:1;text-align:center;border:1px solid #627582;border-radius:8px;padding:30px;box-sizing:border-box;"">
                                    <span class=""label"" style=""display:block;color:#999;text-transform:uppercase;letter-spacing:2px;font-size:13px;margin-bottom:12px;"">
                                        Валиден до
                                    </span>
                                    <span class=""value"" style=""font-size:22px;color:#222;font-weight:500;"">
                                        {dto.ValidUntil:dd.MM.yyyy} г   
                                    </span>
                                    <h6 class=""remove"" style=""font-style:italic"">/попълва автоматично след изтегляне/</h6>
                                </div>

                            </div>
                        </div>  

                        <div class=""footer"" style=""display:flex;justify-content:space-between;align-items:flex-end;position:relative;z-index:2;"">

                            <div class=""signature"" style=""width:240px;"">
                                <div class=""line"" style=""height:1px;background:#222;margin-bottom:8px;""></div>
                                <span style=""font-size:14px;color:#555;"">
                                    <b>PHOTONIC.bg | +359 878 131828</b>
                                </span>
                            </div>

                            <div class=""certificate-number"" style=""font-size:13px;color:#888;letter-spacing:2px;"">
                                <span>{dto.GiftCardNumber}</span>
                                <h6 class=""remove"" style=""font-style:italic"">/попълва автоматично след изтегляне/</h6>
                            </div>

                        </div>
                    </div>
                </div>
            ";
        }
    }
}

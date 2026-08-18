namespace OwlStock.Infrastructure.Common.PdfTemplates.GiftCard
{
    public class CreateGiftCardPdfTemplate
    {
         public static string CreateGiftCard(GiftCardTemplateBaseDTO dto)
        {
            return
            @$"
                <!DOCTYPE html>
        <html>
        <head>
            <meta charset=""utf-8"">

            <style>
                @page {{
                    margin: 0;
                }}

                body {{
                    margin: 0;
                    padding: 0;
                }}

                .page {{
                    width: 100%;
                    align-items: center;
                    margin-top: 30px;
                }}
                

                .card {{
                    width: 100%;
                    height: 430px;
                    background-color: #dae0e3;
                    padding: 15px;
                    margin: 0 auto;
                }}

                .header {{
                    text-align: center;
                    margin-top: 40px;
                }}

                .small-title {{
                    font-size: 12px;
                    color: #777;
                    letter-spacing: 3px;
                }}

                .title {{
                    font-size: 18px;
                    font-family: Cambria, Cochin, Georgia, Times, 'Times New Roman', serif, serif;
                    color: #777;
                    margin-top: 50px;
                    margin-bottom: 10px;
                }}

                .separator {{
                    width: 100%;
                    border-top: 1px solid #c7c7c7;
                    margin: 0 auto;
                    margin-top: 10px;
                    margin-bottom: 30px;
                }}

                .customer {{
                    text-align: center;
                    font-size: 48px;
                    font-family: Brush Script MT;
                    font-style: italic;
                    color: #404040;
                    margin-top: 15px;
                    margin-bottom: 5px;
                }}

                .photoshoot-label {{
                    text-align: center;
                    font-size: 12px;
                    color: #627582;
                    text-transform: uppercase;
                    letter-spacing: 2px;
                }}

                .photoshoot {{
                    text-align: center;
                    font-size: 30px;
                    font-weight: bold;
                    color: #627582;
                    margin-top: 5px;
                    margin-bottom: 25px;
                }}

                .validity-container{{
                    display: inline-block;
                    width: 100%;
                    text-align: center;
                    margin-top: 40px;
                }}

                .valid-until-label {{
                    font-size: 11px;
                    color: #627582;
                }}

                .valid-until {{
                    font-size: 18px;
                    font-weight: bold;
                    letter-spacing: 2px;
                    margin-top: 8px;
                }}

                .footer {{
                    text-align: center;
                    font-size: 12px;
                    color: #627582;
                    margin-top: 50px;
                    margin-bottom: 30px;
                }}

                .code {{
                    font-size: 14px;
                    letter-spacing: 2px;
                    margin-top: 8px;
                    margin-bottom: 10px;
                    color: #9ca8ad
                }}

                .logo {{
                    width: 300px;
                }}
            </style>
        </head>

        <body>

            <div class=""page"">

                <table class=""card"" cellpadding=""0"" cellspacing=""0"">
                    <tr>
                        <td>

                            <div class=""header"">

                                <div class=""small-title"">
                                    ПОДАРЪЧЕН ВАУЧЕР
                                </div>

                                <img
                                    class=""logo""
                                    src=""data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAZAAAACQCAYAAAAmyD14AAAACXBIWXMAAAsTAAALEwEAmpwYAAAE8WlUWHRYTUw6Y29tLmFkb2JlLnhtcAAAAAAAPD94cGFja2V0IGJlZ2luPSLvu78iIGlkPSJXNU0wTXBDZWhpSHpyZVN6TlRjemtjOWQiPz4gPHg6eG1wbWV0YSB4bWxuczp4PSJhZG9iZTpuczptZXRhLyIgeDp4bXB0az0iQWRvYmUgWE1QIENvcmUgMTAuMC1jMDAwIDI1LkcuZDIwZTQ2NiwgMjAyNS8xMi8wOC0yMDo1MDoyMSAgICAgICAgIj4gPHJkZjpSREYgeG1sbnM6cmRmPSJodHRwOi8vd3d3LnczLm9yZy8xOTk5LzAyLzIyLXJkZi1zeW50YXgtbnMjIj4gPHJkZjpEZXNjcmlwdGlvbiByZGY6YWJvdXQ9IiIgeG1sbnM6eG1wPSJodHRwOi8vbnMuYWRvYmUuY29tL3hhcC8xLjAvIiB4bWxuczpkYz0iaHR0cDovL3B1cmwub3JnL2RjL2VsZW1lbnRzLzEuMS8iIHhtbG5zOnBob3Rvc2hvcD0iaHR0cDovL25zLmFkb2JlLmNvbS9waG90b3Nob3AvMS4wLyIgeG1sbnM6eG1wTU09Imh0dHA6Ly9ucy5hZG9iZS5jb20veGFwLzEuMC9tbS8iIHhtbG5zOnN0RXZ0PSJodHRwOi8vbnMuYWRvYmUuY29tL3hhcC8xLjAvc1R5cGUvUmVzb3VyY2VFdmVudCMiIHhtcDpDcmVhdG9yVG9vbD0iQWRvYmUgUGhvdG9zaG9wIDI3LjcgKFdpbmRvd3MpIiB4bXA6Q3JlYXRlRGF0ZT0iMjAyNi0wMy0wN1QxMjozNzoyOSswMjowMCIgeG1wOk1vZGlmeURhdGU9IjIwMjYtMDYtMjhUMDA6NTM6MTErMDM6MDAiIHhtcDpNZXRhZGF0YURhdGU9IjIwMjYtMDYtMjhUMDA6NTM6MTErMDM6MDAiIGRjOmZvcm1hdD0iaW1hZ2UvcG5nIiBwaG90b3Nob3A6Q29sb3JNb2RlPSIzIiB4bXBNTTpJbnN0YW5jZUlEPSJ4bXAuaWlkOjhiMDkwMDZmLWE3ZWMtMDc0Mi1hZWJjLWM4ZWY5MjBlMGI0OSIgeG1wTU06RG9jdW1lbnRJ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""
                                    alt=""PHOTONIC""
                                />

                                <div class=""title"">
                                    Този ваучер е създаден специално за
                                </div>

                                
                            </div>
                            
                            <div class=""customer"">
                                {dto.Receiver}
                            </div>
                            <div class=""separator""></div>

                            <div class=""photoshoot-label"">
                                ФОТОСЕСИЯ
                            </div>

                            <div class=""photoshoot"">
                                {dto.Photoshoot}
                            </div>

                            <div class=""validity-container"">
                                    <div class=""valid-until-label"">
                                        ВАЛИДЕН ДО
                                    </div>
    
                                    <div class=""valid-until"">
                                        {dto.ValidUntil}
                                    </div>
                                </div>

                            <div class=""footer"">
    
                                    <div class=""code"">
                                        {dto.GiftCardNumber}
                                    </div>
                                <b>PHOTONIC</b>.bg | +359 878 131828
                            </div>

                        </td>
                    </tr>
                </table>

            </div>

        </body>
        </html>
    

            ";
        }
    }
}

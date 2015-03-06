using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using TestApi.Models;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;

//Ref: http://tech.pro/tutorial/1366/building-rest-api-with-mvc-4-web-api-part-3
namespace TestApi.Media
{
    public class ChuckFactPngFormatter : BufferedMediaTypeFormatter
    {
        public ChuckFactPngFormatter()
        {
            this.SupportedMediaTypes.Add(new MediaTypeHeaderValue("image/png"));
        }

        public override bool CanWriteType(Type type)
        {
            return type == typeof(ChuckFact);
        }

        public override bool CanReadType(Type type)
        {
            return false;
        }

        public override void WriteToStream(Type type, object value, Stream writeStream, HttpContent content)
        {
            ChuckFact fact = value as ChuckFact;
            if (fact == null)
            {
                throw new InvalidOperationException("Type not supported.");
            }

            WriteFactImage(writeStream, fact.Text);
        }
        private void WriteFactImage(Stream writeStream, string factText)
        {
            using (Bitmap bmp = new Bitmap(320, 240, PixelFormat.Format24bppRgb))
            {
                using (Graphics g = Graphics.FromImage(bmp))
                {
                    g.Clear(Color.Beige);
                    using (Font font = new Font("Arial", 15))
                    {
                        g.DrawString(factText, font, SystemBrushes.WindowText,
                            new Rectangle(0, 0, bmp.Width, bmp.Height),
                            new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
                    }
                }
                bmp.Save(writeStream, ImageFormat.Png);
            }
        }
    }
}
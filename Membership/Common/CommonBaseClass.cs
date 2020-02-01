using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Media.Imaging;

namespace Membership.Common
{
    public class CommonBaseClass
    {
        public static List<string> StateInits => new List<string>()
        {
            "AK","AL","AR","AZ","CA","CO","CT","DC","DE","FL","GA","HI","IA","ID","IL","IN","KS","KY","LA","MA","MD","ME","MI","MN","MO","MS","MT",
            "NC","ND","NE","NH","NJ","NM","NV","NY","OH","OK","OR","PA","RI","SC","SD","TN","TX","UT","VA","VT","WA","WI","WV","WY"
        };

        public static byte[] ImageToByteArray(Image image)
        {
            using (var ms = new MemoryStream())
            {
                image.Save(ms, ImageFormat.Jpeg);
                return ms.ToArray();
            }
        }

        public static Image ByteArrayToImage(byte[] imageData)
        {
            var ms = new MemoryStream(imageData);
            var image = (Bitmap)Image.FromStream(ms, false, true);
            return image;
        }

        public static Image ScaleImage(Image image, int height)
        {
            double ratio = (double)height / image.Height;
            int newWidth = (int)(image.Width * ratio);
            int newHeight = (int)(image.Height * ratio);
            Bitmap newImage = new Bitmap(newWidth, newHeight);
            using (Graphics g = Graphics.FromImage(newImage))
            {
                g.DrawImage(image, 0, 0, newWidth, newHeight);
            }
            image.Dispose();
            return newImage;
        }

        //public static IEnumerable<T>  RemoveCollectionItem<T>(List<T>lst)
        //{
        //    var ret = new List<T>();
        //    var remaining = new List<T>();
        //    foreach (var itm in lst)
        //    {
        //        if (Match(itm))
        //        {
        //            ret.Add(itm);
        //        }
        //        else
        //        {
        //            remaining.Add(itm);
        //        }
        //    }

        //    lst.Clear();
        //    lst.AddRange(remaining);
        //    return ret;
        //}

    }
}

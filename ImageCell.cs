using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using static NeuralNet.Program;

/// <summary>
/// 
/// </summary>
/// <author> Starshipladdev</author>
/// <email> Admin@starshiplad.com</email>
/// <Date> 1/02/2021 </Date>
/// <Title> NeuralNet </Title>
namespace NeuralNet
{
    /// <summary>
    /// Image Cell is a subclass of <see cref="Cell"/>, that handles providing data from Images specifically.
    /// </summary>
    class ImageCell : Cell
    {
        Bitmap img;
        public ImageCell(Bitmap subsection) :base()
        {
            img = subsection;
            DebugWrite("ImageCell->Cell()->", String.Format("Image Width/Height is {0},{1}", img.Width, img.Height), false); // Confirm Iamge is real and halfing each repitition
            BuildValuesNew();

        }
        /// <summary>
        /// CreateSubCell uses <see cref="Bitmap"/> functions to segregate the root image and fill the
        /// 'subCell' array with he relevant subsections of the root image.
        /// </summary>
        /// <param name="i">The X co-ord of the subsection to create</param>
        /// <param name="f">he Y co-ord of the subsection ot create</param>
        /// <returns>The created subcell made from the specified sub-image of the root image</returns>
        public override Cell CreateSubCell(int i, int f)
        {
            Bitmap subImage = new Bitmap(img.Width / 2, img.Height / 2);
            DebugWrite("ImageCell->Cell()->",String.Format("Image Width/Height is {0},{1}",img.Width,img.Height),false);
            // Create new subImage
            for (int z = 0; z < subImage.Height; z++)
            {
                for (int x = 0; x < subImage.Height; x++)
                {
                    subImage.SetPixel(z, x, img.GetPixel((img.Width / 2 * i) + z, (img.Width / 2 * f) + x));
                }
            }
            if (subCells[i, f] != null)
            {
                ImageCell.ChangeImage((ImageCell) subCells[i, f],subImage);
            }
            else
            {
                subCells[i, f] = new ImageCell(subImage);
            }
            return subCells[i, f];
        }

        /// <summary>
        /// Changes the root iamge. Should have <see cref="Cell.ReBuild"/> called after so new root values can
        /// be passed up.
        /// </summary>
        /// <param name="c">The Image Cell to change image for</param>
        /// <param name="img">The iamge to change to</param>
        public static void ChangeImage(ImageCell c, Bitmap img)
        {
            c.img = img;
        }
        public override bool IsLastCell()
        {
            if(img.Height==2 && img.Width == 2)
            {
                return true;
            }
            return false; 
        }
        /// <summary>
        /// Must inherit from <see cref="Cell"/>
        /// </summary>
        public override void FillLastSection()
        {
            values = ImageReader.ReadImage(img, values);
        }
    }
}


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
    /// Image Reader is used to conert a Bitmap Image into an array of <see cref="ValueClass"/>s . This is so 
    /// the process of getting values in the NeuralNet program can be abstracted. E.G - Antoher class could return
    /// the same values bu be tailored to Strings
    /// </summary>
    class ImageReader
    {


        /// <summary>
        /// ReadImage fills a pre-initalized array of ValueCLasses with either 1,2, or 3 as the value,
        /// based on the mirrored co-ordinates between a image's pixels and a Cell's 'values' array.
        /// E.G values[1,0].value will be 1,2 or 3  based on what pixel is in 1,0 of 'imageToRead
        /// </summary>
        /// <param name="imageToRead">The Bitmpa Image to convert to Value Clases</param>
        /// <param name="values">The vaue array to place the converted values in</param>
        /// <returns>A Value class array representing the image passed</returns>
        public static ValueClass[,] ReadImage(Bitmap imageToRead, ValueClass[,] values)
        {

            //START SECTION Get Colors Into an array
            for (int i = 0; i < values.GetLength(0); i++)
            {
                for (int f = 0; f < values.GetLength(1); f++)
                {
                    Color pixel = imageToRead.GetPixel(i, f);

                    DebugWrite("Cell->Cell()", "Getting Pixel " + i + "," + f + ". It is " + imageToRead.GetPixel(i, f), false); //Program is getting ARGB values correctly
                    int valueAtIndex = 0;
                    //Cannot use switch as Color not a const

                    //RED
                    if (pixel == Color.FromArgb(255, 255, 0, 0))
                    {
                        valueAtIndex = 1;
                    }
                    //GREEN
                    if (pixel == Color.FromArgb(255, 0, 255, 0))
                    {
                        valueAtIndex = 2;
                    }
                    //WHITE
                    if (pixel == Color.FromArgb(255, 255, 255, 255))
                    {
                        valueAtIndex = 0;
                    }
                    if (valueAtIndex > 0)
                    {
                        DebugWrite("Cell->Cell()", "Value is " + valueAtIndex, false); // Program is returning R & G
                    }
                    values[i, f].value = valueAtIndex;
                }
            }
            return values;
            //END SECTION Get Colors into an array
        }
    }
}

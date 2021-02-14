using System;
using System.Collections.Generic;
using System.Text;
using static NeuralNet.NeuralNet;

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
    /// Layer is used to store the whole value array of a root Cell.
    /// This is so when inputs are compared, the average of all differnces is factored in rather than a randomly chosen
    /// subsection of a Cell that is provided in Cell.GetCall()
    /// </summary>
   class Layer
    {
        //colour (White Green Red),  x,y
        public ValueClass[,,] values;
        public int[,,] inputs;
        public Cell baseCell;
        public Layer()
        {
            //Color (White,Green,Red,x,y)
            values = new ValueClass[2, 2, 3];
            inputs = new int[2, 2, 3];
            for (int f = 0; f < values.GetLength(0); f++)
            {
                for (int z = 0; z < values.GetLength(1); z++)
                {
                    for (int x = 0; x < values.GetLength(2); x++)
                    {
                        values[f, z, x] = new ValueClass(f, 0);
                        inputs[f, z, x] = 1;
                    }
                }
            }

        }
        /// <summary>
        /// Avrages the addition of a new value at a certain co-ordinate subsection. 
        /// E.G if root Cell values[1,1] had a 3.0 added to he Layer's current sotred value for that co-ordinate of 4.0,
        /// The new average value that co-ordiante should have is 3.5
        /// </summary>
        /// <param name="c">The Cell that is providing the new weight</param>
        public void AddValue(Cell c)
        {
            for (int i = 0; i < values.GetLength(0); i++)
            {
                for (int f = 0; f < values.GetLength(1); f++)
                {
                    DebugWrite("Layer->AddValue->", String.Format("i is {0} and f is {1}", i, f), false);
                    values[i, f, c.values[i, f].value].AvgWeight(c.values[i, f].weight, inputs[i, f, c.values[i, f].value]);
                    inputs[i, f, c.values[i, f].value]++;
                }
            }
        }
        /// <summary>
        /// Reads out all values and weights for each subsection o the root cell. This is for debug purposes
        /// </summary>
        /// <param name="read">A silly redundant variable</param>
        public void ReadValues(bool read = true)
        {
            for (int f = 0; f < values.GetLength(0); f++)
            {
                for (int z = 0; z < values.GetLength(1); z++)
                {
                    for (int x = 0; x < values.GetLength(2); x++)
                    {
                        DebugWrite("Layer->ReadValues->", String.Format("Value (0=W,1=R,2=G) is {0} ,x,y is {2},{3}weight is {1}), false", f, values[f, z, x].weight, z, x), read);
                    }
                }
            }

        }

    }
}

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
    /// An Interace that requires a class has a way to fill 'values' array of ValueClass with data
    /// </summary>
    interface ValueFillClass
    {
        public void BuildValues();
    }
    /// <summary>
    /// Cell is a representation of a subsection of an input, which in itself is the 'root' cell.
    /// 'Leaf' cells (denoted by 'isLastSection' variable) are the Cells that provided base values and
    ///weights, which are offset by set ammounts by their paretn cells, and so on until the 'root' Cell
    /// Provies a randomly selected value and weight from all leaf nodes
    /// </summary>
    abstract class Cell:ValueFillClass
    {
        public Cell[,] subCells;
        public ValueClass[,] values; //For Each cell , Tell me how much weight you put on that cell
        public ValueClass[,,] offsetValues; //For Each cell , Tell me how much weight you give to 
        public bool lastSection = false;
        public double smallChange = 0.001;
        public double bigChange = 0.004;
        public bool[,] lastPickedCell;
        public int valuesTested=3;
        
        public Cell()
        {
            
            values = new ValueClass[2, 2];
            offsetValues = new ValueClass[2, 2, valuesTested];
            lastPickedCell = new bool[2, 2];
            //START SECTION FILL ALL VALUES WITH 0.0
            for (int i = 0; i < values.GetLength(0); i++)
            {
                for (int f = 0; f < values.GetLength(1); f++)
                {
                    values[i, f] = new ValueClass(0, 0.0);
                    for (int offsets = 0; offsets < valuesTested; offsets++)
                    {
                        offsetValues[i, f, offsets] = new ValueClass(offsets, 0.0);
                    }
                }
            }
            
        }
        public abstract void FillLastSection();
        public abstract bool IsLastCell();
        public abstract Cell CreateSubCell(int i, int f);
        /// <summary>
        /// BuildValues is used to fill the Cell's variables. It makes calls to the abstract
        /// fucntions 'FillLastSection' and 'CreateSubCell', as these are dependent on the Cell type
        /// Being created
        /// </summary>
        public void BuildValues()
        {
            //START SECTION IF FINAL CELL
            if (IsLastCell())
            {
                lastSection = true;
                FillLastSection();
            }
            //END SECTION IF FINAL CELL

            //START SECTION IF NOT FINAL CELLS
            else
            {
                //START SECTION CREATE SUBIMAGES/CELS
                subCells = new Cell[2, 2];
                for (int i = 0; i < subCells.GetLength(0); i++)
                {
                    for (int f = 0; f < subCells.GetLength(1); f++)
                    {
                        subCells[i, f] = CreateSubCell(i,f);
                    }
                }
                //END SECTION CREATE SUBIMAGES/CELLS
            }
            //END IF NOT FINAL CELLS
            
        }
        /// <summary>
        /// Mirrors <see cref="BuildValues"/> except it gives each leaf Cell value a weight of 0.25 
        /// as each leaf node currently has 4 values, so each has a 25% weight to start.
        /// </summary>
        public void BuildValuesNew()
        {
            BuildValues();
            if (lastSection)
            {
                for (int i = 0; i < values.GetLength(0); i++)
                {
                    for (int f = 0; f < values.GetLength(1); f++)
                    {
                        values[i, f].weight = 0.25;
                    }
                }
            }
        }
        /// <summary>
        /// Copys the weights and offsets of another Cell so a new input can return an value with this 
        /// cell culmative offsets and weights, for comaprrison purposes.
        /// </summary>
        /// <param name="copy"></param>
        public void CopyCell(Cell copy)
        {
            for (int i = 0; i < 2; i++)
            {
                for (int f = 0; f < 2; f++)
                {
                    values[i, f] = new ValueClass(values[i, f].value, values[i, f].weight);
                    for (int r = 0; r < valuesTested; r++)
                    {
                        offsetValues[i, f, r] = new ValueClass(offsetValues[i, f, r].value, offsetValues[i, f, r].weight);
                    }
                    if (!lastSection)
                    {
                        subCells[i, f].CopyCell(copy.subCells[i, f]);
                    }
                }
            }
        }
        /// <summary>
        /// Rebuild Retreives new values for the Cell based on a new input. 
        /// This is so the same 'trained' cell can use different inputs
        /// </summary>
        public void ReBuild()
        {
            if (IsLastCell())
            {
                lastSection = true;
                FillLastSection();
            }
            //END SECTION IF FINAL CELL
            //START SECTION IF NOT FINAL CELLS
            else
            {
                //START SECTION CREATE SUBIMAGES/CELLS
                for (int i = 0; i < subCells.GetLength(0); i++)
                {
                    for (int f = 0; f < subCells.GetLength(1); f++)
                    {
                        subCells[i, f] = CreateSubCell(i, f);
                        subCells[i, f].ReBuild();
                    }
                }
                //END SECTION CREATE SUBIMAGES/CELLS
            }
        }
        /// <summary>
        /// Radomly selects one of it's subsection's values and weights, adds it's own offsets, and return that value
        /// </summary>
        /// <returns>a random value and attatched weight, that has weight/1.0 chane of being selected</returns>
        public ValueClass GetCall()
        {
            double pickedWeight = 0.0;
            Random rand = new Random();
            double[] weightTotal = new double[4];
            ValueClass pickedValueClass = null;
            int count = 0;
            for (int i = 0; i < values.GetLength(0); i++)
            {
                for (int f = 0; f < values.GetLength(1); f++)
                {

                    if (!lastSection)
                    {
                        values[i, f] = subCells[i, f].GetCall();
                        DebugWrite("Cell-> GetCall() -> Is Not Last Section", "Recursing is Done with value " + values[i, f].value, false); // Confirm recurrsion worked and got values
                    }
                    DebugWrite("Cell->GetCall()", "count is " + count + ", i,f weight values are " + values[i, f].weight, false);//Count isn't being adjusted too much, values are not null

                    for (int v = count; v < weightTotal.Length; v++)
                    {
                        weightTotal[v] += values[i, f].weight;
                    }

                    count++;


                }
            }
            DebugWrite("Cell-> GetCall() -> All Cells", "Values top left  " + values[0, 0].value + "," + values[0, 0].weight, false); //Confirm Cell's value
            DebugWrite("Cell-> GetCall() -> All Cells", "Values top right are " + values[0, 1].value + "," + values[0, 1].weight, false);//Confirm Cell's value
            DebugWrite("Cell-> GetCall() -> All Cells", "Values bottom left are " + values[1, 0].value + "," + values[1, 0].weight, false);//Confirm Cell's value
            DebugWrite("Cell-> GetCall() -> All Cells", "Values bottom right  are " + values[1, 1].value + "," + values[1, 1].weight, false);//Confirm Cell's value
            double pickedNumber = (rand.NextDouble() + 1) % weightTotal[3];
            for (int i = 0; i < weightTotal.Length; i++)
            {
                DebugWrite("Cell->GetCall()", "pickNumber is  " + pickedNumber + ", weightTotal[i] is " + weightTotal[i] + ".", false); //See what range of numbers can be randomly picked from
                if (pickedNumber <= weightTotal[i])
                {
                    int x = 0;
                    int y = 0;
                    if (i < 2)
                    {
                        x = 0;
                        y = i;
                    }
                    else
                    {
                        x = 1;
                        y = i - 2;
                    }
                    lastPickedCell[x, y] = true;
                    pickedValueClass = values[x, y];
                    DebugWrite("Cell->GetCall()-> Pick Made", "x,y is " + x + " " + y + "." + " and returned values are " +values[x,y].value+","+values[x,y].weight, false); //See that there is a good spread of cells picked

                    ValueClass returnValueClass = new ValueClass(pickedValueClass.value, pickedValueClass.weight);
                    DebugWrite("Cell->GetCall()-> Pick Made", String.Format("offsetValues has lengths {0},{1},{2}", offsetValues.GetLength(0), offsetValues.GetLength(1), offsetValues.GetLength(2)), false);//See that the offset Array is valid

                    returnValueClass.weight += this.offsetValues[x, y, returnValueClass.value].weight;
                    i = weightTotal.Length + 1;
                    pickedValueClass = returnValueClass;
                    break;
                }
            }
            DebugWrite("Cell->GetCall()-> Done Getting Cell", " Return Value is " + pickedValueClass.value + ", Picked Weight is " + pickedWeight, false);
            return pickedValueClass;

        }
        /// <summary>
        /// Lowers the weight for the subsection last picked, reccursivly.
        /// This lwoers the confidence the last subsection picked has values that match the tested attribute
        /// </summary>
        public void TellOff()
        {
            for (int i = 0; i < values.GetLength(0); i++)
            {
                for (int f = 0; f < values.GetLength(1); f++)
                {
                    if (lastPickedCell[i, f])
                    {
                        values[i, f].weight -= smallChange;
                    }
                }
            }
            if (!lastSection)
            {
                for (int i = 0; i < subCells.GetLength(0); i++)
                {
                    for (int f = 0; f < subCells.GetLength(1); f++)
                    {
                        if (lastPickedCell[i, f])
                        {
                            offsetValues[i, f, values[i, f].value].weight -= smallChange;
                            subCells[i, f].TellOff();
                        }

                    }
                }
            }
            PreserveValues();

        }

        /// <summary>
        /// Opposite of <see cref="TellOff"/>
        /// </summary>
        public void Reward()
        {
            for (int i = 0; i < values.GetLength(0); i++)
            {
                for (int f = 0; f < values.GetLength(1); f++)
                {
                    if (lastPickedCell[i, f])
                    {
                        offsetValues[i, f, values[i, f].value].weight += smallChange;
                        values[i, f].weight += smallChange;
                    }
                }
            }
            if (!lastSection)
            {
                for (int i = 0; i < subCells.GetLength(0); i++)
                {
                    for (int f = 0; f < subCells.GetLength(1); f++)
                    {
                        if (lastPickedCell[i, f])
                        {
                            subCells[i, f].Reward();
                        }

                    }
                }
            }
            PreserveValues();

        }
        /// <summary>
        /// Preserve Values keeps all value's weights between 0 and 1, for accuracy puposes.
        /// </summary>
        private void PreserveValues()
        {
            if (subCells != null)
            {
                for (int i = 0; i < subCells.GetLength(0); i++)
                {
                    for (int f = 0; f < subCells.GetLength(1); f++)
                    {
                        if (values[i, f].weight < 0)
                        {
                            values[i, f].weight = 0.000001;
                        }
                        if (values[i, f].weight > 1)
                        {
                            values[i, f].weight = 0.9999999;
                        }
                    }
                }
            }


        }
    }
}

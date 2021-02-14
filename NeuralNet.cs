using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;

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
    /// Program is he Main Entry Point of the program.
    /// Inputs -> Input data, in this case Image of apples
    /// Value -> The raw values of the input, in this case red,green and white pixels
    /// Attribute -> A testable option that all inputs are one of, in this case, 'apple' or 'not apple'
    /// </summary>
    /// <note> The type of 'Cell' being actioned needs to be specifically handled in the main method</note>
    /// <note> This version specifically handles Image Cells of color type 'R,G,W'</note>
    ///<example> Loading Images into <see cref="ImageCell"/> classes and then testing them</example> 
    static class NeuralNet
    {


        //Default DebugWrite Tag for all Debug statements
        public static String imageBeingSearched = "Main";
        static String output = "This is Output";

        //Whether DebugWrite will output debug satements
        static bool debug = true;
        //Whether the last iamge tested was an apple
        public static bool lastWasApple = false;
        /// The Average Timespan a creation and testing of a 'cell' takes
        static TimeSpan averageCellTime = TimeSpan.FromSeconds(0);
        static double averageDifference = 0.0;
        public static double averageDifferenceWhenWrong = 0.0;
        public static double highestDifferenceWhenRight = 0.0;

        public static double accuracy = 0.95;
        public static Layer testLayer;

        /// <summary>
        /// Debug Writer is a utility class used to handle the debug writing of statements for developers.
        /// It exsists to have a centeral location to edit how debug statements are written.
        /// </summary>
        /// <param name="tag"> The prefix to the statement, specifiying a point in the program where the statement occured</param>
        /// <param name="input">The Debug message to write</param>
        /// <param name="print">Whether to print statements that arn't mandatory (See next)</param>
        /// <param name="mandatoryPrint">Whether the message should print, even if 'print' is false</param>
        public static void DebugWrite(String tag, String input, bool print,bool mandatoryPrint=false)
        {
            if ((debug && print ) || mandatoryPrint)
            {
                String displayText = imageBeingSearched + "->" + tag + "->\"" + input + "\"" + "\n";
                Debug.WriteLine(displayText);
                Console.WriteLine(displayText);
                output = displayText;
            }
        }
        /// <summary>
        /// RunNeuralNet is the running of a new Neural Net.It creates a <see cref="Layer"/> class that is trained up
        /// up with test data and then compared agaisnt a copy of that Layer that uses new data. The comparison data
        /// is not known, so the difference in values between the Layer that has known inputs and the new layer is used
        /// to determine the 'confidece' that the new data does have attribute. 
        /// </summary>
        /// <note> Used to be the main method of a stand-alone console program</note>
        public static void RunNeuralNet()
        {
            Console.WriteLine("Hello World!");
            int size = 9;
            Bitmap[] inputs = new Bitmap[size];
            String[] inputstrings = new string[size];
            testLayer = new Layer();
            int testAmmounts = 2;

            int rightCount = 0;
            //DO CORRECT IMAGES
            AddWeightsPreTest(9, "Main->Correct Weight Add ->", "True", true, testLayer,testAmmounts);
            //ADD INCORRECT IMAGES TO WEIGHTS
            AddWeightsPreTest(9, "Main->Correct Weight Add ->", "False", false, testLayer, testAmmounts);

            //TEST IMAGING
            //Test False Images
            imageBeingSearched = "Testing";
            for (int i = 0; i < inputs.Length; i++)
            {
                inputstrings[i] = "False/Image" + (i + 1) + ".png";
                DebugWrite("Main -> False Test Section -> ", " Loading Image " + inputstrings[i], false);
                inputs[i] = (Bitmap)Image.FromFile(inputstrings[i]);
            }
            for (int i = 0; i < inputs.Length; i++)
            {
                TestData(inputstrings[i], testLayer, accuracy);
                if(!lastWasApple)
                {
                    rightCount++;
                }
            }
            averageDifferenceWhenWrong = averageDifference;
            //TEST TRUE IMAGES
            imageBeingSearched = "Testing";
            for (int i = 0; i < inputs.Length; i++)
            {
                inputstrings[i] = "True/Image" + (i + 1) + ".png";
                DebugWrite("Main -> True Test Section -> ", " Loading Image " + inputstrings[i], false);
                inputs[i] = (Bitmap)Image.FromFile(inputstrings[i]);
            }
            highestDifferenceWhenRight = 0.0;
            for (int i = 0; i < inputs.Length; i++)
            {
                TestData(inputstrings[i], testLayer, 1 - averageDifferenceWhenWrong);
                if (lastWasApple)
                {
                    rightCount++;
                }
                highestDifferenceWhenRight += averageDifference;
                if (i > 0)
                {
                    highestDifferenceWhenRight = highestDifferenceWhenRight / 2;
                }
                /*
                if (averageDifference > highestDifferenceWhenRight)
                {
                    highestDifferenceWhenRight = averageDifference;
                }
                */
            }
            if (rightCount < (inputs.Length * 2) - 1)
            {
                RunNeuralNet();
            }


        }
        /// <summary>
        /// TestData is the method used to compare two layers.
        /// It outputs the difference in probability that two layers share attributes to the static varable
        /// <see cref="averageDifference"/>. It does this as the main focus of this application.
        /// </summary>
        /// <param name="reply"> The poorly-named variable of the location of the data point being tested</param>
        /// <param name="testLayer"> The Layer object that has had weights assigned to it using data from inputs with known attributes</param>
        /// <param name="accuracy">The accuracy /1 (100%) the layer needs to have to be considered 'similar' </param>
        public static void TestData(String reply,Layer testLayer,double accuracy)
        {
            averageDifference = 0.0;
            Layer newLayer = new Layer();
            imageBeingSearched = reply;
            Cell cell = new ImageCell((Bitmap)Image.FromFile(reply));
            cell.CopyCell(testLayer.baseCell);
            newLayer.baseCell = cell;
            for (int i = 0; i < 40; i++)
            {
                cell.ReBuild();
                cell.GetCall();
                DebugWrite("Main->Testing() ->", reply + " Got Cell Values", false); //Confirm Image can be read and provide values
                newLayer.AddValue(cell);
                DebugWrite("Main->Testing() ->", reply + " Had values added to it's layer, comparing", false); // Confir mLAyer can add values correctly
                CompareLayers(testLayer, newLayer, accuracy, false);
            }
            if (averageDifference < (1.0 - accuracy))
            {
                lastWasApple = true;
                DebugWrite("Main->Testing() ->", reply + " Apparently WAS an apple", true,true);
            }
            else
            {
                lastWasApple = false;
                DebugWrite("Main->Testing() ->", reply + " Apparently WASN'T an apple", true,true);
            }
            DebugWrite("main->Testing->", "Average Difference was " + averageDifference +", accuracy was "+accuracy, true);
        }

        /// <summary>
        /// AddWeightsPreTest is used to modify a Layer with a seires of inputs, training it to either be
        /// biased towards inputs with similar values, or opposed to inputs with simialr values
        /// </summary>
        /// <param name="size">The size of the input arrays</param>
        /// <param name="tag">The tag that <see cref="DebugWrite"/> method will use as it's 'tag'</param>
        /// <param name="imageLocation">The File loation of the data to be checked (.png)</param>
        /// <param name="isCorrect">Whether the Layer will be trained up to bias or anti-bias the inputs' values</param>
        /// <param name="testLayer">The 'Layer' class that is being trained</param>
        /// <param name="testAmmounts">How many itterations of training will be performed</param>
        private static void AddWeightsPreTest(int size, String tag, String imageLocation , bool isCorrect,Layer testLayer , int testAmmounts) {

            Bitmap[] inputs = new Bitmap[size];
            String[] inputstrings = new String[size];
            for (int i = 0; i < inputs.Length; i++)
            {
                inputstrings[i] = imageLocation +"/Image"+ (i + 1) + ".png";
                DebugWrite(tag, " Loading Image " + inputstrings[i], true);// Confirm Image Loaded
                inputs[i] = (Bitmap)Image.FromFile(inputstrings[i]);
            }
            imageBeingSearched = inputstrings[0];
            DebugWrite(tag, " Image 0 height is " + inputs[0].Height, true);// Confirm Images are valid
            testLayer.baseCell = new ImageCell(inputs[0]);
            DebugWrite(tag, " Begining Loop to Test Cells", true);// Section Definition

            for (int i = 0; i < inputs.Length; i++)
            {
                for (int r = 0; r < testAmmounts; r++)
                {

                    DateTime nowTime = DateTime.Now;
                    imageBeingSearched = inputstrings[i];
                    DebugWrite(tag, " Testing & Rewarding Image " + inputstrings[i], true); // State Which image is being actioned
                    ImageCell.ChangeImage((ImageCell)testLayer.baseCell, inputs[i]);
                    DebugWrite(tag, " Rebuilding Image " + inputstrings[i], true); // State that image has changed correctly 
                    testLayer.baseCell.ReBuild();
                    DebugWrite(tag, " Getting Call of Image " + inputstrings[i], true); // State that 'GetCall()' Method is running successfully
                    ValueClass debugValueClass = testLayer.baseCell.GetCall();
                   // DebugWrite(tag, " Image Call was  " + debugValueClass.value + "," + debugValueClass.weight, false); // Get the value Class of  the training

                    for (int xAxis = 0; xAxis < 2; xAxis++)
                    {
                        for (int yAxis = 0; yAxis < 2; yAxis++)
                        {
                            if (isCorrect)
                            {

                                testLayer.AddValue(testLayer.baseCell);
                                testLayer.baseCell.Reward();
                            }
                            else
                            {
                                testLayer.baseCell.TellOff();
                            }


                        }
                    }
                    averageCellTime += DateTime.Now.Subtract(nowTime);
                    double averageCellTimeMili = averageCellTime.TotalMilliseconds;
                    averageCellTime = TimeSpan.FromMilliseconds(averageCellTimeMili / 2);
                }

            }
            DebugWrite(tag, " Average Time Per Cell is " + averageCellTime, true, true); //Optimize time of calculation



        }

        /// <summary>
        /// CompareLayers is used to Compare two Layers, one that is trained correctly and one that is 
        /// of unkown attributes. If the total average difference between them after several tests is less that
        /// a certain accuracy ,As per <see cref="Main(string[])"/>  then the two layers do not share attributes.
        /// </summary>
        /// <param name="correctLayer">The Layer class that has been trained with inputs who's attributes are known</param>
        /// <param name="incorectLayer">The Layer being tested against 'correctLayer'</param>
        /// <param name="accuracy">The accuracy, 1.0 being 100%, the 'incorectLayer' msut have to be considered the same</param>
        /// <param name="debug">Whether debug statements will print</param>
        private static void CompareLayers(Layer correctLayer, Layer incorectLayer, double accuracy, bool debug = true)
        {
            DebugWrite("Main->CompareLayer()", String.Format("Begining comparison"), false); // State that method can run

            for (int i = 0; i < 2; i++)
            {
                for (int f = 0; f < 2; f++)
                {
                    //For each cell
                    double overallDifference = 0.0;
                    double totalDifference;
                    for (int colors = 0; colors < 3; colors++)
                    {
                        //For each color in each cell

                        totalDifference = Math.Abs(correctLayer.values[i, f,colors].weight - incorectLayer.values[i, f,colors].weight);
                        overallDifference += totalDifference;
                        DebugWrite("Main->CompareLayer", String.Format(colors + " is the color( 1 red, 2 green, 0 white), x is {0} , y is {1} , colorWeights are {2} and {3} difference is {4}.", i, f, correctLayer.values[i, f, colors].weight, incorectLayer.values[i, f, colors].weight, totalDifference), false);


                    }
                    overallDifference = overallDifference / 3;
                    averageDifference += overallDifference ;
                    averageDifference = averageDifference / 2;
                    DebugWrite("Main->CompareLayer", String.Format("OverallDifference is {0}, average differnece is {1}",overallDifference,averageDifference), debug);
                    DebugWrite("Main->CompareLayer", String.Format("x is {0} , y is {1} , overall difference for that cell is {2}.", i, f, overallDifference), debug);

                }
            }
        }

        /// <summary>
        /// A class used to store a value(int - enum of a type of value Cells may have) and a weight (double - out of 1.0 contribution that value adds)
        /// </summary>
        public class ValueClass
        {
            public int value;
            public double weight;
            public ValueClass(int value, double weight)
            {
                this.value = value;
                this.weight = weight;
            }
            public void SetWeight(double modifier)
            {
                weight += modifier;
            }
            public void AvgWeight(double modifier, int attempts)
            {
                weight = (weight + modifier) / 2;
            }
            public override string ToString()
            {
                String returnS = base.ToString() + " " + this.value + "," + this.weight;
                return returnS;

            }
        }
    }
}

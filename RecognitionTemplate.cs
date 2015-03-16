using System;
using System.Drawing;
using Microsoft.Ink;

namespace Microsoft.Ink.SymbolRecognizers
{
	/// <summary>
	/// Maps ink pictures to their associated strings.
	/// </summary>
	public class RecognitionTemplate
	{
        private static RecognitionPair[] recognitionSet = 
        {
            new RecognitionPair(new double[] { 0.0652454780361757, 44.350495049505, 41.2673267326733 }, "Q"),
            new RecognitionPair(new double[] { 0.070452474946857, 42.5603448275862, 56.0804597701149 }, "R Underlined")
        };

        public static string Recognize(Ink inputInk)
        {
            double[] currentStatistics = ComputeStatistics(inputInk);
            double bestBet = Double.MinValue;
            string recognizedString = "";

            foreach(RecognitionPair recognitionPair in recognitionSet)
            {
                double similarity = Similarity(recognitionPair.RecognitionVector, currentStatistics);
                if(similarity > bestBet)
                {
                    bestBet = similarity;
                    recognizedString = recognitionPair.RelatedText;
                }
            }

            return recognizedString;
        }

        private static double Similarity(double[] vector1, double[] vector2)
        {
            double dotProduct = 0;

            for(int counter = 0; counter < vector1.Length; counter++)
                dotProduct += (vector1[counter] * vector2[counter]);
            dotProduct /= (Magnitude(vector1) * Magnitude(vector2));

            return dotProduct;
        }

        private static double Magnitude(double[] vector)
        {
            double runningCount = 0;

            for(int counter = 0; counter < vector.Length; counter++)
                runningCount += (vector[counter] * vector[counter]);
            
            return Math.Sqrt(runningCount);
        }

        private static double[] ComputeStatistics(Ink inputInk) 
        {
            byte[] saved = inputInk.Save(PersistenceFormat.Gif, CompressionMode.Maximum);
            System.IO.MemoryStream stream = new System.IO.MemoryStream(saved);
            Bitmap currentImage = new Bitmap(stream);
            stream.Close();

            // Density
            int onCount = 0;
            int offCount = 0;

            // XAvg
            int xCount = 0;

            // YAvg
            int yCount = 0;

            for(int xPos = 0; xPos < currentImage.Width; xPos++)
            {
                for(int yPos = 0; yPos < currentImage.Height; yPos++)
                {
                    Color currentColor = currentImage.GetPixel(xPos, yPos);
                    double brightness = currentColor.GetBrightness();
                    if(brightness < 0.50)
                    {
                        xCount += xPos;
                        yCount += yPos;
                        onCount++;
                    }
                    else
                        offCount++;
                }
            }
            
            // x center
            double xCenter = (double) xCount / (double) onCount;

            // y center
            double yCenter = (double) yCount / (double) onCount;

            // density
            double density = (double) onCount / (double) (offCount + onCount);

            return new double[] { density, xCenter, yCenter };
        }
	}

    internal class RecognitionPair
    {
        private double[] recognitionVector;
        private string relatedText;

        public RecognitionPair(double[] recognitionVector, string relatedText)
        {
            this.recognitionVector = recognitionVector;
            this.relatedText = relatedText;
        }

        public double[] RecognitionVector
        {
            get { return recognitionVector; }
            set { recognitionVector = value; }
        }

        public string RelatedText
        {
            get { return relatedText; }
            set { relatedText = value; }
        }
    }
}

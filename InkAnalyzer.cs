using System;
using System.Text;
using System.Collections;

using Microsoft.Ink;
using System.Drawing;

using System.CodeDom;
using System.CodeDom.Compiler;
using Microsoft.CSharp;

namespace SymbolTrainer
{
	/// <summary>
	/// Summary description for InkAnalyzer.
	/// </summary>
    public class InkAnalyzer
    {
        private ArrayList trainingSet;

        public InkAnalyzer()
        {
            trainingSet = new ArrayList();
        }

        public void AddItem(double[] recognitionVector, string relatedText)
        {
            RecognitionPair recognitionPair = new RecognitionPair(recognitionVector, relatedText);
            trainingSet.Add(recognitionPair);
        }

        public string Generate(string className, string dllPath, string inkDllLocation)
        {
            StringBuilder recognitionTemplate = new StringBuilder(@"
using System;
using System.Collections;
using System.Drawing;
using Microsoft.Ink;

namespace Microsoft.Ink.SymbolRecognizers
{
	/// <summary>
	/// Maps ink pictures to their associated strings.
	/// </summary>
	public class <CLASSNAME>
	{
        <RECOGNITIONSET>

        public static string Recognize(Ink inputInk)
        {
            double[] currentStatistics = ComputeStatistics(inputInk);
            double bestBet = Double.MinValue;
            string recognizedString = """";

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
            if((Magnitude(vector1) * Magnitude(vector2)) > 0)
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

        public static double[] ComputeStatistics(Ink inputInk) 
        {
            ArrayList statistics = new ArrayList();

            double strokeCount = (double) ((double)inputInk.Strokes.Count / (double)5);
            statistics.Add(strokeCount);

            Rectangle r = inputInk.Strokes.GetBoundingBox();
            Point rotatePoint = new Point(r.Left + (r.Width/2), r.Top + (r.Height/2));

            for(float rotateAngle = 0; rotateAngle < 45; rotateAngle += 15)
            {
                inputInk.Strokes.Rotate(15, rotatePoint);

                byte[] saved = inputInk.Save(PersistenceFormat.Gif, CompressionMode.Maximum);
                System.IO.MemoryStream stream = new System.IO.MemoryStream(saved);
                Bitmap currentImage = new Bitmap(stream);
                stream.Close();

                // Density
                int onCount = 0, offCount = 0;

                // XAvg, YAvg
                int xCount = 0, yCount = 0;

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
            
                double xCenter = 0;
                double yCenter = 0;

                if(onCount > 0)
                {
                    xCenter = (double) xCount / (double) (onCount*currentImage.Width);
                    yCenter = (double) yCount / (double) (onCount*currentImage.Height);
                }

                // x mean, y mean
                double xMean = (double) xCount / (double) (currentImage.Width * currentImage.Width * currentImage.Height);
                double yMean = (double) yCount / (double) (currentImage.Height * currentImage.Width * currentImage.Height);

                // density
                double density = (double) onCount / (double) (offCount + onCount);

                // Aspect Ratio
                double aspectRatio = (double) currentImage.Height / (double) (currentImage.Width+currentImage.Height);

                statistics.Add(xCenter);
                statistics.Add(yCenter);
                statistics.Add(xMean);
                statistics.Add(yMean);
                statistics.Add(density);
                statistics.Add(aspectRatio);
            }
            inputInk.Strokes.Rotate(-45, rotatePoint);

            return (double[]) statistics.ToArray(typeof(double));
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
");
            string recognitionPairTemplate =
                "private static RecognitionPair[] recognitionSet = \n" +
                "{\n" +
                GenerateRecognitionPairs() +
                "};\n";

            recognitionTemplate.Replace("<CLASSNAME>", className);
            recognitionTemplate.Replace("<RECOGNITIONSET>", recognitionPairTemplate);

            string codeToCompile = recognitionTemplate.ToString();
            return CompileCode(codeToCompile, dllPath, inkDllLocation);
        }

        private string CompileCode(string codeToCompile, string dllDestination, string inkDllLocation)
        {
            CodeSnippetCompileUnit csu = new CodeSnippetCompileUnit(codeToCompile);            
        
            // Obtains an ICodeCompiler from a CodeDomProvider class.
            CSharpCodeProvider provider = new CSharpCodeProvider();
            ICodeCompiler compiler = provider.CreateCompiler();

            // Configure the compiler parameters
            string[] assemblies = 
            { 
                @"System.Drawing.dll", 
                inkDllLocation
            };
            CompilerParameters cp = new CompilerParameters(assemblies);

            // Indicates that a .dll should be generated.
            cp.GenerateExecutable = false;
            cp.OutputAssembly = dllDestination;

            // Invokes compilation. 
            CompilerResults cr = compiler.CompileAssemblyFromDom(cp, csu);
            
            if(cr.Errors.Count > 0)
            {
                string errorLines = "Error: Could not build DLL:";

                foreach(CompilerError error in cr.Errors)
                    errorLines += "\n\t" + error.Line + ":" + error.ErrorText;
                return errorLines;
            }

            return "";
        }

        private string GenerateRecognitionPairs()
        {
            StringBuilder builtString = new StringBuilder();

            for(int counter = 0; counter < trainingSet.Count; counter++)
            {
                RecognitionPair currentPair = (RecognitionPair) trainingSet[counter];
                String nextString = String.Format("\tnew RecognitionPair(new double[] {0}, \"{1}\")",
                    GetDoubleString(currentPair.RecognitionVector),
                    currentPair.RelatedText);

                builtString.Append(nextString);
                
                if(counter < trainingSet.Count - 1)
                    builtString.Append(",\n");
                else
                    builtString.Append("\n");
            }

            return builtString.ToString();
        }

        private string GetDoubleString(double[] entries)
        {
            StringBuilder builtString = new StringBuilder("{ ");

            for(int counter = 0; counter < entries.Length; counter++)
            {
                builtString.Append(entries[counter].ToString());
                if(counter < entries.Length - 1)
                    builtString.Append(", ");
            }

            builtString.Append(" }");

            return builtString.ToString();
        }

        public static double[] ComputeStatistics(Ink inputInk) 
        {
            ArrayList statistics = new ArrayList();

            double strokeCount = (double) ((double)inputInk.Strokes.Count / (double)5);
            statistics.Add(strokeCount);

            Rectangle r = inputInk.Strokes.GetBoundingBox();
            Point rotatePoint = new Point(r.Left + (r.Width/2), r.Top + (r.Height/2));

            for(float rotateAngle = 0; rotateAngle < 45; rotateAngle += 15)
            {
                inputInk.Strokes.Rotate(15, rotatePoint);

                byte[] saved = inputInk.Save(PersistenceFormat.Gif, CompressionMode.Maximum);
                System.IO.MemoryStream stream = new System.IO.MemoryStream(saved);
                Bitmap currentImage = new Bitmap(stream);
                stream.Close();

                // Density
                int onCount = 0, offCount = 0;

                // XAvg, YAvg
                int xCount = 0, yCount = 0;

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
            
                double xCenter = 0;
                double yCenter = 0;

                if(onCount > 0)
                {
                    xCenter = (double) xCount / (double) (onCount*currentImage.Width);
                    yCenter = (double) yCount / (double) (onCount*currentImage.Height);
                }

                // x mean, y mean
                double xMean = (double) xCount / (double) (currentImage.Width * currentImage.Width * currentImage.Height);
                double yMean = (double) yCount / (double) (currentImage.Height * currentImage.Width * currentImage.Height);

                // density
                double density = (double) onCount / (double) (offCount + onCount);

                // Aspect Ratio
                double aspectRatio = (double) currentImage.Height / (double) (currentImage.Width+currentImage.Height);

                statistics.Add(xCenter);
                statistics.Add(yCenter);
                statistics.Add(xMean);
                statistics.Add(yMean);
                statistics.Add(density);
                statistics.Add(aspectRatio);
            }
            inputInk.Strokes.Rotate(-45, rotatePoint);

            return (double[]) statistics.ToArray(typeof(double));
        }
	}

    public class RecognitionPair
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

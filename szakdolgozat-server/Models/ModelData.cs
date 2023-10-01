using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Microsoft.ML.OnnxRuntime.Tensors;
using System.Drawing;

namespace szakdolgozat_server.Models
{
    public class ModelData
    {
        public float[] ImageData { get; set; }

        public ModelData(byte[] imageBytes)
        {
            this.ImageData = ConvertImageBytesToFloatArray(imageBytes);
        }

        public Tensor<float> AsTensor()
        {
            int width = 224;
            int height = 224;
            int channels = 3;

            // Populate imageData with your image data (e.g., pixel values)

            // Check if the data size matches the expected input tensor size
            if (ImageData.Length != width * height * channels)
            {
                throw new ArgumentException("Input data size does not match the expected input tensor size.");
            }

            // Create the input tensor with the specified dimensions
            int[] dimensions = new int[] { 1, height, width, channels };
            return new DenseTensor<float>(ImageData, dimensions);
        }
        public float[] ConvertImageBytesToFloatArray(byte[] imageBytes)
        {
            // Load the image from byte[]
            Mat image = new Mat();
            CvInvoke.Imdecode(imageBytes, ImreadModes.Color, image);

            // Resize the image to 224x224 if needed
            int targetWidth = 224;
            int targetHeight = 224;
            Size targetSize = new Size(targetWidth, targetHeight);
            if (image.Size != targetSize)
            {
                CvInvoke.Resize(image, image, targetSize, interpolation: Inter.Linear);
            }

            // Convert the image pixels to floats (assuming it's in the range [0, 255])
            float[] floatArray = new float[224 * 224 * 3];
            int index = 0;

            for (int y = 0; y < targetHeight; y++)
            {
                for (int x = 0; x < targetWidth; x++)
                {
                    Bgr pixel = new Bgr(image.ToImage<Bgr, byte>().Data[y, x, 0],
                            image.ToImage<Bgr, byte>().Data[y, x, 1],
                            image.ToImage<Bgr, byte>().Data[y, x, 2]);
                    floatArray[index++] = (float)pixel.Blue / 255.0f;  // Blue channel
                    floatArray[index++] = (float)pixel.Green / 255.0f; // Green channel
                    floatArray[index++] = (float)pixel.Red / 255.0f;   // Red channel
                }
            }

            return floatArray;
        }
    }
}

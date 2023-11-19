using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Face;
using Emgu.CV.Structure;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FaceRecognition
{
    public partial class Login : Form
    {
        private double distance = 1E+19;
        private CascadeClassifier CascadeClassifier = new CascadeClassifier(Environment.CurrentDirectory + "/Haarcascade/haarcascade_frontalface_alt.xml");
        private Image<Bgr, byte> Frame = null;
        private Emgu.CV.Capture camera;
        private Mat mat = new Mat();
        private List<Image<Gray, byte>> trainedFaces = new List<Image<Gray, byte>>();
        private List<int> PersonLabs = new List<int>();
        private PictureBox PictureBox_smallFrame;
        private string setPersonName;
        public bool isTrained = false;
        private List<string> Names = new List<string>();
        private EigenFaceRecognizer eigenFaceRecognizer;
        private string detectedFacesDirectory = Path.Combine(Environment.CurrentDirectory, "DetectedFaces");


        public Login()
        {
            InitializeComponent();
            open_cam(logCam);
            InitializeDetectedFacesDirectory();
        }
        private void InitializeDetectedFacesDirectory()
        {
            try
            {
                if (!Directory.Exists(detectedFacesDirectory))
                {
                    Directory.CreateDirectory(detectedFacesDirectory);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating detected faces directory: {ex.Message}");
            }
        }
        public void open_cam(object P_opencam)
        {
            this.PictureBox_smallFrame = logCam;
            this.camera = new Emgu.CV.Capture();
            this.camera.ImageGrabbed += new EventHandler(this.Camera_ImageGrabbed);
            this.camera.Start();
        }
        private void Camera_ImageGrabbed(object sender, EventArgs e)
        {
            this.camera.Retrieve((IOutputArray)this.mat);
            this.Frame = this.mat.ToImage<Bgr, byte>().Resize(this.PictureBox_smallFrame.Width, this.PictureBox_smallFrame.Height, Inter.Cubic);
            this.detectFace();
            this.PictureBox_smallFrame.Image = (Image)this.Frame.Bitmap;
        }
        private void detectFace()
        {
            Image<Bgr, byte> resultImage = this.Frame.Convert<Bgr, byte>();
            Mat mat = new Mat();
            CvInvoke.CvtColor((IInputArray)this.Frame, (IOutputArray)mat, ColorConversion.Bgr2Gray);
            CvInvoke.EqualizeHist((IInputArray)mat, (IOutputArray)mat);
            Rectangle[] rectangleArray = this.CascadeClassifier.DetectMultiScale((IInputArray)mat, minNeighbors: 4);
            if (rectangleArray.Length != 0)
            {
                foreach (Rectangle rectangle in rectangleArray)
                {
                    CvInvoke.Rectangle((IInputOutputArray)this.Frame, rectangle, new Bgr(Color.LimeGreen).MCvScalar, 2);
                    resultImage.ROI = rectangle;
                    this.checkName(resultImage, rectangle);
                    this.trainedIamge();
                    this.checkName(resultImage, rectangle);
                }
            }
            else
                this.setPersonName = "";
        }
        private void trainedIamge()
        {
            try
            {
                int numComponents = 0;
                this.trainedFaces.Clear();
                this.PersonLabs.Clear();
                this.Names.Clear();
                foreach (string file in Directory.GetFiles(Directory.GetCurrentDirectory() + "\\Image", "*.jpg", SearchOption.AllDirectories))
                {
                    this.trainedFaces.Add(new Image<Gray, byte>(file));
                    this.PersonLabs.Add(numComponents);
                    this.Names.Add(file);
                    ++numComponents;
                }

                // Ensure there are faces to train
                if (numComponents > 0)
                {
                    this.eigenFaceRecognizer = new EigenFaceRecognizer(numComponents, this.distance);
                    this.eigenFaceRecognizer.Train<Gray, byte>(this.trainedFaces.ToArray(), this.PersonLabs.ToArray());
                    this.isTrained = true;
                }
                else
                {
                    this.isTrained = false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in trainedIamge: {ex.Message}");
                this.isTrained = false;
            }
        }

        private void registerbtn_Click(object sender, EventArgs e)
        {
            FaceRec fr = new FaceRec();
            fr.ShowDialog();
        }

        private void checkName(Image<Bgr, byte> resultImage, Rectangle face)
        {
            try
            {
                if (!this.isTrained || this.eigenFaceRecognizer == null || this.trainedFaces == null || this.Names == null)
                    return;

                Image<Gray, byte> image = resultImage.Convert<Gray, byte>().Resize(100, 100, Inter.Cubic);
                CvInvoke.EqualizeHist((IInputArray)image, (IOutputArray)image);

                if (this.trainedFaces.Count > 0 && this.Names.Count > 0)
                {
                    FaceRecognizer.PredictionResult predictionResult = this.eigenFaceRecognizer.Predict((IInputArray)image);

                    if (predictionResult.Label != -1 && predictionResult.Distance < this.distance)
                    {
                        this.setPersonName = this.Names[predictionResult.Label].Replace(Environment.CurrentDirectory + "\\Image\\", "").Replace(".jpg", "");

                        if (predictionResult.Label < this.trainedFaces.Count)
                        {
                            this.PictureBox_smallFrame.Image = (Image)this.trainedFaces[predictionResult.Label].Bitmap;
                        }

                        CvInvoke.PutText((IInputOutputArray)this.Frame, this.setPersonName, new Point(face.X - 2, face.Y - 2), FontFace.HersheyPlain, 1.0, new Bgr(Color.LimeGreen).MCvScalar);

                        string detectedFacePath = Path.Combine(detectedFacesDirectory, this.setPersonName + "_Detected.jpg");
                        if (!File.Exists(detectedFacePath))
                        {
                            resultImage.Save(detectedFacePath);
                        }
                    }
                    else
                    {
                        CvInvoke.PutText((IInputOutputArray)this.Frame, "Unknown", new Point(face.X - 2, face.Y - 2), FontFace.HersheyPlain, 1.0, new Bgr(Color.OrangeRed).MCvScalar);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in checkName: {ex.Message}");
            }
        }


        private void loginbtn_Click(object sender, EventArgs e)
        {
            detectFace();
            bool isuser = isTrained;

            if (isuser)
            {
                string detectedFacePath = Path.Combine(detectedFacesDirectory, setPersonName + "_Detected.jpg");

                if (File.Exists(detectedFacePath))
                {
                    User log = new User();
                    log.ShowDialog();
                }
                else
                {
                    MessageBox.Show("No matching face found for login.");
                }
            }
        }
    }
}

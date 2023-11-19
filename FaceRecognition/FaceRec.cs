using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Face;
using Emgu.CV.Structure;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace FaceRecognition
{
    public partial class FaceRec : Form
    {
        private double distance = 1E+19;
        private CascadeClassifier CascadeClassifier = new CascadeClassifier(Environment.CurrentDirectory + "/Haarcascade/haarcascade_frontalface_alt.xml");
        private Image<Bgr, byte> Frame = null;
        private Emgu.CV.Capture camera;
        private Mat mat = new Mat();
        private List<Image<Gray, byte>> trainedFaces = new List<Image<Gray, byte>>();
        private List<int> PersonLabs = new List<int>();
        private bool isEnable_SaveImage = false;
        private string ImageName;
        private PictureBox PictureBox_Frame;
        private PictureBox PictureBox_smallFrame;
        private string setPersonName;
        public bool isTrained = false;
        private List<string> Names = new List<string>();
        private EigenFaceRecognizer eigenFaceRecognizer;

        public FaceRec()
        {
            InitializeComponent();

            if (!Directory.Exists(Environment.CurrentDirectory + "\\Image"))
                Directory.CreateDirectory(Environment.CurrentDirectory + "\\Image");
        }

        public void getPersonName(Control control)
        {
            Timer timer = new Timer();
            timer.Tick += (sender, e) => control.Text = this.setPersonName;
            timer.Interval = 100;
            timer.Start();
        }

        public void openCamera(PictureBox pictureBox_Camera, PictureBox pictureBox_Trained)
        {
            this.PictureBox_Frame = pictureBox_Camera;
            this.PictureBox_smallFrame = pictureBox_Trained;
            this.camera = new Emgu.CV.Capture();
            this.camera.ImageGrabbed += (sender, e) => Camera_ImageGrabbed(sender, e);
            this.camera.Start();
        }

        public void Save_IMAGE(string imageName)
        {
            this.ImageName = imageName;
            this.isEnable_SaveImage = true;
        }

        private void Camera_ImageGrabbed(object sender, EventArgs e)
        {
            this.camera.Retrieve(this.mat);
            this.Frame = this.mat.ToImage<Bgr, byte>().Resize(this.PictureBox_Frame.Width, this.PictureBox_Frame.Height, Inter.Cubic);
            this.detectFace();
            this.PictureBox_Frame.Image = this.Frame.Bitmap;
        }

        private void detectFace()
        {
            Image<Bgr, byte> resultImage = this.Frame.Convert<Bgr, byte>();
            Mat grayMat = new Mat();
            CvInvoke.CvtColor(this.Frame, grayMat, ColorConversion.Bgr2Gray);
            CvInvoke.EqualizeHist(grayMat, grayMat);
            Rectangle[] rectangleArray = this.CascadeClassifier.DetectMultiScale(grayMat, minNeighbors: 4);
            if (rectangleArray.Length != 0)
            {
                foreach (Rectangle rectangle in rectangleArray)
                {
                    CvInvoke.Rectangle(this.Frame, rectangle, new Bgr(Color.LimeGreen).MCvScalar, 2);
                    this.SaveImage(rectangle);
                    resultImage.ROI = rectangle;
                    this.trainedIamge();
                    this.checkName(resultImage, rectangle);
                }
            }
            else
            {
                this.setPersonName = "";
            }
        }

        private void SaveImage(Rectangle face)
        {
            if (!this.isEnable_SaveImage)
                return;

            using (Image<Bgr, byte> image = this.Frame.Clone())
            {
                image.ROI = face;
                image.Resize(100, 100, Inter.Cubic).Save(Environment.CurrentDirectory + "\\Image\\" + this.ImageName + ".jpg");
            }

            this.isEnable_SaveImage = false;
            this.trainedIamge();
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
                this.eigenFaceRecognizer = new EigenFaceRecognizer(numComponents, this.distance);
                this.eigenFaceRecognizer.Train<Gray, byte>(this.trainedFaces.ToArray(), this.PersonLabs.ToArray());
                this.isTrained = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in trainedIamge: {ex.Message}");
            }
        }

        private void checkName(Image<Bgr, byte> resultImage, Rectangle face)
        {
            try
            {
                if (!this.isTrained)
                    return;

                using (Image<Gray, byte> image = resultImage.Convert<Gray, byte>().Resize(100, 100, Inter.Cubic))
                {
                    CvInvoke.EqualizeHist(image, image);
                    FaceRecognizer.PredictionResult predictionResult = this.eigenFaceRecognizer.Predict(image);
                    if (predictionResult.Label != -1 && predictionResult.Distance < this.distance)
                    {
                        this.PictureBox_smallFrame.Image = this.trainedFaces[predictionResult.Label].Bitmap;
                        this.setPersonName = this.Names[predictionResult.Label].Replace(Environment.CurrentDirectory + "\\Image\\", "").Replace(".jpg", "");
                        CvInvoke.PutText(this.Frame, this.setPersonName, new Point(face.X - 2, face.Y - 2), FontFace.HersheyPlain, 1.0, new Bgr(Color.LimeGreen).MCvScalar);
                    }
                    else
                    {
                        CvInvoke.PutText(this.Frame, "Unknown", new Point(face.X - 2, face.Y - 2), FontFace.HersheyPlain, 1.0, new Bgr(Color.OrangeRed).MCvScalar);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in checkName: {ex.Message}");
            }
        }

        private void opencam_Click(object sender, EventArgs e)
        {
            openCamera(pCamera, pCaptured);
        }

        private void saveimg_Click(object sender, EventArgs e)
        {
            Save_IMAGE(Uname.Text);
        }

        private void detectimg_Click(object sender, EventArgs e)
        {
            detectFace();
        }
    }
}

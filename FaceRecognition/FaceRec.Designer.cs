
namespace FaceRecognition
{
    partial class FaceRec
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.pCamera = new System.Windows.Forms.PictureBox();
            this.pCaptured = new System.Windows.Forms.PictureBox();
            this.opencam = new System.Windows.Forms.Button();
            this.saveimg = new System.Windows.Forms.Button();
            this.detectimg = new System.Windows.Forms.Button();
            this.Uname = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pCamera)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pCaptured)).BeginInit();
            this.SuspendLayout();
            // 
            // pCamera
            // 
            this.pCamera.Location = new System.Drawing.Point(125, 68);
            this.pCamera.Name = "pCamera";
            this.pCamera.Size = new System.Drawing.Size(441, 301);
            this.pCamera.TabIndex = 0;
            this.pCamera.TabStop = false;
            // 
            // pCaptured
            // 
            this.pCaptured.Location = new System.Drawing.Point(638, 68);
            this.pCaptured.Name = "pCaptured";
            this.pCaptured.Size = new System.Drawing.Size(426, 301);
            this.pCaptured.TabIndex = 1;
            this.pCaptured.TabStop = false;
            // 
            // opencam
            // 
            this.opencam.Location = new System.Drawing.Point(125, 437);
            this.opencam.Name = "opencam";
            this.opencam.Size = new System.Drawing.Size(241, 83);
            this.opencam.TabIndex = 2;
            this.opencam.Text = "OPEN CAM";
            this.opencam.UseVisualStyleBackColor = true;
            this.opencam.Click += new System.EventHandler(this.opencam_Click);
            // 
            // saveimg
            // 
            this.saveimg.Location = new System.Drawing.Point(480, 437);
            this.saveimg.Name = "saveimg";
            this.saveimg.Size = new System.Drawing.Size(241, 83);
            this.saveimg.TabIndex = 3;
            this.saveimg.Text = "SAVE IMAGE";
            this.saveimg.UseVisualStyleBackColor = true;
            this.saveimg.Click += new System.EventHandler(this.saveimg_Click);
            // 
            // detectimg
            // 
            this.detectimg.Location = new System.Drawing.Point(823, 437);
            this.detectimg.Name = "detectimg";
            this.detectimg.Size = new System.Drawing.Size(241, 83);
            this.detectimg.TabIndex = 4;
            this.detectimg.Text = "DETECT IMAGE";
            this.detectimg.UseVisualStyleBackColor = true;
            this.detectimg.Click += new System.EventHandler(this.detectimg_Click);
            // 
            // Uname
            // 
            this.Uname.Location = new System.Drawing.Point(638, 388);
            this.Uname.Multiline = true;
            this.Uname.Name = "Uname";
            this.Uname.Size = new System.Drawing.Size(246, 29);
            this.Uname.TabIndex = 5;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(464, 388);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(113, 25);
            this.label1.TabIndex = 6;
            this.label1.Text = "Username :";
            // 
            // FaceRec
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1305, 652);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.Uname);
            this.Controls.Add(this.detectimg);
            this.Controls.Add(this.saveimg);
            this.Controls.Add(this.opencam);
            this.Controls.Add(this.pCaptured);
            this.Controls.Add(this.pCamera);
            this.Name = "FaceRec";
            this.Text = "FaceRecognition";
            ((System.ComponentModel.ISupportInitialize)(this.pCamera)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pCaptured)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pCamera;
        private System.Windows.Forms.PictureBox pCaptured;
        private System.Windows.Forms.Button opencam;
        private System.Windows.Forms.Button saveimg;
        private System.Windows.Forms.Button detectimg;
        private System.Windows.Forms.TextBox Uname;
        private System.Windows.Forms.Label label1;
    }
}


namespace AudioLevel
{
    partial class AudioLevelForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            captureLabel1 = new Label();
            renderLabel2 = new Label();
            progressBar1 = new ProgressBar();
            progressBar2 = new ProgressBar();
            pictureBox1 = new PictureBox();
            pictureBox2 = new PictureBox();
            renderComboBox = new ComboBox();
            contextMenuStrip1 = new ContextMenuStrip(components);
            TopView_ToolStripMenuItem = new ToolStripMenuItem();
            captureComboBox = new ComboBox();
            mute_CheckBox = new CheckBox();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).BeginInit();
            contextMenuStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // captureLabel1
            // 
            captureLabel1.AutoSize = true;
            captureLabel1.Location = new Point(49, 20);
            captureLabel1.Margin = new Padding(4, 0, 4, 0);
            captureLabel1.Name = "captureLabel1";
            captureLabel1.Size = new Size(54, 25);
            captureLabel1.TabIndex = 1;
            captureLabel1.Text = "Input";
            captureLabel1.MouseDown += MouseDown;
            // 
            // renderLabel2
            // 
            renderLabel2.AutoSize = true;
            renderLabel2.Location = new Point(49, 108);
            renderLabel2.Margin = new Padding(4, 0, 4, 0);
            renderLabel2.Name = "renderLabel2";
            renderLabel2.Size = new Size(69, 25);
            renderLabel2.TabIndex = 3;
            renderLabel2.Text = "Output";
            renderLabel2.MouseDown += MouseDown;
            // 
            // progressBar1
            // 
            progressBar1.Location = new Point(17, 50);
            progressBar1.Margin = new Padding(4, 5, 4, 5);
            progressBar1.Name = "progressBar1";
            progressBar1.Size = new Size(433, 38);
            progressBar1.TabIndex = 4;
            progressBar1.MouseDown += MouseDown;
            // 
            // progressBar2
            // 
            progressBar2.Location = new Point(17, 138);
            progressBar2.Margin = new Padding(4, 5, 4, 5);
            progressBar2.Name = "progressBar2";
            progressBar2.Size = new Size(433, 38);
            progressBar2.TabIndex = 4;
            progressBar2.MouseDown += MouseDown;
            // 
            // pictureBox1
            // 
            pictureBox1.Location = new Point(17, 18);
            pictureBox1.Margin = new Padding(4, 5, 4, 5);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(23, 27);
            pictureBox1.TabIndex = 5;
            pictureBox1.TabStop = false;
            pictureBox1.MouseDown += MouseDown;
            // 
            // pictureBox2
            // 
            pictureBox2.Location = new Point(17, 107);
            pictureBox2.Margin = new Padding(4, 5, 4, 5);
            pictureBox2.Name = "pictureBox2";
            pictureBox2.Size = new Size(23, 27);
            pictureBox2.TabIndex = 6;
            pictureBox2.TabStop = false;
            pictureBox2.MouseDown += MouseDown;
            // 
            // renderComboBox
            // 
            renderComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            renderComboBox.FormattingEnabled = true;
            renderComboBox.Location = new Point(49, 100);
            renderComboBox.Margin = new Padding(4, 5, 4, 5);
            renderComboBox.Name = "renderComboBox";
            renderComboBox.Size = new Size(401, 33);
            renderComboBox.TabIndex = 7;
            renderComboBox.SelectedIndexChanged += renderComboBox_SelectedIndexChanged;
            renderComboBox.MouseDown += MouseDown;
            // 
            // contextMenuStrip1
            // 
            contextMenuStrip1.ImageScalingSize = new Size(24, 24);
            contextMenuStrip1.Items.AddRange(new ToolStripItem[] { TopView_ToolStripMenuItem });
            contextMenuStrip1.Name = "contextMenuStrip1";
            contextMenuStrip1.Size = new Size(205, 36);
            contextMenuStrip1.MouseDown += MouseDown;
            // 
            // TopView_ToolStripMenuItem
            // 
            TopView_ToolStripMenuItem.Name = "TopView_ToolStripMenuItem";
            TopView_ToolStripMenuItem.Size = new Size(204, 32);
            TopView_ToolStripMenuItem.Text = "常にトップに表示";
            TopView_ToolStripMenuItem.Click += TopView_ToolStripMenuItem_Click;
            TopView_ToolStripMenuItem.MouseDown += MouseDown;
            // 
            // captureComboBox
            // 
            captureComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            captureComboBox.FormattingEnabled = true;
            captureComboBox.Location = new Point(49, 12);
            captureComboBox.Margin = new Padding(4, 5, 4, 5);
            captureComboBox.Name = "captureComboBox";
            captureComboBox.Size = new Size(360, 33);
            captureComboBox.TabIndex = 8;
            captureComboBox.SelectedIndexChanged += captureComboBox_SelectedIndexChanged;
            // 
            // mute_CheckBox
            // 
            mute_CheckBox.Appearance = Appearance.Button;
            mute_CheckBox.Location = new Point(416, 12);
            mute_CheckBox.Name = "mute_CheckBox";
            mute_CheckBox.Size = new Size(34, 30);
            mute_CheckBox.TabIndex = 9;
            mute_CheckBox.Text = "M";
            mute_CheckBox.UseVisualStyleBackColor = true;
            mute_CheckBox.Click += mute_CheckBox_Click;
            // 
            // AudioLevelForm
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(466, 187);
            Controls.Add(mute_CheckBox);
            Controls.Add(captureComboBox);
            Controls.Add(renderComboBox);
            Controls.Add(pictureBox2);
            Controls.Add(pictureBox1);
            Controls.Add(progressBar2);
            Controls.Add(progressBar1);
            Controls.Add(renderLabel2);
            Controls.Add(captureLabel1);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            Margin = new Padding(4, 5, 4, 5);
            MaximizeBox = false;
            MdiChildrenMinimizedAnchorBottom = false;
            MinimizeBox = false;
            Name = "AudioLevelForm";
            Text = "AudioLevel";
            FormClosing += Form1_FormClosing;
            Load += Form1_Load;
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).EndInit();
            contextMenuStrip1.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private Label captureLabel1;
        private Label renderLabel2;
        private ProgressBar progressBar1;
        private ProgressBar progressBar2;
        private PictureBox pictureBox1;
        private PictureBox pictureBox2;
        private ComboBox renderComboBox;
        private ContextMenuStrip contextMenuStrip1;
        private ToolStripMenuItem TopView_ToolStripMenuItem;
        private ComboBox captureComboBox;
        private CheckBox mute_CheckBox;
    }
}

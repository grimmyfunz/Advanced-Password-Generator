﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Media;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;
using System.Reflection;

namespace Passgen
{
    public partial class Form1 : Form
    {
        public string lowcharArr;
        public string uppcharArr;
        public string numericArr;
        public string basicSymbolArr;
        public string specialSymbolArr;
        public string customArr;
        public string fileArr;
        public string linkArr;
        public string rezArr;
        public string password;
        public string gen;
        public string login;
        public Random rand;
        string urlPattern;
        string emailPattern;
        public bool isPlaying;
        //public SoundPlayer simpleSound;
        public List<String> uniquePswd;
        public List<String> uniqueLogin;
        public SmtpClient SmtpServer;
        public Bitmap easyBMP;
        public Bitmap middleBMP;
        public Bitmap hardBMP;
        public Bitmap impossibleBMP;
        public Thread UrlReaderThread;
        private Object thisLock;
        //static EventWaitHandle _waitHandle = new AutoResetEvent(false);
        //static EventWaitHandle _manualWaitHandle = new ManualResetEvent(false);
        //static CountdownEvent _countEventObject = new CountdownEvent(3);
        static EventWaitHandle _difWaitHandle = new AutoResetEvent(false);

        private void Form1_Load(object sender, EventArgs e) //FORM INITIALIZE
        {
            //simpleSound = new SoundPlayer(@"D:\Downloads\427728__lemigoga__22-musica-ambiente.wav");
            //simpleSound.PlayLooping();
            isPlaying = true;
            lowcharArr = "qwertyuiopasdfghjklzxcvbnm";
            uppcharArr = "QWERTYUIOPASDFGHJKLZXCVBNM";
            numericArr = "1234567890";
            basicSymbolArr = "!@#$%^&*()_+-/*";
            specialSymbolArr = "★☆✡☮☸♈♉☪♊♋♌♍♎♏♐♑♒♓☤☥☧☨☩☫☬☭☯☽☾✙✚✛✜✝✞✟†⊹‡♁♆❖♅✠✢卍卐〷☠☢☣☦ϟ☀☁☂☃☄☉☼☽☾♁♨❄❅❆";
            customArr = "";
            fileArr = "";
            linkArr = "";
            rand = new Random();
            urlPattern = @"https?:\/\/(www\.)?[-a-zA-Z0-9@:%._\+~#=]{1,256}\.[a-zA-Z0-9()]{1,6}\b([-a-zA-Z0-9()@:%_\+.~#?&//=]*)"; // URL address pattern 
            emailPattern = @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$";
            uniqueLogin = new List<string>();
            uniquePswd = new List<string>();
            //USE YOUR OWN SMTP CREDENTIALS!!!
            SmtpServer = GetSMTP();
            //SmtpServer = new SmtpClient("mail.llu.lv");
            //SmtpServer.Port = 587;
            //SmtpServer.Credentials = new NetworkCredential("********", "********");
            SmtpServer.EnableSsl = true;
            easyBMP = new Bitmap(Properties.Resources._1);
            middleBMP = new Bitmap(Properties.Resources._2);
            hardBMP = new Bitmap(Properties.Resources._3);
            impossibleBMP = new Bitmap(Properties.Resources._4);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            thisLock = new Object();
            CheckForIllegalCrossThreadCalls = false;
            new Thread(UpdateDIF) { Priority = ThreadPriority.Lowest }.Start();
        }

        public Form1()
        {
            InitializeComponent();
        }

        public SmtpClient GetSMTP()
        {
            SmtpServer = new SmtpClient("smtp.gmail.com");
            SmtpServer.Port = 587;
            
            return SmtpServer;
        } //GETS SMTP CLIENT

        private void trackBar1_ValueChanged(object sender, EventArgs e) //LENGHT
        {
            label1.Text = "Length : " + trackBar1.Value;
            _difWaitHandle.Set();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e) //LETTERS a-z A-Z
        {
            if (!checkBox1.Checked) //DISABLE BOTH LOWER/UPPER CASE
            {
                checkBox4.Checked = false;
                checkBox5.Checked = false;
                checkBox4.Enabled = false;
                checkBox5.Enabled = false;
            }
            else
            {
                checkBox4.Checked = true;
                checkBox5.Checked = true;
                checkBox4.Enabled = true;
                checkBox5.Enabled = true;
            }
            _difWaitHandle.Set();
        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e) //LOWER
        {
            _difWaitHandle.Set();
        }

        private void checkBox5_CheckedChanged(object sender, EventArgs e) //UPPER
        {
            _difWaitHandle.Set();
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e) //NUMBERS 0-9
        {
            _difWaitHandle.Set();
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e) //SYMBOLS
        {
            if (!checkBox3.Checked) //DISABLE BOTH ASCII/SPECIAL SYMBOLS
            {
                checkBox10.Checked = false;
                checkBox11.Checked = false;
                checkBox10.Enabled = false;
                checkBox11.Enabled = false;
            }
            else
            {
                checkBox10.Checked = true;
                checkBox10.Enabled = true;
                checkBox11.Enabled = true;
            }
            _difWaitHandle.Set();
        }

        private void checkBox10_CheckedChanged(object sender, EventArgs e) //ASCII SYMBOLS
        {
            _difWaitHandle.Set();
        }

        private void checkBox11_CheckedChanged(object sender, EventArgs e) //SPECIAL SYMBOLS
        {
            _difWaitHandle.Set();
        }

        private void checkBox7_CheckedChanged(object sender, EventArgs e) //ADVANCED MODE
        {
            if (!checkBox7.Checked)
            {
                //BASIC ENABLED
                checkBox1.Enabled = true;
                checkBox2.Enabled = true;
                checkBox3.Enabled = true;
                checkBox4.Enabled = true;
                checkBox5.Enabled = true;
                //ADVANCED DISABLED
                checkBox6.Checked = false;
                checkBox8.Checked = false;
                checkBox9.Checked = false;
                checkBox6.Enabled = false;
                checkBox8.Enabled = false;
                checkBox9.Enabled = false;
                textBox4.Enabled = false;
                textBox2.Enabled = false;
                button2.Enabled = false;
                button3.Enabled = false;
            }
            else
            {
                //BASIC DISABLED
                checkBox1.Enabled = false;
                checkBox2.Enabled = false;
                checkBox3.Enabled = false;
                checkBox4.Enabled = false;
                checkBox5.Enabled = false;
                checkBox10.Enabled = false;
                checkBox11.Enabled = false;
                checkBox1.Checked = false;
                checkBox2.Checked = false;
                checkBox3.Checked = false;
                checkBox4.Checked = false;
                checkBox5.Checked = false;
                checkBox10.Checked = false;
                checkBox11.Checked = false;
                //ADVANCED ENABLED
                checkBox6.Enabled = true;
                checkBox8.Enabled = true;
                checkBox9.Enabled = true;
            }
            _difWaitHandle.Set();
            clearEP();
        }

        private void checkBox6_CheckedChanged(object sender, EventArgs e) //CUSTOM
        {
            if (!checkBox6.Checked)
            {
                textBox2.Enabled = false;
                textBox2.Text = "";
                checkBox8.Enabled = true;
                checkBox9.Enabled = true;
            }
            else
            {
                textBox2.Enabled = true;
                checkBox8.Enabled = false;
                checkBox9.Enabled = false;
                checkBox8.Checked = false;
                checkBox9.Checked = false;
            }
            _difWaitHandle.Set();
            clearEP();
        }

        private void textBox2_TextChanged(object sender, EventArgs e) //CUSTOM TEXT CHANGING
        {
            _difWaitHandle.Set();
            clearEP();
        }

        private void checkBox8_CheckedChanged(object sender, EventArgs e) //FILE
        {
            if (!checkBox8.Checked)
            {
                button2.Enabled = false;
                checkBox6.Enabled = true;
                checkBox9.Enabled = true;
            }
            else
            {
                button2.Enabled = true;
                checkBox6.Enabled = false;
                checkBox9.Enabled = false;
                checkBox6.Checked = false;
                checkBox9.Checked = false;
            }
            _difWaitHandle.Set();
            clearEP();
        }

        private void button2_Click(object sender, EventArgs e) //FILE OPEN BUTTON
        {
            OpenFileDialog theDialog = new OpenFileDialog();
            theDialog.Title = "Open Text File";
            theDialog.Filter = "TXT files|*.txt";
            theDialog.InitialDirectory = @"C:\";
            if (theDialog.ShowDialog() == DialogResult.OK)
            {
                label3.Text = theDialog.FileName;
                fileArr = "";
                using (StreamReader sr = new StreamReader(theDialog.FileName.ToString()))
                {
                    String line = sr.ReadToEnd();
                    fileArr += line;
                }
            }
            _difWaitHandle.Set();
            clearEP();
        }

        private void checkBox9_CheckedChanged(object sender, EventArgs e) //URL
        {
            if (!checkBox9.Checked)
            {
                textBox4.Enabled = false;
                button3.Enabled = false;
                checkBox6.Enabled = true;
                checkBox8.Enabled = true;
                clearEP();
            }
            else
            {
                textBox4.Enabled = true;
                button3.Enabled = true;
                checkBox6.Enabled = false;
                checkBox8.Enabled = false;
                checkBox6.Checked = false;
                checkBox8.Checked = false;
            }
            _difWaitHandle.Set();
        }

        private void button3_Click(object sender, EventArgs e) //Read URL
        {
            bool isUrlValid = Regex.IsMatch(textBox4.Text, urlPattern);
            if (isUrlValid)
            {
                UrlReaderThread = new Thread(ReadfromURL) { Priority = ThreadPriority.Highest };
                UrlReaderThread.Start();
                _difWaitHandle.Set();
                clearEP();
            }
            else
            {
                linkArr = "";
                MessageBox.Show("You need to enter valid URL.", "URL Invalid", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                errorProvider3.SetError(textBox4, "You need to enter valid URL");
            }
        }

        public void ReadfromURL() //URL Read function
        {
            lock (thisLock)
            {
                HttpWebRequest webRequest =
                (HttpWebRequest)HttpWebRequest.Create(textBox4.Text);
                webRequest.Method = "GET";

                HttpWebResponse webResponse = (HttpWebResponse)webRequest.GetResponse();
                StreamReader webSource = new StreamReader(webResponse.GetResponseStream());
                string source = webSource.ReadToEnd();
                webResponse.Close();
                linkArr = source;
                //return source;
            }
        }

        private void musicToolStripMenuItem_Click(object sender, EventArgs e) //Music Toggler
        {
            musicToolStripMenuItem.Checked = !musicToolStripMenuItem.Checked;
            if (musicToolStripMenuItem.Checked)
            {
                //simpleSound.PlayLooping();
            }
            else
            {
                //simpleSound.Stop();
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e) //Exit
        {
            Environment.Exit(Environment.ExitCode);
        }

        private void checkBox12_CheckedChanged(object sender, EventArgs e) //BULK GENERATE
        {
            if (!checkBox12.Checked)
            {
                checkBox13.Checked = false;
                checkBox14.Checked = false;
                checkBox13.Enabled = false;
                checkBox14.Enabled = false;
                numericUpDown1.Value = 1;
                numericUpDown1.Enabled = false;
            }
            else
            {
                checkBox13.Enabled = true;
                checkBox14.Enabled = true;
                numericUpDown1.Enabled = true;
            }
        }

        private void checkBox15_CheckedChanged(object sender, EventArgs e) //EMAIL TOGGLER
        {
            if (!checkBox15.Checked)
            {
                textBox3.Enabled = false;
                textBox3.Text = "";
            }
            else
            {
                textBox3.Enabled = true;
            }
        }

        private void button1_Click(object sender, EventArgs e) //GENERATE BUTTON
        {
            GenerateButton();
        }

        public void GenerateButton()
        {
            errorProvider4.Clear();
            password = Generate();
            if (password != "ERROR")
            {
                if (checkBox12.Checked) //BULK
                {
                    Bulk();
                }
                else //BASIC
                {
                    Clipboard.SetText(password);
                    textBox1.Text = password;
                    if (checkBox15.Checked)
                    {
                        bool isEmailValid = Regex.IsMatch(textBox3.Text, emailPattern);
                        if (isEmailValid)
                        {
                            new Thread(() => sendEmail(textBox3.Text, "Password", $"Your password is : {password}")) {Priority = ThreadPriority.Highest }.Start();
                            MessageBox.Show("Password generated, coppied to clipboard and email is sent!", "Success!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            MessageBox.Show("You need to enter valid EMAIL.", "Email Invalid", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            errorProvider4.SetError(textBox3, "You need to enter valid EMAIL");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Password generated and coppied to clipboard!", "Success!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            else
            {
                if (checkBox7.Checked)
                {
                    if (checkBox6.Checked) //CUSTOM
                    {
                        MessageBox.Show("You need to fill custom field with valid symbols.", "Invalid Symbols", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        errorProvider1.SetError(textBox2, "You need to fill custom field with valid symbols");
                    }
                    else if (checkBox8.Checked) //FROM FILE
                    {
                        MessageBox.Show("You need to open valid file.", "File Invalid", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        errorProvider2.SetError(label3, "You need to fill custom field with valid symbols");
                    }
                    else if (checkBox9.Checked) //LINK
                    {
                        MessageBox.Show("You need to read valid URL.", "URL Invalid", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        errorProvider3.SetError(textBox4, "You need to read valid URL");
                    }
                    else
                    {
                        MessageBox.Show("You need check at least one option.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                else
                {
                    MessageBox.Show("You need check at least one option.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        } //GENERATION

        public void Bulk()
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "Text File|*.txt";
            sfd.FileName = "Bulk";
            sfd.Title = "Save Text File";
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                string path = sfd.FileName;
                StreamWriter bw = new StreamWriter(File.Create(path));
                for (int i = 0; i < numericUpDown1.Value; i++)
                {
                    if (checkBox13.Checked)
                    {
                        login = GenerateLogin();
                        if (checkBox14.Checked)
                        {
                            while (uniqueLogin.Exists(x => x == login))
                            {
                                login = GenerateLogin();
                            }
                            uniqueLogin.Add(login);
                        }
                        bw.Write($"{login} : ");
                    }

                    password = Generate();
                    if (checkBox14.Checked)
                    {
                        while (uniquePswd.Exists(x => x == password))
                        {
                            password = Generate();
                        }
                        uniquePswd.Add(password);
                    }
                    bw.WriteLine(password);
                }
                bw.Dispose();
                uniquePswd.Clear();
                uniqueLogin.Clear();

                if (checkBox15.Checked)
                {
                    bool isEmailValid = Regex.IsMatch(textBox3.Text, emailPattern);
                    if (isEmailValid)
                    {
                        new Thread(() => sendEmail(textBox3.Text, "Bulk Passwords", "Your generated passwords is added as attachment.", path)).Start();
                        if (checkBox13.Checked)
                        {
                            MessageBox.Show("Passwords and logins are generated, and email is sent!", "Success!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            MessageBox.Show("Passwords are generated and email is sent!", "Success!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                    else
                    {
                        MessageBox.Show("You need to enter valid EMAIL.", "Email Invalid", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        errorProvider4.SetError(textBox3, "You need to enter valid EMAIL");
                    }
                }
            }
        } //BULK GENERATION

        public string Generate() //GENERATION FUNCTION
        {
            rezArr = "";
            gen = "";

            if (checkBox7.Checked)
            {
                if (checkBox6.Checked) //CUSTOM
                {
                    rezArr = textBox2.Text;
                }
                else if (checkBox8.Checked) //FROM FILE
                {
                    rezArr = fileArr;
                }
                else if (checkBox9.Checked) //LINK
                {
                    UrlReaderThread.Join();
                    rezArr = linkArr;
                }
            }
            else
            {
                if (checkBox4.Checked) //LOWERCASE
                {
                    rezArr += lowcharArr;
                }
                if (checkBox5.Checked) //UPPERCASE
                {
                    rezArr += uppcharArr;
                }
                if (checkBox2.Checked) //NUMERIC
                {
                    rezArr += numericArr;
                }
                if (checkBox10.Checked) //ASCII
                {
                    rezArr += basicSymbolArr;
                }
                if (checkBox11.Checked) //SPECIAL SYMBOLS
                {
                    rezArr += specialSymbolArr;
                }
            }

            if (rezArr.Length != 0)
            {
                for (int i = 0; i < trackBar1.Value; i++)
                {
                    gen += rezArr[rand.Next(0, rezArr.Length)];
                }
                return gen;
            }
            else
            {
                return "ERROR";
            }
        }

        public string GenerateLogin() //LOGIN GENERATOR
        {
            string loginGen = GenerateName(rand.Next(4, 7));
            loginGen += rand.Next(0, 9999);
            return loginGen;
        }

        public string GenerateName(int len) //NAME GENERATOR (USED BY LOGIN GENERATOR)
        {
            string[] consonants = { "b", "c", "d", "f", "g", "h", "j", "k", "l", "m", "l", "n", "p", "q", "r", "s", "sh", "zh", "t", "v", "w", "x" };
            string[] vowels = { "a", "e", "i", "o", "u", "ae", "y" };
            string Name = "";
            Name += consonants[rand.Next(consonants.Length)].ToUpper();
            Name += vowels[rand.Next(vowels.Length)];
            int b = 2; //b tells how many times a new letter has been added. It's 2 right now because the first two letters are already in the name.
            while (b < len)
            {
                Name += consonants[rand.Next(consonants.Length)];
                b++;
                Name += vowels[rand.Next(vowels.Length)];
                b++;
            }
            return Name;
        }

        public void UpdateDIF() //UPDATE PROGRESSBAR / IMAGE
        {
            while (true)
            {
                _difWaitHandle.WaitOne();

                double k = 0;
                if (checkBox7.Checked)
                {
                    if (checkBox6.Checked) //CUSTOM
                    {
                        k = textBox2.Text.Length != 0 ? textBox2.Text.Length * 0.25 : 0;
                    }
                    else if (checkBox8.Checked) //FROM FILE
                    {
                        k = 4;
                    }
                    else if (checkBox9.Checked) //LINK
                    {
                        k = 6;
                    }
                }
                else
                {
                    if (checkBox4.Checked) //LOWERCASE
                    {
                        k += 0.5;
                    }
                    if (checkBox5.Checked) //UPPERCASE
                    {
                        k += 0.5;
                    }
                    if (checkBox2.Checked) //NUMERIC
                    {
                        k += 0.5;
                    }
                    if (checkBox10.Checked) //ASCII
                    {
                        k += 1;
                    }
                    if (checkBox11.Checked) //SPECIAL SYMBOLS
                    {
                        k += 3;
                    }
                }
                int rating = (int)(k * trackBar1.Value);
                if (rating >= 100)
                {
                    progressBar1.Value = 100;
                }
                else
                {
                    progressBar1.Value = rating;
                }

                if (rating < 30)
                {
                    pictureBox1.Image = easyBMP;
                }
                else if (rating < 60)
                {
                    pictureBox1.Image = middleBMP;
                }
                else if (rating < 90)
                {
                    pictureBox1.Image = hardBMP;
                }
                else if (rating >= 90)
                {
                    pictureBox1.Image = impossibleBMP;
                }
            }
        }

        private void sendEmail(string toAdress, string subject, string body, string attachmentFile)
        {
            try
            {
                MailMessage mail = new MailMessage();
                mail.From = new MailAddress("password@gmail.com");
                mail.To.Add(toAdress);
                mail.Subject = subject;
                mail.Body = body;

                Attachment attachment;
                attachment = new Attachment(attachmentFile);
                mail.Attachments.Add(attachment);

                SmtpServer.Send(mail);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        } //EMAIL SENDER WITH ATTACHMENT

        private void sendEmail(string toAdress, string subject, string body)
        {
            try
            {
                MailMessage mail = new MailMessage();
                mail.From = new MailAddress("password@gmail.com");
                mail.To.Add(toAdress);
                mail.Subject = subject;
                mail.Body = body;

                SmtpServer.Send(mail);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        } //EMAIL SENDER

        private void clearEP()
        {
            errorProvider1.Clear();
            errorProvider2.Clear();
            errorProvider3.Clear();
        } //CLEARS ERROR PROVIDERS
    }
}

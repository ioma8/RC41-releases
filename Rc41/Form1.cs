using Microsoft.VisualBasic.ApplicationServices;
using System.ComponentModel.Design.Serialization;
using System.Drawing;
using System.Windows.Forms.VisualStyles;
using System.Xml.Serialization;
using static System.Runtime.CompilerServices.RuntimeHelpers;
using Rc41.T_TapeDrive;
using Rc41.T_Printer;
using Rc41.T_CardReader;
using Rc41.T_Cpu;
using Rc41.T_Extended;
using Rc41.T_TimeModule;
using Rc41.Core.Interfaces;


namespace Rc41
{
    public partial class Form1 : Form, ICalculatorUI
    {
        public Cpu cpu;
        public Debugger debugger;
        public Ui ui;
        PictureBox[] cells;
        PictureBox[] semis;
        protected int displayStart;
        protected int displayEnd;
        protected string display;
        protected Boolean scroll;
        Bitmap PrinterPage;
        protected int[,] chars = new int[,] {
           { 8, 28, 62, 28, 8 },           // 0
           { 0, 20, 8, 20, 0 },            // 1
           { 69, 41, 17, 41, 69 },         // 2
           { 8, 28, 42, 8, 8 },            // 3 Left arrow
           { 56, 68, 68, 56, 68 },         // 4 Alpha
           { 126, 21, 37,37, 26 },         // 5 Beta
           { 63, 1, 1, 1, 3 },             // 6 Gamma
           { 16, 32, 127, 32, 16 },        // 7 Down arrow
           { 96, 88, 78, 88, 96 },         // 8 Delta
           { 56, 68, 68, 60, 4 },          // 9
           { 8, 28, 62, 28, 8 },           // 10 Diamond
           { 98, 20, 8, 16, 96 },          // 11
           { 64, 60, 32, 32, 28 },         // 12 Micro
           { 96, 80, 88, 100, 66  },       // 13
           { 0, 16, 120, 8, 4 },           // 14
           { 8, 85, 119, 85, 8 },          // 15
           { 62, 73, 73, 73, 62 },         // 16
           { 94, 97, 1, 97, 94 },          // 17 Omega
           { 48, 74, 77, 73, 48 },         // 18
           { 120, 20, 21, 20, 120 },       // 19
           { 56, 68, 69, 60, 64 },         // 20
           { 120, 21, 20, 21, 120 },       // 21
           { 56, 69, 68, 61, 64 },         // 22
           { 60, 67, 66, 67, 60 },         // 23
           { 56, 69, 68, 69, 56 },         // 24
           { 62, 65, 64, 65, 62 },         // 25
           { 60, 65, 64, 65, 60 },         // 26
           { 126, 26, 127, 73, 73 },       // 27
           { 56, 68, 56, 92, 88 },         // 28
           { 20, 52, 28, 22, 20 },         // 29
           { 72, 126, 73, 65, 34 },        // 30
           { 85, 42, 85, 42, 85 },         // 31
           { 0, 0, 0, 0, 0 },              // 32 Space
           { 0, 0, 95, 0, 0 },             // 33 !
           { 0, 3, 0, 3, 0 },              // 34 "
           { 20, 127, 20, 127, 20 },       // 35 #
           { 36, 42, 127, 42, 18 },        // 36 $
           { 35, 19, 8, 100, 98 },         // 37 %
           { 54, 73, 86, 32, 80 },         // 38 &
           { 0, 0, 3, 0, 0 },              // 39 '
           { 0, 28, 34, 65, 0 },           // 40 (
           { 0, 65, 34, 28, 0 },           // 41 )
           { 20, 28, 62, 28, 20 },         // 42 *
           { 8, 8, 62, 8, 8 },             // 43 +
           { 0, 64, 48, 0, 0 },            // 44 ,
           { 8, 8, 8, 8, 8 },              // 45 -
           { 0, 96, 96, 0, 0 },            // 46 .
           { 32, 16, 8, 4, 2 },            // 47 /
           { 62, 81, 73, 69, 62 },         // 48 0
           { 0, 66, 127, 64, 0 },          // 49 1
           { 98, 81, 73, 73, 70 },         // 50 2
           { 33, 65, 73, 77, 51 },         // 51 3
           { 24, 20, 18, 127, 16 },        // 52 4
           { 39, 69, 69, 69, 57 },         // 53 5
           { 60, 74, 73, 73, 48 },         // 54 6
           { 1, 113, 9, 5, 3 },            // 55 7
           { 54, 73, 73, 73, 54 },         // 56 8
           { 6, 73, 73, 41, 30 },          // 57 9
           { 0, 0, 36, 0, 0 },             // 58 :
           { 0, 64, 52, 0, 0 },            // 59 ;
           { 8, 20, 34, 65, 0 },           // 60 <
           { 20, 20, 20, 20, 20 },         // 61 =
           { 0, 65, 34, 20, 8 },           // 62 >
           { 2, 1, 81, 9, 6 },             // 63 ?
           { 62, 65, 93, 85, 30 },         // 64 @
           { 126, 17, 17, 17, 126 },       // 65 A
           { 127, 73, 73, 73, 54 },        // 66 B
           { 62, 65, 65, 65, 34 },         // 67 C
           { 65, 127, 65, 65, 62 },        // 68 D
           { 127, 73, 73, 73, 65 },        // 69 E
           { 127, 9, 9, 9, 1 },            // 70 F
           { 62, 65, 65, 81, 114 },        // 71 G
           { 127, 8, 8, 8, 127 },          // 72 H
           { 0, 65, 127, 65, 0 },          // 73 I
           { 32, 64, 64, 64, 63 },         // 74 J
           { 127, 8, 20, 34, 65 },         // 75 K
           { 127, 64, 64, 64, 64 },        // 76 L
           { 127, 2, 12, 2, 127 },         // 77 M
           { 127, 4, 8, 16, 127 },         // 78 N
           { 62, 65, 65, 65, 62 },         // 79 O
           { 127, 9, 9, 9, 6 },            // 80 P
           { 62, 65, 81, 33, 94 },         // 81 Q
           { 127, 25, 25, 41, 70 },        // 82 R
           { 38, 73, 73, 73, 50 },         // 83 S
           { 1, 1, 127, 1, 1 },            // 84 T
           { 63, 64, 64, 64, 63 },         // 85 U
           { 7, 24, 96, 24, 7 },           // 86 V
           { 127, 32, 24, 32, 127 },       // 87 W
           { 99, 20, 8, 20, 99 },          // 88 X
           { 3, 4, 120, 4, 3 },            // 89 Y
           { 97, 81, 73, 69, 67 },         // 90 Z
           { 0, 127, 65, 65, 0 },          // 91 [
           { 2, 4, 8, 16, 32 },            // 92 \
           { 0, 65, 65, 127, 0 },          // 93 ]
           { 4, 2, 127, 2, 4 },            // 94 Up arrow
           { 64, 64, 64, 64, 64 },         // 95 _
           { 0, 1, 7, 1, 0 },              // 96 tiny T
           { 32, 84, 84, 84, 120 },        // 97 a
           { 127, 72, 68, 68, 56 },        // 98 b
           { 56, 68, 68, 68, 32 },         // 99 c
           { 56, 68, 68, 72, 127 },        // 100 d
           { 56, 84, 84, 84, 8 },          // 101 e
           { 8, 126, 9, 2, 0 },            // 102 f
           { 8, 20, 84, 84, 60 },          // 103 g
           { 127, 8, 4, 4, 120 },          // 104 h
           { 0, 68, 125, 64, 0 },          // 105 i
           { 32, 64, 64, 61, 0 },          // 106 j
           { 127, 24, 40, 68, 0 },         // 107 k
           { 0, 65, 127, 64, 0 },          // 108 l
           { 120, 4, 24, 4, 120 },         // 109 m
           { 124, 8, 4, 4, 120 },          // 110 n
           { 56, 68, 68, 68, 56 },         // 111 o
           { 124, 20, 36, 36, 24 },        // 112 p
           { 24, 36, 52, 124, 64 },        // 113 q
           { 124, 8, 4, 4, 8 },            // 114 r
           { 72, 84, 84, 84, 32 },         // 115 s
           { 4, 62, 68, 32, 0 },           // 116 t
           { 60, 64, 64, 32, 124 },        // 117 u
           { 28, 32, 64, 32, 28 },         // 118 v
           { 60, 64, 48, 64, 60 },         // 119 w
           { 68, 40, 16, 40, 68 },         // 120 x
           { 4, 72, 48, 8, 4 },            // 121 y
           { 68, 100, 84, 76, 68 },        // 122 z
           { 8, 120, 8, 120, 4 },          // 123 pi
           { 96, 80, 88, 100, 64 },        // 124
           { 8, 8, 42, 28, 8 },            // 125 right arrow
           { 99, 85, 73, 65, 99 },         // 126 summation
           { 127, 8, 8, 8, 8 },            // 127 append
        };
        string[] printerLines;
        char[] lineJustify;


        public Form1()
        {
            int x;
            InitializeComponent();
            display = "";
            //            B_SumPlus.Text = "\u2211+";
            //            SumMinus.Text = "\u2211-";
            //            ClearSum.Text = "CL\u2211";
            this.Width = 370;
            cells = new PictureBox[12];
            cells[0] = Cell1;
            cells[1] = Cell2;
            cells[2] = Cell3;
            cells[3] = Cell4;
            cells[4] = Cell5;
            cells[5] = Cell6;
            cells[6] = Cell7;
            cells[7] = Cell8;
            cells[8] = Cell9;
            cells[9] = Cell10;
            cells[10] = Cell11;
            cells[11] = Cell12;
            semis = new PictureBox[12];
            semis[0] = Semi1;
            semis[1] = Semi2;
            semis[2] = Semi3;
            semis[3] = Semi4;
            semis[4] = Semi5;
            semis[5] = Semi6;
            semis[6] = Semi7;
            semis[7] = Semi8;
            semis[8] = Semi9;
            semis[9] = Semi10;
            semis[10] = Semi11;
            semis[11] = Semi12;
            x = 15;
            for (var i = 0; i < 12; i++)
            {
                cells[i].Left = x;
                cells[i].Image = imageList1.Images[2];
                x += 16;
                semis[i].Left = x;
                semis[i].Image = imageList2.Images[3];
                x += 8;
            }
            cpu = new Cpu(this);
            cpu.Annunciators();
//            cpu.printer = new Printer(cpu, this);
//            cpu.tapeDrive = new TapeDrive(cpu, this);
//            cpu.cardReader = new CardReader(cpu, this);
//            cpu.timeDate = new TimeDate(cpu, this);
            ui = new Ui(cpu, this);
            cpu.OpenTapeDrive("tape1.dat");
            debugger = new Debugger(this, cpu);
            PrinterPage = new Bitmap(340, 460);
            GraphicPrinter.Image = PrinterPage;
            var gc = Graphics.FromImage(PrinterPage);
            var brush = new SolidBrush(Color.White);
            gc.FillRectangle(brush, 0, 0, 340, 460);
            brush.Dispose();
            gc.Dispose();
            printerLines = new string[250];
            lineJustify = new char[250];
            for (var i = 0; i < 250; i++)
            {
                printerLines[i] = "";
                lineJustify[i] = 'L';
            }


        }

        private void ButtonClick(object sender, EventArgs e)
        {
            if (((Button)sender).Tag.Equals("<prgm>")) ui.Key_Prgm();
            if (((Button)sender).Tag.Equals("<user>")) ui.Key_User();
            if (((Button)sender).Tag.Equals("<on>"))
            {
                if (cpu.calculatorMode == Cpu.CM_SW)
                {
                    cpu.ClearFlag(50);
                    cpu.timeModule.EndSw();
                }
                if (cpu.calculatorMode == Cpu.CM_CLOCK)
                {
                    cpu.ClearFlag(50);
                    cpu.timeModule.EndClock();
                }
                cpu.Save();
                Application.Exit();
            }
        }

        protected void PrintAt(string line, char justify, int lineNo, Graphics gc, Brush black, Brush white)
        {
            int ch;
            int pos;
            int col = 3;
            pos = lineNo * 18 - 10;
            if (justify == 'R')
            {
                col = 168 - cpu.printer.BufferLength(line);
                col *= 2;
                col += 3;
            }
            gc.FillRectangle(white, 0, pos, 340, 20);
            for (var i = 0; i < line.Length; i++)
            {
                ch = line[i];
                if (ch >= 0x80)
                {
                    if ((ch & 1) == 1) gc.FillRectangle(black, col, pos + 0, 2, 2);
                    if ((ch & 2) == 2) gc.FillRectangle(black, col, pos + 2, 2, 2);
                    if ((ch & 4) == 4) gc.FillRectangle(black, col, pos + 4, 2, 2);
                    if ((ch & 8) == 8) gc.FillRectangle(black, col, pos + 6, 2, 2);
                    if ((ch & 16) == 16) gc.FillRectangle(black, col, pos + 8, 2, 2);
                    if ((ch & 32) == 32) gc.FillRectangle(black, col, pos + 10, 2, 2);
                    if ((ch & 64) == 64) gc.FillRectangle(black, col, pos + 12, 2, 2);
                    col += 2;
                }
                else
                {
                    if (ch >= (chars.Length / 5)) ch = (chars.Length / 5) - 1;
                    col += 2;
                    for (var j = 0; j < 5; j++)
                    {
                        if ((chars[ch, j] & 1) == 1) gc.FillRectangle(black, col, pos + 0, 2, 2);
                        if ((chars[ch, j] & 2) == 2) gc.FillRectangle(black, col, pos + 2, 2, 2);
                        if ((chars[ch, j] & 4) == 4) gc.FillRectangle(black, col, pos + 4, 2, 2);
                        if ((chars[ch, j] & 8) == 8) gc.FillRectangle(black, col, pos + 6, 2, 2);
                        if ((chars[ch, j] & 16) == 16) gc.FillRectangle(black, col, pos + 8, 2, 2);
                        if ((chars[ch, j] & 32) == 32) gc.FillRectangle(black, col, pos + 10, 2, 2);
                        if ((chars[ch, j] & 64) == 64) gc.FillRectangle(black, col, pos + 12, 2, 2);
                        col += 2;
                    }
                    col += 2;
                }
            }

        }

        public void Print(string line, char justify)
        {
            var gc = Graphics.FromImage(PrinterPage);
            var black = new SolidBrush(Color.Black);
            var white = new SolidBrush(Color.White);
            if (GraphicsPrinterScroll.Value < 235)
            {
                GraphicsPrinterScroll.Value = 235;
            }
            Rectangle bounds = new Rectangle(0, 26, 340, 460 - 26);
            Bitmap current = PrinterPage.Clone(bounds, PrinterPage.PixelFormat);
            gc.DrawImage(current, 0, 8);
            current.Dispose();
            PrintAt(line, justify, 25, gc, black, white);
            black.Dispose();
            white.Dispose();
            gc.Dispose();
            GraphicPrinter.Image = PrinterPage;
            for (int i = 0; i < 249; i++)
            {
                printerLines[i] = printerLines[i + 1];
                lineJustify[i] = lineJustify[i + 1];
                printerLines[249] = line;
                lineJustify[249] = justify;
            }

        }

        public void Alpha(bool b)
        {
            A_Alpha.Visible = b;
        }

        public void Flag_0(bool b)
        {
            A_0.Visible = b;
        }

        public void Flag_1(bool b)
        {
            A_1.Visible = b;
        }

        public void Flag_2(bool b)
        {
            A_2.Visible = b;
        }

        public void Flag_3(bool b)
        {
            A_3.Visible = b;
        }

        public void Flag_4(bool b)
        {
            A_4.Visible = b;
        }

        public void G(bool b)
        {
            A_G.Visible = b;
        }

        public void Prog(bool b)
        {
            A_Prog.Visible = b;
        }

        public void Rad(bool b)
        {
            A_Rad.Visible = b;
        }

        public void Shift(bool b)
        {
            A_Shift.Visible = b;
        }

        public void User(bool b)
        {
            A_User.Visible = b;
            if (b) EnterUser();
            else ExitUser();
        }

        protected void UpdateDisplay()
        {
            int p;
            char c;
            displayEnd = displayStart;
            p = 0;
            if (display == null) display = "";
            while (p < 24 && displayEnd < display.Length)
            {
                c = display[displayEnd++];
                if (c > 129) c = (char)2;
                if (c == '.' || c == ',' || c == ':' || c == ';')
                {
                    if ((p & 1) == 0)
                    {
                        cells[p / 2].Image = imageList1.Images[32];
                        p++;
                    }
                    if (p < 24)
                    {
                        if (c == '.') semis[p / 2].Image = imageList2.Images[2];
                        if (c == ',') semis[p / 2].Image = imageList2.Images[1];
                        if (c == ':') semis[p / 2].Image = imageList2.Images[3];
                        if (c == ';') semis[p / 2].Image = imageList2.Images[4];
                    }
                }
                else
                {
                    if ((p & 1) == 1)
                    {
                        semis[p / 2].Image = imageList2.Images[0];
                        p++;
                    }
                    if (p < 24)
                    {
                        if (c == '"') cells[p / 2].Image = imageList1.Images[0x7a];
                        else cells[p / 2].Image = imageList1.Images[c];
                    }
                    else displayEnd--;
                }
                p++;
            }
            while (p < 24)
            {
                if ((p & 1) == 0) cells[p / 2].Image = imageList1.Images[32];
                if ((p & 1) == 1) semis[p / 2].Image = imageList2.Images[0];
                p++;
            }
            scroll = (displayEnd < display.Length) ? true : false;
            if (scroll) scrollTimer.Enabled = true; else scrollTimer.Enabled = false;
            Application.DoEvents();
        }

        public void Display(string msg, Boolean sc)
        {
            scroll = false;
            display = msg;
            displayStart = 0;
            UpdateDisplay();
            while (!sc && scroll)
            {
                displayStart++;
                UpdateDisplay();
            }
        }

        private void b_Alpha_Click(object sender, EventArgs e)
        {
            //           cpu.KeyAlpha();
            ui.Key_Alpha();
        }

        private void ButtonDown(object sender, MouseEventArgs e)
        {
            ui.ButtonDown((string)((Button)sender).Tag);
        }

        private void ButtonUp(object sender, MouseEventArgs e)
        {
            ui.ButtonUp((string)((Button)sender).Tag);
        }

        private void b_ShiftClick(object sender, EventArgs e)
        {
            ui.Key_Shift();
        }

        private void PrintButton_Click(object sender, EventArgs e)
        {
            cpu.Print();
        }

        private void AdvButton_Click(object sender, EventArgs e)
        {
            cpu.Adv();
        }

        private void b_Peripherals_Click(object sender, EventArgs e)
        {
            if (b_Peripherals.Text.Equals("-->"))
            {
                this.Width = 770;
                b_Peripherals.Text = "<--";
            }
            else
            {
                this.Width = 370;
                b_Peripherals.Text = "-->";
            }
        }

        private void DisplayTimer_Tick(object sender, EventArgs e)
        {
            cpu.Tick();
        }

        public bool PrintToFile()
        {
            return cb_PrintToFile.Checked;
        }

        public bool DisplayTimerEnabled()
        {
            return DisplayTimer.Enabled;
        }

        public void DisplayTimerEnabled(bool b)
        {
            DisplayTimer.Enabled = b;
        }

        public void SetDisplayTimerInterval(int intervalMs)
        {
            DisplayTimer.Interval = intervalMs;
        }

        public char PrinterMode()
        {
            if (pm_Man.Checked) return 'M';
            if (pm_Trace.Checked) return 'T';
            if (pm_Norm.Checked) return 'N';
            return ' ';
        }

        public bool PrinterOn()
        {
            return PrinterPowerOn.Checked;
        }

        public void ToPrinter(string line, char justify)
        {
            cpu.printer.Print(line, justify);
        }

        private void DebugButton_Click(object sender, EventArgs e)
        {
            this.Width = 1286;
        }

        private void StackButton_Click(object sender, EventArgs e)
        {
            debugger.ShowStatRegs(true);
        }

        public void DebugPrint(string line)
        {
            DebugOutput.AppendText(line + "\r\n");
        }

        private void StatButton_Click(object sender, EventArgs e)
        {
            debugger.ShowStatRegs(false);
        }

        private void FlagsButton_Click(object sender, EventArgs e)
        {
            debugger.ShowFlags();
        }

        private void InfoButton_Click(object sender, EventArgs e)
        {
            debugger.ShowInfo();
        }

        private void DregsButton_Click(object sender, EventArgs e)
        {
            debugger.ShowDregs(Convert.ToInt32(DregsFrom.Text), Convert.ToInt32(DregsTo.Text));
        }

        public int FromHex(string hex)
        {
            int ret;
            ret = 0;
            foreach (char c in hex)
            {
                if (c >= '0' && c <= '9') ret = (ret * 16) + (c - '0');
                if (c >= 'A' && c <= 'F') ret = (ret * 16) + (c - 55);
                if (c >= 'a' && c <= 'f') ret = (ret * 16) + (c - 87);
            }
            return ret;
        }

        private void RegsButton_Click(object sender, EventArgs e)
        {
            debugger.ShowRegs(FromHex(RegsFrom.Text), FromHex(RegsTo.Text));
        }

        private void RunTimer_Tick(object sender, EventArgs e)
        {
            cpu.RunTick();
        }

        public bool Fast()
        {
            return cb_Fast.Checked;
        }

        public void RunTimerEnabled(bool b)
        {
            RunTimer.Enabled = b;
        }

        private void RamClearButton_Click(object sender, EventArgs e)
        {
            if (AllowClear.Checked) cpu.RamClear();
            AllowClear.Checked = false;
        }

        private void ResetButton_Click(object sender, EventArgs e)
        {
            cpu.Reset();
        }

        private void ByteJumper_Click(object sender, EventArgs e)
        {
            SynthDialog dialog = new SynthDialog();
            var result = dialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                if (dialog.ByteJumper() != 0) cpu.AssignKey(0xf1, 0x41, dialog.ByteJumper());    // Byte Jumper
                if (dialog.QLoader() != 0) cpu.AssignKey(0x04, 0x19, dialog.QLoader());          // Q Loader
                if (dialog.RclB() != 0) cpu.AssignKey(0x90, 0x7c, dialog.RclB());                // RCL b
                if (dialog.RclC() != 0) cpu.AssignKey(0x90, 0x7d, dialog.RclC());                // RCL c
                if (dialog.RclD() != 0) cpu.AssignKey(0x90, 0x7e, dialog.RclD());                // RCL d
                if (dialog.RclE() != 0) cpu.AssignKey(0x90, 0x7f, dialog.RclE());                // RCL e
                if (dialog.RclM() != 0) cpu.AssignKey(0x90, 0x75, dialog.RclM());                // RCL M
                if (dialog.RclN() != 0) cpu.AssignKey(0x90, 0x76, dialog.RclN());                // RCL N
                if (dialog.RclO() != 0) cpu.AssignKey(0x90, 0x77, dialog.RclO());                // RCL O
                if (dialog.RclP() != 0) cpu.AssignKey(0x90, 0x78, dialog.RclP());                // RCL P
                if (dialog.RclQ() != 0) cpu.AssignKey(0x90, 0x79, dialog.RclQ());                // RCL Q
                if (dialog.StoB() != 0) cpu.AssignKey(0x91, 0x7c, dialog.StoB());                // Sto b
                if (dialog.StoC() != 0) cpu.AssignKey(0x91, 0x7d, dialog.StoC());                // Sto c
                if (dialog.StoD() != 0) cpu.AssignKey(0x91, 0x7e, dialog.StoD());                // Sto d
                if (dialog.StoE() != 0) cpu.AssignKey(0x91, 0x7f, dialog.StoE());                // Sto e
                if (dialog.StoM() != 0) cpu.AssignKey(0x91, 0x75, dialog.StoM());                // Sto M
                if (dialog.StoN() != 0) cpu.AssignKey(0x91, 0x76, dialog.StoN());                // Sto N
                if (dialog.StoO() != 0) cpu.AssignKey(0x91, 0x77, dialog.StoO());                // Sto O
                if (dialog.StoP() != 0) cpu.AssignKey(0x91, 0x78, dialog.StoP());                // Sto P
                if (dialog.StoQ() != 0) cpu.AssignKey(0x91, 0x79, dialog.StoQ());                // Sto Q
                if (dialog.XexB() != 0) cpu.AssignKey(0xce, 0x7c, dialog.XexB());                // X<> b
                if (dialog.XexC() != 0) cpu.AssignKey(0xce, 0x7d, dialog.XexC());                // X<> c
                if (dialog.XexD() != 0) cpu.AssignKey(0xce, 0x7e, dialog.XexD());                // X<> d
                if (dialog.XexE() != 0) cpu.AssignKey(0xce, 0x7f, dialog.XexE());                // X<> e
                if (dialog.XexM() != 0) cpu.AssignKey(0xce, 0x75, dialog.XexM());                // X<> M
                if (dialog.XexN() != 0) cpu.AssignKey(0xce, 0x76, dialog.XexN());                // X<> N
                if (dialog.XexO() != 0) cpu.AssignKey(0xce, 0x77, dialog.XexO());                // X<> O
                if (dialog.XexP() != 0) cpu.AssignKey(0xce, 0x78, dialog.XexP());                // X<> P
                if (dialog.XexQ() != 0) cpu.AssignKey(0xce, 0x79, dialog.XexQ());                // X<> Q
                if (dialog.HmsMinus() != 0) cpu.AssignKey(0x04, 0x4a, dialog.HmsMinus());        // HMS-
                if (dialog.Del() != 0) cpu.AssignKey(0x04, 0x02, dialog.Del());                  // DEL
                if (dialog.Pack() != 0) cpu.AssignKey(0x04, 0x0a, dialog.Pack());                // PACK
                DebugOutput.AppendText("Synthetic Key Assignments made\r\n");
            }
        }

        private void CardButton_Click(object sender, EventArgs e)
        {
            DialogResult result;
            if (cpu.FlagSet(52))
            {
                result = SaveCardDialog.ShowDialog(this);
                if (result == DialogResult.OK) cpu.Card(SaveCardDialog.FileName);
            }
            else
            {
                result = LoadCardDialog.ShowDialog(this);
                if (result == DialogResult.OK) cpu.Card(LoadCardDialog.FileName);
            }
        }

        public string LoadCard()
        {
            DialogResult result;
            result = LoadCardDialog.ShowDialog();
            if (result == DialogResult.OK)
                return LoadCardDialog.FileName;
            return null;
        }

        public string SaveCard()
        {
            DialogResult result;
            result = SaveCardDialog.ShowDialog();
            if (result == DialogResult.OK)
                return SaveCardDialog.FileName;
            return null;
        }

        private void AllowClear_CheckedChanged(object sender, EventArgs e)
        {
            RamClearButton.Visible = AllowClear.Checked;
        }

        private void TapeButton_Click(object sender, EventArgs e)
        {
            DialogResult result;
            result = SaveCardDialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                cpu.tapeDrive.OpenTapeDrive(SaveCardDialog.FileName);
            }
        }

        public void ExitUser()
        {
            B_SumPlus.Text = "\u2211+";
            b_Recip.Text = "1/X";
            b_Sqrt.Text = "SQRT";
            b_Log.Text = "LOG";
            b_Ln.Text = "LN";
            b_XexY.Text = "X<>Y";
            b_Rdn.Text = "RDN";
            b_Sin.Text = "SIN";
            b_Cos.Text = "COS";
            b_Tan.Text = "TAN";
            b_Xeq.Text = "XEQ";
            b_Sto.Text = "STO";
            b_Rcl.Text = "RCL";
            b_Sst.Text = "SST";
            b_Enter.Text = "ENTER";
            b_Chs.Text = "CHS";
            b_Eex.Text = "EEX";
            b_Clx.Text = "<--";
            b_Minus.Text = "-";
            b_7.Text = "7";
            b_8.Text = "8";
            b_9.Text = "9";
            b_Plus.Text = "+";
            b_4.Text = "4";
            b_5.Text = "5";
            b_6.Text = "6";
            b_Mult.Text = "*";
            b_1.Text = "1";
            b_2.Text = "2";
            b_3.Text = "3";
            b_Div.Text = "/";
            b_0.Text = "0";
            b_Dot.Text = ".";
            b_Rs.Text = "R/S";

            l_SumMinus.Text = "\u2211-";
            l_Recip.Text = "Y^X";
            l_Sqrt.Text = "X^2";
            l_Log.Text = "10^X";
            l_Ln.Text = "E^X";
            l_XexY.Text = "CL\u2211";
            l_Rdn.Text = "%";
            l_Sin.Text = "ASIN";
            l_Cos.Text = "ACOS";
            l_Tan.Text = "ATAN";
            l_Xeq.Text = "ASN";
            l_Sto.Text = "LBL";
            l_Rcl.Text = "GTO";
            l_Sst.Text = "BST";
            l_Enter.Text = "CATALOG";
            l_Chs.Text = "ISG";
            l_Eex.Text = "RTN";
            l_Clx.Text = "CLX/A";
            l_Minus.Text = "X=Y?";
            l_7.Text = "SF";
            l_8.Text = "CF";
            l_9.Text = "FS?";
            l_Plus.Text = "X<=Y?";
            l_4.Text = "BEEP";
            l_5.Text = "P-R";
            l_6.Text = "R-P";
            l_Mult.Text = "X>Y?";
            l_1.Text = "FIX";
            l_2.Text = "SCI";
            l_3.Text = "ENG";
            l_Div.Text = "X=0?";
            l_0.Text = "PI";
            l_Dot.Text = "LASTX";
            l_Rs.Text = "VIEW";
        }


        public string SearchKaPrograms(byte keycode)
        {
            int adr;
            int dst;
            int i;
            int flag;
            int l;
            string ret;
            adr = ((cpu.ram[Cpu.REG_C + 1] & 0x0f) << 8) | cpu.ram[Cpu.REG_C + 0];
            adr = cpu.FromPtr(adr) + 2;
            flag = 0;
            while (flag == 0)
            {
                if (cpu.ram[adr] >= 0xc0 && cpu.ram[adr] <= 0xcd &&
                    cpu.ram[adr - 2] >= 0xf0 && cpu.ram[adr - 3] == keycode)
                {
                    flag = 1;
                }
                else
                {
                    dst = ((cpu.ram[adr] & 0x0f) << 8) | cpu.ram[adr - 1];
                    dst = ((dst & 0x1ff) * 7) + ((dst >> 9) & 0x7);
                    if (dst == 0) flag = 2;
                    else adr += dst - 1;
                }
            }
            if (flag == 1)
            {
                adr -= 2;
                l = cpu.ram[adr] & 0x0f;
                l--;
                adr -= 2;
                ret = "";
                for (i = 0; i < l; i++) ret += ((char)cpu.ram[adr--]).ToString();
                return ret;
            }
            return "";
        }

        public string GetUserLabel(byte keycode)
        {
            int i;
            int j;
            byte b1, b2;
            string ret;

            i = cpu.SearchKaRegisters(keycode);
            if (i >= 0)
            {
                b2 = cpu.ram[i];
                b1 = cpu.ram[i + 1];
                if (b1 >= 0xf0)
                {
                    ret = "BJ";
                }
                else if (b1 >= 0xa0 && b1 <= 0xa7)
                {
                    j = 0;
                    while (cpu.catalog[j].flags != 0xff)
                    {
                        if (cpu.catalog[j].cmd == b1 && cpu.catalog[j].post == b2)
                        {
                            if (cpu.catalog[j].name.Length > 5) return cpu.catalog[j].name.Substring(0, 5);
                            return cpu.catalog[j].name;
                        }
                        j++;
                    }
                    ret = "XROM";
                }
                else if (b1 == 0x04)
                {
                    if (b2 < 0x10)
                    {
                        switch (b2)
                        {
                            case 0x00: ret = "CAT"; break;
                            case 0x01: ret = "@c"; break;
                            case 0x02: ret = "DEL"; break;
                            case 0x03: ret = "COPY"; break;
                            case 0x04: ret = "CLP"; break;
                            case 0x05: ret = "R/S"; break;
                            case 0x06: ret = "SIZE"; break;
                            case 0x07: ret = "BST"; break;
                            case 0x08: ret = "SST"; break;
                            case 0x09: ret = "ON"; break;
                            case 0x0a: ret = "PACK"; break;
                            case 0x0b: ret = "<--"; break;
                            case 0x0c: ret = "ALPHA"; break;
                            case 0x0d: ret = "2__"; break;
                            case 0x0e: ret = "SHIFT"; break;
                            case 0x0f: ret = "ASN"; break;
                            default: ret = ""; break;
                        }
                        return ret;
                    }
                    if (b2 <= 0x1c)
                    {
                        ret = "QLoad";
                        return ret;
                    }
                    ret = cpu.reverse[b2].name;
                }
                else
                {
                    ret = cpu.reverse[b1].name + " ";
                    switch (b2)
                    {
                        case 0x70: ret += "T"; break;
                        case 0x71: ret += "Z"; break;
                        case 0x72: ret += "Y"; break;
                        case 0x73: ret += "X"; break;
                        case 0x74: ret += "L"; break;
                        case 0x75: ret += "M"; break;
                        case 0x76: ret += "N"; break;
                        case 0x77: ret += "O"; break;
                        case 0x78: ret += "P"; break;
                        case 0x79: ret += "Q"; break;
                        case 0x7a: ret += "|-"; break;
                        case 0x7b: ret += "a"; break;
                        case 0x7c: ret += "b"; break;
                        case 0x7d: ret += "c"; break;
                        case 0x7e: ret += "d"; break;
                        case 0x7f: ret += "e"; break;
                        default: ret += $"{b2:d2}"; break;
                    }
                }
                return ret;
            }

            ret = SearchKaPrograms(keycode);
            if (ret.Length > 0)
            {
                if (ret.Length > 5) { ret = ret.Substring(0, 5); }
                return ret;
            }

            i = 0;
            while (ui.keys[i].keycode != keycode && ui.keys[i].skeycode != keycode && ui.keys[i].keycode != 0xff) i++;
            if (ui.keys[i].keycode != 0xff)
            {
                if (ui.keys[i].keycode == keycode) i = ui.keys[i].cmd; else i = ui.keys[i].scmd;
                ret = cpu.catalog[i].name;
                if (ret.Length > 5) ret = ret.Substring(0, 5);
                return ret;
            }
            return "";
        }

        public void EnterUser()
        {
            if (cpu.GetKaFlag(0x01)) B_SumPlus.Text = GetUserLabel(0x01);
            if (cpu.GetKaFlag(0x11)) b_Recip.Text = GetUserLabel(0x11);
            if (cpu.GetKaFlag(0x21)) b_Sqrt.Text = GetUserLabel(0x21);
            if (cpu.GetKaFlag(0x31)) b_Log.Text = GetUserLabel(0x31);
            if (cpu.GetKaFlag(0x41)) b_Ln.Text = GetUserLabel(0x41);

            if (cpu.GetKaFlag(0x02)) b_XexY.Text = GetUserLabel(0x02);
            if (cpu.GetKaFlag(0x12)) b_Rdn.Text = GetUserLabel(0x12);
            if (cpu.GetKaFlag(0x22)) b_Sin.Text = GetUserLabel(0x22);
            if (cpu.GetKaFlag(0x32)) b_Cos.Text = GetUserLabel(0x32);
            if (cpu.GetKaFlag(0x42)) b_Tan.Text = GetUserLabel(0x42);

            if (cpu.GetKaFlag(0x13)) b_Xeq.Text = GetUserLabel(0x13);
            if (cpu.GetKaFlag(0x23)) b_Sto.Text = GetUserLabel(0x23);
            if (cpu.GetKaFlag(0x33)) b_Rcl.Text = GetUserLabel(0x33);
            if (cpu.GetKaFlag(0x43)) b_Sst.Text = GetUserLabel(0x43);

            if (cpu.GetKaFlag(0x04)) b_Enter.Text = GetUserLabel(0x04);
            if (cpu.GetKaFlag(0x24)) b_Chs.Text = GetUserLabel(0x24);
            if (cpu.GetKaFlag(0x34)) b_Eex.Text = GetUserLabel(0x34);
            if (cpu.GetKaFlag(0x44)) b_Clx.Text = GetUserLabel(0x44);

            if (cpu.GetKaFlag(0x05)) b_Minus.Text = GetUserLabel(0x05);
            if (cpu.GetKaFlag(0x15)) b_7.Text = GetUserLabel(0x15);
            if (cpu.GetKaFlag(0x25)) b_8.Text = GetUserLabel(0x25);
            if (cpu.GetKaFlag(0x35)) b_9.Text = GetUserLabel(0x35);

            if (cpu.GetKaFlag(0x06)) b_Plus.Text = GetUserLabel(0x06);
            if (cpu.GetKaFlag(0x16)) b_4.Text = GetUserLabel(0x16);
            if (cpu.GetKaFlag(0x26)) b_5.Text = GetUserLabel(0x26);
            if (cpu.GetKaFlag(0x36)) b_6.Text = GetUserLabel(0x36);

            if (cpu.GetKaFlag(0x07)) b_Mult.Text = GetUserLabel(0x07);
            if (cpu.GetKaFlag(0x17)) b_1.Text = GetUserLabel(0x17);
            if (cpu.GetKaFlag(0x27)) b_2.Text = GetUserLabel(0x27);
            if (cpu.GetKaFlag(0x37)) b_3.Text = GetUserLabel(0x37);

            if (cpu.GetKaFlag(0x08)) b_Div.Text = GetUserLabel(0x08);
            if (cpu.GetKaFlag(0x18)) b_0.Text = GetUserLabel(0x18);
            if (cpu.GetKaFlag(0x28)) b_Dot.Text = GetUserLabel(0x28);
            if (cpu.GetKaFlag(0x38)) b_Rs.Text = GetUserLabel(0x38);

            if (cpu.GetKaFlag(0x09)) l_SumMinus.Text = GetUserLabel(0x09);
            if (cpu.GetKaFlag(0x19)) l_Recip.Text = GetUserLabel(0x19);
            if (cpu.GetKaFlag(0x29)) l_Sqrt.Text = GetUserLabel(0x29);
            if (cpu.GetKaFlag(0x39)) l_Log.Text = GetUserLabel(0x39);
            if (cpu.GetKaFlag(0x49)) l_Ln.Text = GetUserLabel(0x49);

            if (cpu.GetKaFlag(0x0a)) l_XexY.Text = GetUserLabel(0x0a);
            if (cpu.GetKaFlag(0x1a)) l_Rdn.Text = GetUserLabel(0x1a);
            if (cpu.GetKaFlag(0x2a)) l_Sin.Text = GetUserLabel(0x2a);
            if (cpu.GetKaFlag(0x3a)) l_Cos.Text = GetUserLabel(0x3a);
            if (cpu.GetKaFlag(0x4a)) l_Tan.Text = GetUserLabel(0x4a);

            if (cpu.GetKaFlag(0x1b)) l_Xeq.Text = GetUserLabel(0x1b);
            if (cpu.GetKaFlag(0x2b)) l_Sto.Text = GetUserLabel(0x2b);
            if (cpu.GetKaFlag(0x3b)) l_Rcl.Text = GetUserLabel(0x3b);
            if (cpu.GetKaFlag(0x4b)) l_Sst.Text = GetUserLabel(0x4b);

            if (cpu.GetKaFlag(0x0c)) l_Enter.Text = GetUserLabel(0x0c);
            if (cpu.GetKaFlag(0x2c)) l_Chs.Text = GetUserLabel(0x2c);
            if (cpu.GetKaFlag(0x3c)) l_Eex.Text = GetUserLabel(0x3c);
            if (cpu.GetKaFlag(0x4c)) l_Clx.Text = GetUserLabel(0x4c);

            if (cpu.GetKaFlag(0x0d)) l_Minus.Text = GetUserLabel(0x0d);
            if (cpu.GetKaFlag(0x1d)) l_7.Text = GetUserLabel(0x1d);
            if (cpu.GetKaFlag(0x2d)) l_8.Text = GetUserLabel(0x2d);
            if (cpu.GetKaFlag(0x3d)) l_9.Text = GetUserLabel(0x3d);

            if (cpu.GetKaFlag(0x0e)) l_Plus.Text = GetUserLabel(0x0e);
            if (cpu.GetKaFlag(0x1e)) l_4.Text = GetUserLabel(0x1e);
            if (cpu.GetKaFlag(0x2e)) l_5.Text = GetUserLabel(0x2e);
            if (cpu.GetKaFlag(0x3e)) l_6.Text = GetUserLabel(0x3e);

            if (cpu.GetKaFlag(0x0f)) l_Mult.Text = GetUserLabel(0x0f);
            if (cpu.GetKaFlag(0x1f)) l_1.Text = GetUserLabel(0x1f);
            if (cpu.GetKaFlag(0x2f)) l_2.Text = GetUserLabel(0x2f);
            if (cpu.GetKaFlag(0x3f)) l_3.Text = GetUserLabel(0x3f);

            if (cpu.GetKaFlag(0x10)) l_Div.Text = GetUserLabel(0x10);
            if (cpu.GetKaFlag(0x20)) l_0.Text = GetUserLabel(0x20);
            if (cpu.GetKaFlag(0x30)) l_Dot.Text = GetUserLabel(0x30);
            if (cpu.GetKaFlag(0x40)) l_Rs.Text = GetUserLabel(0x40);
        }

        public void Trace(string msg)
        {
            DebugOutput.AppendText(msg + "\r\n");
        }

        public bool Trace()
        {
            return cb_Trace.Checked;
        }

        public char TraceMode()
        {
            if (rb_None.Checked) return 'N';
            if (rb_Stack.Checked) return 'K';
            if (rb_Stat.Checked) return 'S';
            return 'N';
        }

        public void TraceRegs()
        {
            if (TraceMode() == 'K') debugger.ShowStatRegs(true);
            if (TraceMode() == 'S') debugger.ShowStatRegs(false);
        }

        private void scrollTimer_Tick(object sender, EventArgs e)
        {
            scrollTimer.Enabled = false;
            if (scroll)
            {
                scroll = false;
                displayStart++;
                UpdateDisplay();
            }
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Enter) { ui.ButtonDown("04"); ui.ButtonUp("04"); return true; }
            if (keyData == Keys.Back) { ui.ButtonDown("44"); ui.ButtonUp("44"); return true; }
            if (keyData == Keys.Right) { ui.ButtonDown("43"); ui.ButtonUp("43"); return true; }
            if (keyData == Keys.Left) { ui.Key_Shift(); ui.ButtonDown("43"); ui.ButtonUp("43"); return true; }
            if (keyData == Keys.Down) { ui.ButtonDown("43"); ui.ButtonUp("43"); return true; }
            if (keyData == Keys.Up) { ui.Key_Shift(); ui.ButtonDown("43"); ui.ButtonUp("43"); return true; }
            if (keyData == Keys.Tab && Control.ModifierKeys != Keys.Shift) { ui.ButtonDown("38"); ui.ButtonUp("38"); return true; }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void Form1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '"' || e.KeyChar == '\'') { ui.Key_Alpha(); return; }
            if (e.KeyChar == '-') { ui.ButtonDown("05"); ui.ButtonUp("05"); return; }           // - X=Y?
            if (e.KeyChar == '+') { ui.ButtonDown("06"); ui.ButtonUp("06"); return; }           // + X<=Y?
            if (e.KeyChar == '*' && Control.ModifierKeys != Keys.Shift) { ui.ButtonDown("07"); ui.ButtonUp("07"); return; }           // *
            if (e.KeyChar == '/') { ui.ButtonDown("08"); ui.ButtonUp("08"); return; }           // + X<=Y?
            if (Control.ModifierKeys == Keys.Shift) ui.Key_Shift();
            if (e.KeyChar == 'a' || e.KeyChar == 'A') { ui.ButtonDown("01"); ui.ButtonUp("01"); return; }           // E+  E-
            if (e.KeyChar == 'b' || e.KeyChar == 'B') { ui.ButtonDown("11"); ui.ButtonUp("11"); return; }           // 1/X  Y^X
            if (e.KeyChar == 'c' || e.KeyChar == 'C') { ui.ButtonDown("21"); ui.ButtonUp("21"); return; }           // SQRT  X^2
            if (e.KeyChar == 'd' || e.KeyChar == 'D') { ui.ButtonDown("31"); ui.ButtonUp("31"); return; }           // LOG  10^X
            if (e.KeyChar == 'e' || e.KeyChar == 'E') { ui.ButtonDown("41"); ui.ButtonUp("41"); return; }           // LN E^X

            if (e.KeyChar == 'f' || e.KeyChar == 'F') { ui.ButtonDown("02"); ui.ButtonUp("02"); return; }           // X<>Y  CLE
            if (e.KeyChar == 'g' || e.KeyChar == 'G') { ui.ButtonDown("12"); ui.ButtonUp("12"); return; }           // RDN  %
            if (e.KeyChar == 'h' || e.KeyChar == 'H') { ui.ButtonDown("22"); ui.ButtonUp("22"); return; }           // SIN  ASIN
            if (e.KeyChar == 'i' || e.KeyChar == 'I') { ui.ButtonDown("32"); ui.ButtonUp("32"); return; }           // COS  ACOS
            if (e.KeyChar == 'j' || e.KeyChar == 'J') { ui.ButtonDown("42"); ui.ButtonUp("42"); return; }           // TAN  ATAN

            if (e.KeyChar == 'k' || e.KeyChar == 'K') { ui.ButtonDown("13"); ui.ButtonUp("13"); return; }           // XEQ  ASN
            if (e.KeyChar == 'l' || e.KeyChar == 'L') { ui.ButtonDown("23"); ui.ButtonUp("23"); return; }           // STO  LBL
            if (e.KeyChar == 'm' || e.KeyChar == 'M') { ui.ButtonDown("33"); ui.ButtonUp("33"); return; }           // RCL  GTO

            if (e.KeyChar == 'n' || e.KeyChar == 'N') { ui.ButtonDown("04"); ui.ButtonUp("04"); return; }           // ENTER^ CATALOG
            if (e.KeyChar == 'o' || e.KeyChar == 'O') { ui.ButtonDown("24"); ui.ButtonUp("24"); return; }           // CHS ISG
            if (e.KeyChar == 'p' || e.KeyChar == 'P') { ui.ButtonDown("34"); ui.ButtonUp("34"); return; }           // EEX RTN

            if (e.KeyChar == 'q' || e.KeyChar == 'Q') { ui.ButtonDown("05"); ui.ButtonUp("05"); return; }           // - X=Y?
            if (e.KeyChar == 'r' || e.KeyChar == 'R') { ui.ButtonDown("15"); ui.ButtonUp("15"); return; }           // 7 SF
            if (e.KeyChar == 's' || e.KeyChar == 'S') { ui.ButtonDown("25"); ui.ButtonUp("25"); return; }           // 8 CF
            if (e.KeyChar == 't' || e.KeyChar == 'T') { ui.ButtonDown("35"); ui.ButtonUp("35"); return; }           // 9 FS?
            if (e.KeyChar == 'u' || e.KeyChar == 'U') { ui.ButtonDown("06"); ui.ButtonUp("06"); return; }           // + X<=Y?
            if (e.KeyChar == 'v' || e.KeyChar == 'V') { ui.ButtonDown("16"); ui.ButtonUp("16"); return; }           // 4 BEEP
            if (e.KeyChar == 'w' || e.KeyChar == 'W') { ui.ButtonDown("26"); ui.ButtonUp("26"); return; }           // 5 P-R
            if (e.KeyChar == 'x' || e.KeyChar == 'X') { ui.ButtonDown("36"); ui.ButtonUp("36"); return; }           // 6 R-P
            if (e.KeyChar == 'y' || e.KeyChar == 'Y') { ui.ButtonDown("07"); ui.ButtonUp("07"); return; }           // * X>Y?
            if (e.KeyChar == 'z' || e.KeyChar == 'Z') { ui.ButtonDown("17"); ui.ButtonUp("17"); return; }           // 1 FIX
            if (e.KeyChar == '=') { ui.ButtonDown("27"); ui.ButtonUp("27"); return; }           // 2 SCI
            if (e.KeyChar == ':' || e.KeyChar == ';') { ui.ButtonDown("08"); ui.ButtonUp("08"); return; }           // .  LASTX

            if (e.KeyChar == '0' || e.KeyChar == ')') { ui.ButtonDown("18"); ui.ButtonUp("18"); return; }           // 0  PI
            if (e.KeyChar == '1' || e.KeyChar == '!') { ui.ButtonDown("17"); ui.ButtonUp("17"); return; }           // 1 FIX
            if (e.KeyChar == '2' || e.KeyChar == '@') { ui.ButtonDown("27"); ui.ButtonUp("27"); return; }           // 2 SCI
            if (e.KeyChar == '3' || e.KeyChar == '#') { ui.ButtonDown("37"); ui.ButtonUp("37"); return; }           // 3 ENG
            if (e.KeyChar == '4' || e.KeyChar == '$') { ui.ButtonDown("16"); ui.ButtonUp("16"); return; }           // 4 BEEP
            if (e.KeyChar == '5' || e.KeyChar == '%') { ui.ButtonDown("26"); ui.ButtonUp("26"); return; }           // 5 P-R
            if (e.KeyChar == '6' || e.KeyChar == '^') { ui.ButtonDown("36"); ui.ButtonUp("36"); return; }           // 6 R-P
            if (e.KeyChar == '7' || e.KeyChar == '&') { ui.ButtonDown("15"); ui.ButtonUp("15"); return; }           // 7 SF
            if (e.KeyChar == '8' || e.KeyChar == '*') { ui.ButtonDown("25"); ui.ButtonUp("25"); return; }           // 8 CF
            if (e.KeyChar == '9' || e.KeyChar == '(') { ui.ButtonDown("35"); ui.ButtonUp("35"); return; }           // 9 FS?
            if (e.KeyChar == '.' || e.KeyChar == '>') { ui.ButtonDown("28"); ui.ButtonUp("28"); return; }           // .  LASTX
            if (e.KeyChar == ',' || e.KeyChar == '<') { ui.ButtonDown("28"); ui.ButtonUp("28"); return; }           // .  LASTX
            if (e.KeyChar == ' ') { ui.ButtonDown("18"); ui.ButtonUp("18"); return; }           // + X<=Y?

            MessageBox.Show($"Unknown key {e.KeyChar}");
        }

        private void PrinterPowerOn_CheckedChanged(object sender, EventArgs e)
        {
            cpu.printer.power = PrinterPowerOn.Checked;
        }

        private void GraphicsPrinterScroll_ValueChanged(object sender, EventArgs e)
        {
            var gc = Graphics.FromImage(PrinterPage);
            var black = new SolidBrush(Color.Black);
            var white = new SolidBrush(Color.White);
            int topLine = GraphicsPrinterScroll.Value;
            if (topLine < 1) topLine = 1;
            if (topLine > 224) topLine = 224;
            for (int i = 0; i < 25; i++)
            {
                PrintAt(printerLines[topLine + i], lineJustify[topLine + i], i + 1, gc, black, white);
            }
            black.Dispose();
            white.Dispose();
            gc.Dispose();
            GraphicPrinter.Image = PrinterPage;

        }

        private void printerModeChanged(object sender, EventArgs e)
        {
            if (pm_Man.Checked) cpu.printer.printerMode = 'M';
            if (pm_Trace.Checked) cpu.printer.printerMode = 'T';
            if (pm_Norm.Checked) cpu.printer.printerMode = 'N';
        }

        private void ERegsButton_Click(object sender, EventArgs e)
        {
            debugger.ShowEregs(Convert.ToInt32(EregsFrom.Text), Convert.ToInt32(EregsTo.Text));
        }
    }
}

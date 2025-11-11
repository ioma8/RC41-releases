using Microsoft.VisualBasic.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.Linq;
using System.Runtime.Intrinsics.Arm;
using System.Security.Policy;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using Rc41.T_TapeDrive;
using Rc41.T_Printer;
using Rc41.T_CardReader;
using Rc41.T_Extended;
using Rc41.T_TimeModule;

namespace Rc41.T_Cpu
{
    public partial class Cpu
    {
        public const int REG_T = 0;
        public const int REG_Z = 7;
        public const int REG_Y = 14;
        public const int REG_X = 21;
        public const int REG_L = 28;
        public const int REG_M = 35;
        public const int REG_N = 42;
        public const int REG_O = 49;
        public const int REG_P = 56;
        public const int REG_Q = 63;
        public const int REG_R = 70;
        public const int REG_A = 77;
        public const int REG_B = 84;
        public const int REG_C = 91;
        public const int REG_D = 98;
        public const int REG_E = 105;
        public const int LIFT = 112;
        public const int PENDING = 113;
        public const int R_T = 0;
        public const int R_Z = 1;
        public const int R_Y = 2;
        public const int R_X = 3;
        public const int R_L = 4;
        public const int R_M = 5;
        public const int R_N = 6;
        public const int R_O = 7;
        public const int R_P = 8;
        public const int R_Q = 9;
        public const int R_R = 10;
        public const int R_A = 11;
        public const int R_B = 12;
        public const int R_C = 13;
        public const int R_D = 14;
        public const int R_E = 15;
        public const int RAMTOP = 0x200;
        public const int RAMSIZE = RAMTOP * 7;
        public const int F_AUTO = 11;
        public const int F_PRT_DBL = 12;
        public const int F_PRT_LC = 13;
        public const int F_PREN = 21;
        public const int F_NUM_IN = 22;
        public const int F_ALPHA_IN = 23;
        public const int F_IGN_RANGE = 24;
        public const int F_IGN_ERR = 25;
        public const int F_AUDIO = 26;
        public const int F_USER = 27;
        public const int F_CAT = 30;
        public const int F_GRAD = 42;
        public const int F_RAD = 43;
        public const int F_SYS = 45;
        public const int F_PARTIAL = 46;
        public const int F_SHIFT = 47;
        public const int F_ALPHA = 48;
        public const int F_LOW_BAT = 49;
        public const int F_MSG = 50;
        public const int F_SST = 51;
        public const int F_PRGM = 52;
        public const int F_PSE = 54;
        public const int F_PRT_EXIST = 55;
        public const int M_DIRECT = 0;
        public const int M_INP_NUM = 1;
        public const int M_INP_ALPHA = 2;
        public const int M_CAT = 3;

        public const int CM_DIRECT = 0;
        public const int CM_CLOCK = 10;
        public const int CM_SW = 11;

        public const int CMD_ASN = 26;
        public const int CMD_BST = 31;
        public const int CMD_CAT = 32;
        public const int CMD_CLP = 38;
        public const int CMD_DEL = 46;
        public const int CMD_GTO = 61;
        public const int CMD_LBL = 69;
        public const int CMD_PACK = 79;
        public const int CMD_SIZE = 101;
        public const int CMD_SST = 103;
        public const int CMD_RS = 109;
        public const int CMD_XEQ = 125;

        public int CMD_LIST = 0;  // 134;
        public int CMD_PRP = 0;   // 139;
        public int CMD_NEWM = 0;  // 178;

        public int CMD_XEQ_A = 0; // 205;
        public int CMD_LBL_A = 0; // 206;
        public int CMD_GTO_ALPHA = 0; // 207;
        public int CMD_GTO_DOT = 0; // 208;
        public int CMD_GTO_A = 0; // 210;
        public const int CMD_BS = 999;

//        bit 0 \ 00  - no arg
//       1 / 01  - 1 digit arg
//           10  - 2 digit arg
//           11  - 3 digit arg
//       2       - allow stack(X, Y, Z, T, L)
//       3       - allow indirect
//       4       - alpha arg
//       5       - allow dot
//       6
//       7
//      15       - not in program

        public Catalog[] catalog = new Catalog[]
        {
//                    cat   name       cmd  post   key flags sz 
            new Catalog(0, "0",       0x10, 0x00,   82,    0, 0x01 ),    /*  00 00 */
            new Catalog(0, "1",       0x11, 0x00,   82,    0, 0x01 ),    /*  01 01 */
            new Catalog(0, "2",       0x12, 0x00,   73,    0, 0x01 ),    /*  02 02 */
            new Catalog(0, "3",       0x13, 0x00,   74,    0, 0x01 ),    /*  03 03 */
            new Catalog(0, "4",       0x14, 0x00,   62,    0, 0x01 ),    /*  04 04 */
            new Catalog(0, "5",       0x15, 0x00,   63,    0, 0x01 ),    /*  05 05 */
            new Catalog(0, "6",       0x16, 0x00,   64,    0, 0x01 ),    /*  06 06 */
            new Catalog(0, "7",       0x17, 0x00,   52,    0, 0x01 ),    /*  07 07 */
            new Catalog(0, "8",       0x18, 0x00,   53,    0, 0x01 ),    /*  08 08 */
            new Catalog(0, "9",       0x19, 0x00,   54,    0, 0x01 ),    /*  09 09 */
            new Catalog(0, ".",       0x1a, 0x00,   83,    0, 0x01 ),    /*  10 0a */
            new Catalog(0, "EEX",     0x1b, 0x00,   44,    0, 0x01 ),    /*  11 0b */
            new Catalog(3, "+",       0x40, 0x00,   61,    0, 0x01 ),    /*  12 0c */
            new Catalog(3, "-",       0x41, 0x00,   41,    0, 0x01 ),    /*  13 0d */
            new Catalog(3, "*",       0x42, 0x00,   71,    0, 0x01 ),    /*  14 0e */
            new Catalog(3, "/",       0x43, 0x00,   81,    0, 0x01 ),    /*  15 0f */
            new Catalog(3, "1/X",     0x60, 0x00,   12,    0, 0x01 ),    /*  16 10 */
            new Catalog(3, "10^X",    0x57, 0x00,  -14,    0, 0x01 ),    /*  17 11 */
            new Catalog(3, "ABS",     0x61, 0x00,   00,    0, 0x01 ),    /*  18 12 */
            new Catalog(3, "ACOS",    0x5d, 0x00,  -24,    0, 0x01 ),    /*  19 13 */
            new Catalog(3, "ADV",     0x8f, 0x00,   00,    0, 0x01 ),    /*  20 14 */
            new Catalog(3, "AOFF",    0x8b, 0x00,   00,    0, 0x01 ),    /*  21 15 */
            new Catalog(3, "AON",     0x8c, 0x00,   00,    0, 0x01 ),    /*  22 16*/
            new Catalog(3, "ARCL",    0x9b, 0x00,  -34, 0x0e, 0x02 ),    /*  23 17 */
            new Catalog(3, "ASHF",    0x88, 0x00,   00,    0, 0x01 ),    /*  24 18 */
            new Catalog(3, "ASIN",    0x5c, 0x00,  -23,    0, 0x01 ),    /*  25 19 */
            new Catalog(3, "ASN",     0x00, 0x00,  -32, 0x90, 0x01 ),    /*  26 1a */
            new Catalog(3, "ASTO",    0x9a, 0x00,  -33, 0x0e, 0x02 ),    /*  27 1b */
            new Catalog(3, "ATAN",    0x5e, 0x00,  -25,    0, 0x01 ),    /*  28 1c */
            new Catalog(3, "AVIEW",   0x7e, 0x00,  -84,    0, 0x01 ),    /*  29 1d */
            new Catalog(3, "BEEP",    0x86, 0x00,  -62,    0, 0x01 ),    /*  30 1e */
            new Catalog(3, "BST",     0x00, 0x00,  -35, 0x80, 0x01 ),    /*  31 1f */
            new Catalog(3, "CAT",     0x00, 0x00,   41, 0x81, 0x01 ),    /*  32 20 */
            new Catalog(3, "CF",      0xa9, 0x00,  -53, 0x0a, 0x02 ),    /*  33 21 */
            new Catalog(3, "CHS",     0x54, 0x00,   43,    0, 0x01 ),    /*  34 22 */
            new Catalog(3, "CLA",     0x87, 0x00,  -45,    0, 0x01 ),    /*  35 23 */
            new Catalog(3, "CLD",     0x7f, 0x00,   00,    0, 0x01 ),    /*  36 24 */
            new Catalog(3, "CL\x7e",  0x70, 0x00,  -21,    0, 0x01 ),    /*  37 25 */
            new Catalog(3, "CLP",     0x04, 0x04,   00, 0x90, 0x01 ),    /*  38 26 */
            new Catalog(3, "CLRG",    0x8a, 0x00,   00,    0, 0x01 ),    /*  39 27 */
            new Catalog(3, "CLST",    0x73, 0x00,   00,    0, 0x01 ),    /*  40 28 */
            new Catalog(3, "CLX",     0x77, 0x00,  -45,    0, 0x01 ),    /*  41 29 */
            new Catalog(3, "COS",     0x5a, 0x00,   24,    0, 0x01 ),    /*  42 2a */
            new Catalog(3, "D-R",     0x6a, 0x00,   00,    0, 0x01 ),    /*  43 2b */
            new Catalog(3, "DEC",     0x5f, 0x00,   00,    0, 0x01 ),    /*  44 2c */
            new Catalog(3, "DEG",     0x80, 0x00,   00,    0, 0x01 ),    /*  45 2d */
            new Catalog(3, "DEL",     0x04, 0x02,   00, 0x83, 0x01 ),    /*  46 2e */
            new Catalog(3, "DSE",     0x97, 0x00,   00, 0x1e, 0x02 ),    /*  47 2f */
            new Catalog(3, "E^X",     0x55, 0x00,  -15,    0, 0x01 ),    /*  48 30 */
            new Catalog(3, "E^X-1",   0x58, 0x00,   00,    0, 0x01 ),    /*  49 31 */
            new Catalog(3, "END",     0xc0, 0x00,   00,    0, 0x01 ),    /*  50 32 */
            new Catalog(3, "ENG",     0x9e, 0x00,  -74, 0x09, 0x82 ),    /*  51 33 */
            new Catalog(3, "ENTER^",  0x83, 0x00,   41,    0, 0x01 ),    /*  52 34 */
            new Catalog(3, "FACT",    0x62, 0x00,   00,    0, 0x01 ),    /*  53 35 */
            new Catalog(3, "FC?",     0xad, 0x00,   00, 0x0a, 0x02 ),    /*  54 36 */
            new Catalog(3, "FC?C",    0xab, 0x00,   00, 0x0a, 0x02 ),    /*  55 37 */
            new Catalog(3, "FIX",     0x9c, 0x00,  -72, 0x09, 0x82 ),    /*  56 38 */
            new Catalog(3, "FRC",     0x69, 0x00,   00,    0, 0x01 ),    /*  57 39 */
            new Catalog(3, "FS?",     0xac, 0x00,  -54, 0x0a, 0x02 ),    /*  58 3a */
            new Catalog(3, "FS?C",    0xaa, 0x00,   00, 0x0a, 0x02 ),    /*  59 3b */
            new Catalog(3, "GRAD",    0x82, 0x00,   00,    0, 0x01 ),    /*  60 3c */
            new Catalog(3, "GTO",     0xb1, 0x00,  -34, 0x3a, 0x01 ),    /*  61 3d */
            new Catalog(3, "HMS",     0x6c, 0x00,   00,    0, 0x01 ),    /*  62 3e */
            new Catalog(3, "HMS+",    0x49, 0x00,   00,    0, 0x01 ),    /*  63 3f */
            new Catalog(3, "HMS-",    0x4a, 0x00,   00,    0, 0x01 ),    /*  64 40 */
            new Catalog(3, "HR",      0x6d, 0x00,   00,    0, 0x01 ),    /*  65 41 */
            new Catalog(3, "INT",     0x68, 0x00,   00,    0, 0x01 ),    /*  66 42 */
            new Catalog(3, "ISG",     0x96, 0x00,  -43, 0x1e, 0x02 ),    /*  67 43 */
            new Catalog(3, "LASTX",   0x76, 0x00,  -83,    0, 0x01 ),    /*  68 44 */
            new Catalog(3, "LBL",     0x01, 0x00,  -33, 0x1a, 0x01 ),    /*  69 45 */
            new Catalog(3, "LOG",     0x56, 0x00,   14,    0, 0x01 ),    /*  70 46 */
            new Catalog(3, "LN",      0x50, 0x00,   15,    0, 0x01 ),    /*  71 47 */
            new Catalog(3, "LN1+X",   0x65, 0x00,   00,    0, 0x01 ),    /*  72 48 */
            new Catalog(3, "MEAN",    0x7c, 0x00,   00,    0, 0x01 ),    /*  73 49 */
            new Catalog(3, "MOD",     0x4b, 0x00,   00,    0, 0x01 ),    /*  74 4a */
            new Catalog(3, "OCT",     0x6f, 0x00,   00,    0, 0x01 ),    /*  75 4b */
            new Catalog(3, "OFF",     0x8d, 0x00,   00,    0, 0x01 ),    /*  76 4c */
            new Catalog(3, "ON",      0x00, 0x00,   00, 0x80, 0x01 ),    /*  77 4d */
            new Catalog(3, "P-R",     0x4e, 0x00,   63,    0, 0x01 ),    /*  78 4e */
            new Catalog(3, "PACK",    0x04, 0x0a,   00, 0x80, 0x01 ),    /*  79 4f */
            new Catalog(3, "%",       0x4c, 0x00,  -22,    0, 0x01 ),    /*  80 50 */
            new Catalog(3, "%CH",     0x4d, 0x00,   00,    0, 0x01 ),    /*  81 51 */
            new Catalog(3, "PI",      0x72, 0x00,  -82,    0, 0x01 ),    /*  82 52 */
            new Catalog(3, "PROMPT",  0x8e, 0x00,   00,    0, 0x01 ),    /*  83 53 */
            new Catalog(3, "PSE",     0x89, 0x00,   00,    0, 0x01 ),    /*  84 54 */
            new Catalog(3, "R^",      0x74, 0x00,   00,    0, 0x01 ),    /*  85 55 */
            new Catalog(3, "R-D",     0x6b, 0x00,   00,    0, 0x01 ),    /*  86 56 */
            new Catalog(3, "R-P",     0x4f, 0x00,   64,    0, 0x01 ),    /*  87 57 */
            new Catalog(3, "RAD",     0x81, 0x00,   00,    0, 0x01 ),    /*  88 58 */
            new Catalog(3, "RCL",     0x20, 0x00,   34, 0x0e, 0x01 ),    /*  89 59 */
            new Catalog(3, "RDN",     0x75, 0x00,   22,    0, 0x01 ),    /*  90 5a */
            new Catalog(3, "RND",     0x6e, 0x00,   00,    0, 0x01 ),    /*  91 5b */
            new Catalog(3, "RTN",     0x85, 0x00,  -44,    0, 0x01 ),    /*  92 5c */
            new Catalog(3, "SDEV",    0x7d, 0x00,   00,    0, 0x01 ),    /*  93 5d */
            new Catalog(3, "SCI",     0x9d, 0x00,  -73, 0x09, 0x82 ),    /*  94 5e */
            new Catalog(3, "SF",      0xa8, 0x00,  -52, 0x0a, 0x02 ),    /*  95 5f */
            new Catalog(3, "\x7e+",   0x47, 0x00,   11,    0, 0x01 ),    /*  96 60 */
            new Catalog(3, "\x7e-",   0x48, 0x00,  -11,    0, 0x01 ),    /*  97 61 */
            new Catalog(3, "\x7eREG", 0x99, 0x00,   00, 0x12, 0x02 ),    /*  98 62 */
            new Catalog(3, "SIN",     0x59, 0x00,  23,     0, 0x01 ),     /*  99 63 */
            new Catalog(3, "SIGN",    0x7a, 0x00,   00,    0, 0x01 ),    /* 100 64 */
            new Catalog(3, "SIZE",    0x04, 0x06,   00, 0x83, 0x01 ),    /* 101 65 */
            new Catalog(3, "SQRT",    0x52, 0x00,   13,    0, 0x01 ),    /* 102 66 */
            new Catalog(3, "SST",     0x00, 0x00,   35, 0x80, 0x01 ),    /* 103 67 */
            new Catalog(3, "ST+",     0x92, 0x00,   00, 0x0e, 0x02 ),    /* 104 68 */
            new Catalog(3, "ST-",     0x93, 0x00,   00, 0x0e, 0x02 ),    /* 105 69 */
            new Catalog(3, "ST*",     0x94, 0x00,   00, 0x0e, 0x02 ),    /* 106 6a */
            new Catalog(3, "ST/",     0x95, 0x00,   00, 0x0e, 0x02 ),    /* 107 6b */
            new Catalog(3, "STO",     0x30, 0x00,   33, 0x0e, 0x01 ),    /* 108 6c */
            new Catalog(3, "STOP",    0x84, 0x00,   00,    0, 0x01 ),    /* 109 6d */
            new Catalog(3, "TAN",     0x5b, 0x00,   25,    0, 0x01 ),    /* 110 6e */
            new Catalog(3, "TONE",    0x9f, 0x00,   00, 0x09, 0x01 ),    /* 111 6f */
            new Catalog(3, "VIEW",    0x98, 0x00,  -84, 0x0e, 0x02 ),    /* 112 70 */
            new Catalog(3, "X=0?",    0x67, 0x00,  -81,    0, 0x01 ),    /* 113 71 */
            new Catalog(3, "X<>0?",   0x63, 0x00,   00,    0, 0x01 ),    /* 114 72 */
            new Catalog(3, "X<0?",    0x66, 0x00,   00,    0, 0x01 ),    /* 115 73 */
            new Catalog(3, "X<=0?",   0x7b, 0x00,   00,    0, 0x01 ),    /* 116 74 */
            new Catalog(3, "X>0?",    0x64, 0x00,   00,    0, 0x01 ),    /* 117 75 */
            new Catalog(3, "X=Y?",    0x78, 0x00,  -51,    0, 0x01 ),    /* 118 76 */
            new Catalog(3, "X<>Y?",   0x79, 0x00,   00,    0, 0x01 ),    /* 119 77 */
            new Catalog(3, "X<Y?",    0x44, 0x00,   00,    0, 0x01 ),    /* 120 78 */
            new Catalog(3, "X<=Y?",   0x46, 0x00,   61,    0, 0x01 ),    /* 121 79 */
            new Catalog(3, "X>Y?",    0x45, 0x00,  -71,    0, 0x01 ),    /* 122 7a */
            new Catalog(3, "X<>",     0xce, 0x00,   00, 0x0e, 0x02 ),    /* 123 7b */
            new Catalog(3, "X<>Y",    0x71, 0x00,   21,    0, 0x01 ),    /* 124 7c */
            new Catalog(3, "XEQ",     0xe0, 0x00,   32, 0x1a, 0x63 ),    /* 125 7d */
            new Catalog(3, "X^2",     0x51, 0x00,  -13,    0, 0x01 ),    /* 126 7e */
            new Catalog(3, "Y^X",     0x53, 0x00,  -12,    0, 0x01 ),    /* 127 7f */

            new Catalog(0, "",        0x00, 0x00,   00, 0xfe, 0x01 ),    /* 128 */
            new Catalog(0, "XEQ",     0x1e, 0x00,   32, 0x1a, 0x13 ),    /* 129 */     // XEQ alpha
            new Catalog(0, "LBL",     0xc1, 0x00,  -33, 0x1a, 0x01 ),    /* 130 */     // LBL alpha
            new Catalog(0, "GTO",     0x1d, 0x00,  -34, 0x1a, 0x13 ),    /* 131 */     // GTO alpha
            new Catalog(0, "GTO",     0x1d, 0x00,  -34, 0xb3, 0x13 ),    /* 132 */     // GTO .___
            new Catalog(0, "CHS",     0x1c, 0x00,   00, 0x83, 0x01 ),    /* 133 */     // CHS number entry
            new Catalog(0, "GTO",     0x1d, 0x00,  -34, 0xb3, 0x13 ),    /* 134 */     // GTO .alpha

            new Catalog(2, "ACA",     0xa7, 0x41,   00,    0, 0x01 ),    /* 135 80 */
            new Catalog(2, "ACCHR",   0xa7, 0x42,   00,    0, 0x01 ),    /* 129 81 */
            new Catalog(2, "ACCOL",   0xa7, 0x43,   00,    0, 0x01 ),    /* 130 82 */
            new Catalog(2, "ACSPEC",  0xa7, 0x44,   00,    0, 0x01 ),    /* 131 83 */
            new Catalog(2, "ACX",     0xa7, 0x45,   00,    0, 0x01 ),    /* 132 84 */
            new Catalog(2, "BLDSPEC", 0xa7, 0x46,   00,    0, 0x01 ),    /* 133 85 */
            new Catalog(2, "LIST",    0xa7, 0x47,   00, 0x03, 0x01 ),    /* 134 86 */
            new Catalog(2, "PRA",     0xa7, 0x48,   00,    0, 0x01 ),    /* 135 87 */
            new Catalog(2, "PRBUF",   0xa7, 0x4a,   00,    0, 0x01 ),    /* 136 88 */
            new Catalog(2, "PRFLAGS", 0xa7, 0x4b,   00,    0, 0x01 ),    /* 137 89 */
            new Catalog(2, "PRKEYS",  0xa7, 0x4c,   00,    0, 0x01 ),    /* 138 8a */
            new Catalog(2, "PRP",     0xa7, 0x4d,   00, 0x90, 0x01 ),    /* 139 8b */
            new Catalog(2, "PRREG",   0xa7, 0x50,   00,    0, 0x01 ),    /* 140 */
            new Catalog(2, "PRREGX",  0xa7, 0x51,   00,    0, 0x01 ),    /* 141 */
            new Catalog(2, "PR\x7e",  0xa7, 0x52,   00,    0, 0x01 ),    /* 142 */
            new Catalog(2, "PRSTK",   0xa7, 0x53,   00,    0, 0x01 ),    /* 143 */
            new Catalog(2, "PRX",     0xa7, 0x54,   00,    0, 0x01 ),    /* 144 */
            new Catalog(2, "REGPLOT", 0xa7, 0x55,   00,    0, 0x01 ),    /* 145 */
            new Catalog(2, "SKPCHR",  0xa7, 0x56,   00,    0, 0x01 ),    /* 146 */
            new Catalog(2, "SKPCOL",  0xa7, 0x57,   00,    0, 0x01 ),    /* 147 */
            new Catalog(2, "STKPLOT", 0xa7, 0x58,   00,    0, 0x01 ),    /* 148 */

            new Catalog(2, "RDTA",    0xa7, 0x82,   00,    0, 0x01 ),    /* 149 */
            new Catalog(2, "RDTAX",   0xa7, 0x83,   00,    0, 0x01 ),    /* 150 */
            new Catalog(2, "VER",     0xa7, 0x85,   00,    0, 0x01 ),    /* 151 */
            new Catalog(2, "WALL",    0xa7, 0x86,   00,    0, 0x01 ),    /* 152 */
            new Catalog(2, "WDTA",    0xa7, 0x87,   00,    0, 0x01 ),    /* 153 */
            new Catalog(2, "WDTAX",   0xa7, 0x88,   00,    0, 0x01 ),    /* 154 */
            new Catalog(2, "WSTS",    0xa7, 0x8a,   00,    0, 0x01 ),    /* 155 */
            new Catalog(2, "7CLREG",  0xa7, 0x8b,   00,    0, 0x01 ),    /* 156 */
            new Catalog(2, "7DSP0",   0xa7, 0x8c,   00,    0, 0x01 ),    /* 157 */
            new Catalog(2, "7DSP1",   0xa7, 0x8d,   00,    0, 0x01 ),    /* 158 */
            new Catalog(2, "7DSP2",   0xa7, 0x8e,   00,    0, 0x01 ),    /* 159 */
            new Catalog(2, "7DSP3",   0xa7, 0x8f,   00,    0, 0x01 ),    /* 160 */
            new Catalog(2, "7DSP4",   0xa7, 0x90,   00,    0, 0x01 ),    /* 161 */
            new Catalog(2, "7DSP5",   0xa7, 0x91,   00,    0, 0x01 ),    /* 162 */
            new Catalog(2, "7DSP6",   0xa7, 0x92,   00,    0, 0x01 ),    /* 163 */
            new Catalog(2, "7DSP7",   0xa7, 0x93,   00,    0, 0x01 ),    /* 164 */
            new Catalog(2, "7DSP8",   0xa7, 0x94,   00,    0, 0x01 ),    /* 165 */
            new Catalog(2, "7DSP9",   0xa7, 0x95,   00,    0, 0x01 ),    /* 166 */
            new Catalog(2, "7DSPI",   0xa7, 0x96,   00,    0, 0x01 ),    /* 167 */
            new Catalog(2, "7DSZ",    0xa7, 0x97,   00,    0, 0x01 ),    /* 168 */
            new Catalog(2, "7DSZI",   0xa7, 0x98,   00,    0, 0x01 ),    /* 169 */
            new Catalog(2, "7ENG",    0xa7, 0x99,   00,    0, 0x01 ),    /* 170 */
            new Catalog(2, "7FIX",    0xa7, 0x9a,   00,    0, 0x01 ),    /* 171 */
            new Catalog(2, "7ISZ",    0xa7, 0x9d,   00,    0, 0x01 ),    /* 172 */
            new Catalog(2, "7ISZI",   0xa7, 0x9e,   00,    0, 0x01 ),    /* 173 */
            new Catalog(2, "7P<>S",   0xa7, 0x9f,   00,    0, 0x01 ),    /* 174 */
            new Catalog(2, "7SCI",    0xa7, 0xa4,   00,    0, 0x01 ),    /* 175 */

            new Catalog(2, "CREATE",  0xa7, 0x01,   00,    0, 0x01 ),    /* 176 */
            new Catalog(2, "DIR",     0xa7, 0x02,   00,    0, 0x01 ),    /* 177 */
            new Catalog(2, "NEWM",    0xa7, 0x03,   00, 0x03, 0x01 ),    /* 178 */
            new Catalog(2, "PURGE",   0xa7, 0x04,   00,    0, 0x01 ),    /* 179 */
            new Catalog(2, "READA",   0xa7, 0x05,   00,    0, 0x01 ),    /* 180 */
            new Catalog(2, "READK",   0xa7, 0x06,   00,    0, 0x01 ),    /* 181 */
            new Catalog(2, "READP",   0xa7, 0x07,   00,    0, 0x01 ),    /* 182 */
            new Catalog(2, "READR",   0xa7, 0x08,   00,    0, 0x01 ),    /* 183 */
            new Catalog(2, "READRX",  0xa7, 0x09,   00,    0, 0x01 ),    /* 184 */
            new Catalog(2, "READS",   0xa7, 0x0a,   00,    0, 0x01 ),    /* 185 */
            new Catalog(2, "READSUB", 0xa7, 0x0b,   00,    0, 0x01 ),    /* 186 */
            new Catalog(2, "RENAME",  0xa7, 0x0c,   00,    0, 0x01 ),    /* 187 */
            new Catalog(2, "SEC",     0xa7, 0x0d,   00,    0, 0x01 ),    /* 188 */
            new Catalog(2, "SEEKR",   0xa7, 0x0e,   00,    0, 0x01 ),    /* 189 */
            new Catalog(2, "UNSEC",   0xa7, 0x0f,   00,    0, 0x01 ),    /* 190 */
            new Catalog(2, "VERIFY",  0xa7, 0x10,   00,    0, 0x01 ),    /* 191 */
            new Catalog(2, "WRTA",    0xa7, 0x11,   00,    0, 0x01 ),    /* 192 */
            new Catalog(2, "WRTK",    0xa7, 0x12,   00,    0, 0x01 ),    /* 193 */
            new Catalog(2, "WRTP",    0xa7, 0x13,   00,    0, 0x01 ),    /* 194 */
            new Catalog(2, "WRTPV",   0xa7, 0x14,   00,    0, 0x01 ),    /* 195 */
            new Catalog(2, "WRTR",    0xa7, 0x15,   00,    0, 0x01 ),    /* 196 */
            new Catalog(2, "WRTRX",   0xa7, 0x16,   00,    0, 0x01 ),    /* 197 */
            new Catalog(2, "WRTS",    0xa7, 0x17,   00,    0, 0x01 ),    /* 198 */
            new Catalog(2, "ZERO",    0xa7, 0x18,   00,    0, 0x01 ),    /* 199 */

            new Catalog(2, "ALENG",   0xa6, 0x41,    0,    0, 0x01 ),    /* 200 */
            new Catalog(2, "ANUM",    0xa6, 0x42,    0,    0, 0x01 ),    /* 201 */
            new Catalog(2, "APPCHR",  0xa6, 0x43,    0,    0, 0x01 ),    /* 202 */
            new Catalog(2, "APPREC",  0xa6, 0x44,    0,    0, 0x01 ),    /* 203 */
            new Catalog(2, "ARCLREC", 0xa6, 0x45,    0,    0, 0x01 ),    /* 204 */
            new Catalog(2, "AROT",    0xa6, 0x46,    0,    0, 0x01 ),    /* 205 */
            new Catalog(2, "ATOX",    0xa6, 0x47,    0,    0, 0x01 ),    /* 206 */
            new Catalog(2, "CLFL",    0xa6, 0x48,    0,    0, 0x01 ),    /* 207 */
            new Catalog(2, "CLKEYS",  0xa6, 0x49,    0,    0, 0x01 ),    /* 208 */
            new Catalog(2, "CRFLAS",  0xa6, 0x4a,    0,    0, 0x01 ),    /* 209 */
            new Catalog(2, "CRFLD",   0xa6, 0x4b,    0,    0, 0x01 ),    /* 210 */
            new Catalog(2, "DELCHR",  0xa6, 0x4c,    0,    0, 0x01 ),    /* 211 */
            new Catalog(2, "DELREC",  0xa6, 0x4d,    0,    0, 0x01 ),    /* 212 */
            new Catalog(2, "EMDIR",   0xa6, 0x4e,    0,    0, 0x01 ),    /* 213 */
            new Catalog(2, "FLSIZE",  0xa6, 0x4f,    0,    0, 0x01 ),    /* 214 */
            new Catalog(2, "GETAS",   0xa6, 0x50,    0,    0, 0x01 ),    /* 215 */
            new Catalog(2, "GETP",    0xa6, 0x52,    0,    0, 0x01 ),    /* 216 */
            new Catalog(2, "GETR",    0xa6, 0x53,    0,    0, 0x01 ),    /* 217 */
            new Catalog(2, "GETREC",  0xa6, 0x54,    0,    0, 0x01 ),    /* 218 */
            new Catalog(2, "GETRX",   0xa6, 0x55,    0,    0, 0x01 ),    /* 219 */
            new Catalog(2, "GETSUB",  0xa6, 0x56,    0,    0, 0x01 ),    /* 220 */
            new Catalog(2, "GETX",    0xa6, 0x57,    0,    0, 0x01 ),    /* 221 */
            new Catalog(2, "INSCHR",  0xa6, 0x58,    0,    0, 0x01 ),    /* 222 */
            new Catalog(2, "INSREC",  0xa6, 0x59,    0,    0, 0x01 ),    /* 223 */
            new Catalog(2, "PASN",    0xa6, 0x5a,    0,    0, 0x01 ),    /* 224 */
            new Catalog(2, "PCLPS",   0xa6, 0x5b,    0,    0, 0x01 ),    /* 225 */
            new Catalog(2, "POSA",    0xa6, 0x5c,    0,    0, 0x01 ),    /* 226 */
            new Catalog(2, "POSFL",   0xa6, 0x5d,    0,    0, 0x01 ),    /* 227 */
            new Catalog(2, "PSIZE",   0xa6, 0x5e,    0,    0, 0x01 ),    /* 228 */
            new Catalog(2, "PURFL",   0xa6, 0x5f,    0,    0, 0x01 ),    /* 229 */
            new Catalog(2, "RCLFLAG", 0xa6, 0x60,    0,    0, 0x01 ),    /* 230 */
            new Catalog(2, "RCLPT",   0xa6, 0x61,    0,    0, 0x01 ),    /* 231 */
            new Catalog(2, "RCLPTA",  0xa6, 0x62,    0,    0, 0x01 ),    /* 232 */
            new Catalog(2, "REGMOVE", 0xa6, 0x63,    0,    0, 0x01 ),    /* 233 */
            new Catalog(2, "REGSWAP", 0xa6, 0x64,    0,    0, 0x01 ),    /* 234 */
            new Catalog(2, "SAVEAS",  0xa6, 0x65,    0,    0, 0x01 ),    /* 235 */
            new Catalog(2, "SAVEP",   0xa6, 0x66,    0,    0, 0x01 ),    /* 236 */
            new Catalog(2, "SAVER",   0xa6, 0x67,    0,    0, 0x01 ),    /* 237 */
            new Catalog(2, "SAVERX",  0xa6, 0x68,    0,    0, 0x01 ),    /* 238 */
            new Catalog(2, "SAVEX",   0xa6, 0x69,    0,    0, 0x01 ),    /* 239 */
            new Catalog(2, "SEEKPT",  0xa6, 0x6a,    0,    0, 0x01 ),    /* 240 */
            new Catalog(2, "SEEKPTA", 0xa6, 0x6b,    0,    0, 0x01 ),    /* 241 */
            new Catalog(2, "SIZE?",   0xa6, 0x6c,    0,    0, 0x01 ),    /* 242 */
            new Catalog(2, "STOFLAG", 0xa6, 0x6d,    0,    0, 0x01 ),    /* 243 */
            new Catalog(2, "X<>F",    0xa6, 0x6e,    0,    0, 0x01 ),    /* 244 */
            new Catalog(2, "XTOA",    0xa6, 0x6f,    0,    0, 0x01 ),    /* 245 */

            new Catalog(2, "ADATE",   0xa6, 0x81,    0,    0, 0x01 ),
            new Catalog(2, "ATIME",   0xa6, 0x84,    0,    0, 0x01 ),
            new Catalog(2, "ATIME24", 0xa6, 0x85,    0,    0, 0x01 ),
            new Catalog(2, "CLK12",   0xa6, 0x86,    0,    0, 0x01 ),
            new Catalog(2, "CLK24",   0xa6, 0x87,    0,    0, 0x01 ),
            new Catalog(2, "CLKT",    0xa6, 0x88,    0,    0, 0x01 ),
            new Catalog(2, "CLKTD",   0xa6, 0x89,    0,    0, 0x01 ),
            new Catalog(2, "CLOCK",   0xa6, 0x8a,    0,    0, 0x01 ),
            new Catalog(2, "CORRECT", 0xa6, 0x8b,    0,    0, 0x01 ),
            new Catalog(2, "DATE",    0xa6, 0x8c,    0,    0, 0x01 ),
            new Catalog(2, "DATE+",   0xa6, 0x8d,    0,    0, 0x01 ),
            new Catalog(2, "DDAYS",   0xa6, 0x8e,    0,    0, 0x01 ),
            new Catalog(2, "DMY",     0xa6, 0x8f,    0,    0, 0x01 ),
            new Catalog(2, "DOW",     0xa6, 0x90,    0,    0, 0x01 ),
            new Catalog(2, "MDY",     0xa6, 0x91,    0,    0, 0x01 ),
            new Catalog(2, "RCLAF",   0xa6, 0x92,    0,    0, 0x01 ),
            new Catalog(2, "RCLSW",   0xa6, 0x93,    0,    0, 0x01 ),
            new Catalog(2, "RUNSW",   0xa6, 0x94,    0,    0, 0x01 ),
            new Catalog(2, "SETAF",   0xa6, 0x95,    0,    0, 0x01 ),
            new Catalog(2, "SETDATE", 0xa6, 0x96,    0,    0, 0x01 ),
            new Catalog(2, "SETIME",  0xa6, 0x97,    0,    0, 0x01 ),
            new Catalog(2, "SETSW",   0xa6, 0x98,    0,    0, 0x01 ),
            new Catalog(2, "STOPSW",  0xa6, 0x99,    0,    0, 0x01 ),
            new Catalog(2, "SW",      0xa6, 0x9a,    0,    0, 0x01 ),
            new Catalog(2, "T+X",     0xa6, 0x9b,    0,    0, 0x01 ),
            new Catalog(2, "TIME",    0xa6, 0x9c,    0,    0, 0x01 ),

            new Catalog(0, "RCL 00",  0x20, 0x00,    0, 0x00, 0x01 ),
            new Catalog(0, "RCL 01",  0x21, 0x00,    0, 0x00, 0x01 ),
            new Catalog(0, "RCL 02",  0x22, 0x00,    0, 0x00, 0x01 ),
            new Catalog(0, "RCL 03",  0x23, 0x00,    0, 0x00, 0x01 ),
            new Catalog(0, "RCL 04",  0x24, 0x00,    0, 0x00, 0x01 ),
            new Catalog(0, "RCL 05",  0x25, 0x00,    0, 0x00, 0x01 ),
            new Catalog(0, "RCL 06",  0x26, 0x00,    0, 0x00, 0x01 ),
            new Catalog(0, "RCL 07",  0x27, 0x00,    0, 0x00, 0x01 ),
            new Catalog(0, "RCL 08",  0x28, 0x00,    0, 0x00, 0x01 ),
            new Catalog(0, "RCL 09",  0x29, 0x00,    0, 0x00, 0x01 ),
            new Catalog(0, "RCL 10",  0x2a, 0x00,    0, 0x00, 0x01 ),
            new Catalog(0, "RCL 11",  0x2b, 0x00,    0, 0x00, 0x01 ),
            new Catalog(0, "RCL 12",  0x2c, 0x00,    0, 0x00, 0x01 ),
            new Catalog(0, "RCL 13",  0x2d, 0x00,    0, 0x00, 0x01 ),
            new Catalog(0, "RCL 14",  0x2e, 0x00,    0, 0x00, 0x01 ),
            new Catalog(0, "RCL 15",  0x2f, 0x00,    0, 0x00, 0x01 ),
            new Catalog(0, "RCL",     0x90, 0x00,    0,    0, 0x02 ),
            new Catalog(0, "STO",     0x91, 0x00,    0,    0, 0x02 ),
            new Catalog(0, "XEQ",     0xe0, 0x00,    0, 0x00, 0x63 ),
            new Catalog(0, "XEQ",     0xe1, 0x00,    0, 0x00, 0x63 ),
            new Catalog(0, "XEQ",     0xe2, 0x00,    0, 0x00, 0x63 ),
            new Catalog(0, "XEQ",     0xe3, 0x00,    0, 0x00, 0x63 ),
            new Catalog(0, "XEQ",     0xe4, 0x00,    0, 0x00, 0x63 ),
            new Catalog(0, "XEQ",     0xe5, 0x00,    0, 0x00, 0x63 ),
            new Catalog(0, "XEQ",     0xe6, 0x00,    0, 0x00, 0x63 ),
            new Catalog(0, "XEQ",     0xe7, 0x00,    0, 0x00, 0x63 ),
            new Catalog(0, "XEQ",     0xe8, 0x00,    0, 0x00, 0x63 ),
            new Catalog(0, "XEQ",     0xe9, 0x00,    0, 0x00, 0x63 ),
            new Catalog(0, "XEQ",     0xea, 0x00,    0, 0x00, 0x63 ),
            new Catalog(0, "XEQ",     0xeb, 0x00,    0, 0x00, 0x63 ),
            new Catalog(0, "XEQ",     0xec, 0x00,    0, 0x00, 0x63 ),
            new Catalog(0, "XEQ",     0xed, 0x00,    0, 0x00, 0x63 ),
            new Catalog(0, "XEQ",     0xee, 0x00,    0, 0x00, 0x63 ),
            new Catalog(0, "XEQ",     0xef, 0x00,    0, 0x00, 0x63 ),

            new Catalog(0, "",        0x00, 0x00,   00, 0xff, 1 )
       };

        public Rev[] reverse = new Rev[]
        {
    new Rev( 0x00, 0x00, "",            0x01, 0x00,   0 ),
    new Rev( 0x01, 0x00,  "LBL 00",     0x01, 0x22,  69 ),
    new Rev( 0x02, 0x00,  "LBL 01",     0x01, 0x02,  69 ),
    new Rev( 0x03, 0x00,  "LBL 02",     0x01, 0x02,  69 ),
    new Rev( 0x04, 0x00,  "LBL 03",     0x01, 0x02,  69 ),
    new Rev( 0x05, 0x00,  "LBL 04",     0x01, 0x02,  69 ),
    new Rev( 0x06, 0x00,  "LBL 05",     0x01, 0x02,  69 ),
    new Rev( 0x07, 0x00,  "LBL 06",     0x01, 0x02,  69 ),
    new Rev( 0x08, 0x00,  "LBL 07",     0x01, 0x02,  69 ),
    new Rev( 0x09, 0x00,  "LBL 08",     0x01, 0x02,  69 ),
    new Rev( 0x0a, 0x00,  "LBL 09",     0x01, 0x02,  69 ),
    new Rev( 0x0b, 0x00,  "LBL 10",     0x01, 0x02,  69 ),
    new Rev( 0x0c, 0x00,  "LBL 11",     0x01, 0x02,  69 ),
    new Rev( 0x0d, 0x00,  "LBL 12",     0x01, 0x02,  69 ),
    new Rev( 0x0e, 0x00,  "LBL 13",     0x01, 0x02,  69 ),
    new Rev( 0x0f, 0x00,  "LBL 14",     0x01, 0x02,  69 ),
    new Rev( 0x10, 0x00,  "0",          0x01, 0x00,   0 ),
    new Rev( 0x11, 0x00,  "1",          0x01, 0x00,   1 ),
    new Rev( 0x12, 0x00,  "2",          0x01, 0x00,   2 ),
    new Rev( 0x13, 0x00,  "3",          0x01, 0x00,   3 ),
    new Rev( 0x14, 0x00,  "4",          0x01, 0x00,   4 ),
    new Rev( 0x15, 0x00,  "5",          0x01, 0x00,   5 ),
    new Rev( 0x16, 0x00,  "6",          0x01, 0x00,   6 ),
    new Rev( 0x17, 0x00,  "7",          0x01, 0x00,   7 ),
    new Rev( 0x18, 0x00,  "8",          0x01, 0x00,   8 ),
    new Rev( 0x19, 0x00,  "9",          0x01, 0x00,   9 ),
    new Rev( 0x1a, 0x00,  ".",          0x01, 0x00,  10 ),
    new Rev( 0x1b, 0x00,  "EEX",        0x01, 0x00,  11 ),
    new Rev( 0x1c, 0x00,  "CHS",        0x01, 0x00,  34 ),
    new Rev( 0x1d, 0x00,  "GTO",        0x13, 0x00,  61 ),
    new Rev( 0x1e, 0x00,  "XEQ",        0x13, 0x7b, 125 ),
    new Rev( 0x1f, 0x00,  "",           0x01, 0x00,   0 ),
    new Rev( 0x20, 0x00,  "RCL 00",     0x01, 0x0e,  89 ),
    new Rev( 0x21, 0x00,  "RCL 01",     0x01, 0x0e,  89 ),
    new Rev( 0x22, 0x00,  "RCL 02",     0x01, 0x0e,  89 ),
    new Rev( 0x23, 0x00,  "RCL 03",     0x01, 0x0e,  89 ),
    new Rev( 0x24, 0x00,  "RCL 04",     0x01, 0x0e,  89 ),
    new Rev( 0x25, 0x00,  "RCL 05",     0x01, 0x0e,  89 ),
    new Rev( 0x26, 0x00,  "RCL 06",     0x01, 0x0e,  89 ),
    new Rev( 0x27, 0x00,  "RCL 07",     0x01, 0x0e,  89 ),
    new Rev( 0x28, 0x00,  "RCL 08",     0x01, 0x0e,  89 ),
    new Rev( 0x29, 0x00,  "RCL 09",     0x01, 0x0e,  89 ),
    new Rev( 0x2a, 0x00,  "RCL 10",     0x01, 0x0e,  89 ),
    new Rev( 0x2b, 0x00,  "RCL 11",     0x01, 0x0e,  89 ),
    new Rev( 0x2c, 0x00,  "RCL 12",     0x01, 0x0e,  89 ),
    new Rev( 0x2d, 0x00,  "RCL 13",     0x01, 0x0e,  89 ),
    new Rev( 0x2e, 0x00,  "RCL 14",     0x01, 0x0e,  89 ),
    new Rev( 0x2f, 0x00,  "RCL 15",     0x01, 0x0e,  89 ),
    new Rev( 0x30, 0x00,  "STO 00",     0x01, 0x0e, 108 ),
    new Rev( 0x31, 0x00,  "STO 01",     0x01, 0x0e, 108 ),
    new Rev( 0x32, 0x00,  "STO 02",     0x01, 0x0e, 108 ),
    new Rev( 0x33, 0x00,  "STO 03",     0x01, 0x0e, 108 ),
    new Rev( 0x34, 0x00,  "STO 04",     0x01, 0x0e, 108 ),
    new Rev( 0x35, 0x00,  "STO 05",     0x01, 0x0e, 108 ),
    new Rev( 0x36, 0x00,  "STO 06",     0x01, 0x0e, 108 ),
    new Rev( 0x37, 0x00,  "STO 07",     0x01, 0x0e, 108 ),
    new Rev( 0x38, 0x00,  "STO 08",     0x01, 0x0e, 108 ),
    new Rev( 0x39, 0x00,  "STO 09",     0x01, 0x0e, 108 ),
    new Rev( 0x3a, 0x00,  "STO 10",     0x01, 0x0e, 108 ),
    new Rev( 0x3b, 0x00,  "STO 11",     0x01, 0x0e, 108 ),
    new Rev( 0x3c, 0x00,  "STO 12",     0x01, 0x0e, 108 ),
    new Rev( 0x3d, 0x00,  "STO 13",     0x01, 0x0e, 108 ),
    new Rev( 0x3e, 0x00,  "STO 14",     0x01, 0x0e, 108 ),
    new Rev( 0x3f, 0x00,  "STO 15",     0x01, 0x0e, 108 ),
    new Rev( 0x40, 0x00,  "+",          0x01, 0x00,  12 ),
    new Rev( 0x41, 0x00,  "-",          0x01, 0x00,  13 ),
    new Rev( 0x42, 0x00,  "*",          0x01, 0x00,  14 ),
    new Rev( 0x43, 0x00,  "/",          0x01, 0x00,  15 ),
    new Rev( 0x44, 0x00,  "X<Y?",       0x01, 0x00, 120 ),
    new Rev( 0x45, 0x00,  "X>Y?",       0x01, 0x00, 122 ),
    new Rev( 0x46, 0x00,  "X<=Y?",      0x01, 0x00, 121 ),
    new Rev( 0x47, 0x00,  "~+",         0x01, 0x00,  96 ),
    new Rev( 0x48, 0x00,  "~-",         0x01, 0x00,  97 ),
    new Rev( 0x49, 0x00,  "HMS+",       0x01, 0x00,  63 ),
    new Rev( 0x4a, 0x00,  "HMS-",       0x01, 0x00,  64 ),
    new Rev( 0x4b, 0x00,  "MOD",        0x01, 0x00,  74 ),
    new Rev( 0x4c, 0x00,  "%",          0x01, 0x00,  80 ),
    new Rev( 0x4d, 0x00,  "%CH",        0x01, 0x00,  81 ),
    new Rev( 0x4e, 0x00,  "P-R",        0x01, 0x00,  78 ),
    new Rev( 0x4f, 0x00,  "R-P",        0x01, 0x00,  87 ),
    new Rev( 0x50, 0x00,  "LN",         0x01, 0x00,  71 ),
    new Rev( 0x51, 0x00,  "X^2",        0x01, 0x00, 126 ),
    new Rev( 0x52, 0x00,  "SQRT",       0x01, 0x00, 102 ),
    new Rev( 0x53, 0x00,  "Y^X",        0x01, 0x00, 127 ),
    new Rev( 0x54, 0x00,  "CHS",        0x01, 0x00,  34 ),
    new Rev( 0x55, 0x00,  "E^X",        0x01, 0x00,  48 ),
    new Rev( 0x56, 0x00,  "LOG",        0x01, 0x00,  70 ),
    new Rev( 0x57, 0x00,  "10^X",       0x01, 0x00,  17 ),
    new Rev( 0x58, 0x00,  "E^X-1",      0x01, 0x00,  49 ),
    new Rev( 0x59, 0x00,  "SIN",        0x01, 0x00,  99 ),
    new Rev( 0x5a, 0x00,  "COS",        0x01, 0x00,  42 ),
    new Rev( 0x5b, 0x00,  "TAN",        0x01, 0x00, 110 ),
    new Rev( 0x5c, 0x00,  "ASIN",       0x01, 0x00,  25 ),
    new Rev( 0x5d, 0x00,  "ACOS",       0x01, 0x00,  19 ),
    new Rev( 0x5e, 0x00,  "ATAN",       0x01, 0x00,  28 ),
    new Rev( 0x5f, 0x00,  "DEC",        0x01, 0x00,  44 ),
    new Rev( 0x60, 0x00,  "1/X",        0x01, 0x00,  16 ),
    new Rev( 0x61, 0x00,  "ABS",        0x01, 0x00,  18 ),
    new Rev( 0x62, 0x00,  "FACT",       0x01, 0x00,  53 ),
    new Rev( 0x63, 0x00,  "X<>0?",      0x01, 0x00, 114 ),
    new Rev( 0x64, 0x00,  "X>0?",       0x01, 0x00, 117 ),
    new Rev( 0x65, 0x00,  "LN1+X",      0x01, 0x00,  72 ),
    new Rev( 0x66, 0x00,  "X<0?",       0x01, 0x00, 115 ),
    new Rev( 0x67, 0x00,  "X=0?",       0x01, 0x00, 113 ),
    new Rev( 0x68, 0x00,  "INT",        0x01, 0x00,  66 ),
    new Rev( 0x69, 0x00,  "FRC",        0x01, 0x00,  57 ),
    new Rev( 0x6a, 0x00,  "D-R",        0x01, 0x00,  43 ),
    new Rev( 0x6b, 0x00,  "R-D",        0x01, 0x00,  86 ),
    new Rev( 0x6c, 0x00,  "HMS",        0x01, 0x00,  62 ),
    new Rev( 0x6d, 0x00,  "HR",         0x01, 0x00,  65 ),
    new Rev( 0x6e, 0x00,  "RND",        0x01, 0x00,  91 ),
    new Rev( 0x6f, 0x00,  "OCT",        0x01, 0x00,  75 ),
    new Rev( 0x70, 0x00,  "CL~",        0x01, 0x00,  37 ),
    new Rev( 0x71, 0x00,  "X<>Y",       0x01, 0x00, 124 ),
    new Rev( 0x72, 0x00,  "PI",         0x01, 0x00,  82 ),
    new Rev( 0x73, 0x00,  "CLST",       0x01, 0x00,  40 ),
    new Rev( 0x74, 0x00,  "R^",         0x01, 0x00,  85 ),
    new Rev( 0x75, 0x00,  "RDN",        0x01, 0x00,  90 ),
    new Rev( 0x76, 0x00,  "LASTX",      0x01, 0x00,  68 ),
    new Rev( 0x77, 0x00,  "CLX",        0x01, 0x00,  41 ),
    new Rev( 0x78, 0x00,  "X=Y?",       0x01, 0x00, 118 ),
    new Rev( 0x79, 0x00,  "X<>Y?",      0x01, 0x00, 119 ),
    new Rev( 0x7a, 0x00,  "SIGN",       0x01, 0x00, 100 ),
    new Rev( 0x7b, 0x00,  "X<=0?",      0x01, 0x00, 116 ),
    new Rev( 0x7c, 0x00,  "MEAN",       0x01, 0x00,  73 ),
    new Rev( 0x7d, 0x00,  "SDEV",       0x01, 0x00,  93 ),
    new Rev( 0x7e, 0x00,  "AVIEW",      0x01, 0x00,  29 ),
    new Rev( 0x7f, 0x00,  "CLD",        0x01, 0x00,  36 ),
    new Rev( 0x80, 0x00,  "DEG",        0x01, 0x00,  45 ),
    new Rev( 0x81, 0x00,  "RAD",        0x01, 0x00,  88 ),
    new Rev( 0x82, 0x00,  "GRAD",       0x01, 0x00,  60 ),
    new Rev( 0x83, 0x00,  "ENTER^",     0x01, 0x00,  52 ),
    new Rev( 0x84, 0x00,  "STOP",       0x01, 0x00, 109 ),
    new Rev( 0x85, 0x00,  "RTN",        0x01, 0x00,  92 ),
    new Rev( 0x86, 0x00,  "BEEP",       0x01, 0x00,  30 ),
    new Rev( 0x87, 0x00,  "CLA",        0x01, 0x00,  35 ),
    new Rev( 0x88, 0x00,  "ASHF",       0x01, 0x00,  24 ),
    new Rev( 0x89, 0x00,  "PSE",        0x01, 0x00,  84 ),
    new Rev( 0x8a, 0x00,  "CLRG",       0x01, 0x00,  39 ),
    new Rev( 0x8b, 0x00,  "AOFF",       0x01, 0x00,  21 ),
    new Rev( 0x8c, 0x00,  "AON",        0x01, 0x00,  22 ),
    new Rev( 0x8d, 0x00,  "OFF",        0x01, 0x00,  76 ),
    new Rev( 0x8e, 0x00,  "PROMPT",     0x01, 0x00,  83 ),
    new Rev( 0x8f, 0x00,  "ADV",        0x01, 0x00,  20 ),
    new Rev( 0x90, 0x00,  "RCL",        0x02, 0x0e,  89 ),
    new Rev( 0x91, 0x00,  "STO",        0x02, 0x0e, 108 ),
    new Rev( 0x92, 0x00,  "ST+",        0x02, 0x0e, 104 ),
    new Rev( 0x93, 0x00,  "ST-",        0x02, 0x0e, 105 ),
    new Rev( 0x94, 0x00,  "ST*",        0x02, 0x0e, 106 ),
    new Rev( 0x95, 0x00,  "ST/",        0x02, 0x0e, 107 ),
    new Rev( 0x96, 0x00,  "ISG",        0x02, 0x0e,  67 ),
    new Rev( 0x97, 0x00,  "DSE",        0x02, 0x0e,  47 ),
    new Rev( 0x98, 0x00,  "VIEW",       0x02, 0x0e, 112 ),
    new Rev( 0x99, 0x00,  "~REG",       0x02, 0x0e,  98 ),
    new Rev( 0x9a, 0x00,  "ASTO",       0x02, 0x0e,  27 ),
    new Rev( 0x9b, 0x00,  "ARCL",       0x02, 0x0e,  23 ),
    new Rev( 0x9c, 0x00,  "FIX",        0x82, 0x0d,  56 ),
    new Rev( 0x9d, 0x00,  "SCI",        0x82, 0x0d,  94 ),
    new Rev( 0x9e, 0x00,  "ENG",        0x82, 0x0d,  51 ),
    new Rev( 0x9f, 0x00,  "TONE",       0x82, 0x0d, 111 ),
    new Rev( 0xa0, 0x00,  "XROM",       0x22, 0x00,   0 ),
    new Rev( 0xa1, 0x00,  "XROM",       0x22, 0x00,   0 ),
    new Rev( 0xa2, 0x00,  "XROM",       0x22, 0x00,   0 ),
    new Rev( 0xa3, 0x00,  "XROM",       0x22, 0x00,   0 ),
    new Rev( 0xa4, 0x00,  "XROM",       0x22, 0x00,   0 ),
    new Rev( 0xa5, 0x00,  "XROM",       0x22, 0x00,   0 ),
    new Rev( 0xa6, 0x00,  "XROM",       0x22, 0x00,   0 ),
    new Rev( 0xa7, 0x00,  "XROM",       0x22, 0x00,   0 ),
    new Rev( 0xa8, 0x00,  "SF",         0x02, 0x0e,  95 ),
    new Rev( 0xa9, 0x00,  "CF",         0x02, 0x0e,  33 ),
    new Rev( 0xaa, 0x00,  "FS?C",       0x02, 0x0e,  59 ),
    new Rev( 0xab, 0x00,  "FC?C",       0x02, 0x0e,  55 ),
    new Rev( 0xac, 0x00,  "FS?",        0x02, 0x0e,  58 ),
    new Rev( 0xad, 0x00,  "FC?",        0x02, 0x0e,  54 ),
    new Rev( 0xae, 0x00,  "GTO",        0x32, 0x90,  61 ),
    new Rev( 0xaf, 0x00,  "",           0x02, 0x00,   0 ),
    new Rev( 0xb0, 0x00,  "",           0x01, 0x00,   0 ),
    new Rev( 0xb1, 0x00,  "GTO 00",     0x42, 0x5b,  61 ),
    new Rev( 0xb2, 0x00,  "GTO 01",     0x42, 0x00,  61 ),
    new Rev( 0xb3, 0x00,  "GTO 02",     0x42, 0x00,  61 ),
    new Rev( 0xb4, 0x00,  "GTO 03",     0x42, 0x00,  61 ),
    new Rev( 0xb5, 0x00,  "GTO 04",     0x42, 0x00,  61 ),
    new Rev( 0xb6, 0x00,  "GTO 05",     0x42, 0x00,  61 ),
    new Rev( 0xb7, 0x00,  "GTO 06",     0x42, 0x00,  61 ),
    new Rev( 0xb8, 0x00,  "GTO 07",     0x42, 0x00,  61 ),
    new Rev( 0xb9, 0x00,  "GTO 08",     0x42, 0x00,  61 ),
    new Rev( 0xba, 0x00,  "GTO 09",     0x42, 0x00,  61 ),
    new Rev( 0xbb, 0x00,  "GTO 10",     0x42, 0x00,  61 ),
    new Rev( 0xbc, 0x00,  "GTO 11",     0x42, 0x00,  61 ),
    new Rev( 0xbd, 0x00,  "GTO 12",     0x42, 0x00,  61 ),
    new Rev( 0xbe, 0x00,  "GTO 13",     0x42, 0x00,  61 ),
    new Rev( 0xbf, 0x00,  "GTO 14",     0x42, 0x00,  61 ),
    new Rev( 0xc0, 0x00,  "GLOBAL",     0x53, 0xc0,   0 ),
    new Rev( 0xc1, 0x00,  "GLOBAL",     0x53, 0x00,   0 ),
    new Rev( 0xc2, 0x00,  "GLOBAL",     0x53, 0x00,   0 ),
    new Rev( 0xc3, 0x00,  "GLOBAL",     0x53, 0x00,   0 ),
    new Rev( 0xc4, 0x00,  "GLOBAL",     0x53, 0x00,   0 ),
    new Rev( 0xc5, 0x00,  "GLOBAL",     0x53, 0x00,   0 ),
    new Rev( 0xc6, 0x00,  "GLOBAL",     0x53, 0x00,   0 ),
    new Rev( 0xc7, 0x00,  "GLOBAL",     0x53, 0x00,   0 ),
    new Rev( 0xc8, 0x00,  "GLOBAL",     0x53, 0x00,   0 ),
    new Rev( 0xc9, 0x00,  "GLOBAL",     0x53, 0x00,   0 ),
    new Rev( 0xca, 0x00,  "GLOBAL",     0x53, 0x00,   0 ),
    new Rev( 0xcb, 0x00,  "GLOBAL",     0x53, 0x00,   0 ),
    new Rev( 0xcc, 0x00,  "GLOBAL",     0x53, 0x00,   0 ),
    new Rev( 0xcd, 0x00,  "GLOBAL",     0x53, 0x00,   0 ),
    new Rev( 0xce, 0x00,  "X<>",        0x02, 0x0e, 123 ),
    new Rev( 0xcf, 0x00,  "LBL",        0x92, 0x00,  69 ),
    new Rev( 0xd0, 0x00,  "GTO",        0x63, 0x00,  61 ),
    new Rev( 0xd1, 0x00,  "GTO",        0x63, 0x00,  61 ),
    new Rev( 0xd2, 0x00,  "GTO",        0x63, 0x00,  61 ),
    new Rev( 0xd3, 0x00,  "GTO",        0x63, 0x00,  61 ),
    new Rev( 0xd4, 0x00,  "GTO",        0x63, 0x00,  61 ),
    new Rev( 0xd5, 0x00,  "GTO",        0x63, 0x00,  61 ),
    new Rev( 0xd6, 0x00,  "GTO",        0x63, 0x00,  61 ),
    new Rev( 0xd7, 0x00,  "GTO",        0x63, 0x00,  61 ),
    new Rev( 0xd8, 0x00,  "GTO",        0x63, 0x00,  61 ),
    new Rev( 0xd9, 0x00,  "GTO",        0x63, 0x00,  61 ),
    new Rev( 0xda, 0x00,  "GTO",        0x63, 0x00,  61 ),
    new Rev( 0xdb, 0x00,  "GTO",        0x63, 0x00,  61 ),
    new Rev( 0xdc, 0x00,  "GTO",        0x63, 0x00,  61 ),
    new Rev( 0xdd, 0x00,  "GTO",        0x63, 0x00,  61 ),
    new Rev( 0xde, 0x00,  "GTO",        0x63, 0x00,  61 ),
    new Rev( 0xdf, 0x00,  "GTO",        0x63, 0x00,  61 ),
    new Rev( 0xe0, 0x00,  "XEQ",        0x63, 0x00, 125 ),
    new Rev( 0xe1, 0x00,  "XEQ",        0x63, 0x00, 125 ),
    new Rev( 0xe2, 0x00,  "XEQ",        0x63, 0x00, 125 ),
    new Rev( 0xe3, 0x00,  "XEQ",        0x63, 0x00, 125 ),
    new Rev( 0xe4, 0x00,  "XEQ",        0x63, 0x00, 125 ),
    new Rev( 0xe5, 0x00,  "XEQ",        0x63, 0x00, 125 ),
    new Rev( 0xe6, 0x00,  "XEQ",        0x63, 0x00, 125 ),
    new Rev( 0xe7, 0x00,  "XEQ",        0x63, 0x00, 125 ),
    new Rev( 0xe8, 0x00,  "XEQ",        0x63, 0x00, 125 ),
    new Rev( 0xe9, 0x00,  "XEQ",        0x63, 0x00, 125 ),
    new Rev( 0xea, 0x00,  "XEQ",        0x63, 0x00, 125 ),
    new Rev( 0xeb, 0x00,  "XEQ",        0x63, 0x00, 125 ),
    new Rev( 0xec, 0x00,  "XEQ",        0x63, 0x00, 125 ),
    new Rev( 0xed, 0x00,  "XEQ",        0x63, 0x00, 125 ),
    new Rev( 0xee, 0x00,  "XEQ",        0x63, 0x00, 125 ),
    new Rev( 0xef, 0x00,  "XEQ",        0x63, 0x00, 125 ),
    new Rev( 0xf0, 0x00,  "",           0x72, 0x00,   0 ),
    new Rev( 0xf2, 0x00,  "",           0x73, 0x00,   0 ),
    new Rev( 0xf3, 0x00,  "",           0x74, 0x00,   0 ),
    new Rev( 0xf4, 0x00,  "",           0x75, 0x00,   0 ),
    new Rev( 0xf5, 0x00,  "",           0x76, 0x00,   0 ),
    new Rev( 0xf6, 0x00,  "",           0x77, 0x00,   0 ),
    new Rev( 0xf7, 0x00,  "",           0x78, 0x00,   0 ),
    new Rev( 0xf8, 0x00,  "",           0x79, 0x00,   0 ),
    new Rev( 0xf9, 0x00,  "",           0x7a, 0x00,   0 ),
    new Rev( 0xfa, 0x00,  "",           0x7b, 0x00,   0 ),
    new Rev( 0xfb, 0x00,  "",           0x7c, 0x00,   0 ),
    new Rev( 0xfc, 0x00,  "",           0x7d, 0x00,   0 ),
    new Rev( 0xfd, 0x00,  "",           0x7e, 0x00,   0 ),
    new Rev( 0xfe, 0x00,  "",           0x7f, 0x00,   0 ),
          new Rev( 0xff, 0x00,  "",           0x70, 0x00,   0 )
        };

        
        Key[] keys = new Key[]
        {
            new Key("01",     96,  97,  0x01, 0x09, 36, (byte)'A', (byte)'a' ),      // E+     E-
            new Key("11",     16, 127,  0x11, 0x19, 28, (byte)'B', (byte)'b' ),      // 1/X    Y^X
            new Key("21",    102, 126,  0x21, 0x29, 20, (byte)'C', (byte)'c' ),      // SQRT   X^2
            new Key("31",     70,  17,  0x31, 0x39, 12, (byte)'D', (byte)'d' ),      // LOG    10^X
            new Key("41",     71,  48,  0x41, 0x49,  4, (byte)'E', (byte)'e' ),      // LN     E^X
            new Key("02",    124,  37,  0x02, 0x0a, 35, (byte)'F',       126 ),      // X<>Y   CLE
            new Key("12",     90,  80,  0x12, 0x1a, 27, (byte)'G', (byte)'%' ),      // RDN    %
            new Key("22",     99,  25,  0x22, 0x2a, 19, (byte)'H',        29 ),      // SIN    ASIN
            new Key("32",     42,  19,  0x32, 0x3a, 11, (byte)'I', (byte)'<' ),      // COS    ACOS
            new Key("42",    110,  28,  0x42, 0x4a,  3, (byte)'J', (byte)'>' ),      // TAN    ATAN
            new Key("13",    125,  26,  0x13, 0x1b, 26, (byte)'K',      0x81 ),      // XEQ    ASN             append
            new Key("23",    108,  69,  0x23, 0x2b, 18, (byte)'L',      0x82 ),      // STO    LBL             asto
            new Key("33",     89,  61,  0x33, 0x3b, 10, (byte)'M',      0x83 ),      // RCL    GTO             arcl
            new Key("43",    103,  31,  0x43, 0x4b,  2,      0x84,      0x85 ),      // SST    BST     sst     bst
            new Key("04",     52,  32,  0x04, 0x0c, 33, (byte)'N', (byte)'^' ),      // ENTER^ CAT
            new Key("24",     34,  67,  0x24, 0x2c, 17, (byte)'O',        13 ),      // CHS    ISG
            new Key("34",     11,  92,  0x34, 0x3c,  9, (byte)'P', (byte)'$' ),      // EEX    RTN
            new Key("44",    999,  41,  0x44, 0x4c,  1,      0x85,      0x86 ),      // BS     CLX     bs      cla
            new Key("05",     13, 118,  0x05, 0x0d, 32, (byte)'Q', (byte)'-' ),      // -      X=Y?
            new Key("15",      7,  95,  0x15, 0x1d, 24, (byte)'R', (byte)'7' ),      // 7      SF
            new Key("25",      8,  33,  0x25, 0x2d, 16, (byte)'S', (byte)'8' ),      // 8      CF
            new Key("35",      9,  58,  0x35, 0x3d,  8, (byte)'T', (byte)'9' ),      // 9      FS?
            new Key("06",     12, 121,  0x06, 0x0e, 31, (byte)'U', (byte)'+' ),      // +      X<=Y?
            new Key("16",      4,  30,  0x16, 0x1e, 23, (byte)'V', (byte)'4' ),      // 4      BEEP
            new Key("26",      5,  78,  0x26, 0x2e, 15, (byte)'W', (byte)'5' ),      // 5      P-R
            new Key("36",      6,  87,  0x36, 0x3e,  7, (byte)'X', (byte)'6' ),      // 6      R-P
            new Key("07",     14, 122,  0x07, 0x0f, 30, (byte)'Y', (byte)'*' ),      // *      X>Y?
            new Key("17",      1,  56,  0x17, 0x1f, 22, (byte)'Z', (byte)'1' ),      // 1      FIX
            new Key("27",      2,  94,  0x27, 0x2f, 14, (byte)'=', (byte)'2' ),      // 2      SCI
            new Key("37",      3,  51,  0x37, 0x3f,  6, (byte)'?', (byte)'3' ),      // 3      ENG
            new Key("08",     15, 113,  0x08, 0x00, 29, (byte)':', (byte)'/' ),      // /      X=0?
            new Key("18",      0,  82,  0x18, 0x10, 21, (byte)' ', (byte)'0' ),      // 0      PI
            new Key("28",     10,  68,  0x28, 0x20, 13, (byte)',', (byte)'.' ),      // .      LASTX
            new Key("38",    109, 112,  0x38, 0x30,  5,      0x87,      0x88 ),      // R/S    VIEW    rs      aview
            new Key("",      255, 255,  0xff, 0xff,  0,         0,         0 )
        };
       

        public string display;
        public bool errFlag;
        public byte[] ram;
        public bool running;
        public bool on;
        public Number ZERO;
        public Number S_PI;
        public Number S_PI2;
        public Number S_ONE;
        public Number S_TEN;
        public Number S_NEGONE;
        public Number S_E;
        public Number DTOR;
        public Number RTOD;
        public Number GTOR;
        public Number RTOG;
        public Number LOGE;
        public Number ILOGE;
        public DNumber D_ZERO;
        public DNumber D_HALF;
        public DNumber D_ONE;
        public DNumber D_TWO;
        public DNumber D_PI2;
        public bool lift;
        public int dp;
        public int ps;
        public int ex;
        public bool debug;
        public Number a;
        public Number b;
        public Number c;
        public int byteCount;
        public bool linksCleared;
        public Form1 window;
        public Sound sound;
        public int calculatorMode;

        public Printer printer { get; set; }
        public TapeDrive tapeDrive { get; set; }
        public CardReader cardReader { get; set; }
        public Extended extended { get; set; }
        public TimeModule timeModule { get; set; }
        public int pendingKey;
        public int uiMode;
        public string goose;
        public int alphaPos;
        public bool catPause;
        public int pauseCount;
        public int catSize;
        public bool dirMode;

        public Cpu(Form1 w)
        {
            int i;
            this.window = w;
            dirMode = false;
            calculatorMode = CM_DIRECT;
            ram = new byte[3584];
            ZERO     = new Number(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
            S_ONE    = new Number(0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
            S_TEN    = new Number(0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1);
            S_NEGONE = new Number(9, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
            S_PI     = new Number(0, 3, 1, 4, 1, 5, 9, 2, 6, 5, 3, 0, 0, 0);
            S_E      = new Number(0, 2, 7, 1, 8, 2, 8, 1, 8, 2, 8, 0, 0, 0);

            S_PI2 = new Number(0, 1, 5, 7, 0, 7, 9, 6, 3, 2, 6, 0, 0, 0);
            DTOR = new Number(0, 1, 7, 4, 5, 3, 2, 9, 2, 5, 1, 9, 0, 2);
            RTOD = new Number(0, 5, 7, 2, 9, 5, 7, 7, 9, 5, 1, 0, 0, 1);
            GTOR = new Number(0, 1, 5, 7, 0, 7, 9, 6, 3, 2, 6, 9, 0, 2);
            RTOG = new Number(0, 6, 3, 6, 6, 1, 9, 7, 7, 2, 3, 0, 0, 1);
            LOGE = new Number(0, 4, 3, 4, 2, 9, 4, 4, 8, 0, 0, 9, 0, 1);
            ILOGE = new Number(0, 2, 3, 0, 2, 5, 8, 5, 0, 9, 2, 0, 0, 0);

            D_ZERO = new DNumber(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
            D_HALF = new DNumber(0, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 9, 0, 0, 1);
            D_ONE = new DNumber(0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
            D_TWO = new DNumber(0, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
            D_PI2 = new DNumber(0, 1, 5, 7, 0, 7, 9, 6, 3, 2, 6, 7, 9, 4, 8, 9, 6, 6, 1, 9, 2, 0, 0, 0, 0);

            CMD_PRP = FindCatalogEntry("PRP", 2);
            CMD_LIST = FindCatalogEntry("LIST", 2);
            CMD_NEWM = FindCatalogEntry("NEWM", 2);
            CMD_LBL_A = FindCatalogEntry("LBL", 0);
            CMD_XEQ_A = FindCatalogEntry("XEQ", 0);
            CMD_GTO_ALPHA = FindCatalogEntry("GTO", 0);
            CMD_GTO_DOT = CMD_GTO_ALPHA + 1;
            CMD_GTO_A = CMD_GTO_DOT + 2;

            sound = new Sound();

            a = new Number();
            b = new Number();
            c = new Number();
            on = true;
            pendingKey = -1;
            goose = "\x81           ";
            uiMode = 0;
            catPause = false;
            extended = new Extended(this, w);
            printer = new Printer(this, w);
            tapeDrive = new TapeDrive(this, w);
            cardReader = new CardReader(this, w);
            timeModule = new TimeModule(this, w);

            Init();
            if (Load() == false)
            {
                Message("MEMORY LOST");
            }
            SetFlag(F_AUDIO);
            SetFlag(F_PREN);
            ClearFlag(F_ALPHA);
            ClearFlag(F_ALPHA_IN);
            ClearFlag(F_PRGM);
            for (i = 11; i <= 14; i++) ClearFlag((byte)i);
            for (i = 22; i <= 25; i++) ClearFlag((byte)i);
 //           Annunciators();
            window.Display(Display(),true);
            window.DisplayTimerEnabled(true);
        }

        public int FindCommand(byte b1, byte b2)
        {
            int i = 0;
            while (i < catalog.Length)
            {
                if (catalog[i].cmd == b1 && catalog[i].post == b2) return i;
                i++;
            }
            return -1;
        }
        public int FindCatalogEntry(string name,int cat)
        {
            int i = 0;
            while (catalog[i].flags != 0xff)
            {
                if (catalog[i].catalog == cat && catalog[i].name.Equals(name)) return i;
                i++;
            }
            return -1;
        }

        public void AssignKey(byte b1, byte b2, byte keycode)
        {
            int i;
            int addr;
            bool flag;
            UnAsn(keycode, 3);
            SetKaFlag(keycode, true);
            addr = 0x0c0 * 7;
            flag = true;
            while (flag)
            {
                if (ram[addr + 6] != 0xf0) flag = false;
                if (ram[addr + 5] == 0x00) flag = false;
                if (ram[addr + 2] == 0x00) flag = false;
                if (ram[addr + 0] == keycode) flag = false;
                if (ram[addr + 3] == keycode) flag = false;
                if (flag) addr += 7;
            }
            if (ram[addr+6] == 0xf0)
            {
                if (ram[addr+0] == keycode)
                {
                    ram[addr + 2] = b1;
                    ram[addr + 1] = b2;
                }
                else if (ram[addr + 3] == keycode)
                {
                    ram[addr + 5] = b1;
                    ram[addr + 4] = b2;
                }
                else if (ram[addr + 2] == 0x00)
                {
                    ram[addr + 2] = b1;
                    ram[addr + 1] = b2;
                    ram[addr + 0] = keycode;
                }
                else if (ram[addr + 5] == 0x00)
                {
                    ram[addr + 5] = b1;
                    ram[addr + 4] = b2;
                    ram[addr + 3] = keycode;
                }
            }
            else
            {
                for (i = 0; i <= 6; i++) ram[addr + i] = 0x00;
                ram[addr + 6] = 0xf0;
                ram[addr + 2] = b1;
                ram[addr + 1] = b2;
                ram[addr + 0] = keycode;
            }
        }

        public void AppendAlpha(byte c)
        {
            int i;
            for (i = Cpu.REG_P + 2; i > Cpu.REG_M; i--) ram[i] = ram[i - 1];
            ram[Cpu.REG_M] = c;
        }

        // ***********************************************************
        // ***** This function finds the bottom of unused memory *****
        // ***** returned as register number                     *****
        // ***********************************************************
        public int FindBottom()
        {
            int reg;
            reg = 0x0c0;
            while (ram[reg * 7 + 6] == 0xf0) reg++;
            return reg;
        }

        public void OpenTapeDrive(string filename)
        {
            tapeDrive.OpenTapeDrive(filename);
        }

        public void RamClear()
        {
            pendingKey = -1;
            uiMode = 0;
            Init();
            Message("MEMORY LOST");
            Annunciators();
            window.Display(Display(), true);
        }

        public void Reset()
        {
            uiMode = 0;
            ClearFlag(F_PRGM);
            ClearFlag(F_NUM_IN);
            ClearFlag(F_ALPHA_IN);
            running = false;
            Annunciators();
            window.Display(Display(), true);
        }

        void Init()
        {
            int mno;
            int pqr;
            int i;
            running = false;
            on = true;
            for (i = 0; i < RAMSIZE; i++)
                ram[i] = 0;
            mno = 0x0c0 + 46;
            pqr = mno - 1;
            ram[REG_B + 0] = (byte)(mno & 0xff);
            ram[REG_B + 1] = (byte)(0x00 | ((mno >> 8) & 0xf));
            ram[REG_C + 0] = (byte)(pqr & 0xff);
            ram[REG_C + 1] = (byte)(((pqr >> 8) & 0xf) | ((mno & 0xf) << 4));
            ram[REG_C + 2] = (byte)((mno >> 4) & 0xff);
            ram[REG_C + 3] = 0x69;
            ram[REG_C + 4] = 0x01;
            ram[REG_C + 5] = 0xb0;
            ram[REG_C + 6] = 0x00;
            ram[pqr * 7 + 0] = 0x29;
            ram[pqr * 7 + 1] = 0x00;
            ram[pqr * 7 + 2] = 0xc0;
            SetFlag(28);
            SetFlag(29);
            SetFlag(37);
            SetFlag(40);
            SetFlag(F_PRT_EXIST);
            SetFlag(21);
            ZERO.sign = 0;
            ZERO.esign = 0;
            for (i = 0; i < 10; i++) ZERO.mantissa[i] = 0;
            for (i = 0; i < 2; i++) ZERO.exponent[i] = 0;
            extended.RamClear();
        }

        public bool Load()
        {
            if (File.Exists("rc41.ram"))
            {
                BinaryReader file = new BinaryReader(File.Open("rc41.ram", FileMode.Open));
                file.Read(ram);
                extended.Load(file);
                timeModule.Load(file);
                file.Close();
                return true;
            }
            return false;
        }

        public void Save()
        {
            BinaryWriter file = new BinaryWriter(File.Open("rc41.ram",FileMode.OpenOrCreate));
            file.Write(ram);
            extended.Save(file);
            timeModule.Save(file);
            file.Close();
        }

        public void Annunciators()
        {
            window.User(FlagSet(F_USER));
            window.Shift(FlagSet(F_SHIFT));
            window.Prog(FlagSet(F_PRGM));
            window.Alpha(FlagSet(F_ALPHA));
            window.Flag_0(FlagSet(0));
            window.Flag_1(FlagSet(1));
            window.Flag_2(FlagSet(2));
            window.Flag_3(FlagSet(3));
            window.Flag_4(FlagSet(4));
            if (FlagSet(F_GRAD))
            {
                window.G(true);
                window.Rad(true);
            }
            else if (FlagSet(F_RAD))
            {
                window.G(false);
                window.Rad(true);
            }
            else
            {
                window.G(false);
                window.Rad(false);
            }
        }

        public void Prgm()
        {
            if (FlagSet(F_PRGM)) ClearFlag(F_PRGM);
            else SetFlag(F_PRGM);
            window.Prog(FlagSet(F_PRGM));
            window.Display(Display(),true);
        }

        public void Shift()
        {
            if (FlagSet(F_SHIFT)) ClearFlag(F_SHIFT);
            else SetFlag(F_SHIFT);
            if (uiMode == 1 && FlagSet(F_SHIFT))
            {
                if ((catalog[ram[REG_R + 0]].flags & 0x08) != 0) {
                    if ((ram[REG_E + 2] & 0x01) == 0)
                    {
                        ram[REG_E + 2] |= 0x01;
                        display = catalog[ram[REG_R + 0]].name + " IND __";
                        window.Display(display, true);
                        ram[REG_R + 2] &= 0xf0;
                        ram[REG_R + 1] = 0xff;
                    }
                    ClearFlag(F_SHIFT);
                }
            }
            window.Shift(FlagSet(F_SHIFT));
        }

        public void User()
        {
            if (FlagSet(F_USER)) ClearFlag(F_USER);
            else SetFlag(F_USER);
            window.User(FlagSet(F_USER));
        }

        public void Print()
        {
            if (FlagSet(F_ALPHA))
            {
                ram[REG_R + 1] = 0xa7;
                ram[REG_R + 0] = 0x48;
            }
            else
            {
                ram[REG_R + 1] = 0xa7;
                ram[REG_R + 0] = 0x54;
            }
            Execute();
        }

        public void Adv()
        {
            ram[REG_R + 1] = 0x8f;
            ram[REG_R + 0] = 0x00;
            Execute();
        }

        public void ShowNextGlobal(bool first)
        {
            int addr;
            int i;
            int p;
            int end;
            string line;
            addr = FromPtr((ram[REG_B + 1] << 8) | ram[REG_B + 0]) - 1;
            if (ram[addr] >= 0xc0 && ram[addr] <= 0xcd && (ram[addr - 2] & 0xf0) == 0x20)
            {
                uiMode = 0;
                ClearFlag(F_CAT);
                window.Display(Display(), true);
                return;
            }
            while (ram[addr] < 0xc0 || ram[addr] > 0xcd) addr -= isize(addr);
            if (!first)
            {
                addr -= isize(addr);
                while (ram[addr] < 0xc0 || ram[addr] > 0xcd) addr -= isize(addr);
            }
            line = "";
            if (ram[addr - 2] >= 0xf0)
            {
                p = addr - 4;
                line = "LBL\"";
                for (i = 0; i < (int)(ram[addr - 2] & 0x0f) - 1; i++)
                    line += ((char)ram[p--]).ToString();
            }
            else
            {
                if ((ram[addr - 2] & 0xf0) == 0x20)
                {
                    end = ((ram[REG_C + 1] & 0x0f) << 8) | ram[REG_C + 0];
                    line = $".END. REG {end - FindBottom():d}";
                }
                else
                {
                    line = "END";
                }
            }
            window.Display(line, true);
            if (window.PrinterMode() == 'T')
            {
                if (line.StartsWith("LBL")) line = "LBL\x60" + line.Substring(4);
                if (line.StartsWith("END"))
                {
                    line = $"END         {catSize-addr:d02} BYTES";
                    catSize = addr;
                }
                if (line.StartsWith(".END."))
                {
                    line = $".END.       {catSize - addr:d02} BYTES";
                    catSize = addr;
                }
                printer.Print(line, 'L');
            }
            addr = ToPtr(addr + 1);
            ram[REG_B + 1] = (byte)((addr >> 8) & 0xff);
            ram[REG_B + 0] = (byte)(addr & 0xff);
        }
        
        public void ShowPriorGlobal(bool first)
        {
            int addr;
            int i;
            int p;
            int end;
            int next;
            string line;
            addr = (ram[REG_B + 1] << 8) | ram[REG_B + 0];
            addr = FromPtr(addr) - 1;
            next = ((ram[addr] & 0x0f) << 8) | ram[addr - 1];
            next = ((next & 0x1ff) * 7) + ((next >> 9) & 0x07);
            if (next != 0)
            {
                addr += next - 1;
                while (ram[addr] < 0xc0 || ram[addr] > 0xcd) addr -= isize(addr);
            }
            line = "";
            if (ram[addr - 2] >= 0xf0)
            {
                p = addr - 4;
                line = "LBL\x7a";
                for (i = 0; i < (int)(ram[addr - 2] & 0x0f) - 1; i++)
                    line += ((char)ram[p--]).ToString();
            }
            else
            {
                if ((ram[addr - 2] & 0xf0) == 0x20)
                {
                    end = ((ram[REG_C + 1] & 0x0f) << 8) | ram[REG_C + 0];
                    line = $".END. REG {end - 0x0c0:d}";
                }
                else
                {
                    line = "END";
                }
            }
            window.Display(line, true);
            if (window.PrinterMode() == 'T') window.Print(line, 'L');
            addr = ToPtr(addr + 1);
            ram[REG_B + 1] = (byte)((addr >> 8) & 0xff);
            ram[REG_B + 0] = (byte)(addr & 0xff);
        }

        public void Tick()
        {
            int b;
            if (calculatorMode == CM_SW)
            {
                if (!timeModule.swHold)
                {
                    timeModule.SwDisplay();
                    window.Display(Display(), true);
                }
                return;
            }
            if (calculatorMode == CM_CLOCK)
            {
                timeModule.ClockDisplay();
                window.Display(Display(), true);
                return;
            }
            if (running) return;
            if (dirMode)
            {
                if (tapeDrive.ShowNextDirEntry() == false)
                {
                    dirMode = false;
                    window.Display(Display(), true);
                }
                return;
            }
            if (FlagSet(F_CAT) && !catPause)
            {
                if ((ram[REG_R+2] & 0x0f) == 1)
                {
                    ShowNextGlobal(false);
                }
                else
                {
//                    b = (byte)(ram[REG_R + 1] + 1);
                    b = ((ram[REG_R + 1] << 8) | ram[REG_R + 0]) + 1;
                    if (catalog[b].catalog == (ram[REG_R+2] & 0x0f))
                    {
                        window.Display(catalog[b].name, true);
                        if (window.PrinterMode() == 'T') printer.Print(catalog[b].name, 'L');
                        ram[REG_R + 1] = (byte)(b / 256);
                        ram[REG_R + 0] = (byte)(b & 0xff);
                    }
                    else
                    {
                        ClearFlag(F_CAT);
                        catPause = false;
                        window.Display(Display(), true);
                    }
                }
            }
        }

        public void RunTick()
        {
            int i;
            int addr;
            int limit;
            if (!running)
            {
                window.RunTimerEnabled(false);
                window.Display(Display(), true);
                return;
            }
            limit = (window.Fast()) ? 100 : 1;
            window.RunTimerEnabled(false);
            for (i = 0; i < limit; i++)
            {
                if (running)
                {
//                    if (FlagSet(F_PSE))
//                    {
//                        pauseCount--;
//                        if (pauseCount >= 0) return;
//                        ClearFlag(F_PSE);
//                    }
                    addr = (ram[REG_B + 1] << 8) | ram[REG_B + 0];
                    addr = FromPtr(addr) - 1;
                    addr = Exec(addr);
                    if (addr != 0)
                    {
                        addr = ToPtr(addr + 1);
                        ram[REG_B + 1] = (byte)((addr >> 8) & 0xff);
                        ram[REG_B + 0] = (byte)(addr & 0xff);
                    }
                    else
                    {
                        running = false;
                        Annunciators();
                        window.Display(Display(), true);
                    }
                }
            }
            if (running) window.RunTimerEnabled(true);
        }

        public int SearchKaRegisters(byte keycode)
        {
            int addr;
            /* ******************************************* */
            /* ***** Search key assignment registers ***** */
            /* ******************************************* */
            addr = 0x0c0 * 7;
            while (ram[addr + 6] == 0xf0 &&
                   ram[addr + 0] != keycode &&
                   ram[addr + 3] != keycode) addr += 7;

            /* *********************************************** */
            /* ***** Process if key found in KA register ***** */
            /* *********************************************** */
            if (ram[addr + 6] == 0xf0)
            {
                if (ram[addr + 0] == keycode) return addr + 1;
                else return addr + 4;
            }
            return -1;

        }

        public void ByteJump()
        {
            int addr;
            int i;
            if (!FlagSet(F_PRGM))
            {
                addr = (ram[REG_B + 1] << 8) | ram[REG_B + 0];
                addr = FromPtr(addr);
                i = (ram[addr] & 0x0f);
                addr -= i;
                addr = ToPtr(addr);
                ram[REG_B + 1] = (byte)((addr >> 8) & 0xff);
                ram[REG_B + 0] = (byte)(addr & 0xff);

            }
        }

        public void Card(string filename)
        {
            Stream file;
            byte[] card = new byte[5];
            if (FlagSet(F_PRGM))
            {
                cardReader.Wprg(filename);
            }
            else
            {
                file = File.Open(filename, FileMode.Open, FileAccess.ReadWrite);
                if (file == null)
                {
                    Message("CARD ERR");
                    Error();
                    return;
                }
                file.Read(card, 0, 5);
                file.Close();
                if (card[0] == 'S') cardReader.Rsts(filename);
                if (card[0] == 'A') cardReader.Rall(filename);
                if (card[0] == 'P') { cardReader.Rprg(filename); window.Display(Display(), true); }
            }
        }

        public int GetBE(Number x)
        {
            int s = 0;
            int e = 0;
            int p = 0;
            int n;
            int i;
            if (x.esign == 0)
            {
                s = x.mantissa[p++];
                n = x.exponent[0] * 10 + x.exponent[1];
                while (n > 0)
                {
                    s *= 10;
                    if (p < 10) s += x.mantissa[p++];
                    n--;
                }
            }
            else
            {
                n = x.exponent[0] * 10 + x.exponent[1];
                while (n > 0)
                {
                    n--;
                    p--;
                }
                p++;
            }
            for (i = 0; i < 3; i++)
            {
                e *= 10;
                if (p >= 0 && p < 10) e += x.mantissa[p];
                p++;
            }
            return (s * 1000) + e;
        }
    }
}

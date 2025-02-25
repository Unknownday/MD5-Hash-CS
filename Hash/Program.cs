using System.Buffers.Binary;
using System.Runtime.ConstrainedExecution;
using System.Text;

uint A = 0x67452301;
uint B = 0xEFCDAB89;
uint C = 0x98BADCFE;
uint D = 0x10325476;

uint[] T = {
    0xd76aa478, 0xe8c7b756, 0x242070db, 0xc1bdceee,
    0xf57c0faf, 0x4787c62a, 0xa8304613, 0xfd469501,
    0x698098d8, 0x8b44f7af, 0xffff5bb1, 0x895cd7be,
    0x6b901122, 0xfd987193, 0xa679438e, 0x49b40821,
    0xf61e2562, 0xc040b340, 0x265e5a51, 0xe9b6c7aa,
    0xd62f105d, 0x02441453, 0xd8a1e681, 0xe7d3fbc8,
    0x21e1cde6, 0xc33707d6, 0xf4d50d87, 0x455a14ed,
    0xa9e3e905, 0xfcefa3f8, 0x676f02d9, 0x8d2a4c8a,
    0xfffa3942, 0x8771f681, 0x6d9d6122, 0xfde5380c,
    0xa4beea44, 0x4bdecfa9, 0xf6bb4b60, 0xbebfbc70,
    0x289b7ec6, 0xeaa127fa, 0xd4ef3085, 0x04881d05,
    0xd9d4d039, 0xe6db99e5, 0x1fa27cf8, 0xc4ac5665,
    0xf4292244, 0x432aff97, 0xab9423a7, 0xfc93a039,
    0x655b59c3, 0x8f0ccc92, 0xffeff47d, 0x85845dd1,
    0x6fa87e4f, 0xfe2ce6e0, 0xa3014314, 0x4e0811a1,
    0xf7537e82, 0xbd3af235, 0x2ad7d2bb, 0xeb86d391,
};

uint[] S = {
    7, 12, 17, 22, 7, 12, 17, 22, 7, 12, 17, 22, 7, 12, 17, 22,
    5, 9, 14, 20, 5, 9, 14, 20, 5, 9, 14, 20, 5, 9, 14, 20,
    4, 11, 16, 23, 4, 11, 16, 23, 4, 11, 16, 23, 4, 11, 16, 23,
    6, 10, 15, 21, 6, 10, 15, 21, 6, 10, 15, 21, 6, 10, 15, 21,
};

static uint F(uint X, uint Y, uint Z)
{
    return (X & Y) | (~X & Z);
}

static uint G(uint X, uint Y, uint Z)
{
    return (X & Z) | (Y & ~Z);
}

static uint H(uint X, uint Y, uint Z)
{
    return X ^ Y ^ Z;
}

static uint I(uint X, uint Y, uint Z)
{
    return Y ^ (X | ~Z);
}

static uint ShiftLeft(uint integer, int steps)
{
    return integer << steps | integer >> (32 - steps);
}

void getHash(byte[] array)
{
    for (int n = 0; n < array.Length; n += 64)
    {
        uint AA = A;
        uint BB = B;
        uint CC = C;
        uint DD = D;

        //R1
        A = B + ShiftLeft(A + F(B, C, D) + array[n + 0] + T[0], 7);
        D = A + ShiftLeft(D + F(A, B, C) + array[n + 1] + T[1], 12);
        C = D + ShiftLeft(C + F(D, A, B) + array[n + 2] + T[2], 17);
        B = C + ShiftLeft(B + F(C, D, A) + array[n + 3] + T[3], 22);

        A = B + ShiftLeft(A + F(B, C, D) + array[n + 4] + T[4], 7);
        D = A + ShiftLeft(D + F(A, B, C) + array[n + 5] + T[5], 12);
        C = D + ShiftLeft(C + F(D, A, B) + array[n + 6] + T[6], 17);
        B = C + ShiftLeft(B + F(C, D, A) + array[n + 7] + T[7], 22);

        A = B + ShiftLeft(A + F(B, C, D) + array[n + 8] + T[8], 7);
        D = A + ShiftLeft(D + F(A, B, C) + array[n + 9] + T[9], 12);
        C = D + ShiftLeft(C + F(D, A, B) + array[n + 10] + T[10], 17);
        B = C + ShiftLeft(B + F(C, D, A) + array[n + 11] + T[11], 22);

        A = B + ShiftLeft(A + F(B, C, D) + array[n + 12] + T[12], 7);
        D = A + ShiftLeft(D + F(A, B, C) + array[n + 13] + T[13], 12);
        C = D + ShiftLeft(C + F(D, A, B) + array[n + 14] + T[14], 17);
        B = C + ShiftLeft(B + F(C, D, A) + array[n + 15] + T[15], 22);

        //R2
        A = B + ShiftLeft(A + G(B, C, D) + array[n + 1] + T[16], 5);
        D = A + ShiftLeft(D + G(A, B, C) + array[n + 6] + T[17], 9);
        C = D + ShiftLeft(C + G(D, A, B) + array[n + 11] + T[18], 14);
        B = C + ShiftLeft(B + G(C, D, A) + array[n + 0] + T[19], 20);

        A = B + ShiftLeft(A + G(B, C, D) + array[n + 5] + T[20], 5);
        D = A + ShiftLeft(D + G(A, B, C) + array[n + 10] + T[21], 9);
        C = D + ShiftLeft(C + G(D, A, B) + array[n + 15] + T[22], 14);
        B = C + ShiftLeft(B + G(C, D, A) + array[n + 4] + T[23], 20);

        A = B + ShiftLeft(A + G(B, C, D) + array[n + 9] + T[24], 5);
        D = A + ShiftLeft(D + G(A, B, C) + array[n + 14] + T[25], 9);
        C = D + ShiftLeft(C + G(D, A, B) + array[n + 3] + T[26], 14);
        B = C + ShiftLeft(B + G(C, D, A) + array[n + 8] + T[27], 20);

        A = B + ShiftLeft(A + G(B, C, D) + array[n + 13] + T[28], 5);
        D = A + ShiftLeft(D + G(A, B, C) + array[n + 2] + T[29], 9);
        C = D + ShiftLeft(C + G(D, A, B) + array[n + 7] + T[30], 14);
        B = C + ShiftLeft(B + G(C, D, A) + array[n + 12] + T[31], 20);

        //R3
        A = B + ShiftLeft(A + H(B, C, D) + array[n + 5] + T[32], 4);
        D = A + ShiftLeft(D + H(A, B, C) + array[n + 8] + T[33], 11);
        C = D + ShiftLeft(C + H(D, A, B) + array[n + 11] + T[34], 16);
        B = C + ShiftLeft(B + H(C, D, A) + array[n + 14] + T[35], 23);

        A = B + ShiftLeft(A + H(B, C, D) + array[n + 1] + T[36], 4);
        D = A + ShiftLeft(D + H(A, B, C) + array[n + 4] + T[37], 11);
        C = D + ShiftLeft(C + H(D, A, B) + array[n + 7] + T[38], 16);
        B = C + ShiftLeft(B + H(C, D, A) + array[n + 10] + T[39], 23);

        A = B + ShiftLeft(A + H(B, C, D) + array[n + 13] + T[40], 4);
        D = A + ShiftLeft(D + H(A, B, C) + array[n + 0] + T[41], 11);
        C = D + ShiftLeft(C + H(D, A, B) + array[n + 3] + T[42], 16);
        B = C + ShiftLeft(B + H(C, D, A) + array[n + 6] + T[43], 23);

        A = B + ShiftLeft(A + H(B, C, D) + array[n + 9] + T[44], 4);
        D = A + ShiftLeft(D + H(A, B, C) + array[n + 12] + T[45], 11);
        C = D + ShiftLeft(C + H(D, A, B) + array[n + 15] + T[46], 16);
        B = C + ShiftLeft(B + H(C, D, A) + array[n + 2] + T[47], 23);

        //R4
        A = B + ShiftLeft(A + I(B, C, D) + array[n + 0] + T[48], 6);
        D = A + ShiftLeft(D + I(A, B, C) + array[n + 7] + T[49], 10);
        C = D + ShiftLeft(C + I(D, A, B) + array[n + 14] + T[50], 15);
        B = C + ShiftLeft(B + I(C, D, A) + array[n + 5] + T[51], 21);

        A = B + ShiftLeft(A + I(B, C, D) + array[n + 12] + T[52], 6);
        D = A + ShiftLeft(D + I(A, B, C) + array[n + 3] + T[53], 10);
        C = D + ShiftLeft(C + I(D, A, B) + array[n + 10] + T[54], 15);
        B = C + ShiftLeft(B + I(C, D, A) + array[n + 1] + T[55], 21);

        A = B + ShiftLeft(A + I(B, C, D) + array[n + 8] + T[56], 6);
        D = A + ShiftLeft(D + I(A, B, C) + array[n + 15] + T[57], 10);
        C = D + ShiftLeft(C + I(D, A, B) + array[n + 6] + T[58], 15);
        B = C + ShiftLeft(B + I(C, D, A) + array[n + 13] + T[59], 21);

        A = B + ShiftLeft(A + I(B, C, D) + array[n + 4] + T[60], 6);
        D = A + ShiftLeft(D + I(A, B, C) + array[n + 11] + T[61], 10);
        C = D + ShiftLeft(C + I(D, A, B) + array[n + 2] + T[62], 15);
        B = C + ShiftLeft(B + I(C, D, A) + array[n + 9] + T[63], 21);

        A += AA;
        B += BB;
        C += CC;
        D += DD;
    }
}

static void AppendBytesToMsg(ref byte[] msg)
{
    byte[] zero = Encoding.UTF8.GetBytes("0");
    byte[] one = Encoding.UTF8.GetBytes("1");
    byte[] lastBlock = new byte[msg.Length % 64];
    byte[] newBlock = new byte[64];

    for (int i = 0; i < lastBlock.Length; i++)
        lastBlock[i] = msg[msg.Length - msg.Length % 64 + i];

    lastBlock.CopyTo(newBlock, 0);

    for (int i = lastBlock.Length; i < newBlock.Length; i++)
        newBlock[i] = zero[0];

    newBlock[newBlock.Length - 1] = one[0];

    int newLength = msg.Length - lastBlock.Length + 64;
    byte[] final = new byte[newLength];
    msg.CopyTo(final, 0);
    newBlock.CopyTo(final, final.Length - 64);
    msg = final;
}

static string ConvertToHEX(uint value)
{
    return value.ToString("X");
}

string GetHashString(string msg)
{
    string finalHash = "";
    byte[] bytes = Encoding.UTF8.GetBytes(msg);
    AppendBytesToMsg(ref bytes);
    getHash(bytes);
    finalHash += ConvertToHEX(A);
    finalHash += ConvertToHEX(B);
    finalHash += ConvertToHEX(C);
    finalHash += ConvertToHEX(D);
    return finalHash;
}


string GetHashFile(string path)
{
    string finalHash = "";
    byte[] bytes = File.ReadAllBytes(path);
    AppendBytesToMsg(ref bytes);
    getHash(bytes);
    finalHash += ConvertToHEX(A);
    finalHash += ConvertToHEX(B);
    finalHash += ConvertToHEX(C);
    finalHash += ConvertToHEX(D);
    
    return finalHash;
}

Console.WriteLine(GetHashString(" --<-<-<@ "));

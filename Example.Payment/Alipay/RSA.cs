using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Example.Payment.Alipay
{
    public sealed class RSA
    {
        /// <summary>
        /// 签名
        /// </summary>
        /// <param name="content">需要签名的内容</param>
        /// <param name="privateKey">私钥</param>
        /// <param name="inputCharset">编码格式</param>
        /// <returns></returns>
        public static string Sign(string content, string privateKey, string inputCharset)
        {
            return RSASignCharSet(content, privateKey, inputCharset);
        }

        public static string RSASignCharSet(string data, string privateKeyPem, string charset)
        {
            var rsaCsp = LoadCertificateFile(privateKeyPem);
            byte[] dataBytes = null;
            if (string.IsNullOrEmpty(charset))
            {
                dataBytes = Encoding.UTF8.GetBytes(data);
            }
            else
            {
                dataBytes = Encoding.GetEncoding(charset).GetBytes(data);
            }

            var signatureBytes = rsaCsp.SignData(dataBytes, "SHA1");

            return Convert.ToBase64String(signatureBytes);
        }

        private static RSACryptoServiceProvider LoadCertificateFile(string filename)
        {
            using (var fs = File.OpenRead(filename))
            {
                var data = new byte[fs.Length];
                byte[] res = null;
                fs.Read(data, 0, data.Length);
                if (data[0] != 0x30)
                {
                    res = GetPem("RSA PRIVATE KEY", data);
                }
                try
                {
                    RSACryptoServiceProvider rsa = DecodeRSAPrivateKey(res);
                    return rsa;
                }
                catch (Exception)
                {
                    // ignored
                }
                return null;
            }
        }

        private static byte[] GetPem(string type, byte[] data)
        {
            var pem = Encoding.UTF8.GetString(data);
            var header = $"-----BEGIN {type}-----\\n";
            var footer = $"-----END {type}-----";
            var start = pem.IndexOf(header, StringComparison.Ordinal) + header.Length;
            var end = pem.IndexOf(footer, start, StringComparison.Ordinal);
            var base64 = pem.Substring(start, (end - start));

            return Convert.FromBase64String(base64);
        }

        /// <summary>
        /// 验证签名
        /// </summary>
        /// <param name="content">需要验证的内容</param>
        /// <param name="signedString">签名结果</param>
        /// <param name="publicKey">公钥</param>
        /// <param name="inputCharset">编码格式</param>
        public static bool Verify(string content, string signedString, string publicKey, string inputCharset)
        {
            var code = Encoding.GetEncoding(inputCharset);
            var data = code.GetBytes(content);
            var basae64Data = Convert.FromBase64String(signedString);
            var paraPub = ConvertFromPublicKey(publicKey);
            var rsaPub = new RSACryptoServiceProvider();
            rsaPub.ImportParameters(paraPub);

            SHA1 sh = new SHA1CryptoServiceProvider();
            var result = rsaPub.VerifyData(data, sh, basae64Data);
            return result;
        }

        /// <summary>
        /// 用RSA解密
        /// </summary>
        /// <param name="resData">待解密字符串</param>
        /// <param name="privateKey">私钥</param>
        /// <param name="inputCharset">编码格式</param>
        /// <returns>解密结果</returns>
        public static string DecryptData(string resData, string privateKey, string inputCharset)
        {
            var dataToDecrypt = Convert.FromBase64String(resData);
            var result = new List<byte>();

            for (var j = 0; j < dataToDecrypt.Length / 128; j++)
            {
                var buf = new byte[128];
                for (var i = 0; i < 128; i++)
                {
                    buf[i] = dataToDecrypt[i + 128 * j];
                }
                result.AddRange(Decrypt(buf, privateKey, inputCharset));
            }
            var source = result.ToArray();
            var asciiChars = new char[Encoding.GetEncoding(inputCharset).GetCharCount(source, 0, source.Length)];
            Encoding.GetEncoding(inputCharset).GetChars(source, 0, source.Length, asciiChars, 0);
            return new string(asciiChars);
        }

        // ReSharper disable once UnusedParameter.Local
        private static IEnumerable<byte> Decrypt(byte[] data, string privateKey, string inputCharset)
        {
            RSACryptoServiceProvider rsa = DecodePemPrivateKey(privateKey);
            // ReSharper disable once UnusedVariable
            SHA1 sh = new SHA1CryptoServiceProvider();
            return rsa.Decrypt(data, false);
        }

        private static RSACryptoServiceProvider DecodePemPrivateKey(string pemstr)
        {
            var privatekey = Convert.FromBase64String(pemstr);
            var rsa = DecodePrivateKeyInfo(privatekey);
            return rsa;
        }

        private static RSACryptoServiceProvider DecodePrivateKeyInfo(byte[] pkcs8)
        {

            byte[] seqOid = { 0x30, 0x0D, 0x06, 0x09, 0x2A, 0x86, 0x48, 0x86, 0xF7, 0x0D, 0x01, 0x01, 0x01, 0x05, 0x00 };
            var seq = new byte[15];

            var mem = new MemoryStream(pkcs8);
            var lenstream = (int)mem.Length;
            var binr = new BinaryReader(mem);    //wrap Memory Stream with BinaryReader for easy reading
            byte bt;
            ushort twobytes;

            try
            {

                twobytes = binr.ReadUInt16();
                if (twobytes == 0x8130)	//data read as little endian order (actual data order for Sequence is 30 81)
                    binr.ReadByte();	//advance 1 byte
                else if (twobytes == 0x8230)
                    binr.ReadInt16();	//advance 2 bytes
                else
                    return null;


                bt = binr.ReadByte();
                if (bt != 0x02)
                    return null;

                twobytes = binr.ReadUInt16();

                if (twobytes != 0x0001)
                    return null;

                seq = binr.ReadBytes(15);		//read the Sequence OID
                if (!CompareBytearrays(seq, seqOid))	//make sure Sequence for OID is correct
                    return null;

                bt = binr.ReadByte();
                if (bt != 0x04)	//expect an Octet string 
                    return null;

                bt = binr.ReadByte();		//read next byte, or next 2 bytes is  0x81 or 0x82; otherwise bt is the byte count
                if (bt == 0x81)
                    binr.ReadByte();
                else
                    if (bt == 0x82)
                    binr.ReadUInt16();
                //------ at this stage, the remaining sequence should be the RSA private key

                byte[] rsaprivkey = binr.ReadBytes((int)(lenstream - mem.Position));
                RSACryptoServiceProvider rsacsp = DecodeRSAPrivateKey(rsaprivkey);
                return rsacsp;
            }

            catch (Exception)
            {
                return null;
            }

            finally { binr.Close(); }

        }

        private static bool CompareBytearrays(byte[] a, byte[] b)
        {
            if (a.Length != b.Length)
                return false;
            int i = 0;
            foreach (byte c in a)
            {
                if (c != b[i])
                    return false;
                i++;
            }
            return true;
        }

        private static RSACryptoServiceProvider DecodeRSAPrivateKey(byte[] privkey)
        {
            byte[] MODULUS, E, D, P, Q, DP, DQ, IQ;

            // ---------  Set up stream to decode the asn.1 encoded RSA private key  ------
            MemoryStream mem = new MemoryStream(privkey);
            BinaryReader binr = new BinaryReader(mem);    //wrap Memory Stream with BinaryReader for easy reading
            byte bt;
            ushort twobytes;
            int elems;
            try
            {
                twobytes = binr.ReadUInt16();
                if (twobytes == 0x8130)	//data read as little endian order (actual data order for Sequence is 30 81)
                    binr.ReadByte();	//advance 1 byte
                else if (twobytes == 0x8230)
                    binr.ReadInt16();	//advance 2 bytes
                else
                    return null;

                twobytes = binr.ReadUInt16();
                if (twobytes != 0x0102)	//version number
                    return null;
                bt = binr.ReadByte();
                if (bt != 0x00)
                    return null;


                //------  all private key components are Integer sequences ----
                elems = GetIntegerSize(binr);
                MODULUS = binr.ReadBytes(elems);

                elems = GetIntegerSize(binr);
                E = binr.ReadBytes(elems);

                elems = GetIntegerSize(binr);
                D = binr.ReadBytes(elems);

                elems = GetIntegerSize(binr);
                P = binr.ReadBytes(elems);

                elems = GetIntegerSize(binr);
                Q = binr.ReadBytes(elems);

                elems = GetIntegerSize(binr);
                DP = binr.ReadBytes(elems);

                elems = GetIntegerSize(binr);
                DQ = binr.ReadBytes(elems);

                elems = GetIntegerSize(binr);
                IQ = binr.ReadBytes(elems);

                // ------- create RSACryptoServiceProvider instance and initialize with public key -----
                RSACryptoServiceProvider RSA = new RSACryptoServiceProvider();
                RSAParameters RSAparams = new RSAParameters();
                RSAparams.Modulus = MODULUS;
                RSAparams.Exponent = E;
                RSAparams.D = D;
                RSAparams.P = P;
                RSAparams.Q = Q;
                RSAparams.DP = DP;
                RSAparams.DQ = DQ;
                RSAparams.InverseQ = IQ;
                RSA.ImportParameters(RSAparams);
                return RSA;
            }
            catch (Exception)
            {
                return null;
            }
            finally { binr.Close(); }
        }

        private static int GetIntegerSize(BinaryReader binr)
        {
            byte bt;
            byte lowbyte;
            byte highbyte;
            int count;
            bt = binr.ReadByte();
            if (bt != 0x02)		//expect integer
                return 0;
            bt = binr.ReadByte();

            if (bt == 0x81)
                count = binr.ReadByte();	// data size in next byte
            else
                if (bt == 0x82)
            {
                highbyte = binr.ReadByte(); // data size in next 2 bytes
                lowbyte = binr.ReadByte();
                byte[] modint = { lowbyte, highbyte, 0x00, 0x00 };
                count = BitConverter.ToInt32(modint, 0);
            }
            else
            {
                count = bt;     // we already have the data size
            }



            while (binr.ReadByte() == 0x00)
            {	//remove high order zeros in data
                count -= 1;
            }
            binr.BaseStream.Seek(-1, SeekOrigin.Current);		//last ReadByte wasn't a removed zero, so back up a byte
            return count;
        }

        #region 解析.net 生成的Pem
        private static RSAParameters ConvertFromPublicKey(string pemFileConent)
        {

            byte[] keyData = Convert.FromBase64String(pemFileConent);
            if (keyData.Length < 162)
            {
                throw new ArgumentException("pem file content is incorrect.");
            }
            byte[] pemModulus = new byte[128];
            byte[] pemPublicExponent = new byte[3];
            Array.Copy(keyData, 29, pemModulus, 0, 128);
            Array.Copy(keyData, 159, pemPublicExponent, 0, 3);
            RSAParameters para = new RSAParameters();
            para.Modulus = pemModulus;
            para.Exponent = pemPublicExponent;
            return para;
        }

        // ReSharper disable once UnusedMember.Local
        private static RSAParameters ConvertFromPrivateKey(string pemFileConent)
        {
            byte[] keyData = Convert.FromBase64String(pemFileConent);
            if (keyData.Length < 609)
            {
                throw new ArgumentException("pem file content is incorrect.");
            }

            int index = 11;
            byte[] pemModulus = new byte[128];
            Array.Copy(keyData, index, pemModulus, 0, 128);

            index += 128;
            index += 2;//141
            byte[] pemPublicExponent = new byte[3];
            Array.Copy(keyData, index, pemPublicExponent, 0, 3);

            index += 3;
            index += 4;//148
            byte[] pemPrivateExponent = new byte[128];
            Array.Copy(keyData, index, pemPrivateExponent, 0, 128);

            index += 128;
            index += ((int)keyData[index + 1] == 64 ? 2 : 3);//279
            byte[] pemPrime1 = new byte[64];
            Array.Copy(keyData, index, pemPrime1, 0, 64);

            index += 64;
            index += ((int)keyData[index + 1] == 64 ? 2 : 3);//346
            byte[] pemPrime2 = new byte[64];
            Array.Copy(keyData, index, pemPrime2, 0, 64);

            index += 64;
            index += ((int)keyData[index + 1] == 64 ? 2 : 3);//412/413
            byte[] pemExponent1 = new byte[64];
            Array.Copy(keyData, index, pemExponent1, 0, 64);

            index += 64;
            index += ((int)keyData[index + 1] == 64 ? 2 : 3);//479/480
            byte[] pemExponent2 = new byte[64];
            Array.Copy(keyData, index, pemExponent2, 0, 64);

            index += 64;
            index += ((int)keyData[index + 1] == 64 ? 2 : 3);//545/546
            byte[] pemCoefficient = new byte[64];
            Array.Copy(keyData, index, pemCoefficient, 0, 64);

            RSAParameters para = new RSAParameters();
            para.Modulus = pemModulus;
            para.Exponent = pemPublicExponent;
            para.D = pemPrivateExponent;
            para.P = pemPrime1;
            para.Q = pemPrime2;
            para.DP = pemExponent1;
            para.DQ = pemExponent2;
            para.InverseQ = pemCoefficient;
            return para;
        }
        #endregion

    }
}

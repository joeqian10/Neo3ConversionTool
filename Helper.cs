using Neo;
using Neo.IO;
using Neo.Wallets;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Neo3ConversionTool
{
    public static class Helper
    {
        // hex string and string
        public static string HexStringToString(string hex)
        {
            return Encoding.ASCII.GetString(hex.HexToBytes());
        }

        public static string StringToHexString(string s)
        {
            return Encoding.ASCII.GetBytes(s).ToHexString();
        }

        // hex number and integer
        public static string HexNumberToBigInteger(string hex)
        {
            var bigEnd = hex.HexToBytes().Reverse().ToArray().ToHexString();
            return BigInteger.Parse(bigEnd, NumberStyles.HexNumber).ToString();
        }

        public static string BigIntegerToHexNumber(string integer)
        {
            return BigInteger.Parse(integer).ToByteArray().ToArray().ToHexString(); // little endian
        }

        // address and scripthash
        public static (string, string) AddressToScriptHash(string address)
        {
            UInt160 scriptHash = address.ToScriptHash();
            return (scriptHash.ToString(), scriptHash.ToArray().ToHexString()); // big, little
        }

        public static string ScriptHashToAddress(string scriptHash)
        {
            byte[] data;
            if (scriptHash.StartsWith("0x", StringComparison.InvariantCultureIgnoreCase))
            {
                scriptHash = scriptHash.Substring(2); // big endian
                data = scriptHash.HexToBytes();
                Array.Reverse(data);
            }
            else
                data = scriptHash.HexToBytes(); // little endian
            return new UInt160(data).ToAddress();
        }

        public static string BigLittleEndScriptHashConversion(string scriptHash)
        {
            string result;
            if (scriptHash.StartsWith("0x", StringComparison.InvariantCultureIgnoreCase))
                result = scriptHash.Substring(2).HexToBytes().Reverse().ToArray().ToHexString(); // big => little
            else
                result = "0x" + scriptHash.HexToBytes().Reverse().ToArray().ToHexString();
            return result;
        }

        // base64 string and string
        public static string Base64StringToString(string base64)
        {
            return Encoding.ASCII.GetString(Convert.FromBase64String(base64));
        }

        public static string StringToBase64String(string s)
        {
            return Convert.ToBase64String(Encoding.ASCII.GetBytes(s));
        }

        // base64 string and address
        public static string Base64StringToAddress(string base64)
        {
            return new UInt160(Convert.FromBase64String(base64)).ToAddress();
        }

        public static string AddressToBase64String(string address)
        {
            return Convert.ToBase64String(address.ToScriptHash().ToArray());
        }

        // base64 string and integer
        public static string Base64StringToBigInteger(string base64)
        {
            return new BigInteger(Convert.FromBase64String(base64)).ToString();
        }

        public static string BigIntegerToBase64String(string integer)
        {
            return Convert.ToBase64String(BigInteger.Parse(integer).ToByteArray());
        }
    }
}

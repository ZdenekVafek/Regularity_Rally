using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace Regularity_Rally
{
    public static class Utility
    {
        public static string GetRandomString(int len)
        {
            string sym = "ABCDEFGHIJKLMNOPQRSTUVWXYZ123456789";
            string ret = string.Empty;
            Random rand = new Random();
            for (int i = 0; i < len; i++)
            {
                StringBuilder builder = new StringBuilder();
                builder.Append(sym[rand.Next(sym.Length)]);
                ret += (rand.Next(2) == 0) ? builder.ToString().ToLower() : builder.ToString().ToUpper();
            }
            return ret;
        }

        public static string sha256(string randomString)
        {
            var crypt = new SHA256Managed();
            string hash = String.Empty;
            byte[] crypto = crypt.ComputeHash(Encoding.ASCII.GetBytes(randomString));
            foreach (byte theByte in crypto)
            {
                hash += theByte.ToString("x2");
            }
            return hash;
        }

        // https://stackoverflow.com/questions/479410/enum-tostring-with-user-friendly-strings
        public static string GetDescription<T>(this T enumerationValue) where T : struct
        {
            Type type = enumerationValue.GetType();
            if (!type.IsEnum)
            {
                throw new ArgumentException("EnumerationValue must be of Enum type", "enumerationValue");
            }

            //Tries to find a DescriptionAttribute for a potential friendly name for the enum
            MemberInfo[] memberInfo = type.GetMember(enumerationValue.ToString());
            if (memberInfo != null && memberInfo.Length > 0)
            {
                object[] attrs = memberInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);

                if (attrs != null && attrs.Length > 0)
                {
                    //Pull out the description value
                    return ((DescriptionAttribute)attrs[0]).Description;
                }
            }
            //If we have no description attribute, just return the ToString of the enum
            return enumerationValue.ToString();
        }
    }

    class Password
    {
        public static string CreateRS() // of length 12
        {
            return Utility.GetRandomString(12);
        }

        public static bool Valid(string pass, string pass_hash, string pass_rs)
        {
            return string.Equals(pass_hash, CretePasswordHash(pass, pass_rs));
        }

        public static string CretePasswordHash(string pass, string pass_rs)
        {
            // compute hash from passed password
            string final_hash_ = Utility.sha256(pass);
            // this is absolut random string added to proces of validatin to make cracking "mutch" harder
            final_hash_ += "ZR6WhgjhzEvNNGfc";
            final_hash_ += pass_rs;

            return Utility.sha256(final_hash_);
        }
    }
}

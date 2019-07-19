using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace UtilityDAL
{
     public static class GUIDParse
    {
        public static Guid ToGUID(string input)
        {          
            
                byte[] hash =Encoding.Default.GetBytes(input);
                return new Guid(hash);

        }

        public static string FromGUID(Guid input)
        {
 
                byte[] buffer = input.ToByteArray();
                return Encoding.UTF8.GetString(buffer, 0, buffer.Length); ;
     
        }
    }
}

using System;
using System.Linq;
using System.Text;
using System.Security.Cryptography;

namespace AAEmu.PassGen
{
    class Program
    {
        static char[] passChars = new char[] {
            'a', 'b',      'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k',      'm', 'n',      'p', 'q', 'r',      't', 'u', 'v', 'w', 'x', 'y', 'z',
            'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H',      'J', 'K', 'L', 'M', 'N',      'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z',
            '0', '1', '2', '3', '4', '5', '6', '7', '8', '9'
            };

        static string RandomPass(int len)
        {
            string String = string.Empty;
            var Random = new Random();

            for (byte a = 0; a < len; a++)
            {
                String += passChars[Random.Next(0, passChars.Count())];
            };

            return (String);
        }

        static void Main(string[] args)
        {
            Console.WriteLine("AAEmu.PassGen");
            Console.WriteLine("--------------");
            Console.WriteLine("此工具可用于帮助生成MySQL数据库的哈希用户密码.");
            Console.WriteLine("它仅用于AAEmu服务器的默认实现.");
            Console.WriteLine("如果你想认真地使用密码，最好实现");
            Console.WriteLine("你自己的密码哈希对数.");
            Console.WriteLine();
            Console.WriteLine("将密码留空以生成随机密码.");
            Console.WriteLine();
            Console.Write("密码: ");
            var pass = Console.ReadLine();
            if (pass == string.Empty)
            {
                pass = RandomPass(16);
                Console.WriteLine();
                Console.WriteLine("生成的密码: {0}", pass);
                Console.WriteLine();
            }
            byte[] passBytes = Encoding.UTF8.GetBytes(pass);
            using (var sha = SHA256.Create())
            {
                var hash = sha.ComputeHash(passBytes);
                var passHash = Convert.ToBase64String(hash);
                Console.WriteLine("Hashed: {0}", passHash);
            }
            Console.WriteLine();
            Console.Write("提示：您可以将鼠标拖到结果上，然后按ENTER键将文本复制到剪贴板");
            Console.WriteLine();
            Console.Write("Press a key to close ...");
            Console.ReadKey();
        }
    }
}

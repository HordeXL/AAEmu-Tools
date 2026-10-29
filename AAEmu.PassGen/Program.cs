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
            Console.WriteLine("此工具可用于生成 MySQL 数据库的用户密码哈希。");
            Console.WriteLine("仅适用于 AAEmu 服务器的默认实现。");
            Console.WriteLine("如果要在正式环境中使用密码，建议实现");
            Console.WriteLine("你自己的密码哈希算法。");
            Console.WriteLine();
            Console.WriteLine("将密码留空可生成随机密码。");
            Console.WriteLine();
            Console.Write("密码: ");
            var pass = Console.ReadLine();
            if (pass == string.Empty)
            {
                pass = RandomPass(16);
                Console.WriteLine();
                Console.WriteLine("生成的密码：{0}", pass);
                Console.WriteLine();
            }
            byte[] passBytes = Encoding.UTF8.GetBytes(pass);
            using (var sha = SHA256.Create())
            {
                var hash = sha.ComputeHash(passBytes);
                var passHash = Convert.ToBase64String(hash);
                Console.WriteLine("哈希值：{0}", passHash);
            }
            Console.WriteLine();
            Console.Write("提示：可以用鼠标选中结果，然后按 Enter 键复制到剪贴板");
            Console.WriteLine();
            Console.Write("按任意键关闭 ...");
            Console.ReadKey();
        }
    }
}

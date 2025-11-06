using System.Collections.Generic;
using System.Globalization;
using System.Numerics;
using System.Windows.Forms;
using System.Xml;

namespace AAEmu.DBViewer.utils
{
    public static class XmlHelper
    {
        public static Dictionary<string, string> ReadNodeAttributes(XmlNode node)
        {
            var res = new Dictionary<string, string>();
            res.Clear();
            if (node.Attributes != null)
            {
                for (var i = 0; i < node.Attributes.Count; i++)
                    res.Add(node.Attributes.Item(i).Name.ToLower(), node.Attributes.Item(i).Value);
            }

            return res;
        }

        public static Vector3 ReadXmlPos(string posStringBase)
        {
            var baseVec = Vector3.Zero;
            var posValues = posStringBase.Split(',');
            if (posValues.Length != 3)
            {
                MessageBox.Show("Pos函数内的参数值数量无效: " + posStringBase);
            }
            else
                try
                {
                    baseVec = new Vector3(
                        float.Parse(posValues[0], CultureInfo.InvariantCulture),
                        float.Parse(posValues[1], CultureInfo.InvariantCulture),
                        float.Parse(posValues[2], CultureInfo.InvariantCulture)
                    );
                }
                catch
                {
                    MessageBox.Show("Pos参数包含无效浮点数值: " + posStringBase);
                }

            return baseVec;
        }
    }
}

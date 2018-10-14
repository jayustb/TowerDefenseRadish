using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;

//类功能 : 变现命令为XML的类
public class Tool
{
    //1. 访问Levels目录 转化XML文件信息
    public static List<FileInfo> GetLevelFiles()
    {
        // 从访问Const.CS开始
        string[] files = Directory.GetFiles(Const.LevelDir, "*.xml");

        List<FileInfo> list = new List<FileInfo>();
        for (int i = 0; i < files.Length; i++)
        {
            FileInfo file = new FileInfo(files[i]);
            list.Add(file);
        }

        return list;
    }

    //2. 填充Level数据
    public static void FillLevel(string fileName, ref Level level)
    {
        FileInfo file = new FileInfo(fileName);
        StreamReader sr = new StreamReader(file.OpenRead(), Encoding.UTF8);

        XmlDocument doc = new XmlDocument();
        doc.Load(sr);

        level.Name = doc.SelectSingleNode(@"\Level\Name")?.InnerText;
        level.BackGround = doc.SelectSingleNode(@"\Level\BackGround")?.InnerText;
        level.Road = doc.SelectSingleNode(@"\Level\Road")?.InnerText;
        level.InitScore = int.Parse(doc.SelectSingleNode(@"\Level\InitScore")?.InnerText);

        //todo:搞懂这里是不是有错
        XmlNodeList nodes = doc.SelectSingleNode(@"\Level\Holder\Point").ChildNodes;

        for (int i = 0; i < nodes.Count; i++)
        {
            XmlNode node = nodes[i];
            Point p = new Point(int.Parse(node.Attributes["X"].Value), int.Parse(node.Attributes["Y"].Value));
            level.Holder.Add(p);
        }
    }
}
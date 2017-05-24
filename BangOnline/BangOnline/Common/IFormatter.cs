using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BangOnline.Common
{
    public class IFormatter
    {
        public List<FormatHelper> fh;

        public IFormatter()
        {
            fh = new List<FormatHelper>();
        }

        public void AddObject(string[] data)
        {
            fh.Add(new FormatHelper(data));
        }

        public string toFormat()
        {
            string output = string.Empty;

            if (fh.Count == 0) return string.Empty;

            int maxLength = FormatHelper.GetMaxLonger(fh) + 4;
            int numberLine = fh[0].datas.Length;

            for(int i=0; i<numberLine; i++)
            {
                for (int j = 0; j < fh.Count ;j++)
                {
                    string toPrint = fh[j].datas[i];
                    int length = toPrint.Length;
                    int diff = maxLength - length;
                    int offsetLeft = (int)Math.Floor(diff/2.0f);
                    int offsetRight = (int)Math.Ceiling(diff/2.0f);
                    
                    for(int k = 0; k<offsetLeft; k++)
                    {
                        output += " ";
                    }
                    output += toPrint;
                    for (int k = 0; k < offsetRight; k++)
                    {
                        output += " ";
                    }

                    if(j != fh.Count-1)
                    {
                        output += "|";
                    }
                }
                output += "\n";
            }

            return output;
        }

        public static string Formating<T>(List<T> list) where T : IArrayString
        {
            IFormatter formatter = new IFormatter();

            if (list.Count == 0) return string.Empty;

            formatter.AddObject(list[0].BaseInfo());

            foreach (T c in list)
            {
                formatter.AddObject(c.ToArrayString());
            }
            return formatter.toFormat();
        }
    }

    public class FormatHelper
    {
        public int indexLonger = 0;
        public int sizeLonger = 0;
        public string[] datas;

        public FormatHelper(string[] d)
        {
            datas = d;

            for(int i=0; i<datas.Length; i++)
            {
                if(datas[i].Length > sizeLonger)
                {
                    indexLonger = i;
                    sizeLonger = datas[i].Length;
                }
            }
        }

        public static int GetMaxLonger(List<FormatHelper> l)
        {
            int maxLonger = 0;

            foreach(FormatHelper fh in l)
            {
                if(fh.sizeLonger > maxLonger)
                {
                    maxLonger = fh.sizeLonger;
                }
            }

            return maxLonger;
        }
    }
}

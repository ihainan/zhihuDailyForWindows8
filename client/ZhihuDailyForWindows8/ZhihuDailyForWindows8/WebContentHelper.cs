/**
 * Author : ihainan
 * E-mail : ihainan72@gmail.com
 * Created Time : 2013/09/13 17:21
 * Website : http://www.ihainan.me
 * Reference : http://www.dotnetcurry.com/ShowArticle.aspx?ID=838
 **/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZhihuDailyForWindows8
{
    class WebContentHelper
    {
        public static string WrapHtml(string htmlSubString, string[] heads)
        {
            var html = new StringBuilder();
            html.Append("<html>");
            html.Append("<head>");
            foreach (string head in heads)
            {
                html.Append(head);
            }
            html.Append("</head>");
            html.Append("<body>");
            html.Append(htmlSubString);
            html.Append("</body>");
            html.Append("</html>");
            return html.ToString();
        }
    }
}

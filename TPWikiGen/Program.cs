using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace TPWikiGen
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length < 2)
            {
                Console.WriteLine("Usage: TPWikiGen.exe <path to GameProfiles> <path to Descriptions>");
                return;
            }
            else
            {
                Console.WriteLine("TPWikiGenerator by nzgamer41\nDO NOT RUN IN THE teknogods.github.io FOLDER!!!");
                string htmlCode = "";
                GameProfileLoader.LoadProfiles(args[0], args[1]);
                List<GameProfile> profiles = GameProfileLoader.GameProfiles;
                if (Directory.Exists(".\\compatibility"))
                {
                    Directory.Delete(".\\compatibility", true);
                }
                Directory.CreateDirectory(".\\compatibility");

                foreach (var p in profiles)
                {
                    string pnoext = Path.GetFileNameWithoutExtension(p.IconName.Split('\\')[1]);
                    if (!p.DevOnly)
                    {
                        string tableElement = "<tr class=\"tbl-row\"><td class=\"tbl-td1\"><img src=\"https://raw.githubusercontent.com/teknogods/TeknoParrotUIThumbnails/master/Icons/" + pnoext + ".png\" onerror=\"if (this.src != './img/teknoparrot.png') this.src = './img/teknoparrot.png'; \"/></td><td class=\"tbl-td2\"><a class=\"compat-item\" href=\"compatibility/" + pnoext + ".htm\">" + p.GameName + "</a></td><td class=\"tbl-td3\">" + DateTime.Now.ToString("dd/MM/yyyy") + "</td><td class=\"tbl-td4\">";
                        if (p.DevOnly)
                        {
                            Console.WriteLine("DevOnly");
                            tableElement = tableElement + "<i class=\"fas fa-minus-circle unplayable status\"></i></td></tr>";
                        }
                        else if (p.GameInfo.amd != GPUSTATUS.OK || p.GameInfo.nvidia != GPUSTATUS.OK || p.GameInfo.intel != GPUSTATUS.OK)
                        {
                            tableElement = tableElement + "<i class=\"fas fa-exclamation-circle loads status\"></i></td></tr>";
                        }
                        else
                        {
                            tableElement = tableElement + "<i class=\"fas fa-check-circle runs status\"></i></td></tr>";
                        }
                        htmlCode = htmlCode + tableElement + "\n";
                        //File.Create(".\\compatibility\\" + pnoext + ".htm");
                    }
                    if(!File.Exists(".\\compatibility\\" + pnoext + ".htm"))
                    {
                        File.Copy("template.htm.example", ".\\compatibility\\" + pnoext + ".htm");
                        string html = File.ReadAllText(".\\compatibility\\" + pnoext + ".htm");
                        html = html.Replace("TEMPLATEXYX", p.GameName);
                        html = html.Replace("IMAGEXYX", pnoext);
                        html = html.Replace("TEMPLATEXYY", p.GameName);
                        html = html.Replace("ChangeMe", p.GameInfo.platform);
                        File.WriteAllText(".\\compatibility\\" + pnoext + ".htm", html);
                    }
                }
                if (File.Exists("template.txt"))
                {
                    File.Delete("template.txt");
                }
                File.WriteAllText("template.txt", htmlCode);
                Console.WriteLine("All done! Press enter to close.");
                Console.ReadLine();
            }
        }
    }
}

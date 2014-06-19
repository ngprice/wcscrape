using System;
using System.Collections;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Drawing;
using System.Diagnostics;

namespace ConsoleApplication1
{
    class Program
    {
        public static string saveLocation;
        public static int pagelength;
        public static int chapterno;
        
        static void Main()
        {
            Console.Write("---MAIN MENU---\n1) Download pages\n2) Create .cbz\n\nEnter selection: ");
            int choice = Convert.ToInt32(Console.ReadLine());

            switch (choice)
            {
                case 1:
                    Console.WriteLine("--Download Pages--");
                    Console.Write("Enter desired download chapter or range: ");
                    string chapterchoice = Console.ReadLine();

                    if (chapterchoice.Contains('-'))
                    {
                        Console.WriteLine("GETTING ALL MODE ACTIVATED");
                        int beginrange = Convert.ToInt32(chapterchoice.Substring(0, chapterchoice.IndexOf('-')));
                        int endrange = Convert.ToInt32(chapterchoice.Substring(chapterchoice.IndexOf('-') + 1, chapterchoice.Length - chapterchoice.IndexOf('-')-1));

                        for (chapterno = beginrange; chapterno <= endrange; chapterno++)
                        {
                            ArrayList pages = GetPageImgSrc(chapterno);
                            pagelength = (pages.Count);
                            Console.WriteLine(string.Format("************PROCESSING CHAPTER {0}************", chapterno));
                            Console.WriteLine(string.Format("Found {0} pages.", pagelength));

                            for (int i = 0; i < pagelength; ++i)
                            {
                                if (!Directory.Exists(string.Format(@"C:\Users\ngprice\Desktop\Gantz\Chapter {0}\", PadIntForPrint(chapterno, 3))))
                                {
                                    Directory.CreateDirectory(string.Format(@"C:\Users\ngprice\Desktop\Gantz\Chapter {0}\", PadIntForPrint(chapterno, 3)));
                                }
                                saveLocation = string.Format(@"C:\Users\ngprice\Desktop\Gantz\Chapter {0}\Gantz_Chapter{0}_{1}.jpg", PadIntForPrint(chapterno, 3), PadIntForPrint((i + 1), 2));
                                if (SaveImageFromURL(pages[i].ToString()))
                                {
                                    if (File.Exists(saveLocation))
                                    {
                                        Console.Write("File Created: " + saveLocation + "\n");
                                    }
                                    else Console.WriteLine("ERROR CREATING PAGE");
                                }
                            }
                        }
                    }
                    else
                    {
                        chapterno = Convert.ToInt32(chapterchoice);
                        ArrayList pages = GetPageImgSrc(chapterno);
                        pagelength = (pages.Count);
                        Console.WriteLine(string.Format("************PROCESSING CHAPTER {0}************", chapterno));
                        Console.WriteLine(string.Format("Found {0} pages.", pagelength));

                        for (int i = 0; i < pagelength; ++i)
                        {
                            if (!Directory.Exists(string.Format(@"C:\Users\ngprice\Desktop\Gantz\Chapter {0}\", PadIntForPrint(chapterno, 3))))
                            {
                                Directory.CreateDirectory(string.Format(@"C:\Users\ngprice\Desktop\Gantz\Chapter {0}\", PadIntForPrint(chapterno, 3)));
                            }
                            saveLocation = string.Format(@"C:\Users\ngprice\Desktop\Gantz\Chapter {0}\Gantz_Chapter{0}_{1}.jpg", PadIntForPrint(chapterno, 3), PadIntForPrint((i + 1), 2));
                            if (SaveImageFromURL(pages[i].ToString()))
                            {
                                Console.Write("File Created: " + saveLocation + "\n");
                            }
                        }
                    }
                    break;

                case 2:
                    Console.WriteLine("--Create .cbz--");
                    Console.Write("Enter start range: ");
                    int begin = Convert.ToInt32(Console.ReadLine());
                    Console.Write("Enter end range: ");
                    int end = Convert.ToInt32(Console.ReadLine());

                    DirectoryInfo di = Directory.CreateDirectory(@"C:\Users\ngprice\Desktop\Gantz\Gantz_Chapter_" + PadIntForPrint(begin,3) + "-" + PadIntForPrint(end,3));
                    
                    for (int i = begin; i <= end; i++)
                    {
                        string chapterdir = string.Format(@"C:\Users\ngprice\Desktop\Gantz\Chapter {0}\",PadIntForPrint(i,3));
                        string destinationdir = @"C:\Users\ngprice\Desktop\Gantz\Gantz_Chapter_" + PadIntForPrint(begin, 3) + "-" + PadIntForPrint(end, 3) + @"\Chapter_" + PadIntForPrint(i, 3);

                        string arguments = "\"" + destinationdir + "\" \"" + chapterdir + "\"";
                        if (Zip(arguments))
                        {
                            if (Directory.Exists(destinationdir))
                            {
                                Console.WriteLine(string.Format("Chapter created: {0}", destinationdir));
                            }
                        }

                        //string[] directories = Directory.GetFiles(chapterdir,"*.jpg");
                        //foreach (string dir in directories)
                        //{
                        //    int bslash = dir.LastIndexOf("\\");
                        //    string targetname = dir.Substring(bslash + 1, dir.Length - bslash - 1);
                        //    File.Copy(dir, @"C:\Users\ngprice\Desktop\Gantz\Gantz_Chapter_101-200\" + targetname);

                        //    targetname = targetname.Replace(".jpg", ".cbz");
                        //    string arguments = di.FullName + " " + targetname + " \"" + dir + "\"";
                        //    if (Zip(arguments))
                        //    {
                        //        if (File.Exists(targetname))
                        //            Console.WriteLine(string.Format("File created: {0}", targetname));
                        //    }
                        //}
                    }
                    break;

                default:
                    break;
            }
            
            Console.WriteLine("Press Enter to exit.");
            Console.ReadLine();
        }

        public static ArrayList GetPageImgSrc(int iChapter)
        {
            //http://www.mangareader.net/97-34409-1/gantz/chapter-304.html
            //index: C:\Users\ngprice\documents\visual studio 2010\Projects\luelinksDL\luelinksDL\indexHTML\gantzindex.html
            int seedno = 34409;//1162;
            int thelimit = 101;

            ArrayList al = new ArrayList();

            string gantzIndex = File.ReadAllText(@"C:\Users\ngprice\documents\visual studio 2010\Projects\luelinksDL\luelinksDL\indexHTML\gantzindex.html");

            int startindex = gantzIndex.IndexOf(String.Format("Gantz {0}",iChapter.ToString()));


        
            try
            {
                Console.WriteLine(string.Format("Scraping pages for chapter {0}...",iChapter.ToString()));
                for(int pageno=1; pageno < thelimit; pageno++)
                {
                    string seedURL = string.Format("http://www.mangareader.net/97-{0}-{1}/gantz/chapter-{2}.html",(seedno + iChapter).ToString(),pageno,iChapter);
                    string html = ScreenScrape(seedURL);
                    if (html.Length > 0)
                    {
                        al.Add(RipImageStringFromURI(html));
                        continue;
                    }
                    else break;
                    }
            }
            catch(Exception ex)
            {
                return al;
            }

            return al;
        }

        public static string PadIntForPrint(int i, int desiredlength)
        {
            string result = i.ToString();

            if (result.Length < desiredlength)
            {
                result = result.PadLeft(desiredlength, '0');
            }
            return result;
        }

        public static string RipImageStringFromURI(string HTML)
        {
            int index;
            string result;
            saveLocation = @"C:\Users\ngprice\documents\visual studio 2010\Projects\luelinksDL\luelinksDL\indexHTML\gantzindex.html";

            string myHTML = File.ReadAllText(saveLocation);

            index = myHTML.LastIndexOf("/97-34409-1/gantz/chapter-304.html");

            return myHTML;

            //int imgopenindex;
            //int srctagindex;
            //int closingsrctagindex;
            //int length;

            //imgopenindex = HTML.IndexOf("<img id=\"img\"");
            //srctagindex = HTML.IndexOf("src=\"", imgopenindex);
            //closingsrctagindex = HTML.IndexOf("\" alt=\"", srctagindex);

            //length = closingsrctagindex - srctagindex;

            //result = HTML.Substring(srctagindex + 5, length - 5);

            //return result;
        }

        public static string ScreenScrape(string url)
        {
            System.Net.WebClient client = new System.Net.WebClient();
            {
                return client.DownloadString(url);
            }
        }
        public static Boolean SaveImageFromURL(string myURLArray)
        {
            bool returned = false;

            byte[] imageBytes;
            HttpWebRequest imageRequest = (HttpWebRequest)WebRequest.Create(myURLArray);
            WebResponse imageResponse = imageRequest.GetResponse();

            Stream responseStream = imageResponse.GetResponseStream();

            using (BinaryReader br = new BinaryReader(responseStream))
            {
                imageBytes = br.ReadBytes(500000);
                br.Close();
            }
            responseStream.Close();
            imageResponse.Close();

            FileStream fs = new FileStream(saveLocation, FileMode.Create);
            BinaryWriter bw = new BinaryWriter(fs);
            try
            {
                bw.Write(imageBytes);
                returned = true;
            }
            catch (Exception ex)
            {
                returned = false;
                Console.WriteLine(ex.Message);
            }
            finally
            {
                fs.Close();
                bw.Close();
            }

            return returned;
        }

        public static Boolean Zip(string args)
        {
            Boolean breturns = false;
            try
            {
                string batchfile = @"C:\Users\ngprice\documents\visual studio 2010\Projects\luelinksDL\luelinksDL\createcbz.bat";

                System.Diagnostics.Process myProcess = new System.Diagnostics.Process();
                myProcess.StartInfo.Arguments = args;
                myProcess.StartInfo.FileName = batchfile;
                myProcess.StartInfo.UseShellExecute = false;
                myProcess.StartInfo.WorkingDirectory = @"C:\Program Files\WinRAR\";

                myProcess.Start();
                myProcess.WaitForExit();
                myProcess.Close();

                breturns = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
            return breturns;
        }
    }
}


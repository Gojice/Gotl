using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Gatling
{
    public static class MediaManager
    {
        public static List<MediaBox> Render(string path)
        {
            List<string> inputFile = new List<string>();
            List<string> logFile = new List<string>();

            if (Directory.Exists(path))
            {
                //Add kardane filha ta 1 directory dakheli:
                inputFile.AddRange(Directory.GetFiles(path));
                foreach (var directory in Directory.GetDirectories(path))
                {
                    inputFile.AddRange(Directory.GetFiles(directory));
                }

                //کوچک کردن حروف آدرس:
                inputFile = inputFile.Select(s => s.ToLowerInvariant()).ToList();

                //خوندن لاگ:
                if (File.Exists(Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "log.txt")))
                {
                    logFile.AddRange(File.ReadLines(Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "log.txt")).ToList());
                    logFile = logFile.Select(s => s.ToLowerInvariant()).ToList();
                }
                else
                {
                    var Mader = File.Create(Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "log.txt"));
                    Mader.Close();
                }

                //حذف ارسالی ها:
                inputFile = inputFile.Except(logFile).ToList();

                List<string> fileName = new List<string>();

                foreach (var file in inputFile)
                {
                    fileName.Add(Path.GetFileName(file));
                }

                //گروه کردن فایل ها
                var groups = fileName.GroupBy(f => f.Substring(0, f.IndexOf('.') - 1));

               return RenderMedia(groups, path);

            }
            else
            {
                return null;
            }
        }

        private static List<MediaBox> RenderMedia(IEnumerable<IGrouping<string, string>> groups,string path)
        {
            List<MediaBox> medias = new List<MediaBox>();
            List<MediaBox> sMedias = new List<MediaBox>();
            int Video = 0, Picture = 0;
            int VG = 0, PG = 0;
            int SVideo = 0, SPicture = 0;
            int SVG = 0, SPG = 0;

            int NumV = 0;
            int NumP = 0;

            foreach (var group in groups)
            {
                string Caption = "#بدون_شرح .\n .\n .\n .\n .\n .\n" + "@" + UserInfo.Default.Username + "\n :D";
                //Check Caption:
                if ((group.Where(f => f.ToLower().IndexOf("txt") > 0).Count() > 0))
                {
                    Caption = File.ReadAllText(path + "//" + group.Where(f => f.ToLower().IndexOf("txt") > 0).ToList()[0]);
                }

                NumP = 0;
                NumV = 0;
                foreach (var item in group)
                {
                    string[] Kind = item.Split('.');
                    if (Kind[1].ToLower() == "mp4")
                    {
                        NumV++;
                    }
                    else if (Kind[1].ToLower() == "jpg")
                    {
                        NumP++;
                    }
                }


                bool videoType = false;
                bool picType = false;

                //One Post:
                if (NumP == 1)
                {
                    foreach (var item in group)
                    {
                        string[] Kind = item.Split('.');
                        if (Kind[1].ToLower() == "mp4")
                        {
                            videoType = true;
                        }
                        else if (Kind[1].ToLower() == "jpg")
                        {
                            picType = true;
                        }
                    }
                    int X = group.Key.ToLower().IndexOf("p");
                    string XX = group.Where(f => f.ToLower().IndexOf("jpg") > 0).ToList()[0];



                    //Special:
                    if (group.Key.ToLower().IndexOf("s") == 0 && group.Key.ToLower().IndexOf("p") == 1)
                    {
                        if (videoType)
                        {
                            MediaBox bil = new MediaBox()
                            {
                                kind = ItemKind.SVideo,
                                Video = path + "\\" + group.Where(f => f.ToLower().IndexOf("mp4") > 0).ToList()[0],
                                caption = Caption,
                                location = ""
                            };
                            sMedias.Add(bil);
                            SVideo++;
                        }
                        else if (picType)
                        {
                            MediaBox bil = new MediaBox()
                            {
                                kind = ItemKind.SPicture,
                                Picture = path + "\\" + group.Where(f => f.ToLower().IndexOf("jpg") > 0).ToList()[0],
                                caption = Caption,
                                location = ""
                            };
                            sMedias.Add(bil);
                            SPicture++;
                        }
                    }
                    else
                    {
                        if (videoType)
                        {
                            MediaBox bil = new MediaBox()
                            {
                                kind = ItemKind.Video,
                                Video = path + "\\" + group.Where(f => f.ToLower().IndexOf("mp4") > 0).ToList()[0],
                                caption = Caption,
                                location = ""
                            };
                            medias.Add(bil);
                            Video++;
                        }
                        else if (picType)
                        {
                            MediaBox bil = new MediaBox()
                            {
                                kind = ItemKind.Picture,
                                Picture = path + "\\" + group.Where(f => f.ToLower().IndexOf("jpg") > 0).ToList()[0],
                                caption = Caption,
                                location = ""
                            };
                            medias.Add(bil);
                            Picture++;
                        }
                    }
                }

                //Gallery:
                else if (NumP > 1 || NumV > 1)
                {
                    foreach (var item in group)
                    {
                        string[] Kind = item.Split('.');
                        if (Kind[1].ToLower() == "mp4")
                        {
                            videoType = true;
                        }
                        else if (Kind[1].ToLower() == "jpg")
                        {
                            picType = true;
                        }
                    }

                    //Special:
                    if (group.Key.ToLower().IndexOf("s") == 0 && group.Key.ToLower().IndexOf("p") == 1)
                    {

                        if (videoType)
                        {
                            List<string> videos = new List<string>();
                            foreach (var P in group)
                            {
                                if (P.ToLower().IndexOf(".mp4") > 0)
                                {
                                    videos.Add(path + "\\" + P);
                                }
                            }


                            MediaBox bil = new MediaBox()
                            {
                                kind = ItemKind.SVideoGallery,
                                GVideo = videos.ToArray(),
                                caption = Caption,
                                location = ""
                            };

                            sMedias.Add(bil);
                            SVG++;
                        }
                        else
                        {
                            List<string> pictures = new List<string>();

                            foreach (var P in group)
                            {
                                if (P.ToLower().IndexOf(".jpg") > 0)
                                {
                                    pictures.Add(path + "\\" + P);
                                }
                            }

                            MediaBox bil = new MediaBox()
                            {
                                kind = ItemKind.SPictureGallery,
                                GPicture = pictures.ToArray(),
                                caption = Caption,
                                location = ""
                            };

                            sMedias.Add(bil);
                            SPG++;
                        }
                    }
                    else
                    {

                        if (videoType)
                        {
                            List<string> videos = new List<string>();

                            foreach (var P in group)
                            {
                                if (P.ToLower().IndexOf(".mp4") > 0)
                                {
                                    videos.Add(path + "\\" + P);
                                }
                            }

                            MediaBox bil = new MediaBox()
                            {
                                kind = ItemKind.VideoGallery,
                                GVideo = videos.ToArray(),
                                caption = Caption,
                                location = ""
                            };

                            medias.Add(bil);
                            VG++;
                        }
                        else
                        {
                            List<string> pictures = new List<string>();

                            foreach (var P in group)
                            {
                                if (P.ToLower().IndexOf(".jpg") > 0)
                                {
                                    pictures.Add(path + "\\" + P);
                                }
                            }

                            MediaBox bil = new MediaBox()
                            {
                                kind = ItemKind.PictureGallery,
                                GPicture = pictures.ToArray(),
                                caption = Caption,
                                location = ""
                            };

                            medias.Add(bil);
                            PG++;
                        }
                    }
                }
            }

            Shared.Media = new List<MediaBox>();
            Shared.Media.AddRange(medias);
            Shared.Media.AddRange(sMedias);
            return Shared.Media;
        }
    }
}

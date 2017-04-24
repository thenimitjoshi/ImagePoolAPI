using ImagePool.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Hosting;
using System.Web.Http;

namespace ImagePool.Controllers
{
    public class ImageServiceController : ApiController
    {
        #region Variables
        HttpResponseMessage response = new HttpResponseMessage();
        /// <summary>
        /// get and set error object
        /// </summary>
        public object ResponseObject { get; protected set; }
        #endregion

        #region Public Methods
        /// <summary>
        /// This method is created to upload the image.
        /// </summary>
        /// <returns></returns>
        [HttpPost, ActionName("UploadImage")]
        public async Task<HttpResponseMessage> UploadImage()
        {

            string sub = string.Empty;
            int ImageResult = 0;

            if (!Request.Content.IsMimeMultipartContent())
            {
                return Request.CreateResponse<string>(HttpStatusCode.BadRequest, "This is not Mime Multipart Content");
            }

            //This is the static path for user images
            string rootPath = HostingEnvironment.MapPath("~/Uploads/");
            //get the path where content of MIME multipart body parts are written to
            var provider = new MultipartFormDataStreamProvider(HostingEnvironment.MapPath("~/Uploads/"));
            try
            {
                //read multi part data
                await Request.Content.ReadAsMultipartAsync(provider);

                //get the file name from returned data
                string uploadedFileName = provider.FileData.First().Headers.ContentDisposition.FileName;
                if (uploadedFileName.Length == 0)
                {
                    return Request.CreateResponse<string>(HttpStatusCode.BadRequest, "Image file not found");
                }

                string uploadedFileNameWithBackSlash = uploadedFileName.LastIndexOf("\\").ToString();

                int position = uploadedFileName.LastIndexOf('\\');


                string fileNameext = string.Empty;
                var fileName = uploadedFileName.Substring(uploadedFileName.LastIndexOf(("\\")) + 1);

                int fileExtPos = fileName.LastIndexOf(".");

                if (fileExtPos >= 0)
                    fileNameext = (fileName.Substring(fileExtPos, (fileName.Length - fileExtPos))).Replace("\"", "");

                sub = fileNameext;
                //check file is image type
                string[] extensionArray = { ".jpg,.png,.JPG,.PNG,.bmp,.gif" };

                List<string> extentions = extensionArray.FirstOrDefault().Split(',').Where(c => c.Equals(sub.ToLower())).ToList();
                if (extentions.Count == 0)
                {
                    return Request.CreateResponse<string>(HttpStatusCode.BadRequest, "file not supported");
                }

                String strFinalFileName = string.Empty;
                strFinalFileName = Guid.NewGuid().ToString().Trim().Replace(" ", "").Replace("-", "") + sub;

                String strDatabaseFilePath = string.Empty;

                rootPath = rootPath + "\\Images\\";
                strDatabaseFilePath = "/Uploads/" + "/Images/" + strFinalFileName;

                if (!System.IO.Directory.Exists(rootPath))
                {
                    System.IO.Directory.CreateDirectory(rootPath);
                }

                //do stuff to save the file path into the datbase
                //{
                    // save the file path to the database
                //}

                // After saving image compress it and save image to the directory.
                var fullFilePath = provider.FileData.First().LocalFileName + sub;
                var copyToPath = rootPath + strFinalFileName;
                FileStream stream = File.OpenRead(provider.FileData.First().LocalFileName);
                byte[] fileBytes = new byte[stream.Length];

                stream.Read(fileBytes, 0, fileBytes.Length);
                double fileSizeKB = stream.Length / 1024;

                ImageResult = await NewCompressImageWithNewDimensions(stream, copyToPath, provider.FileData.First().Headers.ContentDisposition.Size.HasValue ? Convert.ToDouble(provider.FileData.First().Headers.ContentDisposition.Size) : 0);
                stream.Close();

                //delete temp file
                if (File.Exists(provider.FileData.First().LocalFileName))
                {
                    File.Delete(provider.FileData.First().LocalFileName);
                }

                response = Request.CreateResponse(HttpStatusCode.OK, "Image Uploaded Successfully!");
            }
            catch (Exception ex)
            {
                response = Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
            return response;
        }
        #endregion

        #region TodaysLineUp
        [HttpGet, ActionName("TodaysLineUp")]
        public HttpResponseMessage TodaysLineUp(string offset)
        {            
            try
            {
                var todaysLineUpListResponse = GetTodaysLineUpList(null);

                if (todaysLineUpListResponse != null)
                {
                    ResponseObject = new ResponseObj(Convert.ToInt32(ConstantAndEnum.Status.Ok), Convert.ToInt32(ConstantAndEnum.ErrorCode.General), null);
                    response = Request.CreateResponse(HttpStatusCode.OK, new { ResponseObject, todaysLineUpListResponse });
                }
                else
                {
                    response = Request.CreateResponse(HttpStatusCode.OK, todaysLineUpListResponse);
                }
            }
            catch (Exception ex)
            {
                response = Request.CreateResponse(HttpStatusCode.OK, ex.Message);

            }
            return response;
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// This method is created to compress the image.
        /// </summary>
        /// <param name="stream">file stream</param>
        /// <param name="copyToPath">target path</param>
        /// <param name="fileSizeKB">file size</param>
        /// <returns></returns>
        private async Task<int> NewCompressImageWithNewDimensions(FileStream stream, string copyToPath, double fileSizeKB)
        {
            int result = 0;
            try
            {
                using (var image = Image.FromStream(stream))
                {
                    double scaleFactor;
                    if (fileSizeKB <= 900)
                    {
                        scaleFactor = 0.9;
                    }
                    else if (fileSizeKB <= 1500)
                    {
                        scaleFactor = 0.8;
                    }
                    else if (fileSizeKB <= 2000)
                    {
                        scaleFactor = 0.7;
                    }
                    else
                    {
                        scaleFactor = 0.3;
                    }
                    var newWidth = (int)(image.Width * scaleFactor);
                    var newHeight = (int)(image.Height * scaleFactor);
                    var CompressImage = new Bitmap(newWidth, newHeight);
                    var CompressImageGraph = Graphics.FromImage(CompressImage);
                    CompressImageGraph.CompositingQuality = CompositingQuality.HighQuality;
                    CompressImageGraph.SmoothingMode = SmoothingMode.HighQuality;
                    CompressImageGraph.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    var imageRectangle = new Rectangle(0, 0, newWidth, newHeight);
                    CompressImageGraph.DrawImage(image, imageRectangle);
                    CompressImage.Save(copyToPath, image.RawFormat);
                    if (File.Exists(copyToPath))
                    {
                        result = 1;
                    }
                    else
                    {
                        result = 2;
                    }
                }
                return result;
            }
            catch (Exception)
            {
                return result;
            }
        }

        /// <summary>
        /// This method is created to get today's lineup list.
        /// </summary>
        /// <param name="offset"></param>
        /// <returns>It will return all leagues and games details in todays lineup list.</returns>
        protected Dictionary<string, Dictionary<string, LeagueInfo>> GetTodaysLineUpList(string offset)
        {
            Dictionary<string, List<TodaysLineUp>> objTodaysLineupList = null;
            List<TodaysLineUp> objTodaysLineUpForLive = new List<TodaysLineUp>();
            List<TodaysLineUp> objTodaysLineUpForLater = new List<TodaysLineUp>();
            Dictionary<string, LeagueInfo> liveLeagueInfo = new Dictionary<string, LeagueInfo>();
            Dictionary<string, LeagueInfo> laterLeagueInfo = new Dictionary<string, LeagueInfo>();
            List<string> leagueNameList = new List<string>();
            Dictionary<string, Dictionary<string, LeagueInfo>> leagueInfo = new Dictionary<string, Dictionary<string, LeagueInfo>>();

            try
            {
                //getting data
                objTodaysLineupList = new TodaysLineUpDal().GetTodaysLineUpList(offset);
                //getting Live Section Data
                objTodaysLineupList.TryGetValue("Live", out objTodaysLineUpForLive);
                ///getting Later Section Data
                objTodaysLineupList.TryGetValue("Later", out objTodaysLineUpForLater);

                //Check if Live Section Data is available
                if (!object.Equals(objTodaysLineUpForLive, null))
                {
                    if (objTodaysLineUpForLive.Count > 0)
                    {
                        leagueNameList = objTodaysLineUpForLive.Select(s => s.GameTypeName).Distinct().ToList();

                        if (leagueNameList.Count > 0)
                        {
                            foreach (var leagueName in leagueNameList)
                            {
                                var teams = new List<TeamInfo>();
                                List<TeamInfo> teamDetailList = new List<TeamInfo>();

                                var leagueDetailList = (from x in objTodaysLineUpForLive where x.GameTypeName == leagueName select x).ToList();
                                foreach (var leagueDetail in leagueDetailList)
                                {
                                    var team = new TeamInfo();
                                    team.GameScheduleID = leagueDetail.GameScheduleID;
                                    team.AwayTeamId = leagueDetail.AwayTeamId;
                                    team.AwayTeamLogoUrl = leagueDetail.AwayTeamLogoUrl;
                                    team.AwayTeamName = leagueDetail.AwayTeamName;
                                    team.AwayTeamUrl = leagueDetail.AwayTeamUrl;
                                    team.HomeTeamID = leagueDetail.HomeTeamID;
                                    team.HomeTeamLogoUrl = leagueDetail.HomeTeamLogoUrl;
                                    team.HomeTeamName = leagueDetail.HomeTeamName;
                                    team.HomeTeamUrl = leagueDetail.HomeTeamUrl;
                                    team.Network = leagueDetail.Network;
                                    team.StartTime = leagueDetail.StartTime;
                                    team.EndTime = leagueDetail.EndTime;
                                    team.TotalCastersCount = leagueDetail.LeagueTotalCastersCount;
                                    //team.CasterList = casterDAL.GetPublicGameCastersList(1, 0);
                                    teams.Add(team);
                                }
                                teamDetailList.AddRange(teams);

                                liveLeagueInfo.Add(leagueName, new LeagueInfo { League = new League { LeagueName = leagueName, LeagueTotalListenersCount = objTodaysLineUpForLive.Select(s => s.LeagueTotalListenersCount).FirstOrDefault(), LeagueURL = objTodaysLineUpForLive.Select(s => s.GameTypeURL).FirstOrDefault() }, Teams = teamDetailList });
                            }
                            leagueInfo.Add("Live", liveLeagueInfo);
                        }
                    }
                }

                //Checking if Later Section Data is available
                if (!object.Equals(objTodaysLineUpForLater, null))
                {
                    if (objTodaysLineUpForLater.Count > 0)
                    {
                        leagueNameList = objTodaysLineUpForLater.Select(s => s.GameTypeName).Distinct().ToList();

                        if (leagueNameList.Count > 0)
                        {
                            foreach (var leagueName in leagueNameList)
                            {
                                var teams = new List<TeamInfo>();
                                List<TeamInfo> teamDetailList = new List<TeamInfo>();

                                var leagueDetailList = (from x in objTodaysLineUpForLater where x.GameTypeName == leagueName select x).ToList();
                                foreach (var leagueDetail in leagueDetailList)
                                {
                                    var team = new TeamInfo();
                                    team.GameScheduleID = leagueDetail.GameScheduleID;
                                    team.AwayTeamId = leagueDetail.AwayTeamId;
                                    team.AwayTeamLogoUrl = leagueDetail.AwayTeamLogoUrl;
                                    team.AwayTeamName = leagueDetail.AwayTeamName;
                                    team.AwayTeamUrl = leagueDetail.AwayTeamUrl;
                                    team.HomeTeamID = leagueDetail.HomeTeamID;
                                    team.HomeTeamLogoUrl = leagueDetail.HomeTeamLogoUrl;
                                    team.HomeTeamName = leagueDetail.HomeTeamName;
                                    team.HomeTeamUrl = leagueDetail.HomeTeamUrl;
                                    team.Network = leagueDetail.Network;
                                    team.StartTime = leagueDetail.StartTime;
                                    team.EndTime = leagueDetail.EndTime;
                                    team.TotalCastersCount = leagueDetail.LeagueTotalCastersCount;
                                    teams.Add(team);
                                }
                                teamDetailList.AddRange(teams);

                                laterLeagueInfo.Add(leagueName, new LeagueInfo { League = new League { LeagueName = leagueName, LeagueTotalListenersCount = objTodaysLineUpForLater.Select(s => s.LeagueTotalListenersCount).FirstOrDefault(), LeagueURL = objTodaysLineUpForLater.Select(s => s.GameTypeURL).FirstOrDefault() }, Teams = teamDetailList });
                            }
                            leagueInfo.Add("Later", laterLeagueInfo);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return null;
            }
            return leagueInfo;
        }
        #endregion
    }
}

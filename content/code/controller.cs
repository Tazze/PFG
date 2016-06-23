[HttpPost]
public async Task < JsonResult > GetUserInfoStream() {
 createReceivedDirectory(Server);
 var stream = Request.InputStream;
 FileStream fileStream = null;
 try {
  fileStream = System.IO.File.Create
  (Server.MapPath("~/Received/stream.jpg"));
 } catch (Exception ex) {
  new TelemetryClient().TrackException(ex);
 }
 stream.Seek(0, SeekOrigin.Begin);
 stream.CopyTo(fileStream);
 fileStream.Close();


 string accessToken = await CheckUserIdentity();
 return Json(new UserDataEntity(GetUserTasks(accessToken), 
 GetUserCalendarEvents(accessToken), GetUserEmails(accessToken)));

}

private static void createReceivedDirectory
(HttpServerUtilityBase Server) {
 // Specify the directory you want to manipulate.
 string path = Server.MapPath("~/Received");

 try {
  // Determine whether the directory exists.
  if (Directory.Exists(path)) {
   //Console.WriteLine("That path exists already.");
   return;
  }

  // Try to create the directory.
  DirectoryInfo di = Directory.CreateDirectory(path);
 } catch (Exception e) {
  new TelemetryClient().TrackException(e);
 } finally {}
}

static string detectUrl = 
"https://api.projectoxford.ai/face/v1.0/detect
?returnFaceId=true&returnFaceLandmarks=false";
static string verifyUrl = 
"https://api.projectoxford.ai/face/v1.0/verify";

private async static Task < string > CheckUserIdentity() {
 //TODO: create FaceId for image
 //if there's a face, beging verifying process
 //if faceId to check against is nonexistent or expired, 
 //create a new faceId.
 //if match is found, return id, else return empty string.

 var context = new ApplicationDbContext();
 var users = context.Users;
 JToken token = await postAndReturn(detectUrl, 
 new DetectDataEntity
 ("http://mirrorserverasmvc20160613125538.azurewebsites.net/
 Received/Stream.jpg"));
 var streamFaceId = (string) token.SelectToken("faceId");
 foreach(ApplicationUser user in users) {
  if (await Verify(user, streamFaceId)) {
   var id = user.Id;
   var sContext = new ApplicationDbContext();
   var usr = sContext.Users.Find(id);
   foreach(IdentityUserLogin login in usr.Logins) {
    return login.ProviderKey;
   }
  }
 }
 return "";
}


private async static Task < bool > Verify
(ApplicationUser user, string streamFaceId) {
 //https://api.projectoxford.ai/face/v1.0/detect
 //?returnFaceId=true&returnFaceLandmarks=false&subscription-key=
 // <Your subscription key>
 //+ JSON of url
 // example:
 //"url":"http://example.com/1.jpg"

 //https://api.projectoxford.ai/face/v1.0/verify
 //&subscription-key= <Your subscription key>
 //+ JSON of two face id
 //example:
 //"faceId1":"c5c24a82-6845-4031-9d5d-978df9175426",
 //"faceId2":"015839fb-fbd9-4f79-ace9-7675fc2f1dd9"

 if (DateTime.SpecifyKind(user.LastUpdate, DateTimeKind.Utc)
 .AddHours(24) < DateTime.UtcNow) {
  //update FaceId
  user.LastUpdate = DateTime.Now;
  if (user.PhotoUrl != null) {
   JToken detectToken = await 
   postAndReturn(detectUrl, new DetectDataEntity(user.PhotoUrl));
   user.FaceId = (string) detectToken.SelectToken("faceId");
  }
 }
 //check stream against user

 if (user.FaceId != null && streamFaceId != null) {
  JToken token = await 
postAndReturn(verifyUrl, new VerifyDataEntity
(user.FaceId, streamFaceId));

  return (bool) token.SelectToken("isIdentical");
 } else {
  return false;
 }

}

private async static Task < JToken > postAndReturn
(string url, object data) {
 var client = new HttpClient();
 client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", 
 "45183365fcc74d6093f0ac011d7ba1ce");

 var serializedData = new StringContent
 (new JavaScriptSerializer().Serialize(data), 
 Encoding.UTF8, "application/json");
 
 var response = await client.PostAsync(url, serializedData);

 var responseString = await response.Content.ReadAsStringAsync();
 try {
  var array = JArray.Parse(responseString);
  return (array.First);
 } catch (Exception e) {
  var obj = JObject.Parse(responseString);
  return obj;
 }
}
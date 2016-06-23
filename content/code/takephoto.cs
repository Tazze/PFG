public async Task TakePhotoAsync() {
 stream = new InMemoryRandomAccessStream();

 try {
  //Debug.WriteLine("Taking photo...");
  await _mediaCapture.CapturePhotoToStreamAsync(new ImageEncodingProperties {
   Subtype = "PNG", Width = 600, Height = 800
  }, stream);
  //Debug.WriteLine("Photo taken!");

  var photoOrientation = ConvertOrientationToPhotoOrientation(GetCameraOrientation());
  //await ReencodeAndSavePhotoAsync(stream, "photo.jpg", photoOrientation);
 } catch (Exception ex) {
  //Debug.WriteLine("Exception when taking a photo: {0}", ex.ToString());
 }

}
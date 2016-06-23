//inicialización
var allVideoDevices =
await DeviceInformation.FindAllAsync(DeviceClass.VideoCapture);
var cameraDevice = cameraDevice ?? allVideoDevices.FirstOrDefault();
if (cameraDevice == null)
    {
        //no hay ninguna cámara instalada
        return;
    }
var mediaCapture = new MediaCapture();
var mediaInitSettings = new MediaCaptureInitializationSettings 
{ VideoDeviceId = cameraDevice.Id };
await mediaCapture.InitializeAsync(mediaInitSettings);

//captura
var stream = new InMemoryRandomAccessStream();
await mediaCapture.CapturePhotoToStreamAsync(new 
ImageEncodingProperties { Subtype = "PNG", Width = 600, Height = 800 }, stream);
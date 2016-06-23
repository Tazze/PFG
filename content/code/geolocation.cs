Geolocator geolocator = new Geolocator { DesiredAccuracyInMeters = 0 };
Geoposition pos = await geolocator.GetGeopositionAsync();
pos.Coordinate.Latitude     //Latitud de la posición geográfica
pos.Coordinate.Longitude    //Longitud de la posición geográfica
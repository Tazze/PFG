app.UseMicrosoftAccountAuthentication(
    clientId: "",
    clientSecret: "");

app.UseTwitterAuthentication(
    consumerKey: "",
    consumerSecret: "");

app.UseFacebookAuthentication(
    appId: "",
    appSecret: "");

app.UseGoogleAuthentication(new GoogleOAuth2AuthenticationOptions()
{
    ClientId = "",
    ClientSecret = ""
});
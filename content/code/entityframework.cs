var context = new ApplicationDbContext();
var users = context.Users;
    foreach (ApplicationUser user in users)
    {
        if (await Verify(user, streamFaceId))
        {
            //hace falta un segundo contexto para una nueva consulta
            var sContext = new ApplicationDbContext();
            var usr = sContext.Users.Find(user.id);
            foreach (IdentityUserLogin login in usr.Logins)
            {
                        return login.ProviderKey;
            }
        }
    }
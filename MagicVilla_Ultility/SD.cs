namespace MagicVilla_Ultility
{
    public static class SD
    {
        public enum ApiType {
            GET,
            POST,
            PUT,
            DELETE
        }

        public enum Role{
            Admin,
            User,
            CustomRole
        }

        public static string SessionToken = "JWTToken";
    }

}
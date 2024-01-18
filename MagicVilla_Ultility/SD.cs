namespace MagicVilla_Ultility
{
    public static class SD
    {
        public enum ApiType {
            GET,
            POST,
            PUT,
            DELETE
        };

        public enum Role{
            Admin,
            User,
            CustomRole
        };

        public enum ContentType {
            Json,
            MultipartFormData
        };

        public static string AccessToken = "JWTToken";
        public static string RefreshToken = "RefreshToken";
        public static string CurrentAPIVersion = "v2";
        

        
    }

}
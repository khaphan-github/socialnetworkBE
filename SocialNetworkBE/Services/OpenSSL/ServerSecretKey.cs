namespace SocialNetworkBE.Services.OpenSSL {
    public static class ServerSecretKey {

        private static string GetKeyFromFile(string filePath) {
            return File.ReadAllText(@filePath, Encoding.UTF8);
        }

        public static string PublicKey() {
            string publicKeyPath = "./public-2048.key";
            return GetKeyFromFile(publicKeyPath);
        }

        public static string PrivateKey() {
            string privateKeyPath = "./private-2048.key";
            return GetKeyFromFile(privateKeyPath);
        }
    }
}
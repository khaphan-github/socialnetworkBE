using BCrypt.Net;
using SocialNetworkBE.ServerConfiguration;

namespace SocialNetworkBE.Services.Hash {
    public class BCryptService {
        public string GetRandomSalt() {
            int randomRound = 30;
            return BCrypt.Net.BCrypt.GenerateSalt(randomRound);
        }

        public string GetHashCode(string salt, string textToHash, string secretKey) {
            return salt + textToHash + secretKey;
        }
        public string HashStringBySHA512(string textToHash) {
            if (textToHash == null) return null;
            
            string hashResult =
                BCrypt.Net.BCrypt.EnhancedHashPassword(textToHash, HashType.SHA512);

            return hashResult;
        }

        public bool ValidateStringAndHashBySHA512(string text, string hash) {
            if (text == null) return false;
            if (hash == null) return false;

            return BCrypt.Net.BCrypt.EnhancedVerify(text, hash, HashType.SHA512);
        }
    }
}
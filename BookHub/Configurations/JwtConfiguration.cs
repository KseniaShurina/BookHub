namespace BookHub.Configurations
{
    /// <summary>
    /// Represents configuration settings for JSON Web Tokens (JWT).
    /// </summary>
    public class JwtConfiguration
    {
        /// <summary>
        /// Gets the secret key used for signing JWT tokens.
        /// </summary>
        public string SecretKey { get; private set; }
        /// <summary>
        /// Gets the issuer of the JWT tokens.
        /// </summary>
        public string Issuer { get; private set; }
        /// <summary>
        /// Gets the audience for which the JWT tokens are intended.
        /// </summary>
        public string Audience { get; private set; }

        // Private constructor to prevent direct instantiation.
        private JwtConfiguration()
        {
            SecretKey = string.Empty;
            Issuer = string.Empty;
            Audience = string.Empty;
        }

        /// <summary>
        /// Creates an instance of <see cref="JwtConfiguration"/>
        /// by binding settings from the provided configuration.
        /// </summary>
        /// <param name="configuration">
        /// The configuration instance containing JWT settings.
        /// </param>
        /// <returns>An instance of <see cref="JwtConfiguration"/> fill with values from the configuration.</returns>
        public static JwtConfiguration Create(IConfiguration configuration)
        {
            var config = new JwtConfiguration();
            configuration.GetSection("Jwt").Bind(config, o =>
            {
                o.BindNonPublicProperties = true;
            });

            return config;
        }
    }
}

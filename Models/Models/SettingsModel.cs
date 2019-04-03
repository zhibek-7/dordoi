using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Models.Models
{
    [DataContract]
    public class SettingsModel
    {

        private SettingsModel() { }

        [DataContract]
        public class ConnectionStringsModel
        {
            [DataMember]
            public readonly string db_connection;
            [DataMember]
            public readonly string db_connection_old;
            [DataMember]
            public readonly string db_connection_loc;
        }
        [DataMember]
        public readonly ConnectionStringsModel ConnectionStrings;

        [DataContract]
        public class LoggingModel
        {
            [DataContract]
            public class LogLevelModel
            {
                [DataMember]
                public readonly string Default;
            }
            [DataMember]
            public readonly LogLevelModel LogLevel;
        }
        [DataMember]
        public readonly LoggingModel Logging;

        [DataContract]
        public class EmailModel
        {
            [DataMember]
            public readonly string Login;
            [DataMember]
            public readonly string Password;
            [DataMember]
            public readonly string Host;
            [DataMember]
            public readonly string Port;
        }
        [DataMember]
        public readonly EmailModel Email;

        [DataMember]
        public readonly string AllowedHosts;

        [DataContract]
        public class AuthenticationOptionsModel
        {
            [DataMember]
            public readonly string ISSUER;
            [DataMember]
            public readonly string KEY;
            [DataMember]
            public readonly int LIFETIME;
        }
        [DataMember]
        public readonly AuthenticationOptionsModel AuthenticationOptions;

        [DataContract]
        public class HostModel
        {
            [DataMember]
            public readonly string protocol;
            [DataMember]
            public readonly string name;
        }
        [DataMember]
        public readonly HostModel host;

    }
}

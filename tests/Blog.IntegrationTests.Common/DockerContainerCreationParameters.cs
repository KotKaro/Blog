using System.Collections.Generic;
using Docker.DotNet.Models;

namespace Blog.IntegrationTests.Common
{
    public static class DockerContainerCreationParameters
    {
        private const string MysqlContainerName = "mysql-tests";

        public static CreateContainerParameters GetMySqlContainerParameters(int hostPort)
        {
            return new CreateContainerParameters
            {
                Name = MysqlContainerName,
                Env = new List<string>
                {
                    "MYSQL_ROOT_PASSWORD=toor",
                    "MYSQL_DATABASE=blog",
                    "MYSQL_USER=karol",
                    "MYSQL_PASSWORD=toor"
                },
                ExposedPorts = new Dictionary<string, EmptyStruct>
                {
                    ["3306"] = new EmptyStruct()
                },
                HostConfig = new HostConfig
                {
                    PortBindings = new Dictionary<string, IList<PortBinding>>
                    {
                        ["3306"] = new List<PortBinding> { new PortBinding { HostIP = "0.0.0.0", HostPort = $"{hostPort}" } }
                    }
                },
                Image = "mysql:8.0.21",
            };
        }
    }
}

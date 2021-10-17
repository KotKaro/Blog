using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using Docker.DotNet;
using Docker.DotNet.Models;

namespace Blog.IntegrationTests.Common
{
    public class DockerContainerStarter
    {
        private readonly List<(string, int)> _runningContainers;

        private DockerClient DockerClient { get; }

        public DockerContainerStarter()
        {
            DockerClient = new DockerClientConfiguration(new Uri(GetDockerEngineUri())).CreateClient();
            _runningContainers = new List<(string, int)>();
        }

        public async Task<(string, int)> GetMysqlContainer()
        {
            var port = GetRandomPort();
            var result = await GetContainer(DockerContainerCreationParameters.GetMySqlContainerParameters(port), port);

            await WaitUntilContainerLogStops(result);

            return result;
        }

        // ReSharper disable once UnusedMember.Global
        public async Task PruneContainers()
        {
            foreach (var (containerId, _) in _runningContainers)
            {
                if (await DockerClient.Containers.StopContainerAsync(
                    containerId,
                    new ContainerStopParameters(),
                    CancellationToken.None
                ))
                {
                    await DockerClient.Containers.RemoveContainerAsync(
                        containerId,
                        new ContainerRemoveParameters(),
                        CancellationToken.None
                    );
                }
            }
        }

        private async Task<(string, int)> GetContainer(CreateContainerParameters createContainerParameters, int? port = null)
        {
            port ??= GetRandomPort();

            if (await DoesContainerAlreadyExistsAsync(createContainerParameters))
            {
                return await GetExistingContainerInfoAsync(createContainerParameters);
            }

            return await RunNewContainerAsync(createContainerParameters, port.Value);
        }

        private async Task<(string, int)> RunNewContainerAsync(CreateContainerParameters createContainerParameters, int port)
        {
            var container = await DockerClient.Containers.CreateContainerAsync(createContainerParameters);
            await StartContainer(createContainerParameters.Image, container.ID);

            var containerInfo = new ValueTuple<string, int>(container.ID, port);
            _runningContainers.Add(containerInfo);

            return containerInfo;
        }

        private async Task<(string, int)> GetExistingContainerInfoAsync(CreateContainerParameters createContainerParameters)
        {
            var existingContainer = await GetExistingContainerWithName(createContainerParameters);
            int? port = existingContainer.Ports.First(x => !string.IsNullOrWhiteSpace(x.IP)).PublicPort;
            return (existingContainer.ID, port.Value);
        }

        private async Task<bool> DoesContainerAlreadyExistsAsync(CreateContainerParameters createContainerParameters)
        {
            return await GetExistingContainerWithName(createContainerParameters) != null;
        }

        private static Progress<string> GetProgressWhichCancelTokenWhenLogsStop(CancellationTokenSource tokenSource)
        {
            var counter = 0;
            var adjustedCounter = 0;

            return new Progress<string>(_ =>
            {
                Task.Factory.StartNew(() => { counter++; }, tokenSource.Token)
                    .ContinueWith(_ => Task.Delay(2000, tokenSource.Token), tokenSource.Token)
                    .ContinueWith(async task =>
                    {
                        await task;
                        if (adjustedCounter == counter)
                        {
                            tokenSource.Cancel();
                            return;
                        }

                        adjustedCounter = counter;
                    }, tokenSource.Token);
            });
        }

        private async Task StartContainer(string image,
            string containerId)
        {
            if (!await DockerClient.Containers.StartContainerAsync(containerId, new ContainerStartParameters
            {
                DetachKeys = $"d={GetImageName(image)}"
            }))
            {
                throw new Exception($"Could not start container: {containerId}");
            }
        }

        private async Task WaitUntilContainerLogStops((string, int) result)
        {
            var tokenSource = new CancellationTokenSource();

            try
            {
                await DockerClient.Containers.GetContainerLogsAsync(
                    result.Item1,
                    new ContainerLogsParameters
                    {
                        ShowStdout = true,
                        Follow = true,
                        Timestamps = true,
                    },
                    tokenSource.Token,
                    GetProgressWhichCancelTokenWhenLogsStop(tokenSource)
                );
            }
            catch
            {
                // ignored
            }
        }

        private async Task<ContainerListResponse> GetExistingContainerWithName(CreateContainerParameters createContainerParameters)
        {
            var containerList = await DockerClient.Containers.ListContainersAsync(new ContainersListParameters());

            if (containerList == null)
            {
                return new ContainerListResponse();
            }

            return containerList.FirstOrDefault(
                x => x.Image == createContainerParameters.Image
                     && x.Names.Any(containerName => containerName.Contains(createContainerParameters.Name))
            );
        }

        private static string GetImageName(string image)
        {
            return image.Split(":")[0];
        }

        private static int GetRandomPort()
        {
            return new Random((int)DateTime.UtcNow.Ticks).Next(10000, 12000);
        }

        private static string GetDockerEngineUri()
        {
            const string macUrl = "unix:///var/run/docker.sock";
            const string windowsUrl = "npipe://./pipe/docker_engine";
            
            var isWindows = RuntimeInformation.IsOSPlatform(OSPlatform.Windows);

            return isWindows ? windowsUrl : macUrl;
        }
    }
}

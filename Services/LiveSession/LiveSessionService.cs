using Docker.DotNet;
using Docker.DotNet.Models;

namespace souschef.server.Services.LiveSession
{
    public class LiveSessionService : ILiveSessionService
    {
        readonly string imageName = "souschef-live-session";
        readonly string containerName = "souschef-live-session";
        readonly string staticLocalIP = "";

        private async Task<bool> ImageExist(IDockerClient client, string imageName)
        {
            try
            {
                await client.Images.InspectImageAsync(imageName);
                return true;
            }
            catch (Docker.DotNet.DockerApiException)
            {
                return false;
            }
        }

        private async Task<ContainerInspectResponse?> ContainerExist(IDockerClient client, string containerName)
        {
            try
            {
                var response = await client.Containers.InspectContainerAsync(containerName);
                return response;
            }
            catch (Docker.DotNet.DockerApiException)
            {
                return null;
            }
        }

        // TEMPORARY: Can only start 1 container, AWS will fix this
        public async Task<string?> Start()
        {
            try
            {
                DockerClient client = new DockerClientConfiguration(new Uri("unix:///var/run/docker.sock")).CreateClient();

                if (!await ImageExist(client, imageName))
                {
                    Console.WriteLine("Image {0} does not exist!", imageName);
                    return null;
                }

                var containerExistResponse = await ContainerExist(client, containerName);

                if (containerExistResponse != null)
                {
                    if (containerExistResponse.State.Running)
                    {
                        await client.Containers.StopContainerAsync(containerExistResponse.ID, new ContainerStopParameters { });
                    }
                    await client.Containers.RemoveContainerAsync(containerExistResponse.ID, new ContainerRemoveParameters { });
                }

                // Create a container using your GO Websocket image
                var containerCreateResponse = await client.Containers.CreateContainerAsync(new CreateContainerParameters
                {
                    Image = imageName,
                    ExposedPorts = new Dictionary<string, EmptyStruct>
                    {
                        { "8080/tcp", new EmptyStruct() }
                    },
                    HostConfig = new HostConfig
                    {
                        PortBindings = new Dictionary<string, IList<PortBinding>>
                        {
                            { "8080/tcp", new List<PortBinding> { new PortBinding { HostPort = "8080" } } }
                        }
                    },
                    Name = containerName,
                });

                // Start the container
                await client.Containers.StartContainerAsync(containerCreateResponse.ID, null);

                return staticLocalIP;
            }
            catch (Docker.DotNet.DockerApiException ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }
    }
}

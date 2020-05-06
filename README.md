# HostedServiceIoTHub
Web API with a  background hosted service that will maintain device clients to Azure IoT HuB. Useful as a REST API to receive high scale (high rate and low latency) http messages from devices/ gateways to be sent toAzure IoT Hub. The API will maintain a MQTT connection per device and send the device messages on that channel to Azure IoTHub. The http message needs to include  the device id in the header and the payload in the body. The API can be deployed as a Docker contaner also. To use your configuration file for the device conection strings, mount your appsettings.json to the /app/appsettings.json file in the container.

Note: There is another  project that has been tested for higher throughput ( ~ 250K per min or ~ 4K RPS) with Azure IoT HUB.
Will post the repo link here shortly

Load Test Observations:

IoT Hub Metric (Telemetry messages sent, Count):
11.52K RPM 

Configuration:
IoTHub    S3, 32 Partitions, 1 TPUnits
Devices    4  connected
IoTClient  2  Docker containers each connecting to one DeviceId 
Load       2  JMeter jobs, 500 threads each, 1 sec Ramp time, each job loading one container 

Disclaimer:
THIS CODE IS A SAMPLE AND SHOULD NOT BE USED IN PRODUCTION

# HostedServiceIoTHub
Web API with a  background hosted service that will maintain device clients to Azure IoT HuB. Useful as a REST API to receive high scale (high rate and low latency) http messages from devices/ gateways to be sent toAzure IoT Hub. The API will maintain a MQTT connection per device and send the device messages on that channel to Azure IoTHub. The http message needs to include  the device id in the header and the payload in the body. The API can be deployed as a Docker contaner also. To use your configuration file for the device conection strings, mount your appsettings.json to the /app/appsettings.json file in the container.

Load Test Observations:

IoT Hub Metric (Telemetry messages sent, Count):
11.52K messages 

Configuration:
IoTHub    S3, 32 Partitions, 2 TPUnits
Devices    4  connected
IoTClient  4  Docker containers each connecting to one DeviceId (Nw Outbound flows max 9K)
Load       4  JMeter jobs, 500 threads each, 1 sec Ramp time, each job loading one container 



Disclaimer:
THIS CODE IS A SAMPLE AND SHOULD NOT BE USED IN PRODUCTION

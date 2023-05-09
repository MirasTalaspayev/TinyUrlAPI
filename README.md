To build docker image:
```
docker build -t tinyurlapi:latest . 
```
To run tinyurlapi container:
```
docker run -d -p 5000:5000 -p 5001:5001 -e ASPNETCORE_HTTP_PORT=https://+:5001 -e ASPNETCORE_URLS=http://+:5000 --name tinyurlapi --network tinyurl-network tinyurlapi
```

# Use the official ASP.NET runtime image for the .NET Framework
FROM mcr.microsoft.com/dotnet/framework/aspnet:4.8-windowsservercore-ltsc2019 AS base
WORKDIR /inetpub/wwwroot

# Copy the application files to the container
COPY . .

# Set up default application
SHELL ["powershell", "-Command", "$ErrorActionPreference = 'Stop';"]
RUN Remove-WebSite -Name 'Default Web Site' ; `
    New-WebSite -Name 'MyAspNetApp' -Port 80 -PhysicalPath 'C:\inetpub\wwwroot' -ApplicationPool '.NET v4.5'

EXPOSE 80

ENTRYPOINT ["powershell", "-NoLogo", "-ExecutionPolicy", "Bypass", "-Command", "Start-Service W3SVC; Invoke-WebRequest -UseBasicParsing -Uri http://localhost"]

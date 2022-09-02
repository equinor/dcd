FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /source

COPY . .
WORKDIR /source/api
RUN dotnet publish -c release -o /app

FROM mcr.microsoft.com/dotnet/aspnet:6.0
ARG azappconfig
ARG environment
ARG DYNATRACE_PAAS_TOKEN
ARG DYNATRACE_TENANT
ARG DYNATRACE_URL

WORKDIR /app
COPY --from=build /app ./

RUN mkdir -p /var/lib/dynatrace/oneagent/agent/config/ && touch /var/lib/dynatrace/oneagent/agent/config/hostname.conf ; echo "CONCEPTAPP-$environment" > /var/lib/dynatrace/oneagent/agent/config/hostname.conf

EXPOSE 5000
#Dynatrace config
# COPY --from=DYNATRACE_ONEAGENT_IMAGE / /
ENV DT_TAGS=SHELLVIS SERVICENAME=CONCEPTAPP ENVIRONMENT=$environment
ENV DT_CUSTOM_PROP=SERVICENAME=CONCEPTAPP ENVIRONMENT=$environment
ENV DT_PROCESS_GROUP=CONCEPTAPP-$environment
ENV AppConfiguration__ConnectionString=$azappconfig

RUN apt-get update && apt-get install -y curl && rm -rf /var/lib/apt/lists/*
RUN curl -o /tmp/installer.sh -s "https://$DYNATRACE_URL/e/$DYNATRACE_TENANT/api/v1/deployment/installer/agent/unix/paas-sh/latest?Api-Token=$DYNATRACE_PAAS_TOKEN&arch=x86" && sh /tmp/installer.sh /home
RUN apt-get remove curl -y

# Runtime user change to non-root for added security
USER 1001
ENTRYPOINT ["dotnet", "api.dll", "--urls=http://0.0.0.0:5000"]

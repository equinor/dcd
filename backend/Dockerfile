ARG DYNATRACE_PAAS_TOKEN
ARG DYNATRACE_TENANT
ARG DYNATRACE_URL
FROM ${DYNATRACE_URL}/e/${DYNATRACE_TENANT}/linux/oneagent-codemodules:all as dynatrace_repo

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build

WORKDIR /source
COPY . .
WORKDIR /source/api
RUN dotnet publish -c release -o /app


FROM mcr.microsoft.com/dotnet/aspnet:6.0 
ARG environment
ARG azappconfig
WORKDIR /app
COPY --from=build /app ./
EXPOSE 5000

COPY --from=dynatrace_repo / /
RUN mkdir -p /var/lib/dynatrace/oneagent/agent/config/ && touch /var/lib/dynatrace/oneagent/agent/config/hostname.conf ; echo "CONCEPTAPP-${environment}" > /var/lib/dynatrace/oneagent/agent/config/hostname.conf && mkdir /var/log/dcd && chown -R 1001:1001 /var/log/dcd
ENV LD_PRELOAD /opt/dynatrace/oneagent/agent/lib64/liboneagentproc.so

#Dynatrace config
ENV DT_TAGS=SHELLVIS SERVICENAME=CONCEPTAPP ENVIRONMENT=$environment
ENV DT_CUSTOM_PROP=SERVICENAME=CONCEPTAPP ENVIRONMENT=$environment
ENV DT_PROCESS_GROUP=CONCEPTAPP-$environment
ENV AppConfiguration__ConnectionString=$azappconfig

# Runtime user change to non-root for added security
USER 1001
ENTRYPOINT ["dotnet", "api.dll", "--urls=http://0.0.0.0:5000"]

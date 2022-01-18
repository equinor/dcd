ARG dt_tenant
ARG dt_url
ARG azappconfig

FROM ${dt_url}/e/${dt_tenant}/linux/oneagent-codemodules:all as DYNATRACE_ONEAGENT_IMAGE
FROM node:lts-alpine as build

WORKDIR /app
COPY . .
RUN npm ci
RUN mkdir -p node_modules/.cache && chmod -R 777 node_modules/.cache


#Dynatrace config
COPY --from=DYNATRACE_ONEAGENT_IMAGE / /
ENV LD_PRELOAD /opt/dynatrace/oneagent/agent/lib64/liboneagentproc.so
ENV DT_TAGS=SHELLVIS
ENV REACT_APP_AZURE_APP_CONFIG_CONNECTION_STRING=${azappconfig}

# Runtime user change to non-root for added security
RUN addgroup -S -g 1001 non-root-group
RUN adduser -S -u 1001 -G non-root-group non-root-user
USER 1001

EXPOSE 3000
ENTRYPOINT npm start

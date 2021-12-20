FROM node:lts-alpine as build

WORKDIR /app
COPY . .
RUN npm ci
RUN mkdir -p node_modules/.cache && chmod -R 777 node_modules/.cache

# Runtime user change to non-root for added security
RUN addgroup -S -g 1001 non-root-group
RUN adduser -S -u 1001 -G non-root-group non-root-user
USER 1001

EXPOSE 3000
ENTRYPOINT npm start

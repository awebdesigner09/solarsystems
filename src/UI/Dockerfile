    # Stage 1: Build the Angular application
    FROM node:lts-alpine as base
    WORKDIR /app
COPY UI/package.json UI/package-lock.json ./
    RUN npm install
COPY UI/ .
RUN npm run build

    # Stage 2: Serve the application with Nginx
    FROM nginx:alpine

COPY --from=base /app/dist/ /usr/share/nginx/html
    EXPOSE 80
    CMD ["nginx", "-g", "daemon off;"]

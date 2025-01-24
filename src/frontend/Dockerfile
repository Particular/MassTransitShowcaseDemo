# Use an official Node.js runtime as the base image
FROM node:18 AS build-stage

# Set the working directory
WORKDIR /app

# Copy package.json and package-lock.json
COPY package*.json ./

# Install dependencies
RUN npm install

# Copy the rest of the application code
COPY . .

ARG BILLING_SIGNALR
ARG CLIENT_SIGNALR
ARG SALES_SIGNALR 
ARG SHIPPING_SIGNALR

ENV VITE_CLIENT_SIGNALR=$CLIENT_SIGNALR
ENV VITE_BILLING_SIGNALR=$BILLING_SIGNALR
ENV VITE_SALES_SIGNALR=$SALES_SIGNALR
ENV VITE_SHIPPING_SIGNALR=$SHIPPING_SIGNALR

# Build the application
RUN npm run build

# Use an official Nginx image to serve the content
FROM nginx:alpine AS production-stage

# Copy the build output to Nginx's HTML directory
COPY --from=build-stage /app/dist /usr/share/nginx/html

# Expose port 80
EXPOSE 80

# Start Nginx
ENTRYPOINT ["nginx", "-g", "daemon off;"]

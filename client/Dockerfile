FROM node:10.17.0-alpine as node

WORKDIR /app

COPY package*.json ./
RUN npm install

COPY . .

RUN npm run build
